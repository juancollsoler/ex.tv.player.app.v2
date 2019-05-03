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
using System.Text;

namespace MediaPortal.TagReader
{
  /// <summary>
  /// Simpliest ICueTrackFileBuilder implementation.
  /// Builds string Cue fake track file name from Cue track
  /// </summary>
  internal class CueTrackFileStringBuilder : ICueTrackFileBuilder<string>
  {
    #region Public Methods

    public string getFileName(string fobj)
    {
      return fobj;
    }

    public string build(string cueFileName, CueSheet cueSheet, Track track)
    {
      return CueUtil.buildCueFakeTrackFileName(cueFileName, track);
    }

    #endregion
  }
}