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
using TvDatabase;
using TvLibrary.Log;

namespace SetupTv.Sections
{
  public partial class BlasterSetup : SectionSettings
  {
    public BlasterSetup()
    {
      InitializeComponent();
    }

    public override void OnSectionDeActivated()
    {
      TvBusinessLayer layer = new TvBusinessLayer();
      Setting setting = layer.GetSetting("SrvBlasterType");
      setting.Value = comboBoxType.SelectedIndex.ToString();
      setting.Persist();
      setting = layer.GetSetting("SrvBlasterSpeed");
      setting.Value = comboBoxSpeed.SelectedIndex.ToString();
      setting.Persist();
      setting = layer.GetSetting("SrvBlaster1Card");
      setting.Value = comboBoxBlaster1.SelectedIndex.ToString();
      setting.Persist();
      setting = layer.GetSetting("SrvBlaster2Card");
      setting.Value = comboBoxBlaster2.SelectedIndex.ToString();
      setting.Persist();
      setting = layer.GetSetting("SrvBlasterLog");
      setting.Value = Convert.ToString(checkBoxExtLog.Checked);
      setting.Persist();
      setting = layer.GetSetting("SrvBlasterSendSelect");
      setting.Value = Convert.ToString(checkSendSelect.Checked);
      setting.Persist();

      base.OnSectionDeActivated();
    }

    public override void OnSectionActivated()
    {
      TvBusinessLayer layer = new TvBusinessLayer();
      comboBoxType.SelectedIndex = Convert.ToInt16(layer.GetSetting("SrvBlasterType", "0").Value);
      comboBoxSpeed.SelectedIndex = Convert.ToInt16(layer.GetSetting("SrvBlasterSpeed", "0").Value);
      comboBoxBlaster1.Items.Clear();
      comboBoxBlaster2.Items.Clear();
      comboBoxBlaster1.Items.Add("None");
      comboBoxBlaster2.Items.Add("None");
      for (int i = 0; i < layer.Cards.Count; ++i)
      {
        Card card = layer.Cards[i];
        comboBoxBlaster1.Items.Add(card.Name);
        comboBoxBlaster2.Items.Add(card.Name);
      }
      Log.WriteFile("CB1Size {0}, CB2Size {1}, BT1 {2}, BT2 {3}", comboBoxBlaster1.Items.Count,
                    comboBoxBlaster1.Items.Count, Convert.ToInt16(layer.GetSetting("SrvBlaster1Card", "0").Value),
                    Convert.ToInt16(layer.GetSetting("SrvBlaster2Card", "0").Value));
      comboBoxBlaster1.SelectedIndex = Convert.ToInt16(layer.GetSetting("SrvBlaster1Card", "0").Value);
      comboBoxBlaster2.SelectedIndex = Convert.ToInt16(layer.GetSetting("SrvBlaster2Card", "0").Value);
      checkBoxExtLog.Checked = (layer.GetSetting("SrvBlasterLog").Value == "True");
      checkSendSelect.Checked = (layer.GetSetting("SrvBlasterSendSelect").Value == "True");
    }

    private void ComboBox1SelectedIndexChanged(object sender, EventArgs e)
    {
      bool enabled;

      switch (comboBoxType.SelectedIndex)
      {
        case 0: // Microsoft
          if (OSInfo.OSInfo.VistaOrLater())
          {
            enabled = false;
            checkBoxExtLog.Visible = false;
            mpLabelAdditionalNotes.Text =
              "Because of an architecture change in the driver handling, MCE blasting is no more available under Vista and newer operating systems";
          }
          else
          {
            enabled = true;
          }
          break;

        case 1: // SMK
          enabled = true;
          break;

        case 2: // Hauppauge blasting
          enabled = false;
          checkBoxExtLog.Visible = true;
          mpLabelAdditionalNotes.Text =
            "To configure the Hauppauge IR Blaster, use the original Hauppauge IR configuration software.";
          break;
        default:
          enabled = false;
          break;
      }
      if (enabled)
      {
        comboBoxSpeed.Visible = true;
        comboBoxBlaster1.Visible = true;
        comboBoxBlaster2.Visible = true;
        labelBlasterSpeed.Visible = true;
        labelUseBlaster1.Visible = true;
        labelUseBlaster2.Visible = true;
        checkBoxExtLog.Visible = true;
        checkSendSelect.Visible = true;
        mpLabelAdditionalNotes.Visible = false;
      }
      else
      {
        comboBoxSpeed.Visible = false;
        comboBoxBlaster1.Visible = false;
        comboBoxBlaster2.Visible = false;
        labelBlasterSpeed.Visible = false;
        labelUseBlaster1.Visible = false;
        labelUseBlaster2.Visible = false;
        checkSendSelect.Visible = false;
        mpLabelAdditionalNotes.Visible = true;
      }
    }
  }
}