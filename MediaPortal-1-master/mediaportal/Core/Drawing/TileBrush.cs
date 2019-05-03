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
  public abstract class TileBrush : Brush
  {
    #region Properties

    public AlignmentX AlignmentX
    {
      get { return _alignmentX; }
      set { _alignmentX = value; }
    }

    public AlignmentY AlignmentY
    {
      get { return _alignmentY; }
      set { _alignmentY = value; }
    }

    public Stretch Stretch
    {
      get { return _stretch; }
      set { _stretch = value; }
    }

    public TileMode TileMode
    {
      get { return _tileMode; }
      set { _tileMode = value; }
    }

    public Rect Viewbox
    {
      get { return _viewbox; }
      set { _viewbox = value; }
    }

    public BrushMappingMode ViewboxUnits
    {
      get { return _viewboxUnits; }
      set { _viewboxUnits = value; }
    }

    public Rect Viewport
    {
      get { return _viewport; }
      set { _viewport = value; }
    }

    public BrushMappingMode ViewportUnits
    {
      get { return _viewportUnits; }
      set { _viewportUnits = value; }
    }

    #endregion Properties

    #region Fields

    private AlignmentX _alignmentX;
    private AlignmentY _alignmentY;
    private Stretch _stretch;
    private TileMode _tileMode;
    private Rect _viewbox;
    private BrushMappingMode _viewboxUnits;
    private Rect _viewport;
    private BrushMappingMode _viewportUnits;

    #endregion Fields
  }
}