#region Copyright (C) 2005-2011 Team MediaPortal

// Copyright (C) 2005-2011 Team MediaPortal
// http://www.team-mediaportal.com
// 
// MediaPortal is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 2 of the License, or
// (at your option) any later version.
// 
// MediaPortal is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with MediaPortal. If not, see <http://www.gnu.org/licenses/>.

#endregion

using System;
using System.Collections.Generic;
using DirectShowLib;
using TvLibrary.Implementations.Analog.Components;
using TvLibrary.Interfaces;
using TvLibrary.Interfaces.Analyzer;
using TvLibrary.Implementations;

namespace TvLibrary.Implementations.Analog
{
  /// <summary>
  /// Implementation of <see cref="T:TvLibrary.Interfaces.ITVCard"/> which handles analog tv cards
  /// </summary>
  public class AnalogSubChannel : BaseSubChannel, ITvSubChannel, IAnalogTeletextCallBack, IAnalogVideoAudioObserver
  {
    #region variables

    private readonly TvCardAnalog _card;
    private readonly TvAudio _tvAudio;
    private readonly IBaseFilter _mpFileWriter;
    private readonly IMPRecord _mpRecord;

    #endregion

    #region ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="AnalogSubChannel"/> class.
    /// </summary>
    internal AnalogSubChannel(TvCardAnalog card, int subchnnelId, TvAudio tvAudio, bool hasTeletext,
                              IBaseFilter mpFileWriter)
    {
      _card = card;
      _hasTeletext = hasTeletext;
      _tvAudio = tvAudio;
      _mpFileWriter = mpFileWriter;
      _mpRecord = (IMPRecord)_mpFileWriter;
      _mpRecord.AddChannel(ref _subChannelId);
      _subChannelId = subchnnelId;
    }

    #endregion

    #region tuning and graph methods

    /// <summary>
    /// Should be called before tuning to a new channel
    /// resets the state
    /// </summary>
    public override void OnBeforeTune()
    {
      Log.Log.WriteFile("analog subch:{0} OnBeforeTune", _subChannelId);
      if (IsTimeShifting)
      {
        if (_subChannelId >= 0)
        {
          _mpRecord.PauseTimeShifting(_subChannelId, 1);
        }
      }
    }

    /// <summary>
    /// Should be called when the graph is tuned to the new channel
    /// resets the state
    /// </summary>
    public override void OnAfterTune()
    {
      Log.Log.WriteFile("analog subch:{0} OnAfterTune", _subChannelId);
      if (IsTimeShifting)
      {
        if (_subChannelId >= 0)
        {
          _mpRecord.PauseTimeShifting(_subChannelId, 0);
        }
      }
    }

    /// <summary>
    /// Should be called when the graph is about to start
    /// Resets the state 
    /// If graph is already running, starts the pmt grabber to grab the
    /// pmt for the new channel
    /// </summary>
    public override void OnGraphStart()
    {
      Log.Log.WriteFile("analog subch:{0} OnGraphStart", _subChannelId);
      if (_teletextDecoder != null)
      {
        _teletextDecoder.ClearBuffer();
      }
      OnAfterTuneEvent();
    }

    /// <summary>
    /// Should be called when the graph has been started
    /// sets up the pmt grabber to grab the pmt of the channel
    /// </summary>
    public override void OnGraphStarted()
    {
      Log.Log.WriteFile("analog subch:{0} OnGraphStarted", _subChannelId);
      _dateTimeShiftStarted = DateTime.MinValue;
      OnAfterTuneEvent();
    }

    /// <summary>
    /// Should be called when graph is about to stop.
    /// stops any timeshifting/recording on this channel
    /// </summary>
    public override void OnGraphStop()
    {
      if (_mpRecord != null)
      {
        _mpRecord.StopRecord(_subChannelId);
        _mpRecord.StopTimeShifting(_subChannelId);
      }
    }

    /// <summary>
    /// should be called when graph has been stopped
    /// Resets the graph state
    /// </summary>
    public override void OnGraphStopped() {}

    #endregion

    #region Timeshifting - Recording methods

    /// <summary>
    /// sets the filename used for timeshifting
    /// </summary>
    /// <param name="fileName">timeshifting filename</param>
    protected override bool OnStartTimeShifting(string fileName)
    {
      if (_card.SupportsQualityControl && !IsRecording)
      {
        _card.Quality.StartPlayback();
      }
      _timeshiftFileName = fileName;
      Log.Log.WriteFile("analog:SetTimeShiftFileName:{0}", fileName);
      Log.Log.WriteFile("analog:SetTimeShiftFileName: uses .ts");
      ScanParameters parameters = _card.Parameters;
      _mpRecord.SetVideoAudioObserver(_subChannelId, this);
      _mpRecord.SetTimeShiftParams(_subChannelId, parameters.MinimumFiles, parameters.MaximumFiles,
                                   parameters.MaximumFileSize);
      _mpRecord.SetTimeShiftFileNameW(_subChannelId, fileName);

      //  Set the channel type
      if (CurrentChannel == null)
      {
        Log.Log.Error("Error, CurrentChannel is null when trying to start timeshifting");
        return false;
      }

      // Important: this call needs to be made *before* the call to StartTimeShifting().
      _mpRecord.SetChannelType(_subChannelId, (CurrentChannel.IsTv ? 0 : 1));

      _mpRecord.StartTimeShifting(_subChannelId);
      _dateTimeShiftStarted = DateTime.Now;
      return true;
    }

    /// <summary>
    /// Stops timeshifting
    /// </summary>
    /// <returns></returns>
    protected override void OnStopTimeShifting()
    {
      Log.Log.WriteFile("analog: StopTimeShifting()");
      _mpRecord.SetVideoAudioObserver(_subChannelId, null);
      _mpRecord.StopTimeShifting(_subChannelId);
    }

    /// <summary>
    /// Starts recording
    /// </summary>
    /// <param name="fileName">filename to which to recording should be saved</param>
    /// <returns></returns>
    protected override void OnStartRecording(string fileName)
    {
      if (_card.SupportsQualityControl)
      {
        _card.Quality.StartRecord();
      }
      Log.Log.WriteFile("analog:StartRecord({0})", fileName);
      _mpRecord.SetRecordingFileNameW(_subChannelId, fileName);
      _mpRecord.SetRecorderVideoAudioObserver(_subChannelId, this);

      // Important: this call needs to be made *before* the call to StartRecord().
      _mpRecord.SetChannelType(_subChannelId, (CurrentChannel.IsTv ? 0 : 1));

      _mpRecord.StartRecord(_subChannelId);
      _dateRecordingStarted = DateTime.Now;
    }

    /// <summary>
    /// Stop recording
    /// </summary>
    /// <returns></returns>
    protected override void OnStopRecording()
    {
      Log.Log.WriteFile("analog:StopRecord()");
      _mpRecord.StopRecord(_subChannelId);
      if (_card.SupportsQualityControl && IsTimeShifting)
      {
        _card.Quality.StartPlayback();
      }
    }

    /// <summary>
    /// Returns the position in the current timeshift file and the id of the current timeshift file
    /// </summary>
    /// <param name="position">The position in the current timeshift buffer file</param>
    /// <param name="bufferId">The id of the current timeshift buffer file</param>
    protected override void OnGetTimeShiftFilePosition(ref Int64 position, ref long bufferId)
    {
      position = -1;
      bufferId = -1;
    }

    #endregion

    #region audio streams

    /// <summary>
    /// returns the list of available audio streams
    /// </summary>
    public override List<IAudioStream> AvailableAudioStreams
    {
      get { return _tvAudio.GetAvailableAudioStreams(); }
    }

    /// <summary>
    /// get/set the current selected audio stream
    /// </summary>
    public override IAudioStream CurrentAudioStream
    {
      get { return _tvAudio.CurrentAudioStream; }
      set { _tvAudio.CurrentAudioStream = value; }
    }

    #endregion

    #region video stream

    /// <summary>
    /// Returns true when unscrambled audio/video is received otherwise false
    /// </summary>
    /// <returns>true of false</returns>
    public override bool IsReceivingAudioVideo
    {
      get { return true; }
    }

    /// <summary>
    /// Retursn the video format (always returns MPEG2). 
    /// </summary>
    /// <value>The number of channels decrypting.</value>
    public override IVideoStream GetCurrentVideoStream
    {
      get
      {
        VideoStream stream = new VideoStream();
        return stream;
      }
    }

    #endregion

    #region teletext

    /// <summary>
    /// A derrived class should activate or deactivate the teletext grabbing on the tv card.
    /// </summary>
    protected override void OnGrabTeletext()
    {
      if (_hasTeletext)
      {
        if (_grabTeletext)
        {
          _mpRecord.TTxSetCallback(_subChannelId, this);
        }
        else
        {
          _mpRecord.TTxSetCallback(_subChannelId, null);
        }
      }
      else
      {
        _grabTeletext = false;
        _mpRecord.TTxSetCallback(_subChannelId, null);
      }
    }

    #endregion

    #region OnDecompose

    /// <summary>
    /// Decomposes this subchannel
    /// </summary>
    protected override void OnDecompose()
    {
      if (_mpRecord != null)
      {
        _mpRecord.DeleteChannel(_subChannelId);
      }
    }

    #endregion
  }
}