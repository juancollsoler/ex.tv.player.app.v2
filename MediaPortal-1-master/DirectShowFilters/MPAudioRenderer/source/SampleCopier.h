// Copyright (C) 2005-2012 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#pragma once
#include "stdafx.h"
#include <MMReg.h>  //must be before other Wasapi headers
#include <ks.h>
#include <ksmedia.h>
#include "BaseAudioSink.h"
#include "Logger.h"

class CSampleCopier : public CBaseAudioSink
{
public:
  CSampleCopier(AudioRendererSettings* pSettings, Logger* pLogger);
  virtual ~CSampleCopier();

// type definitions


// IAudioSink implementation
public:
  // Initialization
  HRESULT Init();
  HRESULT Cleanup();

  // Format negotiation
  HRESULT NegotiateFormat(const WAVEFORMATEXTENSIBLE* pwfx, int nApplyChangesDepth, ChannelOrder* pChOrder);

  // Processing
  HRESULT PutSample(IMediaSample* pSample);
  HRESULT EndOfStream();

// Internal implementation
protected:

protected:
  bool m_bPassThrough;

  // Input buffer layout
  int m_nInFrameSize;       // Bytes in a frame. A frame contains a sample for each channel
  int m_nInBytesPerSample;  // Bytes in a sample. Can be 1 to 4. This is the "container" size
  int m_nInBitsPerSample;   // Valid bits in a sample. The valid bits are left aligned in the container for integer samples
                            //   For float samples this field should be 32
  bool m_bInFloatSamples;   // If true, each sample is a 32bit float, otherwise it is an integer (see above) 
  // Output buffer layout
  int m_nOutFrameSize;      // Bytes in a frame. A frame contains a sample for each channel
  int m_nOutBytesPerSample; // Bytes in a sample. Can be 1 to 4. This is the "container" size
  int m_nOutBitsPerSample;  // Valid bits in a sample. The valid bits are left aligned in the container for integer samples
                            //   For float samples this field should be 32
  bool m_bOutFloatSamples;  // If true, each sample is a 32bit float, otherwise it is an integer (see above) 

  REFERENCE_TIME m_rtNextIncomingSampleTime;
  REFERENCE_TIME m_rtInSampleTime; 

  Logger* m_pLogger;
};

