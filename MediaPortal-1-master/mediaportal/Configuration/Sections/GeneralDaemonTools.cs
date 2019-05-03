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
using System.Collections;
using System.ComponentModel;
using System.IO;
using System.Windows.Forms;
using MediaPortal.Profile;
using MediaPortal.UserInterface.Controls;
using MediaPortal.Util;
using Microsoft.Win32;

#pragma warning disable 108

namespace MediaPortal.Configuration.Sections
{
  public class GeneralDaemonTools : SectionSettings
  {
    private MPGroupBox groupBox2;
    private MPCheckBox checkBoxDaemonTools;
    private MPTextBox textBoxDaemonTools;
    private MPButton buttonSelectFolder;
    private MPLabel label1;
    private MPLabel label3;
    private MPComboBox comboBoxDrive;
    private MPLabel label4;
    private MPComboBox comboDriveNo;
    private MPCheckBox checkBoxAskBeforePlaying;
    private MPTextBox textBoxExtensions;
    private MPLabel mpLabel1;
    private MPLabel mpLabel2;
    private MPButton resetButton;
    private MPComboBox comboDriveType;
    private MPLabel mpLabel3;
    private IContainer components = null;

    public GeneralDaemonTools()
      : this("Virtual Drive") {}

    public GeneralDaemonTools(string name)
      : base(name)
    {
      // This call is required by the Windows Form Designer.
      InitializeComponent();
      comboBoxDriveCheck();
    }


    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      if (disposing)
      {
        if (components != null)
        {
          components.Dispose();
        }
      }
      base.Dispose(disposing);
    }

    /// <summary>
    /// 
    /// </summary>
    public override void LoadSettings()
    {
      if (OSInfo.OSInfo.Win8OrLater())
      {
        comboDriveType.Items.AddRange(new object[] {
            "dt",
            "scsi",
            "ide",
            "vcd",
            "native"});
      }
      else
      {
        comboDriveType.Items.AddRange(new object[] {
            "dt",
            "scsi",
            "ide",
            "vcd"});
      }
      
      using (Settings xmlreader = new MPSettings())
      {
        checkBoxDaemonTools.Checked = xmlreader.GetValueAsBool("daemon", "enabled", false);
        textBoxDaemonTools.Text = xmlreader.GetValueAsString("daemon", "path", "");
        textBoxExtensions.Text = xmlreader.GetValueAsString("daemon", "extensions", Util.Utils.ImageExtensionsDefault);
        comboDriveNo.SelectedItem = xmlreader.GetValueAsInt("daemon", "driveNo", 0).ToString();
        comboDriveType.SelectedItem = xmlreader.GetValueAsString("daemon", "driveType", "");
        checkBoxAskBeforePlaying.Checked = xmlreader.GetValueAsBool("daemon", "askbeforeplaying", false);
        // Need to detect letter to fill the correct letter.
        comboBoxDriveCheck();
        comboBoxDrive.SelectedItem = xmlreader.GetValueAsString("daemon", "drive", "E:");
      }
      checkBoxDaemonTools_CheckedChanged(null, null);
      comboDriveType_SelectionChangeCommitted(null, null);

      if (textBoxDaemonTools.Text.Length == 0)
      {
        textBoxDaemonTools.Text = GetInstalledSoftware("virtualclonedrive", true);
      }
      if (textBoxDaemonTools.Text.Length == 0)
      {
        textBoxDaemonTools.Text = GetInstalledSoftware("daemon tools", false);
      }
      if ((comboDriveType.SelectedItem == null || (string) comboDriveType.SelectedItem == string.Empty) && textBoxDaemonTools.Text.ToLowerInvariant().Contains("virtualclonedrive"))
      {
        comboDriveType.SelectedItem = "vcd";
      }
    }

    private string GetInstalledSoftware(string Search, bool searchLocalMachine)
    {
      string SoftwarePath = null;
      string SoftwareKey = @"Software\Microsoft\Windows\CurrentVersion\Run";
      RegistryKey rk;
      if (searchLocalMachine)
        rk = Registry.LocalMachine.OpenSubKey(SoftwareKey);
      else
        rk = Registry.CurrentUser.OpenSubKey(SoftwareKey);

      if (rk != null)
      {
        try
        {
          foreach (string skName in rk.GetValueNames())
          {
            if (skName.ToLowerInvariant().Contains(Search.ToLowerInvariant()))
            {
              SoftwarePath = rk.GetValue(skName).ToString().Replace("\"", "");

              //Old versions of DaemonTools and VirtualCloneDrive
              SoftwarePath = SoftwarePath.Substring(0, SoftwarePath.LastIndexOf(@"\")) + @"\daemon.exe";
              if (System.IO.File.Exists(SoftwarePath))
                break;
              //New versions of DaemonTools
              SoftwarePath = SoftwarePath.Substring(0, SoftwarePath.LastIndexOf(@"\")) + @"\DTLite.exe";
              if (System.IO.File.Exists(SoftwarePath))
                break;
              //DaemonTools Pro
              SoftwarePath = SoftwarePath.Substring(0, SoftwarePath.LastIndexOf(@"\")) + @"\DTAgent.exe";
              break;
            }
          }
          rk.Close();
        }
        catch (Exception) {}
      }
      return SoftwarePath;
    }

    public override void SaveSettings()
    {
      using (Settings xmlwriter = new MPSettings())
      {
        xmlwriter.SetValueAsBool("daemon", "enabled", checkBoxDaemonTools.Checked);
        xmlwriter.SetValue("daemon", "path", textBoxDaemonTools.Text);
        xmlwriter.SetValue("daemon", "extensions", textBoxExtensions.Text);
        xmlwriter.SetValue("daemon", "drive", (string)comboBoxDrive.SelectedItem);
        xmlwriter.SetValue("daemon", "driveNo", Int32.Parse((string)comboDriveNo.SelectedItem));
        xmlwriter.SetValue("daemon", "driveType", (string)comboDriveType.SelectedItem);
        xmlwriter.SetValueAsBool("daemon", "askbeforeplaying", checkBoxAskBeforePlaying.Checked);
      }
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.groupBox2 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.comboDriveType = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.mpLabel3 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.resetButton = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpLabel2 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.textBoxExtensions = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.mpLabel1 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.buttonSelectFolder = new MediaPortal.UserInterface.Controls.MPButton();
      this.comboDriveNo = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.comboBoxDrive = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.textBoxDaemonTools = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.label4 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.label3 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.label1 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.checkBoxDaemonTools = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.checkBoxAskBeforePlaying = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.groupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox2
      // 
      this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox2.Controls.Add(this.comboDriveType);
      this.groupBox2.Controls.Add(this.mpLabel3);
      this.groupBox2.Controls.Add(this.resetButton);
      this.groupBox2.Controls.Add(this.mpLabel2);
      this.groupBox2.Controls.Add(this.textBoxExtensions);
      this.groupBox2.Controls.Add(this.mpLabel1);
      this.groupBox2.Controls.Add(this.buttonSelectFolder);
      this.groupBox2.Controls.Add(this.comboDriveNo);
      this.groupBox2.Controls.Add(this.comboBoxDrive);
      this.groupBox2.Controls.Add(this.textBoxDaemonTools);
      this.groupBox2.Controls.Add(this.label4);
      this.groupBox2.Controls.Add(this.label3);
      this.groupBox2.Controls.Add(this.label1);
      this.groupBox2.Controls.Add(this.checkBoxDaemonTools);
      this.groupBox2.Controls.Add(this.checkBoxAskBeforePlaying);
      this.groupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.groupBox2.Location = new System.Drawing.Point(0, 0);
      this.groupBox2.Name = "groupBox2";
      this.groupBox2.Size = new System.Drawing.Size(472, 233);
      this.groupBox2.TabIndex = 0;
      this.groupBox2.TabStop = false;
      this.groupBox2.Text = "Settings";
      // 
      // comboDriveType
      // 
      this.comboDriveType.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboDriveType.BorderColor = System.Drawing.Color.Empty;
      this.comboDriveType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboDriveType.Location = new System.Drawing.Point(168, 118);
      this.comboDriveType.Name = "comboDriveType";
      this.comboDriveType.Size = new System.Drawing.Size(288, 21);
      this.comboDriveType.TabIndex = 14;
      this.comboDriveType.SelectionChangeCommitted += new System.EventHandler(this.comboDriveType_SelectionChangeCommitted);
      // 
      // mpLabel3
      // 
      this.mpLabel3.AutoSize = true;
      this.mpLabel3.Location = new System.Drawing.Point(16, 121);
      this.mpLabel3.Name = "mpLabel3";
      this.mpLabel3.Size = new System.Drawing.Size(62, 13);
      this.mpLabel3.TabIndex = 13;
      this.mpLabel3.Text = "Drive Type:";
      // 
      // resetButton
      // 
      this.resetButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.resetButton.Location = new System.Drawing.Point(384, 152);
      this.resetButton.Name = "resetButton";
      this.resetButton.Size = new System.Drawing.Size(72, 50);
      this.resetButton.TabIndex = 12;
      this.resetButton.Text = "Default";
      this.resetButton.UseVisualStyleBackColor = true;
      this.resetButton.Click += new System.EventHandler(this.resetButton_Click);
      // 
      // mpLabel2
      // 
      this.mpLabel2.AutoSize = true;
      this.mpLabel2.Location = new System.Drawing.Point(16, 206);
      this.mpLabel2.Name = "mpLabel2";
      this.mpLabel2.Size = new System.Drawing.Size(367, 13);
      this.mpLabel2.TabIndex = 11;
      this.mpLabel2.Text = "Supported tools: Windows native ISO, Virtual CloneDrive and Daemon Tools";
      // 
      // textBoxExtensions
      // 
      this.textBoxExtensions.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxExtensions.BorderColor = System.Drawing.Color.Empty;
      this.textBoxExtensions.Location = new System.Drawing.Point(168, 151);
      this.textBoxExtensions.Name = "textBoxExtensions";
      this.textBoxExtensions.Size = new System.Drawing.Size(208, 20);
      this.textBoxExtensions.TabIndex = 10;
      // 
      // mpLabel1
      // 
      this.mpLabel1.AutoSize = true;
      this.mpLabel1.Location = new System.Drawing.Point(16, 155);
      this.mpLabel1.Name = "mpLabel1";
      this.mpLabel1.Size = new System.Drawing.Size(96, 13);
      this.mpLabel1.TabIndex = 9;
      this.mpLabel1.Text = "Supported Images:";
      // 
      // buttonSelectFolder
      // 
      this.buttonSelectFolder.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonSelectFolder.Location = new System.Drawing.Point(384, 43);
      this.buttonSelectFolder.Name = "buttonSelectFolder";
      this.buttonSelectFolder.Size = new System.Drawing.Size(72, 22);
      this.buttonSelectFolder.TabIndex = 3;
      this.buttonSelectFolder.Text = "Browse";
      this.buttonSelectFolder.UseVisualStyleBackColor = true;
      this.buttonSelectFolder.Click += new System.EventHandler(this.buttonSelectFolder_Click);
      // 
      // comboDriveNo
      // 
      this.comboDriveNo.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboDriveNo.BorderColor = System.Drawing.Color.Empty;
      this.comboDriveNo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboDriveNo.Items.AddRange(new object[] {
            "0",
            "1",
            "2",
            "3"});
      this.comboDriveNo.Location = new System.Drawing.Point(168, 92);
      this.comboDriveNo.Name = "comboDriveNo";
      this.comboDriveNo.Size = new System.Drawing.Size(288, 21);
      this.comboDriveNo.TabIndex = 7;
      // 
      // comboBoxDrive
      // 
      this.comboBoxDrive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxDrive.BorderColor = System.Drawing.Color.Empty;
      this.comboBoxDrive.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxDrive.Location = new System.Drawing.Point(168, 68);
      this.comboBoxDrive.Name = "comboBoxDrive";
      this.comboBoxDrive.Size = new System.Drawing.Size(288, 21);
      this.comboBoxDrive.TabIndex = 5;
      // 
      // textBoxDaemonTools
      // 
      this.textBoxDaemonTools.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxDaemonTools.BorderColor = System.Drawing.Color.Empty;
      this.textBoxDaemonTools.Location = new System.Drawing.Point(168, 44);
      this.textBoxDaemonTools.Name = "textBoxDaemonTools";
      this.textBoxDaemonTools.Size = new System.Drawing.Size(208, 20);
      this.textBoxDaemonTools.TabIndex = 2;
      // 
      // label4
      // 
      this.label4.AutoSize = true;
      this.label4.Location = new System.Drawing.Point(16, 96);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(75, 13);
      this.label4.TabIndex = 6;
      this.label4.Text = "Drive Number:";
      // 
      // label3
      // 
      this.label3.AutoSize = true;
      this.label3.Location = new System.Drawing.Point(16, 48);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(55, 13);
      this.label3.TabIndex = 1;
      this.label3.Text = "Drive tool:";
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(16, 72);
      this.label1.Name = "label1";
      this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.label1.Size = new System.Drawing.Size(65, 13);
      this.label1.TabIndex = 4;
      this.label1.Text = "Virtual drive:";
      // 
      // checkBoxDaemonTools
      // 
      this.checkBoxDaemonTools.AutoSize = true;
      this.checkBoxDaemonTools.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.checkBoxDaemonTools.Location = new System.Drawing.Point(168, 20);
      this.checkBoxDaemonTools.Name = "checkBoxDaemonTools";
      this.checkBoxDaemonTools.Size = new System.Drawing.Size(127, 17);
      this.checkBoxDaemonTools.TabIndex = 0;
      this.checkBoxDaemonTools.Text = "Automount image files";
      this.checkBoxDaemonTools.UseVisualStyleBackColor = true;
      this.checkBoxDaemonTools.CheckedChanged += new System.EventHandler(this.checkBoxDaemonTools_CheckedChanged);
      // 
      // checkBoxAskBeforePlaying
      // 
      this.checkBoxAskBeforePlaying.AutoSize = true;
      this.checkBoxAskBeforePlaying.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.checkBoxAskBeforePlaying.Location = new System.Drawing.Point(168, 180);
      this.checkBoxAskBeforePlaying.Name = "checkBoxAskBeforePlaying";
      this.checkBoxAskBeforePlaying.Size = new System.Drawing.Size(163, 17);
      this.checkBoxAskBeforePlaying.TabIndex = 8;
      this.checkBoxAskBeforePlaying.Text = "Ask before playing image files";
      this.checkBoxAskBeforePlaying.UseVisualStyleBackColor = true;
      // 
      // GeneralDaemonTools
      // 
      this.BackColor = System.Drawing.SystemColors.Control;
      this.Controls.Add(this.groupBox2);
      this.Name = "GeneralDaemonTools";
      this.Size = new System.Drawing.Size(472, 408);
      this.groupBox2.ResumeLayout(false);
      this.groupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private void buttonSelectFolder_Click(object sender, EventArgs e)
    {
      using (OpenFileDialog openFileDialog = new OpenFileDialog())
      {
        openFileDialog.FileName = textBoxDaemonTools.Text;
        openFileDialog.CheckFileExists = true;
        openFileDialog.RestoreDirectory = true;
        openFileDialog.Filter = "exe files (*.exe)|*.exe";
        openFileDialog.FilterIndex = 0;
        openFileDialog.Title = "Select tool";

        DialogResult dialogResult = openFileDialog.ShowDialog();

        if (dialogResult == DialogResult.OK)
        {
          textBoxDaemonTools.Text = openFileDialog.FileName;
        }
      }
    }

    private void checkBoxDaemonTools_CheckedChanged(object sender, EventArgs e)
    {
      comboBoxDriveCheck();
      if (checkBoxDaemonTools.Checked)
      {
        if (comboDriveType.SelectedItem != null && (comboDriveType.SelectedItem.ToString() == "native"))
        {
          textBoxDaemonTools.Enabled = false;
          comboBoxDrive.Enabled = true;
          buttonSelectFolder.Enabled = false;
          comboDriveNo.Enabled = false;
          textBoxExtensions.Enabled = false;
          comboDriveType.Enabled = true;
        }
        else
        {
          textBoxDaemonTools.Enabled = true;
          comboBoxDrive.Enabled = true;
          buttonSelectFolder.Enabled = true;
          comboDriveNo.Enabled = true;
          checkBoxAskBeforePlaying.Enabled = true;
          comboDriveType.Enabled = true;
        }
      }
      else
      {
        textBoxDaemonTools.Enabled = false;
        comboBoxDrive.Enabled = false;
        buttonSelectFolder.Enabled = false;
        comboDriveNo.Enabled = false;
        checkBoxAskBeforePlaying.Enabled = false;
        comboDriveType.Enabled = false;
      }
    }

    private void resetButton_Click(object sender, EventArgs e)
    {
      if (MessageBox.Show(
        "Do you really want to reset the extension list to the default?\r\nAny modification you did will be lost.",
        "MediaPortal Configuration", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2)
          == DialogResult.No) return;

      textBoxExtensions.Text = Util.Utils.ImageExtensionsDefault;
    }

    private void comboDriveType_SelectionChangeCommitted(object sender, EventArgs e)
    {
      comboBoxDriveCheck();
      if (comboDriveType.SelectedItem != null && (comboDriveType.SelectedItem.ToString() == "native"))
      {
        textBoxDaemonTools.Enabled = false;
        comboBoxDrive.Enabled = true;
        buttonSelectFolder.Enabled = false;
        comboDriveNo.Enabled = false;
        textBoxExtensions.Enabled = false;
      }
      else if (checkBoxDaemonTools.Checked)
      {
        textBoxDaemonTools.Enabled = true;
        comboBoxDrive.Enabled = true;
        buttonSelectFolder.Enabled = true;
        comboDriveNo.Enabled = true;
        textBoxExtensions.Enabled = true;
        comboDriveType.Enabled = true;
      }
    }

    private void comboBoxDriveCheck()
    {
      try
      {
        // Clear all item to do a proper filled
        this.comboBoxDrive.Items.Clear();
        System.IO.DriveInfo[] drives = System.IO.DriveInfo.GetDrives();
        foreach (System.IO.DriveInfo drive in drives)
        {
          if (drive.DriveType == System.IO.DriveType.CDRom)
          {
            if (comboDriveType.SelectedItem != null && (comboDriveType.SelectedItem.ToString() != "native"))
            {
              if (!this.comboBoxDrive.Items.Contains(String.Format("{0}", drive.RootDirectory)[0] + ":"))
              {
                this.comboBoxDrive.Items.Add(String.Format("{0}", drive.RootDirectory)[0] + ":");
              }
            }
            else
            {
              // native (remove all fixed CDROM drive
              this.comboBoxDrive.Items.Remove(String.Format("{0}", drive.RootDirectory)[0] + ":");
            }
          }
        }
        if (comboDriveType.SelectedItem != null && (comboDriveType.SelectedItem.ToString() == "native"))
        {
          ArrayList driveLetters = new ArrayList(26); // Allocate space for alphabet
          for (int i = 65; i < 91; i++) // increment from ASCII values for A-Z
          {
            driveLetters.Add(Convert.ToChar(i)); // Add uppercase letters to possible drive letters
          }

          foreach (string drive in Directory.GetLogicalDrives())
          {
            driveLetters.Remove(drive[0]); // removed used drive letters from possible drive letters
          }

          foreach (char drive in driveLetters)
          {
            if (!this.comboBoxDrive.Items.Contains(drive))
            {
              this.comboBoxDrive.Items.Add(drive + ":"); // add unused drive letters to the combo box
            }
          }
        }
        using (Settings xmlreader = new MPSettings())
        {
          // Need to detect letter to fill the correct letter.
          comboBoxDrive.SelectedItem = xmlreader.GetValueAsString("daemon", "drive", "E:");
        }
      }
      catch (Exception)
      {
      }
    }
  }
}