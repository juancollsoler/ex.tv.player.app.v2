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

using System.ComponentModel;
using System.Collections.Generic;
using System.Diagnostics;
using MediaPortal.Profile;
using MediaPortal.UserInterface.Controls;
using MediaPortal.Util;
using MediaPortal.GUI.Library;

#pragma warning disable 108

namespace MediaPortal.Configuration.Sections
{
  public class TVAdvancedOptions : SectionSettings
  {
    private MPGroupBox groupBoxSettings;
    private MPRadioButton radioButton1;
    private MPCheckBox mpUseRtspCheckBox;
    private MPLabel mpWarningLabel;
    private MPCheckBox mpDoNotAllowSlowMotionDuringZappingCheckBox;
    private MPToolTip mpMainToolTip;
    private static bool singleSeat;
    private MPGroupBox mpRtspPathsGroupBox;
    private MPLabel mpLabel1;
    private MPLabel mpLabelRecording;
    private MPLabel mpLabelTimeshifting;
    private System.Windows.Forms.TextBox textBoxTimeshifting;
    private System.Windows.Forms.TextBox textBoxRecording;
    private System.Windows.Forms.Button buttonTimeshiftingPath;
    private System.Windows.Forms.Button buttonRecordingPath;
    private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
    private MPLabel mpLabelWarning;
    private MPLabel mpLabelNote;
    public int pluginVersion;
    private MPLabel mpLabel2;
    private string hostname;


    public TVAdvancedOptions()
      : this("Advanced Options") {}

    public TVAdvancedOptions(string name)
      : base(name)
    {
      // This call is required by the Windows Form Designer.
      InitializeComponent();
    }

    public override void OnSectionActivated()
    {
      CheckAndResetSettings();
      base.OnSectionActivated();
    }

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TVAdvancedOptions));
      this.groupBoxSettings = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.mpLabel2 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpDoNotAllowSlowMotionDuringZappingCheckBox = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.mpUseRtspCheckBox = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.mpWarningLabel = new MediaPortal.UserInterface.Controls.MPLabel();
      this.radioButton1 = new MediaPortal.UserInterface.Controls.MPRadioButton();
      this.mpMainToolTip = new MediaPortal.UserInterface.Controls.MPToolTip();
      this.mpRtspPathsGroupBox = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.mpLabelNote = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabelWarning = new MediaPortal.UserInterface.Controls.MPLabel();
      this.buttonTimeshiftingPath = new System.Windows.Forms.Button();
      this.buttonRecordingPath = new System.Windows.Forms.Button();
      this.mpLabel1 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabelRecording = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabelTimeshifting = new MediaPortal.UserInterface.Controls.MPLabel();
      this.textBoxTimeshifting = new System.Windows.Forms.TextBox();
      this.textBoxRecording = new System.Windows.Forms.TextBox();
      this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.groupBoxSettings.SuspendLayout();
      this.mpRtspPathsGroupBox.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBoxSettings
      // 
      this.groupBoxSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBoxSettings.Controls.Add(this.mpLabel2);
      this.groupBoxSettings.Controls.Add(this.mpDoNotAllowSlowMotionDuringZappingCheckBox);
      this.groupBoxSettings.Controls.Add(this.mpUseRtspCheckBox);
      this.groupBoxSettings.Controls.Add(this.mpWarningLabel);
      this.groupBoxSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.groupBoxSettings.Location = new System.Drawing.Point(0, 0);
      this.groupBoxSettings.Name = "groupBoxSettings";
      this.groupBoxSettings.Size = new System.Drawing.Size(472, 130);
      this.groupBoxSettings.TabIndex = 0;
      this.groupBoxSettings.TabStop = false;
      this.groupBoxSettings.Text = "Settings";
      // 
      // mpLabel2
      // 
      this.mpLabel2.AutoSize = true;
      this.mpLabel2.ForeColor = System.Drawing.Color.Red;
      this.mpLabel2.Location = new System.Drawing.Point(9, 52);
      this.mpLabel2.Name = "mpLabel2";
      this.mpLabel2.Size = new System.Drawing.Size(232, 13);
      this.mpLabel2.TabIndex = 5;
      this.mpLabel2.Text = "Click on \'help\' (above right) for more information.";
      // 
      // mpDoNotAllowSlowMotionDuringZappingCheckBox
      // 
      this.mpDoNotAllowSlowMotionDuringZappingCheckBox.AutoSize = true;
      this.mpDoNotAllowSlowMotionDuringZappingCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpDoNotAllowSlowMotionDuringZappingCheckBox.Location = new System.Drawing.Point(9, 73);
      this.mpDoNotAllowSlowMotionDuringZappingCheckBox.Name = "mpDoNotAllowSlowMotionDuringZappingCheckBox";
      this.mpDoNotAllowSlowMotionDuringZappingCheckBox.Size = new System.Drawing.Size(336, 17);
      this.mpDoNotAllowSlowMotionDuringZappingCheckBox.TabIndex = 4;
      this.mpDoNotAllowSlowMotionDuringZappingCheckBox.Text = "Do not use slow motion to sync video to audio on channel change";
      this.mpMainToolTip.SetToolTip(this.mpDoNotAllowSlowMotionDuringZappingCheckBox, "Selecting this will prevent live TV from playing video until video is in sync wit" +
        "h the audio, instead of playing video in slow motion");
      this.mpDoNotAllowSlowMotionDuringZappingCheckBox.UseVisualStyleBackColor = true;
      // 
      // mpUseRtspCheckBox
      // 
      this.mpUseRtspCheckBox.AutoSize = true;
      this.mpUseRtspCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpUseRtspCheckBox.Location = new System.Drawing.Point(9, 97);
      this.mpUseRtspCheckBox.Name = "mpUseRtspCheckBox";
      this.mpUseRtspCheckBox.Size = new System.Drawing.Size(144, 17);
      this.mpUseRtspCheckBox.TabIndex = 1;
      this.mpUseRtspCheckBox.Text = "-- Label defined in code --\r\n";
      this.mpUseRtspCheckBox.UseVisualStyleBackColor = true;
      this.mpUseRtspCheckBox.CheckedChanged += new System.EventHandler(this.mpUseRtspCheckBox_Checked);
      // 
      // mpWarningLabel
      // 
      this.mpWarningLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.mpWarningLabel.ForeColor = System.Drawing.Color.Red;
      this.mpWarningLabel.Location = new System.Drawing.Point(6, 18);
      this.mpWarningLabel.Name = "mpWarningLabel";
      this.mpWarningLabel.Size = new System.Drawing.Size(460, 37);
      this.mpWarningLabel.TabIndex = 0;
      this.mpWarningLabel.Text = "This section provides special advanced option settings. Some of these settings ar" +
    "e experimental. Do not alter any of the settings below unless you know what you " +
    "are doing.";
      // 
      // radioButton1
      // 
      this.radioButton1.AutoSize = true;
      this.radioButton1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.radioButton1.Location = new System.Drawing.Point(0, 0);
      this.radioButton1.Name = "radioButton1";
      this.radioButton1.Size = new System.Drawing.Size(104, 24);
      this.radioButton1.TabIndex = 0;
      this.radioButton1.UseVisualStyleBackColor = true;
      // 
      // mpRtspPathsGroupBox
      // 
      this.mpRtspPathsGroupBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.mpRtspPathsGroupBox.Controls.Add(this.mpLabelNote);
      this.mpRtspPathsGroupBox.Controls.Add(this.mpLabelWarning);
      this.mpRtspPathsGroupBox.Controls.Add(this.buttonTimeshiftingPath);
      this.mpRtspPathsGroupBox.Controls.Add(this.buttonRecordingPath);
      this.mpRtspPathsGroupBox.Controls.Add(this.mpLabel1);
      this.mpRtspPathsGroupBox.Controls.Add(this.mpLabelRecording);
      this.mpRtspPathsGroupBox.Controls.Add(this.mpLabelTimeshifting);
      this.mpRtspPathsGroupBox.Controls.Add(this.textBoxTimeshifting);
      this.mpRtspPathsGroupBox.Controls.Add(this.textBoxRecording);
      this.mpRtspPathsGroupBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpRtspPathsGroupBox.Location = new System.Drawing.Point(0, 133);
      this.mpRtspPathsGroupBox.Name = "mpRtspPathsGroupBox";
      this.mpRtspPathsGroupBox.Size = new System.Drawing.Size(472, 275);
      this.mpRtspPathsGroupBox.TabIndex = 1;
      this.mpRtspPathsGroupBox.TabStop = false;
      this.mpRtspPathsGroupBox.Text = "Additional UNC settings";
      // 
      // mpLabelNote
      // 
      this.mpLabelNote.AutoSize = true;
      this.mpLabelNote.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.mpLabelNote.ForeColor = System.Drawing.Color.Red;
      this.mpLabelNote.Location = new System.Drawing.Point(6, 24);
      this.mpLabelNote.Name = "mpLabelNote";
      this.mpLabelNote.Size = new System.Drawing.Size(45, 13);
      this.mpLabelNote.TabIndex = 8;
      this.mpLabelNote.Text = "NOTE:";
      // 
      // mpLabelWarning
      // 
      this.mpLabelWarning.ForeColor = System.Drawing.Color.Black;
      this.mpLabelWarning.Location = new System.Drawing.Point(54, 24);
      this.mpLabelWarning.Name = "mpLabelWarning";
      this.mpLabelWarning.Size = new System.Drawing.Size(409, 32);
      this.mpLabelWarning.TabIndex = 7;
      this.mpLabelWarning.Text = "When using UNC paths, all tuners must use the same Timeshifting path and the same" +
    " Recording path! ";
      // 
      // buttonTimeshiftingPath
      // 
      this.buttonTimeshiftingPath.Location = new System.Drawing.Point(408, 139);
      this.buttonTimeshiftingPath.Name = "buttonTimeshiftingPath";
      this.buttonTimeshiftingPath.Size = new System.Drawing.Size(58, 20);
      this.buttonTimeshiftingPath.TabIndex = 6;
      this.buttonTimeshiftingPath.Text = "browse";
      this.buttonTimeshiftingPath.UseVisualStyleBackColor = true;
      this.buttonTimeshiftingPath.Click += new System.EventHandler(this.buttonTimeshiftingPath_Click);
      // 
      // buttonRecordingPath
      // 
      this.buttonRecordingPath.Location = new System.Drawing.Point(408, 90);
      this.buttonRecordingPath.Name = "buttonRecordingPath";
      this.buttonRecordingPath.Size = new System.Drawing.Size(58, 20);
      this.buttonRecordingPath.TabIndex = 5;
      this.buttonRecordingPath.Text = "browse";
      this.buttonRecordingPath.UseVisualStyleBackColor = true;
      this.buttonRecordingPath.Click += new System.EventHandler(this.buttonRecordingPath_Click);
      // 
      // mpLabel1
      // 
      this.mpLabel1.AutoSize = true;
      this.mpLabel1.Location = new System.Drawing.Point(30, 165);
      this.mpLabel1.Name = "mpLabel1";
      this.mpLabel1.Size = new System.Drawing.Size(392, 104);
      this.mpLabel1.TabIndex = 4;
      this.mpLabel1.Text = resources.GetString("mpLabel1.Text");
      // 
      // mpLabelRecording
      // 
      this.mpLabelRecording.AutoSize = true;
      this.mpLabelRecording.Location = new System.Drawing.Point(6, 73);
      this.mpLabelRecording.Name = "mpLabelRecording";
      this.mpLabelRecording.Size = new System.Drawing.Size(83, 13);
      this.mpLabelRecording.TabIndex = 3;
      this.mpLabelRecording.Text = "Recording path:";
      // 
      // mpLabelTimeshifting
      // 
      this.mpLabelTimeshifting.AutoSize = true;
      this.mpLabelTimeshifting.Location = new System.Drawing.Point(6, 121);
      this.mpLabelTimeshifting.Name = "mpLabelTimeshifting";
      this.mpLabelTimeshifting.Size = new System.Drawing.Size(90, 13);
      this.mpLabelTimeshifting.TabIndex = 2;
      this.mpLabelTimeshifting.Text = "Timeshifting path:";
      // 
      // textBoxTimeshifting
      // 
      this.textBoxTimeshifting.Location = new System.Drawing.Point(9, 139);
      this.textBoxTimeshifting.Name = "textBoxTimeshifting";
      this.textBoxTimeshifting.Size = new System.Drawing.Size(398, 20);
      this.textBoxTimeshifting.TabIndex = 1;
      // 
      // textBoxRecording
      // 
      this.textBoxRecording.Location = new System.Drawing.Point(9, 91);
      this.textBoxRecording.Name = "textBoxRecording";
      this.textBoxRecording.Size = new System.Drawing.Size(398, 20);
      this.textBoxRecording.TabIndex = 0;
      // 
      // folderBrowserDialog
      // 
      this.folderBrowserDialog.Description = "Select the appropriate network folder";
      this.folderBrowserDialog.ShowNewFolderButton = false;
      // 
      // TVAdvancedOptions
      // 
      this.Controls.Add(this.mpRtspPathsGroupBox);
      this.Controls.Add(this.groupBoxSettings);
      this.Name = "TVAdvancedOptions";
      this.Size = new System.Drawing.Size(472, 408);
      this.groupBoxSettings.ResumeLayout(false);
      this.groupBoxSettings.PerformLayout();
      this.mpRtspPathsGroupBox.ResumeLayout(false);
      this.mpRtspPathsGroupBox.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    public void CheckAndResetSettings()
    {
      if (hostname != TVRadio.TextBoxHostname)
      {
        Log.Debug("TVAdvancedOptions: TV Server hostname is changed from \"{0}\" to \"{1}\".", hostname, TVRadio.TextBoxHostname);
        Network.Reset();
        singleSeat = Network.IsSingleSeat(TVRadio.TextBoxHostname);
        mpUseRtspCheckBox.Checked = false;
        mpUseRtspCheckBox.Text = singleSeat ? "Single seat setup: force RTSP usage." : "Multi seat setup: use UNC paths.";
        hostname = TVRadio.TextBoxHostname;
      }
    }

    public override void LoadSettings()
    {
      string serverName;
      using (Settings reader = new MPSettings())
      {
        serverName = reader.GetValueAsString("tvservice", "hostname", string.Empty);
      }
      if (serverName != string.Empty)
      {
        singleSeat = Network.IsSingleSeat();
      }
      else
      {
        singleSeat = true;
      }
      
      hostname = serverName;

      bool rtsp;
      using (Settings xmlreader = new MPSettings())
      {
        textBoxRecording.Text = xmlreader.GetValueAsString("tvservice", "recordingpath", "");
        textBoxTimeshifting.Text = xmlreader.GetValueAsString("tvservice", "timeshiftingpath", "");
        rtsp = xmlreader.GetValueAsBool("tvservice", "usertsp", !singleSeat);
        mpUseRtspCheckBox.Checked = singleSeat ? rtsp : !rtsp;
      }
      mpUseRtspCheckBox.Text = singleSeat ? "Single seat setup: force RTSP usage." : "Multi seat setup: use UNC paths.";
      mpRtspPathsGroupBox.Visible = (!singleSeat && !rtsp);

      mpDoNotAllowSlowMotionDuringZappingCheckBox.Checked = DebugSettings.DoNotAllowSlowMotionDuringZapping;
    }

    public override void SaveSettings()
    {
      CheckAndResetSettings();
      bool rtsp = singleSeat ? mpUseRtspCheckBox.Checked : !mpUseRtspCheckBox.Checked;

      using (Settings xmlwriter = new MPSettings())
      {
        xmlwriter.SetValueAsBool("tvservice", "usertsp", rtsp);
        xmlwriter.SetValue("tvservice", "recordingpath", textBoxRecording.Text);
        xmlwriter.SetValue("tvservice", "timeshiftingpath", textBoxTimeshifting.Text);
        xmlwriter.SetValueAsBool("tvservice", "AdvancedOptions", true);
      }
      DebugSettings.DoNotAllowSlowMotionDuringZapping = mpDoNotAllowSlowMotionDuringZappingCheckBox.Checked;
    }

    private void mpUseRtspCheckBox_Checked(object sender, System.EventArgs e)
    {
      mpRtspPathsGroupBox.Visible = !singleSeat && mpUseRtspCheckBox.Checked;
    }

    private void buttonRecordingPath_Click(object sender, System.EventArgs e)
    {
      folderBrowserDialog.ShowDialog();
      textBoxRecording.Text = folderBrowserDialog.SelectedPath;
    }

    private void buttonTimeshiftingPath_Click(object sender, System.EventArgs e)
    {
      folderBrowserDialog.ShowDialog();
      textBoxTimeshifting.Text = folderBrowserDialog.SelectedPath;
    }
  }
}