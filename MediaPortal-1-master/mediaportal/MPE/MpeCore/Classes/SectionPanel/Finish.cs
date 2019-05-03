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
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using MpeCore.Interfaces;

namespace MpeCore.Classes.SectionPanel
{
  public partial class Finish : BaseVerticalLayout, ISectionPanel
  {
    private const string CONST_TEXT1 = "Header text";
    private const string CONST_IMAGE = "Left part image";
    private const string CONST_RADIO = "Show radio buttons";

    private SectionItem Section = new SectionItem();
    private PackageClass _packageClass = new PackageClass();
    private List<CheckBox> CheckBoxs = new List<CheckBox>();
    private List<RadioButton> RadioButtons = new List<RadioButton>();
    private ShowModeEnum Mode = ShowModeEnum.Preview;
    private SectionResponseEnum resp = SectionResponseEnum.Cancel;

    #region ISectionPanel Members

    public string DisplayName
    {
      get { return "Setup Complete"; }
    }

    public string Guid
    {
      get { return "{BB49DFA5-04AB-45d1-8CEB-92C4544615E0}"; }
    }

    public SectionParamCollection GetDefaultParams()
    {
      SectionParamCollection param = new SectionParamCollection();
      param.Add(new SectionParam(CONST_TEXT1, "The Extension Installer Wizard has successfully installed [Name].",
                                 ValueTypeEnum.String, ""));
      param.Add(new SectionParam(CONST_IMAGE, "", ValueTypeEnum.File, ""));
      param.Add(new SectionParam(CONST_RADIO, "", ValueTypeEnum.Bool, "Use radiobutton in place of combobox"));
      param.Add(new SectionParam(ParamNamesConst.SECTION_ICON, "", ValueTypeEnum.File,
                                 "Image in upper right part"));
      return param;
    }

    public void Preview(PackageClass packageClass, SectionItem sectionItem)
    {
      Section = sectionItem;
      _packageClass = packageClass;
      Mode = ShowModeEnum.Preview;
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
      Mode = ShowModeEnum.Real;
      if (!packageClass.Silent)
        ShowDialog();
      else
        resp = SectionResponseEnum.Next;
      Base.ActionExecute(_packageClass, Section, ActionExecuteLocationEnum.AfterPanelHide);
      return resp;
    }

    #endregion

    public Finish()
    {
      InitializeComponent();
      CheckBoxs.Add(checkBox1);
      CheckBoxs.Add(checkBox2);
      CheckBoxs.Add(checkBox3);
      CheckBoxs.Add(checkBox4);
      CheckBoxs.Add(checkBox5);
      CheckBoxs.Add(checkBox6);
      CheckBoxs.Add(checkBox7);

      RadioButtons.Add(radioButton1);
      RadioButtons.Add(radioButton2);
      RadioButtons.Add(radioButton3);
      RadioButtons.Add(radioButton4);
      RadioButtons.Add(radioButton5);
      RadioButtons.Add(radioButton6);
      RadioButtons.Add(radioButton7);
    }

    private void SetValues()
    {
      lbl_desc1.Text = _packageClass.ReplaceInfo(Section.Params[CONST_TEXT1].Value);
      if (File.Exists(Section.Params[CONST_IMAGE].Value))
      {
        base.pictureBox1.Load(Section.Params[CONST_IMAGE].Value);
      }
      int i = 0;
      foreach (CheckBox checkBox in CheckBoxs)
      {
        checkBox.Visible = false;
        RadioButtons[i].Location = checkBox.Location;
        RadioButtons[i].Visible = false;
        i++;
      }
      i = 0;
      foreach (var includedGroup in Section.IncludedGroups)
      {
        if (Section.Params[CONST_RADIO].GetValueAsBool())
        {
          RadioButtons[i].Visible = true;
          RadioButtons[i].Text = _packageClass.Groups[includedGroup].DisplayName;
          RadioButtons[i].Checked = _packageClass.Groups[includedGroup].Checked;
          RadioButtons[i].Tag = _packageClass.Groups[includedGroup];
          this.toolTip1.SetToolTip(RadioButtons[i], _packageClass.Groups[includedGroup].Description);
        }
        else
        {
          CheckBoxs[i].Visible = true;
          CheckBoxs[i].Text = _packageClass.Groups[includedGroup].DisplayName;
          CheckBoxs[i].Checked = _packageClass.Groups[includedGroup].Checked;
          CheckBoxs[i].Tag = _packageClass.Groups[includedGroup];
          this.toolTip1.SetToolTip(CheckBoxs[i], _packageClass.Groups[includedGroup].Description);
        }
        i++;
        if (i > 6)
          break;
      }
      button_next.Text = "Next >";
      switch (Section.WizardButtonsEnum)
      {
        case WizardButtonsEnum.BackNextCancel:
          button_back.Enabled = true;
          button_next.Enabled = true;
          button_cancel.Enabled = true;
          button_next.Visible = true;
          button_cancel.Visible = true;
          button_back.Visible = true;
          break;
        case WizardButtonsEnum.NextCancel:
          button_next.Enabled = true;
          button_cancel.Enabled = true;
          button_back.Enabled = false;
          break;
        case WizardButtonsEnum.BackFinish:
          button_next.Enabled = true;
          button_cancel.Enabled = false;
          button_back.Enabled = true;
          button_next.Text = "Finish";
          break;
        case WizardButtonsEnum.Cancel:
          button_next.Enabled = false;
          button_cancel.Enabled = true;
          button_back.Enabled = false;
          break;
        case WizardButtonsEnum.Next:
          button_next.Enabled = true;
          button_cancel.Enabled = false;
          button_back.Enabled = false;
          break;
        case WizardButtonsEnum.Finish:
          button_next.Enabled = true;
          button_cancel.Enabled = false;
          button_back.Enabled = false;
          button_next.Text = "Finish";
          break;
        default:
          throw new ArgumentOutOfRangeException();
      }
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
      Close();
    }

    private void checkBox_CheckedChanged(object sender, EventArgs e)
    {
      if (Mode == ShowModeEnum.Preview)
        return;
      CheckBox box = (CheckBox)sender;
      GroupItem item = box.Tag as GroupItem;
      item.Checked = box.Checked;
    }

    private void radioButton_CheckedChanged(object sender, EventArgs e)
    {
      if (Mode == ShowModeEnum.Preview)
        return;
      RadioButton box = (RadioButton)sender;
      GroupItem item = box.Tag as GroupItem;
      item.Checked = box.Checked;
    }
  }
}