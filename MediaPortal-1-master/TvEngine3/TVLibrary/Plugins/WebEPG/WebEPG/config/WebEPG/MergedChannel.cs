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

using System.Xml.Serialization;

namespace MediaPortal.WebEPG.Config
{
  /// <summary>
  /// The information for a Channel.
  /// </summary>
  public class MergedChannel
  {
    #region Variables

    private string _id;
    private string _grabber;
    private string _start;
    private string _end;

    [XmlAttribute("id")]
    public string id
    {
      get { return _id; }
      set { _id = value; }
    }

    [XmlAttribute("grabber")]
    public string grabber
    {
      get { return _grabber; }
      set { _grabber = value; }
    }

    [XmlAttribute("start")]
    public string start
    {
      get { return _start; }
      set { _start = value; }
    }

    [XmlAttribute("end")]
    public string end
    {
      get { return _end; }
      set { _end = value; }
    }

    #endregion
  }
}