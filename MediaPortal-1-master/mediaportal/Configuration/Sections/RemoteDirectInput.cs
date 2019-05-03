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
using System.Windows.Forms;
using MediaPortal.InputDevices;
using MediaPortal.UserInterface.Controls;

#pragma warning disable 108

namespace MediaPortal.Configuration.Sections
{
  /// <summary>
  /// Summary description for DirectInputRemote.
  /// </summary>
  public class RemoteDirectInput : SectionSettings
  {
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private Container components = null;

    private DirectInputHandler diHandler = null;
    private MPGroupBox groupBox1;
    private MPButton btnMapping;
    private MPCheckBox cbEnable;
    private MPGroupBox gbI;
    private MPLabel txtMonitor;
    private MPGroupBox gbButtonCombos;
    private MPButton btnLearn;
    private MPLabel label2;
    private MPLabel lblComboKill;
    private MPTextBox txtComboClose;
    private MPTextBox txtComboKill;
    private MPGroupBox gbSettings;
    private MPButton btnRunControlPanel;
    private MPButton buttonDefault;
    private NumericUpDown numDelay;
    private MPComboBox cbDevices;
    private MPLabel lblDelayMS;
    private MPLabel lblDInputDevice;

    private MPTextBox lastEdit;

    public RemoteDirectInput()
      : this("Direct Input") {}


    public RemoteDirectInput(string name)
      : base(name)
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      // TODO: Add any initialization after the InitializeComponent call
      diHandler = new DirectInputHandler();
      diHandler.Init(true);
      diHandler.DoSendActions = false; // only debug/display actions
      diHandler.OnStateChangeText += new DirectInputHandler.diStateChangeText(StateChangeAsText);
    }

    /// <summary> 
    /// Clean up any resources being used.
    /// </summary>
    protected override void Dispose(bool disposing)
    {
      diHandler.OnStateChangeText -= new DirectInputHandler.diStateChangeText(StateChangeAsText);
      diHandler.Stop();
      diHandler = null;
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
      this.groupBox1 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.btnMapping = new MediaPortal.UserInterface.Controls.MPButton();
      this.cbEnable = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.gbI = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.txtMonitor = new MediaPortal.UserInterface.Controls.MPLabel();
      this.gbButtonCombos = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.btnLearn = new MediaPortal.UserInterface.Controls.MPButton();
      this.label2 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblComboKill = new MediaPortal.UserInterface.Controls.MPLabel();
      this.txtComboClose = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.txtComboKill = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.gbSettings = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.btnRunControlPanel = new MediaPortal.UserInterface.Controls.MPButton();
      this.buttonDefault = new MediaPortal.UserInterface.Controls.MPButton();
      this.numDelay = new System.Windows.Forms.NumericUpDown();
      this.cbDevices = new MediaPortal.UserInterface.Controls.MPComboBox();
      this.lblDelayMS = new MediaPortal.UserInterface.Controls.MPLabel();
      this.lblDInputDevice = new MediaPortal.UserInterface.Controls.MPLabel();
      this.groupBox1.SuspendLayout();
      this.gbI.SuspendLayout();
      this.gbButtonCombos.SuspendLayout();
      this.gbSettings.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numDelay)).BeginInit();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.groupBox1.Controls.Add(this.btnMapping);
      this.groupBox1.Controls.Add(this.cbEnable);
      this.groupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.groupBox1.Location = new System.Drawing.Point(6, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(462, 56);
      this.groupBox1.TabIndex = 4;
      this.groupBox1.TabStop = false;
      // 
      // btnMapping
      // 
      this.btnMapping.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnMapping.Enabled = false;
      this.btnMapping.Location = new System.Drawing.Point(374, 20);
      this.btnMapping.Name = "btnMapping";
      this.btnMapping.Size = new System.Drawing.Size(72, 22);
      this.btnMapping.TabIndex = 1;
      this.btnMapping.Text = "Mapping";
      this.btnMapping.UseVisualStyleBackColor = true;
      this.btnMapping.Click += new System.EventHandler(this.btnMapping_Click);
      // 
      // cbEnable
      // 
      this.cbEnable.AutoSize = true;
      this.cbEnable.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cbEnable.Location = new System.Drawing.Point(16, 24);
      this.cbEnable.Name = "cbEnable";
      this.cbEnable.Size = new System.Drawing.Size(274, 17);
      this.cbEnable.TabIndex = 0;
      this.cbEnable.Text = "Use Direct Input devices (Gamepads, Joysticks, etc.)";
      this.cbEnable.UseVisualStyleBackColor = true;
      this.cbEnable.CheckedChanged += new System.EventHandler(this.cbEnable_CheckedChanged);
      // 
      // gbI
      // 
      this.gbI.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.gbI.Controls.Add(this.txtMonitor);
      this.gbI.Enabled = false;
      this.gbI.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.gbI.Location = new System.Drawing.Point(6, 232);
      this.gbI.Name = "gbI";
      this.gbI.Size = new System.Drawing.Size(462, 124);
      this.gbI.TabIndex = 7;
      this.gbI.TabStop = false;
      this.gbI.Text = "Information";
      // 
      // txtMonitor
      // 
      this.txtMonitor.AutoSize = true;
      this.txtMonitor.Font = new System.Drawing.Font("Lucida Console", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.txtMonitor.Location = new System.Drawing.Point(16, 24);
      this.txtMonitor.Name = "txtMonitor";
      this.txtMonitor.Size = new System.Drawing.Size(180, 12);
      this.txtMonitor.TabIndex = 0;
      this.txtMonitor.Text = "No information available.";
      // 
      // gbButtonCombos
      // 
      this.gbButtonCombos.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.gbButtonCombos.Controls.Add(this.btnLearn);
      this.gbButtonCombos.Controls.Add(this.label2);
      this.gbButtonCombos.Controls.Add(this.lblComboKill);
      this.gbButtonCombos.Controls.Add(this.txtComboClose);
      this.gbButtonCombos.Controls.Add(this.txtComboKill);
      this.gbButtonCombos.Enabled = false;
      this.gbButtonCombos.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.gbButtonCombos.Location = new System.Drawing.Point(6, 144);
      this.gbButtonCombos.Name = "gbButtonCombos";
      this.gbButtonCombos.Size = new System.Drawing.Size(462, 80);
      this.gbButtonCombos.TabIndex = 6;
      this.gbButtonCombos.TabStop = false;
      this.gbButtonCombos.Text = "Button Combos";
      // 
      // btnLearn
      // 
      this.btnLearn.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnLearn.Location = new System.Drawing.Point(374, 33);
      this.btnLearn.Name = "btnLearn";
      this.btnLearn.Size = new System.Drawing.Size(72, 22);
      this.btnLearn.TabIndex = 4;
      this.btnLearn.Text = "&Learn";
      this.btnLearn.UseVisualStyleBackColor = true;
      this.btnLearn.Click += new System.EventHandler(this.btnLearn_Click);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(16, 48);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(76, 13);
      this.label2.TabIndex = 2;
      this.label2.Text = "Close process:";
      // 
      // lblComboKill
      // 
      this.lblComboKill.AutoSize = true;
      this.lblComboKill.Location = new System.Drawing.Point(16, 24);
      this.lblComboKill.Name = "lblComboKill";
      this.lblComboKill.Size = new System.Drawing.Size(63, 13);
      this.lblComboKill.TabIndex = 0;
      this.lblComboKill.Text = "Kill process:";
      // 
      // txtComboClose
      // 
      this.txtComboClose.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtComboClose.BorderColor = System.Drawing.Color.Empty;
      this.txtComboClose.Location = new System.Drawing.Point(152, 44);
      this.txtComboClose.Name = "txtComboClose";
      this.txtComboClose.Size = new System.Drawing.Size(214, 20);
      this.txtComboClose.TabIndex = 3;
      this.txtComboClose.TextChanged += new System.EventHandler(this.txtComboClose_TextChanged);
      this.txtComboClose.Enter += new System.EventHandler(this.txtComboClose_Enter);
      // 
      // txtComboKill
      // 
      this.txtComboKill.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.txtComboKill.BorderColor = System.Drawing.Color.Empty;
      this.txtComboKill.Location = new System.Drawing.Point(152, 20);
      this.txtComboKill.Name = "txtComboKill";
      this.txtComboKill.Size = new System.Drawing.Size(214, 20);
      this.txtComboKill.TabIndex = 1;
      this.txtComboKill.TextChanged += new System.EventHandler(this.txtComboKill_TextChanged);
      this.txtComboKill.Enter += new System.EventHandler(this.txtComboKill_Enter);
      // 
      // gbSettings
      // 
      this.gbSettings.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.gbSettings.Controls.Add(this.btnRunControlPanel);
      this.gbSettings.Controls.Add(this.buttonDefault);
      this.gbSettings.Controls.Add(this.numDelay);
      this.gbSettings.Controls.Add(this.cbDevices);
      this.gbSettings.Controls.Add(this.lblDelayMS);
      this.gbSettings.Controls.Add(this.lblDInputDevice);
      this.gbSettings.Enabled = false;
      this.gbSettings.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.gbSettings.Location = new System.Drawing.Point(6, 64);
      this.gbSettings.Name = "gbSettings";
      this.gbSettings.Size = new System.Drawing.Size(462, 72);
      this.gbSettings.TabIndex = 5;
      this.gbSettings.TabStop = false;
      this.gbSettings.Text = "Settings";
      // 
      // btnRunControlPanel
      // 
      this.btnRunControlPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.btnRunControlPanel.Location = new System.Drawing.Point(374, 21);
      this.btnRunControlPanel.Name = "btnRunControlPanel";
      this.btnRunControlPanel.Size = new System.Drawing.Size(72, 22);
      this.btnRunControlPanel.TabIndex = 2;
      this.btnRunControlPanel.Text = "Control Panel";
      this.btnRunControlPanel.UseVisualStyleBackColor = true;
      this.btnRunControlPanel.Click += new System.EventHandler(this.btnRunControlPanel_Click);
      // 
      // buttonDefault
      // 
      this.buttonDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.buttonDefault.Location = new System.Drawing.Point(374, 45);
      this.buttonDefault.Name = "buttonDefault";
      this.buttonDefault.Size = new System.Drawing.Size(72, 22);
      this.buttonDefault.TabIndex = 5;
      this.buttonDefault.Text = "&Reset";
      this.buttonDefault.UseVisualStyleBackColor = true;
      this.buttonDefault.Click += new System.EventHandler(this.buttonDefault_Click);
      // 
      // numDelay
      // 
      this.numDelay.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.numDelay.Increment = new decimal(new int[] {
            10,
            0,
            0,
            0});
      this.numDelay.Location = new System.Drawing.Point(152, 44);
      this.numDelay.Maximum = new decimal(new int[] {
            2000,
            0,
            0,
            0});
      this.numDelay.Name = "numDelay";
      this.numDelay.Size = new System.Drawing.Size(214, 20);
      this.numDelay.TabIndex = 4;
      this.numDelay.Value = new decimal(new int[] {
            150,
            0,
            0,
            0});
      this.numDelay.ValueChanged += new System.EventHandler(this.numDelay_ValueChanged);
      // 
      // cbDevices
      // 
      this.cbDevices.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.cbDevices.BorderColor = System.Drawing.Color.Empty;
      this.cbDevices.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
      this.cbDevices.Location = new System.Drawing.Point(152, 20);
      this.cbDevices.Name = "cbDevices";
      this.cbDevices.Size = new System.Drawing.Size(214, 21);
      this.cbDevices.TabIndex = 1;
      this.cbDevices.SelectedIndexChanged += new System.EventHandler(this.cbDevices_SelectedIndexChanged);
      // 
      // lblDelayMS
      // 
      this.lblDelayMS.AutoSize = true;
      this.lblDelayMS.Location = new System.Drawing.Point(16, 48);
      this.lblDelayMS.Name = "lblDelayMS";
      this.lblDelayMS.Size = new System.Drawing.Size(71, 13);
      this.lblDelayMS.TabIndex = 3;
      this.lblDelayMS.Text = "Delay (msec):";
      // 
      // lblDInputDevice
      // 
      this.lblDInputDevice.AutoSize = true;
      this.lblDInputDevice.Location = new System.Drawing.Point(16, 24);
      this.lblDInputDevice.Name = "lblDInputDevice";
      this.lblDInputDevice.Size = new System.Drawing.Size(100, 13);
      this.lblDInputDevice.TabIndex = 0;
      this.lblDInputDevice.Text = "Direct Input device:";
      // 
      // RemoteDirectInput
      // 
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.gbI);
      this.Controls.Add(this.gbButtonCombos);
      this.Controls.Add(this.gbSettings);
      this.Name = "RemoteDirectInput";
      this.Size = new System.Drawing.Size(472, 408);
      this.groupBox1.ResumeLayout(false);
      this.groupBox1.PerformLayout();
      this.gbI.ResumeLayout(false);
      this.gbI.PerformLayout();
      this.gbButtonCombos.ResumeLayout(false);
      this.gbButtonCombos.PerformLayout();
      this.gbSettings.ResumeLayout(false);
      this.gbSettings.PerformLayout();
      ((System.ComponentModel.ISupportInitialize)(this.numDelay)).EndInit();
      this.ResumeLayout(false);

    }

    #endregion

    private void cbDevices_SelectedIndexChanged(object sender, EventArgs e)
    {
      int index = cbDevices.SelectedIndex;
      if (index >= 0)
      {
        diHandler.SelectDevice(diHandler.DeviceGUIDs[index].ToString());
        txtMonitor.Text = "No information available.";
      }
    }

    private void SyncCombo()
    {
      cbDevices.Items.Clear();
      foreach (string deviceName in diHandler.DeviceNames)
      {
        cbDevices.Items.Add(deviceName);
      }
    }

    private void cbEnable_CheckedChanged(object sender, EventArgs e)
    {
      diHandler.Active = cbEnable.Checked;

      btnMapping.Enabled = cbEnable.Checked;
      gbSettings.Enabled = cbEnable.Checked;
      gbButtonCombos.Enabled = cbEnable.Checked;
      gbI.Enabled = cbEnable.Checked;

      if (cbEnable.Checked)
      {
        SyncCombo();
      }
      else
      {
        txtMonitor.Text = "No information available.";
      }
    }

    private void StateChangeAsText(object sender, string stateText)
    {
      txtMonitor.Text = stateText;
    }

    private void btnRunControlPanel_Click(object sender, EventArgs e)
    {
      diHandler.RunControlPanel();
    }

    public override void LoadSettings()
    {
      diHandler.LoadSettings();

      cbEnable.Checked = diHandler.Active;
      cbDevices.SelectedIndex = diHandler.SelectedDeviceIndex;
      numDelay.Value = diHandler.Delay;
      txtComboKill.Text = diHandler.ButtonComboKill;
      txtComboClose.Text = diHandler.ButtonComboClose;
    }

    public override void SaveSettings()
    {
      diHandler.SaveSettings();
    }

    private void btnMapping_Click(object sender, EventArgs e)
    {
      InputMappingForm dlg = new InputMappingForm("DirectInput");
      dlg.ShowDialog(this);
    }

    private void buttonDefault_Click(object sender, EventArgs e)
    {
      numDelay.Value = (decimal)150;
    }

    private void numDelay_ValueChanged(object sender, EventArgs e)
    {
      diHandler.Delay = (int)numDelay.Value;
    }

    private void btnLearn_Click(object sender, EventArgs e)
    {
      if (lastEdit == txtComboKill)
      {
        txtComboKill.Text = diHandler.GetCurrentButtonCombo();
      }
      else if (lastEdit == txtComboClose)
      {
        txtComboClose.Text = diHandler.GetCurrentButtonCombo();
      }
    }

    private void txtComboKill_Enter(object sender, EventArgs e)
    {
      lastEdit = txtComboKill;
    }

    private void txtComboClose_Enter(object sender, EventArgs e)
    {
      lastEdit = txtComboClose;
    }

    private void txtComboKill_TextChanged(object sender, EventArgs e)
    {
      diHandler.ButtonComboKill = txtComboKill.Text;
    }

    private void txtComboClose_TextChanged(object sender, EventArgs e)
    {
      diHandler.ButtonComboClose = txtComboClose.Text;
    }
  }
}