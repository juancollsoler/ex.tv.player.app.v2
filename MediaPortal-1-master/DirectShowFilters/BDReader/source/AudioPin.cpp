/*
 *  Copyright (C) 2005-2011 Team MediaPortal
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

#pragma warning(disable:4996)
#pragma warning(disable:4995)

#include "StdAfx.h"
#include <afx.h>
#include <afxwin.h>

#include <streams.h>
#include "bdreader.h"
#include "audiopin.h"
#include "videopin.h"
#include "mediaformats.h"

// For more details for memory leak detection see the alloctracing.h header
#include "..\..\alloctracing.h"

#define LPCM_HEADER_SIZE 4

extern void LogDebug(const char *fmt, ...);
extern void SetThreadName(DWORD dwThreadID, char* threadName);

CAudioPin::CAudioPin(LPUNKNOWN pUnk, CBDReaderFilter* pFilter, HRESULT* phr, CCritSec* pSection, CDeMultiplexer& pDemux) :
  CSourceStream(NAME("pinAudio"), phr, pFilter, L"Audio"),
  m_pFilter(pFilter),
  m_section(pSection),
  m_demux(pDemux),
  CSourceSeeking(NAME("pinAudio"), pUnk, phr, pSection),
  m_pPinConnection(NULL),
  m_pReceiver(NULL),
  m_pCachedBuffer(NULL),
  m_bFlushing(false),
  m_bSeekDone(true),
  m_bResetToLibSeek(false),
  m_bDiscontinuity(false),
  m_bUsePCM(false),
  m_bZeroTimeStream(false),
  m_rtStreamTimeOffset(0)
{
  m_bConnected = false;
  m_rtStart = 0;
  m_dwSeekingCaps =
    AM_SEEKING_CanSeekAbsolute  |
    AM_SEEKING_CanSeekForwards  |
    AM_SEEKING_CanSeekBackwards |
    AM_SEEKING_CanGetStopPos  |
    AM_SEEKING_CanGetDuration |
    //AM_SEEKING_CanGetCurrentPos |
    AM_SEEKING_Source;

  m_eFlushStart = new CAMEvent(true);
}

CAudioPin::~CAudioPin()
{
  if (m_eFlushStart)
  {
    m_eFlushStart->Set();
    delete m_eFlushStart;
  }

  if (m_demux.m_eAudioClipSeen)
    m_demux.m_eAudioClipSeen->Set();

  delete m_pCachedBuffer;
}

STDMETHODIMP CAudioPin::NonDelegatingQueryInterface(REFIID riid, void** ppv)
{
  if (riid == IID_IMediaSeeking)
    return CSourceSeeking::NonDelegatingQueryInterface(riid, ppv);
  if (riid == IID_IMediaPosition)
    return CSourceSeeking::NonDelegatingQueryInterface(riid, ppv);

  return CSourceStream::NonDelegatingQueryInterface(riid, ppv);
}

HRESULT CAudioPin::CheckMediaType(const CMediaType* pmt)
{
  CAutoLock lock(m_pFilter->pStateLock());

  CMediaType mt;
  GetMediaTypeInternal(&mt);

  if (pmt->subtype == MEDIASUBTYPE_BD_LPCM_AUDIO)
    mt.subtype = MEDIASUBTYPE_BD_LPCM_AUDIO;

  if (mt == *pmt) 
    return NOERROR;

  return E_FAIL;  
}

HRESULT CAudioPin::GetMediaType(int iPosition, CMediaType* pMediaType)
{
  CAutoLock lock(m_pFilter->pStateLock());

  CMediaType mt;
  GetMediaTypeInternal(&mt);

  if (mt.subtype == MEDIASUBTYPE_PCM && iPosition == 0)
  {
    mt.subtype = MEDIASUBTYPE_BD_LPCM_AUDIO;
    *pMediaType = mt;
  }
  else if ((mt.subtype == MEDIASUBTYPE_PCM && iPosition == 1) || iPosition == 0)
    *pMediaType = mt;
  else
    return VFW_S_NO_MORE_ITEMS;

  return S_OK;
}

HRESULT CAudioPin::GetMediaTypeInternal(CMediaType *pmt)
{
  if (m_mt.formattype == GUID_NULL)
    *pmt = m_mtInitial;
  else
    *pmt = m_mt;

  return S_OK;
}

HRESULT CAudioPin::CheckConnect(IPin *pReceivePin)
{
  //LogDebug("aud:CheckConnect()");
  return CBaseOutputPin::CheckConnect(pReceivePin);
}

HRESULT CAudioPin::DecideBufferSize(IMemAllocator *pAlloc, ALLOCATOR_PROPERTIES *pRequest)
{
  HRESULT hr;
  CheckPointer(pAlloc, E_POINTER);
  CheckPointer(pRequest, E_POINTER);

  if (pRequest->cBuffers == 0)
    pRequest->cBuffers = 30;

  pRequest->cbBuffer = MAX_BUFFER_SIZE;

  ALLOCATOR_PROPERTIES Actual;
  hr = pAlloc->SetProperties(pRequest, &Actual);

  if (FAILED(hr))
    return hr;

  if (Actual.cbBuffer < pRequest->cbBuffer)
    return E_FAIL;

  return S_OK;
}

HRESULT CAudioPin::CompleteConnect(IPin *pReceivePin)
{
  HRESULT hr = CBaseOutputPin::CompleteConnect(pReceivePin);
  if (SUCCEEDED(hr))
  {
    LogDebug("aud:CompleteConnect() done");
    m_bConnected = true;
  }
  else
  {
    LogDebug("aud:CompleteConnect() failed:%x", hr);
    return hr;
  }

  REFERENCE_TIME refTime;
  m_pFilter->GetDuration(&refTime);
  m_rtDuration = CRefTime(refTime);
  
  pReceivePin->QueryInterface(IID_IPinConnection, (void**)&m_pPinConnection);
  m_pReceiver = pReceivePin;

  return hr;
}

HRESULT CAudioPin::BreakConnect()
{
  m_bConnected = false;
  return CSourceStream::BreakConnect();
}

DWORD CAudioPin::ThreadProc()
{
  SetThreadName(-1, "BDReader_AUDIO");
  return __super::ThreadProc();
}

void CAudioPin::SetInitialMediaType(const CMediaType* pmt)
{
  m_mtInitial = *pmt;
}

HRESULT CAudioPin::DoBufferProcessingLoop()
{
  Command com;
  OnThreadStartPlay();

  do 
  {
    while (!CheckRequest(&com)) 
    {
      IMediaSample* pSample;

      HRESULT hr = GetDeliveryBuffer(&pSample, NULL, NULL, 0);
      if (FAILED(hr)) 
      {
        Sleep(1);
        continue;	// go round again. Perhaps the error will go away
        // or the allocator is decommited & we will be asked to
        // exit soon.
      }

      // Virtual function user will override.
      hr = FillBuffer(pSample);

      if (hr == S_OK) 
      {
        hr = Deliver(pSample);     
        pSample->Release();

        // downstream filter returns S_FALSE if it wants us to
        // stop or an error if it's reporting an error.
        if (hr != S_OK)
        {
          DbgLog((LOG_TRACE, 2, TEXT("Deliver() returned %08x; stopping"), hr));
          // Delivery thread will be stalled instead of stopping
          //return S_OK;
        }
      }
      else if (hr == ERROR_NO_DATA)
      {
        pSample->Release(); 
      }
      else if (hr == S_FALSE) 
      {
        // derived class wants us to stop pushing data
        pSample->Release();
        LogDebug("vid: DeliverEndOfStream - downstream filter rejected the sample");
        DeliverEndOfStream();
        return S_OK;
      } 
      else 
      {
        // derived class encountered an error
        pSample->Release();
        DbgLog((LOG_ERROR, 1, TEXT("Error %08lX from FillBuffer!!!"), hr));
        DeliverEndOfStream();
        LogDebug("vid: DeliverEndOfStream - unhandled error in ::FillBuffer");
        m_pFilter->NotifyEvent(EC_ERRORABORT, hr, 0);
        return hr;
      }
     // all paths release the sample
    }
    // For all commands sent to us there must be a Reply call!
	  if (com == CMD_RUN || com == CMD_PAUSE) 
    {
      Reply(NOERROR);
	  } 
    else if (com != CMD_STOP) 
    {
      Reply((DWORD) E_UNEXPECTED);
      DbgLog((LOG_ERROR, 1, TEXT("Unexpected command!!!")));
	  }
  } while (com != CMD_STOP);

  return S_FALSE;
}

HRESULT CAudioPin::FillBuffer(IMediaSample *pSample)
{
  try
  {
    Packet* buffer = NULL;

    do
    {
      if (m_demux.m_bAudioWaitForSeek)
      {
        m_demux.m_bAudioWaitForSeek = false;
        m_bSeekDone = false;
      }

      if (!m_bSeekDone || m_pFilter->IsStopping() || m_bFlushing || m_demux.IsMediaChanging() || m_demux.m_bRebuildOngoing || 
        m_demux.m_eAudioClipSeen->Check())
      {
        Sleep(1);
        return ERROR_NO_DATA;
      }

      if (m_pCachedBuffer)
      {
        LogDebug("aud: cached fetch %6.3f clip: %d playlist: %d", m_pCachedBuffer->rtStart / 10000000.0, m_pCachedBuffer->nClipNumber, m_pCachedBuffer->nPlaylist);
        buffer = m_pCachedBuffer;
        m_pCachedBuffer = NULL;
      }
      else
        buffer = m_demux.GetAudio();

      if (m_demux.EndOfFile())
      {
        LogDebug("aud: set EOF");
        m_demux.m_eAudioClipSeen->Set();
        return S_FALSE;
      }

      if (!buffer)
      {
        Sleep(10);
        return ERROR_NO_DATA;
      }
      else
      {
        bool checkPlaybackState = false;

        {
          CAutoLock lock(m_section);

          if (m_demux.m_bAudioResetStreamPosition)
          {
            m_demux.m_bAudioResetStreamPosition = false;
            m_bZeroTimeStream = true;
          }

          if (buffer->nNewSegment & NS_NEW_CLIP)
          {
            LogDebug("aud: NS_NEW_CLIP pl: %d clip: %d nNewSegment: %d offset: %6.3f rtStart: %6.3f rtPlaylistTime: %6.3f",
              buffer->nPlaylist, buffer->nClipNumber, buffer->nNewSegment, buffer->rtOffset / 10000000.0, buffer->rtStart / 10000000.0, buffer->rtPlaylistTime / 10000000.0);

            checkPlaybackState = true;
            m_demux.m_eAudioClipSeen->Set();
          }

          if (m_bResetToLibSeek)
          {
            m_demux.m_eAudioClipSeen->Set();
            checkPlaybackState = true;
            m_bResetToLibSeek = false;
          }

          // Do not convert LPCM to PCM if audio decoder supports LPCM (LAV audio decoder style)
          if (!m_bUsePCM && buffer->pmt && buffer->pmt->subtype == MEDIASUBTYPE_PCM)
            buffer->pmt->subtype = MEDIASUBTYPE_BD_LPCM_AUDIO;

          if (buffer->pmt && m_mt != *buffer->pmt && !(buffer->nNewSegment & NS_NEW_CLIP))
          {
            HRESULT hrAccept = S_FALSE;
            LogMediaType(buffer->pmt);

            if (m_pPinConnection && false) // TODO - DS audio renderer seems to be only one that supports this
              hrAccept = m_pPinConnection->DynamicQueryAccept(buffer->pmt);
            else if (m_pReceiver)
            {
              //LogDebug("aud: DynamicQueryAccept - not avail");
              GUID guid = buffer->pmt->subtype;
              if (buffer->pmt->subtype == MEDIASUBTYPE_PCM)
              {
                buffer->pmt->subtype = MEDIASUBTYPE_BD_LPCM_AUDIO;
                hrAccept = m_pReceiver->QueryAccept(buffer->pmt);
              }
              
              if (hrAccept != S_OK)
              {
                buffer->pmt->subtype = guid;
                hrAccept = m_pReceiver->QueryAccept(buffer->pmt);
                m_bUsePCM = true;
              }
              else
                m_bUsePCM = false;
            }

            if (hrAccept != S_OK)
            {
              CMediaType mt(*buffer->pmt);
              SetMediaType(&mt);

              LogDebug("aud: graph rebuilding required");

              m_demux.m_bAudioRequiresRebuild = true;
              checkPlaybackState = true;

              DeliverEndOfStream();
            }
            else
            {
              LogDebug("aud: format change accepted");
              CMediaType mt(*buffer->pmt);
              SetMediaType(&mt);
              pSample->SetMediaType(&mt);
              m_pCachedBuffer = buffer;

              return ERROR_NO_DATA;
            }
          }
        } // lock ends

        if (checkPlaybackState)
        {
          m_pCachedBuffer = buffer;
          LogDebug("aud: cached push  %6.3f clip: %d playlist: %d", m_pCachedBuffer->rtStart / 10000000.0, m_pCachedBuffer->nClipNumber, m_pCachedBuffer->nPlaylist);
          
          if (buffer->pmt && m_mt != *buffer->pmt && !(buffer->nNewSegment & NS_NEW_CLIP))
          {
            CMediaType mt(*buffer->pmt);
            SetMediaType(&mt);
          }

          m_pCachedBuffer->nNewSegment = 0;

          return ERROR_NO_DATA;
        }
  
        bool hasTimestamp = buffer->rtStart != Packet::INVALID_TIME;

        REFERENCE_TIME rtCorrectedStartTime = 0;
        REFERENCE_TIME rtCorrectedStopTime = 0;

        if (hasTimestamp && m_dRateSeeking == 1.0)
        {
          bool setPMT = false;

          if (m_bDiscontinuity || buffer->bDiscontinuity)
          {
            LogDebug("aud: set discontinuity");
            pSample->SetDiscontinuity(true);
            setPMT = true;
            m_bDiscontinuity = false;
          }

          if (buffer->pmt || setPMT)
          {
            LogDebug("aud: set PMT");
            pSample->SetMediaType(buffer->pmt);
            m_bDiscontinuity = false;
          }

          if (hasTimestamp)
          {
            if (m_bZeroTimeStream)
            {
              m_rtStreamTimeOffset = buffer->rtStart - buffer->rtClipStartTime;
              m_bZeroTimeStream=false;
            }

            pSample->SetSyncPoint(true); // allow all packets to be seeking targets
            rtCorrectedStartTime = buffer->rtStart - m_rtStreamTimeOffset + m_demux.m_rtStallTime;
            rtCorrectedStopTime = buffer->rtStop - m_rtStreamTimeOffset + m_demux.m_rtStallTime;

            if (rtCorrectedStartTime < 0)
            {
              LogDebug("aud: dropping negative %6.3f corr %6.3f playlist time %6.3f clip: %d playlist: %d", 
                buffer->rtStart / 10000000.0, rtCorrectedStartTime / 10000000.0,
                buffer->rtPlaylistTime / 10000000.0, buffer->nClipNumber, buffer->nPlaylist);

              delete buffer;
              return ERROR_NO_DATA;
            }

            pSample->SetTime(&rtCorrectedStartTime, &rtCorrectedStopTime);
          }
          else
          {
            // Buffer has no timestamp
            pSample->SetTime(NULL, NULL);
            pSample->SetSyncPoint(false);
          }

          {
            CAutoLock lock(&m_csDeliver);

            if (!m_bFlushing)
            {
              BYTE* pSampleBuffer = NULL;
              pSample->SetActualDataLength(buffer->GetDataSize());
              pSample->GetPointer(&pSampleBuffer);
              memcpy(pSampleBuffer, buffer->GetData(), buffer->GetDataSize());

#ifdef LOG_AUDIO_PIN_SAMPLES
             LogDebug("aud: %6.3f corr %6.3f Playlist time %6.3f clip: %d playlist: %d", buffer->rtStart / 10000000.0, rtCorrectedStartTime / 10000000.0,
                buffer->rtPlaylistTime / 10000000.0, buffer->nClipNumber, buffer->nPlaylist);
#endif
            }
            else
            {
              LogDebug("aud: dropped sample as flush is active!");
              delete buffer;
              return ERROR_NO_DATA;
            }
          }

          delete buffer;
        }
        else
        { // Buffer was not displayed because it was out of date, search for next.
          delete buffer;
          buffer = NULL;
        }
      }
    } while (!buffer);
    return NOERROR;
  }

  // Should we return something else than NOERROR when hitting an exception?
  catch (int e)
  {
    LogDebug("aud: FillBuffer exception %d", e);
  }
  catch (...)
  {
    LogDebug("aud: FillBuffer exception ...");
  }

  return NOERROR;
}

bool CAudioPin::IsConnected()
{
  return m_bConnected;
}

HRESULT CAudioPin::OnThreadStartPlay()
{
  {
    CAutoLock lock(CSourceSeeking::m_pLock);
    m_bDiscontinuity = true;

    if (m_demux.m_eAudioClipSeen)
      m_demux.m_eAudioClipSeen->Reset();
  }

  return S_OK;
}

HRESULT CAudioPin::OnThreadDestroy()
{
  // Make sure video pin is not waiting for us
  if (m_demux.m_eAudioClipSeen)
    m_demux.m_eAudioClipSeen->Set();

  return S_OK;
}

HRESULT CAudioPin::DeliverBeginFlush()
{
  CAutoLock lock(&m_csDeliver);

  m_eFlushStart->Set();
  m_bFlushing = true;
  m_bSeekDone = false;
  HRESULT hr = __super::DeliverBeginFlush();
  LogDebug("aud: DeliverBeginFlush - hr: %08lX", hr);

  if (hr != S_OK)
  {
    m_bFlushing = true;
    m_bSeekDone = true;
  }

  return hr;
}

HRESULT CAudioPin::DeliverEndFlush()
{
  HRESULT hr = __super::DeliverEndFlush();
  LogDebug("aud: DeliverEndFlush - hr: %08lX", hr);
  m_bZeroTimeStream = true;
  m_demux.m_eAudioClipSeen->Reset();
  m_bFlushing = false;

  return hr;
}

HRESULT CAudioPin::DeliverNewSegment(REFERENCE_TIME tStart, REFERENCE_TIME tStop, double dRate, bool doFakeSeek)
{
  if (m_bFlushing || !ThreadExists())
  {
    m_bSeekDone = true;
    return S_FALSE;
  }

  LogDebug("aud: DeliverNewSegment start: %6.3f stop: %6.3f rate: %6.3f", tStart / 10000000.0, tStop / 10000000.0, dRate);
  m_rtStart = tStart;

  HRESULT hr = __super::DeliverNewSegment(tStart, tStop, dRate);
  if (FAILED(hr))
    LogDebug("aud: DeliverNewSegment - error: %08lX", hr);

  m_bSeekDone = true;
  m_bResetToLibSeek = doFakeSeek;

  return hr;
}

STDMETHODIMP CAudioPin::SetPositions(LONGLONG* pCurrent, DWORD CurrentFlags, LONGLONG* pStop, DWORD StopFlags)
{
  return m_pFilter->SetPositionsInternal(this, pCurrent, CurrentFlags, pStop, StopFlags);
}

STDMETHODIMP CAudioPin::GetAvailable(LONGLONG* pEarliest, LONGLONG* pLatest )
{
  //LogDebug("aud: GetAvailable");
  return CSourceSeeking::GetAvailable(pEarliest, pLatest);
}

STDMETHODIMP CAudioPin::GetDuration(LONGLONG *pDuration)
{
  //LogDebug("aud:GetDuration");
  REFERENCE_TIME refTime;
  m_pFilter->GetDuration(&refTime);
  m_rtDuration = CRefTime(refTime);

  if (pDuration)
    return CSourceSeeking::GetDuration(pDuration);

  return S_OK;
}

HRESULT CAudioPin::ChangeStart()
{
  return S_OK;
}

HRESULT CAudioPin::ChangeStop()
{
  return S_OK;
}

HRESULT CAudioPin::ChangeRate()
{
  return S_OK;
}

STDMETHODIMP CAudioPin::SetRate(double dRate)
{
  if (dRate != 1.0)
    return VFW_E_UNSUPPORTED_AUDIO;  
  else
    return S_OK;
}

STDMETHODIMP CAudioPin::GetCurrentPosition(LONGLONG* pCurrent)
{
  //LogDebug("aud: GetCurrentPosition");
  return E_NOTIMPL;//CSourceSeeking::GetCurrentPosition(pCurrent);
}

STDMETHODIMP CAudioPin::Notify(IBaseFilter* pSender, Quality q)
{
  return E_NOTIMPL;
}

void CAudioPin::LogMediaType(AM_MEDIA_TYPE* pmt)
{
  if (!pmt)
    LogDebug("aud: missing audio PMT");
  else
  {
    LogDebug("aud: format %d {%08x-%04x-%04x-%02X%02X-%02X%02X%02X%02X%02X%02X}", pmt->cbFormat,
      pmt->subtype.Data1, pmt->subtype.Data2, pmt->subtype.Data3,
      pmt->subtype.Data4[0], pmt->subtype.Data4[1], pmt->subtype.Data4[2],
      pmt->subtype.Data4[3], pmt->subtype.Data4[4], pmt->subtype.Data4[5], 
      pmt->subtype.Data4[6], pmt->subtype.Data4[7]);
  }
}