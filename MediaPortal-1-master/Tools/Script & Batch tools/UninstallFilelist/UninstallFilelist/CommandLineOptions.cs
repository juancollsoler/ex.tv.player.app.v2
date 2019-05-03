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
using System.Collections.Generic;
using MediaPortal.Utilities.CommandLine;

namespace UninstallFilelist
{
  public class CommandLineOptions : ICommandLineOptions
  {
    public enum Option
    {
      dir,
      ignore,
      output,
    }

    private readonly Dictionary<Option, string> _options;

    public CommandLineOptions()
    {
      _options = new Dictionary<Option, string>();
    }

    public void SetOption(string option, string argument)
    {
      _options.Add((Option)Enum.Parse(typeof (Option), option, true), argument);
    }

    public void DisplayOptions()
    {
      Console.WriteLine("Vaid Command Line options:");
      Console.WriteLine("/dir=<directory> directory to search for files");
      Console.WriteLine("/output=<file>   result will be written to that file");
      Console.WriteLine("/ignore=<file>   contains files and foldersm, which will be ignored");
    }

    public bool IsOption(Option option)
    {
      return _options.ContainsKey(option);
    }

    public int Count
    {
      get { return _options.Count; }
    }

    public string GetOption(Option option)
    {
      return _options[option];
    }
  }
}