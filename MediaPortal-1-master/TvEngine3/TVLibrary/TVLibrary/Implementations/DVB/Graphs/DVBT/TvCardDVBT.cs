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
using DirectShowLib;
using DirectShowLib.BDA;
using TvLibrary.Interfaces;
using TvLibrary.Channels;
using TvLibrary.Implementations.Helper;
using TvDatabase;
using TvLibrary.Epg;

namespace TvLibrary.Implementations.DVB
{
  /// <summary>
  /// Implementation of <see cref="T:TvLibrary.Interfaces.ITVCard"/> which handles DVB-T BDA cards
  /// </summary>
  public class TvCardDVBT : TvCardDvbBase
  {
    #region variables

    #endregion

    #region ctor

    /// <summary>
    /// Initializes a new instance of the <see cref="TvCardDVBT"/> class.
    /// </summary>
    /// <param name="device">The device.</param>
    public TvCardDVBT(DsDevice device)
      : base(device)
    {
      _cardType = CardType.DvbT;
    }

    #endregion

    #region graphbuilding

    /// <summary>
    /// Builds the graph.
    /// </summary>
    public override void BuildGraph()
    {
      try
      {
        if (_graphState != GraphState.Idle)
        {
          Log.Log.Error("dvbt:Graph already built");
          throw new TvException("Graph already build");
        }
        Log.Log.WriteFile("dvbt:BuildGraph");
        _graphBuilder = (IFilterGraph2)new FilterGraph();
        _capBuilder = (ICaptureGraphBuilder2)new CaptureGraphBuilder2();
        _capBuilder.SetFiltergraph(_graphBuilder);
        _rotEntry = new DsROTEntry(_graphBuilder);
        AddNetworkProviderFilter(typeof (DVBTNetworkProvider).GUID);
        AddTsWriterFilterToGraph();
        if (!useInternalNetworkProvider)
        {
          CreateTuningSpace();
          AddMpeg2DemuxerToGraph();
        }
        IBaseFilter lastFilter;
        AddAndConnectBDABoardFilters(_device, out lastFilter);
        CompleteGraph(ref lastFilter);
        string graphName = _device.Name + " - DVBT Graph.grf";
        FilterGraphTools.SaveGraphFile(_graphBuilder, graphName);
        GetTunerSignalStatistics();
        _graphState = GraphState.Created;
      }
      catch (Exception ex)
      {
        Log.Log.Write(ex);
        Dispose();
        _graphState = GraphState.Idle;
        throw new TvExceptionGraphBuildingFailed("Graph building failed", ex);
      }
    }

    /// <summary>
    /// Creates the tuning space.
    /// </summary>
    protected void CreateTuningSpace()
    {
      Log.Log.WriteFile("dvbt:CreateTuningSpace()");
      ITuner tuner = (ITuner)_filterNetworkProvider;
      SystemTuningSpaces systemTuningSpaces = new SystemTuningSpaces();
      ITuningSpaceContainer container = systemTuningSpaces as ITuningSpaceContainer;
      if (container == null)
      {
        Log.Log.Error("CreateTuningSpace() Failed to get ITuningSpaceContainer");
        return;
      }
      IEnumTuningSpaces enumTuning;
      ITuningSpace[] spaces = new ITuningSpace[2];
      ITuneRequest request;
      container.get_EnumTuningSpaces(out enumTuning);
      while (true)
      {
        int fetched;
        enumTuning.Next(1, spaces, out fetched);
        if (fetched != 1)
          break;
        string name;
        spaces[0].get_UniqueName(out name);
        if (name == "MediaPortal DVBT TuningSpace")
        {
          Log.Log.WriteFile("dvbt:found correct tuningspace {0}", name);
          _tuningSpace = (IDVBTuningSpace)spaces[0];
          tuner.put_TuningSpace(_tuningSpace);
          _tuningSpace.CreateTuneRequest(out request);
          _tuneRequest = (IDVBTuneRequest)request;
          return;
        }
        Release.ComObject("ITuningSpace", spaces[0]);
      }
      Release.ComObject("IEnumTuningSpaces", enumTuning);
      Log.Log.WriteFile("dvbt:Create new tuningspace");
      _tuningSpace = (IDVBTuningSpace)new DVBTuningSpace();
      IDVBTuningSpace tuningSpace = (IDVBTuningSpace)_tuningSpace;
      tuningSpace.put_UniqueName("MediaPortal DVBT TuningSpace");
      tuningSpace.put_FriendlyName("MediaPortal DVBT TuningSpace");
      tuningSpace.put__NetworkType(typeof (DVBTNetworkProvider).GUID);
      tuningSpace.put_SystemType(DVBSystemType.Terrestrial);

      IDVBTLocator locator = (IDVBTLocator)new DVBTLocator();
      locator.put_CarrierFrequency(-1);
      locator.put_InnerFEC(FECMethod.MethodNotSet);
      locator.put_InnerFECRate(BinaryConvolutionCodeRate.RateNotSet);
      locator.put_Modulation(ModulationType.ModNotSet);
      locator.put_OuterFEC(FECMethod.MethodNotSet);
      locator.put_OuterFECRate(BinaryConvolutionCodeRate.RateNotSet);
      locator.put_SymbolRate(-1);
      object newIndex;
      _tuningSpace.put_DefaultLocator(locator);
      container.Add(_tuningSpace, out newIndex);
      tuner.put_TuningSpace(_tuningSpace);
      Release.ComObject("ITuningSpaceContainer", container);
      _tuningSpace.CreateTuneRequest(out request);
      _tuneRequest = request;
    }

    /// <summary>
    /// Methods which stops the graph
    /// </summary>
    public override void StopGraph()
    {
      base.StopGraph();
      _previousChannel = null;
    }

    #endregion

    #region tuning & recording

    /// <summary>
    /// Tunes the specified channel.
    /// </summary>
    /// <param name="subChannelId">The sub channel id</param>
    /// <param name="channel">The channel.</param>
    /// <returns></returns>
    public override ITvSubChannel Scan(int subChannelId, IChannel channel)
    {
      Log.Log.WriteFile("dvbt: Scan:{0}", channel);
      try
      {
        if (!BeforeTune(channel, ref subChannelId))
        {
          return null;
        }

        ITvSubChannel ch = base.Scan(subChannelId, channel);

        Log.Log.Info("dvbt: tune: Graph running. Returning {0}", ch.ToString());
        return ch;
      }
      catch (TvExceptionNoSignal)
      {
        throw;
      }
      catch (TvExceptionNoPMT)
      {
        throw;
      }
      catch (TvExceptionTuneCancelled)
      {
        throw;
      }
      catch (Exception ex)
      {
        Log.Log.Write(ex);
        throw;
      }
    }

    /// <summary>
    /// Tunes the specified channel.
    /// </summary>
    /// <param name="subChannelId">The sub channel id</param>
    /// <param name="channel">The channel.</param>
    /// <returns></returns>
    public override ITvSubChannel Tune(int subChannelId, IChannel channel)
    {
      Log.Log.WriteFile("dvbt: Tune:{0}", channel);
      try
      {
        if (!BeforeTune(channel, ref subChannelId))
        {
          return null;
        }

        ITvSubChannel ch = base.Tune(subChannelId, channel);

        Log.Log.Info("dvbt: tune: Graph running. Returning {0}", ch.ToString());
        return ch;
      }
      catch (TvExceptionTuneCancelled)
      {
        throw;
      }
      catch (TvExceptionNoSignal)
      {
        throw;
      }
      catch (TvExceptionNoPMT)
      {
        throw;
      }      
      catch (Exception ex)
      {
        Log.Log.Write(ex);
        throw;
      }
    }

    private bool BeforeTune(IChannel channel, ref int subChannelId)
    {
      DVBTChannel dvbtChannel = channel as DVBTChannel;
      if (dvbtChannel == null)
      {
        Log.Log.WriteFile("dvbt:Channel is not a DVBT channel!!! {0}", channel.GetType().ToString());
        return false;
      }
      if (_graphState == GraphState.Idle)
      {
        BuildGraph();
        if (_mapSubChannels.ContainsKey(subChannelId) == false)
        {
          subChannelId = GetNewSubChannel(channel);
        }
      }
      if (useInternalNetworkProvider)
      {
        return true;
      }

      if (_previousChannel == null || _previousChannel.IsDifferentTransponder(dvbtChannel))
      {
        //_pmtPid = -1;
        ILocator locator;
        _tuningSpace.get_DefaultLocator(out locator);
        IDVBTLocator dvbtLocator = (IDVBTLocator)locator;
        dvbtLocator.put_Bandwidth(dvbtChannel.BandWidth);
        IDVBTuneRequest tuneRequest = (IDVBTuneRequest)_tuneRequest;
        tuneRequest.put_ONID(dvbtChannel.NetworkId);
        tuneRequest.put_SID(dvbtChannel.ServiceId);
        tuneRequest.put_TSID(dvbtChannel.TransportId);
        locator.put_CarrierFrequency((int)dvbtChannel.Frequency);
        _tuneRequest.put_Locator(locator);
      }
      return true;
    }

    #endregion

    #region epg & scanning

    /// <summary>
    /// checks if a received EPGChannel should be filtered from the resultlist
    /// </summary>
    /// <value></value>
    protected override bool FilterOutEPGChannel(EpgChannel epgChannel)
    {
      TvBusinessLayer layer = new TvBusinessLayer();
      if (layer.GetSetting("generalGrapOnlyForSameTransponder", "no").Value == "yes")
      {
        DVBBaseChannel chan = epgChannel.Channel as DVBBaseChannel;
        Channel dbchannel = layer.GetChannelByTuningDetail(chan.NetworkId, chan.TransportId, chan.ServiceId);
        DVBTChannel dvbtchannel = new DVBTChannel();
        if (dbchannel == null)
        {
          return false;
        }
        foreach (TuningDetail detail in dbchannel.ReferringTuningDetail())
        {
          if (detail.ChannelType == 4)
          {
            dvbtchannel.Frequency = detail.Frequency;
            dvbtchannel.BandWidth = detail.Bandwidth;
          }
        }
        return this.CurrentChannel.IsDifferentTransponder(dvbtchannel);
      }
      else
        return false;

    }

    /// <summary>
    /// returns the ITVScanning interface used for scanning channels
    /// </summary>
    /// <value></value>
    public override ITVScanning ScanningInterface
    {
      get
      {
        if (!CheckThreadId())
          return null;
        return new DVBTScanning(this);
      }
    }

    #endregion

    /// <summary>
    /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </summary>
    /// <returns>
    /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override string ToString()
    {
      return _name;
    }

    /// <summary>
    /// Method to check if card can tune to the channel specified
    /// </summary>
    /// <param name="channel"></param>
    /// <returns>
    /// true if card can tune to the channel otherwise false
    /// </returns>
    public override bool CanTune(IChannel channel)
    {
      if ((channel as DVBTChannel) == null)
        return false;
      return true;
    }

    protected override DVBBaseChannel CreateChannel(int networkid, int transportid, int serviceid, string name)
    {
      DVBTChannel channel = new DVBTChannel();
      channel.NetworkId = networkid;
      channel.TransportId = transportid;
      channel.ServiceId = serviceid;
      channel.Name = name;
      return channel;
    }
  }
}