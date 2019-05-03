#region Copyright (C) 2005-2010 Team MediaPortal

// Copyright (C) 2005-2010 Team MediaPortal
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
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;

namespace TvPlugin.teletext
{
  /// <summary>
  /// Teletext window of TVE3
  /// </summary>
  public class TVTeletext : TvTeletextBase
  {
    #region gui components

    [SkinControl(502)] protected GUIButtonControl btnPage100 = null;
    [SkinControl(503)] protected GUIButtonControl btnPage200 = null;
    [SkinControl(504)] protected GUIButtonControl btnPage300 = null;
    [SkinControl(505)] protected GUICheckButton btnHidden = null;
    [SkinControl(506)] protected GUISelectButtonControl btnSubPage = null;
    [SkinControl(507)] protected GUIButtonControl btnFullscreen = null;

    #endregion

    #region ctor

    public TVTeletext()
    {
      GetID = (int)Window.WINDOW_TELETEXT;
    }

    #endregion

    #region GUIWindow initializing methods

    public override bool Init()
    {
      return Load(GUIGraphicsContext.GetThemedSkinFile(@"\myteletext.xml"));
    }

    protected override void OnPageDestroy(int newWindowId)
    {
      // Save the settings and then stop the update thread. Also the teletext grabbing
      SaveSettings();
      Join();
      TVHome.Card.GrabTeletext = false;
      base.OnPageDestroy(newWindowId);
    }

    protected override void OnPageLoad()
    {
      base.OnPageLoad();
      // Deactivate fullscreen video and initialize the window
      GUIGraphicsContext.IsFullScreenVideo = false;
      btnSubPage.RestoreSelection = false;
      InitializeWindow(false);
      btnHidden.Selected = _hiddenMode;
    }

    #endregion

    #region OnAction

    public override void OnAction(Action action)
    {
      base.OnAction(action);
      switch (action.wID)
      {
        case Action.ActionType.ACTION_SWITCH_TELETEXT_HIDDEN:
          //Change Hidden Mode
          if (btnHidden != null)
          {
            btnHidden.Selected = _hiddenMode;
          }
          break;
        case Action.ActionType.ACTION_REMOTE_SUBPAGE_UP:
        case Action.ActionType.ACTION_REMOTE_SUBPAGE_DOWN:
          // Subpage up
          if (btnSubPage != null)
          {
            btnSubPage.SelectedItem = Decimal(currentSubPageNumber + 1) - 1;
          }
          break;
      }
    }

    #endregion

    #region OnClicked

    protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
    {
      // Handle the click events, of this window
      if (control == btnPage100)
      {
        currentPageNumber = 0x100;
        currentSubPageNumber = 0;
        RequestUpdate();
        _renderer.PageSelectText = Convert.ToString(currentPageNumber, 16);
      }
      if (control == btnPage200)
      {
        currentPageNumber = 0x200;
        currentSubPageNumber = 0;
        RequestUpdate();
        _renderer.PageSelectText = Convert.ToString(currentPageNumber, 16);
      }
      if (control == btnPage300)
      {
        currentPageNumber = 0x300;
        currentSubPageNumber = 0;
        RequestUpdate();
        _renderer.PageSelectText = Convert.ToString(currentPageNumber, 16);
      }
      if (control == btnHidden)
      {
        if (btnHidden != null)
        {
          _hiddenMode = btnHidden.Selected;
          _renderer.HiddenMode = btnHidden.Selected;
          RequestUpdate(false);
        }
      }
      if (control == btnSubPage)
      {
        if (btnSubPage != null)
        {
          currentSubPageNumber = BCD(btnSubPage.SelectedItem + 1) - 1;
          RequestUpdate(false);
        }
      }
      if (control == btnFullscreen)
      {
        // First the fullscreen video and then switch to the new window. Otherwise the teletext isn't displayed
        GUIGraphicsContext.IsFullScreenVideo = true;
        GUIWindowManager.ActivateWindow((int)Window.WINDOW_FULLSCREEN_TELETEXT);
      }
      base.OnClicked(controlId, control, actionType);
    }

    #endregion

    public override void Render(float timePassed)
    {
      // Render GUI
      base.Render(timePassed);

      if (!_redrawForeground)
      {
        imgTeletextForeground.Render(timePassed);
      }
      else
      {
        imgTeletextBackground.Render(timePassed);
      }
    }
  }
}

// namespace