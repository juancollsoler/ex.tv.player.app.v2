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
using System.Drawing;
using System.Windows.Forms;
using System.Xml;

namespace MPTail
{
  public struct LastSettings
  {
    public int left;
    public int top;
    public int width;
    public int height;
    public int categoryIndex;
    public int tabIndex;
    public FormWindowState windowState;
  }

  // This is just a helper class to allow to insert duplicate keys in a SortedDictionary
  // If a timestamp is equal to an existing one we just return 1 instead of 0
  public class MyDateTime : IComparable
  {
    private readonly DateTime dt;

    public MyDateTime(DateTime dateTime)
    {
      dt = dateTime;
    }

    public MyDateTime(string dateTimeStr)
    {
      dt = DateTime.Parse(dateTimeStr);
    }

    public int CompareTo(object obj)
    {
      MyDateTime mdt = (MyDateTime)obj;
      int ret = DateTime.Compare(dt, mdt.dt);
      if (ret == 0)
        ret = 1;
      return ret;
    }
  }

  public class SearchParameters
  {
    public string searchStr = "";
    public bool caseSensitive = false;
    public Color highlightColor = Color.Yellow;
  }

  public enum LoggerCategory
  {
    MediaPortal,
    TvEngine,
    Custom
  }

  internal class XmlUtils
  {
    public static void NewAttribute(XmlNode node, string name, string value)
    {
      XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
      attr.InnerText = value;
      node.Attributes.Append(attr);
    }

    public static void NewAttribute(XmlNode node, string name, bool value)
    {
      XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
      if (value)
        attr.InnerText = "1";
      else
        attr.InnerText = "0";
      node.Attributes.Append(attr);
    }

    public static void NewAttribute(XmlNode node, string name, int value)
    {
      XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
      attr.InnerText = value.ToString();
      node.Attributes.Append(attr);
    }

    public static void NewAttribute(XmlNode node, string name, float value)
    {
      XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
      attr.InnerText = value.ToString();
      node.Attributes.Append(attr);
    }

    public static void NewAttribute(XmlNode node, string name, Color value)
    {
      XmlAttribute attr = node.OwnerDocument.CreateAttribute(name);
      attr.InnerText = value.ToArgb().ToString();
      node.Attributes.Append(attr);
    }
  }
}