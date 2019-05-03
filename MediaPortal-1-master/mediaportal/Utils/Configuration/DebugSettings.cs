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
using MediaPortal.ServiceImplementations;

namespace MediaPortal.Configuration
{
  /// <summary>
  /// This class is responsible for managing debug settings 
  /// not configured through normal TV Server configuration.
  /// Each setting's status is determined by the existence 
  /// of a similarly named file in the TV Server's data dir
  /// </summary>
  public class DebugSettings
  {
    private static string SettingsPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) +
                                         @"\Team MediaPortal\MediaPortal\debug\";

    /// <summary>
    /// Get the path and filename of the requested debug setting
    /// </summary>
    /// <param name="setting">the name of the setting</param>
    /// <returns>the path</returns>
    public static string SettingPath(string setting)
    {
      return SettingsPath + setting + ".txt";
    }

    /// <summary>
    /// Get the value of a debug setting
    /// </summary>
    /// <param name="setting">the name of the setting</param>
    /// <returns>true if the setting is enabled, otherwise false</returns>
    public static bool GetSetting(string setting)
    {
      return File.Exists(SettingPath(setting));
    }

    /// <summary>
    /// Enables or disbables a debug setting
    /// </summary>
    /// <param name="setting">the name of the setting</param>
    /// <param name="enabled">true to enable the setting, otherwise false</param>
    public static void SetSetting(string setting, bool enabled)
    {
      string settingPath = SettingPath(setting);
      try
      {
        if (File.Exists(settingPath) == enabled)
        {
          return;
        }

        if (enabled)
        {
          if (!Directory.Exists(SettingsPath))
          {
            Directory.CreateDirectory(SettingsPath);
          }
          File.CreateText(settingPath).Close();
        }
        else
        {
          File.Delete(settingPath);
        }
      }
      catch (IOException ex)
      {
        Log.Error("Failed to write debug setting '{0}'", setting);
        Log.Error(ex);
      }
    }

    /// <summary>
    /// Returns true if existing timeshifted video should be
    /// prepended to a "Record Now" recording
    /// </summary>
    public static bool EnableRecordingFromTimeshift
    {
      get { return GetSetting("EnableRecordingFromTimeshift"); }
      set { SetSetting("EnableRecordingFromTimeshift", value); }
    }

    /// <summary>
    /// Returns true if, when changing channels live TV should 
    /// just wait for video to sync to audio instead of using 
    /// slow motion video
    /// </summary>
    public static bool DoNotAllowSlowMotionDuringZapping
    {
      get { return GetSetting("DoNotAllowSlowMotionDuringZapping"); }
      set { SetSetting("DoNotAllowSlowMotionDuringZapping", value); }
    }
  }
}