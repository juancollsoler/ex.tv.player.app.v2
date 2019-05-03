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

using System.Drawing;
using MediaPortal.ExtensionMethods;

namespace MediaPortal.GUI.Library
{
  /// <summary>
  /// This class will draw a placeholder for the current video window
  /// if no video is playing it will present an empty rectangle
  /// </summary>
  public class GUIVideoControl : GUIControl
  {
    [XMLSkinElement("textureFocus")] protected string _focusedTextureName = "";
    [XMLSkinElement("action")] protected int _actionId = -1;

    protected GUIImage blackImage;
    protected GUIImage thumbImage;
    protected GUIAnimation _imageFocusRectangle = null;
    protected Rectangle[] _videoWindows = new Rectangle[1];
    protected bool _setVideoWindow = true;


    public GUIVideoControl(int dwParentID) : base(dwParentID) {}

    public GUIVideoControl(int dwParentID, int dwControlId, int dwPosX, int dwPosY, int dwWidth, int dwHeight,
                           string texturename)
      : base(dwParentID, dwControlId, dwPosX, dwPosY, dwWidth, dwHeight)
    {
      _focusedTextureName = texturename;
      FinalizeConstruction();
    }

    public bool SetVideoWindow
    {
      get { return _setVideoWindow; }
      set { _setVideoWindow = value; }
    }

    public override void FinalizeConstruction()
    {
      base.FinalizeConstruction();
      _imageFocusRectangle = LoadAnimationControl(_parentControlId, _controlId, _positionX, _positionY, _width, _height,
                                                  _focusedTextureName);
      thumbImage = new GUIImage(_parentControlId, _controlId, _positionX, _positionY, _width, _height,
                                "#Play.Current.Thumb", 1);
      blackImage = new GUIImage(_parentControlId, _controlId, _positionX, _positionY, _width, _height, "black.png", 1);

      _imageFocusRectangle.ParentControl = this;
      thumbImage.ParentControl = this;
      blackImage.ParentControl = this;
    }


    public override void AllocResources()
    {
      base.AllocResources();
      _imageFocusRectangle.AllocResources();
      thumbImage.AllocResources();
      blackImage.AllocResources();
    }

    public override void Dispose()
    {
      base.Dispose();
      _imageFocusRectangle.SafeDispose();
      thumbImage.SafeDispose();
      blackImage.SafeDispose();
    }

    public override void OnDeInit()
    {
      // Here is the value that need to be different from need to find to be able to switch between fullscreen to windowed video when overlay available
      GUIGraphicsContext.VideoWindow = new Rectangle(0, 0, 0, 0);
      // For madVR need to set it to hide video window when skin part don't allow displaying it.
      if (GUIGraphicsContext.VideoRenderer == GUIGraphicsContext.VideoRendererType.madVR)
      {
        GUIGraphicsContext.IsWindowVisible = true;
        GUIGraphicsContext.VideoControl = false;
      }
      base.OnDeInit();
    }

    public override void OnInit()
    {
      base.OnInit();

      float x = base.XPosition;
      float y = base.YPosition;
      GUIGraphicsContext.Correct(ref x, ref y);

      _videoWindows[0].X = (int)x;
      _videoWindows[0].Y = (int)y;
      _videoWindows[0].Width = base.Width;
      _videoWindows[0].Height = base.Height;

      if (_setVideoWindow)
      {
        //Log.Error("GUIVideoControl OnInit() {0}", _videoWindows[0]);
        GUIGraphicsContext.VideoWindow = _videoWindows[0];
        GUIGraphicsContext.VideoControl = true;
      }
    }


    public override bool CanFocus()
    {
      if (_imageFocusRectangle.FileName == string.Empty)
      {
        return false;
      }
      return true;
    }


    public override void Render(float timePassed)
    {
      if (GUIGraphicsContext.EditMode == false)
      {
        if (!IsVisible)
        {
          if (_setVideoWindow)
          {
            // Here is to hide video window madVR when skin didn't handle video overlay (the value need to be different from Process() player like VMR7)
            GUIGraphicsContext.VideoWindow = new Rectangle(0, 0, 0, 0);
          }
          base.Render(timePassed);
          return;
        }
      }

      if (!GUIGraphicsContext.Calibrating)
      {
        float x = base.XPosition;
        float y = base.YPosition;
        GUIGraphicsContext.Correct(ref x, ref y);

        _videoWindows[0].X = (int)x;
        _videoWindows[0].Y = (int)y;
        _videoWindows[0].Width = base.Width;
        _videoWindows[0].Height = base.Height;

        if (_setVideoWindow)
        {
          GUIGraphicsContext.VideoWindow = _videoWindows[0];
        }

        if (GUIGraphicsContext.ShowBackground)
        {
          if (Focus)
          {
            x = base.XPosition;
            y = base.YPosition;
            int xoff = 5;
            int yoff = 5;
            int w = 10;
            int h = 10;
            GUIGraphicsContext.ScalePosToScreenResolution(ref xoff, ref yoff);
            GUIGraphicsContext.ScalePosToScreenResolution(ref w, ref h);
            _imageFocusRectangle.SetPosition((int)x - xoff, (int)y - yoff);
            _imageFocusRectangle.Width = base.Width + w;
            _imageFocusRectangle.Height = base.Height + h;
            _imageFocusRectangle.Render(timePassed);
          }

          if (GUIGraphicsContext.graphics != null)
          {
            GUIGraphicsContext.graphics.FillRectangle(Brushes.Black, _videoWindows[0].X, _videoWindows[0].Y, base.Width,
                                                      base.Height);
          }
          else
          {
            //image.SetPosition(_videoWindows[0].X,_videoWindows[0].Y);
            //image.Width=_videoWindows[0].Width;
            //image.Height=_videoWindows[0].Height;
            if (GUIGraphicsContext.VideoWindow.Width < 1)
            {
              thumbImage.Render(timePassed);
            }
            else
            {
              blackImage.Render(timePassed); // causes flickering in fullscreen
            }
          }
        }
        else
        {
          if (GUIGraphicsContext.graphics != null)
          {
            GUIGraphicsContext.graphics.FillRectangle(Brushes.Black, _videoWindows[0].X, _videoWindows[0].Y, base.Width,
                                                      base.Height);
          }
        }
      }
      base.Render(timePassed);
    }

    public override void OnAction(Action action)
    {
      base.OnAction(action);
      GUIMessage message;
      if (Focus)
      {
        if (action.wID == Action.ActionType.ACTION_MOUSE_CLICK || action.wID == Action.ActionType.ACTION_SELECT_ITEM)
        {
          // If this button corresponds to an action generate that action.
          if (ActionID >= 0)
          {
            Action newaction = new Action((Action.ActionType)ActionID, 0, 0);
            GUIGraphicsContext.OnAction(newaction);
            return;
          }

          message = new GUIMessage(GUIMessage.MessageType.GUI_MSG_CLICKED, WindowId, GetID, ParentID, 0, 0, null);
          GUIGraphicsContext.SendMessage(message);
        }
      }
    }

    /// <summary>
    /// Get/set the action ID that corresponds to this button.
    /// </summary>
    public int ActionID
    {
      get { return _actionId; }
      set { _actionId = value; }
    }

    /// <summary>
    /// Checks if the x and y coordinates correspond to the current control.
    /// </summary>
    /// <param name="x">The x coordinate.</param>
    /// <param name="y">The y coordinate.</param>
    /// <returns>True if the control was hit.</returns>
    public override bool HitTest(int x, int y, out int controlID, out bool focused)
    {
      focused = Focus;
      controlID = GetID;
      if (!IsVisible || Disabled || CanFocus() == false)
      {
        return false;
      }
      if (InControl(x, y, out controlID))
      {
        if (CanFocus())
        {
          return true;
        }
      }
      focused = Focus = false;
      return false;
    }
  }
}