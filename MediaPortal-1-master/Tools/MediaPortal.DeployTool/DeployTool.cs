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
using System.IO;
using System.Diagnostics;

namespace MediaPortal.DeployTool
{
  public partial class DeployTool : Form, IDeployDialog
  {
    private DeployDialog _currentDialog;
    public string _currentCulture = "en-US";
    public bool _restart;

    #region IDeployDialog interface

    public void UpdateUI()
    {
      Text = Localizer.GetBestTranslation("MainWindow_AppName");
      backButton.Text = Localizer.GetBestTranslation("MainWindow_backButton");
      nextButton.Text = Localizer.GetBestTranslation("MainWindow_nextButton");
    }

    public DeployDialog GetNextDialog()
    {
      return null;
    }

    public bool SettingsValid()
    {
      return true;
    }

    public void SetProperties()
    {
      return;
    }

    public DeployDialog GetCurrentDialog()
    {
      return _currentDialog;
    }

    #endregion

    public DeployTool()
    {
      //Create necessary directory tree
      if (!Directory.Exists(Application.StartupPath + "\\deploy"))
        Directory.CreateDirectory(Application.StartupPath + "\\deploy");

      //Set default folders
      InstallationProperties.Instance.Set("MPDir",
                                          Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) +
                                          "\\Team MediaPortal\\MediaPortal");
      InstallationProperties.Instance.Set("TVServerDir",
                                          Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles) +
                                          "\\Team MediaPortal\\MediaPortal TV Server");

      string tmpPrg = Environment.GetEnvironmentVariable("ProgramW6432");
      if (!string.IsNullOrEmpty(tmpPrg))
      {
        InstallationProperties.Instance.Set("ProgramFiles", tmpPrg);
      }
      else
      {
        InstallationProperties.Instance.Set("ProgramFiles", Environment.GetEnvironmentVariable("ProgramFiles"));
      }

      // Paint first screen
      InitializeComponent();
      Icon = System.Drawing.Icon.ExtractAssociatedIcon(Application.ExecutablePath);
      Localizer.SwitchCulture("en-US");
      UpdateUI();

      // Delete Run registry key
      DialogType firstDlg;
      if (Utils.AutoRunApplication("delete"))
      {
        firstDlg = DialogType.Installation;
        InstallationProperties.Instance.Load();
        Localizer.SwitchCulture(InstallationProperties.Instance["language"]);
        _restart = true;
      }
      else
      {
        firstDlg = DialogType.Welcome;
        Localizer.SwitchCulture("en-US");
        _restart = false;
      }
      _currentDialog = DialogFlowHandler.Instance.GetDialogInstance(firstDlg);
      splitContainer2.Panel1.Controls.Add(_currentDialog);
      InstallationProperties.Instance.Set("InstallTypeHeader", "Choose installation type");
      backButton.Visible = false;
      UpdateUI();
      if (_restart)
      {
        nextButton.Text = Localizer.GetBestTranslation("Install_buttonInstall");
      }
      nextButton.Focus();
    }

    private void SwitchDialog(DeployDialog dlg)
    {
      splitContainer2.Panel1.Controls.Clear();
      splitContainer2.Panel1.Controls.Add(dlg);
      dlg.Focus();
      nextButton.Focus();
    }

    private void nextButton_Click(object sender, EventArgs e)
    {
      //
      // check Internet connection unless files have already been downloaded
      //
      if (_currentDialog.type == DialogType.DownloadOnly &&
          Directory.GetFiles(Application.StartupPath + "\\deploy").Length < 3)
      {
        if (!InstallationChecks.InternetChecker.CheckConnection())
        {
          MessageBox.Show(Localizer.GetBestTranslation("DownloadOnly_NoConnectionWarning"), "MediaPortal",
                          MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
      }

      // 
      // check if there's is sufficient hard disk space for installation
      // 
      if (_currentDialog.type == DialogType.DBMSSettings || _currentDialog.type == DialogType.TvServerSettings ||
          _currentDialog.type == DialogType.MPSettings)
      {
        // at least 0.5 GB free disk space are required for installation
        const double requiredDiskSpace = 0.5;
        double actualDiskSpace =
          InstallationChecks.DiskSpaceChecker.GetRemainingHardDiskCapacity(_currentDialog.installationPath);

        if (actualDiskSpace < requiredDiskSpace)
        {
          MessageBox.Show(string.Format(Localizer.GetBestTranslation("DiskSpace_Error"), requiredDiskSpace * 1000),
                          "MediaPortal", MessageBoxButtons.OK, MessageBoxIcon.Error);
          return;
        }
      }

      if (nextButton.Text == Localizer.GetBestTranslation("MainWindow_buttonClose"))
      {
        //
        // If in download_only mode, start explorer to show downloaded stuff
        //
        if (InstallationProperties.Instance["InstallType"] == "download_only")
        {
          try
          {
            Process process = new Process();
            process.StartInfo.FileName = "explorer.exe";
            process.StartInfo.Arguments = "/e, " + Application.StartupPath + "\\deploy";
            process.StartInfo.UseShellExecute = true;
            process.Start();
          }
            // Starting processes might fail - prefer a not opening Explorer instead of a big crash window...
          catch (Exception)
          {
          }
        }
        Close();
        return;
      }
      if (nextButton.Text == Localizer.GetBestTranslation("Install_buttonDownload") ||
          nextButton.Text == Localizer.GetBestTranslation("Install_buttonInstall"))
      {
        nextButton.Enabled = false;
      }
      if (!_currentDialog.SettingsValid())
        return;
      _currentDialog.SetProperties();
      if (InstallationProperties.Instance["language"] != _currentCulture)
      {
        _currentCulture = InstallationProperties.Instance["language"];
        Localizer.SwitchCulture(_currentCulture);
        UpdateUI();
      }
      _currentDialog = _currentDialog.GetNextDialog();
      SwitchDialog(_currentDialog);
      if (!backButton.Visible)
        backButton.Visible = true;
      if (InstallationProperties.Instance["finished"] == "yes")
      {
        backButton.Visible = false;
        nextButton.Enabled = true;
        nextButton.Text = Localizer.GetBestTranslation("MainWindow_buttonClose");
      }
      if (!_restart && InstallationProperties.Instance["Install_Dialog"] == "yes")
      {
        nextButton.Text = InstallationProperties.Instance["InstallType"] == "download_only"
                            ? Localizer.GetBestTranslation("Install_buttonDownload")
                            : Localizer.GetBestTranslation("Install_buttonInstall");
        InstallationProperties.Instance.Set("Install_Dialog", "no");
      }
    }

    private void backButton_Click(object sender, EventArgs e)
    {
      bool isFirstDlg = false;
      _currentDialog = DialogFlowHandler.Instance.GetPreviousDlg(ref isFirstDlg);
      if (isFirstDlg)
        backButton.Visible = false;
      nextButton.Text = Localizer.GetBestTranslation("MainWindow_nextButton");
      SwitchDialog(_currentDialog);
    }


    private void bExit_Click(object sender, EventArgs e)
    {
      string message = Localizer.GetBestTranslation("Exit_Installation");
      const string caption = "MediaPortal";
      const MessageBoxButtons buttons = MessageBoxButtons.YesNo;

      DialogResult result = MessageBox.Show(this, message, caption, buttons);

      if (result == DialogResult.Yes)
      {
        Application.Exit();
      }
    }

    private void bHelp_Click(object sender, EventArgs e)
    {
      try
      {
        Process.Start(Application.StartupPath + "\\HelpContent\\DeployToolGuide\\DeployToolGuide.htm");
      }
      catch (Exception ex)
      {
        MessageBox.Show(String.Format("Unable to open the help page with your default browser - {0}", ex.Message),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
      }
    }
  }
}