/* 
 *	Copyright (C) 2006 Team MediaPortal
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
#ifndef FRAMEHEADERPARSER_H
#define FRAMEHEADERPARSER_H

#include "StdAfx.h"

#include <strmif.h>
//#include <mtype.h>
#include "GolombBuffer.h"
#include "PmtParser.h"
#include "HEVC\HevcNalDecode.h"

//H264 NAL type numbers
#define  H264_NAL_SLICE     1
#define  H264_NAL_DPA       2
#define  H264_NAL_DPB       3
#define  H264_NAL_DPC       4
#define  H264_NAL_IDR       5
#define  H264_NAL_SEI       6
#define  H264_NAL_SPS       7
#define  H264_NAL_PPS       8
#define  H264_NAL_AUD       9
#define  H264_NAL_EOSEQ     10
#define  H264_NAL_EOSTREAM  11
#define  H264_NAL_FILL      12

//HEVC NAL type numbers
#define  HEVC_NAL_TRAIL_N       0
#define  HEVC_NAL_TRAIL_R       1
#define  HEVC_NAL_TSA_N         2
#define  HEVC_NAL_TSA_R         3
#define  HEVC_NAL_STSA_N        4
#define  HEVC_NAL_STSA_R        5
#define  HEVC_NAL_RADL_N        6
#define  HEVC_NAL_RADL_R        7
#define  HEVC_NAL_RASL_N        8
#define  HEVC_NAL_RASL_R        9
#define  HEVC_NAL_BLA_W_LP     16
#define  HEVC_NAL_BLA_W_RADL   17
#define  HEVC_NAL_BLA_N_LP     18
#define  HEVC_NAL_IDR_W_RADL   19
#define  HEVC_NAL_IDR_N_LP     20
#define  HEVC_NAL_CRA_NUT      21
#define  HEVC_NAL_IRAP_VCL23   23 
#define  HEVC_NAL_VPS          32
#define  HEVC_NAL_SPS          33
#define  HEVC_NAL_PPS          34
#define  HEVC_NAL_AUD          35
#define  HEVC_NAL_EOS_NUT      36
#define  HEVC_NAL_EOB_NUT      37
#define  HEVC_NAL_FD_NUT       38
#define  HEVC_NAL_SEI_PREFIX   39
#define  HEVC_NAL_SEI_SUFFIX   40
#define  HEVC_NAL_RESERVED    255


enum mpeg_t {mpegunk, mpeg1, mpeg2};

struct pshdr
	{
		mpeg_t type;
		UINT64 scr, bitrate;
	};

	struct pssyshdr
	{
		DWORD rate_bound;
		BYTE video_bound, audio_bound;
		bool fixed_rate, csps;
		bool sys_video_loc_flag, sys_audio_loc_flag;
	};

	struct peshdr
	{
		WORD len;

		BYTE type:2, fpts:1, fdts:1;
		REFERENCE_TIME pts, dts;

		// mpeg1 stuff
		UINT64 std_buff_size;

		// mpeg2 stuff
		BYTE scrambling:2, priority:1, alignment:1, copyright:1, original:1;
		BYTE escr:1, esrate:1, dsmtrickmode:1, morecopyright:1, crc:1, extension:1;
		BYTE hdrlen;

		struct peshdr() {memset(this, 0, sizeof(*this));}
	};

	class seqhdr
	{
	public:
		WORD width;
		WORD height;
		BYTE ar:4;
		DWORD ifps;
		DWORD bitrate;
		DWORD vbv;
		BYTE constrained:1;
		BYTE fiqm:1;
		BYTE iqm[64];
		BYTE fniqm:1;
		BYTE niqm[64];
		// ext
		BYTE startcodeid:4;
		BYTE profile_levelescape:1;
		BYTE profile:3;
		BYTE level:4;
		BYTE progressive:1;
		BYTE chroma:2;
		BYTE lowdelay:1;
		// misc
		int arx, ary;
	};

	class mpahdr
	{
	public:
		WORD sync:11;
		WORD version:2;
		WORD layer:2;
		WORD crc:1;
		WORD bitrate:4;
		WORD freq:2;
		WORD padding:1;
		WORD privatebit:1;
		WORD channels:2;
		WORD modeext:2;
		WORD copyright:1;
		WORD original:1;
		WORD emphasis:2;
		
		int nSamplesPerSec, FrameSize, nBytesPerSec;
		REFERENCE_TIME rtDuration;
	};

	class aachdr
	{
	public:
		WORD sync:12;
		WORD version:1;
		WORD layer:2;
		WORD fcrc:1;
		WORD profile:2;
		WORD freq:4;
		WORD privatebit:1;
		WORD channels:3;
		WORD original:1;
		WORD home:1; // ?

		WORD copyright_id_bit:1;
		WORD copyright_id_start:1;
		WORD aac_frame_length:13;
		WORD adts_buffer_fullness:11;
		WORD no_raw_data_blocks_in_frame:2;

		WORD crc;

		int FrameSize, nBytesPerSec, nSamplesPerSec;
		REFERENCE_TIME rtDuration;
	};

	class ac3hdr
	{
	public:
		WORD sync;
		WORD crc1;
		BYTE fscod:2;
		BYTE frmsizecod:6;
		BYTE bsid:5;
		BYTE bsmod:3;
		BYTE acmod:3;
		BYTE cmixlev:2;
		BYTE surmixlev:2;
		BYTE dsurmod:2;
		BYTE lfeon:1;
		// the rest is unimportant for us
		int nBytesPerSec, nSamplesPerSec;
		WORD nChannels;
	};

	class eac3hdr
	{
	public:
		WORD sync;
		BYTE strmtyp:2;
		BYTE substreamid:3;
		WORD frmsiz;
		BYTE fscod:2;
		BYTE fscod2:2;
		BYTE acmod:3;
		BYTE lfeon:1;
		BYTE bsid:5;
		BYTE bsmod:3;
		// the rest is unimportant for us
		int nBytesPerSec, nSamplesPerSec;
		WORD nChannels;
	};

	class dtshdr
	{
	public:
		DWORD sync;
		BYTE frametype:1;
		BYTE deficitsamplecount:5;
		BYTE fcrc:1;
		BYTE nblocks:7;
		WORD framebytes;
		BYTE amode:6;
		BYTE sfreq:4;
		BYTE rate:5;

		BYTE downmix:1;
		BYTE dynrange:1;
		BYTE timestamp:1;
		BYTE aux_data:1;
		BYTE hdcd:1;
		BYTE ext_descr:3;
		BYTE ext_coding:1;
		BYTE aspf:1;
		BYTE lfe:2;
		BYTE predictor_history:1;

	};

	class lpcmhdr
	{
	public:
		BYTE emphasis:1;
		BYTE mute:1;
		BYTE reserved1:1;
		BYTE framenum:5;
		BYTE quantwordlen:2;
		BYTE freq:2; // 48, 96, 44.1, 32
		BYTE reserved2:1;
		BYTE channels:3; // +1
		BYTE drc; // 0x80: off
	};

	class hdmvlpcmhdr
	{
	public:
		WORD size;
		BYTE channels:4;
		BYTE samplerate:4;
		BYTE bitpersample:2;
	};

	class dvdspuhdr
	{
	public:
		// nothing ;)
	};

	class hdmvsubhdr
	{
	public:
		// nothing ;)
	};
	
	class svcdspuhdr
	{
	public:
		// nothing ;)
	};

	class cvdspuhdr
	{
	public:
		// nothing ;)
	};

	class ps2audhdr
	{
	public:
		// 'SShd' + len (0x18)
		DWORD unk1;
		DWORD freq;
		DWORD channels;
		DWORD interleave; // bytes per channel
		// padding: FF .. FF
		// 'SSbd' + len
		// pcm or adpcm data
	};

	class ps2subhdr
	{
	public:
		// nothing ;)
	};

	struct trhdr
	{
		BYTE sync; // 0x47
		BYTE error:1;
		BYTE payloadstart:1;
		BYTE transportpriority:1;
		WORD pid:13;
		BYTE scrambling:2;
		BYTE adapfield:1;
		BYTE payload:1;
		BYTE counter:4;
		// if adapfield set
		BYTE length;
		BYTE discontinuity:1;
		BYTE randomaccess:1;
		BYTE priority:1;
		BYTE PCR:1;
		BYTE OPCR:1;
		BYTE splicingpoint:1;
		BYTE privatedata:1;
		BYTE extension:1;
		// TODO: add more fields here when the flags above are set (they aren't very interesting...)

		int bytes;
		__int64 next;
	};

	struct trsechdr
	{
		BYTE table_id;
		WORD section_syntax_indicator:1;
		WORD zero:1;
		WORD reserved1:2;
		WORD section_length:12;
		WORD transport_stream_id;
		BYTE reserved2:2;
		BYTE version_number:5;
		BYTE current_next_indicator:1;
		BYTE section_number;
		BYTE last_section_number;
	};

	// http://www.technotrend.de/download/av_format_v1.pdf

	struct pvahdr
	{
		WORD sync; // 'VA'
		BYTE streamid; // 1 - video, 2 - audio
		BYTE counter;
		BYTE res1; // 0x55
		BYTE res2:3;
		BYTE fpts:1;
		BYTE postbytes:2;
		BYTE prebytes:2;
		WORD length;
		REFERENCE_TIME pts;
	};

	struct avchdr
	{
		BYTE profile, level;
		UINT64 chromaFormat;
		WORD lumaDepth, chromaDepth;
		unsigned int width, height;
		bool progressive;
		BYTE * sps;
		BYTE * pps;
		__int64 spslen;
		__int64 ppslen;
		__int64 AvgTimePerFrame;
		int arx, ary;
		BYTE ar;
		BYTE spsid;
		BYTE ppsid;
		avchdr()
		{
			progressive = true;
		  sps = NULL;
		  pps = NULL;
			spslen = 0;
			ppslen = 0;
			AvgTimePerFrame = 370000;  //27 Hz
			ar = 0;
			arx = 0;
			ary = 0;
			width = 0;
			height = 0;
		}
	  ~avchdr()
		{
		  if (sps != NULL) free(sps);
		  if (pps != NULL) free(pps);
		  sps = NULL;
		  pps = NULL;
		}
	};


	struct vc1hdr
	{
		BYTE		profile;
		BYTE		level;
		BYTE		chromaformat;
		BYTE		frmrtq_postproc;
		BYTE		bitrtq_postproc;
		BYTE		postprocflag;
		BYTE		broadcast;
		BYTE		interlace;
		BYTE		tfcntrflag;
		BYTE		finterpflag;
		BYTE		psf;
		UINT		ArX;
		UINT		ArY;
		unsigned int width, height;
	};

struct BasicVideoInfo
{
	int width;
	int height;
	double fps;
	int arx;
	int ary;
	int isInterlaced;
	bool isValid;
	int streamType;
	BYTE * sps;
	BYTE * pps;
	BYTE * vps;
	__int64 spslen;
	__int64 ppslen;
	__int64 vpslen;

	BasicVideoInfo()
	{
		width=0;
		height=0;
		fps=0;
		arx=0;
		ary=0;
		isInterlaced=0;
		streamType=0;
		isValid=false;
		spslen=0;
    ppslen=0;
    vpslen=0;
	  sps = NULL;
	  pps = NULL;
	  vps = NULL;
	}	
};

struct BasicAudioInfo
{
	int sampleRate;
	int channels;
	int streamType;
	unsigned int streamIndex;
	int bitrate;
	int aacObjectType;
	bool isValid;
	bool pmtValid;

	BasicAudioInfo()
	{
		sampleRate=0;
		channels=0;
		streamType = SERVICE_TYPE_AUDIO_UNKNOWN;
		streamIndex=0;
	  bitrate=0;
		aacObjectType=0;
		isValid=false;
		pmtValid=false;
	}
};


class CFrameHeaderParser:public CGolombBuffer, public HEVC::HevcNalDecode
{
	int m_tslen; // transport stream packet length (188 or 192 bytes, auto-detected)

	int MakeAACInitData(BYTE* pData, int profile, int freq, int channels);;
public:
	bool NextMpegStartCode(BYTE& b, __int64 len = 65536);
	bool Read(pshdr& h);
	bool Read(pssyshdr& h);
	bool Read(peshdr& h, BYTE code);
	bool Read(seqhdr& h, int len, CMediaType* pmt = NULL, bool reset = true);
	bool Read(mpahdr& h, int len, bool fAllowV25, CMediaType* pmt = NULL);
	bool Read(aachdr& h, int len, CMediaType* pmt = NULL);
	bool Read(ac3hdr& h, int len, CMediaType* pmt = NULL);
	bool Read(eac3hdr& h, int len, CMediaType* pmt = NULL);
	bool Read(dtshdr& h, int len, CMediaType* pmt = NULL);
	bool Read(lpcmhdr& h, CMediaType* pmt = NULL);
	bool Read(hdmvlpcmhdr& h, CMediaType* pmt = NULL);
	bool Read(dvdspuhdr& h, CMediaType* pmt = NULL);
	bool Read(hdmvsubhdr& h, CMediaType* pmt = NULL, const char* language_code = NULL);
	bool Read(svcdspuhdr& h, CMediaType* pmt = NULL);
	bool Read(cvdspuhdr& h, CMediaType* pmt = NULL);
	bool Read(ps2audhdr& h, CMediaType* pmt = NULL);
	bool Read(ps2subhdr& h, CMediaType* pmt = NULL);
	bool Read(trhdr& h, bool fSync = true);
	bool Read(trsechdr& h);
	bool Read(pvahdr& h, bool fSync = true);
	bool Read(avchdr& h, int len, CMediaType* pmt = NULL, bool reset = true);
	bool Read(HEVC::hevchdr& h, int len, CMediaType* pmt = NULL, bool reset = true);
	bool Read(vc1hdr& h, int len, CMediaType* pmt = NULL);

	void RemoveMpegEscapeCode(BYTE* dst, BYTE* src, int length);

	void DumpSequenceHeader(seqhdr h);
	void DumpAvcHeader(avchdr h);
	void DumpHevcHeader(HEVC::hevchdr h);
};

#endif