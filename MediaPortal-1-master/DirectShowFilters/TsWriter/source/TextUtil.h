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
#pragma once

#include <windows.h>
#include <string>
#include "RegistryUtil.h"

using namespace std;

class CTextUtil
{
public:
	CTextUtil(void);
  int DvbTextToString(BYTE *buf, int bufLen, char *text, int textLen, bool isBbcHuffman);
  string hexStr(const string& in);
	// string UTF8toISO8859_1(const string& in);
public:
	virtual ~CTextUtil(void);
private:
  int OneThreeCopy(BYTE *buf, int bufLen, char *text, int textLen);
  int UTF8toUTF8(BYTE *buf, int bufLen, char *text, int textLen);
  int ISO10646toUTF8(BYTE *buf, int bufLen, char *text, int textLen);
  int ISO6937toUTF8(BYTE *buf, int bufLen, char *text, int textLen);
  int BbcHuffmanToString(BYTE *buf, int bufLen, char *text, int textLen);
  static const BYTE bbc_huffman_data1[];
  static const BYTE bbc_huffman_data2[];
};

