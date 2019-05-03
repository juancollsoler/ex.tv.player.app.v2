/* 
 *  Copyright (C) 2006-2008 Team MediaPortal
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
#pragma warning(disable : 4995)
#include <windows.h>
#include "..\shared\PacketSync.h"


CPacketSync::CPacketSync(void)
{
  m_tempBufferPos = -1;
  m_bInSync = false;
  m_bFirstSynced = false;
  m_packet_len = TS_PACKET_LEN;
}

CPacketSync::~CPacketSync(void)
{
}

void CPacketSync::Reset(void)
{
  m_tempBufferPos = -1;
  m_bInSync = false;
  m_bFirstSynced = false;
  m_packet_len = TS_PACKET_LEN;
}

// Ambass : Now, need to have 2 consecutive TS_PACKET_SYNC to try avoiding bad synchronisation.  
//          In case of data flow change ( Seek, tv Zap .... ) Reset() should be called first to flush buffer.
void CPacketSync::OnRawData(byte* pData, int nDataLen)
{
  int syncOffset=0;
  int tempBuffOffset=0;
  bool goodPacket = false;

  if (m_tempBufferPos > 0 ) //We have some residual data from the last call
  {
    syncOffset = TS_PACKET_LEN - m_tempBufferPos;
    
    if (nDataLen <= syncOffset) 
    {
      //not enough total data to scan through a packet length, 
      //so add pData to the tempBuffer and return
      memcpy(&m_tempBuffer[m_tempBufferPos], pData, nDataLen);
      m_tempBufferPos += nDataLen;
      return ;
    }

    while ((nDataLen > syncOffset) && (m_tempBufferPos > tempBuffOffset)) 
    {
      if ((m_tempBuffer[tempBuffOffset]==TS_PACKET_SYNC) &&
          (pData[syncOffset]==TS_PACKET_SYNC)) //found a good packet
      {
        if (syncOffset) 
        {
          memcpy(&m_tempBuffer[m_tempBufferPos], pData, syncOffset);
        }
        OnTsPacket(&m_tempBuffer[tempBuffOffset]);
        goodPacket = true;
        m_bInSync = true;
        break;
      }
      else if (m_bInSync &&
               ((m_tempBuffer[tempBuffOffset]==TS_PACKET_SYNC) ||
                (pData[syncOffset]==TS_PACKET_SYNC))) //found a good packet
      {
        if (syncOffset) 
        {
          memcpy(&m_tempBuffer[m_tempBufferPos], pData, syncOffset);
        }
        m_tempBuffer[tempBuffOffset] = TS_PACKET_SYNC;
        pData[syncOffset] = TS_PACKET_SYNC;
        OnTsPacket(&m_tempBuffer[tempBuffOffset]);
        goodPacket = true;
        m_bInSync = false;
        break;
      }
      else
      {
        syncOffset++;
        tempBuffOffset++;
        m_bInSync = false;
      }
    }    
    
    if (!goodPacket)
    {
      if (tempBuffOffset >= m_tempBufferPos)
      {
        //We have scanned all of the data in m_tempBuffer,
        //so continue search from the start of pData buffer.
        syncOffset = 0;
      }
      else
      {
        //move data down to discard data we have already scanned
        m_tempBufferPos -= tempBuffOffset;
        memmove(m_tempBuffer, &m_tempBuffer[tempBuffOffset], m_tempBufferPos);
        //add pData to the tempBuffer and return
        memcpy(&m_tempBuffer[m_tempBufferPos], pData, nDataLen);
        m_tempBufferPos += nDataLen;
        return;
      }
    }
  }

  m_tempBufferPos = 0; //We have consumed the residual data

  while (nDataLen > (syncOffset + TS_PACKET_LEN)) //minimum of TS_PACKET_LEN+1 bytes available
  {
    if (!goodPacket && (syncOffset > (TS_PACKET_LEN * 8)) )
    {
      //No sync - abandon the buffer
      Reset();
      return;
    }
    
    if ((pData[syncOffset] == TS_PACKET_SYNC) &&
        (pData[syncOffset + TS_PACKET_LEN]==TS_PACKET_SYNC))
    {
      OnTsPacket( &pData[syncOffset] );
      syncOffset += TS_PACKET_LEN;
      goodPacket = true;
      m_bInSync = true;
    }
    else if (m_bInSync &&
             ((pData[syncOffset] == TS_PACKET_SYNC) ||
              (pData[syncOffset + TS_PACKET_LEN]==TS_PACKET_SYNC)))
    {
      pData[syncOffset] = TS_PACKET_SYNC;
      pData[syncOffset + TS_PACKET_LEN] = TS_PACKET_SYNC;
      OnTsPacket( &pData[syncOffset] );
      syncOffset += TS_PACKET_LEN;
      goodPacket = true;
      m_bInSync = false;
    }
    else
    {
      syncOffset++;
      m_bInSync = false;
    }
  }
  
  // We have less than TS_PACKET_LEN+1 bytes available - store residual data for next time
  m_tempBufferPos= nDataLen - syncOffset;
  if (m_tempBufferPos)
  {
    memcpy( m_tempBuffer, &pData[syncOffset], m_tempBufferPos );
  }
}

// Ambass : Now, need to have 2 consecutive TS_PACKET_SYNC to try avoiding bad synchronisation.  
//          In case of data flow change ( Seek, tv Zap .... ) Reset() should be called first to flush buffer.
// Owlsroost : This version will abandon a buffer if it fails to sync within 8 * TSpacket lengths
// Owlsroost : Modified to automatically handle 192 byte (e.g. BD/AVCHD) and 188 byte packet sizes.
//             Only the basic 188 byte packet data is passed on for '192 byte' streams.
bool CPacketSync::OnRawData2(byte* pData, int nDataLen)
{
  int syncOffset = m_packet_len - m_tempBufferPos;
  int tempBuffOffset=0;
  bool goodPacket = false;

  if (m_bInSync && syncOffset >= 0 && m_tempBufferPos > 0) //We have some residual data from the last call, and we know the packet length
  {    
    if (nDataLen <= syncOffset) 
    {
      //not enough total data to scan through a packet length, 
      //so add pData to the tempBuffer and return
      memcpy(&m_tempBuffer[m_tempBufferPos], pData, nDataLen);
      m_tempBufferPos += nDataLen;
      return false ;
    }

    while ((nDataLen > syncOffset) && (m_tempBufferPos > tempBuffOffset)) 
    {
      if ((m_tempBuffer[tempBuffOffset]==TS_PACKET_SYNC) &&
          (pData[syncOffset]==TS_PACKET_SYNC)) //found a good packet
      {
        if (syncOffset) 
        {
          memcpy(&m_tempBuffer[m_tempBufferPos], pData, syncOffset);
        }
        OnTsPacket(&m_tempBuffer[tempBuffOffset], syncOffset, nDataLen);
        goodPacket = true;
        break;
      }
      else if ((m_tempBuffer[tempBuffOffset]==TS_PACKET_SYNC) ||
               (pData[syncOffset]==TS_PACKET_SYNC)) //found a good packet
      {
        if (syncOffset) 
        {
          memcpy(&m_tempBuffer[m_tempBufferPos], pData, syncOffset);
        }
        m_tempBuffer[tempBuffOffset] = TS_PACKET_SYNC;
        pData[syncOffset] = TS_PACKET_SYNC;
        OnTsPacket(&m_tempBuffer[tempBuffOffset], syncOffset, nDataLen);
        goodPacket = true;
        m_bInSync = false;
        break;
      }
      else
      {
        syncOffset++;
        tempBuffOffset++;
        m_bInSync = false;
      }
    }    
    
    if (!goodPacket)
    {
      if (tempBuffOffset >= m_tempBufferPos)
      {
        //We have scanned all of the data in m_tempBuffer,
        //so continue search from the start of pData buffer.
        syncOffset = 0;
      }
      else
      {
        //move data down to discard data we have already scanned
        m_tempBufferPos -= tempBuffOffset;
        memmove(m_tempBuffer, &m_tempBuffer[tempBuffOffset], m_tempBufferPos);
        //add pData to the tempBuffer and return
        memcpy(&m_tempBuffer[m_tempBufferPos], pData, nDataLen);
        m_tempBufferPos += nDataLen;
        return false;
      }
    }
  }
  else  //No residual data
  {
    syncOffset = 0;
  }

  m_tempBufferPos = 0; //We have consumed the residual data
  
  while (nDataLen > (syncOffset + M2TS_PACKET_LEN)) //minimum of M2TS_PACKET_LEN+1 bytes available
  {
    if (!goodPacket && (syncOffset > (M2TS_PACKET_LEN * 8)) )
    {
      //No sync - abandon the buffer
      Reset();
      return false;
    }

    if (!m_bInSync)
    {
      if ((pData[syncOffset] == TS_PACKET_SYNC) &&
          (pData[syncOffset + M2TS_PACKET_LEN]==TS_PACKET_SYNC) &&
          (pData[syncOffset + TS_PACKET_LEN]!=TS_PACKET_SYNC))
      {
        m_bInSync = true;
        m_packet_len = M2TS_PACKET_LEN;
      }
      else if ((pData[syncOffset] == TS_PACKET_SYNC) &&
          (pData[syncOffset + TS_PACKET_LEN]==TS_PACKET_SYNC) &&
          (pData[syncOffset + M2TS_PACKET_LEN]!=TS_PACKET_SYNC))
      {
        m_bInSync = true;
        m_packet_len = TS_PACKET_LEN;
      }
    }
        
    if (m_bInSync &&
        ((pData[syncOffset] == TS_PACKET_SYNC) &&
         (pData[syncOffset + m_packet_len]==TS_PACKET_SYNC)))
    {
      OnTsPacket( &pData[syncOffset], syncOffset, nDataLen );
      syncOffset += m_packet_len;
      goodPacket = true;
    }
    else if (m_bInSync &&
             ((pData[syncOffset] == TS_PACKET_SYNC) ||
              (pData[syncOffset + m_packet_len]==TS_PACKET_SYNC)))
    {
      pData[syncOffset] = TS_PACKET_SYNC;
      pData[syncOffset + m_packet_len] = TS_PACKET_SYNC;
      OnTsPacket( &pData[syncOffset], syncOffset, nDataLen );
      syncOffset += m_packet_len;
      goodPacket = true;
      m_bInSync = false;
    }
    else
    {
      syncOffset++;
      m_bInSync = false;
    }    
  }

  if (m_bInSync)
  {
    //Catch and process TS_PACKET_LEN (only) size packets at end of buffer
    while (nDataLen > (syncOffset + m_packet_len)) //minimum of TS_PACKET_LEN+1 bytes available
    {        
      if ((pData[syncOffset] == TS_PACKET_SYNC) &&
          (pData[syncOffset + m_packet_len]==TS_PACKET_SYNC))
      {
        OnTsPacket( &pData[syncOffset], syncOffset, nDataLen );
        syncOffset += m_packet_len;
        goodPacket = true;
      }
      else
      {
        syncOffset++;
        m_bInSync = false;
      }    
    }
  
    // We have less than m_packet_len+1 bytes available - store residual data for next time
    m_tempBufferPos = nDataLen - syncOffset;
    if (m_tempBufferPos)
    {
      memcpy( m_tempBuffer, &pData[syncOffset], m_tempBufferPos );
    }
  }
  else if (nDataLen < (M2TS_PACKET_LEN * 5))
  {
    //No sync and short buffer
    return true;
  } 
  
  return false;
}


// Used to perform a simple data integrity check on a buffer.
// (To decide if it needs to be re-read from disk).
// Does not update any temp buffers or pointers, so that
// it can be re-run on a new copy of the buffer.
// Must be followed eventually by a call to OnRawData2() to process
// the buffer fully (which does any necessary pointer updating etc.)
// Returns a count of bytes scanned while not in-sync with the TS packet boundaries.
int CPacketSync::OnRawDataCheck(byte* pData, int nDataLen)
{
  if (!m_bInSync) return 0; //We don't know what the packet length is yet..
    
  int syncOffset=0;
  int tempBuffOffset=0;
  bool goodPacket = false;
  int syncErrors=0;

  if (m_tempBufferPos > 0 ) //We have some residual data from the last call
  {
    syncOffset = m_packet_len - m_tempBufferPos;
    
    if (nDataLen <= syncOffset) 
    {
      //not enough total data to scan through a packet length, 
      //so just return 'no error'
      return 0;
    }

    while ((nDataLen > syncOffset) && (m_tempBufferPos > tempBuffOffset)) 
    {
      if ((m_tempBuffer[tempBuffOffset]==TS_PACKET_SYNC) &&
          (pData[syncOffset]==TS_PACKET_SYNC)) //found a good packet
      {
        goodPacket = true;
        m_bFirstSynced = true; 
        break;
      }
      else
      {
        syncOffset++;
        tempBuffOffset++;
        if (m_bFirstSynced)
        {
          syncErrors++;
        }
      }
    }    
    
    if (!goodPacket)
    {
      if (tempBuffOffset >= m_tempBufferPos)
      {
        //We have scanned all of the data in m_tempBuffer,
        //so continue search from the start of pData buffer.
        syncOffset = 0;
      }
      else
      {
        //not enough data left to continue scanning
        return syncErrors;
      }
    }
  }

  while (nDataLen > (syncOffset + m_packet_len)) //minimum of m_packet_len+1 bytes available
  {
    if (!goodPacket && (syncOffset > (m_packet_len * 8)) )
    {
      //No sync - abandon the buffer
      //Reset();
      return syncErrors;
    }
    
    if ((pData[syncOffset] == TS_PACKET_SYNC) &&
        (pData[syncOffset + m_packet_len]==TS_PACKET_SYNC))
    {
      syncOffset += m_packet_len;
      goodPacket = true;
      m_bFirstSynced = true; 
    }
    else
    {
      syncOffset++;
      if (m_bFirstSynced)
      {
        syncErrors++;
      }
    }
  }

  return syncErrors;
}



void CPacketSync::OnTsPacket(byte* tsPacket)
{
}

void CPacketSync::OnTsPacket(byte* tsPacket, int bufferOffset, int bufferLength)
{
}
