/* 
 *	Copyright (C) 2006-2018 Team MediaPortal
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
#pragma once

#include <windows.h>
#include <AtlBase.h>
#include <AtlConv.h>

#define MAX_REG_LENGTH 256

class CRegistryUtil
{
public:
	CRegistryUtil(void);
  void    ReadSettingsFromReg();
  void    ReadRegistryKeyDword(HKEY hKey, LPCTSTR& lpSubKey, DWORD& data);
  void    WriteRegistryKeyDword(HKEY hKey, LPCTSTR& lpSubKey, DWORD& data);
  void    ReadRegistryKeyString(HKEY hKey, LPCTSTR& lpSubKey, LPCTSTR& data);
  void    WriteRegistryKeyString(HKEY hKey, LPCTSTR& lpSubKey, LPCTSTR& data);
  LONG    ReadOnlyRegistryKeyDword(HKEY hKey, LPCTSTR& lpSubKey, DWORD& data);
  //Shared settings variables
  static bool m_bNoGeneralInGenre;
  static bool m_bPassThruISO6937;
public:
	virtual ~CRegistryUtil(void);
};
