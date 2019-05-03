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
using System.Xml.Serialization;

namespace MediaPortal.Player.DSP
{
  /// <summary>
  /// This class holds information about a Paramter of a VST Plugin
  /// </summary>
  [Serializable]
  public class VSTPluginParm
  {
    /// <summary>
    /// Needed for XmlSerializer
    /// </summary>
    public VSTPluginParm() {}

    public VSTPluginParm(string name, int index, string value)
    {
      Name = name;
      Index = index;
      Value = value;
    }

    /// <summary>
    /// Name of the Parameter
    /// </summary>
    [XmlAttribute] public string Name = "";

    /// <summary>
    /// Index of the Parameter
    /// </summary>
    [XmlAttribute] public int Index = 0;

    /// <summary>
    /// Value of the Parameter
    /// </summary>
    [XmlAttribute] public string Value = "";
  }
}