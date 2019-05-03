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
using System.Windows.Forms;
using System.Diagnostics;
using System.IO;

namespace MediaPortal.DeployTool
{
  public partial class ManualDownload : Form
  {
    private string _url;

    private void UpdateUI()
    {
      Text = Localizer.GetBestTranslation("ManualDownload_Title");
      labelHeading.Text = Localizer.GetBestTranslation("ManualDownload_labelHeading");
      linkURL.Text = labelTargetFile.Text = Localizer.GetBestTranslation("ManualDownload_linkURL");
      linkDir.Text = Localizer.GetBestTranslation("ManualDownload_linkDir");
      labelDesc.Text = Localizer.GetBestTranslation("ManualDownload_labelDesc");
      buttonContinue.Text = Localizer.GetBestTranslation("ManualDownload_buttonContinue");
    }

    public ManualDownload()
    {
      InitializeComponent();
      Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
      UpdateUI();
    }

    public DialogResult ShowDialog(string url, string targetFile, string targetDir)
    {
      _url = url;
      labelTargetFile.Text = targetFile;
      labelTargetDir.Text = targetDir;
      return ShowDialog();
    }

    private void linkURL_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        Process.Start(_url);
      }
      catch (Exception) {}
    }

    private void linkDir_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
    {
      try
      {
        Process.Start(labelTargetDir.Text);
      }
      catch (Exception) {}
    }

    private void buttonContinue_Click(object sender, EventArgs e)
    {
      string target_dir = labelTargetDir.Text;
      string target_file = labelTargetFile.Text;
      if (!File.Exists(target_dir + "\\" + target_file))
      {
        Dispose();
        ManualDownloadFileMissing FileFind = new ManualDownloadFileMissing();
        FileFind.ShowDialog(target_dir, target_file);
      }
      DialogResult = DialogResult.OK;
    }
  }
}