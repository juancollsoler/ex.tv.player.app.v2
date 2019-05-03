#region Copyright (C) 2005-2016 Team MediaPortal

// Copyright (C) 2005-2016 Team MediaPortal
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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace MPTvClient
{
    public partial class frmEPG : Form
  {
    private ServerInterface server;

    public frmEPG(ServerInterface serverIf, List<ChannelInfo> infos)
    {
      InitializeComponent();
      server = serverIf;
      foreach (ChannelInfo info in infos)
        lbChannels.Items.Add(info.channelID + " " + info.name);
    }

    private void lbChannels_Click(object sender, EventArgs e)
    {
      if (lbChannels.SelectedIndex == -1)
        return;
      grid.Rows.Clear();
      string id = lbChannels.SelectedItem.ToString();
      id = id.Substring(0, id.IndexOf(" "));
      List<EPGInfo> infos = server.GetEPGForChannel(id);
      bool isAlternating = false;
      foreach (EPGInfo epg in infos)
      {
        DataGridViewRow row = new DataGridViewRow();
        row.CreateCells(grid);
        row.Cells[0].Value = epg.startTime.ToString("dd.MM.yy") + " " + epg.startTime.ToString("HH:mm") + "-" +
                             epg.endTime.ToString("HH:mm");
        row.Cells[1].Value = epg.title;
        row.DefaultCellStyle.Font = new Font("Tahoma", 8, FontStyle.Bold);
        if (isAlternating)
          row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
        grid.Rows.Add(row);
        row = new DataGridViewRow();
        row.CreateCells(grid);
        row.Cells[1].Value = epg.description;
        if (isAlternating)
          row.DefaultCellStyle.BackColor = System.Drawing.Color.LightGray;
        grid.Rows.Add(row);
        isAlternating = (!isAlternating);
      }
      grid.AutoResizeColumns();
    }
  }
}