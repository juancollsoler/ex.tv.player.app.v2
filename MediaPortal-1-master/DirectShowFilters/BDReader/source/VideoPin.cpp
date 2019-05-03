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

#include <streams.h>
#include "bdreader.h"
#include "AudioPin.h"
#include "Videopin.h"
#include "mediaformats.h"
#include <wmcodecdsp.h>

// For more details for memory leak detection see the alloctracing.h header
#include "..\..\alloctracing.h"

extern void LogDebug(const char *fmt, ...);
extern void SetThreadName(DWORD dwThreadID, char* threadName);

#define HALF_SECOND 5000000LL

void LogMediaSample(IMediaSample * pSample, int iFrameNumber)
{
  char filename[1024]="c:\\BDReaderAnalysis\\BDReader\\Log.log";
  char frameFilename[1024];
  sprintf(frameFilename,"c:\\BDReaderAnalysis\\BDReader\\Frames\\%d.log",iFrameNumber);
  FILE* fp = fopen(filename,"a+");
  long iSampleSize = 0;

  if (fp)
  {
    REFERENCE_TIME rtStart, rtStop;
    pSample->GetMediaTime(&rtStart, &rtStop);
    AM_MEDIA_TYPE * pMediaType;
    pSample->GetMediaType(&pMediaType);
    REFERENCE_TIME rtTimeStart,rtTimeStop;
    pSample->GetTime(&rtTimeStart,&rtTimeStop);
    bool bDiscontinuity = pSample->IsDiscontinuity() == S_OK;
    bool bPreRoll = pSample->IsPreroll() == S_OK;
    bool bSyncPoint = pSample->IsSyncPoint() == S_OK;
    iSampleSize = pSample->GetActualDataLength();
    // We'll leave Mediatype for now
    fprintf(fp,"%d %I64d %I64d %I64d %I64d %d %d %d %d\n",
      iFrameNumber, rtStart, rtStop, rtTimeStart, rtTimeStop, bDiscontinuity, bPreRoll, bSyncPoint, iSampleSize);
    fclose(fp);
  }

  fp = fopen(frameFilename,"w+");
  if (fp)
  {
    BYTE * pData;
    pSample->GetPointer(&pData);
    for (int i= 0;i<(iSampleSize+79)/80;i++)
    {
      for (int j=0;j<80;j++)
      {
        if (i*80+j<iSampleSize)
          fprintf(fp,"%02X",pData[i*16+j]);
      }
      fprintf(fp,"\n");
    }
    fclose(fp);  
  }
};

CVideoPin::CVideoPin(LPUNKNOWN pUnk, CBDReaderFilter* pFilter, HRESULT* phr, CCritSec* pSection, CDeMultiplexer& pDemux) :
  CSourceStream(NAME("pinVideo"), phr, pFilter, L"Video"),
  m_pFilter(pFilter),
  m_section(pSection),
  m_demux(pDemux),
  CSourceSeeking(NAME("pinVideo"), pUnk, phr, pSection),
  m_pReceiver(NULL),
  m_pCachedBuffer(NULL),
  m_rtStreamOffset(0),
  m_bFlushing(false),
  m_bSeekDone(true),
  m_bDiscontinuity(false),
  m_bZeroTimeStream(false),
  m_bInitDuration(true),
  m_bProvidePMT(false),
  m_bClipEndingNotified(false),
  m_bStopWait(false),
  m_rtStreamTimeOffset(0),
  m_rtTitleDuration(0),
  m_nSampleCounter(0),
  m_bDoFakeSeek(false),
  m_bResetToLibSeek(false),
  m_H264decoder(GUID_NULL),
  m_VC1decoder(GUID_NULL),
  m_MPEG2decoder(GUID_NULL),
  m_VC1Override(GUID_NULL),
  m_currentDecoder(GUID_NULL)
{
  m_rtStart = 0;
  m_bConnected = false;
  m_dwSeekingCaps =
    AM_SEEKING_CanSeekAbsolute  |
    AM_SEEKING_CanSeekForwards  |
    AM_SEEKING_CanSeekBackwards |
    AM_SEEKING_CanGetStopPos  |
    AM_SEEKING_CanGetDuration |
    //AM_SEEKING_CanGetCurrentPos |
    AM_SEEKING_Source;

  m_eFlushStart = new CAMEvent(true);
  m_eSyncClips = new CAMEvent(true);
}

CVideoPin::~CVideoPin()
{
  m_eFlushStart->Set();
  m_eSyncClips->Set();

  delete m_eFlushStart;
  delete m_eSyncClips;
  delete m_pCachedBuffer;
}

bool CVideoPin::IsConnected()
{
  return m_bConnected;
}

STDMETHODIMP CVideoPin::NonDelegatingQueryInterface(REFIID riid, void** ppv)
{
  if (riid == IID_IMediaSeeking)
    return CSourceSeeking::NonDelegatingQueryInterface(riid, ppv);
  if (riid == IID_IMediaPosition)
    return CSourceSeeking::NonDelegatingQueryInterface(riid, ppv);

  return CSourceStream::NonDelegatingQueryInterface(riid, ppv);
}

HRESULT CVideoPin::GetMediaType(CMediaType* pmt)
{
  GetMediaTypeInternal(pmt);

  if (pmt->subtype == FOURCCMap('1CVW') && m_VC1Override != GUID_NULL)
  {
    pmt->subtype = m_VC1Override;
    LogDebug("vid: GetMediaType - force VC-1 GUID");
  }

  return S_OK;
}

HRESULT CVideoPin::GetMediaTypeInternal(CMediaType* pmt)
{
  if (m_mt.formattype == GUID_NULL)
    *pmt = m_mtInitial;
  else
    *pmt = m_mt;

  return S_OK;
}

bool CVideoPin::CompareMediaTypes(AM_MEDIA_TYPE* lhs_pmt, AM_MEDIA_TYPE* rhs_pmt)
{
  return (IsEqualGUID(lhs_pmt->majortype, rhs_pmt->majortype) &&
    IsEqualGUID(lhs_pmt->subtype, rhs_pmt->subtype) &&
    IsEqualGUID(lhs_pmt->formattype, rhs_pmt->formattype) &&
    (lhs_pmt->cbFormat == rhs_pmt->cbFormat) &&
    ((lhs_pmt->cbFormat == 0) ||
      lhs_pmt->pbFormat && rhs_pmt->pbFormat &&
      (memcmp(lhs_pmt->pbFormat, rhs_pmt->pbFormat, lhs_pmt->cbFormat) == 0))); 
}

void CVideoPin::SetInitialMediaType(const CMediaType* pmt)
{
  m_mtInitial = *pmt;
}

void CVideoPin::SetVideoDecoder(int format, GUID* decoder)
{
  AM_MEDIA_TYPE tmp;
  tmp.cbFormat = 0;

  if (format == BLURAY_STREAM_TYPE_VIDEO_H264)
  {
    m_H264decoder = tmp.subtype = *decoder;
    LogDebug("vid: SetVideoDecoder for H264");
    LogMediaType(&tmp);
  }
  else if (format == BLURAY_STREAM_TYPE_VIDEO_VC1)
  {
    m_VC1decoder = tmp.subtype = *decoder;
    LogDebug("vid: SetVideoDecoder for VC1");
    LogMediaType(&tmp);
  }
  else if (format == BLURAY_STREAM_TYPE_VIDEO_MPEG2)
  {
    m_MPEG2decoder = tmp.subtype = *decoder;
    LogDebug("vid: SetVideoDecoder for MPEG2");
    LogMediaType(&tmp);
  }
  else
  {
    LogDebug("vid: SetVideoDecoder - trying to set a decoder for invalid format %d", format);
    return;
  }
}

void CVideoPin::SetVC1Override(GUID* subtype)
{
  AM_MEDIA_TYPE tmp;
  tmp.cbFormat = 0;
  m_VC1Override = tmp.subtype = *subtype; 
  
  LogDebug("vid: SetVC1Override");
  LogMediaType(&tmp);
}

bool CVideoPin::CheckVideoFormat(GUID* pFormat, CLSID* pDecoder)
{
  GUID* decoder = NULL;

  if (IsEqualGUID(*pFormat, MPG4_SubType))
    decoder = &m_H264decoder;
  else if (IsEqualGUID(*pFormat, VC1_SubType))
    decoder = &m_VC1decoder;
  else if (IsEqualGUID(*pFormat, MEDIASUBTYPE_MPEG2_VIDEO))
    decoder = &m_MPEG2decoder;
  else
    decoder = &m_MPEG2decoder; // MPEG1 uses the same decoder as MPEG2

  // When no decoder has been assigned for a specific format
  // assume the current decoder can be used
  if (IsEqualGUID(*decoder, GUID_NULL))
    return true;

  if (IsEqualGUID(*decoder, *pDecoder))
    return true;
  else
    return false;
}

HRESULT CVideoPin::DecideBufferSize(IMemAllocator* pAlloc, ALLOCATOR_PROPERTIES* pRequest)
{
  CheckPointer(pAlloc, E_POINTER);
  CheckPointer(pRequest, E_POINTER);

  if (pRequest->cBuffers == 0)
    pRequest->cBuffers = 1;

  // Would be better if this would be allocated on sample basis
  pRequest->cbBuffer = 0x1000000;

  ALLOCATOR_PROPERTIES Actual;
  HRESULT hr = pAlloc->SetProperties(pRequest, &Actual);
  if (FAILED(hr))
    return hr;

  if (Actual.cbBuffer < pRequest->cbBuffer)
  {
    LogDebug("vid:DecideBufferSize - failed to get buffer");
    return E_FAIL;
  }

  return S_OK;
}

HRESULT CVideoPin::CheckConnect(IPin* pReceivePin)
{
  return CBaseOutputPin::CheckConnect(pReceivePin);
}

HRESULT CVideoPin::CompleteConnect(IPin* pReceivePin)
{
  HRESULT hr = CBaseOutputPin::CompleteConnect(pReceivePin);
  if (SUCCEEDED(hr))
  {
    LogDebug("vid:CompleteConnect() done");
    m_bConnected = true;
    m_currentDecoder = GetDecoderCLSID(pReceivePin);
  }
  else
  {
    LogDebug("vid:CompleteConnect() failed:%x", hr);
    m_currentDecoder = GUID_NULL;
    return hr;
  }

  REFERENCE_TIME refTime;
  m_pFilter->GetDuration(&refTime);
  m_rtDuration = CRefTime(refTime);

  m_pReceiver = pReceivePin;

  return hr;
}

HRESULT CVideoPin::BreakConnect()
{
  m_bConnected = false;
  return CSourceStream::BreakConnect();
}

DWORD CVideoPin::ThreadProc()
{
  SetThreadName(-1, "BDReader_VIDEO");
  return __super::ThreadProc();
}

void CVideoPin::StopWait()
{
  m_bStopWait = true;

  if (m_eFlushStart)
    m_eFlushStart->Set();
}

void CVideoPin::SyncClipBoundary()
{
  if (m_eSyncClips)
    m_eSyncClips->Set();
}

HRESULT CVideoPin::DoBufferProcessingLoop()
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

void CVideoPin::CheckStall()
{
  if (m_demux.m_bTitleChanged)
  {
    m_demux.m_bTitleChanged = false;

    REFERENCE_TIME rtTime;
    REFERENCE_TIME rtStallDuration;
    m_pFilter->GetTime(&rtTime);

    rtStallDuration = rtTime - m_demux.m_rtTitleChangeStarted;

    if (rtStallDuration > HALF_SECOND)
      m_bDoFakeSeek = true;

    LogDebug("vid: stall time: %6.3f total: %6.3f", rtStallDuration / 10000000.0, m_demux.m_rtStallTime / 10000000.0);
  }
}

void CVideoPin::CheckPlaybackState()
{
  if (m_demux.m_bVideoClipSeen)
  {
    HANDLE handles[2] = { *m_demux.m_eAudioClipSeen, *m_eSyncClips };
    
    DWORD error = 0;
    DWORD wait = WaitForMultipleObjects(2, handles, false, INFINITE);

    if (wait == WAIT_OBJECT_0 + 1)
      return;
    else if (wait == WAIT_FAILED)
    {
      error = GetLastError();
      LogDebug("vid: WaitForMultipleObjects failed: %d", error);
    }

    CheckStall();

    if (m_demux.m_bVideoRequiresRebuild || m_demux.m_bAudioRequiresRebuild)
    {
      LogDebug("vid: REBUILD");
      m_demux.m_bVideoRequiresRebuild = false;
      m_demux.m_bAudioRequiresRebuild = false;
      m_pFilter->IssueCommand(REBUILD, m_rtStreamOffset);
      m_demux.m_bRebuildOngoing = true;
    }
    else if (!m_bStopWait && m_bDoFakeSeek)
    {
      LogDebug("vid: Request zeroing the stream time");
      m_eFlushStart->Reset();
      m_demux.m_bAudioWaitForSeek = true;
      m_pFilter->IssueCommand(SEEK, m_rtStreamOffset);
      m_eFlushStart->Wait();
    }

    m_bStopWait = m_bDoFakeSeek = false;

    LogDebug("vid: reseting audio and video seen indicators");

    m_demux.m_eAudioClipSeen->Reset();
    m_demux.m_bVideoClipSeen = false;
  }
  else
  {
    // Audio stream requires a rebuild (in middle of the clip - user initiated)
    if (!m_demux.m_eAudioClipSeen->Check() && !m_demux.m_bVideoClipSeen && m_demux.m_bAudioRequiresRebuild)
    {
      m_demux.m_bAudioRequiresRebuild = false;
      m_demux.m_eAudioClipSeen->Reset();

      LogDebug("vid: REBUILD for audio - keep stream position");
      m_pFilter->IssueCommand(REBUILD, -1);
    }
    else
      Sleep(5);        
  }
}

HRESULT CVideoPin::FillBuffer(IMediaSample* pSample)
{
  try
  {
    Packet* buffer = NULL;

    do
    {
      if (m_pFilter->IsStopping() || m_demux.IsMediaChanging() || m_bFlushing || !m_bSeekDone || m_demux.m_bRebuildOngoing)
      {
        Sleep(1);
        return ERROR_NO_DATA;
      }

      if (m_demux.EndOfFile())
      {
        LogDebug("vid: set EOF");
        return S_FALSE;
      }

      if (m_demux.m_bVideoClipSeen || m_demux.m_bAudioRequiresRebuild && !m_demux.m_bVideoClipSeen && !m_demux.m_eAudioClipSeen->Check())
      {
        CheckPlaybackState();
        return ERROR_NO_DATA;
      }

      if (m_pCachedBuffer)
      {
        LogDebug("vid: cached fetch %6.3f clip: %d playlist: %d", m_pCachedBuffer->rtStart / 10000000.0, m_pCachedBuffer->nClipNumber, m_pCachedBuffer->nPlaylist);
        buffer = m_pCachedBuffer;
        m_pCachedBuffer = NULL;
        buffer->bDiscontinuity = true;
        
        if (m_bProvidePMT)
        {
          CMediaType mt(*buffer->pmt);
          SetMediaType(&mt);
          pSample->SetMediaType(&mt);
          m_bProvidePMT = false;
        }
      }
      else
        buffer = m_demux.GetVideo();

      if (!buffer)
      {
        if (m_nSampleCounter == 0)
          Sleep(10);
        else 
        {
          if (!m_bClipEndingNotified)
          {
            // Deliver end of stream notification to flush the video decoder.
            // This should only happen when the stream enters into paused state
            LogDebug("vid: FillBuffer - DeliverEndOfStream");
            DeliverEndOfStream();
            m_bClipEndingNotified = true;
          }
          else
            Sleep(10);

          return ERROR_NO_DATA;
        }
      }
      else
      {
        bool checkPlaybackState = false;

        {
          CAutoLock lock(m_section);

          if (buffer->nNewSegment > 0)
          {
            if (buffer->nNewSegment & NS_NEW_CLIP)
            {
              LogDebug("vid: NS_NEW_CLIP pl: %d clip: %d nNewSegment: %d offset: %6.3f rtStart: %6.3f rtPlaylistTime: %6.3f",
                buffer->nPlaylist, buffer->nClipNumber, buffer->nNewSegment, buffer->rtOffset / 10000000.0, buffer->rtStart / 10000000.0, buffer->rtPlaylistTime / 10000000.0);

              m_demux.m_bVideoClipSeen = true;
              checkPlaybackState = true;

              if (m_nSampleCounter > EOS_THRESHOLD)
                m_bClipEndingNotified = false;
              else
              {
                DeliverEndOfStream();
                LogDebug("DeliverEndOFStream - EOS_THRESHOLD");
                m_bClipEndingNotified = true;
              }

              if (buffer->bResuming || buffer->nNewSegment & NS_INTERRUPTED)
              {
                m_bDoFakeSeek = true;
                m_bZeroTimeStream = true;
                m_demux.m_bAudioResetStreamPosition = true;

                m_rtStreamOffset = buffer->rtPlaylistTime;
                m_bInitDuration = true;
              }

              // LAV video decoder requires an end of stream notification to be able to provide complete video frames
              // to downstream filters in a case where we are waiting for the audio pin to see the clip boundary as
              // we cannot provide yet the next clip's PMT downstream since audio stream could require a rebuild
              if (m_currentDecoder == CLSID_LAVVideo && !m_bClipEndingNotified && (buffer->nNewSegment & NS_NEW_PLAYLIST))
              {
                LogDebug("DeliverEndOFStream LAV Only for audio pin wait (%d,%d)", buffer->nPlaylist, buffer->nClipNumber);
                DeliverEndOfStream();
                m_bClipEndingNotified = true;
              }
            }
            
            if (m_bResetToLibSeek)
            {
              m_demux.m_bVideoClipSeen = true;
              checkPlaybackState = true;
              m_bDoFakeSeek = true;
              m_bResetToLibSeek = false;
            }

            if (buffer->nNewSegment & NS_STREAM_RESET)
            {
              m_rtStreamOffset = buffer->rtPlaylistTime;
              m_bInitDuration = true;
            }
          }

          if (buffer->pmt)
          {
            GUID subtype = subtype = buffer->pmt->subtype;

            if (buffer->pmt->subtype == FOURCCMap('1CVW') && m_VC1Override != GUID_NULL)
            {
              buffer->pmt->subtype = m_VC1Override;
              LogDebug("vid: FillBuffer - force VC-1 GUID");
            }

            if (!CompareMediaTypes(buffer->pmt, &m_mt))
            {
              LogMediaType(buffer->pmt);
            
              HRESULT hrAccept = S_FALSE;
              m_bProvidePMT = true;

              if (m_pReceiver && CheckVideoFormat(&buffer->pmt->subtype, &m_currentDecoder))
              {
                // Currently only LAV Video Decoder seems to be capable of handling
                // dynamic format changes. Even LAV doesn't handle it perfectly. Sometimes
                // the change results in a black screen (LAV v0.68.1). We disable internal
                // handling to avoid such problems *and* enable external handlers to update
                // the refresh rate.
                //if (m_currentDecoder == CLSID_LAVVideo)
                //  hrAccept = m_pReceiver->QueryAccept(buffer->pmt);
              }

              if (hrAccept != S_OK)
              {
                CMediaType mt(*buffer->pmt);
                SetMediaType(&mt);

                LogDebug("vid: graph rebuilding required");

                m_demux.m_bVideoRequiresRebuild = true;
                m_bZeroTimeStream = true;
                checkPlaybackState = true;

                //LogDebug("DeliverEndOFStream for rebuild (%d,%d)", buffer->nPlaylist, buffer->nClipNumber);
                //DeliverEndOfStream();
              }
              else
              {
                LogDebug("vid: format change accepted");
                CMediaType mt(*buffer->pmt);
                SetMediaType(&mt);
                pSample->SetMediaType(&mt);

                buffer->nNewSegment = 0;
                m_pCachedBuffer = buffer;
				
                //if (m_currentDecoder == CLSID_LAVVideo)
                //{
                //  LogDebug("DeliverEndOFStream LAV Only (%d,%d)", buffer->nPlaylist, buffer->nClipNumber);
                //  DeliverEndOfStream();
                //}

                return ERROR_NO_DATA;
              }
            } // comparemediatypes
          }
        } // lock ends

        m_rtTitleDuration = buffer->rtTitleDuration;

        if (checkPlaybackState)
        {
          buffer->nNewSegment = 0;
          m_pCachedBuffer = buffer;

          CheckPlaybackState();

          LogDebug("vid: cached push  %6.3f clip: %d playlist: %d", m_pCachedBuffer->rtStart / 10000000.0, m_pCachedBuffer->nClipNumber, m_pCachedBuffer->nPlaylist);

          return ERROR_NO_DATA;
        }

        bool hasTimestamp = buffer->rtStart != Packet::INVALID_TIME;
        REFERENCE_TIME rtCorrectedStartTime = 0;
        REFERENCE_TIME rtCorrectedStopTime = 0;

        if (hasTimestamp)
        {
          if (m_bZeroTimeStream)
          {
            m_rtStreamTimeOffset = buffer->rtStart - buffer->rtClipStartTime;
            m_bZeroTimeStream = false;
          }

          if (m_bDiscontinuity || buffer->bDiscontinuity)
          {
            LogDebug("vid: set discontinuity");
            pSample->SetDiscontinuity(true);
            pSample->SetMediaType(buffer->pmt);
            m_bDiscontinuity = false;
          }

          rtCorrectedStartTime = buffer->rtStart - m_rtStreamTimeOffset + m_demux.m_rtStallTime;
          rtCorrectedStopTime = buffer->rtStop - m_rtStreamTimeOffset + m_demux.m_rtStallTime;

          pSample->SetTime(&rtCorrectedStartTime, &rtCorrectedStopTime);

          if (m_bInitDuration)
          {
            m_pFilter->SetTitleDuration(m_rtTitleDuration);
            m_pFilter->ResetPlaybackOffset(m_rtStreamOffset);
            m_bInitDuration = false;
          }

          m_pFilter->OnPlaybackPositionChange();
        }
        else // Buffer has no timestamp
          pSample->SetTime(NULL, NULL);

        pSample->SetSyncPoint(buffer->bSyncPoint);

        {
          CAutoLock lock(&m_csDeliver);

          if (!m_bFlushing)
          {
            BYTE* pSampleBuffer;
            pSample->SetActualDataLength(buffer->GetDataSize());
            pSample->GetPointer(&pSampleBuffer);
            memcpy(pSampleBuffer, buffer->GetData(), buffer->GetDataSize());

            m_nSampleCounter++;

#ifdef LOG_VIDEO_PIN_SAMPLES
            LogDebug("vid: %6.3f corr %6.3f playlist time %6.3f clip: %d playlist: %d size: %d", buffer->rtStart / 10000000.0, rtCorrectedStartTime / 10000000.0, 
              buffer->rtPlaylistTime / 10000000.0, buffer->nClipNumber, buffer->nPlaylist, buffer->GetCount());
#endif
          }
          else
          {
            LogDebug("vid: dropped sample as flush is active!");
            return ERROR_NO_DATA;
          }
        }

        //static int iFrameNumber = 0;
        //LogMediaSample(pSample, iFrameNumber++);

        delete buffer;
      }
    } while (!buffer);
    return NOERROR;
  }

  catch(...)
  {
    LogDebug("vid: FillBuffer exception");
  }

  return S_OK;
}

HRESULT CVideoPin::OnThreadStartPlay()
{
  {
    CAutoLock lock(CSourceSeeking::m_pLock);
    m_bDiscontinuity = true;
    m_nSampleCounter = 0;
    m_bClipEndingNotified = false;
  }

  return S_OK;
}

HRESULT CVideoPin::DeliverBeginFlush()
{
  CAutoLock lock(&m_csDeliver);

  m_eFlushStart->Set();
  m_bFlushing = true;
  m_bSeekDone = false;
  HRESULT hr = __super::DeliverBeginFlush();
  LogDebug("vid: DeliverBeginFlush - hr: %08lX", hr);

  if (hr != S_OK)
  {
    m_bFlushing = true;
    m_bSeekDone = true;
  }

  return hr;
}

HRESULT CVideoPin::DeliverEndFlush()
{
  HRESULT hr = __super::DeliverEndFlush();
  LogDebug("vid: DeliverEndFlush - hr: %08lX", hr);
  m_bZeroTimeStream = true;
  m_demux.m_bVideoClipSeen = false;
  m_bFlushing = false;

  return hr;
}

HRESULT CVideoPin::DeliverNewSegment(REFERENCE_TIME tStart, REFERENCE_TIME tStop, double dRate, bool doFakeSeek)
{
  if (m_bFlushing || !ThreadExists()) 
  {
    m_bSeekDone = true;
    return S_FALSE;
  }

  LogDebug("vid: DeliverNewSegment start: %6.3f stop: %6.3f rate: %6.3f", tStart / 10000000.0, tStop / 10000000.0, dRate);
  m_rtStart = tStart;
  m_bInitDuration = true;
  
  HRESULT hr = __super::DeliverNewSegment(tStart, tStop, dRate);
  if (FAILED(hr))
    LogDebug("vid: DeliverNewSegment - error: %08lX", hr);

  m_bSeekDone = true;
  m_bResetToLibSeek = doFakeSeek;

  return hr;
}

STDMETHODIMP CVideoPin::SetPositions(LONGLONG* pCurrent, DWORD CurrentFlags, LONGLONG* pStop, DWORD StopFlags)
{
  return m_pFilter->SetPositionsInternal(this, pCurrent, CurrentFlags, pStop, StopFlags);
}

STDMETHODIMP CVideoPin::GetAvailable(LONGLONG* pEarliest, LONGLONG* pLatest)
{
  //LogDebug("vid:GetAvailable");
  return CSourceSeeking::GetAvailable(pEarliest, pLatest);
}

STDMETHODIMP CVideoPin::GetDuration(LONGLONG *pDuration)
{
  REFERENCE_TIME refTime;
  m_pFilter->GetDuration(&refTime);
  m_rtDuration = CRefTime(refTime);

  return CSourceSeeking::GetDuration(pDuration);
}

HRESULT CVideoPin::ChangeStart()
{
  return S_OK;
}

HRESULT CVideoPin::ChangeStop()
{
  return S_OK;
}

HRESULT CVideoPin::ChangeRate()
{
  return S_OK;
}

STDMETHODIMP CVideoPin::SetRate(double dRate)
{
  if (dRate != 1.0)
    return VFW_E_UNSUPPORTED_AUDIO;  
  else
    return S_OK;
}

STDMETHODIMP CVideoPin::GetCurrentPosition(LONGLONG *pCurrent)
{
  //LogDebug("vid:GetCurrentPosition");
  return E_NOTIMPL;//CSourceSeeking::GetCurrentPosition(pCurrent);
}

STDMETHODIMP CVideoPin::Notify(IBaseFilter* pSender, Quality q)
{
  return E_NOTIMPL;
}

CLSID CVideoPin::GetDecoderCLSID(IPin* pPin)
{
  if (!pPin) 
    return GUID_NULL;

  CLSID clsid = GUID_NULL;
  PIN_INFO pi;

  if (SUCCEEDED(pPin->QueryPinInfo(&pi))) 
  {
    if (pi.pFilter)
    {
      pi.pFilter->GetClassID(&clsid);
      pi.pFilter->Release();
    }
  }

  return clsid;
}

void CVideoPin::LogMediaType(AM_MEDIA_TYPE* pmt)
{
  if (!pmt)
    LogDebug("Missing Video PMT");
  else
  {
    LogDebug("Video format %d {%08x-%04x-%04x-%02X%02X-%02X%02X%02X%02X%02X%02X}", pmt->cbFormat,
      pmt->subtype.Data1, pmt->subtype.Data2, pmt->subtype.Data3,
      pmt->subtype.Data4[0], pmt->subtype.Data4[1], pmt->subtype.Data4[2],
      pmt->subtype.Data4[3], pmt->subtype.Data4[4], pmt->subtype.Data4[5], 
      pmt->subtype.Data4[6], pmt->subtype.Data4[7]);
    if (pmt->formattype == FORMAT_MPEG2Video)
    {
      MPEG2VIDEOINFO * mp2vi = (MPEG2VIDEOINFO *) pmt->pbFormat;
      LogDebug("MPEG2 flags %4X, level %d profile %d", mp2vi->dwFlags, mp2vi->dwLevel, mp2vi->dwProfile);
    }
    else if (pmt->formattype == FORMAT_VideoInfo)
    {
      VIDEOINFOHEADER * vih = (VIDEOINFOHEADER *) pmt->pbFormat;
      LogDebug("VIH AvgTimePerFrame %I64d", vih->AvgTimePerFrame); 
    }
    else if (pmt->formattype == FORMAT_VideoInfo2)
    {
      VIDEOINFOHEADER2 * vih2 = (VIDEOINFOHEADER2 *) pmt->pbFormat;
      LogDebug("VIH2 AvgTimePerFrame %I64d interlaced %4X Aspect ratio X %d Aspect ratio Y %d Control flags %4X", vih2->AvgTimePerFrame, vih2->dwInterlaceFlags, vih2->dwPictAspectRatioX, vih2->dwPictAspectRatioY, vih2->dwControlFlags);  
    }
    else if (pmt->formattype == FORMAT_MPEGVideo)
    {
      MPEG1VIDEOINFO * mp1vi = (MPEG1VIDEOINFO *) pmt->pbFormat;
      LogDebug("MPEG1 GOP start %d", mp1vi->dwStartTimeCode);  
    }
  }
}

