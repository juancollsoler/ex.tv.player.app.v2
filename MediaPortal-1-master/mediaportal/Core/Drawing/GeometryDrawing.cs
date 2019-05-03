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
  public sealed class GeometryDrawing
  {
    #region Constructors

    public GeometryDrawing() {}

    public GeometryDrawing(Brush brush, Pen pen, Geometry geometry) {}

    #endregion Constructors

    #region Methods

    private void RaiseChanged() {}

    #endregion Methods

    #region Properties

    public Brush Brush
    {
      get { return _brush; }
      set
      {
        if (!Equals(_brush, value))
        {
          _brush = value;
          RaiseChanged();
        }
      }
    }

    public Geometry Geometry
    {
      get { return _geometry; }
      set
      {
        if (!Equals(_geometry, value))
        {
          _geometry = value;
          RaiseChanged();
        }
      }
    }

    public Pen Pen
    {
      get { return _pen; }
      set
      {
        if (!Equals(_pen, value))
        {
          _pen = value;
          RaiseChanged();
        }
      }
    }

    #endregion Properties

    #region Fields

    private Brush _brush;
    private Geometry _geometry;
    private Pen _pen;

    #endregion Fields
  }
}