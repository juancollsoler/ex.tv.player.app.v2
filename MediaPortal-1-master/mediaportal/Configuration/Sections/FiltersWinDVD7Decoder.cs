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
using System.ComponentModel;
using MediaPortal.Profile;
using MediaPortal.UserInterface.Controls;
using Microsoft.Win32;

#pragma warning disable 108

namespace MediaPortal.Configuration.Sections
{
  public class FiltersWinDVD7Decoder : SectionSettings
  {
    private MPCheckBox checkBoxDxVA;
    private MPCheckBox checkBoxHWMC;
    private MPComboBox comboBoxDeInterlace;
    private MPGroupBox mpGroupBox1;
    private MPLabel mpLabel1;
    private MPGroupBox mpGroupBox2;
    private MPComboBox comboBoxSpeakerConfig;
    private MPComboBox comboBoxOutPutMode;
    private MPLabel mpLabel3;
    private MPLabel mpLabel2;
    private MPLabel mpLabel4;
    private IContainer components = null;

    /// <summary>
    /// 
    /// </summary>
    public FiltersWinDVD7Decoder()
      : this("WinDVD 7 Decoder") {}

    /// <summary>
    /// 
    /// </summary>
    public FiltersWinDVD7Decoder(string name)
      : base(name)
    {
      // This call is required by the Windows Form Designer.
      InitializeComponent();
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

    #region Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.checkBoxDxVA = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.checkBoxHWMC = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.comboBoxDeInterlace = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.mpGroupBox1 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.mpLabel4 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel1 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpGroupBox2 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.mpLabel3 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpLabel2 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.comboBoxOutPutMode = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.comboBoxSpeakerConfig = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.mpGroupBox1.SuspendLayout();
      this.mpGroupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // checkBoxDxVA
      // 
      this.checkBoxDxVA.AutoSize = true;
      this.checkBoxDxVA.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.checkBoxDxVA.Location = new System.Drawing.Point(62, 25);
      this.checkBoxDxVA.Name = "checkBoxDxVA";
      this.checkBoxDxVA.Size = new System.Drawing.Size(87, 17);
      this.checkBoxDxVA.TabIndex = 2;
      this.checkBoxDxVA.Text = "Enable DxVA";
      this.checkBoxDxVA.UseVisualStyleBackColor = true;
      // 
      // checkBoxHWMC
      // 
      this.checkBoxHWMC.AutoSize = true;
      this.checkBoxHWMC.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.checkBoxHWMC.Location = new System.Drawing.Point(62, 48);
      this.checkBoxHWMC.Name = "checkBoxHWMC";
      this.checkBoxHWMC.Size = new System.Drawing.Size(95, 17);
      this.checkBoxHWMC.TabIndex = 3;
      this.checkBoxHWMC.Text = "Enable HWMC";
      this.checkBoxHWMC.UseVisualStyleBackColor = true;
      // 
      // comboBoxDeInterlace
      // 
      this.comboBoxDeInterlace.BorderColor = System.Drawing.Color.Empty;
      this.comboBoxDeInterlace.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxDeInterlace.FormattingEnabled = true;
      this.comboBoxDeInterlace.Items.AddRange(new object[]
                                                {
                                                  "Auto",
                                                  "Bob",
                                                  "Weave",
                                                  "Progressive"
                                                });
      this.comboBoxDeInterlace.Location = new System.Drawing.Point(62, 102);
      this.comboBoxDeInterlace.Name = "comboBoxDeInterlace";
      this.comboBoxDeInterlace.Size = new System.Drawing.Size(125, 21);
      this.comboBoxDeInterlace.TabIndex = 4;
      // 
      // mpGroupBox1
      // 
      this.mpGroupBox1.Controls.Add(this.mpLabel4);
      this.mpGroupBox1.Controls.Add(this.mpLabel1);
      this.mpGroupBox1.Controls.Add(this.checkBoxHWMC);
      this.mpGroupBox1.Controls.Add(this.comboBoxDeInterlace);
      this.mpGroupBox1.Controls.Add(this.checkBoxDxVA);
      this.mpGroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox1.Location = new System.Drawing.Point(6, 0);
      this.mpGroupBox1.Name = "mpGroupBox1";
      this.mpGroupBox1.Size = new System.Drawing.Size(462, 143);
      this.mpGroupBox1.TabIndex = 5;
      this.mpGroupBox1.TabStop = false;
      this.mpGroupBox1.Text = "Video Decoder Settings";
      // 
      // mpLabel4
      // 
      this.mpLabel4.AutoSize = true;
      this.mpLabel4.Location = new System.Drawing.Point(182, 25);
      this.mpLabel4.MaximumSize = new System.Drawing.Size(240, 0);
      this.mpLabel4.Name = "mpLabel4";
      this.mpLabel4.RightToLeft = System.Windows.Forms.RightToLeft.No;
      this.mpLabel4.Size = new System.Drawing.Size(224, 39);
      this.mpLabel4.TabIndex = 6;
      this.mpLabel4.Text = "It is recommended to enable these options for better CPU utlization. If DxVA is e" +
                           "nabled, De-Interlacing is done using VMR9.";
      // 
      // mpLabel1
      // 
      this.mpLabel1.AutoSize = true;
      this.mpLabel1.Location = new System.Drawing.Point(7, 77);
      this.mpLabel1.Name = "mpLabel1";
      this.mpLabel1.Size = new System.Drawing.Size(106, 13);
      this.mpLabel1.TabIndex = 5;
      this.mpLabel1.Text = "De-Interlace Settings";
      // 
      // mpGroupBox2
      // 
      this.mpGroupBox2.Anchor =
        ((System.Windows.Forms.AnchorStyles)
         (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
      this.mpGroupBox2.Controls.Add(this.mpLabel3);
      this.mpGroupBox2.Controls.Add(this.mpLabel2);
      this.mpGroupBox2.Controls.Add(this.comboBoxOutPutMode);
      this.mpGroupBox2.Controls.Add(this.comboBoxSpeakerConfig);
      this.mpGroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox2.Location = new System.Drawing.Point(6, 152);
      this.mpGroupBox2.Name = "mpGroupBox2";
      this.mpGroupBox2.Size = new System.Drawing.Size(462, 166);
      this.mpGroupBox2.TabIndex = 0;
      this.mpGroupBox2.TabStop = false;
      this.mpGroupBox2.Text = "Audio Decoder Settings";
      // 
      // mpLabel3
      // 
      this.mpLabel3.AutoSize = true;
      this.mpLabel3.Location = new System.Drawing.Point(7, 96);
      this.mpLabel3.Name = "mpLabel3";
      this.mpLabel3.Size = new System.Drawing.Size(69, 13);
      this.mpLabel3.TabIndex = 3;
      this.mpLabel3.Text = "Output Mode";
      // 
      // mpLabel2
      // 
      this.mpLabel2.AutoSize = true;
      this.mpLabel2.Location = new System.Drawing.Point(7, 32);
      this.mpLabel2.Name = "mpLabel2";
      this.mpLabel2.Size = new System.Drawing.Size(88, 13);
      this.mpLabel2.TabIndex = 2;
      this.mpLabel2.Text = "Speaker Settings";
      // 
      // comboBoxOutPutMode
      // 
      this.comboBoxOutPutMode.Anchor =
        ((System.Windows.Forms.AnchorStyles)
         (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxOutPutMode.BorderColor = System.Drawing.Color.Empty;
      this.comboBoxOutPutMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxOutPutMode.DropDownWidth = 372;
      this.comboBoxOutPutMode.Location = new System.Drawing.Point(62, 123);
      this.comboBoxOutPutMode.Name = "comboBoxOutPutMode";
      this.comboBoxOutPutMode.Size = new System.Drawing.Size(362, 21);
      this.comboBoxOutPutMode.TabIndex = 0;
      // 
      // comboBoxSpeakerConfig
      // 
      this.comboBoxSpeakerConfig.Anchor =
        ((System.Windows.Forms.AnchorStyles)
         (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
      this.comboBoxSpeakerConfig.BorderColor = System.Drawing.Color.Empty;
      this.comboBoxSpeakerConfig.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.comboBoxSpeakerConfig.Items.AddRange(new object[]
                                                  {
                                                    "2 Speaker",
                                                    "4 Speaker",
                                                    "6 Speaker (5.1)",
                                                    "7 Speaker (6.1)",
                                                    "8 Speaker (7.1)",
                                                    "S/PDIF Out"
                                                  });
      this.comboBoxSpeakerConfig.Location = new System.Drawing.Point(62, 57);
      this.comboBoxSpeakerConfig.Name = "comboBoxSpeakerConfig";
      this.comboBoxSpeakerConfig.Size = new System.Drawing.Size(362, 21);
      this.comboBoxSpeakerConfig.TabIndex = 1;
      this.comboBoxSpeakerConfig.SelectedIndexChanged +=
        new System.EventHandler(this.comboBoxSpeakerConfig_SelectedIndexChanged);
      // 
      // FiltersWinDVD7Decoder
      // 
      this.Controls.Add(this.mpGroupBox2);
      this.Controls.Add(this.mpGroupBox1);
      this.Name = "FiltersWinDVD7Decoder";
      this.Size = new System.Drawing.Size(472, 408);
      this.mpGroupBox1.ResumeLayout(false);
      this.mpGroupBox1.PerformLayout();
      this.mpGroupBox2.ResumeLayout(false);
      this.mpGroupBox2.PerformLayout();
      this.ResumeLayout(false);
    }

    #endregion

    public override void LoadSettings()
    {
      int iIndex = 0;
      Int32 regAUDIO;
      Int32 regAUDIOCHAN;

      using (RegistryKey subkey = Registry.CurrentUser.CreateSubKey(@"Software\InterVideo\Common\AudioDec\MediaPortal"))
      {
        if (subkey != null)
        {
          try
          {
            regAUDIOCHAN = (Int32)subkey.GetValue("AUDIOCHAN", 1);
            switch (regAUDIOCHAN)
            {
                //2 Speaker (comboBoxSpeakerConfig index 0)
              case 1:

                #region 2 Speaker

                comboBoxSpeakerConfig.SelectedIndex = 0;
                comboBoxOutPutMode.Items.Clear();
                comboBoxOutPutMode.Enabled = true;
                comboBoxOutPutMode.Items.AddRange(new object[]
                                                    {
                                                      "Mono",
                                                      "Stereo",
                                                      "Dolby Surround Capable"
                                                    });
                regAUDIO = (Int32)subkey.GetValue("AUDIO", 1);
                if (regAUDIO == 0)
                {
                  iIndex = 0;
                }
                if (regAUDIO == 1)
                {
                  iIndex = 1;
                }
                if (regAUDIO == 2)
                {
                  iIndex = 2;
                }
                comboBoxOutPutMode.SelectedIndex = iIndex;

                #endregion

                break;
                //4 Speaker (comboBoxSpeakerConfig index 1)
              case 3:

                #region 4 Speaker

                comboBoxSpeakerConfig.SelectedIndex = 1;
                comboBoxOutPutMode.Items.Clear();
                comboBoxOutPutMode.Enabled = false;

                #endregion

                break;
                //6 Speaker (comboBoxSpeakerConfig index 2)
              case 4:

                #region 6 Speaker (5.1)

                comboBoxSpeakerConfig.SelectedIndex = 2;
                comboBoxOutPutMode.Items.Clear();
                comboBoxOutPutMode.Enabled = false;

                #endregion

                break;
                //7 Speaker (comboBoxSpeakerConfig index 3)
              case 5:

                #region 7 Speaker (6.1)

                comboBoxSpeakerConfig.SelectedIndex = 3;
                comboBoxOutPutMode.Items.Clear();
                comboBoxOutPutMode.Enabled = false;

                #endregion

                break;
                //8 Speaker (comboBoxSpeakerConfig index 4)
              case 8:

                #region 8 Speaker (7.1)

                comboBoxSpeakerConfig.SelectedIndex = 4;
                comboBoxOutPutMode.Items.Clear();
                comboBoxOutPutMode.Enabled = false;

                #endregion

                break;
                //SPDIF Out (comboBoxSpeakerConfig index 5)
              case 6:

                #region S/PDIF Out

                comboBoxSpeakerConfig.SelectedIndex = 5;
                comboBoxOutPutMode.Items.Clear();
                comboBoxOutPutMode.Enabled = false;

                #endregion

                break;
            }
          }
          catch (Exception) {}
        }
      }

      using (RegistryKey subkey = Registry.CurrentUser.CreateSubKey(@"Software\InterVideo\Common\VideoDec\MediaPortal"))
      {
        if (subkey != null)
        {
          try
          {
            Int32 regDxVA = (Int32)subkey.GetValue("DXVA", 1);
            if (regDxVA == 1)
            {
              checkBoxDxVA.Checked = true;
            }
            else
            {
              checkBoxDxVA.Checked = false;
            }

            Int32 regHWMC = (Int32)subkey.GetValue("HWMC", 1);
            if (regHWMC == 1)
            {
              checkBoxHWMC.Checked = true;
            }
            else
            {
              checkBoxHWMC.Checked = false;
            }

            Int32 regBOBWEAVE = (Int32)subkey.GetValue("BOBWEAVE", 0);
            if (regBOBWEAVE == 0)
            {
              comboBoxDeInterlace.SelectedIndex = 0;
            }
            if (regBOBWEAVE == 1)
            {
              comboBoxDeInterlace.SelectedIndex = 1;
            }
            if (regBOBWEAVE == 2)
            {
              comboBoxDeInterlace.SelectedIndex = 2;
            }
            if (regBOBWEAVE == 3)
            {
              comboBoxDeInterlace.SelectedIndex = 3;
            }
            else
            {
              comboBoxDeInterlace.SelectedIndex = 0;
            }
          }
          catch (Exception) {}
        }
      }
    }

    public override void SaveSettings()
    {
      Int32 regValue;
      int OutPutModeIndex;

      using (RegistryKey subkey = Registry.CurrentUser.CreateSubKey(@"Software\InterVideo\Common\AudioDec\MediaPortal"))
      {
        if (subkey != null)
        {
          switch (comboBoxSpeakerConfig.SelectedIndex)
          {
              //2 Speakers
            case 0:

              #region 2 Speaker

              regValue = 1;
              subkey.SetValue("AUDIOCHAN", regValue);
              OutPutModeIndex = comboBoxOutPutMode.SelectedIndex;
              if (OutPutModeIndex == 0)
              {
                regValue = 0;
                subkey.SetValue("AUDIO", regValue);
              }
              if (OutPutModeIndex == 1)
              {
                regValue = 1;
                subkey.SetValue("AUDIO", regValue);
              }
              if (OutPutModeIndex == 2)
              {
                regValue = 2;
                subkey.SetValue("AUDIO", regValue);
              }

              #endregion

              break;
              //4 Speakers
            case 1:

              #region 4 Speaker

              regValue = 3;
              subkey.SetValue("AUDIOCHAN", regValue);
              regValue = 4;
              subkey.SetValue("AUDIO", regValue);

              #endregion

              break;
              //6 Speakers
            case 2:

              #region 6 Speaker (5.1)

              regValue = 4;
              subkey.SetValue("AUDIOCHAN", regValue);
              regValue = 5;
              subkey.SetValue("AUDIO", regValue);

              #endregion

              break;
              //7 Speakers
            case 3:

              #region 7 Speaker (6.1)

              regValue = 5;
              subkey.SetValue("AUDIOCHAN", regValue);
              regValue = 6;
              subkey.SetValue("AUDIO", regValue);

              #endregion

              break;
              //8 Speakers
            case 4:

              #region 8 Speaker (7.1)

              regValue = 8;
              subkey.SetValue("AUDIOCHAN", regValue);
              regValue = 7;
              subkey.SetValue("AUDIO", regValue);

              #endregion

              break;
              // SPDIF Out 
            case 5:

              #region S/PDIF Out

              regValue = 6;
              subkey.SetValue("AUDIOCHAN", regValue);
              regValue = 7;
              subkey.SetValue("AUDIO", regValue);

              #endregion

              break;
          }
        }
      }

      using (RegistryKey subkey = Registry.CurrentUser.CreateSubKey(@"Software\InterVideo\Common\VideoDec\MediaPortal"))
      {
        if (subkey != null)
        {
          Int32 regDxVA;
          if (checkBoxDxVA.Checked)
          {
            regDxVA = 1;
          }
          else
          {
            regDxVA = 0;
          }
          subkey.SetValue("DXVA", regDxVA);
          using (Settings xmlwriter = new MPSettings())
          {
            xmlwriter.SetValue("videocodec", "intervideo", regDxVA);
          }

          Int32 regHWMC;
          if (checkBoxHWMC.Checked)
          {
            regHWMC = 1;
          }
          else
          {
            regHWMC = 0;
          }
          subkey.SetValue("HWMC", regHWMC);

          int DeInterlace;
          Int32 regDeInterlace;
          DeInterlace = comboBoxDeInterlace.SelectedIndex;
          if (DeInterlace == 0)
          {
            regDeInterlace = 0;
            subkey.SetValue("BOBWEAVE", regDeInterlace);
          }
          if (DeInterlace == 1)
          {
            regDeInterlace = 1;
            subkey.SetValue("BOBWEAVE", regDeInterlace);
          }
          if (DeInterlace == 2)
          {
            regDeInterlace = 2;
            subkey.SetValue("BOBWEAVE", regDeInterlace);
          }
          if (DeInterlace == 3)
          {
            regDeInterlace = 3;
            subkey.SetValue("BOBWEAVE", regDeInterlace);
          }
        }
      }
    }

    private void comboBoxSpeakerConfig_SelectedIndexChanged(object sender, EventArgs e)
    {
      switch (comboBoxSpeakerConfig.SelectedIndex)
      {
          //2 Speaker
        case 0:

          #region 2 Speaker

          comboBoxOutPutMode.Items.Clear();
          comboBoxOutPutMode.Enabled = true;
          comboBoxOutPutMode.Items.AddRange(new object[]
                                              {
                                                "Mono",
                                                "Stereo",
                                                "Dolby Surround Capable"
                                              });

          #endregion

          break;
          //4, 6, 7, 8 Speaker & SPDIF
        case 1:
        case 2:
        case 3:
        case 4:
        case 5:
          comboBoxOutPutMode.Items.Clear();
          comboBoxOutPutMode.Enabled = false;
          break;
      }
    }
  }
}