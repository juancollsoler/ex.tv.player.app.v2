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
using MediaPortal.ExtensionMethods;

namespace MediaPortal.GUI.Library
{
  /// <summary>
  /// Summary description for GUIVolumeBar.
  /// </summary>
  public class GUIVolumeBar : GUIControl
  {
    private GUIImage _imageVolumeBar;
    private int _current = 0;
    private int _maximum = 10;
    private int _image1 = 0;
    private int _image2 = 1;
    private Rectangle _destinationRectangle = new Rectangle(0, 0, 0, 0);
    private Rectangle _sourceRectangle = new Rectangle(0, 0, 0, 0);
    [XMLSkinElement("align")] protected Alignment _alignment = Alignment.ALIGN_RIGHT;
    [XMLSkinElement("texture")] protected string _textureName = "";
    [XMLSkinElement("imageHeight")] protected int _imageHeight = 3;

    public GUIVolumeBar(int dwParentID) : base(dwParentID)
    {
      _imageVolumeBar = new GUIImage(dwParentID, 0, 0, 0, 0, 0, "", 0);
      _imageVolumeBar.ParentControl = this;
      DimColor = base.DimColor;
    }

    public GUIVolumeBar(int dwParentID, int dwControlId, int dwPosX, int dwPosY, int dwWidth, int dwHeight)
      : base(dwParentID, dwControlId, dwPosX, dwPosY, dwWidth, dwHeight)
    {
      _imageVolumeBar = new GUIImage(dwParentID, dwControlId, dwPosX, dwPosY, 0, 0, _textureName, 0);
      _imageVolumeBar.ParentControl = this;
    }

    public override void Render(float timePassed)
    {
      if (!IsVisible)
      {
        base.Render(timePassed);
        return;
      }
      if (_imageVolumeBar.TextureWidth <= 0)
      {
        base.Render(timePassed);
        return;
      }

      try
      {
        double widthRatio = _width > 0 ? (double)((_maximum - 1) * _imageVolumeBar.TextureWidth) / _width : 1.0f;
        int width = (int)Math.Round(((_maximum - 1) * _imageVolumeBar.TextureWidth) / widthRatio);
        int rectWidth = (int)Math.Round(_imageVolumeBar.TextureWidth / widthRatio);
        int correctedWidth = rectWidth * (_maximum - 1);

        _sourceRectangle.Y = _image1 * (_imageVolumeBar.TextureHeight / _imageHeight);
        _sourceRectangle.Width = _imageVolumeBar.TextureWidth;
        _sourceRectangle.Height = _imageVolumeBar.TextureHeight / _imageHeight;

        switch (_alignment)
        {
          case Alignment.ALIGN_LEFT:
            _destinationRectangle.X = _positionX;
            break;
          case Alignment.ALIGN_CENTER:
            _destinationRectangle.X = _positionX -
                                      (((correctedWidth)) / 2);
            break;
          case Alignment.ALIGN_RIGHT:
            _destinationRectangle.X = rectWidth + _positionX -
                                      (correctedWidth);
            break;
        }

        _destinationRectangle.Y = _positionY;
        _destinationRectangle.Width = rectWidth; 
        _destinationRectangle.Height = _height;

        for (int index = 0; index < _current; ++index)
        {
          _imageVolumeBar.RenderRect(timePassed, _sourceRectangle, _destinationRectangle);

          _destinationRectangle.X += _destinationRectangle.Width;
        }

        if (_image2 != _image1)
        {
          _sourceRectangle.Y = _image2 * (_imageVolumeBar.TextureHeight / _imageHeight);
        }

        for (int index = _current + 1; index < _maximum; ++index)
        {
          _imageVolumeBar.RenderRect(timePassed, _sourceRectangle, _destinationRectangle);

          _destinationRectangle.X += _destinationRectangle.Width;
        }
      }
      catch (Exception e)
      {
        Log.Info(e.Message);
      }

      base.Render(timePassed);
    }

    public override void AllocResources()
    {
      base.AllocResources();
      _imageVolumeBar.SetFileName(_textureName);
      _imageVolumeBar.AllocResources();
    }

    public override void Dispose()
    {
      base.Dispose();
      _imageVolumeBar.SafeDispose();
    }

    public int Image1
    {
      get { return _image1; }
      set { _image1 = value; }
    }

    public int Image2
    {
      get { return _image2; }
      set { _image2 = value; }
    }

    public int ImageHeight
    {
      get { return _imageHeight; }
      set { _imageHeight = value; }
    }

    public int Current
    {
      get { return _current; }
      set { _current = Math.Max(0, Math.Min(value, _maximum)); }
    }

    public int Maximum
    {
      get { return _maximum; }
      set { _maximum = value; }
    }

    public int TextureHeight
    {
      get { return _imageVolumeBar.TextureHeight; }
    }

    public int TextureWidth
    {
      get { return _imageVolumeBar.TextureWidth; }
    }

    public override int DimColor
    {
      get { return base.DimColor; }
      set
      {
        base.DimColor = value;
        if (_imageVolumeBar != null)
        {
          _imageVolumeBar.DimColor = value;
        }
      }
    }

    public string TextureName
    {
      get { return _textureName; }
      set { _textureName = value; }
    }

    public Alignment TextAlignment
    {
      get { return _alignment; }
      set { _alignment = value; }
    }
  }
}