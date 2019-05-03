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
using System.Xml.Serialization;
using MediaPortal.Utils.Web;

namespace MediaPortal.WebEPG.Config.Grabber
{
  /// <summary>
  /// WebParser information.
  /// </summary>
  [Serializable]
  public class WebParserTemplate
  {
    #region Variables

    [XmlElement("Template")] public List<HtmlParserTemplate> Templates;
    [XmlArray("DataPreference")] [XmlArrayItem("Preference")] public List<DataPreference> preferences;
    [XmlArray("Sublinks")] [XmlArrayItem("Sublink")] public List<SublinkInfo> sublinks;
    [XmlArray("Searches")] [XmlArrayItem("Search")] public List<WebSearchData> searchList;
    [XmlArray("DateTime")] [XmlArrayItem("Month")] public string[] months;

    #endregion

    #region Public Methods

    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <param name="name">The template name.</param>
    /// <returns></returns>
    public HtmlParserTemplate GetTemplate(string name)
    {
      for (int i = 0; i < Templates.Count; i++)
      {
        if (Templates[i].Name == name)
        {
          return Templates[i];
        }
      }
      return null;
    }

    /// <summary>
    /// Gets the preference.
    /// </summary>
    /// <param name="name">The template name.</param>
    /// <returns>data preference</returns>
    public DataPreference GetPreference(string name)
    {
      for (int i = 0; i < preferences.Count; i++)
      {
        if (preferences[i].Template == name)
        {
          return preferences[i];
        }
      }
      return null;
    }

    #endregion
  }
}