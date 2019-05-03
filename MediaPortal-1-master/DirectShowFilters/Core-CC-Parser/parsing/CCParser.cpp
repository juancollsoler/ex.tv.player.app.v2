//    -----------------------------------------
//    Notes for original code version (2.0.0.6)
//    -----------------------------------------
//    CCCP (Core Closed Captioning Parser) is a DirectShow filter 
//    that extracts Closed Captioning data from MPEG2 video. 
//    The data is normally used by the downstream filter to 
//    render and mix the CC with main video to aid hearing
//    or language-impaired users
//
//    Closed Captioning MPEG2 Parser
//    Original author: Zodiak
//    Copyright (C) 2004 zodiak@dvbn
//
//    Also more information on the SourceForge CCCP project is here:
//    http://sourceforge.net/p/cccp/discussion/496070/thread/6f610e28/
//    -----------------------------------------

/*
 *  Modified to add H.264 Closed Caption parsing 
 *  and converted to Unicode.
 *
 *  Copyright (C) 2015 Team MediaPortal
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

#include "CCParser.h"

//#ifdef _DEBUG
//#define new DEBUG_NEW
//#undef THIS_FILE
//static char THIS_FILE[] = __FILE__;
//#endif

// uncomment the //LogDebug to enable extra logging
#define LOG_DETAIL //LogDebug

extern void LogDebug(const char *fmt, ...) ;

/////////////////////////////////////////////////////////////////////////////
// CCcParser

inline TCHAR UpdateExcepionChar( char c )
{
	switch( c )
	{
		case 0x2A: return _T('�');
		case 0x5C: return _T('�');
		case 0x5E: return _T('�');
		case 0x5F: return _T('�');
		case 0x60: return _T('�');
		case 0x7B: return _T('�');
		case 0x7C: return _T('�');
		case 0x7D: return _T('�');
		case 0x7E: return _T('�');
	};

	return (TCHAR)c;
}

void CCWORD::GetString( TCHAR szString[3] ) const
{
	if( !IsText())
	{
		szString[0] = 0; 
		
		return;
	}
	
	szString[1] = 0;

	if( IsExtChar())
	{
		switch( b1() & 0x13 )
		{
			case 0x12:
			{
				switch( b2())
				{
					case 0x20: szString[0] = _T('�'); return;
					case 0x21: szString[0] = _T('�'); return;
					case 0x22: szString[0] = _T('�'); return;
					case 0x23: szString[0] = _T('�'); return;
					case 0x24: szString[0] = _T('�'); return;
					case 0x25: szString[0] = _T('�'); return;
					case 0x26: szString[0] = _T('\''); return;
					case 0x27: szString[0] = _T('�'); return;
					case 0x28: szString[0] = _T('*'); return; // actual asterisk
					case 0x29: szString[0] = _T('\''); return;
					case 0x2a: szString[0] = _T('-'); return;
					case 0x2b: szString[0] = _T('�'); return;
					case 0x2c: szString[0] = _T('*'); return; // {sm}
					case 0x2d: szString[0] = _T('�'); return;
					case 0x2e: szString[0] = _T('"'); return;
					case 0x2f: szString[0] = _T('"'); return;
					case 0x30: szString[0] = _T('�'); return;
					case 0x31: szString[0] = _T('�'); return;
					case 0x32: szString[0] = _T('�'); return;
					case 0x33: szString[0] = _T('�'); return;
					case 0x34: szString[0] = _T('�'); return;
					case 0x35: szString[0] = _T('�'); return;
					case 0x36: szString[0] = _T('�'); return;
					case 0x37: szString[0] = _T('�'); return;
					case 0x38: szString[0] = _T('�'); return;
					case 0x39: szString[0] = _T('�'); return;
					case 0x3a: szString[0] = _T('�'); return;
					case 0x3b: szString[0] = _T('�'); return;
					case 0x3c: szString[0] = _T('�'); return;
					case 0x3d: szString[0] = _T('�'); return;
					case 0x3e: szString[0] = _T('�'); return;
					case 0x3f: szString[0] = _T('�'); return;
				};
			}
			break;

			case 0x13:
			{
				switch( b2())
				{
					case 0x20: szString[0] = _T('�'); return;
					case 0x21: szString[0] = _T('�'); return;
					case 0x22: szString[0] = _T('�'); return;
					case 0x23: szString[0] = _T('�'); return;
					case 0x24: szString[0] = _T('�'); return;
					case 0x25: szString[0] = _T('�'); return;
					case 0x26: szString[0] = _T('�'); return;
					case 0x27: szString[0] = _T('�'); return;
					case 0x28: szString[0] = _T('�'); return;
					case 0x29: szString[0] = _T('{'); return;
					case 0x2a: szString[0] = _T('}'); return;
					case 0x2b: szString[0] = _T('\\'); return;
					case 0x2c: szString[0] = _T('^'); return;
					case 0x2d: szString[0] = _T('_'); return;
					case 0x2e: szString[0] = _T('|'); return;
					case 0x2f: szString[0] = _T('~'); return;
					case 0x30: szString[0] = _T('�'); return;
					case 0x31: szString[0] = _T('�'); return;
					case 0x32: szString[0] = _T('�'); return;
					case 0x33: szString[0] = _T('�'); return;
					case 0x34: szString[0] = _T('�'); return;
					case 0x35: szString[0] = _T('�'); return;
					case 0x36: szString[0] = _T('�'); return;
					case 0x37: szString[0] = _T('|'); return;
					case 0x38: szString[0] = _T('�'); return;
					case 0x39: szString[0] = _T('�'); return;
					case 0x3a: szString[0] = _T('�'); return;
					case 0x3b: szString[0] = _T('�'); return;
					case 0x3c: szString[0] = _T('*'); return; // {ul}
					case 0x3d: szString[0] = _T('*'); return; // {ur}
					case 0x3e: szString[0] = _T('*'); return; // {ll}
					case 0x3f: szString[0] = _T('*'); return; // {lr}
				};
			}
			break;

			default:
				ASSERT(0);
		}
	}
	

	szString[0] = UpdateExcepionChar( reinterpret_cast<const char*>( &m_w )[0]& 0x7F );
	szString[1] = UpdateExcepionChar( reinterpret_cast<const char*>( &m_w )[1]& 0x7F );
	szString[2] = 0;
}

/////////////////////////////////////////////////////////////////////////////
// CCcParser

CCcParser::CCcParser()
{
  m_CcParserH264 = new CcParseH264();
  Reset();
}

CCcParser::~CCcParser()
{
  delete m_CcParserH264;
}

void CCcParser::Reset()
{
	m_ccsetLastPorI.Empty();
	
	m_ccsetH264WrIdx = 0;
    
  for (int i = 0; i < _countof(m_ccsetH264); i++ )
  {
    m_ccsetH264[i].m_timeStamp = _I64_MAX;
  }

//	m_picture_coding_type = typeNone;
}

enum
{ 
	mask_temporal_reference  = 0xC0FF, // 11111111 11000000
	mask_picture_coding_type = 0x3800, // 00000000 00111000
	typeNone				 = 0,
	typeI 					 = 0x0800, // 00000000 00001000	
	typeP 					 = 0x1000, // 00000000 00010000	
	typeB 					 = 0x1800, // 00000000 00011000	
};

inline bool RecognizePictureHeader( WORD& picture_coding_type, const BYTE* p, int iRollBack = 0 )
{
	ASSERT( iRollBack <= 4 );
	
	if(( *reinterpret_cast<const UNALIGNED DWORD*>(p) << 8*iRollBack ) != ( (DWORD)0x00010000 & ( (DWORD)0xFFFFFFFF << 8*iRollBack )))
		return false;

	WORD test_picture_coding_type = 
		(WORD)( *reinterpret_cast<const UNALIGNED WORD*>(p+4-iRollBack) & mask_picture_coding_type ); 

	if( test_picture_coding_type == typeI ||
		test_picture_coding_type == typeP ||
		test_picture_coding_type == typeB
	  )
	{
		picture_coding_type = test_picture_coding_type;

		return true;
	}

	return false;
}

bool CCcParser::OnDataArrivalMPEG( const BYTE* pData, UINT cbData, REFERENCE_TIME sourceTime )
{
	const BYTE* pStop = pData + cbData;

	ASSERT( 4 == sizeof(DWORD));
	ASSERT( 2 == sizeof(WORD));

	WORD picture_coding_type = typeNone;
	WORD temporal_reference = 0;

	for( const BYTE* p = pData; p < pStop - sizeof(DWORD); ++p )
	{
		if(( *reinterpret_cast<const UNALIGNED DWORD*>(p) & 0x00FFFFFF ) == 0x00010000 ) // 00000100 : start code prefix
		{
			switch( p[3])
			{
				case 0x00:   // picture_start_code
				{
					VERIFY( RecognizePictureHeader ( picture_coding_type, p ));
					temporal_reference = MAKEWORD( p[5] & 0xC0, p[4] ) >> 6;
				}
				break;

				case 0xb2:
				{
					if( picture_coding_type == typeNone )
					{
						// Sometimes pData starts a byte or so into the picture header.
						//	That's the "start code prefix" is not recognized.
						
						for( int iRollBack = 1; iRollBack <= 4; iRollBack++ )
							if( RecognizePictureHeader( picture_coding_type, pData, iRollBack ))
								break; // for
					}
					
					bool bPorI = ( picture_coding_type == typeP || picture_coding_type == typeI );
					ASSERT( bPorI || picture_coding_type == typeB );
					
					p = OnUserData( bPorI, p + 4, pStop, sourceTime, false );
					if( !p )
						return false;

					picture_coding_type = typeNone;
				}
				break;
			}
		}
		else
		{
			enum{ cReportInterval = 1000 };
			if( (p - pData) % cReportInterval == 0 )
			{
				if( !OnProgress( (p - pData) * 100 / cbData ))
					return false;
			}

			continue;
		}
	}

	return true;
}

bool CCcParser::OnDataArrivalAVC1( const BYTE* pData, UINT cbData, DWORD dwFlags, REFERENCE_TIME sourceTime )
{
	ASSERT( 4 == sizeof(DWORD));
	ASSERT( 2 == sizeof(WORD));

  DWORD ccBuffLength = m_CcParserH264->parseAVC1sample(pData, cbData, dwFlags);
  
  if (ccBuffLength < sizeof(DWORD))
  {
    return false;
  }

  const BYTE* pBuffer = m_CcParserH264->get_cc_buffer_pointer();
  DWORD ccPackLen;
	const BYTE* pStop;

  //LogDebug ("CCcParser: OnDataArrivalAVC1() - Start loop, pBuffer: %d", pBuffer);

  while (ccBuffLength >= sizeof(DWORD))
  {  
    ccPackLen = *(UNALIGNED DWORD*)pBuffer;  //The length field is little-endian format

    pBuffer += sizeof(DWORD);
    ccBuffLength -= sizeof(DWORD);
        
    if (ccBuffLength < ccPackLen)
    {
			LogDebug ("CCcParser: OnDataArrivalAVC1() - ccPackLen error: %d", ccBuffLength - ccPackLen);
  	  return false;
    }
    
    pStop = pBuffer + ccPackLen;

	  OnUserData( false, pBuffer, pStop, sourceTime, true );
		//LogDebug ("CCcParser: OnDataArrivalAVC1() - OnUserData() processed, pBuffer: %d", pBuffer);
          
    ccBuffLength -= ccPackLen;
    pBuffer += ccPackLen;
    
  }

	return true;
}


const BYTE* CCcParser::OnUserData( bool bPorI, const BYTE* p, const BYTE* pStop, REFERENCE_TIME sourceTime, bool bIsSubtypeAVC1 )
{
	if( *reinterpret_cast<const UNALIGNED DWORD*>(p) == 0x34394147 ) // 47413934
		return Parse_ATSC_A53( bPorI, p + 4, pStop, sourceTime, bIsSubtypeAVC1 );
	
	if( *reinterpret_cast<const UNALIGNED WORD*>(p) == 0x0205 ) // 0502
		return Parse_Echostar( bPorI, p, pStop );
	
	return Parse_Unknown( bPorI, p, pStop );
}

inline const BYTE* CCcParser::Parse_ATSC_A53( bool bPorI, const BYTE* p, const BYTE* pStop, REFERENCE_TIME sourceTime, bool bIsSubtypeAVC1 )
{
	CCWORDSET cc;
	int iField1 = 0;
	int iField2 = 0;
	
	enum{ typeCCData = 0x03, typeBarData = 0x06 };
	if( *p == typeCCData ) //
	{
		p++;
		
		enum{ process_cc_data_flag = 0x40, mask_cc_count = 0x1F };
		if( p < pStop && ( p[0] & process_cc_data_flag ))
		{
			const int cCC_count = p[0] & mask_cc_count;
			if( cCC_count > 0 )
			{
				p++; // flags
				p++; // reserved 0xff

				enum{ cbToken = 3 };
				
				for( int iCC = 0; iCC < cCC_count && p+cbToken < pStop; iCC++ )
				{
					enum
					{ 
						flag_cc_valid = 0x04, mask_cc_type = 0x03, 
						typeField1 = 0x00, typeField2 = 0x01,  
					};

					//if( p[0] & flag_cc_valid )
					if( true )
					{
						switch( p[0] & mask_cc_type )
						{
							case typeField1:
							{
								if( iField1 < _countof( cc.m_ccField1 ))
								{
									cc.m_ccField1[iField1] = CCWORD( &p[1] );
									cc.m_timeStamp = sourceTime;
									iField1++;
		              LOG_DETAIL ("CCcParser: Parse_ATSC_A53() - Triplet F1 %d, Hex: 0x%x 0x%x 0x%x, ASCII: %c %c", iCC, *(p+0), *(p+1), *(p+2), (0x7F & *(p+1)), (0x7F & *(p+2)));
								}
								else
									ASSERT(0);
							}
							break;

							case typeField2:
							{
								if( iField2 < _countof( cc.m_ccField2 ))
								{
									cc.m_ccField2[iField2] = CCWORD( &p[1] );
									cc.m_timeStamp = sourceTime;
									iField2++;
		              LOG_DETAIL ("CCcParser: Parse_ATSC_A53() - Triplet F2 %d, Hex: 0x%x 0x%x 0x%x, ASCII: %c %c", iCC, *(p+0), *(p+1), *(p+2), (0x7F & *(p+1)), (0x7F & *(p+2)));
								}
								else
									ASSERT(0);
							}
							break;
						}
					}

					p += cbToken;
				}
			}
		}
	}
	
	if( !OnCCSet( bPorI, cctypeATSC_A53, cc, bIsSubtypeAVC1 ))
		return false; 

	return SkipUserData( p, pStop ); //TODO
}

inline const BYTE* CCcParser::Parse_Echostar( bool bPorI, const BYTE* pData, const BYTE* pStop )
{
	CCWORDSET cc;
	
	//bPorI = false;
	bool bError = false;
	
	enum{ cTokenHeader = 2 };
	for( const BYTE* p = pData; 
	     !bError && p < pStop - cTokenHeader - p[0]; 
		 p += ( cTokenHeader + p[0])
	   )
	{
		switch( p[0] )
		{
			case 0x05:
			{
				if( p[1] == 0x04 )
				{
					ASSERT( bPorI );
					//bPorI = true;
					
				}
				else if( p[1] == 0x02 )
					; // Do nothing
				else
					bError = true;

			}
			break;
			
			case 0x02:
			{
				if( p[1] == 0x09 )
				{
					cc.m_ccField1[0] = CCWORD( &p[2]);
				}
				else if( p[1] == 0x0a )
				{
					cc.m_ccField2[0] = CCWORD( &p[2]);
				}
				else
					bError = true;
			}
			break;

			case 0x1b:
			{
				if( p[1] == 0x09 )
				{
					cc.m_ccField1[0] = CCWORD( &p[2]);
				}
				else if( p[1] == 0x0a )
				{
					cc.m_ccField2[0] = CCWORD( &p[2]);
				}
				else
					bError = true;
			}
			break;
			
			case 0x04:
			{
				if( p[1] == 0x09 )
				{
					cc.m_ccField1[0] = CCWORD( &p[2]);
					cc.m_ccField1[1] = CCWORD( &p[4]);
				}
				else if( p[1] == 0x0a )
				{
					cc.m_ccField2[0] = CCWORD( &p[2]);
					cc.m_ccField2[1] = CCWORD( &p[4]);
				}
				else
					bError = true;
			}
			break;
			
			case 0x00:
			{
				if( !OnCCSet( bPorI, cctypeEchostar, cc, false ))
					return NULL;

				return SkipUserData( p, pStop ); 
			};
			break;


			default:
				bError = true;
		}
	}
	
	return Parse_Unknown( bPorI, pData, pStop );
}

const BYTE* CCcParser::Parse_Unknown ( bool bPorI, const BYTE* p, const BYTE* pStop )
{
	return SkipUserData( p, pStop ); //TODO
}

const BYTE* CCcParser::SkipUserData( const BYTE* p, const BYTE* pStop )
{
	while( p < pStop - 3 && !( p[0] == 0x00 && p[1] == 0x00 && p[2] == 0x01 ))
	{
		p++;
	}

	return p + 2;
}

bool CCcParser::OnCCSet( bool bPorI, int nType, const CCWORDSET& cc, bool bIsSubtypeAVC1 )
{
  if (!bIsSubtypeAVC1)
  {
  	if( bPorI)
  	{
  		if( !m_ccsetLastPorI.IsEmpty())
  		{
  			if( !SendCCSet( nType, m_ccsetLastPorI ))
  				return NULL;
  		}
  
  		m_ccsetLastPorI = cc;
  
  		return true;
  	}
  
  	return SendCCSet( nType, cc );
  }
  else
  {
    //Use a buffer to re-order the samples into the correct temporal display order
    bool retval = false;
    //write to buffer
    for (int i = 0; i < _countof(m_ccsetH264); i++ )
    {
      if (m_ccsetH264[i].m_timeStamp == _I64_MAX) //Found empty slot - push buffer
      {
        m_ccsetH264[i] = cc;
        m_ccsetH264WrIdx++;
        LOG_DETAIL("CCcParser: OnCCSet - write buffer, i: %d, m_ccsetH264WrIdx: %d", i, m_ccsetH264WrIdx);
        break;
      }
    }
        
    if (m_ccsetH264WrIdx >= _countof(m_ccsetH264))
    {      
      REFERENCE_TIME earliestTime = _I64_MAX;
      int earliestCCidx = 0;
      for (int i = 0; i < m_ccsetH264WrIdx; i++)
      {
        if (m_ccsetH264[i].m_timeStamp < earliestTime)
        {
          earliestCCidx = i;
          earliestTime = m_ccsetH264[i].m_timeStamp;
        }
      }
    
      if (earliestTime < _I64_MAX) //Found a valid timestamp - pop buffer
      {
        LOG_DETAIL("CCcParser: OnCCSet - read buffer, earliestCCidx: %d, m_ccsetH264WrIdx: %d", earliestCCidx, m_ccsetH264WrIdx);
        retval = SendCCSet( nType, m_ccsetH264[earliestCCidx]);
        m_ccsetH264[earliestCCidx].m_timeStamp = _I64_MAX; //invalidate buffer
        m_ccsetH264WrIdx--;
      }
    }

  	return retval;
  }
}

bool CCcParser::SendCCSet( int nType, const CCWORDSET& cc )
{
	if( !cc.m_ccField1[0].IsEmpty())
	{
		if( !OnCc( nType, fieldOdd, cc.m_ccField1[0] ))
			return false;

		if( !cc.m_ccField1[1].IsEmpty())
		{
			if( !OnCc( nType, fieldOdd, cc.m_ccField1[1] ))
				return false;
		}
	}

	if( !cc.m_ccField2[0].IsEmpty())
	{
		if( !OnCc( nType, fieldEven, cc.m_ccField2[0] ))
			return false;

		if( !cc.m_ccField1[1].IsEmpty())
		{
			if( !OnCc( nType, fieldEven, cc.m_ccField2[1] ))
				return false;
		}
	}

	return true;
}

void CCcTextParser::Reset( UINT idChannel, UINT idMode )
{
	ASSERT( idChannel <= 3 );
	m_ccproLookedFor.m_idChannel = idChannel;

	ASSERT( idChannel < CCPROFILE::cModes );
	m_ccproLookedFor.m_idMode = idMode;

	m_ccproCurrent.m_idMode = CCPROFILE::modeNone;
	m_ccproCurrent.m_idChannel = 0;

	m_ccLastCode = 0;
}

bool CCcTextParser::OnCc( int nType, int iField, CCWORD cc )
{
	if( nType != 0 && !cc.IsEmpty() && m_ccproLookedFor.IsFieldOK( iField ))
	{
		if( cc.IsStartXDS()) 
		{
			ASSERT( iField == CCcParser::fieldEven );
			m_ccproCurrent.m_idMode = CCPROFILE::modeXDS;
		}
		else if( cc.IsEndXDS())
		{
			ASSERT( iField == CCcParser::fieldEven );
			m_ccproCurrent.m_idMode = CCPROFILE::modeCC;
		}
		else if( cc.IsCode() && !cc.IsExtChar())
		{
			m_ccproCurrent.m_idMode = CCPROFILE::modeCC;

			if( cc == m_ccLastCode )
			{
				m_ccLastCode = 0;
				return true;
			}

			m_ccLastCode = cc;
				
			m_ccproCurrent.UpdateChannelFlag( cc.b1() );
			if( m_ccproCurrent == m_ccproLookedFor )
			{
				if( !OnCode( cc ))
					return false;
			}
		}
		else if( m_ccproCurrent == m_ccproLookedFor )
		{
			ASSERT( cc.IsText());
			
			if( !OnText( cc ))
				return false;
		}
	}

	

	return true;
}

