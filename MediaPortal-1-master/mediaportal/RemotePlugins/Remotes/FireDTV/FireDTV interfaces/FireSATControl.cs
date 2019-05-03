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

#region Usings

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using MediaPortal.Configuration;
using MediaPortal.GUI.Library;
using MediaPortal.Util;

#endregion

namespace MediaPortal.InputDevices.FireDTV
{
  /// <summary>
  /// Summary description for FireDTVControl.
  /// </summary>
  public class FireDTVControl
  {
    #region Constructor / Destructor

    /// <summary>
    /// Try to locate the FireDTV API library and initialise the library.
    /// </summary>
    /// <param name="windowHandle"></param>
    public FireDTVControl(IntPtr windowHandle)
    {
      try
      {
        string prgPath = Environment.GetEnvironmentVariable("ProgramW6432");
        if (string.IsNullOrEmpty(prgPath))
        {
          prgPath = Environment.GetEnvironmentVariable("ProgramFiles");
        }

        // Look for Digital Everywhere's software which uses a hardcoded path
        string fullDllPath = Path.Combine(prgPath, @"FireDTV\Tools\FiresatApi.dll");

        try
        {
          Win32API.SetDllDirectory(Path.GetDirectoryName(fullDllPath));
        }
        catch (Exception ex1)
        {
          Log.Error("FireDTV: Trying to enable FireDTV remote but failed to set its path. Error: {0}", ex1.Message);
        }
      }
      catch (Exception ex)
      {
        Log.Error("FireDTV: Trying to enable FireDTV remote but failed with error: {0}", ex.Message);
        return;
      }

      _windowHandle = windowHandle;

      // initialise the library
      InitializeLibrary();
      RegisterGeneralNotifications();
    }

    /// <summary>
    /// Default contructer should not be called.
    /// </summary>
    private FireDTVControl() {}


    ~FireDTVControl()
    {
      CloseDrivers();
    }

    #endregion

    #region Private Methods

    #region Initialization

    [DebuggerStepThrough]
    internal void InitializeLibrary()
    {
      if (!LibrayInitialized)
      {
        try
        {
          uint returnCode = FireDTVAPI.FS_Initialize();
          if ((FireDTVConstants.FireDTVStatusCodes)returnCode != FireDTVConstants.FireDTVStatusCodes.Success)
          {
            throw new FireDTVInitializationException("Initilization Failure (" + returnCode.ToString() + ")");
          }
          LibrayInitialized = true;
          Log.Info("FireDTV: dll initialized");
        }
        catch (Exception e)
        {
          Log.Error("FireDTV: error initializing {0}", e.Message);
        }
      }
    }

    #endregion

    #region FireDTV Open/Close Device

    internal uint OpenWDMDevice(int deviceIndex)
    {
      uint DeviceHandle;
      uint returnCode = FireDTVAPI.FS_OpenWDMDeviceHandle((uint)deviceIndex, out DeviceHandle);
      if ((FireDTVConstants.FireDTVStatusCodes)returnCode != FireDTVConstants.FireDTVStatusCodes.Success)
      {
        throw new FireDTVDeviceOpenException((FireDTVConstants.FireDTVStatusCodes)returnCode, "Open WDM Device Error!");
      }
      return DeviceHandle;
    }

    internal uint OpenBDADevice(int deviceIndex)
    {
      uint DeviceHandle;
      uint returnCode = FireDTVAPI.FS_OpenBDADeviceHandle((uint)deviceIndex, out DeviceHandle);
      if ((FireDTVConstants.FireDTVStatusCodes)returnCode != FireDTVConstants.FireDTVStatusCodes.Success)
      {
        throw new FireDTVDeviceOpenException((FireDTVConstants.FireDTVStatusCodes)returnCode, "Open BDA Device Error!");
      }
      return DeviceHandle;
    }

    internal void CloseFireDTVHandle(FireDTVSourceFilterInfo currentSourceFilter)
    {
      try
      {
        uint returnCode = FireDTVAPI.FS_CloseDeviceHandle(currentSourceFilter.Handle);
        if ((FireDTVConstants.FireDTVStatusCodes)returnCode != FireDTVConstants.FireDTVStatusCodes.Success)
        {
          throw new FireDTVException((FireDTVConstants.FireDTVStatusCodes)returnCode, "Device Close Failure");
        }
      }
      catch (Exception) {}
    }

    internal int getWDMCount()
    {
      try
      {
        uint WDMCount;
        uint returnCode = FireDTVAPI.FS_GetNumberOfWDMDevices(out WDMCount);
        if ((FireDTVConstants.FireDTVStatusCodes)returnCode != FireDTVConstants.FireDTVStatusCodes.Success)
        {
          throw new FireDTVException((FireDTVConstants.FireDTVStatusCodes)returnCode, "Unable to WDM Driver Count");
        }
        return (int)WDMCount;
      }
      catch (Exception ex)
      {
        Log.Error("FireSATControl: Error getting WDM Devices {0}", ex.Message);
      }
      return 0;
    }

    internal int getBDACount()
    {
      try
      {
        uint BDACount;
        uint returnCode = FireDTVAPI.FS_GetNumberOfBDADevices(out BDACount);
        if ((FireDTVConstants.FireDTVStatusCodes)returnCode != FireDTVConstants.FireDTVStatusCodes.Success)
        {
          throw new FireDTVException((FireDTVConstants.FireDTVStatusCodes)returnCode, "Unable to BDA Driver Count");
        }
        return (int)BDACount;
      }
      catch (Exception ex)
      {
        Log.Error("FireSATControl: Error getting BDA Devices {0}", ex.Message);
      }
      return 0;
    }

    #endregion

    #region FireDTV Register Notifications

    internal void UnRegisterNotifications(uint widowHandle)
    {
      if (NotificationsRegistered)
      {
        uint returnCode = FireDTVAPI.FS_UnregisterNotifications(widowHandle);
        if ((FireDTVConstants.FireDTVStatusCodes)returnCode != FireDTVConstants.FireDTVStatusCodes.Success)
        {
          throw new FireDTVException((FireDTVConstants.FireDTVStatusCodes)returnCode,
                                     "Unable to unRegister Notifiations");
        }
        NotificationsRegistered = false;
      }
    }

    [DebuggerStepThrough]
    internal void RegisterGeneralNotifications()
    {
      if (!NotificationsRegistered)
      {
        try
        {
          uint returnCode = FireDTVAPI.FS_RegisterGeneralNotifications((int)WindowsHandle);
          if ((FireDTVConstants.FireDTVStatusCodes)returnCode != FireDTVConstants.FireDTVStatusCodes.Success)
          {
            throw new FireDTVException((FireDTVConstants.FireDTVStatusCodes)returnCode,
                                       "Unable to Register General Notifiations");
          }
          NotificationsRegistered = true;
        }
        catch (Exception) {}
      }
    }

    #endregion

    #endregion

    #region Private Variables

    private bool LibrayInitialized = false;
    private bool NotificationsRegistered = false;
    private IntPtr _windowHandle = (IntPtr)0;
    private SourceFilterCollection _sourceFilterCollection = new SourceFilterCollection();

    #endregion

    #region Properties

    public SourceFilterCollection SourceFilters
    {
      get { return _sourceFilterCollection; }
      set { _sourceFilterCollection = value; }
    }

    /// <summary>
    /// Get the API version of the FireDTV libary
    /// </summary>
    public string APIVersion
    {
      get { return Marshal.PtrToStringAnsi(FireDTVAPI.FS_GetApiVersion()); }
    }

    public IntPtr WindowsHandle
    {
      get
      {
        if (_windowHandle == (IntPtr)0)
        {
          return (IntPtr)Win32API.GetActiveWindow();
        }
        else
        {
          return _windowHandle;
        }
      }
    }

    #endregion

    #region Public Methods

    /// <summary>
    /// Open the communication channels with the FireDTV's.
    /// </summary>
    /// <returns>true if success</returns>
    public bool OpenDrivers()
    {
      if (!LibrayInitialized)
      {
        return false;
      }

      int BDADriverCount = getBDACount();
      int WDMDriverCount = getWDMCount();

      Log.Info("FireDTV: BDA {0}, WMA {1}", BDADriverCount, WDMDriverCount);


      for (int BDACount = 0; BDACount < BDADriverCount; BDACount++)
      {
        FireDTVSourceFilterInfo bdaSourceFilter = new FireDTVSourceFilterInfo(OpenBDADevice(BDACount), _windowHandle);
        if (bdaSourceFilter != null)
        {
          Log.Info("FireDTV: add BDA Source {0}", bdaSourceFilter.ToString());
        }

        _sourceFilterCollection.Add(bdaSourceFilter);
      }

      for (int WDMCount = 0; WDMCount < WDMDriverCount; WDMCount++)
      {
        FireDTVSourceFilterInfo wdmSourceFilter = new FireDTVSourceFilterInfo(OpenWDMDevice(WDMCount), _windowHandle);
        if (wdmSourceFilter != null)
        {
          Log.Info("FireDTV: add WDM Source");
        }
        _sourceFilterCollection.Add(wdmSourceFilter);
      }
      return true;
    }

    public void CloseDrivers()
    {
      for (int DeviceCount = 0; DeviceCount < SourceFilters.Count; DeviceCount++)
      {
        FireDTVSourceFilterInfo SourceFilter = SourceFilters.Item(DeviceCount);
        _sourceFilterCollection.RemoveAt(DeviceCount);
      }
    }

    public bool StopRemoteControlSupport()
    {
      foreach (FireDTVSourceFilterInfo SourceFilter in _sourceFilterCollection)
      {
        if (SourceFilter.RemoteRunning)
        {
          SourceFilter.StopFireDTVRemoteControlSupport();
          return SourceFilter.RemoteRunning;
        }
      }
      return false;
    }

    #endregion
  }
}