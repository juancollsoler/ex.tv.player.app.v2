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

using System.IO;

namespace MediaPortal.Support
{
  public class PsLogEventsLogger : ILogCreator
  {
    private ProcessRunner runner;
    private string[] logNames = {"Application", "System"};

    public PsLogEventsLogger(ProcessRunner runner)
    {
      this.runner = runner;
    }

    public void CreateLogs(string destinationFolder)
    {
      destinationFolder = Path.GetFullPath(destinationFolder) + "\\";
      runner.Executable = Path.GetFullPath("psloglist.exe");
      foreach (string logName in logNames)
      {
        runner.Arguments = "-g \"" + destinationFolder + logName + ".evt\" " + logName;
        runner.Run();
      }
    }

    public string ActionMessage
    {
      get { return "Gathering generated eventlogs..."; }
    }
  }
}