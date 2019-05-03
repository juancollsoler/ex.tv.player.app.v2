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

namespace MediaPortal.DeployTool.Sections
{
  public partial class DownloadOnlyDlg : DeployDialog
  {
    private bool rbDownloadOnlyChecked;

    public DownloadOnlyDlg()
    {
      InitializeComponent();
      type = DialogType.DownloadOnly;
      labelSectionHeader.Text = "";
      bInstallNow.Image = Images.Choose_button_on;
      rbDownloadOnlyChecked = false;
      UpdateUI();
    }

    #region IDeployDialog interface

    public override void UpdateUI()
    {
      labelSectionHeader.Text = Localizer.GetBestTranslation("DownloadOnly_labelSectionHeader");
      rbDownloadOnly.Text = Localizer.GetBestTranslation("DownloadOnly_no");
      rbInstallNow.Text = Localizer.GetBestTranslation("DownloadOnly_yes");
    }

    public override DeployDialog GetNextDialog()
    {
      if (rbDownloadOnlyChecked)
      {
        // Download only
        return DialogFlowHandler.Instance.GetDialogInstance(DialogType.DownloadSettings);
      }
      CheckResult resultTvServer = Utils.CheckNSISUninstallString("MediaPortal TV Server", "MementoSection_SecServer");
      CheckResult resultTvClient = Utils.CheckNSISUninstallString("Mediaportal Tv Server", "MementoSection_SecClient");
      // "NoRepair" key is not the best choice but we need a key returing 1 as value ;)
      CheckResult resultMp = Utils.CheckNSISUninstallString("MediaPortal", "NoRepair");

      bool TvServer = resultTvServer.state != CheckState.NOT_INSTALLED;
      bool TvClient = resultTvClient.state != CheckState.NOT_INSTALLED;
      bool Mp = resultMp.state != CheckState.NOT_INSTALLED;

      if (TvServer || TvClient || Mp)
      {
        // Upgrade 
        return DialogFlowHandler.Instance.GetDialogInstance(DialogType.Upgrade);
      }
      // Normal deploy
      return DialogFlowHandler.Instance.GetDialogInstance(DialogType.WatchTV);
    }

    public override bool SettingsValid()
    {
      return true;
    }

    #endregion

    private void bInstallNow_Click(object sender, EventArgs e)
    {
      bInstallNow.Image = Images.Choose_button_on;
      bDownloadOnly.Image = Images.Choose_button_off;
      rbDownloadOnlyChecked = false;
    }

    private void bDownloadOnly_Click(object sender, EventArgs e)
    {
      bInstallNow.Image = Images.Choose_button_off;
      bDownloadOnly.Image = Images.Choose_button_on;
      rbDownloadOnlyChecked = true;
    }
  }
}