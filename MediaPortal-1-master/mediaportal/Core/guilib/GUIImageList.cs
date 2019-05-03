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
using System.Collections;
using MediaPortal.ExtensionMethods;

namespace MediaPortal.GUI.Library
{
  /// <summary>
  /// Summary description for GUIImageList.
  /// </summary>
  public class GUIImageList : GUIControl
  {
    [XMLSkinElement("align")] private Alignment _alignment = Alignment.ALIGN_LEFT;
    [XMLSkinElement("orientation")] private eOrientation _orientation = eOrientation.Horizontal;
    [XMLSkinElement("textureWidth")] private int _textureWidth = 32;
    [XMLSkinElement("textureHeight")] private int _textureHeight = 32;
    [XMLSkinElement("percentage")] private string _tagLine = string.Empty;
    [XMLSkinElement("imagesToDraw")] private int _imagesToDraw = -1;

    private int m_iPercentage;

    private ArrayList _itemList = new ArrayList();

    public GUIImageList(int dwParentID)
      : base(dwParentID)
    {
      m_iPercentage = 0;
    }


    public override void FinalizeConstruction()
    {
      base.FinalizeConstruction();

      GUIGraphicsContext.ScaleHorizontal(ref _textureWidth);
      GUIGraphicsContext.ScaleVertical(ref _textureHeight);

      for (int i = 0; i < SubItemCount; ++i)
      {
        string strTexture = (string)GetSubItem(i);
        GUIImage img = new GUIImage(_parentControlId, _controlId, _positionX, _positionY, _textureWidth, _textureHeight,
                                    strTexture, 0);
        img.ParentControl = this;
        img.DimColor = DimColor;
        _itemList.Add(img);
      }
    }


    public override void AllocResources()
    {
      foreach (GUIImage img in _itemList)
      {
        img.AllocResources();
      }
    }

    public override void Dispose()
    {
      base.Dispose();
      _itemList.SafeDispose();
    }

    public override void Render(float timePassed)
    {
      if (!IsVisible)
      {
        base.Render(timePassed);
        return;
      }
      if (_tagLine != string.Empty)
      {
        string percent = GUIPropertyManager.Parse(_tagLine);
        try
        {
          Percentage = (int)(Math.Round(Double.Parse(percent)) * 10d);
        }
        catch (Exception) {}
      }
      if (_orientation == eOrientation.Horizontal)
      {
        RenderHorizontal(timePassed);
      }
      else
      {
        RenderVertical(timePassed);
      }
      base.Render(timePassed);
    }

    private void RenderHorizontal(float timePassed)
    {
      int startx = _positionX;
      int imagesToDraw = _width / _textureWidth; // in case no fixed value exists => calculate it
      if (_imagesToDraw > -1)
      {
        imagesToDraw = _imagesToDraw; // we have a fixed value
      }

      for (int i = 0; i < imagesToDraw; ++i)
      {
        int texture = 0;
        int currentPercent = ((i + 1) * 100) / imagesToDraw;
        if (_alignment == Alignment.ALIGN_RIGHT)
        {
          currentPercent = ((imagesToDraw - i) * 100) / (imagesToDraw);
        }
        if (currentPercent <= Percentage)
        {
          int textureCount = _itemList.Count - 1;
          float fcurrentPercent = currentPercent;
          fcurrentPercent /= 100f;
          fcurrentPercent *= ((float)textureCount);
          texture = (int)fcurrentPercent;

          //if (_alignment==Alignment.ALIGN_RIGHT)
          //	texture=textureCount-texture;
          if (texture < 1)
          {
            texture = 1;
          }
          if (texture >= _itemList.Count)
          {
            texture = _itemList.Count - 1;
          }
        }

        if (texture < 0 || texture >= _itemList.Count || _itemList.Count <= 0) return;

        GUIImage img = (GUIImage)_itemList[texture];
        img.SetPosition(startx, _positionY);
        img.Render(timePassed);
        if (_alignment == Alignment.ALIGN_LEFT)
        {
          startx += _textureWidth;
        }
        else
        {
          startx -= _textureWidth;
        }
      }
    }

    private void RenderVertical(float timePassed) {}

    public int Percentage
    {
      get { return m_iPercentage; }
      set { m_iPercentage = value; }
    }

    public override int DimColor
    {
      get { return base.DimColor; }
      set
      {
        base.DimColor = value;
        foreach (GUIImage img in _itemList)
        {
          img.DimColor = value;
        }
      }
    }
  }
}