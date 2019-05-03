/*
 *  Copyright (C) 2005-2013 Team MediaPortal
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


#include "StdAfx.h"
#include "TsAVRT.h"

// For more details for memory leak detection see the alloctracing.h header
#include "..\..\alloctracing.h"

extern void LogDebug(const char *fmt, ...) ;
extern void StopLogger();


TsAVRT::TsAVRT()
{
  m_hModuleAVRT = NULL;
  
  m_pAvSetMmThreadCharacteristicsW = NULL;
  m_pAvSetMmThreadPriority = NULL;
  m_pAvRevertMmThreadCharacteristics = NULL;
  
  LoadAVRT();
}

TsAVRT::~TsAVRT()
{
  m_pAvSetMmThreadCharacteristicsW = NULL;
  m_pAvSetMmThreadPriority = NULL;
  m_pAvRevertMmThreadCharacteristics = NULL;
  
  if (m_hModuleAVRT)
  {
    if (!FreeLibrary(m_hModuleAVRT))
    {
      LogDebug("TsAVRT::dtor - avrt.dll could not be unloaded");
      StopLogger(); //Needed since logging thread might be re-started after CDeMultiplexer() destructor has called StopLogger()
    }
  }
  
  m_hModuleAVRT = NULL;
}

bool TsAVRT::LoadAVRT()
{
  LogDebug("TsAVRT::LoadAVRT() - Loading AVRT libraries");
  TCHAR systemFolder[MAX_PATH];
  TCHAR DLLFileName[MAX_PATH];
  GetSystemDirectory(systemFolder,sizeof(systemFolder));
  
  _stprintf_s(DLLFileName, MAX_PATH, _T("%s\\avrt.dll"), systemFolder);
  m_hModuleAVRT = LoadLibrary(DLLFileName);
  // Vista and later OS only, allowed to return NULL. Remember to check against NULL when using
  if (m_hModuleAVRT)
  {
    //LogDebug("TsAVRT::LoadAVRT() - Successfully loaded AVRT dll");
    m_pAvSetMmThreadCharacteristicsW   = (TAvSetMmThreadCharacteristicsW*)GetProcAddress(m_hModuleAVRT,"AvSetMmThreadCharacteristicsW");
    m_pAvSetMmThreadPriority           = (TAvSetMmThreadPriority*)GetProcAddress(m_hModuleAVRT,"AvSetMmThreadPriority");
    m_pAvRevertMmThreadCharacteristics = (TAvRevertMmThreadCharacteristics*)GetProcAddress(m_hModuleAVRT,"AvRevertMmThreadCharacteristics");
    
    if (m_pAvSetMmThreadCharacteristicsW && m_pAvSetMmThreadPriority && m_pAvRevertMmThreadCharacteristics)
    {
      return TRUE;
    }
  }

  m_pAvSetMmThreadCharacteristicsW = NULL;
  m_pAvSetMmThreadPriority = NULL;
  m_pAvRevertMmThreadCharacteristics = NULL;

  if (m_hModuleAVRT)
  {
    if (!FreeLibrary(m_hModuleAVRT))
    {
      LogDebug("TsAVRT::LoadAVRT() - avrt.dll could not be unloaded");
    }
  }
  
  m_hModuleAVRT = NULL;

  LogDebug("TsAVRT::LoadAVRT() - Failed to load avrt.dll (not supported on Win XP)");
  return FALSE;
} 

HANDLE TsAVRT::SetMMCSThreadPlayback(LPDWORD pDwTaskIndex, AVRT_PRIORITY AvrtPriority)
{  
  HANDLE hAvrt = INVALID_HANDLE_VALUE;
  
  if (m_pAvSetMmThreadCharacteristicsW) 
  {
    hAvrt = m_pAvSetMmThreadCharacteristicsW(L"Playback", pDwTaskIndex);
    
    if ((hAvrt != INVALID_HANDLE_VALUE) && m_pAvSetMmThreadPriority) 
    {
      if (m_pAvSetMmThreadPriority(hAvrt, AVRT_PRIORITY_NORMAL))
      {
        LogDebug("TsAVRT::SetMMCSThread - Priority: %d, AvrtHandle: %d", AVRT_PRIORITY_NORMAL, hAvrt);
      }
    }
  }
  
  return hAvrt;
}

void TsAVRT::RevertMMCSThread(HANDLE hAvrt)
{
  if ((hAvrt != INVALID_HANDLE_VALUE) && m_pAvRevertMmThreadCharacteristics) 
  {
    if (m_pAvRevertMmThreadCharacteristics(hAvrt))
    {
      LogDebug("TsAVRT::RevertMMCSThread - AvrtHandle: %d", hAvrt);
    }
  } 
}

  
