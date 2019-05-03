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
using System.IO;
using MediaPortal.GUI.Library;
using Action = MediaPortal.GUI.Library.Action;
using Alignment = MediaPortal.GUI.Library.GUIControl.Alignment;
using VAlignment = MediaPortal.GUI.Library.GUIControl.VAlignment;

namespace MediaPortal.Dialogs
{
  /// <summary>
  /// 
  /// </summary>
  public class GUIDialogNotify : GUIDialogWindow
  {
    [SkinControl(4)] protected GUIButtonControl btnClose = null;
    [SkinControl(3)] protected GUILabelControl lblHeading = null;
    [SkinControl(5)] protected GUIImage imgLogo = null;
    [SkinControl(6)] protected GUITextControl txtArea = null;

    private int timeOutInSeconds = 5;
    private DateTime timeStart = DateTime.Now;
    private bool m_bNeedRefresh = false;
    private string logoUrl = string.Empty;


    public GUIDialogNotify()
    {
      GetID = (int)Window.WINDOW_DIALOG_NOTIFY;
    }

    public override bool Init()
    {
      return Load(GUIGraphicsContext.GetThemedSkinFile(@"\DialogNotify.xml"));
    }

    #region Base Dialog Members

    public override void DoModal(int dwParentId)
    {
      timeStart = DateTime.Now;
      base.DoModal(dwParentId);
    }

    public override bool ProcessDoModal()
    {
      base.ProcessDoModal();
      TimeSpan timeElapsed = DateTime.Now - timeStart;
      if (TimeOut > 0)
      {
        if (timeElapsed.TotalSeconds >= TimeOut)
        {
          PageDestroy();
          return false;
        }
      }
      return true;
    }

    #endregion

    protected override void OnClicked(int controlId, GUIControl control, Action.ActionType actionType)
    {
      base.OnClicked(controlId, control, actionType);
      if (control == btnClose)
      {
        PageDestroy();
      }
    }

    public override bool OnMessage(GUIMessage message)
    {
      //needRefresh = true;
      switch (message.Message)
      {
        case GUIMessage.MessageType.GUI_MSG_WINDOW_DEINIT:
          {
            if (lblHeading != null) lblHeading.Label = string.Empty;
            base.OnMessage(message);
            return true;
          }

        case GUIMessage.MessageType.GUI_MSG_WINDOW_INIT:
          {
            base.OnMessage(message);
            if (imgLogo != null)
            {
              SetImage(logoUrl);
            }
          }

          return true;
      }

      return base.OnMessage(message);
    }

    public override void Reset()
    {
      timeOutInSeconds = 5;
      logoUrl = string.Empty;
    }

    public void SetHeading(string strLine)
    {
      //LoadSkin();
      AllocResources();
      InitControls();

      if (lblHeading != null) lblHeading.Label = strLine;
    }


    public void SetHeading(int iString)
    {
      SetHeading(GUILocalizeStrings.Get(iString));
    }

    public void SetText(string text)
    {
      txtArea.Label = text;
    }

    public void SetImage(string filename)
    {
      logoUrl = filename;
      if (Util.Utils.FileExistsInCache(filename))
      {
        if (imgLogo != null)
        {
          imgLogo.SetFileName(filename);
          m_bNeedRefresh = true;
          imgLogo.IsVisible = true;
        }
      }
      else
      {
        if (imgLogo != null)
        {
          imgLogo.IsVisible = false;
          m_bNeedRefresh = true;
        }
      }
    }

    public void SetImageDimensions(Size size, bool keepAspectRatio, bool centered)
    {
      if (imgLogo == null)
      {
        return;
      }
      imgLogo.Width = size.Width;
      imgLogo.Height = size.Height;
      imgLogo.KeepAspectRatio = keepAspectRatio;
      imgLogo.ImageAlignment = Alignment.ALIGN_CENTER;
      imgLogo.ImageVAlignment = VAlignment.ALIGN_MIDDLE;
    }

    public int TimeOut
    {
      get { return timeOutInSeconds; }
      set { timeOutInSeconds = value; }
    }

    public override bool NeedRefresh()
    {
      if (m_bNeedRefresh)
      {
        m_bNeedRefresh = false;
        return true;
      }
      return false;
    }
  }
}