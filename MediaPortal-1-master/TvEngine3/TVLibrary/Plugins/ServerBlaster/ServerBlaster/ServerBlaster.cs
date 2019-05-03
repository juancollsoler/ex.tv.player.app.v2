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
using System.IO;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;
using TvLibrary.Log;
using TvControl;
using TvEngine.Events;
using TvLibrary.Interfaces;
using TvLibrary.Implementations;
using TVEngine.Devices;
using TvDatabase;

// Modified to include Hauppauge Blasting
// Uses code from prior MP versions for HCWIRBlaster
// ralphy

namespace TvEngine
{
  public class ServerBlaster : AnalogChannel, ITvServerPlugin
  {
    #region properties

    private const string _Author = "joboehl with ralphy mods";
    private const string _PluginName = "ServerBlaster";

    private const string _version = "1.1.1.0";

    private HCWIRBlaster irblaster;

    /// <summary>
    /// returns the name of the plugin
    /// </summary>
    public new string Name
    {
      get { return _PluginName; }
    }

    /// <summary>
    /// returns the version of the plugin
    /// </summary>
    public string Version
    {
      get { return _version; }
    }

    /// <summary>
    /// returns the author of the plugin
    /// </summary>
    public string Author
    {
      get { return _Author; }
    }

    /// <summary>
    /// returns if the plugin should only run on the master server
    /// or also on slave servers
    /// </summary>
    public bool MasterOnly
    {
      get { return false; }
    }

    #endregion

    #region public methods

    public void Start(IController controller)
    {
      Log.WriteFile("ServerBlaster.Start Version {0}: Starting", _version);
      ITvServerEvent events = GlobalServiceProvider.Instance.Get<ITvServerEvent>();
      events.OnTvServerEvent += events_OnTvServerEvent;

      Blaster.DeviceArrival += OnDeviceArrival;
      Blaster.DeviceRemoval += OnDeviceRemoval;
      LoadRemoteCodes();

      Log.WriteFile("plugin: ServerBlaster start sender");
      Thread thread = new Thread(Sender);
      thread.SetApartmentState(ApartmentState.STA);
      thread.IsBackground = true;
      thread.Name = "Remote blaster";
      _running = true;
      thread.Start();


      Log.WriteFile("plugin: ServerBlaster.Start started");
    }

    public void Stop()
    {
      Log.WriteFile("plugin: ServerBlaster.Stop stopping");
      _running = false;

      if (GlobalServiceProvider.Instance.IsRegistered<ITvServerEvent>())
      {
        GlobalServiceProvider.Instance.Get<ITvServerEvent>().OnTvServerEvent -= events_OnTvServerEvent;
      }
      Log.WriteFile("plugin: ServerBlaster.Stop stopped");
    }

    public SetupTv.SectionSettings Setup
    {
      get { return new SetupTv.Sections.BlasterSetup(); }
    }

    #endregion public implementation

    #region Implementation

    private void events_OnTvServerEvent(object sender, EventArgs eventArgs)
    {
      TvServerEventArgs tvEvent = (TvServerEventArgs)eventArgs;
      AnalogChannel analogChannel = tvEvent.channel as AnalogChannel;
      if (analogChannel == null) return;
      if (tvEvent.EventType == TvServerEventType.StartZapChannel)
      {
        Log.WriteFile("ServerBlaster - CardId: {0}, Channel: {1} - Channel:{2} - VideoSource: {3}",
                      tvEvent.Card.Id, analogChannel.ChannelNumber, analogChannel.Name,
                      analogChannel.VideoSource.ToString());
        _send = true;
        _channel = analogChannel.ChannelNumber;
        _card = tvEvent.Card.Id;
        _videoInputType = analogChannel.VideoSource; // ralphy
        Log.WriteFile("ServerBlaster - Done");
      }
    }

    private static void OnDeviceArrival()
    {
      Log.WriteFile("ServerBlaster.OnDeviceArrival: Device installed");
    }

    private static void OnDeviceRemoval()
    {
      Log.WriteFile("ServerBlaster.OnDeviceRemoval: Device removed");
    }

    private void LoadRemoteCodes()
    {
      try
      {
        using (
          FileStream fs = new FileStream(String.Format(@"{0}\ServerBlaster.dat", PathManager.GetDataPath), FileMode.Open,
                                         FileAccess.Read))
        {
          BinaryFormatter bf = new BinaryFormatter();
          _packetCollection = bf.Deserialize(fs) as Hashtable;

          if (_packetCollection != null)
          {
            foreach (string buttonName in _packetCollection.Keys)
            {
              Log.WriteFile("ServerBlaster.LoadRemoteCodes: Packet '{0}' ({1} bytes)", buttonName,
                            ((byte[])_packetCollection[buttonName]).Length);
            }
          }
        }
      }
      catch (Exception e)
      {
        Log.WriteFile("ServerBlaster.LoadRemoteCodes: {0}", e.Message);
      }

      try
      {
        TvBusinessLayer layer = new TvBusinessLayer();
        _sendSelect = (layer.GetSetting("SrvBlasterSendSelect", "False").Value == "True");
        _sleepTime = 100; //xmlreader.GetValueAsInt("ServerBlaster", "delay", 100);
        _sendPort = 1; //xmlreader.GetValueAsInt("ServerBlaster", "forceport", 1);
        _blaster1Card = Convert.ToInt16(layer.GetSetting("SrvBlaster1Card", "0").Value);
        _blaster2Card = Convert.ToInt16(layer.GetSetting("SrvBlaster2Card", "0").Value);
        _deviceType = Convert.ToInt16(layer.GetSetting("SrvBlasterType", "0").Value);
        _deviceSpeed = Convert.ToInt16(layer.GetSetting("SrvBlasterSpeed", "0").Value);
        _advandeLogging = (layer.GetSetting("SrvBlasterLog", "False").Value == "True");
        _sendPort = Math.Max(1, Math.Min(2, _sendPort));

        Log.WriteFile("ServerBlaster.LoadRemoteCodes: Default port {0}", _sendPort);
        Log.WriteFile("ServerBlaster.RemoteType {0}", _deviceType);
        Log.WriteFile("ServerBlaster.DeviceSpeed {0}", _deviceSpeed);
        Log.WriteFile("ServerBlaster.Blaster1Card {0}", _blaster1Card);
        Log.WriteFile("ServerBlaster.Blaster2Card {0}", _blaster2Card);
        Log.WriteFile("ServerBlaster.Type {0}", _deviceType);
        Log.WriteFile("ServerBlaster.AdvancedLogging {0}", _advandeLogging);
        Log.WriteFile("ServerBlaster.SendSelect {0}", _sendSelect);
      }
      catch (Exception e)
      {
        Log.WriteFile("ServerBlaster.LoadRemoteCodes: {0}", e.Message);
      }

      return;
    }

    private void Sender()
    {
      irblaster = new HCWIRBlaster();
      while (_running)
      {
        if (_sending || !_send)
        {
          Thread.Sleep(50);
          continue;
        }
        _sending = true;
        Log.WriteFile("Blaster Sending: Channel:{0}, Card:{1}, VideoInput:{2}", _channel, _card,
                      _videoInputType.ToString());
        switch (_deviceType)
        {
          case 0:
            Send(_channel, _card);
            break;
          case 1:
            Send(_channel, _card);
            break;
          case 2:
            Log.WriteFile("ServerBlaster.Send: Case 2");
            if (_videoInputType.ToString() == "Tuner")
            {
              Log.WriteFile("ServerBlaster.Send: Channel {0} not blasted}", _channel);
            }
            else
            {
              Log.WriteFile("ServerBlaster.Send: Channel {0} blasted}", _channel);
              Send(_channel); // Hauppauge blasting
            }
            break;
          default:
            Log.WriteFile("ServerBlaster: Invalid _deviceType {0}", _deviceType);
            break;
        }
        _sending = false;
        _send = false;
        Log.WriteFile("ServerBlaster:Send Finished");
      }
      irblaster = null;
    }

    /// <summary>
    /// Overload for HCWIRBlasting
    /// </summary>
    private void Send(int externalChannel)
    {
      irblaster.blast(externalChannel.ToString(), _advandeLogging);
    }

    /// <summary>
    /// Overload for MS or SMK Blasting 
    /// </summary>
    private void Send(int externalChannel, int card)
    {
      if (_blaster1Card == card) _sendPort = 1;
      else if (_blaster2Card == card) _sendPort = 2;
      else if (_blaster1Card == _blaster2Card) _sendPort = 0;

      if (_advandeLogging)
        Log.WriteFile("ServerBlaster.Send: C {0} - B1{1} - B2{2}. Channel is {3}", card, _blaster1Card, _blaster2Card,
                      externalChannel);

      try
      {
        foreach (char ch in externalChannel.ToString())
        {
          if (char.IsDigit(ch) == false) continue;
          if (_advandeLogging) Log.WriteFile("ServerBlaster.Sending {0} on blaster {1}", ch, _sendPort);

          byte[] packet = _packetCollection[ch.ToString()] as byte[];

          if (packet == null) Log.WriteFile("ServerBlaster.Send: Missing packet for '{0}'", ch);
          if (packet != null) Blaster.Send(_sendPort, packet, _deviceType, _deviceSpeed, _advandeLogging);
          if (packet != null && _sleepTime != 0) Thread.Sleep(_sleepTime);
          if (_advandeLogging) Log.WriteFile("ServerBlaster.Send logic is done");
        }

        if (_sendSelect)
        {
          //byte[] packet = _packetCollection["Select"] as byte[];
          if (_advandeLogging) Log.Write("ServerBlaster.Send: Sending Select");
          byte[] packet = _packetCollection["Select"] as byte[];
          if (packet != null) Blaster.Send(_sendPort, packet, _deviceType, _deviceSpeed, _advandeLogging);
        }
      }
      catch (Exception e)
      {
        Log.WriteFile("ServerBlaster.Send: {0}", e.Message);
      }
    }

    #endregion Implementation

    #region Members

    private Hashtable _packetCollection;
    private int _sendPort = 1;
    private int _sleepTime = 100;
    private bool _sendSelect;
    private int _blaster1Card = -1;
    private int _blaster2Card = 1;
    private bool _advandeLogging;
    private int _deviceType = 1;
    private int _deviceSpeed;
    private int _channel;
    private int _card;
    private bool _send;
    private bool _sending;
    private bool _running;
    private VideoInputType _videoInputType;

    #endregion Members
  }
}