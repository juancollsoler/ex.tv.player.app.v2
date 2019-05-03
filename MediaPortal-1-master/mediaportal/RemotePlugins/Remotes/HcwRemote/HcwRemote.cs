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
using System.Diagnostics;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Profile;
using UdpHelper;

namespace MediaPortal.InputDevices
{
  /// <summary>
  /// Hauppauge HCW remote control class
  /// all remotes are supported, if the buttons are defined in the XML file
  /// </summary>
  public class HcwRemote : IInputDevice
  {
    private bool _controlEnabled = false; // HCW remote enabled
    private bool _allowExternal = false; // External processes are controlled by the Hauppauge app
    private bool _keepControl = false; // Keep control, if MP loses focus
    private bool _logVerbose = false; // Verbose logging
    private DateTime _lastTime = DateTime.Now; // Timestamp of last recieved keycode from remote

    private int _sameCommandCount = 0;
    // Counts how many times a button has been pressed (used to get progressive repetition delay, first time, long delay, then short delay)

    private int _lastExecutedCommandCount = 0;
    private int _lastCommand = 0; // Last executed command
    private InputHandler _inputHandler;
    private Connection _connection = null;
    private bool _exit = false;
    private int _port = 2110;

    private TimeSpan _buttonRelease;
    private int _repeatFilter = 0;
    private int _repeatSpeed = 0;
    private bool _filterDoubleKlicks = false;

    private const int WM_ACTIVATE = 0x0006;
    private const int WM_POWERBROADCAST = 0x0218;
    private const int WA_INACTIVE = 0;
    private const int WA_ACTIVE = 1;
    private const int WA_CLICKACTIVE = 2;

    private const int PBT_APMRESUMEAUTOMATIC = 0x0012;
    private const int PBT_APMRESUMECRITICAL = 0x0006;

    #region Callback

    //Sets up callback so that other forms can catch a key press
    public delegate void HCWEvent(int keypress);

    public event HCWEvent HCWKeyPressed;

    #endregion

    /// <summary>
    /// HCW control enabled
    /// </summary>
    /// <returns>Returns true/false.</returns>
    public bool Enabled
    {
      get { return _controlEnabled; }
    }


    /// <summary>
    /// Constructor
    /// </summary>
    public HcwRemote() {}


    public void Init(IntPtr hwnd)
    {
      Init();
    }


    /// <summary>
    /// Stop IR.exe and initiate HCW start
    /// </summary>
    public void Init()
    {
      _exit = false;
      using (Settings xmlreader = new MPSettings())
      {
        _controlEnabled = xmlreader.GetValueAsBool("remote", "HCW", false);
        _allowExternal = xmlreader.GetValueAsBool("remote", "HCWAllowExternal", false);
        _keepControl = xmlreader.GetValueAsBool("remote", "HCWKeepControl", false);
        _logVerbose = xmlreader.GetValueAsBool("remote", "HCWVerboseLog", false);
        _buttonRelease = TimeSpan.FromMilliseconds(xmlreader.GetValueAsInt("remote", "HCWButtonRelease", 200));
        _repeatFilter = xmlreader.GetValueAsInt("remote", "HCWRepeatFilter", 2);
        _repeatSpeed = xmlreader.GetValueAsInt("remote", "HCWRepeatSpeed", 0);
        _filterDoubleKlicks = xmlreader.GetValueAsBool("remote", "HCWFilterDoubleKlicks", false);
        _port = xmlreader.GetValueAsInt("remote", "HCWHelperPort", 2110);
      }
      if (_controlEnabled)
      {
        string exePath = irremote.GetHCWPath();
        string dllPath = irremote.GetDllPath();

        bool hcwDriverUpToDate = true;

        if (File.Exists(exePath + "Ir.exe"))
        {
          FileVersionInfo exeVersionInfo = FileVersionInfo.GetVersionInfo(exePath + "Ir.exe");
          if (exeVersionInfo.FileVersion.CompareTo(irremote.CurrentVersion) < 0)
          {
            hcwDriverUpToDate = false;
          }
        }

        if (File.Exists(dllPath + "irremote.DLL"))
        {
          FileVersionInfo dllVersionInfo = FileVersionInfo.GetVersionInfo(dllPath + "irremote.DLL");
          if (dllVersionInfo.FileVersion.CompareTo(irremote.CurrentVersion) < 0)
          {
            hcwDriverUpToDate = false;
          }
        }

        if (!hcwDriverUpToDate)
        {
          Log.Info("HCW: ==============================================================================================");
          Log.Info("HCW: Your remote control driver components are not up to date! To avoid problems, you should");
          Log.Info("HCW: get the latest Hauppauge drivers here: http://www.hauppauge.co.uk/board/showthread.php?p=25253");
          Log.Info("HCW: ==============================================================================================");
        }

        _inputHandler = new InputHandler("Hauppauge HCW");
        if (!_inputHandler.IsLoaded)
        {
          _controlEnabled = false;
          Log.Info("HCW: Error loading default mapping file - please reinstall MediaPortal");
        }
      }

      if (_controlEnabled)
      {
        _connection = new Connection(_logVerbose);

        _connection.Start(_port + 1);
        _connection.ReceiveEvent += new Connection.ReceiveEventHandler(OnReceive);

        Process process = Process.GetCurrentProcess();
        Log.Info("Process: {0}", process.ProcessName);

        Process procHelper = new Process();
        procHelper.StartInfo.FileName = string.Format("{0}\\HcwHelper.exe", Application.StartupPath);
        procHelper.Start();
        if (_allowExternal)
        {
          Log.Info("HCW: AllowExternal");
          Util.Utils.OnStartExternal += new Util.Utils.UtilEventHandler(OnStartExternal);
          Util.Utils.OnStopExternal += new Util.Utils.UtilEventHandler(OnStopExternal);
        }
        Thread checkThread = new Thread(new ThreadStart(CheckThread));
        checkThread.IsBackground = true;
        checkThread.Name = "HcwHelperCheck";
        checkThread.Priority = ThreadPriority.AboveNormal;
        checkThread.Start();
      }
    }


    /// <summary>
    /// Makes sure that HCWHelper is always running when we need it
    /// </summary>
    private void CheckThread()
    {
      do
      {
        Thread.Sleep(1000);

        while (!_exit && (Process.GetProcessesByName("HcwHelper").Length > 0))
        {
          Thread.Sleep(1000);
        }

        if (!_exit)
        {
          using (Settings xmlreader = new MPSettings())
          {
            _controlEnabled = xmlreader.GetValueAsBool("remote", "HCW", false);
          }
          if (_controlEnabled)
          {
            Process.Start(Application.StartupPath + @"\HcwHelper.exe");
          }
          else
          {
            _exit = true;
          }
        }
      } while (!_exit);
    }


    /// <summary>
    /// DeInit all
    /// </summary>
    public void DeInit()
    {
      if (_controlEnabled)
      {
        _exit = true;
        if (_allowExternal)
        {
          Util.Utils.OnStartExternal -= new Util.Utils.UtilEventHandler(OnStartExternal);
          Util.Utils.OnStopExternal -= new Util.Utils.UtilEventHandler(OnStopExternal);
        }
        _connection.ReceiveEvent -= new Connection.ReceiveEventHandler(OnReceive);
        _connection.Send(_port, "APP", "SHUTDOWN", DateTime.Now);
        _connection.Stop();
        _connection = null;
      }
    }


    /// <summary>
    /// Start HCW control
    /// </summary>
    public void StartHcw()
    {
      _connection.Send(_port, "APP", "IR_START", DateTime.Now);
    }


    /// <summary>
    /// Stop HCW control
    /// </summary>
    public void StopHcw()
    {
      _connection.Send(_port, "APP", "IR_STOP", DateTime.Now);
    }


    private void OnReceive(string strReceive)
    {
      if (_logVerbose)
      {
        Log.Info("HCW: received: {0}", strReceive);
      }
      string msg = strReceive.Split('~')[0];
      if (_logVerbose)
      {
        Log.Info("HCW: Accepted: {0}", msg);
      }
      switch (msg.Split('|')[0])
      {
        case "CMD":
          {
            // Time of button press - Use this for repeat delay calculations
            DateTime sentTime = DateTime.FromBinary(Convert.ToInt64(msg.Split('|')[2]));
            int newCommand = Convert.ToInt16(msg.Split('|')[1]);

            if (_logVerbose)
            {
              Log.Info("HCW: elapsed time: {0}", ((TimeSpan)(sentTime - _lastTime)).Milliseconds);
            }
            if (_logVerbose)
            {
              Log.Info("HCW: sameCommandCount: {0}", _sameCommandCount.ToString());
            }

            if (_lastCommand == newCommand)
            {
              // button release time elapsed since last identical command
              // if so, reset counter & start new session
              if ((sentTime - _lastTime) > _buttonRelease)
              {
                _sameCommandCount = 0; // new session with this button
                if (_logVerbose)
                {
                  Log.Info("HCW: same command, timeout true");
                }
              }
              else
              {
                if (_logVerbose)
                {
                  Log.Info("HCW: same command, timeout false");
                }
                _sameCommandCount++; // button release time not elapsed
              }
            }
            else
            {
              _sameCommandCount = 0; // we got a new button
            }

            bool executeKey = false;

            // new button / session
            if (_sameCommandCount == 0)
            {
              executeKey = true;
              //here
            }

            //// we got the identical button often enough to accept it
            if (_sameCommandCount == _repeatFilter)
            {
              executeKey = true;
            }

            // we got the identical button accepted and still pressed, repeat with repeatSpeed
            if ((_sameCommandCount > _repeatFilter) && (_sameCommandCount > _lastExecutedCommandCount + _repeatSpeed))
            {
              executeKey = true;
            }

            if (HCWKeyPressed != null)
            {
              _filterDoubleKlicks = true;
            }

            // double click filter
            if (executeKey && _filterDoubleKlicks)
            {
              int keyCode = newCommand;

              // strip remote type
              if (keyCode > 2000)
              {
                keyCode = keyCode - 2000;
              }
              else if (keyCode > 1000)
              {
                keyCode = keyCode - 1000;
              }

              if ((_sameCommandCount > 0) &&
                  (keyCode == 46 || //46 = fullscreen/green button
                   keyCode == 37 || //37 = OK button
                   keyCode == 56 || //56 = yellow button
                   keyCode == 11 || //11 = red button
                   keyCode == 41 || //41 = blue button
                   keyCode == 13 || //13 = menu button
                   keyCode == 15 || //15 = mute button
                   keyCode == 48)) //48 = pause button
              {
                executeKey = false;
                if (_logVerbose)
                {
                  Log.Info("HCW: doubleclick supressed: {0}", newCommand.ToString());
                }
              }
              else
              {
                //Send command for remote control learning
                if (HCWKeyPressed != null)
                {
                  HCWKeyPressed(newCommand);
                }
              }
            }

            if (executeKey)
            {
              _lastExecutedCommandCount = _sameCommandCount;
              _lastCommand = newCommand;
              //Send command to application...
              if (_inputHandler != null)
              {
                if (!_inputHandler.MapAction(newCommand))
                {
                  Log.Info("HCW: No mapping found");
                }
                else
                {
                  if (_logVerbose)
                  {
                    Log.Info("HCW: repeat filter accepted: {0}", newCommand.ToString());
                  }
                }
              }
            }
            _lastTime = sentTime;
          }
          break;
        case "APP":

          if (msg.Split('|')[1] == "STOP")
          {
            if (_logVerbose)
            {
              Log.Info("HCW: received STOP from HcwHelper");
            }
            _controlEnabled = false;
            _exit = true;
            StopHcw();
          }
          break;
      }
    }


    /// <summary>
    /// External process (e.g. myPrograms) started
    /// </summary>
    /// <param name="proc"></param>
    /// <param name="waitForExit"></param>
    public void OnStartExternal(Process proc, bool waitForExit)
    {
      StopHcw();
    }


    /// <summary>
    /// External process (e.g. myPrograms) stopped
    /// </summary>
    /// <param name="proc"></param>
    /// <param name="waitForExit"></param>
    public void OnStopExternal(Process proc, bool waitForExit)
    {
      StartHcw();
    }


    /// <summary>
    /// Handle energy saving situations
    /// </summary>
    /// <param name="msg">Message</param>
    /// <returns>Message handled</returns>
    public bool WndProc(Message msg)
    {
      if (_controlEnabled)
      {
        switch (msg.Msg)
        {
          case WM_POWERBROADCAST:
            if (msg.WParam.ToInt32() == PBT_APMRESUMEAUTOMATIC)
            {
              try
              {
                StartHcw();
              }
              catch (Exception e)
              {
                Log.Error("StartHCW failed. Exception is: {0}", e.Message);
              }
              return true;
            }
            break;

          case WM_ACTIVATE:
            if (_allowExternal && !_keepControl)
            {
              switch ((int)msg.WParam)
              {
                case WA_INACTIVE:
                  if (_logVerbose)
                  {
                    Log.Info("HCW: lost focus");
                  }
                  {
                    StopHcw();
                    return true;
                  }
                case WA_ACTIVE:
                case WA_CLICKACTIVE:
                  if (_logVerbose)
                  {
                    Log.Info("HCW: got focus");
                  }
                  {
                    StartHcw();
                    return true;
                  }
              }
            }
            break;
        }
      }
      return false;
    }

    /// <summary>
    /// Required for the IInputDevice interface
    /// </summary>    
    /// <param name="msg"></param>
    /// <param name="action"></param>
    /// <param name="key"></param>
    /// <param name="keyCode"></param>
    /// <returns></returns>
    public bool WndProc(ref System.Windows.Forms.Message msg, out GUI.Library.Action action, out char key, out Keys keyCode)
    {
      action = null;
      key = (char)0;
      keyCode = Keys.A;
      return WndProc(msg);
    }

    /// <summary>
    /// Get the mapping for this wndproc action
    /// </summary>
    /// <param name="msg"></param>
    /// <returns></returns>
    public MediaPortal.InputDevices.InputHandler.Mapping GetMapping(Message msg)
    {
      return null;
    }
  }
}