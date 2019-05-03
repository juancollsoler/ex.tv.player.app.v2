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
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using DirectShowLib;
using TvLibrary.ChannelLinkage;
using TvLibrary.Channels;
using TvLibrary.Epg;
using TvLibrary.Implementations.Analog.QualityControl;
using TvLibrary.Interfaces;
using TvLibrary.Implementations.DVB;
using TvLibrary.Implementations.Helper;

namespace TvLibrary.Implementations.Analog
{
  /// <summary>
  /// Class for handling supported capture cards, including the Hauppauge HD PVR and Colossus.
  /// </summary>
  public class TvCardHDPVR : TvCardBase, ITVCard
  {
    #region constants

    public const string CROSSBAR_NAME_HDPVR = "Hauppauge HD PVR Crossbar";
    public const string CROSSBAR_NAME_COLOSSUS = "Hauppauge Colossus Crossbar";
    public const string CROSSBAR_NAME_HDPVR2_COLOSSUS2 = "Hauppauge Siena Crossbar";
    public const string CROSSBAR_NAME_HDPVR60 = "HD PVR 60 Crossbar Filter (HD)";

    // Assume the capture card is a Hauppauge HD PVR by default.
    private readonly string _deviceType = "HD-PVR";
    private readonly string _crossbarDeviceName = CROSSBAR_NAME_HDPVR;
    private readonly string _captureDeviceName = "Hauppauge HD PVR Capture Device";
    private readonly string _encoderDeviceName = "Hauppauge HD PVR Encoder";

    #endregion

    #region imports

    [ComImport, Guid("fc50bed6-fe38-42d3-b831-771690091a6e")]
    private class MpTsAnalyzer {}

    #endregion

    #region variables

    private DsROTEntry _rotEntry;
    private ICaptureGraphBuilder2 _capBuilder;
    private DsDevice _crossBarDevice;
    private DsDevice _captureDevice;
    private DsDevice _encoderDevice;
    private IBaseFilter _filterCrossBar;
    private IBaseFilter _filterCapture;
    private IBaseFilter _filterEncoder;
    private IBaseFilter _filterTsWriter;
    private Configuration _configuration;
    private AnalogChannel _previousChannel;
    private IQuality _qualityControl;

    /// <summary>
    /// The mapping of the video input sources to their pin index
    /// </summary>
    private Dictionary<AnalogChannel.VideoInputType, int> _videoPinMap;

    /// <summary>
    /// The mapping of the video input sources to their related audio pin index
    /// </summary>
    private Dictionary<AnalogChannel.VideoInputType, int> _videoPinRelatedAudioMap;

    /// <summary>
    /// The mapping of the audio input sources to their pin index
    /// </summary>
    private Dictionary<AnalogChannel.AudioInputType, int> _audioPinMap;

    private int _videoOutPinIndex;
    private int _audioOutPinIndex;

    #endregion

    #region ctor

    ///<summary>
    /// Constructor for a capture card device.
    ///</summary>
    ///<param name="device">A crossbar device for a supported capture card.</param>
    public TvCardHDPVR(DsDevice device)
      : base(device)
    {
      // Determine what type of card this is.
      if (device.Name.Contains(CROSSBAR_NAME_COLOSSUS))
      {
        Match match = Regex.Match(device.Name, @".*?(\d+)$");
        int deviceNumber = 0;
        if (match.Success)
        {
          deviceNumber = Convert.ToInt32(match.Groups[1].Value);
        }
        _deviceType = "Colossus";
        _crossbarDeviceName = device.Name;
        _captureDeviceName = "Hauppauge Colossus Capture " + deviceNumber;
        _encoderDeviceName = "Hauppauge Colossus TS Encoder " + deviceNumber;
      }
      else if (device.Name.Equals(CROSSBAR_NAME_HDPVR2_COLOSSUS2))
      {
        _deviceType = "HD-PVR 2";
        _crossbarDeviceName = device.Name;
        _captureDeviceName = "Hauppauge Siena Video Capture";
        _encoderDeviceName = "Hauppauge Siena Encoder";
      }
      else if (device.Name.Equals(CROSSBAR_NAME_HDPVR60))
      {
        _deviceType = "HD-PVR 60";
        _crossbarDeviceName = device.Name;
        _captureDeviceName = "HD PVR 60 Capture Filter (HD)";
        _encoderDeviceName = "HD PVR 60 Encoder Filter (HD)";
      }

      _mapSubChannels = new Dictionary<Int32, BaseSubChannel>();
      _supportsSubChannels = true;
      _minChannel = 0;
      _maxChannel = 128;
      _camType = CamType.Default;
      _conditionalAccess = null;
      _cardType = CardType.Analog;
      _epgGrabbing = false;
      _configuration = Configuration.readConfiguration(_cardId, _name, _devicePath);
      Configuration.writeConfiguration(_configuration);
    }

    #endregion

    #region public methods

    /// <summary>
    /// Method to check if card can tune to the channel specified
    /// </summary>
    /// <returns>true if card can tune to the channel otherwise false</returns>
    public bool CanTune(IChannel channel)
    {
      if ((channel as AnalogChannel) == null || channel.IsRadio)
      {
        return false;
      }
      return true;
    }

    /// <summary>
    /// Stops the current graph
    /// </summary>
    /// <returns></returns>
    public override void PauseGraph()
    {
      FreeAllSubChannels();
      FilterState state;
      if (_graphBuilder == null)
        return;
      IMediaControl mediaCtl = (_graphBuilder as IMediaControl);
      if (mediaCtl == null)
      {
        throw new TvException("Can not convert graphBuilder to IMediaControl");
      }
      mediaCtl.GetState(10, out state);
      Log.Log.WriteFile("HDPVR: PauseGraph state:{0}", state);
      _isScanning = false;
      if (state != FilterState.Running)
      {
        _graphState = GraphState.Created;
        return;
      }
      int hr = mediaCtl.Pause();
      if (hr < 0 || hr > 1)
      {
        Log.Log.WriteFile("HDPVR: PauseGraph returns:0x{0:X}", hr);
        throw new TvException("Unable to pause graph");
      }
      Log.Log.WriteFile("HDPVR: Graph paused");
    }

    /// <summary>
    /// Stops the current graph
    /// </summary>
    /// <returns></returns>
    public override void StopGraph()
    {
      FreeAllSubChannels();
      FilterState state;
      if (_graphBuilder == null)
        return;
      IMediaControl mediaCtl = (_graphBuilder as IMediaControl);
      if (mediaCtl == null)
      {
        throw new TvException("Can not convert graphBuilder to IMediaControl");
      }
      mediaCtl.GetState(10, out state);
      Log.Log.WriteFile("HDPVR: StopGraph state:{0}", state);
      _isScanning = false;
      if (state == FilterState.Stopped)
      {
        _graphState = GraphState.Created;
        return;
      }
      int hr = mediaCtl.Stop();
      if (hr < 0 || hr > 1)
      {
        Log.Log.WriteFile("HDPVR: StopGraph returns:0x{0:X}", hr);
        throw new TvException("Unable to stop graph");
      }
      Log.Log.WriteFile("HDPVR: Graph stopped");
    }

    #endregion

    #region Channel linkage handling

    /// <summary>
    /// Starts scanning for linkage info
    /// </summary>
    public void StartLinkageScanner(BaseChannelLinkageScanner callback) {}

    /// <summary>
    /// Stops/Resets the linkage scanner
    /// </summary>
    public void ResetLinkageScanner() {}

    /// <summary>
    /// Returns the channel linkages grabbed
    /// </summary>
    public List<PortalChannel> ChannelLinkages
    {
      get { return null; }
    }

    #endregion

    #region epg & scanning

    /// <summary>
    /// Grabs the epg.
    /// </summary>
    /// <param name="callback">The callback which gets called when epg is received or canceled.</param>
    public void GrabEpg(BaseEpgGrabber callback) {}

    /// <summary>
    /// Start grabbing the epg while timeshifting
    /// </summary>
    public void GrabEpg() {}

    /// <summary>
    /// Aborts grabbing the epg. This also triggers the OnEpgReceived callback.
    /// </summary>
    public void AbortGrabbing() {}

    /// <summary>
    /// returns a list of all epg data for each channel found.
    /// </summary>
    /// <value>The epg.</value>
    public List<EpgChannel> Epg
    {
      get { return null; }
    }

    /// <summary>
    /// returns the ITVScanning interface used for scanning channels
    /// </summary>
    public ITVScanning ScanningInterface
    {
      get { return null; }
    }

    #endregion

    #region tuning & recording

    /// <summary>
    /// Scans the specified channel.
    /// </summary>
    /// <param name="subChannelId">The sub channel id.</param>
    /// <param name="channel">The channel.</param>
    /// <returns>true if succeeded else false</returns>
    public ITvSubChannel Scan(int subChannelId, IChannel channel)
    {
      return Tune(subChannelId, channel);
    }

    /// <summary>
    /// Tunes the specified channel.
    /// </summary>
    /// <param name="subChannelId">The sub channel id.</param>
    /// <param name="channel">The channel.</param>
    /// <returns>true if succeeded else false</returns>
    public ITvSubChannel Tune(int subChannelId, IChannel channel)
    {
      Log.Log.WriteFile("HDPVR: Tune:{0}, {1}", subChannelId, channel);
      if (_graphState == GraphState.Idle)
      {
        BuildGraph();
      }
      BaseSubChannel subChannel;
      if (_mapSubChannels.ContainsKey(subChannelId))
      {
        subChannel = _mapSubChannels[subChannelId];
      }
      else
      {
        subChannelId = GetNewSubChannel(channel);
        subChannel = _mapSubChannels[subChannelId];
      }
      subChannel.CurrentChannel = channel;
      subChannel.OnBeforeTune();
      PerformTuning(channel);
      subChannel.OnAfterTune();
      try
      {
        RunGraph(subChannelId);
      }
      catch (Exception)
      {
        FreeSubChannel(subChannelId);
        throw;
      }

      return subChannel;
    }

    #endregion

    #region subchannel management

    /// <summary>
    /// Allocates a new instance of HDPVRChannel which handles the new subchannel
    /// </summary>
    /// <returns>handle for to the subchannel</returns>
    private Int32 GetNewSubChannel(IChannel channel)
    {
      int id = _subChannelId++;
      Log.Log.Info("HDPVR: GetNewSubChannel:{0} #{1}", _mapSubChannels.Count, id);
      HDPVRChannel subChannel = new HDPVRChannel(this, _deviceType, id, _filterTsWriter, _graphBuilder);
      subChannel.Parameters = Parameters;
      subChannel.CurrentChannel = channel;
      _mapSubChannels[id] = subChannel;
      return id;
    }

    #endregion

    #region quality control

    /// <summary>
    /// Get/Set the quality
    /// </summary>
    public IQuality Quality
    {
      get { return _qualityControl; }
      set { }
    }

    /// <summary>
    /// Property which returns true if card supports quality control
    /// </summary>
    public bool SupportsQualityControl
    {
      get
      {
        if (_graphState == GraphState.Idle)
        {
          BuildGraph();
        }
        return _qualityControl != null;
      }
    }

    /// <summary>
    /// Reloads the quality control configuration
    /// </summary>
    public void ReloadCardConfiguration()
    {
      if (_qualityControl != null)
      {
        _configuration = Configuration.readConfiguration(_cardId, _name, _devicePath);
        Configuration.writeConfiguration(_configuration);
        _qualityControl.SetConfiguration(_configuration);
      }
    }

    #endregion

    #region properties

    /// <summary>
    /// A derrived class should update the signal informations of the tv cards
    /// </summary>
    protected override void UpdateSignalQuality(bool force)
    {
      UpdateSignalQuality();
    }

    /// <summary>
    /// When the tuner is locked onto a signal this property will return true
    /// otherwise false
    /// </summary>
    protected override void UpdateSignalQuality()
    {
      TimeSpan ts = DateTime.Now - _lastSignalUpdate;
      if (ts.TotalMilliseconds < 5000 || _graphState == GraphState.Idle)
      {
        _tunerLocked = false;
      }
      else
      {
        _tunerLocked = true;
      }
      if (_tunerLocked)
      {
        _signalLevel = 100;
        _signalQuality = 100;
      }
      else
      {
        _signalLevel = 0;
        _signalQuality = 0;
      }
    }

    /// <summary>
    /// Gets or sets the unique id of this card
    /// </summary>
    public override int CardId
    {
      get { return _cardId; }
      set
      {
        _cardId = value;
        _configuration = Configuration.readConfiguration(_cardId, _name, _devicePath);
        Configuration.writeConfiguration(_configuration);
      }
    }

    #endregion

    #region Disposable

    /// <summary>
    /// Disposes this instance.
    /// </summary>
    public virtual void Dispose()
    {
      if (_graphBuilder == null)
        return;
      Log.Log.WriteFile("HDPVR:  Dispose()");
      if (_graphState == GraphState.TimeShifting || _graphState == GraphState.Recording)
      {
        // Stop the graph first. To ensure that the timeshift files are no longer blocked
        StopGraph();
      }
      FreeAllSubChannels();
      // Decompose the graph
      IMediaControl mediaCtl = (_graphBuilder as IMediaControl);
      if (mediaCtl == null)
      {
        throw new TvException("Can not convert graphBuilder to IMediaControl");
      }
      // Decompose the graph
      mediaCtl.Stop();
      FilterGraphTools.RemoveAllFilters(_graphBuilder);
      Log.Log.WriteFile("HDPVR:  All filters removed");
      if (_filterCrossBar != null)
      {
        while (Release.ComObject(_filterCrossBar) > 0) {}
        _filterCrossBar = null;
      }
      if (_filterCapture != null)
      {
        while (Release.ComObject(_filterCapture) > 0) {}
        _filterCapture = null;
      }
      if (_filterEncoder != null)
      {
        while (Release.ComObject(_filterEncoder) > 0) {}
        _filterEncoder = null;
      }
      if (_filterTsWriter != null)
      {
        while (Release.ComObject(_filterTsWriter) > 0) {}
        _filterTsWriter = null;
      }
      _rotEntry.Dispose();
      Release.ComObject("Graphbuilder", _graphBuilder);
      _graphBuilder = null;
      DevicesInUse.Instance.Remove(_tunerDevice);
      _graphState = GraphState.Idle;
      if (_crossBarDevice != null)
      {
        DevicesInUse.Instance.Remove(_crossBarDevice);
        _crossBarDevice = null;
      }
      if (_captureDevice != null)
      {
        DevicesInUse.Instance.Remove(_captureDevice);
        _captureDevice = null;
      }
      if (_encoderDevice != null)
      {
        DevicesInUse.Instance.Remove(_encoderDevice);
        _encoderDevice = null;
      }
      _graphState = GraphState.Idle;
      Log.Log.WriteFile("HDPVR:  dispose completed");
    }

    public void CancelTune(int subChannel)
    {
    }

    public event OnNewSubChannelDelegate OnNewSubChannelEvent;

    #endregion

    #region graph handling

    /// <summary>
    /// Builds the directshow graph for this analog tvcard
    /// </summary>
    public override void BuildGraph()
    {
      if (_cardId == 0)
      {
        GetPreloadBitAndCardId();
        _configuration = Configuration.readConfiguration(_cardId, _name, _devicePath);
        Configuration.writeConfiguration(_configuration);
      }

      _lastSignalUpdate = DateTime.MinValue;
      _tunerLocked = false;
      Log.Log.WriteFile("HDPVR: build graph");
      try
      {
        if (_graphState != GraphState.Idle)
        {
          Log.Log.WriteFile("HDPVR: graph already built!");
          throw new TvException("Graph already built");
        }
        _graphBuilder = (IFilterGraph2)new FilterGraph();
        _rotEntry = new DsROTEntry(_graphBuilder);
        _capBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
        _capBuilder.SetFiltergraph(_graphBuilder);
        AddCrossBarFilter();
        AddCaptureFilter();
        AddEncoderFilter();
        AddTsWriterFilterToGraph();
        _qualityControl = QualityControlFactory.createQualityControl(_configuration, _filterEncoder, _filterCapture,
                                                                     null, null);
        if (_qualityControl == null)
        {
          Log.Log.WriteFile("HDPVR: No quality control support found");
        }

        _graphState = GraphState.Created;
        _configuration.Graph.Crossbar.Name = _crossBarDevice.Name;
        _configuration.Graph.Crossbar.VideoPinMap = _videoPinMap;
        _configuration.Graph.Crossbar.AudioPinMap = _audioPinMap;
        _configuration.Graph.Crossbar.VideoPinRelatedAudioMap = _videoPinRelatedAudioMap;
        _configuration.Graph.Crossbar.VideoOut = _videoOutPinIndex;
        _configuration.Graph.Crossbar.AudioOut = _audioOutPinIndex;
        _configuration.Graph.Capture.Name = _captureDevice.Name;
        _configuration.Graph.Capture.FrameRate = -1d;
        _configuration.Graph.Capture.ImageHeight = -1;
        _configuration.Graph.Capture.ImageWidth = -1;
        Configuration.writeConfiguration(_configuration);
      }
      catch (Exception ex)
      {
        Log.Log.Write(ex);
        Dispose();
        _graphState = GraphState.Idle;
        throw;
      }
    }

    private void AddCrossBarFilter()
    {
      Log.Log.WriteFile("HDPVR: Add Crossbar Filter");
      //get list of all crossbar devices installed on this system
      _crossBarDevice = _tunerDevice;
      IBaseFilter tmp;
      int hr;
      try
      {
        //add the crossbar to the graph
        hr = _graphBuilder.AddSourceFilterForMoniker(_crossBarDevice.Mon, null, _crossBarDevice.Name, out tmp);
      }
      catch (Exception)
      {
        Log.Log.WriteFile("HDPVR: cannot add filter to graph");
        throw new TvException("Unable to add crossbar to graph");
      }
      if (hr == 0)
      {
        _filterCrossBar = tmp;
        CheckCapabilities();
        return;
      }
      Log.Log.WriteFile("HDPVR: cannot add filter to graph");
      throw new TvException("Unable to add crossbar to graph");
    }

    private void AddCaptureFilter()
    {
      DsDevice[] devices;
      Log.Log.WriteFile("HDPVR: Add Capture Filter");
      //get a list of all video capture devices
      try
      {
        devices = DsDevice.GetDevicesOfCat(FilterCategory.AMKSCapture);
        devices = DeviceSorter.Sort(devices, _tunerDevice, _filterCrossBar, _captureDevice, _filterEncoder);
      }
      catch (Exception)
      {
        Log.Log.WriteFile("HDPVR: AddTvCaptureFilter no tvcapture devices found");
        return;
      }
      if (devices.Length == 0)
      {
        Log.Log.WriteFile("HDPVR: AddTvCaptureFilter no tvcapture devices found");
        return;
      }
      //try each video capture filter
      for (int i = 0; i < devices.Length; i++)
      {
        if (devices[i].Name != _captureDeviceName)
        {
          continue;
        }
        Log.Log.WriteFile("HDPVR: AddTvCaptureFilter try:{0} {1}", devices[i].Name, i);
        // if video capture filter is in use, then we can skip it
        if (DevicesInUse.Instance.IsUsed(devices[i]))
        {
          continue;
        }
        IBaseFilter tmp;
        int hr;
        try
        {
          // add video capture filter to graph
          hr = _graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out tmp);
        }
        catch (Exception)
        {
          Log.Log.WriteFile("HDPVR: cannot add filter to graph");
          continue;
        }
        if (hr != 0)
        {
          //cannot add video capture filter to graph, try next one
          if (tmp != null)
          {
            _graphBuilder.RemoveFilter(tmp);
            Release.ComObject("TvCaptureFilter", tmp);
          }
          continue;
        }
        // connect crossbar->video capture filter
        hr = _capBuilder.RenderStream(null, null, _filterCrossBar, null, tmp);
        if (hr == 0)
        {
          // That worked. Since most crossbar devices require 2 connections from
          // crossbar->video capture filter, we do it again to connect the 2nd pin
          _capBuilder.RenderStream(null, null, _filterCrossBar, null, tmp);
          _filterCapture = tmp;
          _captureDevice = devices[i];
          DevicesInUse.Instance.Add(_captureDevice);
          Log.Log.WriteFile("HDPVR: AddTvCaptureFilter connected to crossbar successfully");
          break;
        }
        // cannot connect crossbar->video capture filter, remove filter from graph
        // cand continue with the next vieo capture filter
        Log.Log.WriteFile("HDPVR: AddTvCaptureFilter failed to connect to crossbar");
        _graphBuilder.RemoveFilter(tmp);
        Release.ComObject("capture filter", tmp);
      }
      if (_filterCapture == null)
      {
        Log.Log.Error("HDPVR: unable to add TvCaptureFilter to graph");
        //throw new TvException("Unable to add TvCaptureFilter to graph");
      }
    }

    private void AddEncoderFilter()
    {
      DsDevice[] devices;
      Log.Log.WriteFile("HDPVR: AddEncoderFilter");
      // first get all encoder filters available on this system
      try
      {
        devices = DsDevice.GetDevicesOfCat(FilterCategory.WDMStreamingEncoderDevices);
        devices = DeviceSorter.Sort(devices, _tunerDevice, _filterCrossBar, _captureDevice, _filterEncoder);
      }
      catch (Exception)
      {
        Log.Log.WriteFile("HDPVR: AddTvEncoderFilter no encoder devices found (Exception)");
        return;
      }

      if (devices == null)
      {
        Log.Log.WriteFile("HDPVR: AddTvEncoderFilter no encoder devices found (devices == null)");
        return;
      }

      if (devices.Length == 0)
      {
        Log.Log.WriteFile("HDPVR: AddTvEncoderFilter no encoder devices found");
        return;
      }

      //for each encoder
      Log.Log.WriteFile("HDPVR: AddTvEncoderFilter found:{0} encoders", devices.Length);
      for (int i = 0; i < devices.Length; i++)
      {
        if (devices[i].Name != _encoderDeviceName)
        {
          continue;
        }

        //if encoder is in use, we can skip it
        if (DevicesInUse.Instance.IsUsed(devices[i]))
        {
          Log.Log.WriteFile("HDPVR:  skip :{0} (inuse)", devices[i].Name);
          continue;
        }

        Log.Log.WriteFile("HDPVR:  try encoder:{0} {1}", devices[i].Name, i);
        IBaseFilter tmp;
        int hr;
        try
        {
          //add encoder filter to graph
          hr = _graphBuilder.AddSourceFilterForMoniker(devices[i].Mon, null, devices[i].Name, out tmp);
        }
        catch (Exception)
        {
          Log.Log.WriteFile("HDPVR: cannot add filter {0} to graph", devices[i].Name);
          continue;
        }
        if (hr != 0)
        {
          //failed to add filter to graph, continue with the next one
          if (tmp != null)
          {
            _graphBuilder.RemoveFilter(tmp);
            Release.ComObject("TvEncoderFilter", tmp);
          }
          continue;
        }
        if (tmp == null)
        {
          continue;
        }
        hr = _capBuilder.RenderStream(null, null, _filterCapture, null, tmp);
        if (hr == 0)
        {
          // That worked. Since most crossbar devices require 2 connections from
          // crossbar->video capture filter, we do it again to connect the 2nd pin
          _capBuilder.RenderStream(null, null, _filterCapture, null, tmp);
          _filterEncoder = tmp;
          _encoderDevice = devices[i];
          DevicesInUse.Instance.Add(_encoderDevice);
          Log.Log.WriteFile("HDPVR: AddTvEncoderFilter connected to catpure successfully");
          //and we're done
          return;
        }
        // cannot connect crossbar->video capture filter, remove filter from graph
        // cand continue with the next vieo capture filter
        Log.Log.WriteFile("HDPVR: AddTvEncoderFilter failed to connect to capture");
        _graphBuilder.RemoveFilter(tmp);
        Release.ComObject("capture filter", tmp);
      }
      Log.Log.WriteFile("HDPVR: AddTvEncoderFilter no encoder found");
    }

    private void AddTsWriterFilterToGraph()
    {
      if (_filterTsWriter == null)
      {
        Log.Log.WriteFile("HDPVR: Add Mediaportal TsWriter filter");
        _filterTsWriter = (IBaseFilter)new MpTsAnalyzer();
        int hr = _graphBuilder.AddFilter(_filterTsWriter, "MediaPortal Ts Analyzer");
        if (hr != 0)
        {
          Log.Log.Error("HDPVR:  Add main Ts Analyzer returns:0x{0:X}", hr);
          throw new TvException("Unable to add Ts Analyzer filter");
        }
        IPin pinOut = DsFindPin.ByDirection(_filterEncoder, PinDirection.Output, 0);
        if (pinOut == null)
        {
          Log.Log.Error("HDPVR:  Unable to find output pin on the encoder filter");
          throw new TvException("unable to find output pin on the encoder filter");
        }
        IPin pinIn = DsFindPin.ByDirection(_filterTsWriter, PinDirection.Input, 0);
        if (pinIn == null)
        {
          Log.Log.Error("HDPVR:  Unable to find the input pin on ts analyzer filter");
          throw new TvException("Unable to find the input pin on ts analyzer filter");
        }
        //Log.Log.Info("HDPVR: Render [Encoder]->[TsWriter]");
        hr = _graphBuilder.Connect(pinOut, pinIn);
        Release.ComObject("pinTsWriterIn", pinIn);
        Release.ComObject("pinEncoderOut", pinOut);
        if (hr != 0)
        {
          Log.Log.Error("HDPVR:  Unable to connect encoder to ts analyzer filter :0x{0:X}", hr);
          throw new TvException("unable to connect encoder to ts analyzer filter");
        }
        Log.Log.WriteFile("HDPVR: AddTsWriterFilterToGraph connected to encoder successfully");
      }
    }

    /// <summary>
    /// Method which starts the graph
    /// </summary>
    public override void RunGraph(int subChannel)
    {
      bool graphRunning = GraphRunning();

      if (_mapSubChannels.ContainsKey(subChannel))
      {
        _mapSubChannels[subChannel].AfterTuneEvent -= new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        _mapSubChannels[subChannel].AfterTuneEvent += new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        _mapSubChannels[subChannel].OnGraphStart();
      }

      if (graphRunning)
      {
        Log.Log.Write("HDPVR: Graph already running");
        return;
      }

      int hr = 0;
      IMediaControl mediaCtrl = _graphBuilder as IMediaControl;
      if (mediaCtrl == null)
      {
        Log.Log.WriteFile("HDPVR: RunGraph returns:0x{0:X}", hr);
        throw new TvException("Unable to start graph");
      }
      Log.Log.WriteFile("HDPVR: RunGraph");
      hr = mediaCtrl.Run();
      if (hr < 0 || hr > 1)
      {
        Log.Log.WriteFile("HDPVR: RunGraph returns:0x{0:X}", hr);
        throw new TvException("Unable to start graph");
      }
      if (GraphRunning())
      {
        Log.Log.Write("HDPVR: Graph running");
      }
      if (_mapSubChannels.ContainsKey(subChannel))
      {
        _mapSubChannels[subChannel].AfterTuneEvent -= new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        _mapSubChannels[subChannel].AfterTuneEvent += new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        _mapSubChannels[subChannel].OnGraphStarted();
      }
    }

    #endregion

    #region private helper

    private void PerformTuning(IChannel channel)
    {
      Log.Log.WriteFile("HDPVR: Tune");
      AnalogChannel analogChannel = channel as AnalogChannel;
      if (analogChannel == null)
      {
        throw new NullReferenceException();
      }

      // Set up the crossbar.
      IAMCrossbar crossBarFilter = _filterCrossBar as IAMCrossbar;

      // Video
      if ((_previousChannel == null || _previousChannel.VideoSource != analogChannel.VideoSource) &&
        _videoPinMap.ContainsKey(analogChannel.VideoSource))
      {
        Log.Log.WriteFile("HDPVR:   video input -> {0}", analogChannel.VideoSource);
        crossBarFilter.Route(_videoOutPinIndex, _videoPinMap[analogChannel.VideoSource]);
      }

      // Audio
      if (analogChannel.AudioSource == AnalogChannel.AudioInputType.Automatic)
      {
        if (_videoPinRelatedAudioMap.ContainsKey(analogChannel.VideoSource))
        {
          Log.Log.WriteFile("HDPVR:   audio input -> (auto)");
          crossBarFilter.Route(_audioOutPinIndex, _videoPinRelatedAudioMap[analogChannel.VideoSource]);
        }
      }
      else if ((_previousChannel == null || _previousChannel.AudioSource != analogChannel.AudioSource) &&
        _audioPinMap.ContainsKey(analogChannel.AudioSource))
      {
        Log.Log.WriteFile("HDPVR:   audio input -> {0}", analogChannel.AudioSource);
        crossBarFilter.Route(_audioOutPinIndex, _audioPinMap[analogChannel.AudioSource]);
      }

      _lastSignalUpdate = DateTime.MinValue;
      _tunerLocked = false;
      _previousChannel = analogChannel;
      Log.Log.WriteFile("HDPVR: Tuned to channel {0}", channel.Name);
      if (_graphState == GraphState.Idle)
      {
        _graphState = GraphState.Created;
      }
      _lastSignalUpdate = DateTime.MinValue;
    }

    /// <summary>
    /// Checks the capabilities
    /// </summary>
    private void CheckCapabilities()
    {
      IAMCrossbar crossBarFilter = _filterCrossBar as IAMCrossbar;
      if (crossBarFilter != null)
      {
        int outputs, inputs;
        crossBarFilter.get_PinCounts(out outputs, out inputs);
        _videoOutPinIndex = -1;
        _audioOutPinIndex = -1;
        _videoPinMap = new Dictionary<AnalogChannel.VideoInputType, int>();
        _audioPinMap = new Dictionary<AnalogChannel.AudioInputType, int>();
        _videoPinRelatedAudioMap = new Dictionary<AnalogChannel.VideoInputType, int>();
        int relatedPinIndex;
        PhysicalConnectorType connectorType;
        for (int i = 0; i < outputs; ++i)
        {
          crossBarFilter.get_CrossbarPinInfo(false, i, out relatedPinIndex, out connectorType);
          if (connectorType == PhysicalConnectorType.Video_VideoDecoder)
          {
            _videoOutPinIndex = i;
          }
          if (connectorType == PhysicalConnectorType.Audio_AudioDecoder)
          {
            _audioOutPinIndex = i;
          }
        }

        int audioLine = 0;
        int audioSPDIF = 0;
        int audioAux = 0;
        int audioAes = 0;
        int videoCvbsNr = 0;
        int videoSvhsNr = 0;
        int videoYrYbYNr = 0;
        int videoRgbNr = 0;
        int videoHdmiNr = 0;
        for (int i = 0; i < inputs; ++i)
        {
          crossBarFilter.get_CrossbarPinInfo(true, i, out relatedPinIndex, out connectorType);
          Log.Log.Write(" crossbar pin:{0} type:{1}", i, connectorType);
          switch (connectorType)
          {
            case PhysicalConnectorType.Audio_Tuner:
              _audioPinMap.Add(AnalogChannel.AudioInputType.Tuner, i);
              break;
            case PhysicalConnectorType.Video_Tuner:
              _videoPinMap.Add(AnalogChannel.VideoInputType.Tuner, i);
              _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.Tuner, relatedPinIndex);
              break;
            case PhysicalConnectorType.Audio_Line:
              audioLine++;
              switch (audioLine)
              {
                case 1:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.LineInput1, i);
                  break;
                case 2:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.LineInput2, i);
                  break;
                case 3:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.LineInput3, i);
                  break;
              }
              break;
            case PhysicalConnectorType.Audio_SPDIFDigital:
              audioSPDIF++;
              switch (audioSPDIF)
              {
                case 1:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.SPDIFInput1, i);
                  break;
                case 2:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.SPDIFInput2, i);
                  break;
                case 3:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.SPDIFInput3, i);
                  break;
              }
              break;
            case PhysicalConnectorType.Audio_AUX:
              audioAux++;
              switch (audioAux)
              {
                case 1:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.AUXInput1, i);
                  break;
                case 2:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.AUXInput2, i);
                  break;
                case 3:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.AUXInput3, i);
                  break;
              }
              break;
            case PhysicalConnectorType.Audio_AESDigital:
              audioAes++;
              switch (audioAes)
              {
                case 1:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.AesInput1, i);
                  break;
                case 2:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.AesInput2, i);
                  break;
                case 3:
                  _audioPinMap.Add(AnalogChannel.AudioInputType.AesInput3, i);
                  break;
              }
              break;
            case PhysicalConnectorType.Video_Composite:
              videoCvbsNr++;
              switch (videoCvbsNr)
              {
                case 1:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.VideoInput1, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.VideoInput1, relatedPinIndex);
                  break;
                case 2:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.VideoInput2, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.VideoInput2, relatedPinIndex);
                  break;
                case 3:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.VideoInput3, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.VideoInput3, relatedPinIndex);
                  break;
              }
              break;
            case PhysicalConnectorType.Video_SVideo:
              videoSvhsNr++;
              switch (videoSvhsNr)
              {
                case 1:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.SvhsInput1, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.SvhsInput1, relatedPinIndex);
                  break;
                case 2:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.SvhsInput2, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.SvhsInput2, relatedPinIndex);
                  break;
                case 3:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.SvhsInput3, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.SvhsInput3, relatedPinIndex);
                  break;
              }
              break;
            case PhysicalConnectorType.Video_RGB:
              videoRgbNr++;
              switch (videoRgbNr)
              {
                case 1:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.RgbInput1, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.RgbInput1, relatedPinIndex);
                  break;
                case 2:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.RgbInput2, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.RgbInput2, relatedPinIndex);
                  break;
                case 3:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.RgbInput3, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.RgbInput3, relatedPinIndex);
                  break;
              }
              break;
            case PhysicalConnectorType.Video_YRYBY:
              videoYrYbYNr++;
              switch (videoYrYbYNr)
              {
                case 1:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.YRYBYInput1, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.YRYBYInput1, relatedPinIndex);
                  break;
                case 2:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.YRYBYInput2, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.YRYBYInput2, relatedPinIndex);
                  break;
                case 3:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.YRYBYInput3, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.YRYBYInput3, relatedPinIndex);
                  break;
              }
              break;
            case PhysicalConnectorType.Video_SerialDigital:
            case PhysicalConnectorType.Video_ParallelDigital:
              videoHdmiNr++;
              switch (videoHdmiNr)
              {
                case 1:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.HdmiInput1, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.HdmiInput1, relatedPinIndex);
                  break;
                case 2:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.HdmiInput2, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.HdmiInput2, relatedPinIndex);
                  break;
                case 3:
                  _videoPinMap.Add(AnalogChannel.VideoInputType.HdmiInput3, i);
                  _videoPinRelatedAudioMap.Add(AnalogChannel.VideoInputType.HdmiInput3, relatedPinIndex);
                  break;
              }
              break;
          }
        }
      }
    }

    #endregion

    #region abstract implemented Methods

    /// <summary>
    /// A derrived class should activate / deactivate the scanning
    /// </summary>
    protected override void OnScanning() {}

    /// <summary>
    /// A derrived class should activate / deactivate the epg grabber
    /// </summary>
    /// <param name="value">Mode</param>
    protected override void UpdateEpgGrabber(bool value) {}

    #endregion
  }
}