/* 
*	Copyright (C) 2006-2008 Team MediaPortal
*	http://www.team-mediaportal.com
*
*  This Program is free software; you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation; either version 2, or (at your option)
*  any later version.
*   
*  This Program is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
*  GNU General Public License for more details.
*   
*  You should have received a copy of the GNU General Public License
*  along with GNU Make; see the file COPYING.  If not, write to
*  the Free Software Foundation, 675 Mass Ave, Cambridge, MA 02139, USA. 
*  http://www.gnu.org/copyleft/gpl.html
*
*/

#pragma warning(disable : 4995)
#include <windows.h>
#include <commdlg.h>
#include <bdatypes.h>
#include <time.h>
#include <streams.h>
#include <initguid.h>
#include <cassert>

#include "DiskRecorder.h"
#include "pmtparser.h"
#include "..\..\shared\dvbutil.h"
#include "..\..\shared\section.h"

#define TS_PACKET_SIZE	188

#define THROTTLE_MAXIMUM_RADIO  ((NUMBER_THROTTLE_BUFFER_SIZES/2) - 1)		//	Throttle to 11 for radio
#define THROTTLE_MAXIMUM_TV	(NUMBER_THROTTLE_BUFFER_SIZES - 1)		//	Throttle to 19 for tv for reduced disk IOs
#define REC_THTL_MULT 8 // Multiplier to increase buffer sizes for recordings c.f. timeshift

#define IGNORE_AFTER_TUNE 25                        // how many TS packets to ignore after tuning

#define PID_PAT                               0     // PID for PAT table
#define TABLE_ID_PAT                          0     // TABLE ID for PAT
#define TABLE_ID_SDT                        0x42    // TABLE ID for SDT

#define ADAPTION_FIELD_LENGTH_OFFSET        0x4     // byte offset from the start of a TS packet to the adaption field length byte
#define PCR_OFFSET                          0x6     // byte offset from the start of a TS packet to the start of the PCR
#define PCR_LENGTH                          0x6     // the length of a PCR timestamp in bytes
#define ADAPTION_FIELD_FLAG_OFFSET          0x5     // byte offset from the start of a TS packet to the adaption field flags byte
#define DISCONTINUITY_FLAG_BIT              0x80    // bitmask for the DISCONTINUITY flag
#define RANDOM_ACCESS_FLAG_BIT              0x40    // bitmask for the RANDOM_ACCESS_FLAG flag
#define ES_PRIORITY_FLAG_BIT                0x20    // bitmask for the ES_PRIORITY_FLAG flag
#define PCR_FLAG_BIT                        0x10    // bitmask for the PCR flag
#define OPCR_FLAG_BIT                       0x8     // bitmask for the OPCR flag
#define SPLICING_FLAG_BIT                   0x4     // bitmask for the SPLICING flag
#define TRANSPORT_PRIVATE_DATA_FLAG_BIT     0x2     // bitmask for the TRANSPORT_PRIVATE_DATA flag
#define ADAPTION_FIELD_EXTENSION_FLAG_BIT   0x1     // bitmask for the DAPTION_FIELD_EXTENSION flag

//#define ERROR_FILE_TOO_LARGE 223  - already defined in winerror.h

int DR_FAKE_NETWORK_ID   = 0x456;                // network id we use in our PAT
int DR_FAKE_TRANSPORT_ID = 0x4;                  // transport id we use in our PAT
int DR_FAKE_SERVICE_ID   = 0x89;                 // service id we use in our PAT
int DR_FAKE_PMT_PID      = 0x20;                 // pid we use for our PMT
int DR_FAKE_PCR_PID      = 0x30;//0x21;          // pid we use for our PCR
int DR_FAKE_VIDEO_PID    = 0x30;                 // pid we use for the video stream
int DR_FAKE_AUDIO_PID    = 0x40;                 // pid we use for the audio strean
int DR_FAKE_SUBTITLE_PID = 0x50;                 // pid we use for subtitles
int DR_FAKE_TELETEXT_PID = 0x60; // Ziphnor

double DR_MAX_ALLOWED_PCR_DIFF = 10.0; // (clock value) if pcr/pts/dts difference between last and current value is higher than
								    //			   this value a hole/jump is assumed

extern void LogDebug(const char *fmt, ...) ;
extern void LogDebug(const wchar_t *fmt, ...) ;

//*******************************************************************
//* ctor
//*******************************************************************
CDiskRecorder::CDiskRecorder(RecordingMode mode) 
{
	m_recordingMode=mode;
	m_bPaused=FALSE;
	m_params.chunkSize=268435424;
	m_params.maxFiles=20;
	m_params.maxSize=268435424;
	m_params.minFiles=6;

	m_iPmtPid=-1;
	m_iOriginalPcrPid=-1;
	m_iFakePcrPid=DR_FAKE_PCR_PID;
	m_iServiceId=-1;

	m_bRunning=false;
	m_pTimeShiftFile=NULL;
	m_pRecordingFile=NULL;

  m_iTsContinuityCounter=0;

	m_bStartPcrFound=false;
	m_bDetermineNewStartPcr=false;
	m_iPatVersion=0;
	m_iPmtVersion=0;
	m_eChannelType = TV;


	m_TsPacketCount=0;
	m_bClearTsQueue=false;
	m_pPmtParser=new CPmtParser();
	m_pVideoAudioObserver=NULL;
	m_mapLastPtsDts.clear();

	//	Populate the throttle
	m_iThrottleBufferSizes[0] = 2;		//	2 total
	m_iThrottleBufferSizes[1] = 3;		//	5
	m_iThrottleBufferSizes[2] = 5;		//	10
	m_iThrottleBufferSizes[3] = 5;		//	15
	m_iThrottleBufferSizes[4] = 5;		//	20
	m_iThrottleBufferSizes[5] = 5;		//	25
	m_iThrottleBufferSizes[6] = 5;		//	30
	m_iThrottleBufferSizes[7] = 10;		//	40
	m_iThrottleBufferSizes[8] = 10;		//	50
	m_iThrottleBufferSizes[9] = 10;		//	60 (Max for radio)
	m_iThrottleBufferSizes[10] = 10;	//	70
	m_iThrottleBufferSizes[11] = 10;	//	80
	m_iThrottleBufferSizes[12] = 20;	//	100
	m_iThrottleBufferSizes[13] = 20;	//	120
	m_iThrottleBufferSizes[14] = 20;	//	140
	m_iThrottleBufferSizes[15] = 32;	//	172	*sync with streamingserver
	m_iThrottleBufferSizes[16] = 40;	//	212
	m_iThrottleBufferSizes[17] = 50;	//	262
	m_iThrottleBufferSizes[18] = 82;	//	344	*sync with streamingserver
	m_iThrottleBufferSizes[19] = 174;	//	518	*sync with streamingserver (max for TV)

  ResetThrottle(false); //initialise m_iWriteBufferSize etc.
  
	//LogDebug("CDiskRecorder::Ctor - end");
}
//*******************************************************************
//* dtor
//*******************************************************************
CDiskRecorder::~CDiskRecorder(void)
{
	CEnterCriticalSection enter(m_section);

	if (m_pTimeShiftFile!=NULL)
	{
		delete m_pTimeShiftFile;
		m_pTimeShiftFile=NULL;
	}
	if (m_pRecordingFile!=NULL)
	{
		delete m_pRecordingFile;
		m_pRecordingFile=NULL;
	}

	m_pPmtParser->Reset();
	delete m_pPmtParser;
	
	//LogDebug("CDiskRecorder::Dtor - end");
}

void CDiskRecorder::SetChannelType(int channelType)
{
	CEnterCriticalSection enter(m_section);

	LogDebug("CDiskRecorder::SetChannelType() - Channel type is %d", channelType);

	try
	{
		//	If tv (0)
		if(channelType == 0)
			m_eChannelType = TV;		
		else  //Else assume radio
			m_eChannelType = Radio;
			
	  m_bThrottleAtMax = FALSE; //Allow throttle adjustment
	}
	catch(...)
	{
		WriteLog("SetChannelType exception");
	}
}

void CDiskRecorder::SetFileNameW(wchar_t* pwszFileName)
{
	CEnterCriticalSection enter(m_section);
	try
	{
		m_iPacketCounter=0;
		m_iPmtPid=-1;
		m_iOriginalPcrPid=-1;
		m_iFakePcrPid=DR_FAKE_PCR_PID;
		m_vecPids.clear();
		m_AudioOrVideoSeen=false ;
		m_bStartPcrFound=false;
		m_bDetermineNewStartPcr=false;
		wcscpy(m_wszFileName,pwszFileName);
		if (m_recordingMode==RecordingMode::TimeShift)
			wcscat(m_wszFileName, L".tsbuffer");
		WriteLog(L"set filename:%s", m_wszFileName);
	}
	catch(...)
	{
		WriteLog(L"SetFilename exception");
	}
}

bool CDiskRecorder::Start()
{
	CEnterCriticalSection enter(m_section);
	try
	{
		if (wcslen(m_wszFileName)==0) return false;
		  
		if (m_recordingMode==RecordingMode::TimeShift)
		{
			m_pTimeShiftFile = new MultiFileWriter(&m_params);
			if (FAILED(m_pTimeShiftFile->Open(m_wszFileName))) 
			{
				WriteLog(L"failed to open timeshift file:%s %d",m_wszFileName,GetLastError());
				delete m_pTimeShiftFile;
				m_pTimeShiftFile=NULL;
				return false;
			}
		}
		else
		{
			m_pRecordingFile = new FileWriterThreaded();
			if (FAILED(m_pRecordingFile->Open(m_wszFileName))) 
			{
				WriteLog(L"failed to open recording file:%s %d",m_wszFileName,GetLastError());
				delete m_pRecordingFile;
				m_pRecordingFile=NULL;
				return false;
			}		  
		}
		m_iPmtContinuityCounter=-1;
		m_iPatContinuityCounter=-1;
    m_iTsContinuityCounter=0;
		m_bDetermineNewStartPcr=false;
		m_bStartPcrFound=false;
		m_mapLastPtsDts.clear();
		m_iPacketCounter=0;
		WriteLog(L"Start '%s'",m_wszFileName);
		m_bRunning=true;
		m_bPaused=FALSE;
	}
	catch(...)
	{
		WriteLog("Start exception");
	}
	return true;
}

void CDiskRecorder::Stop()
{
	CEnterCriticalSection enter(m_section);
	try
	{
		WriteLog(L"Stop '%s'",m_wszFileName);
		m_bRunning=false;
		m_pPmtParser->Reset();
		if (m_pTimeShiftFile!=NULL)
		{
		  m_pTimeShiftFile->Close();
			delete m_pTimeShiftFile;
			m_pTimeShiftFile=NULL;
		}
		if (m_pRecordingFile!=NULL)
		{
		  m_pRecordingFile->Close();
			delete m_pRecordingFile;
			m_pRecordingFile=NULL;
		}
		
		m_iPmtPid=-1;
		Reset();
	}
	catch(...)
	{
		WriteLog("Stop  exception");
	}
}

void CDiskRecorder::Pause(BYTE onOff) 
{
	CEnterCriticalSection enter(m_section);
  if (onOff!=0)
  {
		m_bClearTsQueue=true;
    m_bPaused=TRUE;
  }
	else
  {
		m_bPaused=FALSE;
  }
	if (m_bPaused)
  {
		WriteLog("paused=yes"); 
		
  }
	else
	{
		WriteLog("paused=no"); 
		//Flush();
		if (m_vecPids.size()==0)
			WriteLog("PANIC changed status to running but i have not a single stream to record !!!!");
	}
}

void CDiskRecorder::Reset()
{
	CEnterCriticalSection enter(m_section);
	try
	{
		WriteLog("Reset");
		m_iPmtPid=-1;
		m_iOriginalPcrPid=-1;
		m_bDetermineNewStartPcr=false;
		m_bStartPcrFound=false;
		m_vecPids.clear();
		m_AudioOrVideoSeen=false ;
		m_pPmtParser->Reset();
		DR_FAKE_NETWORK_ID   = 0x456;
		DR_FAKE_TRANSPORT_ID = 0x4;
		DR_FAKE_SERVICE_ID   = 0x89;
		DR_FAKE_PMT_PID      = 0x20;
		DR_FAKE_PCR_PID      = 0x30;//0x21;
		DR_FAKE_VIDEO_PID    = 0x30;
		DR_FAKE_AUDIO_PID    = 0x40;
		DR_FAKE_SUBTITLE_PID = 0x50;
		m_iFakePcrPid=DR_FAKE_PCR_PID;
		m_iPacketCounter=0;
		m_bPaused=FALSE;
		m_mapLastPtsDts.clear();
    m_iTsContinuityCounter=0;

		//	Reset the write buffer throttle
		ResetThrottle(false);
	  //LogDebug("CDiskRecorder::Reset() - end");
	}
	catch(...)
	{
		WriteLog("Reset exception");
	}
}

void CDiskRecorder::GetRecordingMode(int *mode) 
{
	*mode=(int)m_recordingMode;
}

void CDiskRecorder::SetPmtPid(int pmtPid,int serviceId,byte* pmtData,int pmtLength)
{
	CEnterCriticalSection enter(m_section);
	WriteLog("Received from TvService: pmt pid:0x%x serviceId: 0x%x pmtlength:%d",pmtPid,serviceId,pmtLength);
  m_iPmtPid=pmtPid;
	m_iServiceId=serviceId;
	m_pPmtParser->Reset();
	m_pPmtParser->SetFilter(pmtPid,serviceId);
	CSection section;
	section.Reset();
	section.BufferPos=pmtLength;
	memcpy(section.Data,pmtData,pmtLength);
	section.DecodeHeader();
	m_vecPids.clear();
	m_AudioOrVideoSeen=false ;
	WriteLog("Old pids cleared");
	WriteLog("got pmt - tableid: 0x%x section_length: %d sid: 0x%x",section.table_id,section.section_length,section.table_id_extension);
	int pcrPid;
	bool hasCaDescriptor = false;
	vector<PidInfo2> pidInfos;
	if (!m_pPmtParser->DecodePmt(section,pcrPid,hasCaDescriptor,pidInfos))
	{
		WriteLog("!!! PANIC - DecodePmt(...) returned FALSE !!!");
		return;
	}
	WriteLog("PMT parsed  - Pid 0x%x ServiceId 0x%x stream count: %d",pmtPid, m_iServiceId,pidInfos.size());
	SetPcrPid(pcrPid);
	ivecPidInfo2 it=pidInfos.begin();
	while (it!=pidInfos.end())
	{
		PidInfo2 info=*it;
		AddStream(info);
		++it;
	}
}

void CDiskRecorder::SetVideoAudioObserver (IVideoAudioObserver* callback)
{
  if(callback)
  {
    WriteLog("SetVideoAudioObserver observer ok");
    m_pVideoAudioObserver = callback;
  }
  else
  {
    WriteLog("SetVideoAudioObserver observer was null");  
		return;
  }
}

void CDiskRecorder::GetBufferSize(long *size)
{
	*size = 0;
}

void CDiskRecorder::GetMinTSFiles(WORD *minFiles)
{
	*minFiles = (WORD) m_params.minFiles;
}

void CDiskRecorder::SetMinTSFiles(WORD minFiles)
{
	m_params.minFiles=(long)minFiles;
}

void CDiskRecorder::GetMaxTSFiles(WORD *maxFiles)
{
	*maxFiles = (WORD) m_params.maxFiles;
}

void CDiskRecorder::SetMaxTSFiles(WORD maxFiles)
{
	m_params.maxFiles=(long)maxFiles;
}

void CDiskRecorder::GetMaxTSFileSize(__int64 *maxSize)
{
	*maxSize = m_params.maxSize;
}

void CDiskRecorder::SetMaxTSFileSize(__int64 maxSize)
{
	m_params.maxSize=maxSize;
}

void CDiskRecorder::GetChunkReserve(__int64 *chunkSize)
{
	*chunkSize = m_params.chunkSize;
}

void CDiskRecorder::SetChunkReserve(__int64 chunkSize)
{
	m_params.chunkSize=chunkSize;
}

void CDiskRecorder::GetTimeShiftPosition(__int64 * position,long * bufferId)
{
	m_pTimeShiftFile->GetPosition(position, bufferId);	
}

void CDiskRecorder::GetDiscontinuityCounter(int* counter)
{
  (*counter) = m_iTsContinuityCounter;
}

void CDiskRecorder::GetTotalBytes(int* packetsProcessed)
{
  (*packetsProcessed) = m_TsPacketCount;
}

void CDiskRecorder::OnTsPacket(byte* tsPacket)
{
	if (m_bPaused) return;
	if (m_bRunning)
	{
		CEnterCriticalSection enter(m_section);		
		CTsHeader header(tsPacket);
		if (header.Pid==0x1fff) return;
		if (header.SyncByte!=0x47) return;
		if (header.TransportError) return;
  	WriteTs(tsPacket);
	}
}

void CDiskRecorder::Write(byte* buffer,int len)
{
	CEnterCriticalSection enter(m_section);
	if (m_recordingMode==RecordingMode::TimeShift)
		WriteToTimeshiftFile(buffer,len);
	else
		WriteToRecording(buffer,len);
}

void CDiskRecorder::WriteToRecording(byte* buffer, int len)
{
	CEnterCriticalSection enter(m_section);
	if (!m_bRunning) return;
  if (m_bPaused || m_bClearTsQueue) 
  {
    try
    {
      WriteLog("clear TS packet queue"); 
      m_bClearTsQueue = false;
      m_pRecordingFile->DiscardBuffer();
      ResetThrottle(true);
    }
    catch(...)
    {
      WriteLog("Write exception - 1");
    }
    if (m_bPaused) return;	 // If m_bPaused == false, this packet should be stored.
  }
  
  if (buffer==NULL) return;
  if (len != 188) return; //sanity check 
	try
	{
	  //This will create a new buffer if needed
    if (m_pRecordingFile->AddToBuffer(buffer, len, m_iWriteBufferSize) == S_FALSE)
    {	
      //We have just rolled over into another buffer, so check if throttle needs adjustment
      AdjustThrottle();
    }
  } 
  catch (...) 
  { 
    WriteLog("Exception in WriteToRecording");
  }
}

void CDiskRecorder::WriteToTimeshiftFile(byte* buffer, int len)
{
  CEnterCriticalSection enter(m_section);

  if (!m_bRunning) return;
  if (m_bPaused || m_bClearTsQueue) 
  {
     try
     {
        WriteLog("clear TS packet queue"); 
        m_bClearTsQueue = false;
        m_pTimeShiftFile->DiscardBuffer();
    		ResetThrottle(true);
     }
     catch(...)
     {
        WriteLog("Write exception - 1");
     }
     if (m_bPaused) return;	 // If m_bPaused == false, this packet should be stored.
  }
    
  if (buffer == NULL) return;
  if (len != 188) return; //sanity check
  
  try  
  {   
     //This will create a new buffer if needed   
    if (m_pTimeShiftFile->AddToBuffer(buffer, len, m_iWriteBufferSize) == S_FALSE)
    {	
      //We have just rolled over into another buffer, so check if throttle needs adjustment
      AdjustThrottle();
    }
  }
  catch(...)
  {
    WriteLog("Write exception - 3");
    return;
  }
}

void CDiskRecorder::ResetThrottle(bool logging)
{
  m_bThrottleAtMax = FALSE;
	m_iWriteBufferThrottle = 0;

  if (m_recordingMode == TimeShift)
  {
    m_iWriteBufferSize = m_iThrottleBufferSizes[m_iWriteBufferThrottle] * TS_PACKET_SIZE;
	}
	else
	{
    m_iWriteBufferSize = m_iThrottleBufferSizes[m_iWriteBufferThrottle] * TS_PACKET_SIZE * REC_THTL_MULT;
	}
  
  if (logging)
  {
    LogDebug("CDiskRecorder::ResetThrottle() - Throttle to %d bytes", m_iWriteBufferSize);
  }
}

void CDiskRecorder::AdjustThrottle()
{
  if (m_bThrottleAtMax) return;
       
  int throttleToNumber = THROTTLE_MAXIMUM_TV;  
  //	If radio, we want to throttle to a smaller buffer size
  if(m_eChannelType == Radio)
  	throttleToNumber = THROTTLE_MAXIMUM_RADIO;

  //Check throttle limits  	
  m_iWriteBufferThrottle = min(max(m_iWriteBufferThrottle, 0), throttleToNumber); 
  
  //	Throttle up if we are not at maximum		
  if(m_iWriteBufferThrottle < throttleToNumber)
  {	  
  	m_iWriteBufferThrottle++;
  }
  
  if(m_iWriteBufferThrottle >= throttleToNumber)
  {
  	m_bThrottleAtMax = TRUE;
  }
  
  if (m_recordingMode == TimeShift)
  {
    m_iWriteBufferSize = m_iThrottleBufferSizes[m_iWriteBufferThrottle] * TS_PACKET_SIZE;
	}
	else
	{
    m_iWriteBufferSize = m_iThrottleBufferSizes[m_iWriteBufferThrottle] * TS_PACKET_SIZE * REC_THTL_MULT;
	}
  
  LogDebug("CDiskRecorder::AdjustThrottle() - Throttle to %d bytes", m_iWriteBufferSize);
}

void CDiskRecorder::WriteLog(const char* fmt,...)
{
	char logbuffer[2000]; 
	va_list ap;
	va_start(ap,fmt);

	int tmp;
	va_start(ap,fmt);
	tmp=vsprintf(logbuffer, fmt, ap);
	va_end(ap); 

	if (m_recordingMode==RecordingMode::TimeShift)
		LogDebug("Recorder: TIMESHIFT %s",logbuffer);
	else
		LogDebug("Recorder: RECORD    %s",logbuffer);
}

void CDiskRecorder::WriteLog(const wchar_t* fmt,...)
{
  wchar_t logbuffer[2000]; 
  va_list ap;
  va_start(ap,fmt);

  int tmp;
  va_start(ap,fmt);
  tmp=vswprintf_s(logbuffer, fmt, ap);
  va_end(ap); 

  if (m_recordingMode==RecordingMode::TimeShift)
    LogDebug(L"Recorder: TIMESHIFT %s",logbuffer);
  else
    LogDebug(L"Recorder: RECORD    %s",logbuffer);
}

void CDiskRecorder::SetPcrPid(int pcrPid)
{
	CEnterCriticalSection enter(m_section);
	WriteLog("pcr pid:0x%x",pcrPid); 
    WriteLog("SetPcrPid clear old PIDs");
    m_vecPids.clear();
	m_AudioOrVideoSeen=false ;
    m_bClearTsQueue=true;
    m_TsPacketCount=0;

	if (m_bRunning)
	{
		WriteLog("determine new start pcr"); 
		m_bDetermineNewStartPcr=true;
		m_mapLastPtsDts.clear();
	}
	m_iOriginalPcrPid=pcrPid;
	m_vecPids.clear();
	m_AudioOrVideoSeen=false ;
	DR_FAKE_NETWORK_ID   = 0x456;
	DR_FAKE_TRANSPORT_ID = 0x4;
	DR_FAKE_SERVICE_ID   = 0x89;
	DR_FAKE_PMT_PID      = 0x20;
	DR_FAKE_PCR_PID      = 0x30;//0x21;
	DR_FAKE_VIDEO_PID    = 0x30;
	DR_FAKE_AUDIO_PID    = 0x40;
	DR_FAKE_SUBTITLE_PID = 0x50;
	m_iFakePcrPid=DR_FAKE_PCR_PID;
	m_iPatVersion++;
	if (m_iPatVersion>15) 
		m_iPatVersion=0;
	m_iPmtVersion++;
	if (m_iPmtVersion>15) 
		m_iPmtVersion=0;
}

bool CDiskRecorder::IsStreamWanted(int stream_type)
{
	return (stream_type==SERVICE_TYPE_VIDEO_MPEG1 || 
					stream_type==SERVICE_TYPE_VIDEO_MPEG2 || 
					stream_type==SERVICE_TYPE_VIDEO_MPEG2_DCII ||
					stream_type==SERVICE_TYPE_VIDEO_MPEG4 || 
					stream_type==SERVICE_TYPE_VIDEO_H264 ||
					stream_type==SERVICE_TYPE_VIDEO_HEVC ||
					stream_type==SERVICE_TYPE_AUDIO_MPEG1 || 
					stream_type==SERVICE_TYPE_AUDIO_MPEG2 || 
					stream_type==SERVICE_TYPE_AUDIO_AC3 ||
					stream_type==SERVICE_TYPE_AUDIO_E_AC3 ||
					stream_type==SERVICE_TYPE_DVB_SUBTITLES2 ||
					stream_type==DESCRIPTOR_DVB_TELETEXT ||
          stream_type==SERVICE_TYPE_AUDIO_AAC ||
          stream_type==SERVICE_TYPE_AUDIO_LATM_AAC
					);
}

void CDiskRecorder::AddStream(PidInfo2 pidInfo)
{
  ivecPidInfo2 it = m_vecPids.begin();
  while (it!=m_vecPids.end())
  {
		PidInfo2 info=*it;
		if (info.elementaryPid==pidInfo.elementaryPid)
      return;
    ++it;
  }
  //ITV HD workaround, this enables TSWriter to timeshift / record & avoids no audio/video found.
  if (pidInfo.streamType==SERVICE_TYPE_DVB_SUBTITLES2 && pidInfo.logicalStreamType==0xffffffff && pidInfo.elementaryPid==0xd49)
  {
    pidInfo.streamType=SERVICE_TYPE_VIDEO_H264;
    pidInfo.logicalStreamType=SERVICE_TYPE_VIDEO_H264;
    LogDebug("AddStream: set ITV HD video stream to H.264");
  }
  //end of workaround
	if (IsStreamWanted(pidInfo.logicalStreamType))
	{
		PidInfo2 pi;
		pi.NPktQ=0 ;
		pi.ccPrev=255;
		pi.seenStart=false;
		pi.fakePid=-1;
		pi.elementaryPid=pidInfo.elementaryPid;
		pi.streamType=pidInfo.streamType;
		pi.logicalStreamType=pidInfo.logicalStreamType;
		pi.rawDescriptorSize=pidInfo.rawDescriptorSize;
		memset(pi.rawDescriptorData,0xFF,pi.rawDescriptorSize);
		memcpy(pi.rawDescriptorData,pidInfo.rawDescriptorData,pi.rawDescriptorSize);
		if (pidInfo.logicalStreamType==SERVICE_TYPE_AUDIO_AC3 || pidInfo.logicalStreamType==SERVICE_TYPE_AUDIO_E_AC3 || pidInfo.streamType==SERVICE_TYPE_AUDIO_MPEG1 || pidInfo.streamType==SERVICE_TYPE_AUDIO_MPEG2 || pidInfo.streamType==SERVICE_TYPE_AUDIO_AAC  || pidInfo.streamType==SERVICE_TYPE_AUDIO_LATM_AAC)
		{
			pi.fakePid=DR_FAKE_AUDIO_PID;
			DR_FAKE_AUDIO_PID++;
			m_vecPids.push_back(pi);
			WriteLog("add audio stream pid: 0x%x fake pid: 0x%x stream type: 0x%x logical type: 0x%x descriptor length: %d",pidInfo.elementaryPid,pi.fakePid,pidInfo.streamType,pidInfo.logicalStreamType,pidInfo.rawDescriptorSize);
		}
		else if (pidInfo.streamType==SERVICE_TYPE_VIDEO_MPEG1 || 
      		   pidInfo.streamType==SERVICE_TYPE_VIDEO_MPEG2 ||
      		   pidInfo.streamType==SERVICE_TYPE_AUDIO_MPEG1 || 
      		   pidInfo.streamType==SERVICE_TYPE_VIDEO_MPEG4 || 
      		   pidInfo.streamType==SERVICE_TYPE_AUDIO_MPEG1 || 
      		   pidInfo.streamType==SERVICE_TYPE_VIDEO_H264 || 
      		   pidInfo.streamType==SERVICE_TYPE_VIDEO_MPEG2_DCII || 
      		   pidInfo.streamType==SERVICE_TYPE_VIDEO_HEVC)
		{
			pi.fakePid=DR_FAKE_VIDEO_PID;
			DR_FAKE_VIDEO_PID++;
			m_vecPids.push_back(pi);
			WriteLog("add video stream pid: 0x%x fake pid: 0x%x stream type: 0x%x logical type: 0x%x descriptor length: %d",pidInfo.elementaryPid,pi.fakePid,pidInfo.streamType,pidInfo.logicalStreamType,pidInfo.rawDescriptorSize);			
		}
		if (pidInfo.logicalStreamType==DESCRIPTOR_DVB_TELETEXT)
		{
			pi.fakePid=DR_FAKE_TELETEXT_PID;
			DR_FAKE_TELETEXT_PID++;
			m_vecPids.push_back(pi);
			WriteLog("add teletext stream pid: 0x%x fake pid: 0x%x stream type: 0x%x logical type: 0x%x descriptor length: %d",pidInfo.elementaryPid,pi.fakePid,pidInfo.streamType,pidInfo.logicalStreamType,pidInfo.rawDescriptorSize);
		}
		else if (pidInfo.logicalStreamType==SERVICE_TYPE_DVB_SUBTITLES1 || pidInfo.logicalStreamType==SERVICE_TYPE_DVB_SUBTITLES2)
		{
			pi.fakePid=DR_FAKE_SUBTITLE_PID;
			DR_FAKE_SUBTITLE_PID++;
			m_vecPids.push_back(pi);
			WriteLog("add subtitle stream pid: 0x%x fake pid: 0x%x stream type: 0x%x logical type: 0x%x descriptor length: %d",pidInfo.elementaryPid,pi.fakePid,pidInfo.streamType,pidInfo.logicalStreamType,pidInfo.rawDescriptorSize);
		}

		if (pi.elementaryPid==m_iOriginalPcrPid)
			m_iFakePcrPid=pi.fakePid;
	}
	else
		WriteLog("stream rejected - pid: 0x%x stream type: 0x%x logical type: 0x%x descriptor length: %d",pidInfo.elementaryPid,pidInfo.streamType,pidInfo.logicalStreamType,pidInfo.rawDescriptorSize);
}


int GetPesHeader(byte* tsPacket, CTsHeader& header, PidInfo2& PidInfo)
{
	int MaxLengthToCopy = 188 - header.PayLoadStart ;
	if (header.PayloadUnitStart)
	{
		PidInfo.PesHeaderLength = 0 ;
		PidInfo.NPktQ = 0 ;
		PidInfo.PesHeaderLength = 0 ;
	}
	if (PidInfo.NPktQ>=4) 
	{
		LogDebug("Recorder:PesHeader Pid %x Cannot decrypt PES ( splitted in more than 4 ts packets )",header.Pid ) ;
		return 1 ; // therefore, write packets.
	}
		
	memcpy(&PidInfo.TsPktQ[PidInfo.NPktQ][0], tsPacket, 188) ;
	PidInfo.TsHeaderQ[PidInfo.NPktQ] = header ;
	PidInfo.NPktQ++ ;

	if (PidInfo.PesHeaderLength+MaxLengthToCopy > 19) MaxLengthToCopy = 19 - PidInfo.PesHeaderLength ;
	memcpy(&PidInfo.PesHeader[PidInfo.PesHeaderLength], &tsPacket[header.PayLoadStart], MaxLengthToCopy) ;

	PidInfo.PesHeaderLength += MaxLengthToCopy ;

	if ((PidInfo.PesHeaderLength >= 7) && ((PidInfo.PesHeader[6] & 0xC0)!=0x80))
	{
//		LogDebug("Recorder:PesHeader Pid %x ( No-PTS-DTS -4 ) Sz %d Length : %d %2.2x %2.2x %2.2x %2.2x %2.2x %2.2x %2.2x",header.Pid, (PidInfo.PesHeader[4]<<8)+PidInfo.PesHeader[5],PidInfo.PesHeaderLength,PidInfo.PesHeader[0],PidInfo.PesHeader[1],PidInfo.PesHeader[2],PidInfo.PesHeader[3],PidInfo.PesHeader[4],PidInfo.PesHeader[5],PidInfo.PesHeader[6]) ;
		return 0 ;	// Empty PES.
	}

	if (PidInfo.PesHeaderLength >= 8)
	{
		int RequiredLength = 8 ;
		switch (PidInfo.PesHeader[7] & 0xC0)
		{
			case 0x80 :
			case 0x40 : if (PidInfo.PesHeaderLength>=14)
						{
//							if (PidInfo.NPktQ > 1)
//								LogDebug("Recorder:PesHeader Pid %x ( Ready PTS or DTS ) Sz %d Length : %d %2.2x %2.2x, Q:%d",header.Pid, (PidInfo.PesHeader[4]<<8)+PidInfo.PesHeader[5],PidInfo.PesHeaderLength,PidInfo.PesHeader[6],PidInfo.PesHeader[7],PidInfo.NPktQ) ;
							return 1 ;	// Ready to read.
						}
						else
						{
//							LogDebug("Recorder:PesHeader Pid %x ( Wait next - 1) Sz %d Length : %d %2.2x %2.2x, Q:%d",header.Pid, (PidInfo.PesHeader[4]<<8)+PidInfo.PesHeader[5],PidInfo.PesHeaderLength,PidInfo.PesHeader[6],PidInfo.PesHeader[7],PidInfo.NPktQ) ;
							return 2 ;	// Not enough data.
						}

			case 0xC0 : if (PidInfo.PesHeaderLength>=19)
						{
//							if (PidInfo.NPktQ > 1)
//								LogDebug("Recorder:PesHeader Pid %x ( Ready : PTS+DTS ) Sz %d Length : %d %2.2x %2.2x, Q:%d",header.Pid, (PidInfo.PesHeader[4]<<8)+PidInfo.PesHeader[5],PidInfo.PesHeaderLength,PidInfo.PesHeader[6],PidInfo.PesHeader[7],PidInfo.NPktQ) ;
							return 1 ;	// Ready to read.
						}
						else
						{
//							LogDebug("Recorder:PesHeader Pid %x ( Wait next - 2) Sz %d Length : %d %2.2x %2.2x, Q:%d",header.Pid, (PidInfo.PesHeader[4]<<8)+PidInfo.PesHeader[5],PidInfo.PesHeaderLength,PidInfo.PesHeader[6],PidInfo.PesHeader[7],PidInfo.NPktQ) ;
							return 2 ;	// Not enough data.
						}
			default:
			case 0x00 :	// no PTS-DTS 
						{
//						LogDebug("Recorder:PesHeader Pid %x ( No PTS-DTS - 3) Sz %d Length : %d %2.2x %2.2x, Q:%d",header.Pid, (PidInfo.PesHeader[4]<<8)+PidInfo.PesHeader[5],PidInfo.PesHeaderLength,PidInfo.PesHeader[6],PidInfo.PesHeader[7],PidInfo.NPktQ) ;
						return 0 ; // no PTS-DTS...
						}
		}
	}
	else
	{
//		LogDebug("Recorder:PesHeader Pid %x ( Wait next - 5 ) Sz %d Length : %d %2.2x %2.2x %2.2x %2.2x %2.2x %2.2x %2.2x",header.Pid, (PidInfo.PesHeader[4]<<8)+PidInfo.PesHeader[5],PidInfo.PesHeaderLength,PidInfo.PesHeader[0],PidInfo.PesHeader[1],PidInfo.PesHeader[2],PidInfo.PesHeader[3],PidInfo.PesHeader[4],PidInfo.PesHeader[5],PidInfo.PesHeader[6]) ;
		return 2 ;	// Not enough data.
	}
}

void UpdatePesHeader(PidInfo2& PidInfo)
{
	int i=0 ;
	int pos = 0 ;
	do
	{
		int lenH = 188-PidInfo.TsHeaderQ[i].PayLoadStart ;
		if (lenH+pos > PidInfo.PesHeaderLength) lenH = PidInfo.PesHeaderLength - pos ;
		if (lenH > 0)
			memcpy(&PidInfo.TsPktQ[i][PidInfo.TsHeaderQ[i].PayLoadStart], &PidInfo.PesHeader[pos], lenH) ;
		pos+=lenH ;
		i++ ;
	} while(i<PidInfo.NPktQ) ;
}

void CDiskRecorder::WriteTs(byte* tsPacket)
{	  	  
	CEnterCriticalSection enter(m_section);
	try{
		m_TsPacketCount++;
		if (m_TsPacketCount < IGNORE_AFTER_TUNE) return;
		if (m_iOriginalPcrPid<0 || m_vecPids.size()==0 || m_iPmtPid<0) return;

		m_tsHeader.Decode(tsPacket);
		if (m_tsHeader.TScrambling)
    {
      // If the packet has no payload then don't drop it - it might have PCR in the adaption field
      // that is not actually scrambled. This is a workaround for bad CAM behaviour.
      if (!m_tsHeader.AdaptionFieldOnly())
      {
        return;
      }
      // Mark the packet as not scrambled.
      m_tsHeader.TScrambling = 0;
      tsPacket[3] = tsPacket[3] & 0x3f;
    }
		if (m_tsHeader.TransportError) 	{ LogDebug("Recorder:Pid %x : Transport error flag set!", m_tsHeader.Pid) ; return ; }

		if (m_iPacketCounter>=100)
		{
			WriteFakePAT();
			WriteFakePMT();
			m_iPacketCounter=0;
		}

		int PayLoadUnitStart=0;
		if (m_tsHeader.PayloadUnitStart) PayLoadUnitStart=1;

		ivecPidInfo2 it=m_vecPids.begin();
		while (it!=m_vecPids.end())
		{	
			PidInfo2 &info=*it;

			if (m_tsHeader.Pid==info.elementaryPid)
			{
				if (m_tsHeader.AdaptionFieldLength && (tsPacket[5] & 0x80))
          LogDebug("Recorder:Pid %x : Discontinuity header bit set!", m_tsHeader.Pid);
				if (info.ccPrev!=255)
				{
					// Do not check 1st packet after channel change.
					if (m_tsHeader.HasPayload)
					{
						// Check Ts packet continuity with payload, remove duplicate frames.
						if ((m_tsHeader.ContinuityCounter != ((info.ccPrev+1) & 0x0F)))
						{
              m_iTsContinuityCounter++;
							/*
              if (m_tsHeader.ContinuityCounter == info.ccPrev)
							{	
								// May be duplicated....
								int PayLoadLen = 188-m_tsHeader.PayLoadStart ;
								if (PayLoadLen<0) PayLoadLen = 0 ;
								if (((PayLoadLen) && (memcmp(info.m_Pkt+m_tsHeader.PayLoadStart, tsPacket+m_tsHeader.PayLoadStart, PayLoadLen)==0)) || (PayLoadLen==0)) 
								{
									LogDebug("Recorder:Pid %x Same ts packet...( removed ) %x, PayLoadLen : %d", m_tsHeader.Pid, info.ccPrev, PayLoadLen) ;
									return ; 
								}
								else
                
                LogDebug("Recorder:Pid %x Continuity error...! Should be same ! %x ( prev %x ), PayLoadLen : %d", m_tsHeader.Pid, m_tsHeader.ContinuityCounter, info.ccPrev, PayLoadLen) ;
							}
							else
              */
						  LogDebug("Recorder:Pid %x Continuity error... %x ( prev %x ) - bad signal?", m_tsHeader.Pid, m_tsHeader.ContinuityCounter, info.ccPrev) ;
						}
					}
					else
					{
						// Check Ts packet continuity without payload.
						if (m_tsHeader.ContinuityCounter != info.ccPrev)
								LogDebug("Recorder:Pid %x , No PayLoad, Continuity Counter should be the same ! %x ( prev %x )", m_tsHeader.Pid, m_tsHeader.ContinuityCounter, info.ccPrev) ;
					}
				}
				info.ccPrev = m_tsHeader.ContinuityCounter ;

				memcpy(info.m_Pkt,tsPacket,188);
	
				if (info.streamType==SERVICE_TYPE_VIDEO_MPEG1 || 
  				  info.streamType==SERVICE_TYPE_VIDEO_MPEG2 ||
  				  info.streamType==SERVICE_TYPE_VIDEO_MPEG4 ||
  				  info.streamType==SERVICE_TYPE_VIDEO_H264 || 
  				  info.streamType==SERVICE_TYPE_VIDEO_MPEG2_DCII ||
  				  info.streamType==SERVICE_TYPE_VIDEO_HEVC)
				{
					//video
					if (!info.seenStart) 
					{
						if (PayLoadUnitStart)
						{
							info.seenStart=true;
							m_AudioOrVideoSeen=true;
							WriteLog("start of video detected");
	              			if(m_pVideoAudioObserver) m_pVideoAudioObserver->OnNotify(PidType::Video);
						}
					else return ;
					}
				}
	
				if (info.streamType==SERVICE_TYPE_AUDIO_MPEG1 || info.streamType==SERVICE_TYPE_AUDIO_MPEG2|| info.logicalStreamType==SERVICE_TYPE_AUDIO_AC3 || info.logicalStreamType==SERVICE_TYPE_AUDIO_E_AC3 || info.logicalStreamType==SERVICE_TYPE_AUDIO_AAC || info.logicalStreamType==SERVICE_TYPE_AUDIO_LATM_AAC)
				{
					//audio
					if (!info.seenStart)
					{
						if (PayLoadUnitStart)
						{
							info.seenStart=true;
							m_AudioOrVideoSeen=true;
							WriteLog("start of audio detected");
	              			if(m_pVideoAudioObserver) m_pVideoAudioObserver->OnNotify(PidType::Audio);
						}
						else return ;
					}
				}
	
				if (info.logicalStreamType==SERVICE_TYPE_DVB_SUBTITLES1 || info.streamType==SERVICE_TYPE_DVB_SUBTITLES2)
				{
					// subtitles
					if (!info.seenStart)
					{
						if (PayLoadUnitStart) info.seenStart=true;
						else return ;
					}                        
				}


        //---------------------------------------------------------------------
        // Handling for PCR timestamps when they are carried in one of the main
        // streams.

        // If the channel PMT indicates there is no PCR PID (see ISO 13818-1
        // MPEG 2 TS spec page 47, TS_program_map_section, PCR_PID) then check
        // if this packet contains PCR that we can use. We do this as a last
        // resort. Without PCR, we currently can't timeshift/record the stream.
        // See http://forum.team-mediaportal.com/threads/dvb-c-radio-not-working.118141/
        if (m_iOriginalPcrPid == 0x1fff && m_tsHeader.HasAdaptionField && m_tsHeader.AdaptionFieldLength >= 7 && (info.m_Pkt[ADAPTION_FIELD_FLAG_OFFSET] & PCR_FLAG_BIT) != 0)
        {
          WriteLog("currently no PCR PID, PID 0x%x appears to have PCR so we'll attempt to use it", m_tsHeader.Pid);
          m_iOriginalPcrPid = m_tsHeader.Pid;
          m_iFakePcrPid = info.fakePid;
        }

        // Normal case: PCR timestamps carried in a main stream.
        if (m_tsHeader.Pid == m_iOriginalPcrPid)
        {
          // Overwrite the PCR timestamp.
          PatchPcr(info.m_Pkt, m_tsHeader);
        }
        // Edge case: undesirable PCR timestamps.
        else if (info.fakePid == m_iFakePcrPid && m_tsHeader.HasAdaptionField && m_tsHeader.AdaptionFieldLength >= 7 && (info.m_Pkt[ADAPTION_FIELD_FLAG_OFFSET] & PCR_FLAG_BIT) != 0)
        {
          // If we get to here then we've found a PCR timestamp in a packet
          // that is not expected to contain PCR timestamps. That isn't a
          // problem unless this packet comes from the stream that we are
          // *inserting* PCR timestamps into (see below). We have identified
          // such a problem here!
          // If we don't do anything about the problem, it will break
          // TsReader's ability to skip, fast-forward and rewind.
          // The solution is simple: remove the PCR timestamp from the adaption
          // field!

          // Clear the PCR flag.
          info.m_Pkt[ADAPTION_FIELD_FLAG_OFFSET] &= (~PCR_FLAG_BIT);
          // Overwrite the PCR timestamp with the rest of the adaption field.
          // The -1 is for the adaption field flag byte.
          memcpy(&info.m_Pkt[PCR_OFFSET], &tsPacket[PCR_OFFSET + PCR_LENGTH], m_tsHeader.AdaptionFieldLength - PCR_LENGTH - 1);
          // The end of the adaption field is now stuffing.
          memset(&info.m_Pkt[PCR_OFFSET + m_tsHeader.AdaptionFieldLength - PCR_LENGTH - 1], 0xff, PCR_LENGTH);
        }
        //---------------------------------------------------------------------


        //---------------------------------------------------------------------
        // Handling for regular packets from one of the main streams.
        if (!m_bDetermineNewStartPcr && m_bStartPcrFound) 
        {
          // Overwrite the PID.
          int pid = info.fakePid;
          info.m_Pkt[1] = (PayLoadUnitStart << 6) + ((pid >> 8) & 0x1f);
          info.m_Pkt[2] = (pid & 0xff);

          if (info.seenStart && (PayLoadUnitStart || info.NPktQ))
          {
            int i = 0;
            switch (GetPesHeader(info.m_Pkt, m_tsHeader, info))
            {
              case 2:
                break;    // Wait for the next TS packet.
              case 1:
                PatchPtsDts(info.PesHeader, m_tsHeader, info);
                UpdatePesHeader(info);
              case 0:
                do
                {
                  Write(&info.TsPktQ[i++][0], 188);
                  m_iPacketCounter++;
                }
                while (--info.NPktQ);
            }
          }
          else
          {
            Write(info.m_Pkt, 188);
            m_iPacketCounter++;
          }
        }
        return;
        //---------------------------------------------------------------------
      }
      ++it;
    }

    //-------------------------------------------------------------------------
    // Handling for PCR timestamps when they are *not* carried in one of the
    // main streams.
    if (m_tsHeader.Pid == m_iOriginalPcrPid && m_AudioOrVideoSeen)
    {
      // Insert a PCR timestamp in a fresh packet.
      byte pkt[200];
      memcpy(pkt, tsPacket, 188);
      PatchPcr(pkt, m_tsHeader);
      pkt[1] = ((m_iFakePcrPid >> 8) & 0x1f);
      pkt[2] = (m_iFakePcrPid & 0xff);
      pkt[3] = (2 << 4);// Adaption Field Control==adaptation field only, no payload
      pkt[4] = 0xb7;

      if (!m_bDetermineNewStartPcr && m_bStartPcrFound) 
      {            
        Write(pkt,188);
        m_iPacketCounter++;
      }
      return;
    }
    //-------------------------------------------------------------------------
  }
  catch (...)
  {
    WriteLog("Exception in WriteTs");
  }
}

void CDiskRecorder::WriteFakePAT()
{
	int tableId=TABLE_ID_PAT;
	int transportId=DR_FAKE_TRANSPORT_ID;
	int pmtPid=DR_FAKE_PMT_PID;
	int sectionLenght=9+4;
	int current_next_indicator=1;
	int section_number = 0;
	int last_section_number = 0;

	int pid=PID_PAT;
	int PayLoadUnitStart=1;
	int AdaptionControl=1;
	m_iPatContinuityCounter++;
	if (m_iPatContinuityCounter>0xf) m_iPatContinuityCounter=0;

	BYTE pat[200];
	memset(pat,0xFF,sizeof(pat));
	pat[0]=0x47;
	pat[1]=(PayLoadUnitStart<<6) + ( (pid>>8) & 0x1f);
	pat[2]=(pid&0xff);
	pat[3]=(AdaptionControl<<4) +m_iPatContinuityCounter;		//0x10
	pat[4]=0;																								//0

	pat[5]=tableId;//table id																//0
	pat[6]=0xb0+((sectionLenght>>8)&0xf);										//0xb0
	pat[7]=sectionLenght&0xff;
	pat[8]=(transportId>>8)&0xff;
	pat[9]=(transportId)&0xff;
	pat[10]=((m_iPatVersion&0x1f)<<1)+current_next_indicator;
	pat[11]=section_number;
	pat[12]=last_section_number;
	pat[13]=(DR_FAKE_SERVICE_ID>>8)&0xff;
	pat[14]=(DR_FAKE_SERVICE_ID)&0xff;
	pat[15]=((pmtPid>>8)&0xff)|0xe0;
	pat[16]=(pmtPid)&0xff;

	int len=17;
	DWORD crc= crc32((char*)&pat[5],len-5);
	pat[len]=(byte)((crc>>24)&0xff);
	pat[len+1]=(byte)((crc>>16)&0xff);
	pat[len+2]=(byte)((crc>>8)&0xff);
	pat[len+3]=(byte)((crc)&0xff);
	Write(pat,188);
}

void CDiskRecorder::WriteFakePMT()
{
	int program_info_length=0;
	int sectionLenght=9+2*5+5;

	int current_next_indicator=1;
	int section_number = 0;
	int last_section_number = 0;
	int transportId=DR_FAKE_TRANSPORT_ID;

	int tableId=2;
	int pid=DR_FAKE_PMT_PID;
	int PayLoadUnitStart=1;
	int AdaptionControl=1;

	m_iPmtContinuityCounter++;
	if (m_iPmtContinuityCounter>0xf) m_iPmtContinuityCounter=0;

	BYTE pmt[MAX_SECTION_LENGTH]; 

	memset(pmt,0xff,sizeof(pmt));
	pmt[0]=0x47;
	pmt[1]=(PayLoadUnitStart<<6) + ( (pid>>8) & 0x1f);
	pmt[2]=(pid&0xff);
	pmt[3]=(AdaptionControl<<4) +m_iPmtContinuityCounter;
	pmt[4]=0;
	byte* pmtPtr=&pmt[4];
	pmt[5]=tableId;//table id
	pmt[6]=0;
	pmt[7]=0;
	pmt[8]=(DR_FAKE_SERVICE_ID>>8)&0xff;
	pmt[9]=(DR_FAKE_SERVICE_ID)&0xff;
	pmt[10]=((m_iPmtVersion&0x1f)<<1)+current_next_indicator;
	pmt[11]=section_number;
	pmt[12]=last_section_number;
	pmt[13]=(m_iFakePcrPid>>8)&0xff;
	pmt[14]=(m_iFakePcrPid)&0xff;
	pmt[15]=(program_info_length>>8)&0xff;
	pmt[16]=(program_info_length)&0xff;

	int pmtLength=9+4;
	int offset=17;
	ivecPidInfo2 it=m_vecPids.begin();
	while (it!=m_vecPids.end())
	{
		PidInfo2 info=*it;
		pmt[offset++]=info.streamType;
		pmt[offset++]=0xe0+((info.fakePid>>8)&0x1F); // reserved; elementary_pid (high)
		pmt[offset++]=(info.fakePid)&0xff; // elementary_pid (low)
		pmt[offset++]=0; // es_length (high)
		pmt[offset++]=0; // es_length (low)
		pmtLength+=5;
		if (info.rawDescriptorData!=NULL)
		{
			pmt[offset-2]=0xF0+((info.rawDescriptorSize>>8)&0x3); //Mask to 2 bits then set 'reserved' bits
			pmt[offset-1]=(info.rawDescriptorSize&0xFF);
			memcpy(&pmt[offset],info.rawDescriptorData,info.rawDescriptorSize);
			offset += info.rawDescriptorSize;
			pmtLength += info.rawDescriptorSize;
		}
		++it;
	}
	unsigned section_length = (pmtLength);
	pmt[6]=0xb0+((section_length>>8)&0xf);
	pmt[7]=section_length&0xff;

	DWORD crc= crc32((char*)&pmt[5],offset-5);
	pmt[offset++]=(byte)((crc>>24)&0xff);
	pmt[offset++]=(byte)((crc>>16)&0xff);
	pmt[offset++]=(byte)((crc>>8)&0xff);
	pmt[offset++]=(byte)((crc)&0xff);

	//if(pmtLength > 188) WriteLog("ERROR: Pmt length : %i ( >188 )!!!!",pmtLength);

	Write(pmt,188);
	pmtLength -= 180 ;
	int pointer = 188 ;
	while(pmtLength > 0)
	{
		byte packet[188];
		memset(packet,0xff,188);
		packet[0]=0x47;
		packet[1]=(pid>>8) & 0x1f;
		packet[2]=(pid&0xff);
		m_iPmtContinuityCounter++;
		if (m_iPmtContinuityCounter>0xf) m_iPmtContinuityCounter=0;
		packet[3]=(AdaptionControl<<4) +m_iPmtContinuityCounter;
		int Length = pmtLength ;
		if (Length > 184) Length = 184 ;
		memcpy(&packet[4],&pmt[pointer],Length);
		Write(packet,188);
		pmtLength -= Length ;
		pointer += Length ;
	}
}


// - Ambass -
// To be able to patch in an easy way PCR & PTS/DTS, all this part will use their 90kHz clock resolution. ( 27MHz extension is never used. )

__int64 CDiskRecorder::EcPcrTime(__int64 New, __int64 Prev)
{   // Compute a signed difference between new and previous 33bits timer.
		__int64 dt = New - Prev ;
		if (dt & 0x100000000) dt |= 0xFFFFFFFF00000000LL ;  // Negative
		else                  dt &= 0x00000000FFFFFFFFLL ;  // Positive
		return dt ;
}

void CDiskRecorder::PatchPcr(byte* tsPacket,CTsHeader& header)
{
	bool wr=false;
	bool wjump=false;
	bool verbose=false ;															// Debug verbosity(true) or Info (false)

	if (header.PayLoadOnly()) return;
	m_adaptionField.Decode(header,tsPacket);
	if (m_adaptionField.PcrFlag==false) return;
	CPcr pcrNew=m_adaptionField.Pcr;

	DWORD TimeStamp = GetTickCount();                 // try to have an idea of elapsed time from previous PCR ( used for long delays )
	__int64 dt2 = ((__int64)(TimeStamp - m_prevTimeStamp)) * 90 /*KHz*/  ;
	
	if (m_bStartPcrFound==false)                      // Tv or recording turned on. !
	{
		m_bDetermineNewStartPcr=false;
		m_bStartPcrFound=true;
		m_JumpInProgress=0 ;
		m_PcrSpeed = 4500 ;															// Should start with a non stupid value, common value seems 6~50 mS ( 540~4500 ).
		m_PcrCompensation = (0x000000LL - (__int64) pcrNew.PcrReferenceBase) & 0x1FFFFFFFFLL ;  // Compensation to apply ( Expected fake PCR start - current PCR ) always modulo 33 bits.
		m_prevPcr = pcrNew ;

		WriteFakePAT();
		WriteFakePMT();
		m_iPacketCounter=0;

		wr=verbose ;      
		{                       // Info.
			CPcr NextPcr ;
			NextPcr.PcrReferenceBase = 0x1FFFFFFFF - pcrNew.PcrReferenceBase ;
			NextPcr.PcrReferenceExtension = 0 ;
			WriteLog("Info : Next broadcaster program clock reference rollover : %s", NextPcr.ToString()) ;
		}
	}
	else
	{
		if ((pcrNew.PcrReferenceBase < 0x10000) && (m_prevPcr.PcrReferenceBase > 0x1FFFF0000))  // Just info, on rollover ,previous was 1FFFxxxx, new is 0x00000xxxx
			WriteLog("Info : Normal broadcaster program clock reference rollover passed !") ;

		__int64 dt = EcPcrTime( pcrNew.PcrReferenceBase, m_prevPcr.PcrReferenceBase ) ;  // Delta from previous PCR
		if (m_bDetermineNewStartPcr)
		{
			m_bDetermineNewStartPcr=false;
			m_JumpInProgress=0 ;
			m_PcrCompensation -= dt ;                   // Now it result as a "zero" delta on Fake PCR.
			m_PcrCompensation += (__int64)m_PcrSpeed ;  // Increase it a bit with averaged delta should not be stupid.
			m_prevPcr = pcrNew ;

			WriteFakePAT();
			WriteFakePMT();
			m_iPacketCounter=0;

			wr=verbose ;
			{                       // Info.
				CPcr NextPcr ;
				NextPcr.PcrReferenceBase = 0x1FFFFFFFF - pcrNew.PcrReferenceBase ;
				NextPcr.PcrReferenceExtension = 0 ;
				WriteLog("Info : Next broadcaster program clock reference rollover : %s", NextPcr.ToString()) ;
			}
		}
		else
		{
			if (dt < 0)                                                                 // Could not admit a negative jump in Fake PCR
			{
				__int64 FutureComp   = m_PcrCompensation - dt + (__int64)m_PcrSpeed ;       // If the jump is real, this will be ne future compensation to apply.
				if (m_JumpInProgress)                                                       // Jump in progress...
				{
					__int64 EcFutureComp = EcPcrTime(m_PcrFutureCompensation, FutureComp) ;     // Check if future compensation quite equal to previous one ( Arbitrary values proportional on common PCR delta ) 
					if ((EcFutureComp < - 8*m_PcrSpeed) || (EcFutureComp > 8*m_PcrSpeed))
					{                                                                             // false ! Jump is different !!!
						m_PcrFutureCompensation = FutureComp ;
						m_prevPcr.PcrReferenceBase += (__int64)( m_PcrSpeed  / 2 ) ;                // Evaluate current PCR to recover in case of false jump. 
						m_JumpInProgress=1 ;                                                        // Restart confirmation cycle

						if (verbose) WriteLog("PCR backward jump ( %I64d ) New !! ( Confirm count = %d ), Wait again for confirmation.", dt, m_JumpInProgress) ;
						wjump=verbose ;
					}
					else
					{
						m_JumpInProgress++ ;                                                        // true !
						if (m_JumpInProgress >= 3)                                                  // Jump comleted ?
						{
							m_PcrCompensation = FutureComp ;                                             // Update compensation
							m_prevPcr = pcrNew ;    
							m_JumpInProgress=0 ;                                                         // Jump completed.

							WriteLog("Info : Program clock reference backward jump ( %I64d ).", dt) ;
						  wjump=verbose ;
						}
						else
							m_prevPcr.PcrReferenceBase += (__int64)( m_PcrSpeed / 2 ) ;           // Evaluate current PCR to recover in case of false jump. 
					}
				}
				else
				{                                                                           // Jump detected, but may be false, need to confirm, no change 
					m_JumpInProgress=1 ;
					m_PcrFutureCompensation = FutureComp ;
					m_prevPcr.PcrReferenceBase += (__int64)( m_PcrSpeed  / 2 ) ;              // Evaluate current PCR to recover in case of false jump. 
                                                                                    // Approximative and slower than reality to avoid falling in negative case.
					if (verbose) WriteLog("PCR backward jump ( %I64d ) detected, Wait for confirmation.", dt) ;
					wjump=verbose ;
				}
			}
			else
			{                                                                             // Positive jump.
				if (dt > 8*m_PcrSpeed)                                                      // Arbitrary threshold ( Arbitrary values proportional on common PCR delta  )
				{                                                                           // "long" jump ?
					__int64 dt3 = dt2-dt ;
					if ((dt3 < 15000) && (dt3 > -15000))                                         // Arbitrary values, Is the jump coherent to system clock 
					{                                                                                // Yes, probable signal lost, do nothing.
						if (verbose) WriteLog("PCR forward jump ( %I64d ) detected with coherent time jump ( %I64d ) : Nothing done ( signal lost ? ).", dt, dt2) ;
						m_prevPcr = pcrNew ;    
					}
					else
					{                                                                                // No,
						__int64 FutureComp   = m_PcrCompensation - dt + (__int64)m_PcrSpeed ;          // If the jump is real, this will be the future compensation to apply.
						if (m_JumpInProgress)
						{
							__int64 EcFutureComp = EcPcrTime(m_PcrFutureCompensation, FutureComp) ;      // Check if future compensation quite equal to previous one ( Arbitrary values proportional on common PCR delta ) 
							if ((EcFutureComp < - 8*m_PcrSpeed) || (EcFutureComp > 8*m_PcrSpeed))                                                                                                                          
							{                                                                               // false ! Jump is different !!!                                                                                
								m_PcrFutureCompensation = FutureComp ;                                                                                                                                                       
								m_prevPcr.PcrReferenceBase += (__int64)( m_PcrSpeed  / 2 ) ;                  // Evaluate current PCR to recover in case of false jump. 
								m_JumpInProgress=1 ;                                                          // Restart confirmation cycle

								if (verbose) WriteLog("PCR forward jump ( %I64d ) New !! ( Confirm count = %d ), Wait again for confirmation.", dt, m_JumpInProgress) ;
								wjump=verbose ;                                             
							}
							else
							{
								m_JumpInProgress++ ;                                                          // true !                 
								if (m_JumpInProgress >= 3)                                                    // Jump comleted ?        
								{                                                                                                 
									m_PcrCompensation = FutureComp ;                                              // Update compensation 
									m_prevPcr = pcrNew ;    
									m_JumpInProgress=0 ;                                                          // Jump completed.

									WriteLog("Info : Program clock reference forward jump ( %I64d ).", dt) ;
									wjump=verbose ;
								}
								else
									m_prevPcr.PcrReferenceBase += (__int64)( m_PcrSpeed  / 2 ) ;                  // Evaluate current PCR to recover in case of false jump. 
							}
						}
						else
						{      
							m_JumpInProgress=1 ;                                                      // Jump detected, but may be false, need to confirm, no change             
							m_PcrFutureCompensation = m_PcrCompensation - dt + (__int64)m_PcrSpeed ;                                                              
							m_prevPcr.PcrReferenceBase += (__int64)( m_PcrSpeed  / 2 ) ;              // Evaluate current PCR to recover in case of false jump.                  
                                                                                        // Approximative and slower than reality to avoid falling in negative case.
							if (verbose) WriteLog("PCR forward jump ( %I64d, Time Jump %I64d ) detected, Wait for confirmation.", dt, dt2) ;
							wjump=verbose ;
						}
					}
				}
				else
				{
   				m_PcrSpeed += ((float)dt - m_PcrSpeed) * 0.1f ;                                // Time average between 2 PCR
					if (m_JumpInProgress && verbose)
					{
						WriteLog("PCR jump aborted after %d confirmation.", m_JumpInProgress) ;
					}
					m_JumpInProgress = 0 ;                                                        // Normal.
					m_prevPcr = pcrNew ;
				}
			}
		}
	}

	if (wjump)
	{
		WriteLog("PcrCompensation %I64x, newPcr %I64x, prevPcr %I64x, FutureComp %I64x, Speed %f,  ",m_PcrCompensation, pcrNew.PcrReferenceBase, m_prevPcr.PcrReferenceBase, m_PcrFutureCompensation, m_PcrSpeed) ;
//    WriteLog("tsheader:%02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x",
//					tsPacket[0],tsPacket[1],tsPacket[2],tsPacket[3],tsPacket[4],tsPacket[5],tsPacket[6],tsPacket[7],tsPacket[8],tsPacket[9],tsPacket[10],tsPacket[11]);
	}

	__int64 PcrObj = (m_prevPcr.PcrReferenceBase + m_PcrCompensation) & 0x1FFFFFFFFLL ;

	if (wr) WriteLog("Pcr : %I64x ( Prev : %I64x ), Comp : %I64x, >> Fake Pcr : %I64x ( TimeStamp %x) ", pcrNew.PcrReferenceBase, m_prevPcr.PcrReferenceBase, m_PcrCompensation, PcrObj, TimeStamp ) ;

	tsPacket[6] = (byte)(((PcrObj>>25)&0xff));
	tsPacket[7] = (byte)(((PcrObj>>17)&0xff));
	tsPacket[8] = (byte)(((PcrObj>>9)&0xff));
	tsPacket[9] = (byte)(((PcrObj>>1)&0xff));
	tsPacket[10]=	(byte)(((PcrObj&0x1)<<7) + (tsPacket[10] & 0x7e ));

	m_prevTimeStamp = TimeStamp ;
}

void CDiskRecorder::PatchPtsDts(byte* tsPacket,CTsHeader& header,PidInfo2& PidInfo)
{
	DWORD TimeStamp = GetTickCount();
	
	if (tsPacket[0] !=0 || tsPacket[1] !=0 || tsPacket[2] !=1) return; 

	byte* pesHeader=tsPacket;
	CPcr pts;
	CPcr dts;
	if (!CPcr::DecodeFromPesHeader(pesHeader,0,pts,dts))
	{
		WriteLog("No PTS-DTS to decode, should not be detected here ! ") ;
		return ;
	}

// ---- As it's really difficult to detect random jumps on non monotonic Video PTS ( occurs first, then audio, then PCR ) 
// ---- this is just to monitor random PTS/DTS jumps. PCR compensation is used as default. 
// ---- It seems enough to avoid definitive freezes & crashes....

	imapLastPtsDts it=m_mapLastPtsDts.begin() ;
	while(it != m_mapLastPtsDts.end())
	{
		if (it->m_Pid == header.Pid)  break ; 
		it++ ;
	}

	if (it==m_mapLastPtsDts.end())
	{
		LastPtsDtsRecord lastRec;
		lastRec.m_prevPts = -1;
		lastRec.m_prevDts = -1;
		lastRec.m_PtsCompensation = m_PcrCompensation ;
		lastRec.m_DtsCompensation = m_PcrCompensation ;
		lastRec.m_Pid = header.Pid ;
		m_mapLastPtsDts.push_back(lastRec) ;
		it = m_mapLastPtsDts.end() ;
		it-- ;
//Remove verbosity		WriteLog(">>> Add Pid %x ( Type : %x, Logical %x, fake pid %x ) <<<<<", it->m_Pid, PidInfo.streamType, PidInfo.logicalStreamType, PidInfo.fakePid ) ;
	}

	if (pts.IsValid)
	{
		if (it->m_prevPts!=-1)
		{
			__int64 dt = EcPcrTime( pts.PcrReferenceBase, it->m_prevPts ) ;
			__int64 dt2 = ((__int64)(TimeStamp - it->m_prevPtsTimeStamp)) * 90 /*KHz*/  ;
			__int64 dt3 = dt2-dt ;
//Remove verbosity if (((dt3 < -30000LL) || (dt3 > 30000LL)) && (((PidInfo.fakePid & 0xFF0)==DR_FAKE_VIDEO_PID) || ((PidInfo.fakePid & 0xFF0)==DR_FAKE_AUDIO_PID)))      
//Remove verbosity			{
//Remove verbosity				WriteLog(">>> Pts Jump detected %I64d ( Time %I64d  / DT %I64d )( new %I64x, prev %I64x, pid %x pespid %x)<<<<<", dt, dt2, dt3,  pts.PcrReferenceBase, it->m_prevPts, header.Pid, tsPacket[start+3]) ;
//Remove verbosity				WriteLog("head:%02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x", tsPacket[0],tsPacket[1],tsPacket[2],tsPacket[3],tsPacket[4],tsPacket[5],tsPacket[6],tsPacket[7],tsPacket[8],tsPacket[9],tsPacket[10],tsPacket[11]);
//Remove verbosity			}
		}

		it->m_prevPts = pts.PcrReferenceBase ;

		__int64 ptsPatched = (pts.PcrReferenceBase + m_PcrCompensation /*it->m_PtsCompensation*/) & 0x1FFFFFFFFLL ;

		// 9       10        11        12      13
		//76543210 76543210 76543210 76543210 76543210
		//0011pppM pppppppp pppppppM pppppppp pppppppM 
		//LogDebug("Recorder:pts: org:%s new:%s start:%s", ptsorg.ToString(),pts.ToString(),startPcr.ToString()); 

		pesHeader[13]=(byte)( ((ptsPatched&0x7f)<<1) + (pesHeader[13] & 0x01));
		ptsPatched>>=7;
		pesHeader[12]=(byte)(   (ptsPatched&0xff));				   ptsPatched>>=8;
		pesHeader[11]=(byte)( ((ptsPatched&0x7f)<<1) + (pesHeader[11] & 0x01));
		ptsPatched>>=7;
		pesHeader[10]=(byte)(   (ptsPatched&0xff));					 ptsPatched>>=8;
		pesHeader[9] =(byte)( ((ptsPatched&7)<<1)    + (pesHeader[9] & 0xF1)) ; 

		it->m_prevPtsTimeStamp = TimeStamp ;

		if (dts.IsValid)
		{
			if (it->m_prevDts!=-1)
			{
				__int64 dt  = EcPcrTime( dts.PcrReferenceBase, it->m_prevDts ) ;
				__int64 dt2 = ((__int64)(TimeStamp - it->m_prevPtsTimeStamp)) * 90 /*KHz*/  ;
				__int64 dt3 = dt2-dt ;
//Remove verbosity				if (((dt3 < -30000LL) || (dt3 > 30000LL)) && (((PidInfo.fakePid & 0xFF0)==DR_FAKE_VIDEO_PID) || ((PidInfo.fakePid & 0xFF0)==DR_FAKE_AUDIO_PID)))           
//Remove verbosity				{
//Remove verbosity					WriteLog(">>> Dts Jump detected %I64d ( new %I64x, prev %I64x, pid %x)<<<<<", dt, dts.PcrReferenceBase, it->m_prevDts, header.Pid) ;
//Remove verbosity					WriteLog("head:%02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x %02.2x", tsPacket[0],tsPacket[1],tsPacket[2],tsPacket[3],tsPacket[4],tsPacket[5],tsPacket[6],tsPacket[7],tsPacket[8],tsPacket[9],tsPacket[10],tsPacket[11]);
//Remove verbosity				}
			}

			it->m_prevPts = dts.PcrReferenceBase ;
			__int64 dtsPatched = (dts.PcrReferenceBase +  m_PcrCompensation /*it->m_DtsCompensation*/) & 0x1FFFFFFFFLL ;

//      WriteLog(" 14- %x 16- %x 18 %x",pesHeader[14] & 0xF1,pesHeader[16] & 0x01,pesHeader[18] & 0x01) ;
			// 14       15        16        17      18
			//76543210 76543210 76543210 76543210 76543210
			//0001pppM pppppppp pppppppM pppppppp pppppppM 
			//LogDebug("Recorder:dts: org:%s new:%s start:%s", dtsorg.ToString(),dts.ToString(),startPcr.ToString()); 
			pesHeader[18]=(byte)( ((dtsPatched&0x7f)<<1) + (pesHeader[18] & 0x01));
			dtsPatched>>=7;
			pesHeader[17]=(byte)(   (dtsPatched&0xff));				  dtsPatched>>=8;
			pesHeader[16]=(byte)( ((dtsPatched&0x7f)<<1) + (pesHeader[16] & 0x01));
			dtsPatched>>=7;
			pesHeader[15]=(byte)(   (dtsPatched&0xff));					dtsPatched>>=8;
			pesHeader[14]=(byte)( ((dtsPatched&7)<<1)    + (pesHeader[14] & 0xF1)); 

			it->m_prevDtsTimeStamp = TimeStamp ;
		}
	}
}

