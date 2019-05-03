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
using System.Windows.Forms;

namespace MediaPortal.DeployTool.Sections
{
  public partial class FinishedDlg : DeployDialog
  {
    public FinishedDlg()
    {
      InitializeComponent();
      type = DialogType.Finished;
      UpdateUI();
    }

    #region IDeployDialog interface

    public override void UpdateUI()
    {
      labelHeading1.Text = Localizer.GetBestTranslation("Finished_labelHeading1");
      if (InstallationProperties.Instance["InstallType"] == "download_only")
      {
        labelHeading2.Text = Localizer.GetBestTranslation("Finished_labelHeading2_download");
        labelHeading3.Text = Localizer.GetBestTranslation("Finished_labelHeading3_download");
        labelHeading3.Location = new Point(labelHeading3.Location.X, 79);
      }
      else
      {
        labelHeading2.Text = Localizer.GetBestTranslation("Finished_labelHeading2_install");
        labelHeading3.Text = Localizer.GetBestTranslation("Finished_labelHeading3_install");
      }
      linkHomepage.Text = Localizer.GetBestTranslation("Finished_linkHomepage");
      linkForum.Text = Localizer.GetBestTranslation("Finished_linkForum");
      linkWiki.Text = Localizer.GetBestTranslation("Finished_linkWiki");
      labelEbay.Text = Localizer.GetBestTranslation("Finished_labelEbay");
      labelSectionHeader.Text = "";
    }

    public override DeployDialog GetNextDialog()
    {
      return null;
    }

    public override bool SettingsValid()
    {
      return false;
    }

    public override void SetProperties() {}

    #endregion

    #region Hyperlink handler

    private static void OpenURL(string url)
    {
      try
      {
        System.Diagnostics.Process.Start(url);
      }
      catch (System.Exception) {}
    }

    private void linkHomepage_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      OpenURL("http://www.team-mediaportal.com");
    }

    private void linkForum_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      OpenURL("http://forum.team-mediaportal.com");
    }

    private void linkWiki_LinkClicked_1(object sender, LinkLabelLinkClickedEventArgs e)
    {
      OpenURL("http://wiki.team-mediaportal.com/TeamMediaPortal/MP1QuickSetupGuide/");
    }

    #endregion
  }
}