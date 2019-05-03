﻿using System.Windows.Forms;
using MediaPortal.UserInterface.Controls;

namespace MediaPortal.Configuration.Sections
{
  partial class Music
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private new System.ComponentModel.IContainer components = null;

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

    #region Component Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
      this.MusicSettingsTabCtl = new MediaPortal.UserInterface.Controls.MPTabControl();
      this.PlayerTabPg = new System.Windows.Forms.TabPage();
      this.tabControlPlayerSettings = new MediaPortal.UserInterface.Controls.MPTabControl();
      this.tabPageBassPlayerSettings = new System.Windows.Forms.TabPage();
      this.EnableAlbumReplayGainChkBox = new System.Windows.Forms.CheckBox();
      this.EnableReplayGainChkBox = new System.Windows.Forms.CheckBox();
      this.trackBarCrossfade = new System.Windows.Forms.TrackBar();
      this.trackBarBuffering = new System.Windows.Forms.TrackBar();
      this.UseSkipStepsCheckBox = new System.Windows.Forms.CheckBox();
      this.BufferingSecondsLbl = new System.Windows.Forms.Label();
      this.CrossFadeSecondsLbl = new System.Windows.Forms.Label();
      this.label12 = new System.Windows.Forms.Label();
      this.GaplessPlaybackChkBox = new System.Windows.Forms.CheckBox();
      this.StreamOutputLevelNud = new System.Windows.Forms.NumericUpDown();
      this.CrossFadingLbl = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel1 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.FadeOnStartStopChkbox = new System.Windows.Forms.CheckBox();
      this.tabPageASIOPlayerSettings = new System.Windows.Forms.TabPage();
      this.lbBalance = new MediaPortal.UserInterface.Controls.MPLabel();
      this.hScrollBarBalance = new System.Windows.Forms.HScrollBar();
      this.mpLabel6 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel7 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel4 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.btAsioDeviceSettings = new MediaPortal.UserInterface.Controls.MPButton();
      this.tabPageWASAPIPLayerSettings = new System.Windows.Forms.TabPage();
      this.mpLabel15 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.WasApiSpeakersCombo = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.mpLabel14 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.WasapiExclusiveModeCkBox = new System.Windows.Forms.CheckBox();
      this.mpLabel5 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.tabPagePlayerUpmixSettings = new System.Windows.Forms.TabPage();
      this.mpGroupBox2 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.cbUpmixFiveDotOne = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.mpLabel11 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cbUpmixQuadro = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.mpLabel10 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cbUpmixStereo = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.mpLabel9 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cbUpmixMono = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.mpLabel8 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpGroupBox1 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.mpLabel2 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.soundDeviceComboBox = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.label2 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.audioPlayerComboBox = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.PlaySettingsTabPg = new System.Windows.Forms.TabPage();
      this.mpGroupBox4 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.tbResumeSearchValue = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.mpLabel13 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.cbResumeSelect = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.mpLabel12 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel3 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.tbResumeAfter = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.chkEnableResumeSupport = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.groupBox3 = new System.Windows.Forms.GroupBox();
      this.PlayNowJumpToCmbBox = new System.Windows.Forms.ComboBox();
      this.label8 = new System.Windows.Forms.Label();
      this.grpSelectOptions = new System.Windows.Forms.GroupBox();
      this.cmbSelectOption = new System.Windows.Forms.ComboBox();
      this.chkAddAllTracks = new System.Windows.Forms.CheckBox();
      this.tabPageNowPlaying = new System.Windows.Forms.TabPage();
      this.groupBoxVUMeter = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.radioButtonVULed = new MediaPortal.UserInterface.Controls.MPRadioButton();
      this.radioButtonVUAnalog = new MediaPortal.UserInterface.Controls.MPRadioButton();
      this.radioButtonVUNone = new MediaPortal.UserInterface.Controls.MPRadioButton();
      this.groupBoxDynamicContent = new System.Windows.Forms.GroupBox();
      this.chkDisableSimilarTrackLookups = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.PlaylistTabPg = new System.Windows.Forms.TabPage();
      this.groupBox1 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.PlayListUTF8CheckBox = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.PlaylistCurrentCheckBox = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.autoShuffleCheckBox = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.ResumePlaylistChkBox = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.SavePlaylistOnExitChkBox = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.repeatPlaylistCheckBox = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.playlistButton = new MediaPortal.UserInterface.Controls.MPButton();
      this.playlistFolderTextBox = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.label1 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.label4 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.checkBox2 = new System.Windows.Forms.CheckBox();
      this.MusicSettingsTabCtl.SuspendLayout();
      this.PlayerTabPg.SuspendLayout();
      this.tabControlPlayerSettings.SuspendLayout();
      this.tabPageBassPlayerSettings.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarCrossfade)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarBuffering)).BeginInit();
      ((System.ComponentModel.ISupportInitialize)(this.StreamOutputLevelNud)).BeginInit();
      this.tabPageASIOPlayerSettings.SuspendLayout();
      this.tabPageWASAPIPLayerSettings.SuspendLayout();
      this.tabPagePlayerUpmixSettings.SuspendLayout();
      this.mpGroupBox2.SuspendLayout();
      this.mpGroupBox1.SuspendLayout();
      this.PlaySettingsTabPg.SuspendLayout();
      this.mpGroupBox4.SuspendLayout();
      this.groupBox3.SuspendLayout();
      this.grpSelectOptions.SuspendLayout();
      this.tabPageNowPlaying.SuspendLayout();
      this.groupBoxVUMeter.SuspendLayout();
      this.groupBoxDynamicContent.SuspendLayout();
      this.PlaylistTabPg.SuspendLayout();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // MusicSettingsTabCtl
      // 
      this.MusicSettingsTabCtl.Controls.Add(this.PlayerTabPg);
      this.MusicSettingsTabCtl.Controls.Add(this.PlaySettingsTabPg);
      this.MusicSettingsTabCtl.Controls.Add(this.tabPageNowPlaying);
      this.MusicSettingsTabCtl.Controls.Add(this.PlaylistTabPg);
      this.MusicSettingsTabCtl.Location = new System.Drawing.Point(0, 8);
      this.MusicSettingsTabCtl.Name = "MusicSettingsTabCtl";
      this.MusicSettingsTabCtl.SelectedIndex = 0;
      this.MusicSettingsTabCtl.Size = new System.Drawing.Size(472, 447);
      this.MusicSettingsTabCtl.TabIndex = 1;
      // 
      // PlayerTabPg
      // 
      this.PlayerTabPg.Controls.Add(this.tabControlPlayerSettings);
      this.PlayerTabPg.Controls.Add(this.mpGroupBox1);
      this.PlayerTabPg.Location = new System.Drawing.Point(4, 22);
      this.PlayerTabPg.Name = "PlayerTabPg";
      this.PlayerTabPg.Padding = new System.Windows.Forms.Padding(3);
      this.PlayerTabPg.Size = new System.Drawing.Size(464, 421);
      this.PlayerTabPg.TabIndex = 1;
      this.PlayerTabPg.Text = "Player settings";
      this.PlayerTabPg.UseVisualStyleBackColor = true;
      // 
      // tabControlPlayerSettings
      // 
      this.tabControlPlayerSettings.Controls.Add(this.tabPageBassPlayerSettings);
      this.tabControlPlayerSettings.Controls.Add(this.tabPageASIOPlayerSettings);
      this.tabControlPlayerSettings.Controls.Add(this.tabPageWASAPIPLayerSettings);
      this.tabControlPlayerSettings.Controls.Add(this.tabPagePlayerUpmixSettings);
      this.tabControlPlayerSettings.Location = new System.Drawing.Point(16, 109);
      this.tabControlPlayerSettings.Name = "tabControlPlayerSettings";
      this.tabControlPlayerSettings.SelectedIndex = 0;
      this.tabControlPlayerSettings.Size = new System.Drawing.Size(432, 306);
      this.tabControlPlayerSettings.TabIndex = 14;
      // 
      // tabPageBassPlayerSettings
      // 
      this.tabPageBassPlayerSettings.Controls.Add(this.EnableAlbumReplayGainChkBox);
      this.tabPageBassPlayerSettings.Controls.Add(this.EnableReplayGainChkBox);
      this.tabPageBassPlayerSettings.Controls.Add(this.trackBarCrossfade);
      this.tabPageBassPlayerSettings.Controls.Add(this.trackBarBuffering);
      this.tabPageBassPlayerSettings.Controls.Add(this.UseSkipStepsCheckBox);
      this.tabPageBassPlayerSettings.Controls.Add(this.BufferingSecondsLbl);
      this.tabPageBassPlayerSettings.Controls.Add(this.CrossFadeSecondsLbl);
      this.tabPageBassPlayerSettings.Controls.Add(this.label12);
      this.tabPageBassPlayerSettings.Controls.Add(this.GaplessPlaybackChkBox);
      this.tabPageBassPlayerSettings.Controls.Add(this.StreamOutputLevelNud);
      this.tabPageBassPlayerSettings.Controls.Add(this.CrossFadingLbl);
      this.tabPageBassPlayerSettings.Controls.Add(this.mpLabel1);
      this.tabPageBassPlayerSettings.Controls.Add(this.FadeOnStartStopChkbox);
      this.tabPageBassPlayerSettings.Location = new System.Drawing.Point(4, 22);
      this.tabPageBassPlayerSettings.Name = "tabPageBassPlayerSettings";
      this.tabPageBassPlayerSettings.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageBassPlayerSettings.Size = new System.Drawing.Size(424, 280);
      this.tabPageBassPlayerSettings.TabIndex = 0;
      this.tabPageBassPlayerSettings.Text = "General BASS Player Settings";
      this.tabPageBassPlayerSettings.UseVisualStyleBackColor = true;
      // 
      // EnableAlbumReplayGainChkBox
      // 
      this.EnableAlbumReplayGainChkBox.AutoSize = true;
      this.EnableAlbumReplayGainChkBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.EnableAlbumReplayGainChkBox.Location = new System.Drawing.Point(166, 50);
      this.EnableAlbumReplayGainChkBox.Name = "EnableAlbumReplayGainChkBox";
      this.EnableAlbumReplayGainChkBox.Size = new System.Drawing.Size(209, 17);
      this.EnableAlbumReplayGainChkBox.TabIndex = 19;
      this.EnableAlbumReplayGainChkBox.Text = "Enable Album Replay Gain (if available)";
      this.EnableAlbumReplayGainChkBox.UseVisualStyleBackColor = true;
      // 
      // EnableReplayGainChkBox
      // 
      this.EnableReplayGainChkBox.AutoSize = true;
      this.EnableReplayGainChkBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.EnableReplayGainChkBox.Location = new System.Drawing.Point(17, 50);
      this.EnableReplayGainChkBox.Name = "EnableReplayGainChkBox";
      this.EnableReplayGainChkBox.Size = new System.Drawing.Size(118, 17);
      this.EnableReplayGainChkBox.TabIndex = 18;
      this.EnableReplayGainChkBox.Text = "Enable Replay Gain";
      this.EnableReplayGainChkBox.UseVisualStyleBackColor = true;
      // 
      // trackBarCrossfade
      // 
      this.trackBarCrossfade.LargeChange = 500;
      this.trackBarCrossfade.Location = new System.Drawing.Point(96, 165);
      this.trackBarCrossfade.Maximum = 10000;
      this.trackBarCrossfade.Name = "trackBarCrossfade";
      this.trackBarCrossfade.Size = new System.Drawing.Size(188, 45);
      this.trackBarCrossfade.TabIndex = 17;
      this.trackBarCrossfade.TickFrequency = 100;
      this.trackBarCrossfade.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.trackBarCrossfade.Value = 4000;
      this.trackBarCrossfade.Scroll += new System.EventHandler(this.trackBarCrossfade_Scroll);
      // 
      // trackBarBuffering
      // 
      this.trackBarBuffering.LargeChange = 100;
      this.trackBarBuffering.Location = new System.Drawing.Point(96, 216);
      this.trackBarBuffering.Maximum = 2000;
      this.trackBarBuffering.Minimum = 100;
      this.trackBarBuffering.Name = "trackBarBuffering";
      this.trackBarBuffering.Size = new System.Drawing.Size(188, 45);
      this.trackBarBuffering.TabIndex = 16;
      this.trackBarBuffering.TickFrequency = 100;
      this.trackBarBuffering.TickStyle = System.Windows.Forms.TickStyle.Both;
      this.trackBarBuffering.Value = 500;
      this.trackBarBuffering.Scroll += new System.EventHandler(this.trackBarBuffering_Scroll);
      // 
      // UseSkipStepsCheckBox
      // 
      this.UseSkipStepsCheckBox.AutoSize = true;
      this.UseSkipStepsCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.UseSkipStepsCheckBox.Location = new System.Drawing.Point(17, 82);
      this.UseSkipStepsCheckBox.Name = "UseSkipStepsCheckBox";
      this.UseSkipStepsCheckBox.Size = new System.Drawing.Size(190, 17);
      this.UseSkipStepsCheckBox.TabIndex = 15;
      this.UseSkipStepsCheckBox.Text = "Use Skip steps on forward / rewind";
      this.UseSkipStepsCheckBox.UseVisualStyleBackColor = true;
      // 
      // BufferingSecondsLbl
      // 
      this.BufferingSecondsLbl.Location = new System.Drawing.Point(310, 230);
      this.BufferingSecondsLbl.Name = "BufferingSecondsLbl";
      this.BufferingSecondsLbl.Size = new System.Drawing.Size(80, 13);
      this.BufferingSecondsLbl.TabIndex = 9;
      this.BufferingSecondsLbl.Text = "00.0 Seconds";
      this.BufferingSecondsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // CrossFadeSecondsLbl
      // 
      this.CrossFadeSecondsLbl.Location = new System.Drawing.Point(310, 180);
      this.CrossFadeSecondsLbl.Name = "CrossFadeSecondsLbl";
      this.CrossFadeSecondsLbl.Size = new System.Drawing.Size(80, 13);
      this.CrossFadeSecondsLbl.TabIndex = 6;
      this.CrossFadeSecondsLbl.Text = "00.0 Seconds";
      this.CrossFadeSecondsLbl.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
      // 
      // label12
      // 
      this.label12.AutoSize = true;
      this.label12.Location = new System.Drawing.Point(14, 230);
      this.label12.Name = "label12";
      this.label12.Size = new System.Drawing.Size(52, 13);
      this.label12.TabIndex = 7;
      this.label12.Text = "Buffering:";
      // 
      // GaplessPlaybackChkBox
      // 
      this.GaplessPlaybackChkBox.AutoSize = true;
      this.GaplessPlaybackChkBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.GaplessPlaybackChkBox.Location = new System.Drawing.Point(17, 138);
      this.GaplessPlaybackChkBox.Name = "GaplessPlaybackChkBox";
      this.GaplessPlaybackChkBox.Size = new System.Drawing.Size(108, 17);
      this.GaplessPlaybackChkBox.TabIndex = 3;
      this.GaplessPlaybackChkBox.Text = "Gapless playback";
      this.GaplessPlaybackChkBox.UseVisualStyleBackColor = true;
      this.GaplessPlaybackChkBox.CheckedChanged += new System.EventHandler(this.GaplessPlaybackChkBox_CheckedChanged);
      // 
      // StreamOutputLevelNud
      // 
      this.StreamOutputLevelNud.Location = new System.Drawing.Point(87, 14);
      this.StreamOutputLevelNud.Name = "StreamOutputLevelNud";
      this.StreamOutputLevelNud.Size = new System.Drawing.Size(52, 20);
      this.StreamOutputLevelNud.TabIndex = 1;
      this.StreamOutputLevelNud.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
      // 
      // CrossFadingLbl
      // 
      this.CrossFadingLbl.AutoSize = true;
      this.CrossFadingLbl.Location = new System.Drawing.Point(14, 180);
      this.CrossFadingLbl.Name = "CrossFadingLbl";
      this.CrossFadingLbl.Size = new System.Drawing.Size(68, 13);
      this.CrossFadingLbl.TabIndex = 4;
      this.CrossFadingLbl.Text = "Cross-fading:";
      // 
      // mpLabel1
      // 
      this.mpLabel1.AutoSize = true;
      this.mpLabel1.Location = new System.Drawing.Point(14, 16);
      this.mpLabel1.Name = "mpLabel1";
      this.mpLabel1.Size = new System.Drawing.Size(67, 13);
      this.mpLabel1.TabIndex = 0;
      this.mpLabel1.Text = "Output level:";
      // 
      // FadeOnStartStopChkbox
      // 
      this.FadeOnStartStopChkbox.AutoSize = true;
      this.FadeOnStartStopChkbox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.FadeOnStartStopChkbox.Location = new System.Drawing.Point(17, 110);
      this.FadeOnStartStopChkbox.Name = "FadeOnStartStopChkbox";
      this.FadeOnStartStopChkbox.Size = new System.Drawing.Size(185, 17);
      this.FadeOnStartStopChkbox.TabIndex = 2;
      this.FadeOnStartStopChkbox.Text = "Fade-in on start / fade-out on stop";
      this.FadeOnStartStopChkbox.UseVisualStyleBackColor = true;
      // 
      // tabPageASIOPlayerSettings
      // 
      this.tabPageASIOPlayerSettings.Controls.Add(this.lbBalance);
      this.tabPageASIOPlayerSettings.Controls.Add(this.hScrollBarBalance);
      this.tabPageASIOPlayerSettings.Controls.Add(this.mpLabel6);
      this.tabPageASIOPlayerSettings.Controls.Add(this.mpLabel7);
      this.tabPageASIOPlayerSettings.Controls.Add(this.mpLabel4);
      this.tabPageASIOPlayerSettings.Controls.Add(this.btAsioDeviceSettings);
      this.tabPageASIOPlayerSettings.Location = new System.Drawing.Point(4, 22);
      this.tabPageASIOPlayerSettings.Name = "tabPageASIOPlayerSettings";
      this.tabPageASIOPlayerSettings.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageASIOPlayerSettings.Size = new System.Drawing.Size(424, 280);
      this.tabPageASIOPlayerSettings.TabIndex = 1;
      this.tabPageASIOPlayerSettings.Text = "ASIO";
      this.tabPageASIOPlayerSettings.UseVisualStyleBackColor = true;
      // 
      // lbBalance
      // 
      this.lbBalance.AutoSize = true;
      this.lbBalance.Location = new System.Drawing.Point(357, 186);
      this.lbBalance.Name = "lbBalance";
      this.lbBalance.Size = new System.Drawing.Size(28, 13);
      this.lbBalance.TabIndex = 16;
      this.lbBalance.Text = "0.00";
      // 
      // hScrollBarBalance
      // 
      this.hScrollBarBalance.Location = new System.Drawing.Point(69, 181);
      this.hScrollBarBalance.Minimum = -100;
      this.hScrollBarBalance.Name = "hScrollBarBalance";
      this.hScrollBarBalance.Size = new System.Drawing.Size(262, 18);
      this.hScrollBarBalance.TabIndex = 15;
      this.hScrollBarBalance.ValueChanged += new System.EventHandler(this.hScrollBarBalance_ValueChanged);
      // 
      // mpLabel6
      // 
      this.mpLabel6.AutoSize = true;
      this.mpLabel6.Location = new System.Drawing.Point(72, 218);
      this.mpLabel6.Name = "mpLabel6";
      this.mpLabel6.Size = new System.Drawing.Size(282, 26);
      this.mpLabel6.TabIndex = 14;
      this.mpLabel6.Text = "In case of multi-channel (not stereo) the left/right positions \r\nare interleaved " +
    "between the additional channels.";
      // 
      // mpLabel7
      // 
      this.mpLabel7.AutoSize = true;
      this.mpLabel7.Location = new System.Drawing.Point(15, 181);
      this.mpLabel7.Name = "mpLabel7";
      this.mpLabel7.Size = new System.Drawing.Size(49, 13);
      this.mpLabel7.TabIndex = 13;
      this.mpLabel7.Text = "Balance:";
      // 
      // mpLabel4
      // 
      this.mpLabel4.AutoSize = true;
      this.mpLabel4.Location = new System.Drawing.Point(21, 20);
      this.mpLabel4.Name = "mpLabel4";
      this.mpLabel4.Size = new System.Drawing.Size(184, 13);
      this.mpLabel4.TabIndex = 1;
      this.mpLabel4.Text = "Playback via BASS using ASIO driver";
      // 
      // btAsioDeviceSettings
      // 
      this.btAsioDeviceSettings.Location = new System.Drawing.Point(21, 54);
      this.btAsioDeviceSettings.Name = "btAsioDeviceSettings";
      this.btAsioDeviceSettings.Size = new System.Drawing.Size(140, 23);
      this.btAsioDeviceSettings.TabIndex = 0;
      this.btAsioDeviceSettings.Text = "Asio Device Settings";
      this.btAsioDeviceSettings.UseVisualStyleBackColor = true;
      this.btAsioDeviceSettings.Click += new System.EventHandler(this.btAsioDeviceSettings_Click);
      // 
      // tabPageWASAPIPLayerSettings
      // 
      this.tabPageWASAPIPLayerSettings.Controls.Add(this.mpLabel15);
      this.tabPageWASAPIPLayerSettings.Controls.Add(this.WasApiSpeakersCombo);
      this.tabPageWASAPIPLayerSettings.Controls.Add(this.mpLabel14);
      this.tabPageWASAPIPLayerSettings.Controls.Add(this.WasapiExclusiveModeCkBox);
      this.tabPageWASAPIPLayerSettings.Controls.Add(this.mpLabel5);
      this.tabPageWASAPIPLayerSettings.Location = new System.Drawing.Point(4, 22);
      this.tabPageWASAPIPLayerSettings.Name = "tabPageWASAPIPLayerSettings";
      this.tabPageWASAPIPLayerSettings.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageWASAPIPLayerSettings.Size = new System.Drawing.Size(424, 280);
      this.tabPageWASAPIPLayerSettings.TabIndex = 2;
      this.tabPageWASAPIPLayerSettings.Text = "WASAPI";
      this.tabPageWASAPIPLayerSettings.UseVisualStyleBackColor = true;
      // 
      // mpLabel15
      // 
      this.mpLabel15.AutoSize = true;
      this.mpLabel15.Location = new System.Drawing.Point(24, 129);
      this.mpLabel15.Name = "mpLabel15";
      this.mpLabel15.Size = new System.Drawing.Size(356, 26);
      this.mpLabel15.TabIndex = 6;
      this.mpLabel15.Text = "Note: If automatic speaker detection doesn\'t work, we assume it is Stereo.\r\nYou m" +
    "ay use the above combo to select your speaker settings.";
      // 
      // WasApiSpeakersCombo
      // 
      this.WasApiSpeakersCombo.BorderColor = System.Drawing.Color.Empty;
      this.WasApiSpeakersCombo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.WasApiSpeakersCombo.FormattingEnabled = true;
      this.WasApiSpeakersCombo.Items.AddRange(new object[] {
            "Mono (1 Speaker)",
            "Stereo (2 Speakers)",
            "Quadrophonic (4 Speakers)",
            "5.1 (6 Speakers)",
            "7.1 (8 Speakers)"});
      this.WasApiSpeakersCombo.Location = new System.Drawing.Point(108, 87);
      this.WasApiSpeakersCombo.Name = "WasApiSpeakersCombo";
      this.WasApiSpeakersCombo.Size = new System.Drawing.Size(171, 21);
      this.WasApiSpeakersCombo.TabIndex = 5;
      // 
      // mpLabel14
      // 
      this.mpLabel14.AutoSize = true;
      this.mpLabel14.Location = new System.Drawing.Point(21, 90);
      this.mpLabel14.Name = "mpLabel14";
      this.mpLabel14.Size = new System.Drawing.Size(81, 13);
      this.mpLabel14.TabIndex = 4;
      this.mpLabel14.Text = "Speaker Setup:";
      // 
      // WasapiExclusiveModeCkBox
      // 
      this.WasapiExclusiveModeCkBox.AutoSize = true;
      this.WasapiExclusiveModeCkBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.WasapiExclusiveModeCkBox.Location = new System.Drawing.Point(21, 53);
      this.WasapiExclusiveModeCkBox.Name = "WasapiExclusiveModeCkBox";
      this.WasapiExclusiveModeCkBox.Size = new System.Drawing.Size(166, 17);
      this.WasapiExclusiveModeCkBox.TabIndex = 3;
      this.WasapiExclusiveModeCkBox.Text = "Use WASAPI Exclusive Mode";
      this.WasapiExclusiveModeCkBox.UseVisualStyleBackColor = true;
      // 
      // mpLabel5
      // 
      this.mpLabel5.AutoSize = true;
      this.mpLabel5.Location = new System.Drawing.Point(18, 22);
      this.mpLabel5.Name = "mpLabel5";
      this.mpLabel5.Size = new System.Drawing.Size(349, 13);
      this.mpLabel5.TabIndex = 0;
      this.mpLabel5.Text = "Playback via BASS using Windows Audio Session API (WASAPI) drivers";
      // 
      // tabPagePlayerUpmixSettings
      // 
      this.tabPagePlayerUpmixSettings.Controls.Add(this.mpGroupBox2);
      this.tabPagePlayerUpmixSettings.Location = new System.Drawing.Point(4, 22);
      this.tabPagePlayerUpmixSettings.Name = "tabPagePlayerUpmixSettings";
      this.tabPagePlayerUpmixSettings.Padding = new System.Windows.Forms.Padding(3);
      this.tabPagePlayerUpmixSettings.Size = new System.Drawing.Size(424, 280);
      this.tabPagePlayerUpmixSettings.TabIndex = 3;
      this.tabPagePlayerUpmixSettings.Text = "Upmixing";
      this.tabPagePlayerUpmixSettings.UseVisualStyleBackColor = true;
      // 
      // mpGroupBox2
      // 
      this.mpGroupBox2.Controls.Add(this.cbUpmixFiveDotOne);
      this.mpGroupBox2.Controls.Add(this.mpLabel11);
      this.mpGroupBox2.Controls.Add(this.cbUpmixQuadro);
      this.mpGroupBox2.Controls.Add(this.mpLabel10);
      this.mpGroupBox2.Controls.Add(this.cbUpmixStereo);
      this.mpGroupBox2.Controls.Add(this.mpLabel9);
      this.mpGroupBox2.Controls.Add(this.cbUpmixMono);
      this.mpGroupBox2.Controls.Add(this.mpLabel8);
      this.mpGroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox2.Location = new System.Drawing.Point(6, 17);
      this.mpGroupBox2.Name = "mpGroupBox2";
      this.mpGroupBox2.Size = new System.Drawing.Size(412, 242);
      this.mpGroupBox2.TabIndex = 0;
      this.mpGroupBox2.TabStop = false;
      this.mpGroupBox2.Text = "Upmix source to ...";
      // 
      // cbUpmixFiveDotOne
      // 
      this.cbUpmixFiveDotOne.BorderColor = System.Drawing.Color.Empty;
      this.cbUpmixFiveDotOne.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbUpmixFiveDotOne.FormattingEnabled = true;
      this.cbUpmixFiveDotOne.Location = new System.Drawing.Point(167, 174);
      this.cbUpmixFiveDotOne.Name = "cbUpmixFiveDotOne";
      this.cbUpmixFiveDotOne.Size = new System.Drawing.Size(221, 21);
      this.cbUpmixFiveDotOne.TabIndex = 7;
      // 
      // mpLabel11
      // 
      this.mpLabel11.AutoSize = true;
      this.mpLabel11.Location = new System.Drawing.Point(14, 177);
      this.mpLabel11.Name = "mpLabel11";
      this.mpLabel11.Size = new System.Drawing.Size(69, 13);
      this.mpLabel11.TabIndex = 6;
      this.mpLabel11.Text = "Upmix 5.1 to:";
      // 
      // cbUpmixQuadro
      // 
      this.cbUpmixQuadro.BorderColor = System.Drawing.Color.Empty;
      this.cbUpmixQuadro.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbUpmixQuadro.FormattingEnabled = true;
      this.cbUpmixQuadro.Location = new System.Drawing.Point(167, 131);
      this.cbUpmixQuadro.Name = "cbUpmixQuadro";
      this.cbUpmixQuadro.Size = new System.Drawing.Size(221, 21);
      this.cbUpmixQuadro.TabIndex = 5;
      // 
      // mpLabel10
      // 
      this.mpLabel10.AutoSize = true;
      this.mpLabel10.Location = new System.Drawing.Point(14, 134);
      this.mpLabel10.Name = "mpLabel10";
      this.mpLabel10.Size = new System.Drawing.Size(69, 13);
      this.mpLabel10.TabIndex = 4;
      this.mpLabel10.Text = "Upmix 4.0 to:";
      // 
      // cbUpmixStereo
      // 
      this.cbUpmixStereo.BorderColor = System.Drawing.Color.Empty;
      this.cbUpmixStereo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbUpmixStereo.FormattingEnabled = true;
      this.cbUpmixStereo.Location = new System.Drawing.Point(167, 85);
      this.cbUpmixStereo.Name = "cbUpmixStereo";
      this.cbUpmixStereo.Size = new System.Drawing.Size(221, 21);
      this.cbUpmixStereo.TabIndex = 3;
      // 
      // mpLabel9
      // 
      this.mpLabel9.AutoSize = true;
      this.mpLabel9.Location = new System.Drawing.Point(14, 88);
      this.mpLabel9.Name = "mpLabel9";
      this.mpLabel9.Size = new System.Drawing.Size(85, 13);
      this.mpLabel9.TabIndex = 2;
      this.mpLabel9.Text = "Upmix Stereo to:";
      // 
      // cbUpmixMono
      // 
      this.cbUpmixMono.BorderColor = System.Drawing.Color.Empty;
      this.cbUpmixMono.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbUpmixMono.FormattingEnabled = true;
      this.cbUpmixMono.Location = new System.Drawing.Point(167, 40);
      this.cbUpmixMono.Name = "cbUpmixMono";
      this.cbUpmixMono.Size = new System.Drawing.Size(221, 21);
      this.cbUpmixMono.TabIndex = 1;
      // 
      // mpLabel8
      // 
      this.mpLabel8.AutoSize = true;
      this.mpLabel8.Location = new System.Drawing.Point(14, 43);
      this.mpLabel8.Name = "mpLabel8";
      this.mpLabel8.Size = new System.Drawing.Size(81, 13);
      this.mpLabel8.TabIndex = 0;
      this.mpLabel8.Text = "Upmix Mono to:";
      // 
      // mpGroupBox1
      // 
      this.mpGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.mpGroupBox1.Controls.Add(this.mpLabel2);
      this.mpGroupBox1.Controls.Add(this.soundDeviceComboBox);
      this.mpGroupBox1.Controls.Add(this.label2);
      this.mpGroupBox1.Controls.Add(this.audioPlayerComboBox);
      this.mpGroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox1.Location = new System.Drawing.Point(16, 13);
      this.mpGroupBox1.Name = "mpGroupBox1";
      this.mpGroupBox1.Size = new System.Drawing.Size(432, 85);
      this.mpGroupBox1.TabIndex = 0;
      this.mpGroupBox1.TabStop = false;
      this.mpGroupBox1.Text = "General settings";
      // 
      // mpLabel2
      // 
      this.mpLabel2.AutoSize = true;
      this.mpLabel2.Location = new System.Drawing.Point(7, 54);
      this.mpLabel2.Name = "mpLabel2";
      this.mpLabel2.Size = new System.Drawing.Size(78, 13);
      this.mpLabel2.TabIndex = 4;
      this.mpLabel2.Text = "Sound Device:";
      // 
      // soundDeviceComboBox
      // 
      this.soundDeviceComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.soundDeviceComboBox.BorderColor = System.Drawing.Color.Empty;
      this.soundDeviceComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.soundDeviceComboBox.Location = new System.Drawing.Point(91, 51);
      this.soundDeviceComboBox.Name = "soundDeviceComboBox";
      this.soundDeviceComboBox.Size = new System.Drawing.Size(289, 21);
      this.soundDeviceComboBox.TabIndex = 5;
      this.soundDeviceComboBox.SelectedIndexChanged += new System.EventHandler(this.soundDeviceComboBox_SelectedIndexChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(46, 27);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(39, 13);
      this.label2.TabIndex = 0;
      this.label2.Text = "Player:";
      // 
      // audioPlayerComboBox
      // 
      this.audioPlayerComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.audioPlayerComboBox.BorderColor = System.Drawing.Color.Empty;
      this.audioPlayerComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.audioPlayerComboBox.Location = new System.Drawing.Point(91, 24);
      this.audioPlayerComboBox.Name = "audioPlayerComboBox";
      this.audioPlayerComboBox.Size = new System.Drawing.Size(289, 21);
      this.audioPlayerComboBox.TabIndex = 1;
      this.audioPlayerComboBox.SelectedIndexChanged += new System.EventHandler(this.audioPlayerComboBox_SelectedIndexChanged);
      // 
      // PlaySettingsTabPg
      // 
      this.PlaySettingsTabPg.Controls.Add(this.mpGroupBox4);
      this.PlaySettingsTabPg.Controls.Add(this.groupBox3);
      this.PlaySettingsTabPg.Controls.Add(this.grpSelectOptions);
      this.PlaySettingsTabPg.Location = new System.Drawing.Point(4, 22);
      this.PlaySettingsTabPg.Name = "PlaySettingsTabPg";
      this.PlaySettingsTabPg.Size = new System.Drawing.Size(464, 421);
      this.PlaySettingsTabPg.TabIndex = 3;
      this.PlaySettingsTabPg.Text = "Play Settings";
      this.PlaySettingsTabPg.UseVisualStyleBackColor = true;
      // 
      // mpGroupBox4
      // 
      this.mpGroupBox4.Controls.Add(this.tbResumeSearchValue);
      this.mpGroupBox4.Controls.Add(this.mpLabel13);
      this.mpGroupBox4.Controls.Add(this.cbResumeSelect);
      this.mpGroupBox4.Controls.Add(this.mpLabel12);
      this.mpGroupBox4.Controls.Add(this.mpLabel3);
      this.mpGroupBox4.Controls.Add(this.tbResumeAfter);
      this.mpGroupBox4.Controls.Add(this.chkEnableResumeSupport);
      this.mpGroupBox4.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox4.Location = new System.Drawing.Point(14, 224);
      this.mpGroupBox4.Name = "mpGroupBox4";
      this.mpGroupBox4.Size = new System.Drawing.Size(432, 107);
      this.mpGroupBox4.TabIndex = 8;
      this.mpGroupBox4.TabStop = false;
      this.mpGroupBox4.Text = "Playback Resume";
      // 
      // tbResumeSearchValue
      // 
      this.tbResumeSearchValue.BorderColor = System.Drawing.Color.Empty;
      this.tbResumeSearchValue.Location = new System.Drawing.Point(249, 59);
      this.tbResumeSearchValue.Name = "tbResumeSearchValue";
      this.tbResumeSearchValue.Size = new System.Drawing.Size(168, 20);
      this.tbResumeSearchValue.TabIndex = 6;
      // 
      // mpLabel13
      // 
      this.mpLabel13.AutoSize = true;
      this.mpLabel13.Location = new System.Drawing.Point(196, 61);
      this.mpLabel13.Name = "mpLabel13";
      this.mpLabel13.Size = new System.Drawing.Size(47, 13);
      this.mpLabel13.TabIndex = 5;
      this.mpLabel13.Text = "contains";
      // 
      // cbResumeSelect
      // 
      this.cbResumeSelect.BorderColor = System.Drawing.Color.Empty;
      this.cbResumeSelect.FormattingEnabled = true;
      this.cbResumeSelect.Items.AddRange(new object[] {
            "Genre",
            "Title",
            "Filename",
            "Album",
            "Artist",
            "Albumartist",
            "Composer",
            "Conductor"});
      this.cbResumeSelect.Location = new System.Drawing.Point(76, 58);
      this.cbResumeSelect.Name = "cbResumeSelect";
      this.cbResumeSelect.Size = new System.Drawing.Size(104, 21);
      this.cbResumeSelect.TabIndex = 4;
      // 
      // mpLabel12
      // 
      this.mpLabel12.AutoSize = true;
      this.mpLabel12.Location = new System.Drawing.Point(40, 62);
      this.mpLabel12.Name = "mpLabel12";
      this.mpLabel12.Size = new System.Drawing.Size(30, 13);
      this.mpLabel12.TabIndex = 3;
      this.mpLabel12.Text = "AND";
      // 
      // mpLabel3
      // 
      this.mpLabel3.AutoSize = true;
      this.mpLabel3.Location = new System.Drawing.Point(319, 33);
      this.mpLabel3.Name = "mpLabel3";
      this.mpLabel3.Size = new System.Drawing.Size(47, 13);
      this.mpLabel3.TabIndex = 2;
      this.mpLabel3.Text = "seconds";
      // 
      // tbResumeAfter
      // 
      this.tbResumeAfter.BorderColor = System.Drawing.Color.Empty;
      this.tbResumeAfter.Location = new System.Drawing.Point(274, 29);
      this.tbResumeAfter.MaxLength = 5;
      this.tbResumeAfter.Name = "tbResumeAfter";
      this.tbResumeAfter.Size = new System.Drawing.Size(39, 20);
      this.tbResumeAfter.TabIndex = 1;
      this.tbResumeAfter.Text = "0";
      this.tbResumeAfter.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
      // 
      // chkEnableResumeSupport
      // 
      this.chkEnableResumeSupport.AutoSize = true;
      this.chkEnableResumeSupport.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkEnableResumeSupport.Location = new System.Drawing.Point(19, 29);
      this.chkEnableResumeSupport.Name = "chkEnableResumeSupport";
      this.chkEnableResumeSupport.Size = new System.Drawing.Size(249, 17);
      this.chkEnableResumeSupport.TabIndex = 0;
      this.chkEnableResumeSupport.Text = "Enable Resume Support on Playback position >";
      this.chkEnableResumeSupport.UseVisualStyleBackColor = true;
      // 
      // groupBox3
      // 
      this.groupBox3.Controls.Add(this.PlayNowJumpToCmbBox);
      this.groupBox3.Controls.Add(this.label8);
      this.groupBox3.Location = new System.Drawing.Point(14, 128);
      this.groupBox3.Name = "groupBox3";
      this.groupBox3.Size = new System.Drawing.Size(432, 74);
      this.groupBox3.TabIndex = 7;
      this.groupBox3.TabStop = false;
      this.groupBox3.Text = "Jump To Behaviour";
      // 
      // PlayNowJumpToCmbBox
      // 
      this.PlayNowJumpToCmbBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.PlayNowJumpToCmbBox.FormattingEnabled = true;
      this.PlayNowJumpToCmbBox.Location = new System.Drawing.Point(124, 27);
      this.PlayNowJumpToCmbBox.Name = "PlayNowJumpToCmbBox";
      this.PlayNowJumpToCmbBox.Size = new System.Drawing.Size(293, 21);
      this.PlayNowJumpToCmbBox.TabIndex = 7;
      // 
      // label8
      // 
      this.label8.AutoSize = true;
      this.label8.Location = new System.Drawing.Point(16, 31);
      this.label8.Name = "label8";
      this.label8.Size = new System.Drawing.Size(106, 13);
      this.label8.TabIndex = 6;
      this.label8.Text = "Jump on \"Play now\":";
      this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
      // 
      // grpSelectOptions
      // 
      this.grpSelectOptions.Controls.Add(this.cmbSelectOption);
      this.grpSelectOptions.Controls.Add(this.chkAddAllTracks);
      this.grpSelectOptions.Location = new System.Drawing.Point(14, 17);
      this.grpSelectOptions.Name = "grpSelectOptions";
      this.grpSelectOptions.Size = new System.Drawing.Size(432, 74);
      this.grpSelectOptions.TabIndex = 6;
      this.grpSelectOptions.TabStop = false;
      this.grpSelectOptions.Text = "OK / Enter / Select Button";
      // 
      // cmbSelectOption
      // 
      this.cmbSelectOption.FormattingEnabled = true;
      this.cmbSelectOption.Items.AddRange(new object[] {
            "Play",
            "Queue"});
      this.cmbSelectOption.Location = new System.Drawing.Point(15, 31);
      this.cmbSelectOption.Name = "cmbSelectOption";
      this.cmbSelectOption.Size = new System.Drawing.Size(211, 21);
      this.cmbSelectOption.TabIndex = 3;
      // 
      // chkAddAllTracks
      // 
      this.chkAddAllTracks.AutoSize = true;
      this.chkAddAllTracks.Location = new System.Drawing.Point(259, 33);
      this.chkAddAllTracks.Name = "chkAddAllTracks";
      this.chkAddAllTracks.Size = new System.Drawing.Size(95, 17);
      this.chkAddAllTracks.TabIndex = 4;
      this.chkAddAllTracks.Text = "Add All Tracks";
      this.chkAddAllTracks.UseVisualStyleBackColor = true;
      // 
      // tabPageNowPlaying
      // 
      this.tabPageNowPlaying.Controls.Add(this.groupBoxVUMeter);
      this.tabPageNowPlaying.Controls.Add(this.groupBoxDynamicContent);
      this.tabPageNowPlaying.Location = new System.Drawing.Point(4, 22);
      this.tabPageNowPlaying.Name = "tabPageNowPlaying";
      this.tabPageNowPlaying.Padding = new System.Windows.Forms.Padding(3);
      this.tabPageNowPlaying.Size = new System.Drawing.Size(464, 421);
      this.tabPageNowPlaying.TabIndex = 5;
      this.tabPageNowPlaying.Text = "Now playing";
      this.tabPageNowPlaying.UseVisualStyleBackColor = true;
      // 
      // groupBoxVUMeter
      // 
      this.groupBoxVUMeter.Controls.Add(this.radioButtonVULed);
      this.groupBoxVUMeter.Controls.Add(this.radioButtonVUAnalog);
      this.groupBoxVUMeter.Controls.Add(this.radioButtonVUNone);
      this.groupBoxVUMeter.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.groupBoxVUMeter.Location = new System.Drawing.Point(16, 111);
      this.groupBoxVUMeter.Name = "groupBoxVUMeter";
      this.groupBoxVUMeter.Size = new System.Drawing.Size(432, 64);
      this.groupBoxVUMeter.TabIndex = 5;
      this.groupBoxVUMeter.TabStop = false;
      this.groupBoxVUMeter.Text = "VUMeter (BASS player only)";
      // 
      // radioButtonVULed
      // 
      this.radioButtonVULed.AutoSize = true;
      this.radioButtonVULed.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.radioButtonVULed.Location = new System.Drawing.Point(234, 33);
      this.radioButtonVULed.Name = "radioButtonVULed";
      this.radioButtonVULed.Size = new System.Drawing.Size(45, 17);
      this.radioButtonVULed.TabIndex = 2;
      this.radioButtonVULed.Text = "LED";
      this.radioButtonVULed.UseVisualStyleBackColor = true;
      // 
      // radioButtonVUAnalog
      // 
      this.radioButtonVUAnalog.AutoSize = true;
      this.radioButtonVUAnalog.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.radioButtonVUAnalog.Location = new System.Drawing.Point(119, 33);
      this.radioButtonVUAnalog.Name = "radioButtonVUAnalog";
      this.radioButtonVUAnalog.Size = new System.Drawing.Size(57, 17);
      this.radioButtonVUAnalog.TabIndex = 1;
      this.radioButtonVUAnalog.Text = "Analog";
      this.radioButtonVUAnalog.UseVisualStyleBackColor = true;
      // 
      // radioButtonVUNone
      // 
      this.radioButtonVUNone.AutoSize = true;
      this.radioButtonVUNone.Checked = true;
      this.radioButtonVUNone.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.radioButtonVUNone.Location = new System.Drawing.Point(11, 33);
      this.radioButtonVUNone.Name = "radioButtonVUNone";
      this.radioButtonVUNone.Size = new System.Drawing.Size(50, 17);
      this.radioButtonVUNone.TabIndex = 0;
      this.radioButtonVUNone.TabStop = true;
      this.radioButtonVUNone.Text = "None";
      this.radioButtonVUNone.UseVisualStyleBackColor = true;
      // 
      // groupBoxDynamicContent
      // 
      this.groupBoxDynamicContent.Controls.Add(this.chkDisableSimilarTrackLookups);
      this.groupBoxDynamicContent.Location = new System.Drawing.Point(16, 16);
      this.groupBoxDynamicContent.Name = "groupBoxDynamicContent";
      this.groupBoxDynamicContent.Size = new System.Drawing.Size(432, 86);
      this.groupBoxDynamicContent.TabIndex = 4;
      this.groupBoxDynamicContent.TabStop = false;
      this.groupBoxDynamicContent.Text = "Dynamic content";
      // 
      // chkDisableSimilarTrackLookups
      // 
      this.chkDisableSimilarTrackLookups.AutoSize = true;
      this.chkDisableSimilarTrackLookups.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.chkDisableSimilarTrackLookups.Location = new System.Drawing.Point(11, 25);
      this.chkDisableSimilarTrackLookups.Name = "chkDisableSimilarTrackLookups";
      this.chkDisableSimilarTrackLookups.Size = new System.Drawing.Size(215, 17);
      this.chkDisableSimilarTrackLookups.TabIndex = 10;
      this.chkDisableSimilarTrackLookups.Text = "Disable internet lookups for similar tracks";
      this.chkDisableSimilarTrackLookups.UseVisualStyleBackColor = true;
      // 
      // PlaylistTabPg
      // 
      this.PlaylistTabPg.Controls.Add(this.groupBox1);
      this.PlaylistTabPg.Location = new System.Drawing.Point(4, 22);
      this.PlaylistTabPg.Name = "PlaylistTabPg";
      this.PlaylistTabPg.Size = new System.Drawing.Size(464, 421);
      this.PlaylistTabPg.TabIndex = 2;
      this.PlaylistTabPg.Text = "Playlist settings";
      this.PlaylistTabPg.UseVisualStyleBackColor = true;
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.PlayListUTF8CheckBox);
      this.groupBox1.Controls.Add(this.PlaylistCurrentCheckBox);
      this.groupBox1.Controls.Add(this.autoShuffleCheckBox);
      this.groupBox1.Controls.Add(this.ResumePlaylistChkBox);
      this.groupBox1.Controls.Add(this.SavePlaylistOnExitChkBox);
      this.groupBox1.Controls.Add(this.repeatPlaylistCheckBox);
      this.groupBox1.Controls.Add(this.playlistButton);
      this.groupBox1.Controls.Add(this.playlistFolderTextBox);
      this.groupBox1.Controls.Add(this.label1);
      this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.groupBox1.Location = new System.Drawing.Point(16, 16);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(432, 222);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Playlist settings";
      // 
      // PlayListUTF8CheckBox
      // 
      this.PlayListUTF8CheckBox.AutoSize = true;
      this.PlayListUTF8CheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.PlayListUTF8CheckBox.Location = new System.Drawing.Point(91, 176);
      this.PlayListUTF8CheckBox.Name = "PlayListUTF8CheckBox";
      this.PlayListUTF8CheckBox.Size = new System.Drawing.Size(324, 17);
      this.PlayListUTF8CheckBox.TabIndex = 8;
      this.PlayListUTF8CheckBox.Text = "Save Playlist in UTF8 Format (not compatible with some players)";
      this.PlayListUTF8CheckBox.UseVisualStyleBackColor = true;
      // 
      // PlaylistCurrentCheckBox
      // 
      this.PlaylistCurrentCheckBox.AutoSize = true;
      this.PlaylistCurrentCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.PlaylistCurrentCheckBox.Location = new System.Drawing.Point(91, 153);
      this.PlaylistCurrentCheckBox.Name = "PlaylistCurrentCheckBox";
      this.PlaylistCurrentCheckBox.Size = new System.Drawing.Size(194, 17);
      this.PlaylistCurrentCheckBox.TabIndex = 7;
      this.PlaylistCurrentCheckBox.Text = "Playlist screen shows current playlist";
      this.PlaylistCurrentCheckBox.UseVisualStyleBackColor = true;
      // 
      // autoShuffleCheckBox
      // 
      this.autoShuffleCheckBox.AutoSize = true;
      this.autoShuffleCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.autoShuffleCheckBox.Location = new System.Drawing.Point(91, 84);
      this.autoShuffleCheckBox.Name = "autoShuffleCheckBox";
      this.autoShuffleCheckBox.Size = new System.Drawing.Size(180, 17);
      this.autoShuffleCheckBox.TabIndex = 4;
      this.autoShuffleCheckBox.Text = "Auto shuffle playlists after loading";
      this.autoShuffleCheckBox.UseVisualStyleBackColor = true;
      // 
      // ResumePlaylistChkBox
      // 
      this.ResumePlaylistChkBox.AutoSize = true;
      this.ResumePlaylistChkBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.ResumePlaylistChkBox.Location = new System.Drawing.Point(91, 130);
      this.ResumePlaylistChkBox.Name = "ResumePlaylistChkBox";
      this.ResumePlaylistChkBox.Size = new System.Drawing.Size(229, 17);
      this.ResumePlaylistChkBox.TabIndex = 5;
      this.ResumePlaylistChkBox.Text = "Load default playlist on MediaPortal startup ";
      this.ResumePlaylistChkBox.UseVisualStyleBackColor = true;
      // 
      // SavePlaylistOnExitChkBox
      // 
      this.SavePlaylistOnExitChkBox.AutoSize = true;
      this.SavePlaylistOnExitChkBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.SavePlaylistOnExitChkBox.Location = new System.Drawing.Point(91, 107);
      this.SavePlaylistOnExitChkBox.Name = "SavePlaylistOnExitChkBox";
      this.SavePlaylistOnExitChkBox.Size = new System.Drawing.Size(293, 17);
      this.SavePlaylistOnExitChkBox.TabIndex = 5;
      this.SavePlaylistOnExitChkBox.Text = "Save current playlist as default when leaving MediaPortal";
      this.SavePlaylistOnExitChkBox.UseVisualStyleBackColor = true;
      // 
      // repeatPlaylistCheckBox
      // 
      this.repeatPlaylistCheckBox.AutoSize = true;
      this.repeatPlaylistCheckBox.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.repeatPlaylistCheckBox.Location = new System.Drawing.Point(91, 61);
      this.repeatPlaylistCheckBox.Name = "repeatPlaylistCheckBox";
      this.repeatPlaylistCheckBox.Size = new System.Drawing.Size(219, 17);
      this.repeatPlaylistCheckBox.TabIndex = 3;
      this.repeatPlaylistCheckBox.Text = "Repeat/loop music playlists (m3u, b4, pls)";
      this.repeatPlaylistCheckBox.UseVisualStyleBackColor = true;
      // 
      // playlistButton
      // 
      this.playlistButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.playlistButton.Location = new System.Drawing.Point(365, 22);
      this.playlistButton.Name = "playlistButton";
      this.playlistButton.Size = new System.Drawing.Size(61, 22);
      this.playlistButton.TabIndex = 2;
      this.playlistButton.Text = "Browse";
      this.playlistButton.UseVisualStyleBackColor = true;
      this.playlistButton.Click += new System.EventHandler(this.playlistButton_Click);
      // 
      // playlistFolderTextBox
      // 
      this.playlistFolderTextBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.playlistFolderTextBox.BorderColor = System.Drawing.Color.Empty;
      this.playlistFolderTextBox.Location = new System.Drawing.Point(91, 24);
      this.playlistFolderTextBox.Name = "playlistFolderTextBox";
      this.playlistFolderTextBox.Size = new System.Drawing.Size(268, 20);
      this.playlistFolderTextBox.TabIndex = 1;
      // 
      // label1
      // 
      this.label1.AutoSize = true;
      this.label1.Location = new System.Drawing.Point(14, 27);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(71, 13);
      this.label1.TabIndex = 0;
      this.label1.Text = "Playlist folder:";
      // 
      // label4
      // 
      this.label4.Location = new System.Drawing.Point(0, 0);
      this.label4.Name = "label4";
      this.label4.Size = new System.Drawing.Size(100, 23);
      this.label4.TabIndex = 0;
      // 
      // checkBox2
      // 
      this.checkBox2.AutoSize = true;
      this.checkBox2.Location = new System.Drawing.Point(259, 21);
      this.checkBox2.Name = "checkBox2";
      this.checkBox2.Size = new System.Drawing.Size(95, 17);
      this.checkBox2.TabIndex = 6;
      this.checkBox2.Text = "Add All Tracks";
      this.checkBox2.UseVisualStyleBackColor = true;
      // 
      // Music
      // 
      this.Controls.Add(this.MusicSettingsTabCtl);
      this.Name = "Music";
      this.Size = new System.Drawing.Size(472, 455);
      this.MusicSettingsTabCtl.ResumeLayout(false);
      this.PlayerTabPg.ResumeLayout(false);
      this.tabControlPlayerSettings.ResumeLayout(false);
      this.tabPageBassPlayerSettings.ResumeLayout(false);
      this.tabPageBassPlayerSettings.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarCrossfade)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.trackBarBuffering)).EndInit();
      ((System.ComponentModel.ISupportInitialize)(this.StreamOutputLevelNud)).EndInit();
      this.tabPageASIOPlayerSettings.ResumeLayout(false);
      this.tabPageASIOPlayerSettings.PerformLayout();
      this.tabPageWASAPIPLayerSettings.ResumeLayout(false);
      this.tabPageWASAPIPLayerSettings.PerformLayout();
      this.tabPagePlayerUpmixSettings.ResumeLayout(false);
      this.mpGroupBox2.ResumeLayout(false);
      this.mpGroupBox2.PerformLayout();
      this.mpGroupBox1.ResumeLayout(false);
      this.mpGroupBox1.PerformLayout();
      this.PlaySettingsTabPg.ResumeLayout(false);
      this.mpGroupBox4.ResumeLayout(false);
      this.mpGroupBox4.PerformLayout();
      this.groupBox3.ResumeLayout(false);
      this.groupBox3.PerformLayout();
      this.grpSelectOptions.ResumeLayout(false);
      this.grpSelectOptions.PerformLayout();
      this.tabPageNowPlaying.ResumeLayout(false);
      this.groupBoxVUMeter.ResumeLayout(false);
      this.groupBoxVUMeter.PerformLayout();
      this.groupBoxDynamicContent.ResumeLayout(false);
      this.groupBoxDynamicContent.PerformLayout();
      this.PlaylistTabPg.ResumeLayout(false);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private FolderBrowserDialog folderBrowserDialog;
    private MPLabel label4;
    private MPTabControl MusicSettingsTabCtl;
    private TabPage PlayerTabPg;
    private Label BufferingSecondsLbl;
    private Label CrossFadeSecondsLbl;
    private NumericUpDown StreamOutputLevelNud;
    private CheckBox FadeOnStartStopChkbox;
    private Label label12;
    private MPLabel mpLabel1;
    private MPLabel CrossFadingLbl;
    private MPGroupBox mpGroupBox1;
    private MPLabel label2;
    private MPComboBox audioPlayerComboBox;
    private TabPage PlaylistTabPg;
    private MPGroupBox groupBox1;
    private MPCheckBox autoShuffleCheckBox;
    private MPCheckBox ResumePlaylistChkBox;
    private MPCheckBox SavePlaylistOnExitChkBox;
    private MPCheckBox repeatPlaylistCheckBox;
    private MPButton playlistButton;
    private MPTextBox playlistFolderTextBox;
    private MPLabel label1;
    private TabPage PlaySettingsTabPg;
    private CheckBox GaplessPlaybackChkBox;
    private TabPage tabPageNowPlaying;
    private GroupBox groupBoxDynamicContent;
    private MPCheckBox chkDisableSimilarTrackLookups;
    private MPLabel mpLabel2;
    private MPComboBox soundDeviceComboBox;
    private MPGroupBox groupBoxVUMeter;
    private MPRadioButton radioButtonVUAnalog;
    private MPRadioButton radioButtonVUNone;
    private MPRadioButton radioButtonVULed;
    private CheckBox chkAddAllTracks;
    private ComboBox cmbSelectOption;
    private GroupBox grpSelectOptions;
    private GroupBox groupBox3;
    private ComboBox PlayNowJumpToCmbBox;
    private Label label8;
    private CheckBox checkBox2;
    private MPCheckBox PlaylistCurrentCheckBox;
    private MPTabControl tabControlPlayerSettings;
    private TabPage tabPageBassPlayerSettings;
    private TabPage tabPageASIOPlayerSettings;
    private TabPage tabPageWASAPIPLayerSettings;
    private MPButton btAsioDeviceSettings;
    private MPLabel mpLabel4;
    private MPLabel mpLabel5;
    private MPLabel lbBalance;
    private HScrollBar hScrollBarBalance;
    private MPLabel mpLabel6;
    private MPLabel mpLabel7;
    private CheckBox WasapiExclusiveModeCkBox;
    private TabPage tabPagePlayerUpmixSettings;
    private MPGroupBox mpGroupBox2;
    private MPComboBox cbUpmixFiveDotOne;
    private MPLabel mpLabel11;
    private MPComboBox cbUpmixQuadro;
    private MPLabel mpLabel10;
    private MPComboBox cbUpmixStereo;
    private MPLabel mpLabel9;
    private MPComboBox cbUpmixMono;
    private MPLabel mpLabel8;
    private CheckBox UseSkipStepsCheckBox;
    private TrackBar trackBarBuffering;
    private TrackBar trackBarCrossfade;
    private CheckBox EnableAlbumReplayGainChkBox;
    private CheckBox EnableReplayGainChkBox;
    private MPGroupBox mpGroupBox4;
    private MPLabel mpLabel3;
    private MPTextBox tbResumeAfter;
    private MPCheckBox chkEnableResumeSupport;
    private MPTextBox tbResumeSearchValue;
    private MPLabel mpLabel13;
    private MPComboBox cbResumeSelect;
    private MPLabel mpLabel12;
    private MPComboBox WasApiSpeakersCombo;
    private MPLabel mpLabel14;
    private MPLabel mpLabel15;
    private MPCheckBox PlayListUTF8CheckBox;
  }
}
