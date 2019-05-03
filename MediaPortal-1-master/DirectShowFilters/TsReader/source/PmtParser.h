/* 
 *  Copyright (C) 2006-2013 Team MediaPortal
 *  http://www.team-mediaportal.com
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
#pragma once
#include "..\..\shared\sectiondecoder.h"
#include "..\..\shared\tsheader.h"
#include "pidtable.h"
#include <map>
using namespace std;

#define SERVICE_TYPE_VIDEO_UNKNOWN  -1
#define SERVICE_TYPE_VIDEO_MPEG1    0x1
#define SERVICE_TYPE_VIDEO_MPEG2    0x2
#define SERVICE_TYPE_PRIVATE_DATA   0x6
#define SERVICE_TYPE_DCII_OR_LPCM   0x80 // can be DC-II MPEG2 Video OR LPCM Audio if registration descriptor=HDMV
#define SERVICE_TYPE_VIDEO_MPEG4    0x10
#define SERVICE_TYPE_VIDEO_H264     0x1b
#define SERVICE_TYPE_VIDEO_HEVC     0x24
#define SERVICE_TYPE_VIDEO_SHVC     0x27 //SHVC extension to HEVC
#define SERVICE_TYPE_AUDIO_UNKNOWN  -1
#define SERVICE_TYPE_AUDIO_MPEG1    0x3
#define SERVICE_TYPE_AUDIO_MPEG2    0x4
#define SERVICE_TYPE_AUDIO_AC3      0x81 //ATSC AC3 stream
#define SERVICE_TYPE_AUDIO_AAC      0x0f
#define SERVICE_TYPE_AUDIO_LATM_AAC 0x11 //LATM AAC audio
#define SERVICE_TYPE_AUDIO_DD_PLUS  0x84 
#define SERVICE_TYPE_AUDIO_E_AC3    0x87 //ATSC E-AC3 stream
#define SERVICE_TYPE_AUDIO_DTS      0x82
#define SERVICE_TYPE_AUDIO_DTS_HD   0x85
#define SERVICE_TYPE_AUDIO_DTS_HDMA 0x86 

#define SERVICE_TYPE_DVB_SUBTITLES1 0x5
#define SERVICE_TYPE_DVB_SUBTITLES2 0x6

#define DESCRIPTOR_REGISTRATION     0x05
#define DESCRIPTOR_VBI_TELETEXT     0x46
#define DESCRIPTOR_DVB_AC3          0x6a
#define DESCRIPTOR_DVB_E_AC3        0x7a
#define DESCRIPTOR_DVB_TELETEXT     0x56
#define DESCRIPTOR_DVB_SUBTITLING   0x59
#define DESCRIPTOR_MPEG_ISO639_Lang 0x0a
#define DESCRIPTOR_VIDEO_STREAM     0x02   
#define DESCRIPTOR_AUDIO_STREAM     0x03   
#define DESCRIPTOR_AVC_VIDEO        0x28   
#define DESCRIPTOR_HEVC_VIDEO       0x38   
#define DESCRIPTOR_DVB_DTS          0x7b

//TsReader/MP player video stream IDs
#define VIDEO_STREAM_TYPE_MPEG2 		1
#define VIDEO_STREAM_TYPE_H264 			2
#define VIDEO_STREAM_TYPE_HEVC 			3


class IPmtCallBack
{
public:
  virtual void OnPmtReceived(int pmtPid)=0;
  virtual void OnPidsReceived(const CPidTable& info)=0;
};

class CPmtParser: public  CSectionDecoder
{
public:
  CPmtParser(void);
  virtual     ~CPmtParser(void);
  void        OnNewSection(CSection& section);
  void        SetPmtCallBack(IPmtCallBack* callback);
  bool        IsReady();
  void        ClearReady();
  CPidTable&  GetPidInfo();

private:
  int           m_pmtPid;
  bool          m_isFound;
  IPmtCallBack* m_pmtCallback;
  CTsHeader     m_tsHeader;
  CPidTable     m_pidInfo;  
};
