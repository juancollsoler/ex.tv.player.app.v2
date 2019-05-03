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
using DirectShowLib.BDA;
using TvLibrary.Interfaces;

namespace TvLibrary.Channels
{
  /// <summary>
  /// class holding all tuning details for ATSC
  /// </summary>
  [Serializable]
  public class ATSCChannel : DVBBaseChannel
  {
    #region variables

    private int _physicalChannel;
    private int _majorChannel;
    private int _minorChannel;
    private ModulationType _modulation = ModulationType.ModNotSet;

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="ATSCChannel"/> class.
    /// </summary>
    /// <param name="chan">The chan.</param>
    public ATSCChannel(ATSCChannel chan)
      : base(chan)
    {
      _majorChannel = chan.MajorChannel;
      _minorChannel = chan.MinorChannel;
      _physicalChannel = chan.PhysicalChannel;
      _modulation = chan.ModulationType;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="ATSCChannel"/> class.
    /// </summary>
    public ATSCChannel()
    {
      _majorChannel = -1;
      _minorChannel = -1;
      _physicalChannel = -1;
      _modulation = ModulationType.Mod8Vsb;
    }

    #region properties

    /// <summary>
    /// gets/sets the modulation type
    /// </summary>
    public ModulationType ModulationType
    {
      get { return _modulation; }
      set { _modulation = value; }
    }

    /// <summary>
    /// gets/sets the physical channel
    /// </summary>
    public int PhysicalChannel
    {
      get { return _physicalChannel; }
      set { _physicalChannel = value; }
    }

    /// <summary>
    /// gets/sets the major channel
    /// </summary>
    public int MajorChannel
    {
      get { return _majorChannel; }
      set { _majorChannel = value; }
    }

    /// <summary>
    /// gets/sets the minor channel
    /// </summary>
    public int MinorChannel
    {
      get { return _minorChannel; }
      set { _minorChannel = value; }
    }

    #endregion

    /// <summary>
    /// Toes the string.
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
      return String.Format("ATSC:{0} phys:{1} maj:{2} min:{3} mod:{4}", base.ToString(), _physicalChannel, _majorChannel,
                           _minorChannel, _modulation);
    }

    /// <summary>
    /// Equalses the specified obj.
    /// </summary>
    /// <param name="obj">The obj.</param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
      if ((obj as ATSCChannel) == null)
      {
        return false;
      }
      if (!base.Equals(obj))
      {
        return false;
      }
      ATSCChannel ch = obj as ATSCChannel;
      if (ch.MajorChannel != MajorChannel)
      {
        return false;
      }
      if (ch.MinorChannel != MinorChannel)
      {
        return false;
      }
      if (ch.ModulationType != ModulationType)
      {
        return false;
      }
      if (ch.PhysicalChannel != PhysicalChannel)
      {
        return false;
      }
      return true;
    }

    /// <summary>
    /// Serves as a hash function for a particular type. <see cref="M:System.Object.GetHashCode"></see> is suitable for use in hashing algorithms and data structures like a hash table.
    /// </summary>
    /// <returns>
    /// A hash code for the current <see cref="T:System.Object"></see>.
    /// </returns>
    public override int GetHashCode()
    {
      return base.GetHashCode() ^ _physicalChannel.GetHashCode() ^ _majorChannel.GetHashCode() ^
             _minorChannel.GetHashCode() ^ _modulation.GetHashCode();
    }

    /// <summary>
    /// Checks if the given channel and this instance are on the different transponder
    /// </summary>
    /// <param name="channel">Channel to check</param>
    /// <returns>true, if the channels are on the same transponder</returns>
    public override bool IsDifferentTransponder(IChannel channel)
    {
      ATSCChannel atscChannel = channel as ATSCChannel;
      if (atscChannel == null)
      {
        return true;
      }

      if (_modulation != atscChannel.ModulationType)
      {
        return true;
      }

      // For terrestrial digital (ATSC) and digital cable (SCTE) channels,
      // physical channel is an arbitrary number that is mapped by the network
      // provider to the frequency that must be tuned by the hardware. It
      // relates to frequency/channel/band plans defined by standards bodies
      // like the FCC, SCTE and ATSC.
      return _physicalChannel == 0 || atscChannel.PhysicalChannel != _physicalChannel;
    }

    /// <summary>
    /// Calculate the physical channel number corresponding with a frequency
    /// in the QAM standard frequency plan.
    /// </summary>
    /// <param name="carrierFrequency">The frequency, in kHz.</param>
    /// <returns>the physical channel number corresponding with <paramref name="carrierFrequency"/></returns>
    public static int GetPhysicalChannelFromFrequency(int carrierFrequency)
    {
      if (carrierFrequency >= 651000)
      {
        return 100 + ((carrierFrequency - 651000) / 6000);
      }
      if (carrierFrequency >= 219000)
      {
        return 23 + ((carrierFrequency - 219000) / 6000);
      }
      if (carrierFrequency >= 177000)
      {
        return 7 + ((carrierFrequency - 177000) / 6000);
      }
      if (carrierFrequency >= 123000)
      {
        return 14 + ((carrierFrequency - 123000) / 6000);
      }
      if (carrierFrequency >= 93000)
      {
        return 95 + ((carrierFrequency - 93000) / 6000);
      }
      if (carrierFrequency >= 79000)
      {
        return 5 + ((carrierFrequency - 79000) / 6000);
      }
      if (carrierFrequency >= 57000)
      {
        return 2 + ((carrierFrequency - 57000) / 6000);
      }
      return 0;
    }
  }
}