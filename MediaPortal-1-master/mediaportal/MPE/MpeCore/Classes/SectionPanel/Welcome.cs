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
using System.IO;
using MpeCore.Interfaces;

namespace MpeCore.Classes.SectionPanel
{
  public partial class Welcome : BaseVerticalLayout, ISectionPanel
  {
    private const string CONST_TEXT1 = "Header text";
    private const string CONST_TEXT2 = "Description";
    private const string CONST_IMAGE = "Left part image";

    private SectionItem Section = new SectionItem();
    private PackageClass _packageClass = new PackageClass();
    private SectionResponseEnum resp = SectionResponseEnum.Cancel;

    #region ISectionPanel Members

    public string DisplayName
    {
      get { return "Welcome Screen"; }
    }

    public string Guid
    {
      get { return "{DF252CB6-872D-4f61-ABC7-729B12C8C686}"; }
    }
    
    public SectionParamCollection GetDefaultParams()
    {
      SectionParamCollection param = new SectionParamCollection();
      param.Add(new SectionParam(CONST_TEXT1, "Welcome to the Extension Installer for [Name]", ValueTypeEnum.String, ""));
      param.Add(new SectionParam(CONST_TEXT2, "This will install [Name] version [Version] on your computer.\n" +
                                              "It is recommended that you close all other applications before continuing.\n" +
                                              "Click Next to continue or Cancel to exit Setup.", ValueTypeEnum.String,
                                 ""));
      param.Add(new SectionParam(CONST_IMAGE, "", ValueTypeEnum.File, ""));
      param.Add(new SectionParam(ParamNamesConst.SECTION_ICON, "", ValueTypeEnum.File,
                                 "Image in upper right part"));
      return param;
    }

    public void Preview(PackageClass packageClass, SectionItem sectionItem)
    {
      Section = sectionItem;
      _packageClass = packageClass;
      SetValues();
      ShowDialog();
    }

    public SectionResponseEnum Execute(PackageClass packageClass, SectionItem sectionItem)
    {
      Section = sectionItem;
      _packageClass = packageClass;
      SetValues();
      Base.ActionExecute(_packageClass, Section, ActionExecuteLocationEnum.BeforPanelShow);
      Base.ActionExecute(_packageClass, Section, ActionExecuteLocationEnum.AfterPanelShow);
      if (!packageClass.Silent)
        ShowDialog();
      else
        resp = SectionResponseEnum.Next;
      Base.ActionExecute(_packageClass, Section, ActionExecuteLocationEnum.AfterPanelHide);

      return resp;
    }

    #endregion

    public Welcome()
    {
      InitializeComponent();
    }

    private void SetValues()
    {
      lbl_desc1.Text = _packageClass.ReplaceInfo(Section.Params[CONST_TEXT1].Value);
      lbl_desc2.Text = _packageClass.ReplaceInfo(Section.Params[CONST_TEXT2].Value);
      if (File.Exists(Section.Params[CONST_IMAGE].Value))
      {
        base.pictureBox1.Load(Section.Params[CONST_IMAGE].Value);
      }
      button_next.Text = "Next >";
      switch (Section.WizardButtonsEnum)
      {
        case WizardButtonsEnum.BackNextCancel:
          button_next.Visible = true;
          button_cancel.Visible = true;
          button_back.Visible = true;
          break;
        case WizardButtonsEnum.NextCancel:
          button_next.Visible = true;
          button_cancel.Visible = true;
          button_back.Visible = false;
          break;
        case WizardButtonsEnum.BackFinish:
          button_next.Visible = true;
          button_cancel.Visible = false;
          button_back.Visible = true;
          button_next.Text = "Finish";
          break;
        case WizardButtonsEnum.Cancel:
          button_next.Visible = false;
          button_cancel.Visible = true;
          button_back.Visible = false;
          break;
        case WizardButtonsEnum.Next:
          button_next.Visible = true;
          button_cancel.Visible = false;
          button_back.Visible = false;
          break;
        case WizardButtonsEnum.Finish:
          button_next.Visible = true;
          button_cancel.Visible = false;
          button_back.Visible = false;
          button_next.Text = "Finish";
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
    }

    private void Welcome_Load(object sender, EventArgs e)
    {
      this.BringToFront();
    }

    private void button_back_Click(object sender, EventArgs e)
    {
      resp = SectionResponseEnum.Back;
      this.Close();
    }

    private void button_next_Click(object sender, EventArgs e)
    {
      resp = SectionResponseEnum.Next;
      this.Close();
    }

    private void button_cancel_Click(object sender, EventArgs e)
    {
      resp = SectionResponseEnum.Cancel;
      this.Close();
    }
  }
}