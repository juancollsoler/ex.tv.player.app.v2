﻿#region Copyright (C) 2005-2013 Team MediaPortal

// Copyright (C) 2005-2013 Team MediaPortal
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
using System.Collections;
using MediaPortal.GUI.Library;
using TvEngine.PowerScheduler.Interfaces;

#endregion

namespace MediaPortal.Plugins.Process.Handlers
{
  /// <summary>
  /// Wakeup handler for Wakeable plugins
  /// </summary>
  public class WakeableWakeupHandler : IWakeupHandler
  {
    #region Variables

    private DateTime _nextWakeupTime;
    private string _handlerName;

    #endregion

    #region IWakeupHandler implementation

    /// <summary>
    /// GetNextWakeupTime()
    /// </summary>
    /// <param name="earliestWakeupTime"></param>
    /// <returns></returns>
    public DateTime GetNextWakeupTime(DateTime earliestWakeupTime)
    {
      _nextWakeupTime = DateTime.MaxValue;
      _handlerName = "WakeableWakeupPlugins";

      // Inspect all found IWakeable plugins from PluginManager
      ArrayList wakeables = PluginManager.WakeablePlugins;
      foreach (IWakeable wakeable in wakeables)
      {
        DateTime nextTime = wakeable.GetNextEvent(earliestWakeupTime);
        if (nextTime < earliestWakeupTime)
          continue;
        if (nextTime < _nextWakeupTime)
        {
          _nextWakeupTime = nextTime;
          _handlerName = wakeable.PluginName();
        }
      }
      return _nextWakeupTime;
    }

    /// <summary>
    /// HandlerName
    /// </summary>
    public string HandlerName
    {
      get { return _handlerName; }
    }

    #endregion
  }
}
