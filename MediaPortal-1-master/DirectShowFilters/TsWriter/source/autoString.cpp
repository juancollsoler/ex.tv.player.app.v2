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
#include <stdio.h>
#include <windows.h>
#include "autostring.h"
CAutoString::CAutoString(int len)
{
	m_pBuffer = new char[len];
	if (m_pBuffer)
	{  
    memset(m_pBuffer,0,len);
    m_Len = len;
  }
}
CAutoString::~CAutoString()
{
	if (m_pBuffer)
	{  
  	delete [] m_pBuffer;
  	m_pBuffer=NULL;
  }
}

char* CAutoString::GetBuffer() 
{
	return m_pBuffer;
}

char* CAutoString::GetBuffer(int newLength)
{
  if (newLength>0 && newLength<m_Len)
  {    
    //Copy data into new, smaller buffer
    char* pNewString = new char[newLength];
    if (pNewString)
    {
      memcpy(pNewString, m_pBuffer, newLength);
      delete[] m_pBuffer;
      m_pBuffer = pNewString;
    }
  }
  return m_pBuffer;
}

