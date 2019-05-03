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
using System.Runtime.InteropServices;
using DirectShowLib;
using TvLibrary.Interfaces;

namespace TvLibrary.Implementations.DVB
{
  /// <summary>
  /// DVB IP class based on Elecard
  /// </summary>
  public class TvCardDVBIPElecard : TvCardDVBIP
  {
    /// <summary>
    /// CLSID_ElecardNWSourcePlus
    /// </summary>
    [ComImport, Guid("62341545-9318-4671-9D62-9CAACDD5D20A")]
    public class ElecardNWSourcePlus {}

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="device"></param>
    /// <param name="sequence"></param>
    public TvCardDVBIPElecard(DsDevice device, int sequence)
      : base(device, sequence)
    {
      _defaultUrl = "elecard://0.0.0.0:1234:t=m2t/udp";
    }

    /// <summary>
    /// AddStreamSourceFilter
    /// </summary>
    /// <param name="url"></param>
    protected override void AddStreamSourceFilter(string url)
    {
      Log.Log.WriteFile("dvbip:Add NWSource-Plus");
      _filterStreamSource = FilterGraphTools.AddFilterFromClsid(_graphBuilder, typeof (ElecardNWSourcePlus).GUID,
                                                                "Elecard NWSource-Plus");
      AMMediaType mpeg2ProgramStream = new AMMediaType();
      mpeg2ProgramStream.majorType = MediaType.Stream;
      mpeg2ProgramStream.subType = MediaSubType.Mpeg2Transport;
      mpeg2ProgramStream.unkPtr = IntPtr.Zero;
      mpeg2ProgramStream.sampleSize = 0;
      mpeg2ProgramStream.temporalCompression = false;
      mpeg2ProgramStream.fixedSizeSamples = true;
      mpeg2ProgramStream.formatType = FormatType.None;
      mpeg2ProgramStream.formatSize = 0;
      mpeg2ProgramStream.formatPtr = IntPtr.Zero;
      ((IFileSourceFilter)_filterStreamSource).Load(url, mpeg2ProgramStream);
      //connect the [stream source] -> [inf tee]
      Log.Log.WriteFile("dvb:  Render [source]->[inftee]");
      int hr = _capBuilder.RenderStream(null, null, _filterStreamSource, null, _infTeeMain);
      if (hr != 0)
      {
        Log.Log.Error("dvb:Add source returns:0x{0:X}", hr);
        throw new TvException("Unable to add  source filter");
      }
    }

    /// <summary>
    /// RemoveStreamSourceFilter
    /// </summary>
    protected override void RemoveStreamSourceFilter()
    {
      if (_filterStreamSource != null)
      {
        _graphBuilder.RemoveFilter(_filterStreamSource);
        Release.ComObject("Elecard NWSource-Plus", _filterStreamSource);
        _filterStreamSource = null;
      }
    }

    /// <summary>
    /// RunGraph
    /// </summary>
    /// <param name="subChannel"></param>
    /// <param name="url"></param>
    protected override void RunGraph(int subChannel, string url)
    {
      int hr;
      FilterState state;
      (_graphBuilder as IMediaControl).GetState(10, out state);
      if (state == FilterState.Running)
      {
        hr = (_graphBuilder as IMediaControl).StopWhenReady();
        if (hr < 0 || hr > 1)
        {
          Log.Log.WriteFile("dvb:  StopGraph returns: 0x{0:X}", hr);
          throw new TvException("Unable to stop graph");
        }
        if (_mapSubChannels.ContainsKey(subChannel))
        {
          _mapSubChannels[subChannel].OnGraphStopped();
        }
      }
      if (_mapSubChannels.ContainsKey(subChannel))
      {
        _mapSubChannels[subChannel].AfterTuneEvent -= new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        _mapSubChannels[subChannel].AfterTuneEvent += new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        _mapSubChannels[subChannel].OnGraphStart();
      }
      RemoveStreamSourceFilter();
      AddStreamSourceFilter(url);
      Log.Log.Info("dvb:  RunGraph");
      hr = (_graphBuilder as IMediaControl).Run();
      if (hr < 0 || hr > 1)
      {
        Log.Log.WriteFile("dvb:  RunGraph returns: 0x{0:X}", hr);
        throw new TvException("Unable to start graph");
      }
      //GetTunerSignalStatistics();
      _epgGrabbing = false;
      if (_mapSubChannels.ContainsKey(subChannel) && (url != _defaultUrl))
      {
        _mapSubChannels[subChannel].AfterTuneEvent -= new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        _mapSubChannels[subChannel].AfterTuneEvent += new BaseSubChannel.OnAfterTuneDelegate(OnAfterTuneEvent);
        _mapSubChannels[subChannel].OnGraphStarted();
      }
    }
  }
}