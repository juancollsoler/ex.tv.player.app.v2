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
using DirectShowLib.BDA;

namespace SetupTv.Dialogs
{
  public partial class FormATSCTuningDetail : SetupControls.FormTuningDetailCommon
  {
    public FormATSCTuningDetail()
    {
      InitializeComponent();
    }

    private void FormATSCTuningDetail_Load(object sender, EventArgs e)
    {
      if (TuningDetail != null)
      {
        textBoxPhysicalChannel.Text = TuningDetail.ChannelNumber.ToString();
        textBoxFrequency.Text = TuningDetail.Frequency.ToString();
        textBoxMajor.Text = TuningDetail.MajorChannel.ToString();
        textBoxMinor.Text = TuningDetail.MinorChannel.ToString();
        switch ((ModulationType)TuningDetail.Modulation)
        {
          case ModulationType.ModNotSet:
            comboBoxQAMModulation.SelectedIndex = 0;
            break;
          case ModulationType.Mod8Vsb:
            comboBoxQAMModulation.SelectedIndex = 1;
            break;
          case ModulationType.Mod64Qam:
            comboBoxQAMModulation.SelectedIndex = 2;
            break;
          case ModulationType.Mod256Qam:
            comboBoxQAMModulation.SelectedIndex = 3;
            break;
        }
        textBoxQamTSID.Text = TuningDetail.TransportId.ToString();
        textBoxQamSID.Text = TuningDetail.ServiceId.ToString();
        textBoxQamSource.Text = TuningDetail.NetworkId.ToString();
        textBoxQamPmt.Text = TuningDetail.PmtPid.ToString();
        textBoxQamProvider.Text = TuningDetail.Provider;
        checkBoxQamfta.Checked = TuningDetail.FreeToAir;
      }
      else
      {
        textBoxPhysicalChannel.Text = "";
        textBoxFrequency.Text = "";
        textBoxMajor.Text = "";
        textBoxMinor.Text = "";
        comboBoxQAMModulation.SelectedIndex = -1;
        textBoxQamTSID.Text = "";
        textBoxQamSID.Text = "";
        textBoxQamSource.Text = "";
        textBoxQamPmt.Text = "";
        textBoxQamProvider.Text = "";
        checkBoxQamfta.Checked = false;
      }
    }

    private void mpButtonOk_Click(object sender, EventArgs e)
    {
      if (ValidateInput())
      {
        if (TuningDetail == null)
        {
          TuningDetail = CreateInitialTuningDetail();
        }
        UpdateTuningDetail();
        DialogResult = DialogResult.OK;
        Close();
      }
    }

    private void UpdateTuningDetail()
    {
      TuningDetail.ChannelType = 1;
      TuningDetail.ChannelNumber = Int32.Parse(textBoxPhysicalChannel.Text);
      TuningDetail.Frequency = Int32.Parse(textBoxFrequency.Text);
      TuningDetail.MajorChannel = Int32.Parse(textBoxMajor.Text);
      TuningDetail.MinorChannel = Int32.Parse(textBoxMinor.Text);
      switch (comboBoxQAMModulation.SelectedIndex)
      {
        case 0:
          TuningDetail.Modulation = (int)ModulationType.ModNotSet;
          break;
        case 1:
          TuningDetail.Modulation = (int)ModulationType.Mod8Vsb;
          break;
        case 2:
          TuningDetail.Modulation = (int)ModulationType.Mod64Qam;
          break;
        case 3:
          TuningDetail.Modulation = (int)ModulationType.Mod256Qam;
          break;
      }
      TuningDetail.TransportId = Int32.Parse(textBoxQamTSID.Text);
      TuningDetail.ServiceId = Int32.Parse(textBoxQamSID.Text);
      TuningDetail.NetworkId = Int32.Parse(textBoxQamSource.Text);
      TuningDetail.PmtPid = Int32.Parse(textBoxQamPmt.Text);
      TuningDetail.Provider = textBoxQamProvider.Text;
      TuningDetail.FreeToAir = checkBoxQamfta.Checked;
    }

    private bool ValidateInput()
    {
      if (textBoxPhysicalChannel.Text.Length == 0)
      {
        MessageBox.Show(this, "Please enter a physical channel number!", "Incorrect input");
        return false;
      }
      int physical, frequency, major, minor, tsid, serviceId, sourceId, pmt;
      if (!Int32.TryParse(textBoxPhysicalChannel.Text, out physical))
      {
        MessageBox.Show(this, "Please enter a valid physical channel number!", "Incorrect input");
        return false;
      }
      if (textBoxFrequency.Text.Length == 0)
      {
        MessageBox.Show(this, "Please enter a frequency!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxFrequency.Text, out frequency))
      {
        MessageBox.Show(this, "Please enter a valid frequency!", "Incorrect input");
        return false;
      }
      // Frequency must be set to -1 for ATSC over-the-air channels as they are tuned by
      // channel number. Otherwise a valid frequency must be provided.
      if ((comboBoxQAMModulation.SelectedIndex == 1 && frequency != -1) || (comboBoxQAMModulation.SelectedIndex != 1 && frequency <= 0))
      {
        MessageBox.Show(this, "Please enter a valid frequency!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxMajor.Text, out major))
      {
        MessageBox.Show(this, "Please enter a valid major channel number!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxMinor.Text, out minor))
      {
        MessageBox.Show(this, "Please enter a valid minor channel number!", "Incorrect input");
        return false;
      }
      if (physical <= 0 && (major <= 0 || minor <= 0))
      {
        MessageBox.Show(this, "Please enter valid physical or major and minor channel numbers!", "Incorrect input");
        return false;
      }
      if (comboBoxQAMModulation.SelectedIndex < 0)
      {
        MessageBox.Show(this, "Please enter a valid modulation!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxQamTSID.Text, out tsid))
      {
        MessageBox.Show(this, "Please enter a valid transport ID!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxQamSID.Text, out serviceId))
      {
        MessageBox.Show(this, "Please enter a valid service ID!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxQamSource.Text, out sourceId))
      {
        MessageBox.Show(this, "Please enter a valid source ID!", "Incorrect input");
        return false;
      }
      if (sourceId < 0 || tsid < 0 || serviceId < 0)
      {
        MessageBox.Show(this, "Please enter valid transport, service and source IDs!", "Incorrect input");
        return false;
      }
      if (!Int32.TryParse(textBoxQamPmt.Text, out pmt))
      {
        MessageBox.Show(this, "Please enter a valid PMT PID!", "Incorrect input");
        return false;
      }
      if (pmt < 0)
      {
        MessageBox.Show(this, "Please enter a valid PMT PID!", "Incorrect input");
        return false;
      }
      return true;
    }
  }
}