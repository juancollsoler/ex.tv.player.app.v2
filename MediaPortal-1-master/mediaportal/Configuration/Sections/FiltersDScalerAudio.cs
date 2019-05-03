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
using MediaPortal.UserInterface.Controls;
using Microsoft.Win32;

#pragma warning disable 108

namespace MediaPortal.Configuration.Sections
{
  public class FiltersDScalerAudio : SectionSettings
  {
    private MPGroupBox groupBox1;
    private MPLabel label3;
    private MPLabel label5;
    private MPCheckBox checkBoxSPDIF;
    private MPComboBox comboBoxSpeakerConfig;
    private MPCheckBox checkBoxDynamicRange;
    private MPTextBox textBoxAudioOffset;
    private MPCheckBox checkBoxMPEGOverSPDIF;
    private IContainer components = null;

    /// <summary>
    /// 
    /// </summary>
    public FiltersDScalerAudio()
      : this("DScaler Audio Decoder") {}

    /// <summary>
    /// 
    /// </summary>
    public FiltersDScalerAudio(string name)
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
      this.groupBox1 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.textBoxAudioOffset = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.comboBoxSpeakerConfig = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.checkBoxMPEGOverSPDIF = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.label5 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.checkBoxSPDIF = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.checkBoxDynamicRange = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.label3 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor =
        ((System.Windows.Forms.AnchorStyles)
         (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.textBoxAudioOffset);
      this.groupBox1.Controls.Add(this.comboBoxSpeakerConfig);
      this.groupBox1.Controls.Add(this.checkBoxMPEGOverSPDIF);
      this.groupBox1.Controls.Add(this.label5);
      this.groupBox1.Controls.Add(this.checkBoxSPDIF);
      this.groupBox1.Controls.Add(this.checkBoxDynamicRange);
      this.groupBox1.Controls.Add(this.label3);
      this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.groupBox1.Location = new System.Drawing.Point(6, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(462, 168);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Settings";
      // 
      // textBoxAudioOffset
      // 
      this.textBoxAudioOffset.Anchor =
        ((System.Windows.Forms.AnchorStyles)
         (((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
           | System.Windows.Forms.AnchorStyles.Right)));
      this.textBoxAudioOffset.BorderColor = System.Drawing.Color.Empty;
      this.textBoxAudioOffset.Location = new System.Drawing.Point(168, 132);
      this.textBoxAudioOffset.Name = "textBoxAudioOffset";
      this.textBoxAudioOffset.Size = new System.Drawing.Size(278, 20);
      this.textBoxAudioOffset.TabIndex = 6;
      this.textBoxAudioOffset.Text = "0";
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
                                                    "Stereo",
                                                    "Dolby Stereo",
                                                    "4.0 (2 Front + 2 Rear)",
                                                    "4.1 (2 Front + 2 Rear + 1 Sub)",
                                                    "5.0 (3 Front + 2 Rear)",
                                                    "5.1 (3 Front + 2 Rear + 1 Sub)"
                                                  });
      this.comboBoxSpeakerConfig.Location = new System.Drawing.Point(168, 20);
      this.comboBoxSpeakerConfig.Name = "comboBoxSpeakerConfig";
      this.comboBoxSpeakerConfig.Size = new System.Drawing.Size(278, 21);
      this.comboBoxSpeakerConfig.TabIndex = 1;
      // 
      // checkBoxMPEGOverSPDIF
      // 
      this.checkBoxMPEGOverSPDIF.AutoSize = true;
      this.checkBoxMPEGOverSPDIF.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.checkBoxMPEGOverSPDIF.Location = new System.Drawing.Point(16, 104);
      this.checkBoxMPEGOverSPDIF.Name = "checkBoxMPEGOverSPDIF";
      this.checkBoxMPEGOverSPDIF.Size = new System.Drawing.Size(148, 17);
      this.checkBoxMPEGOverSPDIF.TabIndex = 4;
      this.checkBoxMPEGOverSPDIF.Text = "MPEG Audio over S/PDIF";
      this.checkBoxMPEGOverSPDIF.UseVisualStyleBackColor = true;
      // 
      // label5
      // 
      this.label5.Location = new System.Drawing.Point(16, 136);
      this.label5.Name = "label5";
      this.label5.Size = new System.Drawing.Size(152, 16);
      this.label5.TabIndex = 5;
      this.label5.Text = "S/PDIF delay offset (msec.):";
      // 
      // checkBoxSPDIF
      // 
      this.checkBoxSPDIF.AutoSize = true;
      this.checkBoxSPDIF.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.checkBoxSPDIF.Location = new System.Drawing.Point(16, 56);
      this.checkBoxSPDIF.Name = "checkBoxSPDIF";
      this.checkBoxSPDIF.Size = new System.Drawing.Size(147, 17);
      this.checkBoxSPDIF.TabIndex = 2;
      this.checkBoxSPDIF.Text = "Use S/PDIF for AC3/DTS";
      this.checkBoxSPDIF.UseVisualStyleBackColor = true;
      // 
      // checkBoxDynamicRange
      // 
      this.checkBoxDynamicRange.AutoSize = true;
      this.checkBoxDynamicRange.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.checkBoxDynamicRange.Location = new System.Drawing.Point(16, 80);
      this.checkBoxDynamicRange.Name = "checkBoxDynamicRange";
      this.checkBoxDynamicRange.Size = new System.Drawing.Size(136, 17);
      this.checkBoxDynamicRange.TabIndex = 3;
      this.checkBoxDynamicRange.Text = "Dynamic Range Control";
      this.checkBoxDynamicRange.UseVisualStyleBackColor = true;
      // 
      // label3
      // 
      this.label3.Location = new System.Drawing.Point(16, 24);
      this.label3.Name = "label3";
      this.label3.Size = new System.Drawing.Size(88, 16);
      this.label3.TabIndex = 0;
      this.label3.Text = "Speaker config:";
      // 
      // FiltersDScalerAudio
      // 
      this.Controls.Add(this.groupBox1);
      this.Name = "FiltersDScalerAudio";
      this.Size = new System.Drawing.Size(472, 408);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.ResumeLayout(false);
    }

    #endregion

    public override void LoadSettings()
    {
      using (RegistryKey subkey = Registry.CurrentUser.CreateSubKey(@"Software\DScaler5\Mpeg Audio Filter"))
      {
        if (subkey != null)
        {
          try
          {
            Int32 regValue = (Int32)subkey.GetValue("Dynamic Range Control");
            if (regValue == 1)
            {
              checkBoxDynamicRange.Checked = true;
            }
            else
            {
              checkBoxDynamicRange.Checked = false;
            }

            regValue = (Int32)subkey.GetValue("MPEG Audio over SPDIF");
            if (regValue == 1)
            {
              checkBoxMPEGOverSPDIF.Checked = true;
            }
            else
            {
              checkBoxMPEGOverSPDIF.Checked = false;
            }

            regValue = (Int32)subkey.GetValue("Use SPDIF for AC3 & DTS");
            if (regValue == 1)
            {
              checkBoxSPDIF.Checked = true;
            }
            else
            {
              checkBoxSPDIF.Checked = false;
            }

            regValue = (Int32)subkey.GetValue("SPDIF Audio Time Offset");
            textBoxAudioOffset.Text = regValue.ToString();

            regValue = (Int32)subkey.GetValue("Speaker Config");
            comboBoxSpeakerConfig.SelectedIndex = regValue;
          }
          catch (Exception) {}
        }
      }
    }

    public override void SaveSettings()
    {
      using (RegistryKey subkey = Registry.CurrentUser.CreateSubKey(@"Software\DScaler5\Mpeg Audio Filter"))
      {
        if (subkey != null)
        {
          Int32 regValue;
          if (checkBoxDynamicRange.Checked)
          {
            regValue = 1;
          }
          else
          {
            regValue = 0;
          }
          subkey.SetValue("Dynamic Range Control", regValue);


          if (checkBoxMPEGOverSPDIF.Checked)
          {
            regValue = 1;
          }
          else
          {
            regValue = 0;
          }
          subkey.SetValue("MPEG Audio over SPDIF", regValue);

          if (checkBoxSPDIF.Checked)
          {
            regValue = 1;
          }
          else
          {
            regValue = 0;
          }
          subkey.SetValue("Use SPDIF for AC3 & DTS", regValue);

          regValue = Int32.Parse(textBoxAudioOffset.Text);
          subkey.SetValue("SPDIF Audio Time Offset", regValue);

          regValue = comboBoxSpeakerConfig.SelectedIndex;
          subkey.SetValue("Speaker Config", regValue);
        }
      }
    }
  }
}