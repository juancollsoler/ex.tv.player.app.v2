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
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.InputDevices;
using MediaPortal.Player;
using MediaPortal.ProcessPlugins.MiniDisplayPlugin.xPL;
using Action = MediaPortal.GUI.Library.Action;

namespace MediaPortal.ProcessPlugins.MiniDisplayPlugin.Drivers
{
  public class xPL_Connector : BaseDisplay
  {
    private bool _BlankDisplayOnExit;
    private bool _DisplayChanged;
    private readonly string _ErrorMessage = "";
    private bool _IsConnected;
    private bool _IsDisabled = false;
    private readonly string[] _lines = new string[2];
    private bool _StatusChanged = true;
    private int _Tcols;
    private int _Trows;
    private bool DoDebug = Assembly.GetEntryAssembly().FullName.Contains("Configuration");
    private DateTime LastSettingsCheck = DateTime.Now;
    private XplListener Listener;
    private readonly string mDeviceID = "cdisplay";
    private string mInstanceID = "";
    private SystemStatus MPStatus = new SystemStatus();
    private SystemStatus MPStatus_old = new SystemStatus();
    private readonly string mVendorID = "mportal";
    private DateTime SettingsLastModTime;

    private void AdvancedSettings_OnSettingsChanged()
    {
      Log.Info("xPL_Connector.AdvancedSettings_OnSettingsChanged(): called");
      this.CleanUp();
      this.LoadAdvancedSettings();
      Thread.Sleep(100);
      this.Setup(Settings.Instance.Port, Settings.Instance.TextHeight, Settings.Instance.TextWidth,
                 Settings.Instance.TextComDelay, Settings.Instance.GraphicHeight, Settings.Instance.GraphicWidth,
                 Settings.Instance.GraphicComDelay, Settings.Instance.BackLightControl, Settings.Instance.Backlight,
                 Settings.Instance.ContrastControl, Settings.Instance.Contrast, Settings.Instance.BlankOnExit);
      this.Initialize();
    }

    public override void CleanUp()
    {
      AdvancedSettings.OnSettingsChanged -=
        new AdvancedSettings.OnSettingsChangedHandler(this.AdvancedSettings_OnSettingsChanged);
      this.Listener.XplMessageReceived -=
        new XplListener.XplMessageReceivedEventHandler(this.Listener_XplMessageReceived);
      this.Listener.Dispose();
    }

    private void Clear() {}

    public override void Configure()
    {
      Form form = new xPL_Connector_AdvancedSetupForm();
      form.ShowDialog();
      form.Dispose();
    }

    public override void Dispose() { }

    public override void DrawImage(Bitmap bitmap) { }

    public override void Initialize()
    {
      if (this._IsDisabled)
      {
        Log.Info("xPL_Connector.Initialize(): DRIVER DISABLED!");
      }
      else
      {
        this._lines[0] = string.Empty;
        this._lines[1] = string.Empty;
        Log.Info("xPL_Connector.Initialize(): called");
        this.Listener = new XplListener(this.mVendorID + "-" + this.mDeviceID, 1);
        Log.Info("xPL_Connector.Initialize(): waiting to join xPL network. (Awaiting config = {0})",
                 new object[] {this.Listener.AwaitingConfiguration});
        if (this.Listener.AwaitingConfiguration)
        {
          this.Listener.ConfigItems.Add("newconf", "", xplConfigTypes.xReconf);
          this.Listener.ConfigItems.Add("interval", "", xplConfigTypes.xOption);
        }
        this.Listener.XplMessageReceived +=
          new XplListener.XplMessageReceivedEventHandler(this.Listener_XplMessageReceived);
        this.Listener.XplJoinedxPLNetwork +=
          new XplListener.XplJoinedxPLNetworkEventHandler(this.Listener_XplJoinedxPLNetwork);
        this.Listener.Listen();
        int num = 0x7530;
        while (!this.Listener.JoinedxPLNetwork)
        {
          Log.Info("xPL_Connector.Initialize(): waiting to join the xPL network");
          Thread.Sleep(100);
          num -= 100;
          if (num < 0)
          {
            break;
          }
        }
        if (!this.Listener.JoinedxPLNetwork)
        {
          Log.Info("xPL_Connector.Initialize(): Could NOT join the xPL network");
        }
        else
        {
          this._IsConnected = true;
        }
        if (this._IsConnected)
        {
          AdvancedSettings.OnSettingsChanged +=
            new AdvancedSettings.OnSettingsChangedHandler(this.AdvancedSettings_OnSettingsChanged);
          this.Clear();
        }
        this.mInstanceID = this.Listener.InstanceName;
        Log.Info("xPL_Connector.Initialize(): completed");
      }
    }

    private void Listener_XplJoinedxPLNetwork()
    {
      this._IsConnected = true;
      this._DisplayChanged = false;
      this._StatusChanged = false;
      Log.Info("xPLConnector_Listener_XplJoinedxPLNetwork: Joined xPL Network");
      this.UpdateMessage(true);
    }

    private void Listener_XplMessageReceived(object sender, XplListener.XplEventArgs e)
    {
      if (e.XplMsg.Schema.msgClass.ToLowerInvariant().Equals("hbeat") && e.XplMsg.Schema.msgType.ToLowerInvariant().Equals("app"))
      {
        if (this.DoDebug)
        {
          Log.Info("xPLConnector_Listener_XplMessageReceived: Received HEARTBEAT");
        }
        this._IsConnected = true;
      }
      else if (e.XplMsg.Schema.msgClass.ToLowerInvariant().Equals("config"))
      {
        if (this.DoDebug)
        {
          Log.Info("xPLConnector_Listener_XplMessageReceived: Received CONFIG message");
        }
      }
      else if ((e.XplMsg.Source.Vendor.ToLowerInvariant().Equals(this.mVendorID.ToLowerInvariant()) &&
                e.XplMsg.Source.Device.ToLowerInvariant().Equals(this.mDeviceID.ToLowerInvariant())) &&
               e.XplMsg.Source.Instance.ToLowerInvariant().Equals(this.mInstanceID.ToLowerInvariant()))
      {
        if (this.DoDebug)
        {
          Log.Info("xPLConnector_Listener_XplMessageReceived: Received ECHO");
        }
      }
      else
      {
        if (this.DoDebug)
        {
          Log.Info("xPLConnector_Listener_XplMessageReceived: {0} - {1} - {2}",
                   new object[] {e.XplMsg.Source.Vendor, e.XplMsg.Source.Device, e.XplMsg.Content});
        }
        string str = e.XplMsg.Schema.msgClass.ToLowerInvariant() + "." + e.XplMsg.Schema.msgType.ToLowerInvariant();
        string str11 = str;
        if (str11 != null)
        {
          if (!(str11 == "media.basic"))
          {
            if (!(str11 == "media.request"))
            {
              if (str11 == "remote.basic")
              {
                foreach (string str9 in e.XplMsg.GetParam(1, "keys").Split(new char[] {','}))
                {
                  if (this.DoDebug)
                  {
                    Log.Info("xPL_Connector.Listener_XplMessageReceived(): Received remote.basic \"{0}\"",
                             new object[] {str9});
                  }
                  if (Enum.IsDefined(typeof (GUIWindow.Window), str9.ToUpperInvariant()))
                  {
                    if (this.DoDebug)
                    {
                      Log.Info("xPL_Connector.Listener_XplMessageReceived(): Received remote.basic window name",
                               new object[0]);
                    }
                    this.XPL_Send_Remote_Confirm_Message(e);
                    int num8 = (int)Enum.Parse(typeof (GUIWindow.Window), str9.ToUpperInvariant());
                    if (!GUIWindowManager.ActiveWindow.Equals(num8))
                    {
                      GUIWindowManager.SendThreadCallbackAndWait(
                        new GUIWindowManager.Callback(this.ThreadMessageCallback), 1, num8, null);
                      return;
                    }
                    break;
                  }
                  if (Enum.IsDefined(typeof (Keys), str9))
                  {
                    if (this.DoDebug)
                    {
                      Log.Info("xPL_Connector.Listener_XplMessageReceived(): Received remote.basic key name",
                               new object[0]);
                    }
                    this.XPL_Send_Remote_Confirm_Message(e);
                    Key key = new Key(0, (int)Enum.Parse(typeof (Keys), str9));
                    Action action3 = new Action();
                    if (ActionTranslator.GetAction(GUIWindowManager.ActiveWindow, key, ref action3))
                    {
                      GUIWindowManager.OnAction(action3);
                      return;
                    }
                  }

                  int result = 0;
                  int.TryParse(str9, out result);
                  if (result != 0)
                  {
                    if (this.DoDebug)
                    {
                      Log.Info("xPL_Connector.Listener_XplMessageReceived(): Received remote.basic raw keycode",
                               new object[0]);
                    }
                    this.XPL_Send_Remote_Confirm_Message(e);
                    Key key2 = new Key(0, result);
                    Action action4 = new Action();
                    if (ActionTranslator.GetAction(GUIWindowManager.ActiveWindow, key2, ref action4))
                    {
                      GUIWindowManager.OnAction(action4);
                      return;
                    }
                    break;
                  }
                }
              }
            }
            else
            {
              switch (e.XplMsg.GetParam(1, "request"))
              {
                case "devinfo":
                  this.XPL_SendDevInfo("xpl-stat");
                  return;

                case "devstate":
                  if (e.XplMsg.GetParam(1, "mp").ToLowerInvariant().Equals("player"))
                  {
                    this.XPL_SendPlayerDevstate("xpl-stat");
                  }
                  return;

                case "mpinfo":
                  if (e.XplMsg.GetParam(1, "mp").ToLowerInvariant().Equals("player"))
                  {
                    this.XPL_SendPlayerMediaPlayerInfo("xpl-stat");
                    this.XPL_SendPlayerMediaPlayerInputInfo("xpl-stat");
                  }
                  return;

                case "mptrnspt":
                  if (e.XplMsg.GetParam(1, "mp").ToLowerInvariant().Equals("player"))
                  {
                    this.XPL_SendPlayerTransportState("xpl-stat");
                  }
                  return;

                case "mpmedia":
                  if (e.XplMsg.GetParam(1, "mp").ToLowerInvariant().Equals("player"))
                  {
                    this.XPL_SendMediaInfo("xpl-stat");
                  }
                  return;

                case "mpconfig":
                  if (e.XplMsg.GetParam(1, "mp").ToLowerInvariant().Equals("player"))
                  {
                    this.XPL_SendPlayerMediaPlayerConfig("xpl-stat");
                  }
                  return;

                case "mpqueue":
                  return;
              }
            }
          }
          else
          {
            int num;
            switch (e.XplMsg.GetParam(1, "command").ToLowerInvariant())
            {
              case "record":
              case "position":
              case "chapter":
              case "power":
              case "reboot":
              case "input":
              case "options":
                return;

              case "play":
                {
                  string path = e.XplMsg.GetParam(1, "url").ToLowerInvariant();
                  if (!(path.Equals(string.Empty) & g_Player.Paused))
                  {
                    if (path.Equals(g_Player.currentFileName) & g_Player.Paused)
                    {
                      g_Player.Pause();
                      if (this.DoDebug)
                      {
                        Log.Info(
                          "xPLConnector_Listener_XplMessageReceived: Received media.basic play file command (unpause)",
                          new object[0]);
                        return;
                      }
                      return;
                    }
                    if (File.Exists(path) && !g_Player.currentFileName.Equals(path))
                    {
                      GUIMessage message = new GUIMessage();
                      message.Message = GUIMessage.MessageType.GUI_MSG_PLAY_FILE;
                      message.Label = path;
                      GUIWindowManager.SendThreadMessage(message);
                      if (!this.DoDebug)
                      {
                        return;
                      }
                      Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic play file command",
                               new object[0]);
                    }
                    return;
                  }
                  g_Player.Pause();
                  if (this.DoDebug)
                  {
                    Log.Info(
                      "xPLConnector_Listener_XplMessageReceived: Received media.basic play file command (unpause)",
                      new object[0]);
                  }
                  return;
                }
              case "stop":
                {
                  GUIMessage message2 = new GUIMessage();
                  message2.Message = GUIMessage.MessageType.GUI_MSG_STOP_FILE;
                  GUIWindowManager.SendThreadMessage(message2);
                  if (this.DoDebug)
                  {
                    Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic stop command",
                             new object[0]);
                  }
                  return;
                }
              case "pause":
                if (this.DoDebug)
                {
                  Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic pause command",
                           new object[0]);
                }
                if (!g_Player.Paused)
                {
                  g_Player.Pause();
                }
                return;

              case "forward":
                {
                  string str4 = e.XplMsg.GetParam(1, "speed").ToLowerInvariant();
                  num = 0;
                  if (!str4.Equals(string.Empty))
                  {
                    num = int.Parse(str4.Replace("x", ""));
                    break;
                  }
                  num = g_Player.Speed * 2;
                  break;
                }
              case "rewind":
                {
                  string str5 = e.XplMsg.GetParam(1, "speed").ToLowerInvariant();
                  int num2 = 0;
                  if (!str5.Equals(string.Empty))
                  {
                    num2 = int.Parse(str5.Replace("x", ""));
                  }
                  else
                  {
                    num2 = Math.Abs(g_Player.Speed) * 2;
                  }
                  if (num2 > 0x20)
                  {
                    num2 = 0x20;
                  }
                  if (this.DoDebug)
                  {
                    Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic rewind ({0}x) command",
                             new object[] {num2});
                  }
                  g_Player.Speed = -num2;
                  return;
                }
              case "next":
                Action action;
                if (!g_Player.IsDVD)
                {
                  action = new Action(Action.ActionType.ACTION_NEXT_ITEM, 0f, 0f);
                }
                else
                {
                  action = new Action(Action.ActionType.ACTION_NEXT_CHAPTER, 0f, 0f);
                }
                GUIGraphicsContext.OnAction(action);
                if (this.DoDebug)
                {
                  Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic next command",
                           new object[0]);
                }
                return;

              case "back":
                Action action2;
                if (!g_Player.IsDVD)
                {
                  action2 = new Action(Action.ActionType.ACTION_PREV_ITEM, 0f, 0f);
                }
                else
                {
                  action2 = new Action(Action.ActionType.ACTION_PREV_CHAPTER, 0f, 0f);
                }
                GUIGraphicsContext.OnAction(action2);
                if (this.DoDebug)
                {
                  Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic back command",
                           new object[0]);
                }
                return;

              case "mute":
                if (!(e.XplMsg.GetParam(1, "state").ToLowerInvariant() == "on"))
                {
                  if (this.DoDebug)
                  {
                    Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic mute off command",
                             new object[0]);
                  }
                  VolumeHandler.Instance.IsMuted = false;
                  return;
                }
                if (this.DoDebug)
                {
                  Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic mute on command",
                           new object[0]);
                }
                VolumeHandler.Instance.IsMuted = true;
                return;

              case "volume":
                {
                  string str13;
                  string s = e.XplMsg.GetParam(1, "level").ToLowerInvariant();
                  if (((str13 = s.Substring(0, 1)) == null) || (!(str13 == "+") && !(str13 == "-")))
                  {
                    int num7 = int.Parse(s);
                    VolumeHandler.Instance.Volume = (VolumeHandler.Instance.Maximum / 100) * num7;
                    if (this.DoDebug)
                    {
                      Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic volume command",
                               new object[0]);
                    }
                    return;
                  }
                  int volume = VolumeHandler.Instance.Volume;
                  int num5 = int.Parse(s) * 0x28f;
                  int num6 = volume + num5;
                  if (num6 < 0)
                  {
                    num6 = 0;
                  }
                  if (num6 > 0xffdc)
                  {
                    num6 = 0xffdc;
                  }
                  VolumeHandler.Instance.Volume = num6;
                  if (this.DoDebug)
                  {
                    Log.Info(
                      "xPLConnector_Listener_XplMessageReceived: Received media.basic volume {0} command = orig = {1}, new = {2}, delta = {3}",
                      new object[] {s, volume, num6, num5});
                  }
                  return;
                }
              default:
                return;
            }
            if (num > 0x20)
            {
              num = 0x20;
            }
            if (this.DoDebug)
            {
              Log.Info("xPLConnector_Listener_XplMessageReceived: Received media.basic play forward ({0}x) command",
                       new object[] {num});
            }
            g_Player.Speed = num;
          }
        }
      }
    }

    private void LoadAdvancedSettings()
    {
      Log.Info("xPL_Connector.LoadAdvancedSettings(): Called");
      AdvancedSettings.Load();
      Log.Info("xPL_Connector.LoadAdvancedSettings(): Extensive Logging: {0}",
               new object[] {Settings.Instance.ExtensiveLogging});
      Log.Info("xPL_Connector.LoadAdvancedSettings(): Device Port: {0}", new object[] {Settings.Instance.Port});
      FileInfo info = new FileInfo(Config.GetFile(Config.Dir.Config, "MiniDisplay_xPL_Connector.xml"));
      this.SettingsLastModTime = info.LastWriteTime;
      this.LastSettingsCheck = DateTime.Now;
    }

    public override void SetCustomCharacters(int[][] customCharacters) { }

    public override void SetLine(int line, string message, ContentAlignment aAlignment)
    {
      if (this._IsDisabled)
      {
        if (this.DoDebug)
        {
          Log.Info("xPL_Connector.SetLine(): Unable to display text - driver disabled");
        }
      }
      else
      {
        this.UpdateAdvancedSettings();
        if (this.DoDebug)
        {
          Log.Info("xPL_Connector.SetLine() Called");
        }
        if (!this._lines[line].Equals(message))
        {
          this._DisplayChanged = true;
          this._lines[line] = message;
        }
        if (line == (this._Trows - 1))
        {
          this.UpdateMPStatus();
          this.UpdateMessage();
        }
      }
    }

    public override void Setup(string _port, int _lines, int _cols, int _delay, int _linesG, int _colsG, int _delayG,
                      bool _backLight, int _backLightLevel, bool _contrast, int _contrastLevel, bool _blankOnExit)
    {
      this.DoDebug = Assembly.GetEntryAssembly().FullName.Contains("Configuration") | Settings.Instance.ExtensiveLogging;
      Log.Info("{0}", new object[] {this.Description});
      Log.Info("xPL_Connector.Setup(): called");
      this._BlankDisplayOnExit = _blankOnExit;
      this.LoadAdvancedSettings();
      this._Trows = _lines;
      this._Tcols = _cols;
      this._IsConnected = false;
      Log.Info("xPL_Connector.Setup(): completed");
    }

    private int ThreadMessageCallback(int param1, int param2, object data)
    {
      if (param1 == 1)
      {
        GUIWindowManager.ActivateWindow(param2);
        return 0;
      }
      return -1;
    }

    private void UpdateAdvancedSettings()
    {
      if (DateTime.Now.Ticks >= this.LastSettingsCheck.AddMinutes(1.0).Ticks)
      {
        if (this.DoDebug)
        {
          Log.Info("xPL_Connector.UpdateAdvancedSettings(): called");
        }
        if (File.Exists(Config.GetFile(Config.Dir.Config, "MiniDisplay_xPL_Connector.xml")))
        {
          FileInfo info = new FileInfo(Config.GetFile(Config.Dir.Config, "MiniDisplay_xPL_Connector.xml"));
          if (info.LastWriteTime.Ticks > this.SettingsLastModTime.Ticks)
          {
            if (this.DoDebug)
            {
              Log.Info("xPL_Connector.UpdateAdvancedSettings(): updating advanced settings");
            }
            this.LoadAdvancedSettings();
          }
        }
        if (this.DoDebug)
        {
          Log.Info("xPL_Connector.UpdateAdvancedSettings(): completed");
        }
      }
    }

    private void UpdateMessage()
    {
      this.UpdateMessage(false);
    }

    private void UpdateMessage(bool ForceUpdate)
    {
      string strMessage = string.Empty;
      if (!ForceUpdate)
      {
        if (!this._DisplayChanged && !this._StatusChanged)
        {
          return;
        }
      }
      else if (this.DoDebug)
      {
        Log.Info("xPLConnector.UpdateMessage(): FORCING Status update");
      }
      if (this._DisplayChanged || ForceUpdate)
      {
        object obj2 = strMessage + "command=clear" + '\n';
        strMessage = string.Concat(new object[] {obj2, "text=", this._lines[0], @"\n", this._lines[1], '\n'});
        this.Listener.SendMessage("xpl-cmnd", "*", "osd.basic", strMessage);
        this._DisplayChanged = false;
        strMessage = string.Empty;
        if (this.DoDebug)
        {
          Log.Info("xPLConnector.UpdateMessage(): display update sent ({0})",
                   new object[] {this._lines[0] + @" \ " + this._lines[1]});
        }
      }
      if (this._StatusChanged || ForceUpdate)
      {
        this.XPL_SendPlayerDevstate("xpl-trig");
        this.XPL_SendPlayerTransportState("xpl-trig");
        if (this.MPStatus.MediaPlayer_Playing || this.MPStatus.MediaPlayer_Paused)
        {
          this.XPL_SendMediaInfo("xpl-trig");
        }
        if (this.DoDebug)
        {
          Log.Info("xPLConnector.UpdateMessage(): Status update sent");
        }
      }
    }

    private void UpdateMPStatus()
    {
      this.MPStatus_old = this.MPStatus;
      MiniDisplayHelper.GetSystemStatus(ref this.MPStatus);
      if (this.MPStatus.Equals(this.MPStatus_old))
      {
        this._StatusChanged = false;
      }
      else
      {
        this._StatusChanged = true;
      }
    }

    private void XPL_Send_Remote_Confirm_Message(XplListener.XplEventArgs e)
    {
      if (this.DoDebug)
      {
        Log.Info("XPL_Send_Remote_Confirm_Message(): PREPARING REMOTE.CONFIRM MESSAGE!");
      }
      string strMessage = string.Empty;
      string strTarget = string.Empty;
      string param = string.Empty;
      string str4 = string.Empty;
      string str5 = string.Empty;
      try
      {
        strTarget = e.XplMsg.GetParam(0, "source");
        param = e.XplMsg.GetParam(1, "keys");
        str4 = e.XplMsg.GetParam(1, "device");
        str5 = e.XplMsg.GetParam(1, "zone");
        object obj2 = strMessage;
        strMessage = string.Concat(new object[] {obj2, "keys=", param, '\n'});
        if (!str4.Equals(string.Empty))
        {
          object obj3 = strMessage;
          strMessage = string.Concat(new object[] {obj3, "device=", str4, '\n'});
        }
        if (!str5.Equals(string.Empty))
        {
          object obj4 = strMessage;
          strMessage = string.Concat(new object[] {obj4, "zone=", str5, '\n'});
        }
        if (this.DoDebug)
        {
          Log.Info("XPL_Send_Remote_Confirm_Message(): SENDING REMOTE.CONFIRM MESSAGE: {0}", new object[] {strMessage});
        }
        this.Listener.SendMessage("xpl-trig", strTarget, "remote.confirm", strMessage);
        if (this.DoDebug)
        {
          Log.Info("XPL_Send_Remote_Confirm_Message(): SENT REMOTE.CONFIRM MESSAGE!");
        }
      }
      catch
      {
        Log.Info("XPL_Send_Remote_Confirm_Message(): CAUGHT EXCEPTION: {0}, {1}, {2}, {3}!",
                 new object[] {strTarget, param, str5, str4});
      }
    }

    private void XPL_SendDevInfo(string msgType)
    {
      object obj2 = string.Empty + "name=mediaportal" + '\n';
      string strMessage = ((string.Concat(new object[] {obj2, "version=", this.Description, '\n'}) + "author=CybrMage" +
                            '\n') + "info-url=www.team-mediaportal.com" + '\n') + "mplist=player,recorder" + '\n';
      this.Listener.SendMessage(msgType, "*", "media.devinfo", strMessage);
      strMessage = string.Empty;
    }

    private void XPL_SendMediaInfo(string msgType)
    {
      object obj2 = string.Empty;
      string strMessage = string.Concat(new object[] {obj2, "duration=", this.MPStatus.Media_Duration.ToString(), '\n'});
      string str2 =
        MiniDisplayHelper.PluginIconsToAudioFormat(this.MPStatus.CurrentIconMask).Replace("ICON_", "").Trim().Replace(
          " ", ", ");
      string str3 =
        MiniDisplayHelper.PluginIconsToVideoFormat(this.MPStatus.CurrentIconMask).Replace("ICON_", "").Trim().Replace(
          " ", ", ");
      if (!str2.Equals(string.Empty) && !str3.Equals(string.Empty))
      {
        object obj3 = strMessage;
        strMessage = string.Concat(new object[] {obj3, "format=", str2, ", ", str3, '\n'});
      }
      else
      {
        object obj4 = strMessage;
        strMessage = string.Concat(new object[] {obj4, "format=", !str2.Equals(string.Empty) ? str2 : str3, '\n'});
      }
      this.Listener.SendMessage(msgType, "*", "media.mpmedia", strMessage);
      strMessage = string.Empty;
    }

    private void XPL_SendPlayerDevstate(string msgType)
    {
      object obj2 = (string.Empty + "power=on" + '\n') + "connected=true" + '\n';
      object obj3 =
        string.Concat(new object[]
                        {obj2, "volume=", Math.Floor((double)((this.MPStatus.SystemVolumeLevel / 0xffff) * 100)), '\n'});
      string strMessage = string.Concat(new object[] { obj3, "mute=", this.MPStatus.IsMuted.ToString().ToLower(CultureInfo.CurrentCulture), '\n' });
      this.Listener.SendMessage(msgType, "*", "media.devstate", strMessage);
      strMessage = string.Empty;
    }

    private void XPL_SendPlayerMediaPlayerConfig(string msgType)
    {
      object obj2 = (((string.Empty + "mp=player" + '\n') + "input=player" + '\n') + "power=on" + '\n') +
                    "connected=true" + '\n';
      object obj3 =
        string.Concat(new object[]
                        {obj2, "volume=", Math.Floor((double)((this.MPStatus.SystemVolumeLevel / 0xffff) * 100)), '\n'});
      string strMessage = string.Concat(new object[] { obj3, "mute=", this.MPStatus.IsMuted.ToString().ToLower(CultureInfo.CurrentCulture), '\n' });
      this.Listener.SendMessage(msgType, "*", "media.mpconfig", strMessage);
      strMessage = string.Empty;
    }

    private void XPL_SendPlayerMediaPlayerInfo(string msgType)
    {
      string strMessage = ((((((((((((string.Empty + "mp=player" + '\n') + "name=mediaportal player" + '\n') +
                                    "command-list=play,stop,pause,forward,rewind" + '\n') + "format-list=*" + '\n') +
                                  "input-list=*" + '\n') + "filter-list=*" + '\n') +
                                "forward-speeds=1x,2x,4x,8x,16x,32x" + '\n') + "rewind-speeds=1x,2x,4x,8x,16x,32x" +
                               '\n') + "audio=true" + '\n') + "video=true" + '\n') + "playlist=false" + '\n') +
                           "random=false" + '\n') + "repeat=false" + '\n';
      this.Listener.SendMessage(msgType, "*", "media.mpinfo", strMessage);
      strMessage = string.Empty;
    }

    private void XPL_SendPlayerMediaPlayerInputInfo(string msgType)
    {
      string strMessage = (string.Empty + "mp=player" + '\n') + "input=player" + '\n';
      this.Listener.SendMessage(msgType, "*", "media.mpinput", strMessage);
      strMessage = string.Empty;
    }

    private void XPL_SendPlayerTransportState(string msgType)
    {
      string strMessage = string.Empty + "mp=player" + '\n';
      if (!this.MPStatus.MediaPlayer_Active)
      {
        strMessage = strMessage + "command=stop" + '\n';
      }
      else
      {
        if (this.MPStatus.MediaPlayer_Playing)
        {
          if (this.MPStatus.Media_Speed == 1)
          {
            strMessage = strMessage + "command=play" + '\n';
          }
          else if (this.MPStatus.Media_Speed < 0)
          {
            strMessage = strMessage + "command=rewind" + '\n';
          }
          else
          {
            strMessage = strMessage + "command=forward" + '\n';
          }
          object obj2 = strMessage;
          strMessage =
            string.Concat(new object[] {obj2, "position=", this.MPStatus.Media_CurrentPosition.ToString(), '\n'});
        }
        else if (this.MPStatus.MediaPlayer_Paused)
        {
          object obj3 = strMessage + "command=paused" + '\n';
          strMessage =
            string.Concat(new object[] {obj3, "position=", this.MPStatus.Media_CurrentPosition.ToString(), '\n'});
        }
        else
        {
          strMessage = strMessage + "command=stop" + '\n';
        }
        this.Listener.SendMessage(msgType, "*", "media.mptrnspt", strMessage);
        strMessage = string.Empty;
      }
    }

    private void XPL_SendRecorderDevstate(string msgType)
    {
      string strMessage = (string.Empty + "power=on" + '\n') + "connected=true" + '\n';
      this.Listener.SendMessage(msgType, "*", "media.devstate", strMessage);
      strMessage = string.Empty;
    }

    public override string Description
    {
      get { return "xPL_Connector driver v05_05_2008"; }
    }

    public override string ErrorMessage
    {
      get { return this._ErrorMessage; }
    }

    public override bool IsDisabled
    {
      get { return this._IsDisabled; }
    }

    public override string Name
    {
      get { return "xPL_Connector"; }
    }

    public override bool SupportsGraphics
    {
      get { return false; }
    }

    public override bool SupportsText
    {
      get { return true; }
    }

    [Serializable]
    public class AdvancedSettings
    {
      private bool m_BlankDisplayWhenIdle;
      private bool m_BlankDisplayWithVideo;
      private int m_BlankIdleTime = 30;
      private bool m_EnableDisplayAction;
      private int m_EnableDisplayActionTime = 5;
      private static AdvancedSettings m_Instance;

      public static event OnSettingsChangedHandler OnSettingsChanged;

      private static void Default(AdvancedSettings _settings)
      {
        Log.Info("xPL_Connector.AdvancedSettings.Default(): called");
        _settings.BlankDisplayWithVideo = false;
        _settings.EnableDisplayAction = false;
        _settings.EnableDisplayActionTime = 5;
        _settings.BlankDisplayWhenIdle = false;
        _settings.BlankIdleTime = 30;
        Log.Info("xPL_Connector.AdvancedSettings.Default(): completed");
      }

      public static AdvancedSettings Load()
      {
        AdvancedSettings settings;
        Log.Info("xPL_Connector.AdvancedSettings.Load(): started");
        if (File.Exists(Config.GetFile(Config.Dir.Config, "MiniDisplay_xPL_Connector.xml")))
        {
          Log.Info("xPL_Connector.AdvancedSettings.Load(): Loading settings from XML file");
          XmlSerializer serializer = new XmlSerializer(typeof (AdvancedSettings));
          XmlTextReader xmlReader = new XmlTextReader(Config.GetFile(Config.Dir.Config, "MiniDisplay_xPL_Connector.xml"));
          settings = (AdvancedSettings)serializer.Deserialize(xmlReader);
          xmlReader.Close();
        }
        else
        {
          Log.Info("xPL_Connector.AdvancedSettings.Load(): Loading settings from defaults");
          settings = new AdvancedSettings();
          Default(settings);
        }
        Log.Info("xPL_Connector.AdvancedSettings.Load(): completed");
        return settings;
      }

      public static void NotifyDriver()
      {
        if (OnSettingsChanged != null)
        {
          OnSettingsChanged();
        }
      }

      public static void Save()
      {
        Save(Instance);
      }

      public static void Save(AdvancedSettings ToSave)
      {
        Log.Info("xPL_Connector.AdvancedSettings.Save(): Saving settings to XML file");
        XmlSerializer serializer = new XmlSerializer(typeof (AdvancedSettings));
        XmlTextWriter writer = new XmlTextWriter(Config.GetFile(Config.Dir.Config, "MiniDisplay_xPL_Connector.xml"),
                                                 Encoding.UTF8);
        writer.Formatting = Formatting.Indented;
        writer.Indentation = 2;
        serializer.Serialize((XmlWriter)writer, ToSave);
        writer.Close();
        Log.Info("xPL_Connector.AdvancedSettings.Save(): completed");
      }

      public static void SetDefaults()
      {
        Default(Instance);
      }

      [XmlAttribute]
      public bool BlankDisplayWhenIdle
      {
        get { return this.m_BlankDisplayWhenIdle; }
        set { this.m_BlankDisplayWhenIdle = value; }
      }

      [XmlAttribute]
      public bool BlankDisplayWithVideo
      {
        get { return this.m_BlankDisplayWithVideo; }
        set { this.m_BlankDisplayWithVideo = value; }
      }

      [XmlAttribute]
      public int BlankIdleTime
      {
        get { return this.m_BlankIdleTime; }
        set { this.m_BlankIdleTime = value; }
      }

      [XmlAttribute]
      public bool EnableDisplayAction
      {
        get { return this.m_EnableDisplayAction; }
        set { this.m_EnableDisplayAction = value; }
      }

      [XmlAttribute]
      public int EnableDisplayActionTime
      {
        get { return this.m_EnableDisplayActionTime; }
        set { this.m_EnableDisplayActionTime = value; }
      }

      public static AdvancedSettings Instance
      {
        get
        {
          if (m_Instance == null)
          {
            m_Instance = Load();
          }
          return m_Instance;
        }
        set { m_Instance = value; }
      }

      public delegate void OnSettingsChangedHandler();
    }
  }
}