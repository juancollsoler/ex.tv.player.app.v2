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

namespace MediaPortal.Drawing
{
  public class DashStyles
  {
    #region Properties

    public static DashStyle Dash
    {
      get
      {
        if (_dash == null)
        {
          _dash = new DashStyle(new double[] {2, 2}, 1);
        }
        return _dash;
      }
    }

    public static DashStyle DashDot
    {
      get
      {
        if (_dashDot == null)
        {
          _dashDot = new DashStyle(new double[] {2, 2, 0, 2}, 1);
        }
        return _dashDot;
      }
    }

    public static DashStyle DashDotDot
    {
      get
      {
        if (_dashDotDot == null)
        {
          _dashDotDot = new DashStyle(new double[] {2, 2, 0, 2, 0, 2}, 1);
        }
        return _dashDotDot;
      }
    }

    public static DashStyle Dot
    {
      get
      {
        if (_dot == null)
        {
          _dot = new DashStyle(new double[] {0, 2}, 1);
        }
        return _dot;
      }
    }

    public static DashStyle Solid
    {
      get
      {
        if (_solid == null)
        {
          _solid = new DashStyle();
        }
        return _solid;
      }
    }

    #endregion Properties

    #region Fields

    private static DashStyle _dash;
    private static DashStyle _dashDot;
    private static DashStyle _dashDotDot;
    private static DashStyle _dot;
    private static DashStyle _solid;

    #endregion Fields
  }
}