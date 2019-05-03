/* 
 *	Copyright (C) 2006-2013 Team MediaPortal
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

#include "StdAfx.h"

#include <atlbase.h>
#include "PidTable.h"

// For more details for memory leak detection see the alloctracing.h header
#include "..\..\alloctracing.h"

void LogDebug(const char *fmt, ...) ; 


CPidTable::CPidTable(const CPidTable& pids)
{
  Copy(pids);
}


CPidTable::CPidTable(void)
{
  Reset();
}


CPidTable::~CPidTable(void)
{
}


bool CPidTable::operator ==(const CPidTable& other) const
{
  // Not all members are compared, this is how DeMultiplexer class has 
  // been comparing "PMTs" to detect channel changes.
  if(subtitlePids != other.subtitlePids
    || audioPids != other.audioPids 
    || videoPids != other.videoPids
    || PcrPid != other.PcrPid
    || PmtPid != other.PmtPid)
  {
    return false;
  }
  else
  {
    return true;
  }
}


void CPidTable::Reset()
{
	//LogDebug("Pid table reset");
  PcrPid=0;
  PmtPid=0;
  ServiceId=-1;
  
  videoPids.clear();
  audioPids.clear();
  subtitlePids.clear();

  TeletextPid=0;
  // no reason to reset TeletextSubLang
}


CPidTable& CPidTable::operator = (const CPidTable &pids)
{
  if (&pids==this)
  {
    return *this;
  }
  Copy(pids);
  return *this;
}


void CPidTable::Copy(const CPidTable &pids)
{
  //LogDebug("Pid table copy");
  ServiceId=pids.ServiceId;

  PcrPid=pids.PcrPid;
  PmtPid=pids.PmtPid;

  videoPids=pids.videoPids;
  audioPids=pids.audioPids;
  subtitlePids=pids.subtitlePids;

  TeletextPid=pids.TeletextPid;
  TeletextInfo=pids.TeletextInfo;
}


bool CPidTable::HasTeletextPageInfo(int page)
{
	std::vector<TeletextServiceInfo>::iterator vit = TeletextInfo.begin();
	while(vit != TeletextInfo.end())
  { // is the page already registrered
		TeletextServiceInfo& info = *vit;
		if(info.page == page)
    {
			return true;
			break;
		}
		else vit++;
	}
	return false;
}

void CPidTable::LogPIDs()
{
  USES_CONVERSION;

  LogDebug(" pcr      pid: 0x%04x ",PcrPid);
  LogDebug(" pmt      pid: 0x%04x ",PmtPid);

  // Log all video streams (Blu-ray can have multiple video streams)
  for(unsigned int i(0) ; i < videoPids.size() ; i++)
  {
    LogDebug(" Video    pid: 0x%04x type: %s (0x%02x) DescriptorData: 0x%02x",
      videoPids[i].Pid, 
      T2A(StreamFormatAsString(videoPids[i].VideoServiceType)),
      videoPids[i].VideoServiceType,
      videoPids[i].DescriptorData);
  }

  // Log all audio streams
  for(unsigned int i(0) ; i < audioPids.size() ; i++)
  {
	  LogDebug(" Audio    pid: 0x%04x type: %s (0x%02x) language: %3s",
      audioPids[i].Pid, 
      T2A(StreamFormatAsString(audioPids[i].AudioServiceType)),
      audioPids[i].AudioServiceType,
      audioPids[i].Lang);
  }
  
  // Log all subtitle streams
  for(unsigned int i(0) ; i < subtitlePids.size() ; i++)
  {
	  LogDebug(" Subtitle pid: 0x%04x type: %s (0x%02x) language: %3s",
      subtitlePids[i].Pid, 
      T2A(StreamFormatAsString(subtitlePids[i].SubtitleServiceType)),
      subtitlePids[i].SubtitleServiceType, 
      subtitlePids[i].Lang); 
  }  
  
  // Log all teletext subtitle streams
  for(unsigned int i(0) ; i < TeletextInfo.size() ; i++)
  {
	  LogDebug(" Teletext pid: 0x%04x type: %s (0x%02x) language: %3s",
      TeletextPid, 
      T2A(TeletextTypeAsString(TeletextInfo[i].type)),
      TeletextInfo[i].type, 
      TeletextInfo[i].lang); 
  }  
}


LPCTSTR CPidTable::StreamFormatAsString(int streamType)
{
	switch (streamType)
	{
	case 0x01:
		return _T("MPEG1");
	case 0x02:
		return _T("MPEG2");
	case 0x03:
		return _T("MPEG1 - audio");
	case 0x04:
		return _T("MPEG2 - audio");
	case 0x05:
		return _T("DVB subtitle 1");
	case 0x06:
		return _T("DVB subtitle 2");
	case 0x10:
		return _T("MPEG4");
	case 0x1B:
		return _T("H264");
	case 0x24:
		return _T("HEVC");
	case 0xEA:
		return _T("VC1");
	case 0x80:
		return _T("LPCM");
	case 0x81:
		return _T("AC3");
	case 0x82:
		return _T("DTS");
	case 0x83:
		return _T("MLP");
	case 0x84:
		return _T("DD+");
	case 0x85:
		return _T("DTS-HD");
	case 0x86:
		return _T("DTS-HD Master Audio");
	case 0x87:
		return _T("DD+");
  case 0x0f:
		return _T("ADTS AAC");
	case 0x11:
		return _T("LATM AAC");
  case 0xA1:
		return _T("DD+");
	case 0xA2:
		return _T("DTS-HD");
	case 0x90:
		return _T("PGS");
	case 0x91:
		return _T("IG");
	case 0x92:
		return _T("Text");
	default:
		return _T("Unknown");
	}
}

LPCTSTR CPidTable::TeletextTypeAsString(int streamType)
{
	switch (streamType)
	{
	case 0x01:
		return _T("Teletext - initial");
	case 0x02:
		return _T("Teletext - subtitles");
	case 0x03:
		return _T("Teletext - additional information");
	case 0x04:
		return _T("Teletext - programme schedule");
	case 0x05:
		return _T("Teletext - subtitles for hearing impaired");
	default:
		return _T("Unknown");
	}
}
