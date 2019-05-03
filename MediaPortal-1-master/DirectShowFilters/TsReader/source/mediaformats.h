/**
*  MediaFormats.h
*  Copyright (C) 2004-2006 bear
*
*  This file is part of TSFileSource, a directshow push source filter that
*  provides an MPEG transport stream output.
*
*  TSFileSource is free software; you can redistribute it and/or modify
*  it under the terms of the GNU General Public License as published by
*  the Free Software Foundation; either version 2 of the License, or
*  (at your option) any later version.
*
*  TSFileSource is distributed in the hope that it will be useful,
*  but WITHOUT ANY WARRANTY; without even the implied warranty of
*  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
*  GNU General Public License for more details.
*
*  You should have received a copy of the GNU General Public License
*  along with TSFileSource; if not, write to the Free Software
*  Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*
*  bear can be reached on the forums at
*    http://forums.dvbowners.com/
*/

// MPEG2VIDEOINFO

#ifndef MEDIAFORMATS_H
#define MEDIAFORMATS_H

static BYTE Mpeg2ProgramVideo [] = {
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcSource.left              = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcSource.top               = 0x00000000
	0xD0, 0x02, 0x00, 0x00,                         //  .hdr.rcSource.right             = 0x000002d0
	0xE0, 0x01, 0x00, 0x00,                         //  .hdr.rcSource.bottom            = 0x000001e0
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcTarget.left              = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcTarget.top               = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcTarget.right             = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcTarget.bottom            = 0x00000000
	0x00, 0x09, 0x3D, 0x00,                         //  .hdr.dwBitRate                  = 0x003d0900
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.dwBitErrorRate             = 0x00000000
	0x63, 0x17, 0x05, 0x00, 0x00, 0x00, 0x00, 0x00, //  .hdr.AvgTimePerFrame            = 0x0000000000051763
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.dwInterlaceFlags           = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.dwCopyProtectFlags         = 0x00000000
	0x04, 0x00, 0x00, 0x00,                         //  .hdr.dwPictAspectRatioX         = 0x00000004
	0x03, 0x00, 0x00, 0x00,                         //  .hdr.dwPictAspectRatioY         = 0x00000003
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.dwReserved1                = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.dwReserved2                = 0x00000000
	0x28, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biSize           = 0x00000028
	0xD0, 0x02, 0x00, 0x00,                         //  .hdr.bmiHeader.biWidth          = 0x000002d0
	0xE0, 0x01, 0x00, 0x00,                         //  .hdr.bmiHeader.biHeight         = 0x00000000
	0x00, 0x00,                                     //  .hdr.bmiHeader.biPlanes         = 0x0000
	0x00, 0x00,                                     //  .hdr.bmiHeader.biBitCount       = 0x0000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biCompression    = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biSizeImage      = 0x00000000
	0xD0, 0x07, 0x00, 0x00,                         //  .hdr.bmiHeader.biXPelsPerMeter  = 0x000007d0
	0x27, 0xCF, 0x00, 0x00,                         //  .hdr.bmiHeader.biYPelsPerMeter  = 0x0000cf27
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biClrUsed        = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biClrImportant   = 0x00000000
	0x98, 0xF4, 0x06, 0x00,                         //  .dwStartTimeCode                = 0x0006f498
	0x56, 0x00, 0x00, 0x00,                         //  .cbSequenceHeader               = 0x00000056
	0x02, 0x00, 0x00, 0x00,                         //  .dwProfile                      = 0x00000002
	0x02, 0x00, 0x00, 0x00,                         //  .dwLevel                        = 0x00000002
	0x00, 0x00, 0x00, 0x00,                         //  .Flags                          = 0x00000000
													//  .dwSequenceHeader [1]
	0x00, 0x00, 0x01, 0xB3, 0x2D, 0x01, 0xE0, 0x24,
	0x09, 0xC4, 0x23, 0x81, 0x10, 0x11, 0x11, 0x12,
	0x12, 0x12, 0x13, 0x13, 0x13, 0x13, 0x14, 0x14,
	0x14, 0x14, 0x14, 0x15, 0x15, 0x15, 0x15, 0x15,
	0x15, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16,
	0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17,
	0x18, 0x18, 0x18, 0x19, 0x18, 0x18, 0x18, 0x19,
	0x1A, 0x1A, 0x1A, 0x1A, 0x19, 0x1B, 0x1B, 0x1B,
	0x1B, 0x1B, 0x1C, 0x1C, 0x1C, 0x1C, 0x1E, 0x1E,
	0x1E, 0x1F, 0x1F, 0x21, 0x00, 0x00, 0x01, 0xB5,
	0x14, 0x82, 0x00, 0x01, 0x00, 0x00
};

static 
BYTE
g_Mpeg2ProgramVideo [] = {
	0x00, 0x00, 0x00, 0x00,							//  .hdr.rcSource.left
	0x00, 0x00, 0x00, 0x00,							//  .hdr.rcSource.top
	0xd0, 0x02, 0x00, 0x00,							//  .hdr.rcSource.right
	0x40, 0x02, 0x00, 0x00,							//  .hdr.rcSource.bottom
	0x00, 0x00, 0x00, 0x00,							//  .hdr.rcTarget.left
	0x00, 0x00, 0x00, 0x00,							//  .hdr.rcTarget.top
	0x00, 0x00, 0x00, 0x00,							//  .hdr.rcTarget.right
	0x00, 0x00, 0x00, 0x00,							//  .hdr.rcTarget.bottom
	0xc0, 0xe1, 0xe4, 0x00,							//  .hdr.dwBitRate
	0x00, 0x00, 0x00, 0x00,							//  .hdr.dwBitErrorRate
	0x80, 0x1a, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, //  .hdr.AvgTimePerFrame
	0x00, 0x00, 0x00, 0x00,							//  .hdr.dwInterlaceFlags
	0x00, 0x00, 0x00, 0x00,							//  .hdr.dwCopyProtectFlags
	0x00, 0x00, 0x00, 0x00,							//  .hdr.dwPictAspectRatioX
	0x00, 0x00, 0x00, 0x00,							//  .hdr.dwPictAspectRatioY
	0x00, 0x00, 0x00, 0x00,							//  .hdr.dwReserved1
	0x00, 0x00, 0x00, 0x00,							//  .hdr.dwReserved2
	0x28, 0x00, 0x00, 0x00,							//  .hdr.bmiHeader.biSize
	0xd0, 0x02, 0x00, 0x00,							//  .hdr.bmiHeader.biWidth
	0x40, 0x02, 0x00, 0x00,							//  .hdr.bmiHeader.biHeight
	0x00, 0x00,										//  .hdr.bmiHeader.biPlanes
	0x00, 0x00,										//  .hdr.bmiHeader.biBitCount
	0x00, 0x00, 0x00, 0x00,							//  .hdr.bmiHeader.biCompression
	0x00, 0x00, 0x00, 0x00,							//  .hdr.bmiHeader.biSizeImage
	0xd0, 0x07, 0x00, 0x00,							//  .hdr.bmiHeader.biXPelsPerMeter
	0x42, 0xd8, 0x00, 0x00,							//  .hdr.bmiHeader.biYPelsPerMeter
	0x00, 0x00, 0x00, 0x00,							//  .hdr.bmiHeader.biClrUsed
	0x00, 0x00, 0x00, 0x00,							//  .hdr.bmiHeader.biClrImportant
	0x00, 0x00, 0x00, 0x00,							//  .dwStartTimeCode
	0x4c, 0x00, 0x00, 0x00,							//  .cbSequenceHeader
	0x00, 0x00, 0x00, 0x00,							//  .dwProfile
	0x00, 0x00, 0x00, 0x00,							//  .dwLevel
	0x00, 0x00, 0x00, 0x00,							//  .Flags
													//  .dwSequenceHeader [1]
	0x00, 0x00, 0x01, 0xb3, 0x2d, 0x02, 0x40, 0x33, 
	0x24, 0x9f, 0x23, 0x81, 0x10, 0x11, 0x11, 0x12, 
	0x12, 0x12, 0x13, 0x13, 0x13, 0x13, 0x14, 0x14, 
	0x14, 0x14, 0x14, 0x15, 0x15, 0x15, 0x15, 0x15, 
	0x15, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 0x16, 
	0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 0x17, 
	0x18, 0x18, 0x18, 0x19, 0x18, 0x18, 0x18, 0x19, 
	0x1a, 0x1a, 0x1a, 0x1a, 0x19, 0x1b, 0x1b, 0x1b, 
	0x1b, 0x1b, 0x1c, 0x1c, 0x1c, 0x1c, 0x1e, 0x1e, 
	0x1e, 0x1f, 0x1f, 0x21, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
};

// VIDEOINFOHEADER
static BYTE H264VideoFormat [] = {
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcSource.left              = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcSource.top               = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcSource.right             = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcSource.bottom            = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcTarget.left              = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcTarget.top               = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcTarget.right             = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.rcTarget.bottom            = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.dwBitRate                  = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.dwBitErrorRate             = 0x00000000
//	0x80, 0x1a, 0x06, 0x00, 0x00, 0x00, 0x00, 0x00, //  .hdr.AvgTimePerFrame            = 0x0000000000061a80
	0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, //  .hdr.AvgTimePerFrame            = 0x0000000000061a80
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biSize           = 0x00000028
	0xD0, 0x02, 0x00, 0x00,                         //  .hdr.bmiHeader.biWidth          = 0x000002d0
	0x40, 0x02, 0x00, 0x00,                         //  .hdr.bmiHeader.biHeight         = 0x00000240
	0x00, 0x00,                                     //  .hdr.bmiHeader.biPlanes         = 0x0001
	0x00, 0x00,                                     //  .hdr.bmiHeader.biBitCount       = 0x0018
	0x68, 0x32, 0x36, 0x34,                         //  .hdr.bmiHeader.biCompression    = "h264"
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biSizeImage      = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biXPelsPerMeter  = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biYPelsPerMeter  = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biClrUsed        = 0x00000000
	0x00, 0x00, 0x00, 0x00,                         //  .hdr.bmiHeader.biClrImportant   = 0x00000000
};

static BYTE MPEG1AudioFormat [] = {
	0x50, 0x00,				//wFormatTag
	0x02, 0x00,				//nChannels
	0x80, 0xBB,	0x00, 0x00, //nSamplesPerSec
	0x00, 0x7D,	0x00, 0x00, //nAvgBytesPerSec
	0x00, 0x03,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x16, 0x00,				//cbSize
	0x02, 0x00,				//wValidBitsPerSample
	0x00, 0xE8,				//wSamplesPerBlock
	0x03, 0x00,				//wReserved
	0x01, 0x00,	0x01,0x00,  //dwChannelMask
	0x01, 0x00,	0x1C, 0x00, 0x00, 0x00,	0x00, 0x00, 0x00, 0x00, 0x00, 0x00
} ;

static BYTE MPEG2AudioFormat [] = {
	0x50, 0x00,				//wFormatTag
	0x02, 0x00,				//nChannels
	0x80, 0xbb, 0x00, 0x00, //nSamplesPerSec
	0x00, 0x7d, 0x00, 0x00, //nAvgBytesPerSec
	0x01, 0x00,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x16, 0x00,				//cbSize
	0x02, 0x00,				//wValidBitsPerSample
	0x00, 0xe8,				//wSamplesPerBlock
	0x03, 0x00,				//wReserved
	0x01, 0x00, 0x01, 0x00, //dwChannelMask
	0x01, 0x00, 0x16, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00
};

static BYTE AC3AudioFormat [] = {
	0x00, 0x20,				//wFormatTag
	0x06, 0x00,				//nChannels
	0x80, 0xBB, 0x00, 0x00, //nSamplesPerSec
	0xC0, 0x5D, 0x00, 0x00, //nAvgBytesPerSec
	0x00, 0x03,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x00, 0x00				//cbSize
};

static BYTE AACRawAudioFormat2 [] = {
	0xFF, 0x00,				//wFormatTag
	0x02, 0x00,				//nChannels
	0x80, 0xBB, 0x00, 0x00, //nSamplesPerSec
	0x00, 0x00, 0x00, 0x00, //nAvgBytesPerSec
	0x01, 0x00,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x02, 0x00,				//cbSize
	0x11, 0x90        //AAC-LC, 48kHz, 2 channels
};

static BYTE AACRawAudioFormat6 [] = {
	0xFF, 0x00,				//wFormatTag
	0x06, 0x00,				//nChannels
	0x80, 0xBB, 0x00, 0x00, //nSamplesPerSec
	0x00, 0x00, 0x00, 0x00, //nAvgBytesPerSec
	0x01, 0x00,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x02, 0x00,				//cbSize
	0x11, 0xB0        //AAC-LC, 48kHz, 6 channels
};

static BYTE AACLatmAudioFormat [] = {
	0xFF, 0x01,				//wFormatTag
	0x02, 0x00,				//nChannels
	0x80, 0xBB, 0x00, 0x00, //nSamplesPerSec
	0x00, 0x00, 0x00, 0x00, //nAvgBytesPerSec
	0x01, 0x00,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x00, 0x00				//cbSize
};

static BYTE AACAdtsAudioFormat [] = {
	0x00, 0x16,				//wFormatTag
	0x02, 0x00,				//nChannels
	0x80, 0xBB, 0x00, 0x00, //nSamplesPerSec
	0x00, 0x00, 0x00, 0x00, //nAvgBytesPerSec
	0x01, 0x00,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x00, 0x00				//cbSize
};

static BYTE AACLoasAudioFormat [] = {
	0x02, 0x16,				//wFormatTag
	0x02, 0x00,				//nChannels
	0x80, 0xBB, 0x00, 0x00, //nSamplesPerSec
	0x00, 0x00, 0x00, 0x00, //nAvgBytesPerSec
	0x01, 0x00,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x00, 0x00				//cbSize
};

static BYTE DTSAudioFormat [] = {
	0x01, 0x20,				//wFormatTag
	0x06, 0x00,				//nChannels
	0x80, 0xBB, 0x00, 0x00, //nSamplesPerSec
	0x34, 0x18, 0x00, 0x00, //nAvgBytesPerSec
	0x18, 0x00,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x00, 0x00				//cbSize
};

static BYTE DTSHDAudioFormat [] = {
	0x01, 0x20,				//wFormatTag
	0x06, 0x00,				//nChannels
	0x80, 0xBB, 0x00, 0x00, //nSamplesPerSec
	0x34, 0x18, 0x00, 0x00, //nAvgBytesPerSec
	0x18, 0x00,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x00, 0x00				//cbSize
};

static BYTE LPCMAudioFormat [] = {
	0x48, 0x44,				//wFormatTag
	0x02, 0x00,				//nChannels
	0x80, 0xBB, 0x00, 0x00, //nSamplesPerSec
	0x00, 0x65, 0x04, 0x00, //nAvgBytesPerSec
	0x08, 0x00,				//nBlockAlign
	0x00, 0x00,				//wBitsPerSample
	0x01, 0x00,				//cbSize
	0x03              //2 channels
};
// {000000FF-0000-0010-8000-00AA00389B71}

struct WAVEFORMATEX_HDMV_LPCM : public WAVEFORMATEX
{
    BYTE channel_conf;

	struct WAVEFORMATEX_HDMV_LPCM()
	{
		memset(this, 0, sizeof(*this)); 
		cbSize = sizeof(WAVEFORMATEX_HDMV_LPCM) - sizeof(WAVEFORMATEX);
	}
};

struct WAVEFORMATEXPS2 : public WAVEFORMATEX
{
    DWORD dwInterleave;

	struct WAVEFORMATEXPS2()
	{
		memset(this, 0, sizeof(*this)); 
		cbSize = sizeof(WAVEFORMATEXPS2) - sizeof(WAVEFORMATEX);
	}
};

#pragma pack(push, 1)
typedef struct {
	DWORD dwOffset;	
	CHAR IsoLang[4]; // three letter lang code + terminating zero
	WCHAR TrackName[256]; // 256 chars ought to be enough for everyone :)
} SUBTITLEINFO;
#pragma pack(pop)

#define WAVE_FORMAT_MP3 0x0055
#define WAVE_FORMAT_AAC 0x00FF
#define WAVE_FORMAT_DOLBY_AC3 0x2000
#define WAVE_FORMAT_DVD_DTS 0x2001
#define WAVE_FORMAT_PS2_PCM 0xF521
#define WAVE_FORMAT_PS2_ADPCM 0xF522

// {A23EB7FC-510B-466F-9FBF-5F878F69347C}   LAV specific
static GUID MEDIASUBTYPE_BD_LPCM_AUDIO   = {0xa23eb7fc, 0x510b, 0x466f, 0x9f, 0xbf, 0x5f, 0x87, 0x8f, 0x69, 0x34, 0x7c};

// {949F97FD-56F6-4527-B4AE-DDEB375AB80F}		Mpc-hc specific   
static GUID MEDIASUBTYPE_HDMV_LPCM_AUDIO = {0x949f97fd, 0x56f6, 0x4527, 0xb4, 0xae, 0xdd, 0xeb, 0x37, 0x5a, 0xb8, 0xf};

static GUID MEDIASUBTYPE_HDMVSUB         = {0x4eba53e, 0x9330, 0x436c, 0x91, 0x33, 0x55, 0x3e, 0xc8, 0x70, 0x31, 0xdc};
static GUID MEDIASUBTYPE_SVCD_SUBPICTURE = {0xda5b82ee, 0x6bd2, 0x426f, 0xbf, 0x1e, 0x30, 0x11, 0x2d, 0xa7, 0x8a, 0xe1};
static GUID MEDIASUBTYPE_CVD_SUBPICTURE  = {0x7b57308f, 0x5154, 0x4c36, 0xb9, 0x3, 0x52, 0xfe, 0x76, 0xe1, 0x84, 0xfc};
static GUID MEDIATYPE_Subtitle           = {0xe487eb08, 0x6b26, 0x4be9, 0x9d, 0xd3, 0x99, 0x34, 0x34, 0xd3, 0x13, 0xfd};
static GUID MEDIASUBTYPE_PS2_SUB         = {0x4f3d3d21, 0x6d7c, 0x4f73, 0xaa, 0x5, 0xe3, 0x97, 0xb5, 0xea, 0xe0, 0xaa};

static GUID H264_SubType                 = {0x8D2D71CB, 0x243F, 0x45E3, {0xB2, 0xD8, 0x5F, 0xD7, 0x96, 0x7E, 0xC0, 0x9B}};
static GUID MPG4_SubType                 = FOURCCMap(MAKEFOURCC('A','V','C','1')) ;

static GUID MEDIASUBTYPE_AAC             = {0x000000ff, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71};
static GUID MEDIASUBTYPE_LATM_AAC        = {0x000001ff, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71};
//static GUID MEDIASUBTYPE_MPEG_ADTS_AAC   = {0x00001600, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xaa, 0x00, 0x38, 0x9b, 0x71};

// 43564548-0000-0010-8000-00AA00389B71
static GUID MEDIASUBTYPE_HEVC            = {0x43564548, 0x0000, 0x0010, 0x80, 0x00, 0x00, 0xAA, 0x00, 0x38, 0x9B, 0x71};


static inline unsigned short make_aac_config(DWORD srate, WORD chans)
{
	// Create two-byte AAC AudioSpecificConfig

	unsigned short asc = 0;

	// AudioSpecificConfig, 2 bytes

	//   LSB      MSB
	// xxxxx... ........  object type (00010 = AAC LC)
	// .....xxx x.......  sampling frequency index (0 to 11)
	// ........ .xxxx...  channels configuration (1 to 7)
	// ........ .....x..  frame length flag (0 = 1024, 1 = 960)
	// ........ ......x.  depends on core coder (0)
	// ........ .......x  extensions flag (0)
		
	//   LSB      MSB
	// .....000 0....... = 96000
	// .....000 1....... = 88200
	// .....001 0....... = 64000
	// .....001 1....... = 48000
	// .....010 0....... = 44100
	// .....010 1....... = 32000
	// .....011 0....... = 24000
	// .....011 1....... = 22050
	// .....100 0....... = 16000
	// .....100 1....... = 12000
	// .....101 0....... = 11025
	// .....101 1....... =  8000

	switch (srate) {
	case 96000: break;
	case 88200: asc |= 0x8000; break;
	case 64000: asc |= 0x0001; break;
	case 48000: asc |= 0x8001; break;
	case 44100: asc |= 0x0002; break;
	case 32000: asc |= 0x8002; break;
	case 24000: asc |= 0x0003; break;
	case 22050: asc |= 0x8003; break;
	case 16000: asc |= 0x0004; break;
	case 12000: asc |= 0x8004; break;
	case 11025: asc |= 0x0005; break;
	case  8000: asc |= 0x8005; break;
	default:
		return 0;
	}

	//   LSB      MSB
	// ........ .0001... = 1 channel
	// ........ .0010... = 2 channels
	// ........ .0011... = 3 channels
	// ........ .0100... = 4 channels
	// ........ .0101... = 5 channels
	// ........ .0110... = 6 channels
	// ........ .0111... = 8 channels

	switch (chans) {
	case 1: asc |= 0x0800; break;
	case 2: asc |= 0x1000; break;
	case 3: asc |= 0x1800; break;
	case 4: asc |= 0x2000; break;
	case 5: asc |= 0x2800; break;
	case 6: asc |= 0x3000; break;
	case 8: asc |= 0x3800; break;
	default:
		return 0;
	}

	return (asc | 0x0010);
}

#endif