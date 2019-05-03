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
using MpeCore.Interfaces;

namespace MpeCore.Classes.SectionPanel
{
  public partial class InstallSection : BaseHorizontalLayout, ISectionPanel
  {
    #region ISectionPanel Members

    public string DisplayName
    {
      get { return "Install Section"; }
    }

    public string Guid
    {
      get { return "{839F908C-05A5-47ac-8AD4-BE8A7BC44DAE}"; }
    }

    public SectionParamCollection GetDefaultParams()
    {
      return new SectionParamCollection(base.Params);
    }

    public void Preview(PackageClass packageClass, SectionItem sectionItem)
    {
      Mode = ShowModeEnum.Preview;
      Section = sectionItem;
      Package = packageClass;
      SetValues();
      timer1.Enabled = true;
      ShowDialog();
    }

    public SectionResponseEnum Execute(PackageClass packageClass, SectionItem sectionItem)
    {
      Mode = ShowModeEnum.Real;
      Package = packageClass;
      Section = sectionItem;
      base.DisableX_Click();
      SetValues();
      Base.ActionExecute(Package, Section, ActionExecuteLocationEnum.BeforPanelShow);
      ShowDialog();
      Base.ActionExecute(Package, Section, ActionExecuteLocationEnum.AfterPanelHide);
      return base.Resp;
    }

    #endregion

    public InstallSection()
    {
      InitializeComponent();
    }

    private void packageClass_FileInstalled(object sender, Events.InstallEventArgs e)
    {
      progressBar1.Value++;
      if (!string.IsNullOrEmpty(e.Description))
      {
        lbl_curr_file.Text = e.Description;
        lst_changes.Items.Add(e.Description);
      }
      else
      {
        lbl_curr_file.Text = e.Item.DestinationFilename;
        lst_changes.Items.Add(e.Item.ExpandedDestinationFilename);
      }
      if (lst_changes.Items.Count > 1)
        lst_changes.SetSelected(lst_changes.Items.Count - 1, true);
      RefreshControls();
    }

    private void RefreshControls()
    {
      lbl_curr_file.Refresh();
      lst_changes.Refresh();
      progressBar1.Refresh();
      Update();
      Refresh();
      Invalidate(true);
    }

    private void SetValues()
    {
      button_back.Enabled = false;
      button_cancel.Enabled = false;
    }

    private void InstallSection_Shown(object sender, EventArgs e)
    {
      this.BringToFront();
      if (Mode == ShowModeEnum.Real)
      {
        base.button_next.Enabled = false;
        base.button_back.Enabled = false;
        base.button_cancel.Enabled = false;
        foreach (var actionItem in Section.Actions.Items)
        {
          if (actionItem.ExecuteLocation != ActionExecuteLocationEnum.AfterPanelShow)
            continue;
          if (!string.IsNullOrEmpty(actionItem.ConditionGroup) && !Package.Groups[actionItem.ConditionGroup].Checked)
            continue;

          // use a new instance of the action, 
          // because if we chain MPE installations, 
          // reusing the action will result in multiple event subscription to advance the progress bar
          var action = Activator.CreateInstance(MpeInstaller.ActionProviders[actionItem.ActionType].GetType()) as IActionType;

          progressBar1.Value = progressBar1.Minimum;
          progressBar1.Maximum = action.ItemsCount(Package, actionItem);
          action.ItemProcessed += packageClass_FileInstalled;
          action.Execute(Package, actionItem);
          action.ItemProcessed -= packageClass_FileInstalled;
        }
        lbl_curr_file.Text = "Done";
        if (Package.Silent)
        {
          Resp = SectionResponseEnum.Ok;
          Close();
        }
        lst_changes.Items.Add("");
        lst_changes.Items.Add("Done");
        lst_changes.SetSelected(lst_changes.Items.Count - 1, true);
        button_next.Enabled = true;
      }
    }

    private void timer1_Tick(object sender, EventArgs e)
    {
      progressBar1.Value++;
      if (progressBar1.Value > progressBar1.Maximum - 2)
        progressBar1.Value = 0;
    }

    private void timer2_Tick(object sender, EventArgs e)
    {
      RefreshControls();
    }
  }
}