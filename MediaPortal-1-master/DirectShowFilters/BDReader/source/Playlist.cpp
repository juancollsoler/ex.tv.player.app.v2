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

#include "Playlist.h"

// For more details for memory leak detection see the alloctracing.h header
#include "..\..\alloctracing.h"

extern void LogDebug(const char *fmt, ...);

CPlaylist::CPlaylist(int playlistNumber, REFERENCE_TIME firstPacketTime)
{
  Reset(playlistNumber, firstPacketTime);
}

CPlaylist::~CPlaylist(void)
{
  CAutoLock vectorLock(&m_sectionVector);
  if (m_vecClips.size() > 0)
  {
    ivecClip it = m_vecClips.begin();
    while (it != m_vecClips.end())
    {
      CClip* clip = *it;
      it = m_vecClips.erase(it);
      delete clip;
    }
  }
}

Packet* CPlaylist::ReturnNextAudioPacket()
{
  CAutoLock vectorLock(&m_sectionVector);
  firstPacketRead = true;
  firstAudioPESPacketSeen = true;
  Packet* ret = (*m_itCurrentAudioPlayBackClip)->ReturnNextAudioPacket(playlistFirstPacketTime);
  if (ret)
    ret->nPlaylist = nPlaylist;
  else
  {
    if (m_itCurrentAudioPlayBackClip - m_vecClips.begin() == m_vecClips.size() - 1) 
      SetEmptiedAudio();
    else
    {
      (*m_itCurrentAudioPlayBackClip)->Supersede(AUDIO_RETURN);
      m_itCurrentAudioPlayBackClip++;
      ret = ReturnNextAudioPacket();
    }
  }

  return ret;
}

Packet* CPlaylist::ReturnNextVideoPacket()
{
  CAutoLock vectorLock(&m_sectionVector);
  firstPacketRead=true;
  Packet* ret = (*m_itCurrentVideoPlayBackClip)->ReturnNextVideoPacket(playlistFirstPacketTime);
  if (ret)
    ret->nPlaylist = nPlaylist;
  else
  {
    if ((m_itCurrentVideoPlayBackClip+1) == m_vecClips.end())
      SetEmptiedVideo();
    else
    {
      (*(m_itCurrentVideoPlayBackClip))->Supersede(VIDEO_RETURN);
      m_itCurrentVideoPlayBackClip++;
      ret = ReturnNextVideoPacket();
    }
  }

  return ret;
}

bool CPlaylist::AcceptAudioPacket(Packet* packet)
{
  CAutoLock vectorLock(&m_sectionVector);
  bool ret = true;
  if (!m_vecClips.size())
    return false;

  if ((*m_itCurrentAudioSubmissionClip)->nClip == packet->nClipNumber)
  {
    if (!firstAudioPESPacketSeen && (*m_itCurrentAudioSubmissionClip)->nPlaylist != packet->nPlaylist)
      packet->nNewSegment |= NS_NEW_PLAYLIST;

    ret = (*m_itCurrentAudioSubmissionClip)->AcceptAudioPacket(packet);
  }
  else
    LogDebug("CPlaylist Panic in Accept Audio Packet");

  if (!firstAudioPESPacketSeen && ret && packet->rtStart != Packet::INVALID_TIME)
    firstAudioPESPacketSeen = true;

  return ret;
}

bool CPlaylist::AcceptVideoPacket(Packet* packet)
{
  CAutoLock vectorLock(&m_sectionVector);
  bool ret = true;
  REFERENCE_TIME prevVideoPosition = 0;
  if (nPlaylist != packet->nPlaylist)
  {
    (*m_itCurrentVideoSubmissionClip)->Supersede(VIDEO_FILL);
    return false;
  }
  if ((*m_itCurrentVideoSubmissionClip)->nClip == packet->nClipNumber)
  {
    if (!firstVideoPESPacketSeen && (*m_itCurrentVideoSubmissionClip)->nPlaylist != packet->nPlaylist)
      packet->nNewSegment |= NS_NEW_PLAYLIST;

    ret = (*m_itCurrentVideoSubmissionClip)->AcceptVideoPacket(packet);
  }

  if (!firstVideoPESPacketSeen && ret && packet->rtStart != Packet::INVALID_TIME)
    firstVideoPESPacketSeen = true;

  return ret;
}

void CPlaylist::CurrentClipFilled()
{
  if (m_vecClips.size())
  {
    (*m_itCurrentAudioSubmissionClip)->Supersede(AUDIO_FILL);
    (*m_itCurrentVideoSubmissionClip)->Supersede(VIDEO_FILL);
  }
}

bool CPlaylist::CreateNewClip(int clipNumber, REFERENCE_TIME clipStart, REFERENCE_TIME clipOffset, bool audioPresent, REFERENCE_TIME duration, REFERENCE_TIME playlistClipOffset, REFERENCE_TIME streamStartOffset, bool interrupted)
{
  CAutoLock vectorLock(&m_sectionVector);
  bool ret = true;

  if (ClipExists(clipNumber))
  {
    LogDebug("CPlaylist::CreateNewClip - filtering out a duplicate clip - pl: %d clip: %d", nPlaylist, clipNumber);
    ret = false;
  }
  else
  {
    CClip* videoClip = NULL;
    CClip* audioClip = NULL;

    int clipsSize = m_vecClips.size();

    if (clipsSize > 0)
    {
      videoClip = *m_itCurrentVideoSubmissionClip;
      audioClip = *m_itCurrentAudioSubmissionClip;
    }

    if (videoClip && videoClip->firstVideo)
    {
      Packet* packet = new Packet();
      packet->rtStart = videoClip->lastAudioPosition;
      packet->nClipNumber = videoClip->nClip;
      packet->nPlaylist = videoClip->nPlaylist;

      AcceptVideoPacket(packet);
    }
  
    if (audioClip && audioClip->firstAudio)
    {
      Packet* packet = new Packet();
      packet->rtStart = audioClip->lastAudioPosition;
      packet->nClipNumber = audioClip->nClip;
      packet->nPlaylist = audioClip->nPlaylist;

      AcceptAudioPacket(packet);
    }

    if (audioClip)
      audioClip->Supersede(AUDIO_FILL);

    if (videoClip)
      videoClip->Supersede(VIDEO_FILL);

    PushClips();
    m_vecClips.push_back(new CClip(clipNumber, nPlaylist, clipStart, clipOffset, playlistClipOffset, audioPresent, duration, streamStartOffset, interrupted));

    // initialise
    if (clipsSize == 0)
      m_itCurrentAudioPlayBackClip = m_itCurrentVideoPlayBackClip = m_itCurrentAudioSubmissionClip = m_itCurrentVideoSubmissionClip = m_vecClips.begin();
    else
    {
      PopClips();
      m_itCurrentAudioSubmissionClip++;
      m_itCurrentVideoSubmissionClip++;
    }
  }

  return ret;
}

bool CPlaylist::IsEmptiedAudio()
{
  return playlistEmptiedAudio;
}

bool CPlaylist::IsEmptiedVideo()
{
  return playlistEmptiedVideo;
}

void CPlaylist::SetEmptiedVideo()
{
  playlistEmptiedVideo = true;
}

void CPlaylist::SetEmptiedAudio()
{
  playlistEmptiedAudio = true;
}

bool CPlaylist::IsFilledAudio()
{
  return playlistFilledAudio;
}

bool CPlaylist::IsFilledVideo()
{
  return playlistFilledVideo;
}

void CPlaylist::SetFilledVideo()
{
  playlistFilledVideo = true;
}

void CPlaylist::SetFilledAudio()
{
  playlistFilledAudio = true;
}

void CPlaylist::FlushAudio()
{
  CAutoLock vectorLock(&m_sectionVector);
  ivecClip it = m_vecClips.begin();
  while (it != m_vecClips.end())
  {
    CClip* clip = *it;
    clip->FlushAudio();
    ++it;
  }
}
void CPlaylist::FlushVideo()
{
  CAutoLock vectorLock(&m_sectionVector);
  ivecClip it = m_vecClips.begin();
  while (it != m_vecClips.end())
  {
    CClip* clip = *it;
    clip->FlushVideo();
    ++it;
  }
}

void CPlaylist::ClearClips(REFERENCE_TIME totalStreamOffset, bool skipCurrentClip)
{
  CAutoLock vectorLock(&m_sectionVector);
  ivecClip it = m_vecClips.begin();
  while (it != m_vecClips.end())
  {
    CClip* clip =* it;
    if (clip == m_vecClips.back() && skipCurrentClip)
      ++it;
    else
    {
      it = m_vecClips.erase(it);
      delete clip;
    }
  }

  if (m_vecClips.size() > 0)
  {
    m_itCurrentAudioPlayBackClip = m_itCurrentVideoPlayBackClip = m_itCurrentAudioSubmissionClip = m_itCurrentVideoSubmissionClip = m_vecClips.begin();
    Reset(nPlaylist, playlistFirstPacketTime);
    (*m_itCurrentAudioPlayBackClip)->Reset(totalStreamOffset);
  }
}

void CPlaylist::Reset(int playlistNumber, REFERENCE_TIME firstPacketTime)
{
  CAutoLock vectorLock(&m_sectionVector);
  nPlaylist = playlistNumber;
  playlistFirstPacketTime = firstPacketTime;
  playlistFilledAudio = false;
  playlistFilledVideo = false;
  playlistEmptiedVideo = false;
  playlistEmptiedAudio = false;

  firstAudioPESPacketSeen = false;
  firstVideoPESPacketSeen = false;

  firstPacketRead = false;
}

bool CPlaylist::HasAudio()
{
  CAutoLock vectorLock(&m_sectionVector);
  if (!m_vecClips.size()) 
    return false;

  if ((*m_itCurrentAudioPlayBackClip)->HasAudio())
    return true;

  if (++m_itCurrentAudioPlayBackClip == m_vecClips.end())
    m_itCurrentAudioPlayBackClip--;
  else
  {
    bool ret = (*m_itCurrentAudioPlayBackClip)->HasAudio();
    m_itCurrentAudioPlayBackClip--;
    return ret;
  }

  return false;
}

bool CPlaylist::HasVideo()
{
  CAutoLock vectorLock(&m_sectionVector);
  if (!m_vecClips.size()) 
    return false;
  if ((*m_itCurrentVideoPlayBackClip)->HasVideo())
    return true;
  if (++m_itCurrentVideoPlayBackClip == m_vecClips.end())
    m_itCurrentVideoPlayBackClip--;
  else
  {
    bool ret = (*m_itCurrentVideoPlayBackClip)->HasVideo();
    m_itCurrentVideoPlayBackClip--;
    return ret;
  }

  return false;
}

REFERENCE_TIME CPlaylist::PlayedDuration()
{
  CAutoLock vectorLock(&m_sectionVector);
  if (m_vecClips.size() > 0)
    return m_vecClips.back()->PlayedDuration();

  return 0LL;
}

void CPlaylist::SetVideoPMT(AM_MEDIA_TYPE * pmt, int nClip)
{
  CAutoLock vectorLock(&m_sectionVector);
  (*m_itCurrentVideoSubmissionClip)->SetVideoPMT(pmt);
}

bool CPlaylist::RemoveRedundantClips()
{
  CAutoLock vectorLock(&m_sectionVector);
  ivecClip it = m_vecClips.end();
  if (it != m_vecClips.begin()) --it;
  while (it != m_vecClips.begin())
  {
    CClip* clip = *it;
    if (clip->IsSuperseded(AUDIO_RETURN | VIDEO_RETURN | AUDIO_FILL | VIDEO_FILL))
    {
      it = m_vecClips.erase(it);
      delete clip;
    }
    else
      --it;
  }
  
  if (m_vecClips.size() == 0)
    return true;

  return false;
}

vector<CClip*> CPlaylist::Superceed()
{
  CAutoLock vectorLock(&m_sectionVector);
  vector<CClip*> ret;
  ivecClip it = m_vecClips.begin();
  while (it != m_vecClips.end())
  {
    CClip* clip = *it;
    clip->Supersede(AUDIO_FILL);

    ++it;
  }
  return ret;
}

bool CPlaylist::ClipExists(int nClip)
{
  CAutoLock vectorLock(&m_sectionVector);
  bool ret = false;
  ivecClip it = m_vecClips.begin();
  while (it != m_vecClips.end())
  {
    CClip* clip = *it;
    if (clip->nPlaylist == nPlaylist && clip->nClip == nClip)
    {
      ret = true;
      break;
    }
    ++it;
  }

  return ret;
}

void CPlaylist::PushClips()
{
  if (m_vecClips.size() > 0)
  {
    m_itCurrentAudioPlayBackClipPos = m_itCurrentAudioPlayBackClip - m_vecClips.begin();
    m_itCurrentVideoPlayBackClipPos = m_itCurrentVideoPlayBackClip - m_vecClips.begin();
    m_itCurrentAudioSubmissionClipPos = m_itCurrentAudioSubmissionClip - m_vecClips.begin();
    m_itCurrentVideoSubmissionClipPos = m_itCurrentVideoSubmissionClip - m_vecClips.begin();
  }
}

void CPlaylist::PopClips()
{
  m_itCurrentAudioPlayBackClip = m_vecClips.begin() + m_itCurrentAudioPlayBackClipPos;
  m_itCurrentVideoPlayBackClip = m_vecClips.begin() + m_itCurrentVideoPlayBackClipPos;
  m_itCurrentAudioSubmissionClip = m_vecClips.begin() + m_itCurrentAudioSubmissionClipPos;
  m_itCurrentVideoSubmissionClip = m_vecClips.begin() + m_itCurrentVideoSubmissionClipPos;
}

bool CPlaylist::AllowBuffering()
{
  bool ret = true;

  if (*m_itCurrentAudioPlayBackClip)
    ret = (*m_itCurrentAudioPlayBackClip)->AllowBuffering();

  return ret;
}

