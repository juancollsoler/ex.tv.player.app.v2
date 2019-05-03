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
// EnterCriticalSection.h: interface for the CEnterCriticalSection class.
//
//////////////////////////////////////////////////////////////////////

#if !defined(AFX_ENTERCRITICALSECTION_H__AFDC94FA_28D1_47FA_BB4F_F2F852C1B660__INCLUDED_)
#define AFX_ENTERCRITICALSECTION_H__AFDC94FA_28D1_47FA_BB4F_F2F852C1B660__INCLUDED_

#if _MSC_VER > 1000
#pragma once
#endif // _MSC_VER > 1000


//#ifndef _WINDOWS_
//// Minimum system required: Windows NT 4.0
//#define _WIN32_WINNT  0x0400 
//#define WINVER        0x0400
//#include <Windows.h>
//#endif /* _WINDOWS_ */

#if !defined(AFX_CRITICALSECTION_H__3B3A15BD_92D5_4044_8D69_5E1B8F15F369__INCLUDED_)
#include "CriticalSection.h"
#endif // !defined(AFX_CRITICALSECTION_H__3B3A15BD_92D5_4044_8D69_5E1B8F15F369__INCLUDED_)


namespace Mediaportal
{
  // Use to enter a critical section
  // Can only be used in blocking fashion
  // Critical section ends with object scope
  class CEnterCriticalSection
  {
  public:
    // Constructor
    // Obtain ownership of the cricital section
	  CEnterCriticalSection(CCriticalSection& cs);
    // Constructor
    // Obtain ownership of the cricital section
    // The const attribute will be removed with the const_cast operator
    // This enables the use of critical sections in const members
    CEnterCriticalSection(const CCriticalSection& cs);
    // Destructor
    // Leaves the critical section
	  virtual ~CEnterCriticalSection();

    // Test critical section ownership
    // Returns true if ownership was granted
    bool IsOwner() const;
    // Obtain ownership (may block)
    // Returns true when ownership was granted
    bool Enter();
    // Leave the critical section
    void Leave();

  private:
    CEnterCriticalSection(const CEnterCriticalSection& src);
    CEnterCriticalSection& operator=(const CEnterCriticalSection& src);

    // Reference to critical section object
    CCriticalSection&   m_cs;
    // Ownership flag
    bool                m_bIsOwner;
  };
}

#endif // !defined(AFX_ENTERCRITICALSECTION_H__AFDC94FA_28D1_47FA_BB4F_F2F852C1B660__INCLUDED_)
