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
using System.Threading;
using System.Windows.Forms;
using MediaPortal.GUI.Library;

namespace MediaPortal.InputDevices.HcwHelper
{
  internal static class Program
  {
    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    private static void Main()
    {
      Log.Info("HCWHelper: Starting up");
      Thread.CurrentThread.Priority = ThreadPriority.Highest;

      if ((Process.GetProcessesByName("HcwHelper").Length == 1) &&
          ((Process.GetProcessesByName("MediaPortal").Length > 0) ||
           Process.GetProcessesByName("Configuration").Length > 0 ||
           (Process.GetProcessesByName("MediaPortal.vshost").Length > 0)))
      {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);
        Process.GetCurrentProcess().PriorityClass = ProcessPriorityClass.RealTime;
        Form hcwHelper = new HcwHelper();
        try
        {
          hcwHelper.ShowDialog();
        }
        catch (ObjectDisposedException) {}
      }
      else if (Process.GetProcessesByName("HcwHelper").Length != 1)
      {
        Log.Info("HCWHelper: HCWHelper already running - exiting");
      }
      else
      {
        Log.Info("HCWHelper: MediaPortal not running - exiting");
      }
      Log.Info("HCWHelper: Shutting down");
    }
  }
}