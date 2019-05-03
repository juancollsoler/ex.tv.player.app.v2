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

#include "PlaylistManager.h"

// For more details for memory leak detection see the alloctracing.h header
#include "..\..\alloctracing.h"

extern void LogDebug(const char *fmt, ...);

CPlaylistManager::CPlaylistManager(void)
{
  LogDebug("Playlist Manager Created");
  m_rtPlaylistOffset = 0LL;
  firstVideo = firstAudio = true;
}


CPlaylistManager::~CPlaylistManager(void)
{
  CAutoLock lock (&m_sectionAudio);
  CAutoLock lockv (&m_sectionVideo);
  LogDebug("Playlist Manager Closing");

  CAutoLock vectorLock(&m_sectionVector);
  ivecPlaylists it = m_vecPlaylists.begin();
  while (it != m_vecPlaylists.end())
  {
    CPlaylist* playlist = *it;
    it = m_vecPlaylists.erase(it);
    delete playlist;
  }
}

void CPlaylistManager::CreateNewPlaylistClip(int nPlaylist, int nClip, bool audioPresent, REFERENCE_TIME firstPacketTime, REFERENCE_TIME clipOffsetTime, REFERENCE_TIME duration, REFERENCE_TIME streamStartPosition, bool interrupted)
{
  CAutoLock lock (&m_sectionAudio);
  CAutoLock lockv (&m_sectionVideo);
  CAutoLock vectorLock(&m_sectionVector);

  LogDebug("Playlist Manager new Playlist %d clip %d start %6.3f clipOffset %6.3f Audio %d duration %6.3f",nPlaylist, nClip, firstPacketTime/10000000.0, clipOffsetTime/10000000.0, audioPresent, duration/10000000.0);

  // Mark current playlist as filled
  CurrentClipFilled();

  REFERENCE_TIME playedDuration = ClipPlayTime();

  //LogDebug("Playlist Manager::TimeStamp Correction changed to %I64d adding %I64d",m_rtPlaylistOffset + playedDuration, playedDuration);

  m_rtPlaylistOffset += playedDuration;

  if (m_vecPlaylists.empty())
  {
    // First playlist
    CPlaylist* firstPlaylist = new CPlaylist(nPlaylist, firstPacketTime);
    if (firstPlaylist->CreateNewClip(nClip, firstPacketTime, clipOffsetTime, audioPresent, duration, m_rtPlaylistOffset, streamStartPosition, false))
    {
      m_vecPlaylists.push_back(firstPlaylist);
      m_itCurrentAudioPlayBackPlaylist = m_itCurrentVideoPlayBackPlaylist = m_itCurrentAudioSubmissionPlaylist = m_itCurrentVideoSubmissionPlaylist = m_vecPlaylists.begin();
    }
  }
  else if (m_vecPlaylists.back()->nPlaylist == nPlaylist)
  {
    // New clip in existing playlist
    CPlaylist* existingPlaylist = m_vecPlaylists.back();
    existingPlaylist->CreateNewClip(nClip, firstPacketTime, clipOffsetTime, audioPresent, duration, m_rtPlaylistOffset, streamStartPosition, interrupted);
  }
  else
  {
    CPlaylist* newPlaylist = new CPlaylist(nPlaylist,firstPacketTime);
    if (newPlaylist->CreateNewClip(nClip, firstPacketTime, clipOffsetTime, audioPresent, duration, m_rtPlaylistOffset, streamStartPosition, interrupted))
    {
      PushPlaylists();
      m_vecPlaylists.push_back(newPlaylist);
      PopPlaylists(0);

      (*m_itCurrentAudioSubmissionPlaylist)->SetFilledAudio();
      (*m_itCurrentVideoSubmissionPlaylist)->SetFilledVideo();

      //move to this playlist
      m_itCurrentAudioSubmissionPlaylist++;
      m_itCurrentVideoSubmissionPlaylist++;
    }
  }
}

bool CPlaylistManager::SubmitAudioPacket(Packet * packet)
{
  CAutoLock lock(&m_sectionAudio);
  CAutoLock vectorLock(&m_sectionVector);
  bool ret = false;
  if (m_vecPlaylists.size() == 0) 
  {
    LogDebug("m_currentAudioSubmissionPlaylist is NULL!!!");
    return false;
  }
  ret = (*m_itCurrentAudioSubmissionPlaylist)->AcceptAudioPacket(packet);
  if (ret) 
  {
#ifdef LOG_AUDIO_PACKETS
    LogDebug("Audio Packet %I64d Accepted in %d %d", packet->rtStart, packet->nPlaylist, packet->nClipNumber);
#endif
  }

  return ret;
}

bool CPlaylistManager::SubmitVideoPacket(Packet * packet)
{
  CAutoLock lock (&m_sectionVideo);
  CAutoLock vectorLock(&m_sectionVector);
  bool ret = false;
  if (m_vecPlaylists.size() == 0)
  {
    LogDebug("m_currentVideoSubmissionPlaylist is NULL!!!");
    return false;
  }
  ret=(*m_itCurrentVideoSubmissionPlaylist)->AcceptVideoPacket(packet);
  if (ret)
  {
#ifdef LOG_VIDEO_PACKETS
    LogDebug("Video Packet %I64d Accepted in %d %d", packet->rtStart, packet->nPlaylist, packet->nClipNumber);
#endif
  }

  return ret;
}

Packet* CPlaylistManager::GetNextAudioPacket()
{
  CAutoLock lock (&m_sectionAudio);
  CAutoLock vectorLock(&m_sectionVector);
  Packet* ret=(*m_itCurrentAudioPlayBackPlaylist)->ReturnNextAudioPacket();
  if (!ret)
  {
    if (m_itCurrentAudioPlayBackPlaylist - m_vecPlaylists.begin() != m_vecPlaylists.size() - 1)
    {
      (*(m_itCurrentAudioPlayBackPlaylist))->SetEmptiedAudio();
      ret = (*(m_itCurrentAudioPlayBackPlaylist++))->ReturnNextAudioPacket();
      //LogDebug("playlistManager: setting audio playback playlist to %d",(*m_itCurrentAudioPlayBackPlaylist)->nPlaylist);
    }
  }

  if (ret && firstAudio)
  {
    firstAudio = false;
    ret->nNewSegment = 0;
  }

  return ret;
}

Packet* CPlaylistManager::GetNextVideoPacket()
{
  CAutoLock lock (&m_sectionVideo);
  CAutoLock vectorLock(&m_sectionVector);
  Packet* ret=(*m_itCurrentVideoPlayBackPlaylist)->ReturnNextVideoPacket();
  if (!ret)
  {
    if (m_itCurrentVideoPlayBackPlaylist + 1 != m_vecPlaylists.end())
    {
      (*(m_itCurrentVideoPlayBackPlaylist))->SetEmptiedVideo();
      ret = (*(m_itCurrentVideoPlayBackPlaylist++))->ReturnNextVideoPacket();
      //LogDebug("playlistManager: setting video playback playlist to %d",(*m_itCurrentVideoPlayBackPlaylist)->nPlaylist);
    }
  }
  if (!ret)
    return NULL;

  if (firstVideo && ret->rtStart != Packet::INVALID_TIME)
  {
    firstVideo = false;
    ret->nNewSegment = 0;
  }

  return ret;
}

void CPlaylistManager::FlushAudio(void)
{
  CAutoLock lock (&m_sectionAudio);
  CAutoLock vectorLock(&m_sectionVector);

  LogDebug("Playlist Manager Flush Audio");

  ivecPlaylists it = m_vecPlaylists.begin();
  while (it != m_vecPlaylists.end())
  {
    CPlaylist* playlist = *it;
    playlist->FlushAudio();
    ++it;
  }

  m_itCurrentAudioPlayBackPlaylist=m_itCurrentAudioSubmissionPlaylist=m_vecPlaylists.begin();
}

void CPlaylistManager::FlushVideo(void)
{
  CAutoLock lock (&m_sectionVideo);
  CAutoLock vectorLock(&m_sectionVector);

  LogDebug("Playlist Manager Flush Video");

  ivecPlaylists it = m_vecPlaylists.begin();
  while (it != m_vecPlaylists.end())
  {
    CPlaylist* playlist = *it;
    playlist->FlushVideo();
    ++it;
  }

  m_itCurrentVideoPlayBackPlaylist=m_itCurrentVideoSubmissionPlaylist=m_vecPlaylists.begin();
}

bool CPlaylistManager::HasAudio()
{
  CAutoLock vectorLock(&m_sectionVector);

  if (m_vecPlaylists.size() == 0)
    return false;

  if ((*m_itCurrentAudioPlayBackPlaylist)->HasAudio())
    return true;

  if (++m_itCurrentAudioPlayBackPlaylist == m_vecPlaylists.end())
    m_itCurrentAudioPlayBackPlaylist--;
  else 
    return (*m_itCurrentAudioPlayBackPlaylist)->HasAudio();

  return false;
}

bool CPlaylistManager::HasVideo()
{
  CAutoLock vectorLock(&m_sectionVector);

  if (m_vecPlaylists.size() == 0)
    return false;

  if ((*m_itCurrentVideoPlayBackPlaylist)->HasVideo())
    return true;

  if (++m_itCurrentVideoPlayBackPlaylist == m_vecPlaylists.end())
    m_itCurrentVideoPlayBackPlaylist--;
  else
    return (*m_itCurrentVideoPlayBackPlaylist)->HasVideo();

  return false;
}

void CPlaylistManager::ClearClips(bool skipCurrentClip)
{
  CAutoLock locka (&m_sectionAudio);
  CAutoLock lockv (&m_sectionVideo);
  CAutoLock vectorLock(&m_sectionVector);

  if (m_vecPlaylists.size() == 0)
    return;

  LogDebug("CPlaylistManager::ClearClips");

  ivecPlaylists it = m_vecPlaylists.begin();
  while (it != m_vecPlaylists.end())
  {
    CPlaylist* playlist = *it;
    if (playlist == m_vecPlaylists.back() && skipCurrentClip)
    {
      playlist->FlushAudio();
      playlist->FlushVideo();
      ++it;
    }
    else
    {
      it = m_vecPlaylists.erase(it);
      delete playlist;
    }
  }

  if (m_vecPlaylists.size() > 0)
  {
    m_itCurrentAudioPlayBackPlaylist = m_itCurrentVideoPlayBackPlaylist = m_itCurrentAudioSubmissionPlaylist = m_itCurrentVideoSubmissionPlaylist = m_vecPlaylists.begin() + (m_vecPlaylists.size() - 1);
    (*m_itCurrentVideoPlayBackPlaylist)->ClearClips(m_rtPlaylistOffset, skipCurrentClip);
  }
}

REFERENCE_TIME CPlaylistManager::ClipPlayTime()
{
  CAutoLock vectorLock(&m_sectionVector);

  REFERENCE_TIME ret = 0LL;

  if (!m_vecPlaylists.empty())
    ret = m_vecPlaylists.back()->PlayedDuration();

  return ret;
}

void CPlaylistManager::SetVideoPMT(AM_MEDIA_TYPE *pmt, int nPlaylist, int nClip)
{
  if (pmt)
  {
    CAutoLock vectorLock(&m_sectionVector);
    LogDebug("CPlaylistManager: Setting video PMT {%08x-%04x-%04x-%02X%02X-%02X%02X%02X%02X%02X%02X} for (%d, %d)",
      pmt->subtype.Data1, pmt->subtype.Data2, pmt->subtype.Data3,
      pmt->subtype.Data4[0], pmt->subtype.Data4[1], pmt->subtype.Data4[2],
      pmt->subtype.Data4[3], pmt->subtype.Data4[4], pmt->subtype.Data4[5], 
      pmt->subtype.Data4[6], pmt->subtype.Data4[7], nPlaylist, nClip);
    (*m_itCurrentVideoSubmissionPlaylist)->SetVideoPMT(pmt, nClip);
  }
}

void CPlaylistManager::PushPlaylists()
{
  m_itCurrentAudioPlayBackPlaylistPos = m_itCurrentAudioPlayBackPlaylist-m_vecPlaylists.begin();
  m_itCurrentVideoPlayBackPlaylistPos = m_itCurrentVideoPlayBackPlaylist-m_vecPlaylists.begin();
  m_itCurrentAudioSubmissionPlaylistPos = m_itCurrentAudioSubmissionPlaylist-m_vecPlaylists.begin();
  m_itCurrentVideoSubmissionPlaylistPos = m_itCurrentVideoSubmissionPlaylist-m_vecPlaylists.begin();
}

void CPlaylistManager::PopPlaylists(int difference)
{
  CAutoLock vectorLock(&m_sectionVector);

  if (m_itCurrentAudioPlayBackPlaylistPos - difference < 0)
    m_itCurrentAudioPlayBackPlaylistPos = difference;

  m_itCurrentAudioPlayBackPlaylist = m_vecPlaylists.begin() + (m_itCurrentAudioPlayBackPlaylistPos - difference);

  if (m_itCurrentVideoPlayBackPlaylistPos - difference < 0)
    m_itCurrentVideoPlayBackPlaylistPos = difference;

  m_itCurrentVideoPlayBackPlaylist = m_vecPlaylists.begin() + (m_itCurrentVideoPlayBackPlaylistPos - difference);
  m_itCurrentAudioSubmissionPlaylist = m_vecPlaylists.begin() + (m_itCurrentAudioSubmissionPlaylistPos - difference);
  m_itCurrentVideoSubmissionPlaylist = m_vecPlaylists.begin() + (m_itCurrentVideoSubmissionPlaylistPos - difference);
}

void CPlaylistManager::CurrentClipFilled()
{
  CAutoLock vectorLock(&m_sectionVector);

  if (m_vecPlaylists.size())
  {
    LogDebug("CPlaylistManager::CurrentClipFilled");
    (*m_itCurrentVideoSubmissionPlaylist)->CurrentClipFilled();
  }
}

bool CPlaylistManager::AllowBuffering()
{
  bool ret = true;

  CAutoLock vectorLock(&m_sectionVector);

  if (*m_itCurrentAudioPlayBackPlaylist)
    ret = (*m_itCurrentAudioPlayBackPlaylist)->AllowBuffering();

  return ret;
}
