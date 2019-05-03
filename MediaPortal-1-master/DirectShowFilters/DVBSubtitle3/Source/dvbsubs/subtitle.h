/* 
 *	Copyright (C) 2006-2009 Team MediaPortal
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

#ifndef _BITMAP_H
#define _BITMAP_H
#define _AFXDLL
#include <windows.h>
#include <WinDef.h>

typedef unsigned __int64 uint64_t;
typedef unsigned __int16 uint16_t;
typedef unsigned __int8 uint8_t;

class CSubtitle
{
public:

  CSubtitle( int subW, int subH, int screenW, int screenH );
  
  ~CSubtitle();
  BITMAP m_bitmap;
  BITMAP* GetBitmap();

  int RenderBitmap( unsigned char* buffer, unsigned char* my_palette, 
    unsigned char* my_trans, int col_count );
  void DrawRect( SHORT nX, SHORT nY, SHORT nCount, int R, int G, int B, 
    int transparent );

  int Width();
  int Height();
  int ScreenWidth();
  int ScreenHeight();
  uint64_t PTS();
  void SetPTS( uint64_t PTS );
  uint64_t Timestamp();
  void SetTimestamp( uint64_t PTS );
  uint64_t Timeout();
  void SetTimeout( uint64_t timeout );
  void SetFirstScanline( int );
  int FirstScanline();
  void SetHorizontalPosition( int x );
  int HorizontalPosition();

private:

  unsigned char* m_Data;
  int m_firstScanline;
  uint64_t m_PTS;
  uint64_t m_timestamp;
  uint64_t m_timeout;
  int m_screenWidth;
  int m_screenHeight;
  int m_horizontalPosition;
};
#endif
