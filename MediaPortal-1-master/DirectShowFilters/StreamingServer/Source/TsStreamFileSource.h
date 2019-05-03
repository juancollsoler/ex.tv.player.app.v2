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
59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
**********/
// "liveMedia"
// Copyright (c) 1996-2005 Live Networks, Inc.  All rights reserved.
// A file source that is a plain byte stream (rather than frames)
// C++ header

#pragma once

#ifndef _FRAMED_FILE_SOURCE_HH
#include "FramedFileSource.hh"
#endif
#include "TsBuffer.h"
#include "TsDuration.h"
#include <streams.h>    // CRefTime


class TsStreamFileSource: public FramedFileSource {
public:
	static TsStreamFileSource* createNew(UsageEnvironment& env,
		wchar_t const* fileName,
		unsigned preferredFrameSize = 0,
		unsigned playTimePerFrame = 0,
		int channelType = 0);
	// "preferredFrameSize" == 0 means 'no preference'
	// "playTimePerFrame" is in microseconds
	//	channelType determines the buffer size (for more reliable streaming)

	static TsStreamFileSource* createNew(UsageEnvironment& env,
		FILE* fid,
		Boolean deleteFidOnClose = False,
		unsigned preferredFrameSize = 0,
		unsigned playTimePerFrame = 0,
		int channelType = 0);
	// an alternative version of "createNew()" that's used if you already have
	// an open file.

	u_int64_t fileSize() const { return fFileSize; }
	// 0 means zero-length, unbounded, or unknown

	void seekToByteAbsolute(u_int64_t byteNumber);
	void seekToByteRelative(int64_t offset);
	void seekToTimeAbsolute(CRefTime& seekTime, CTsDuration& duration); 

protected:
	TsStreamFileSource(UsageEnvironment& env,
		FILE* fid, Boolean deleteFidOnClose,
		unsigned preferredFrameSize,
		unsigned playTimePerFrame,
		int channelType);
	// called only by createNew()

	virtual ~TsStreamFileSource();

private:
	// redefined virtual functions:
	virtual void doGetNextFrame();

private:
	CTSBuffer m_buffer;
	unsigned fPreferredFrameSize;
	unsigned fPlayTimePerFrame;
	unsigned fLastPlayTime;
	u_int64_t fFileSize;
	Boolean fDeleteFidOnClose;
	Boolean m_isTimeshift;
};