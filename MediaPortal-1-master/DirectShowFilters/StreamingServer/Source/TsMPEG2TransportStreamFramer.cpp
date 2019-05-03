/**********
This library is free software; you can redistribute it and/or modify it under
the terms of the GNU Lesser General Public License as published by the
Free Software Foundation; either version 2.1 of the License, or (at your
option) any later version. (See <http://www.gnu.org/copyleft/lesser.html>.)

This library is distributed in the hope that it will be useful, but WITHOUT
ANY WARRANTY; without even the implied warranty of MERCHANTABILITY or FITNESS
FOR A PARTICULAR PURPOSE.  See the GNU Lesser General Public License for
more details.

You should have received a copy of the GNU Lesser General Public License
along with this library; if not, write to the Free Software Foundation, Inc.,
51 Franklin Street, Fifth Floor, Boston, MA 02110-1301  USA
**********/
// "liveMedia"
// Copyright (c) 1996-2009 Live Networks, Inc.  All rights reserved.
// A filter that passes through (unchanged) chunks that contain an integral number
// of MPEG-2 Transport Stream packets, but returning (in "fDurationInMicroseconds")
// an updated estimate of the time gap between chunks.
// Implementation

#include "TsMPEG2TransportStreamFramer.h"
#include "StreamingDefs.h"

#ifndef _GROUPSOCK_HELPER_HH
#include <GroupsockHelper.hh>   // gettimeofday()
#endif

#define NEW_DURATION_WEIGHT 0.5
// How much weight to give to the latest duration measurement (must be <= 1)
#define TIME_ADJUSTMENT_FACTOR 0.8
// A factor by which to adjust the duration estimate to ensure that the overall
// packet transmission times remains matched with the PCR times (which will be the
// times that we expect receivers to play the incoming packets).
// (must be <= 1)
#define MAX_PLAYOUT_BUFFER_DURATION 0.1 // (seconds)
#define PCR_PERIOD_VARIATION_RATIO 0.5

extern void LogDebug(const char *fmt, ...) ;

////////// PIDStatus //////////

class TsPIDStatus {
public:
	TsPIDStatus(double _firstClock, double _firstRealTime)
		: firstClock(_firstClock), lastClock(_firstClock),
		firstRealTime(_firstRealTime), lastRealTime(_firstRealTime),
		lastPacketNum(0) {
	}

	double firstClock, lastClock, firstRealTime, lastRealTime;
	unsigned lastPacketNum;
};


////////// MPEG2TransportStreamFramer //////////

TsMPEG2TransportStreamFramer* TsMPEG2TransportStreamFramer
::createNew(UsageEnvironment& env, FramedSource* inputSource) {
	return new TsMPEG2TransportStreamFramer(env, inputSource);
}

TsMPEG2TransportStreamFramer
::TsMPEG2TransportStreamFramer(UsageEnvironment& env, FramedSource* inputSource)
: FramedFilter(env, inputSource),
fTSPacketCount(0), fTSPacketDurationEstimate(0.0), fTSPCRCount(0), fTSPacketNullTime(0) {
	fPIDStatusTable = HashTable::create(ONE_WORD_HASH_KEYS);
}

TsMPEG2TransportStreamFramer::~TsMPEG2TransportStreamFramer() {
	clearPIDStatusTable();
	delete fPIDStatusTable;
}

void TsMPEG2TransportStreamFramer::clearPIDStatusTable() {
	TsPIDStatus* pidStatus;
	while ((pidStatus = (TsPIDStatus*)fPIDStatusTable->RemoveNext()) != NULL) {
		delete pidStatus;
	}
}

void TsMPEG2TransportStreamFramer::doGetNextFrame() {
	// Read directly from our input source into our client's buffer:
	fFrameSize = 0;
	fInputSource->getNextFrame(fTo, fMaxSize,
		afterGettingFrame, this,
		FramedSource::handleClosure, this);
}

void TsMPEG2TransportStreamFramer::doStopGettingFrames() {
	FramedFilter::doStopGettingFrames();
	fTSPacketCount = 0;
	fTSPCRCount = 0;
	fTSPacketNullTime = 0;

	clearPIDStatusTable();
}

void TsMPEG2TransportStreamFramer
::afterGettingFrame(void* clientData, unsigned frameSize,
					unsigned /*numTruncatedBytes*/,
struct timeval presentationTime,
	unsigned /*durationInMicroseconds*/) {
		TsMPEG2TransportStreamFramer* framer = (TsMPEG2TransportStreamFramer*)clientData;
		framer->afterGettingFrame1(frameSize, presentationTime);
}

void TsMPEG2TransportStreamFramer::afterGettingFrame1(unsigned frameSize,
struct timeval presentationTime) {
	fFrameSize += frameSize;
	unsigned const numTSPackets = fFrameSize/TRANSPORT_PACKET_SIZE;
  unsigned const dataGoingToBeLost=fFrameSize % TRANSPORT_PACKET_SIZE;
	fFrameSize = numTSPackets*TRANSPORT_PACKET_SIZE; // an integral # of TS packets
	if (fFrameSize == 0) {
		// We didn't read a complete TS packet; assume that the input source has closed.
		handleClosure(this);
		return;
	}
  if (dataGoingToBeLost>0)
  {
    //need to handle a mid buffer
  }
	// Make sure the data begins with a sync byte:
	unsigned syncBytePosition;
	for (syncBytePosition = 0; syncBytePosition < fFrameSize; ++syncBytePosition) {
		if (fTo[syncBytePosition] == TRANSPORT_SYNC_BYTE) break;
	}
	if (syncBytePosition == fFrameSize) {
		envir() << "No Transport Stream sync byte in data.";
		handleClosure(this);
		return;
	} else if (syncBytePosition > 0) {
		// There's a sync byte, but not at the start of the data.  Move the good data
		// to the start of the buffer, then read more to fill it up again:
		memmove(fTo, &fTo[syncBytePosition], frameSize - syncBytePosition);
		fFrameSize -= syncBytePosition-dataGoingToBeLost;
		fInputSource->getNextFrame(&fTo[fFrameSize], syncBytePosition,
			afterGettingFrame, this,
			FramedSource::handleClosure, this);
		return;
	}
  else if (dataGoingToBeLost>0)// there is a problem in the buffer somewhere
  {
    unsigned badPacket = 0;
    for (badPacket=0;badPacket<numTSPackets;badPacket++)
    {
      if (fTo[badPacket*TRANSPORT_PACKET_SIZE]!=TRANSPORT_SYNC_BYTE && badPacket*TRANSPORT_PACKET_SIZE<frameSize) break;
    }
    //we know it's the previous one...
    if (badPacket!=0)
    {
	    for (syncBytePosition = 1; syncBytePosition < TRANSPORT_PACKET_SIZE; ++syncBytePosition) {
		    if (fTo[badPacket*TRANSPORT_PACKET_SIZE-syncBytePosition] == TRANSPORT_SYNC_BYTE) break;
	    }
   		memmove(&fTo[(badPacket-1)*TRANSPORT_PACKET_SIZE], &fTo[badPacket*TRANSPORT_PACKET_SIZE-syncBytePosition], frameSize - (badPacket*TRANSPORT_PACKET_SIZE-syncBytePosition));
		  fFrameSize -= TRANSPORT_PACKET_SIZE-syncBytePosition-dataGoingToBeLost;
		  fInputSource->getNextFrame(&fTo[fFrameSize], syncBytePosition,
			  afterGettingFrame, this,
			  FramedSource::handleClosure, this);
		  return;
    }
  }// else normal case: the data begins with a sync byte


	fPresentationTime = presentationTime;

	// Scan through the TS packets that we read, and update our estimate of
	// the duration of each packet:
	struct timeval tvNow;
	gettimeofday(&tvNow, NULL);
	double timeNow = tvNow.tv_sec + tvNow.tv_usec/1000000.0;
	for (unsigned i = 0; i < numTSPackets; ++i) {
		updateTSPacketDurationEstimate(&fTo[i*TRANSPORT_PACKET_SIZE], timeNow);
	}

	fDurationInMicroseconds
		= numTSPackets * (unsigned)(fTSPacketDurationEstimate*1000000);

	// Complete the delivery to our client:
	afterGetting(this);
}

void TsMPEG2TransportStreamFramer
::updateTSPacketDurationEstimate(unsigned char* pkt, double timeNow) {
	// Sanity check: Make sure we start with the sync byte:
	if (pkt[0] != TRANSPORT_SYNC_BYTE) {
		envir() << "Missing sync byte!\n";
		return;
	}

  // Get the pid
	unsigned pid = ((pkt[1]&0x1F)<<8) | pkt[2];
	
	if ((pid & 0x1FFF) == 0x1FFF) //NULL TS packet PID
	{
	  if (pkt[3] == NULL_TS_CONTINUITY_BYTE) //CTSBuffer::GetNullTsBuffer() has inserted it
	  {
	    fTSPacketNullTime = *(DWORD *)(pkt+4); //Get the NULL duration timestamp (milliseconds)
	    pkt[3] &= 0xF0; // force the continuity value to zero
	  }
	  return;
	}

	++fTSPacketCount; //Only count the real stream packets, not the extra NULL padding packets CTSBuffer might insert
	
	// If this packet doesn't contain a PCR, then we're not interested in it:
	u_int8_t const adaptation_field_control = (pkt[3]&0x30)>>4;
	if (adaptation_field_control != 2 && adaptation_field_control != 3) return;
	// there's no adaptation_field

	u_int8_t const adaptation_field_length = pkt[4];
	if (adaptation_field_length < 7) return;

	u_int8_t discontinuity_indicator = pkt[5]&0x80;
	u_int8_t const pcrFlag = pkt[5]&0x10;
	if (pcrFlag == 0) return; // no PCR

	// There's a PCR.  Get it, and the PID:
	++fTSPCRCount;
	u_int32_t pcrBaseHigh = (pkt[6]<<24)|(pkt[7]<<16)|(pkt[8]<<8)|pkt[9];
	double clock = pcrBaseHigh/45000.0;
	if ((pkt[10]&0x80) != 0) clock += 1/90000.0; // add in low-bit (if set)
	unsigned short pcrExt = ((pkt[10]&0x01)<<8) | pkt[11];
	clock += pcrExt/27000000.0;


	// Check whether we already have a record of a PCR for this PID:
	TsPIDStatus* pidStatus = (TsPIDStatus*)(fPIDStatusTable->Lookup((char*)pid));

	if (pidStatus == NULL) {
		// We're seeing this PID's PCR for the first time:
		pidStatus = new TsPIDStatus(clock, timeNow);
		fPIDStatusTable->Add((char*)pid, pidStatus);
#ifdef DEBUG_PCR
		fprintf(stderr, "PID 0x%x, FIRST PCR 0x%08x+%d:%03x == %f @ %f, pkt #%lu\n", pid, pcrBaseHigh, pkt[10]>>7, pcrExt, clock, timeNow, fTSPacketCount);
#endif
	} else {
		// We've seen this PID's PCR before; update our per-packet duration estimate:
		double clockDiff = clock - pidStatus->lastClock;
		double durationPerPacket = clockDiff/(fTSPacketCount - pidStatus->lastPacketNum);
		
		//Contiguous NULL TS packet duration
		double durNullPackets = ((double)fTSPacketNullTime)/1000.0;
		fTSPacketNullTime = 0;
		
		pidStatus->firstRealTime += durNullPackets; //Adjust to compensate for NULL packet insertion
			
		 //Detect PCR rollover or large forward jumps in PCR (maximum normal clockDiff is approx. +100ms) or too many NULL packets in a block
	  if ((clockDiff < 0.0) || (clockDiff > 0.25) || (durNullPackets > 0.25) || (discontinuity_indicator > 0))
    {
      discontinuity_indicator |= 0x01; // force a reset of the stored clock and real-time values
	    LogDebug("TsMp2TSFramer - PCR jump: %f s, NULL dur: : %f s, packet count %d", (float)clockDiff, (float)durNullPackets, fTSPacketCount);  
    } else {
  		// Hack (suggested by "Romain"): Don't update our estimate if this PCR appeared unusually quickly.
  		// (This can produce more accurate estimates for wildly VBR streams.)
  		double meanPCRPeriod = 0.0;
  		if (fTSPCRCount > 0) {
  			meanPCRPeriod=(double)fTSPacketCount/fTSPCRCount;
  			if (fTSPacketCount - pidStatus->lastPacketNum < meanPCRPeriod*PCR_PERIOD_VARIATION_RATIO) return;
  		}
  	}

		if (fTSPacketDurationEstimate == 0.0) { // we've just started
			fTSPacketDurationEstimate = durationPerPacket;
			if (durationPerPacket > 0.0)
			{
	      LogDebug("TsMp2TSFramer - Average bitrate at start: %f kbit/s calculated over %d TS packets", (float)((TRANSPORT_PACKET_SIZE*8)/(durationPerPacket*1000.0)), (fTSPacketCount - pidStatus->lastPacketNum));  
	    }
		} else if (discontinuity_indicator == 0 && durationPerPacket >= 0.0) {
			fTSPacketDurationEstimate
				= durationPerPacket*NEW_DURATION_WEIGHT
				+ fTSPacketDurationEstimate*(1-NEW_DURATION_WEIGHT);

			// Also adjust the duration estimate to try to ensure that the transmission
			// rate matches the playout rate:
			double transmitDuration = timeNow - pidStatus->firstRealTime;
			double playoutDuration = clock - pidStatus->firstClock;
			
			if (transmitDuration > 0.0 && playoutDuration > 0.0)
			{
  			if (transmitDuration > playoutDuration) {
  				fTSPacketDurationEstimate *= TIME_ADJUSTMENT_FACTOR; // reduce estimate
  			} else if (transmitDuration + MAX_PLAYOUT_BUFFER_DURATION < playoutDuration) {
  				fTSPacketDurationEstimate /= TIME_ADJUSTMENT_FACTOR; // increase estimate
  			}
  	  }
		} else {
			// the PCR has a discontinuity from its previous value; don't use it now,
			// but reset our PCR and real-time values to compensate:
			pidStatus->firstClock = clock;
			pidStatus->firstRealTime = timeNow;
		}
#ifdef DEBUG_PCR
		fprintf(stderr, "PID 0x%x, PCR 0x%08x+%d:%03x == %f @ %f (diffs %f @ %f), pkt #%lu, discon %d => this duration %f, new estimate %f, mean PCR period=%f\n", pid, pcrBaseHigh, pkt[10]>>7, pcrExt, clock, timeNow, clock - pidStatus->firstClock, timeNow - pidStatus->firstRealTime, fTSPacketCount, discontinuity_indicator != 0, durationPerPacket, fTSPacketDurationEstimate, meanPCRPeriod );
#endif
	}

	pidStatus->lastClock = clock;
	pidStatus->lastRealTime = timeNow;
	pidStatus->lastPacketNum = fTSPacketCount;
}
