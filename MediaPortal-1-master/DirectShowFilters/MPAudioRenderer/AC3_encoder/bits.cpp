//-----------------------------------------------------------------------------
//
//	Musepack Demuxer
//
//	Author : Igor Janos
//
//-----------------------------------------------------------------------------
#include "..\source\stdafx.h"
#include "bits.h"

typedef UINT8 uint8;
typedef UINT16 uint16;
typedef UINT32 uint32;


//-----------------------------------------------------------------------------
//
//	Bitstream class
//
//	podporuje citanie bitstreamu
//
//----------------------------------------------------------------------------

const int32 Bitstream::EXP_GOLOMB_MAP[2][48] =
{
	{
		47, 31, 15,  0, 23, 27, 29, 30,
		 7, 11, 13, 14, 39, 43, 45, 46,
		16,  3,  5, 10, 12, 19, 21, 26,
		28, 35, 37, 42, 44,  1,  2,  4,
		 8, 17, 18, 20, 24,  6,  9, 22,
	    25, 32, 33, 34, 36, 40, 38, 41
	},
	{
		 0, 16,  1,  2,  4,  8, 32,  3,
	     5, 10, 12, 15, 47,  7, 11, 13,
	    14,  6,  9, 31, 35, 37, 42, 44,
		33, 34, 36, 40, 39, 43, 45, 46,
		17, 18, 20, 24, 19, 21, 26, 28,
		23, 27, 29, 30, 22, 25, 38, 41
	}
};

const int32 Bitstream::EXP_GOLOMB_MAP_INV[2][48] =
{
	{
		 3, 29, 30, 17, 31, 18, 37,  8,
	    32, 38, 19,  9, 20, 10, 11,  2,
		16, 33, 34, 21, 35, 22, 39,  4,
		36, 40, 23,  5, 24,  6,  7,  1,
		41, 42, 43, 25, 44, 26, 46, 12,
		45, 47, 27, 13, 28, 14, 15,  0
	},
	{
		 0,  2,  3,  7,  4,  8, 17, 13,
	     5, 18,  9, 14, 10, 15, 16, 11,
	     1, 32, 33, 36, 34, 37, 44, 40,
	    35, 45, 38, 41, 39, 42, 43, 19,
		 6, 24, 25, 20, 26, 21, 46, 28,
	    27, 47, 22, 29, 23, 30, 31, 12 
	}
};


// Exp-Golomb Codes

const int32 Bitstream::EXP_GOLOMB_SIZE[255] =
{
    1, 3, 3, 5, 5, 5, 5, 7, 7, 7, 7, 7, 7, 7, 7,
    9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9, 9,
    11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,11,
    11,11,11,11,11,11,11,11,11,13,13,13,13,13,13,13,13,13,13,13,13,13,13,
    13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,
    13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,13,
    13,13,13,13,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,
    15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,
    15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,
    15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,
    15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,
    15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15,15
};

uint32 Bitstream::Get_UE()
{
	int32 len = 0;
	NeedBits24();
	while (!UGetBits(1)) len++;
	NeedBits24();
	return (len == 0 ? 0 : (1<<len)-1 + UGetBits(len));
}

int32 Bitstream::Get_SE()
{
	int32 len = 0;
	NeedBits24();
	while (!UGetBits(1)) len++;
	if (len == 0) return 0;
	NeedBits24();
	int32 val = (1 << len) | UGetBits(len);
    return (val&1) ? -(val>>1) : (val>>1);
}

int32 Bitstream::Get_ME(int32 mode)
{
	// nacitame UE a potom podla mapovacej tabulky
	int32 codeNum = Get_UE();
	if (codeNum >= 48) return -1;		// chyba
	return EXP_GOLOMB_MAP[mode][codeNum];
}

int32 Bitstream::Get_TE(int32 range)
{
	/* ISO/IEC 14496-10 - Section 9.1 */
	if (range > 1) {
		return Get_UE();
	} else {
		return (!UGetBits(1))&0x01;
	}
}

int32 Bitstream::Get_Golomb(int k)
{
	int32 l=0;
	NeedBits();
	while (UBits(8) == 0) {
		l += 8;
		DumpBits(8);
		NeedBits();	
	}
	while (UGetBits(1) == 0) l++;
	NeedBits();
	return (l << k) | UGetBits(k);
}

