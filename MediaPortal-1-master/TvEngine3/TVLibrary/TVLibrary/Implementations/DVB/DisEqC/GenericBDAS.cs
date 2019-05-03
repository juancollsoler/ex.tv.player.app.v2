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
using TvLibrary.Channels;

namespace TvLibrary.Implementations.DVB
{
  /// <summary>
  /// Generic BDA DiSEqC support.
  /// </summary>
  public class GenericBDAS : IDisposable
  {
    #region variables

    private readonly IKsPropertySet _propertySet;
    protected IBDA_Topology _TunerDevice;
    protected bool _isGenericBDAS;
    protected String _CardName = "GenericBDAS";

    #endregion

    #region constants

    private readonly Guid guidBdaDigitalDemodulator = new Guid(0xef30f379, 0x985b, 0x4d10, 0xb6, 0x40, 0xa7, 0x9d, 0x5e,
                                                               0x4, 0xe1, 0xe0);

    #endregion

    /// <summary>
    /// Initializes a new instance of the <see cref="GenericBDAS"/> class.
    /// </summary>
    /// <param name="tunerFilter">The tuner filter.</param>
    public GenericBDAS(IBaseFilter tunerFilter)
    {
      _TunerDevice = (IBDA_Topology)tunerFilter;
      //check if the BDA driver supports DiSEqC
      IPin pin = DsFindPin.ByName(tunerFilter, "MPEG2 Transport");
      if (pin != null)
      {
        _propertySet = pin as IKsPropertySet;
        if (_propertySet != null)
        {
          KSPropertySupport supported;
          _propertySet.QuerySupported(guidBdaDigitalDemodulator, (int)BdaDigitalModulator.MODULATION_TYPE, out supported);
          if ((supported & KSPropertySupport.Set) != 0)
          {
            Log.Log.Debug(FormatMessage("DiSEqC capable card found!"));
            _isGenericBDAS = true;
          }
        }
      }
      else
        Log.Log.Info(FormatMessage("tuner pin not found!"));
    }

    /// <summary>
    /// Formats the log message to contain card type name at start.
    /// </summary>
    /// <param name="LogMessage"></param>
    /// <returns></returns>
    protected String FormatMessage(String LogMessage)
    {
      return String.Format("{0}:{1}", _CardName, LogMessage);
    }

    /// <summary>
    /// Sends the diseq command.
    /// </summary>
    /// <param name="channel">The channel.</param>
    /// <param name="parameters">The channels scanning parameters.</param>
    public void SendDiseqCommand(ScanParameters parameters, DVBSChannel channel)
    {
      switch (channel.DisEqc)
      {
        case DisEqcType.Level1AA:
          Log.Log.Info(FormatMessage("  Level1AA - SendDiSEqCCommand(0x00)"));
          SendDiSEqCCommand(0x00);
          break;
        case DisEqcType.Level1AB:
          Log.Log.Info(FormatMessage("  Level1AB - SendDiSEqCCommand(0x01)"));
          SendDiSEqCCommand(0x01);
          break;
        case DisEqcType.Level1BA:
          Log.Log.Info(FormatMessage("  Level1BA - SendDiSEqCCommand(0x0100)"));
          SendDiSEqCCommand(0x0100);
          break;
        case DisEqcType.Level1BB:
          Log.Log.Info(FormatMessage("  Level1BB - SendDiSEqCCommand(0x0101)"));
          SendDiSEqCCommand(0x0101);
          break;
        case DisEqcType.SimpleA:
          Log.Log.Info(FormatMessage("  SimpleA - SendDiSEqCCommand(0x00)"));
          SendDiSEqCCommand(0x00);
          break;
        case DisEqcType.SimpleB:
          Log.Log.Info(FormatMessage("  SimpleB - SendDiSEqCCommand(0x01)"));
          SendDiSEqCCommand(0x01);
          break;
        default:
          return;
      }
    }

    /// <summary>
    /// Sends the DiSEqC command.
    /// </summary>
    /// <param name="ulRange">The DisEqCPort</param>
    /// <returns>true if succeeded, otherwise false</returns>
    protected bool SendDiSEqCCommand(ulong ulRange)
    {
      Log.Log.Info(FormatMessage("  SendDiSEqC Command {0}"), ulRange);
      // get ControlNode of tuner control node
      object ControlNode;
      int hr = _TunerDevice.GetControlNode(0, 1, 0, out ControlNode);
      if (hr == 0)
        // retrieve the BDA_DeviceControl interface 
      {
        IBDA_DeviceControl DecviceControl = (IBDA_DeviceControl)_TunerDevice;
        if (DecviceControl != null)
        {
          if (ControlNode != null)
          {
            IBDA_FrequencyFilter FrequencyFilter = ControlNode as IBDA_FrequencyFilter;
            hr = DecviceControl.StartChanges();
            if (hr == 0)
            {
              if (FrequencyFilter != null)
              {
                hr = FrequencyFilter.put_Range(ulRange);
                Log.Log.Info(FormatMessage("  put_Range:{0} success:{1}"), ulRange, hr);
                if (hr == 0)
                {
                  // did it accept the changes? 
                  hr = DecviceControl.CheckChanges();
                  if (hr == 0)
                  {
                    hr = DecviceControl.CommitChanges();
                    if (hr == 0)
                    {
                      Log.Log.Info(FormatMessage("  CommitChanges() Succeeded"));
                      return true;
                    }
                    // reset configuration
                    Log.Log.Info(FormatMessage("  CommitChanges() Failed!"));
                    DecviceControl.StartChanges();
                    DecviceControl.CommitChanges();
                    return false;
                  }
                  Log.Log.Info(FormatMessage("  CheckChanges() Failed!"));
                  return false;
                }
                Log.Log.Info(FormatMessage("  put_Range Failed!"));
                return false;
              }
            }
          }
        }
      }
      Log.Log.Info(FormatMessage("  GetControlNode Failed!"));
      return false;
    }

    /// <summary>
    /// gets the diseqc reply
    /// </summary>
    /// <param name="pulRange">The DisEqCPort Port.</param>
    /// <returns>true if succeeded, otherwise false</returns>
    protected bool ReadDiSEqCCommand(out ulong pulRange)
    {
      // get ControlNode of tuner control node
      object ControlNode;
      int hr = _TunerDevice.GetControlNode(0, 1, 0, out ControlNode);
      if (hr == 0)
        // retrieve the BDA_DeviceControl interface 
      {
        IBDA_DeviceControl DecviceControl = (IBDA_DeviceControl)_TunerDevice;
        if (DecviceControl != null)
        {
          if (ControlNode != null)
          {
            IBDA_FrequencyFilter FrequencyFilter = ControlNode as IBDA_FrequencyFilter;
            hr = DecviceControl.StartChanges();
            if (hr == 0)
            {
              if (FrequencyFilter != null)
              {
                hr = FrequencyFilter.get_Range(out pulRange);
                Log.Log.Info(FormatMessage("  get_Range:{0} success:{1}"), pulRange, hr);
                if (hr == 0)
                {
                  return true;
                }
                Log.Log.Info(FormatMessage("  get_Range Failed!"));
                return false;
              }
            }
          }
        }
      }
      Log.Log.Info(FormatMessage("  GetControlNode Failed!"));
      pulRange = 0;
      return false;
    }

    /// <summary>
    /// Determines whether [is cam present].
    /// </summary>
    /// <returns>
    /// 	<c>true</c> if [is cam present]; otherwise, <c>false</c>.
    /// </returns>
    public bool IsCamPresent()
    {
      return false;
    }

    /// <summary>
    /// Gets a value indicating whether this instance is a Generic BDA-S based card.
    /// </summary>
    /// <value>
    /// 	<c>true</c> if this instance is; otherwise, <c>false</c>.
    /// </value>
    public bool IsGenericBDAS
    {
      get { return _isGenericBDAS; }
    }

    #region IDisposable Member

    public virtual void Dispose() {}

    #endregion
  }
}