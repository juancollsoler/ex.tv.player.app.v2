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
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.Globalization;
using TvControl;
using TvDatabase;
using MediaPortal.UserInterface.Controls;

namespace SetupTv.Sections
{
  public partial class TvSchedules : SectionSettings
  {
    private readonly MPListViewStringColumnSorter lvwColumnSorter;

    public TvSchedules()
      : this("Schedules") { }

    public TvSchedules(string name)
      : base(name)
    {
      InitializeComponent();
      lvwColumnSorter = new MPListViewStringColumnSorter();
      lvwColumnSorter.Order = SortOrder.None;
      listView1.ListViewItemSorter = lvwColumnSorter;
    }

    public override void OnSectionActivated()
    {
      base.OnSectionActivated();
      LoadSchedules();
    }

    private void LoadSchedules()
    {
      IFormatProvider mmddFormat = new CultureInfo(String.Empty, false);
      listView1.Items.Clear();
      IList<Schedule> schedules = Schedule.ListAll();
      foreach (Schedule schedule in schedules)
      {
        ListViewItem item = new ListViewItem(schedule.Priority.ToString());
        item.SubItems.Add(schedule.ReferencedChannel().DisplayName);
        item.Tag = schedule;
        switch ((ScheduleRecordingType)schedule.ScheduleType)
        {
          case ScheduleRecordingType.Daily:
            item.ImageIndex = 0;
            item.SubItems.Add("Daily");
            item.SubItems.Add(String.Format("{0}", schedule.StartTime.ToString("HH:mm:ss", mmddFormat)));
            break;
          case ScheduleRecordingType.Weekly:
            item.ImageIndex = 0;
            item.SubItems.Add("Weekly");
            item.SubItems.Add(String.Format("{0} {1}", schedule.StartTime.DayOfWeek,
                                            schedule.StartTime.ToString("HH:mm:ss", mmddFormat)));
            break;
          case ScheduleRecordingType.Weekends:
            item.ImageIndex = 0;
            item.SubItems.Add("Weekends");
            item.SubItems.Add(String.Format("{0}", schedule.StartTime.ToString("HH:mm:ss", mmddFormat)));
            break;
          case ScheduleRecordingType.WorkingDays:
            item.ImageIndex = 0;
            item.SubItems.Add("WorkingDays");
            item.SubItems.Add(String.Format("{0}", schedule.StartTime.ToString("HH:mm:ss", mmddFormat)));
            break;
          case ScheduleRecordingType.Once:
            item.ImageIndex = 1;
            item.SubItems.Add("Once");
            item.SubItems.Add(String.Format("{0}", schedule.StartTime.ToString("dd-MM-yyyy HH:mm:ss", mmddFormat)));
            break;
          case ScheduleRecordingType.WeeklyEveryTimeOnThisChannel:
            item.ImageIndex = 0;
            item.SubItems.Add("Weekly Always");
            item.SubItems.Add(schedule.StartTime.DayOfWeek.ToString());
            break;
          case ScheduleRecordingType.EveryTimeOnThisChannel:
            item.ImageIndex = 0;
            item.SubItems.Add("Always");
            item.SubItems.Add("");
            break;
          case ScheduleRecordingType.EveryTimeOnEveryChannel:
            item.ImageIndex = 0;
            item.SubItems.Add("Always");
            item.SubItems.Add("All channels");
            break;
        }
        item.SubItems.Add(schedule.ProgramName);
        item.SubItems.Add(String.Format("{0} mins", schedule.PreRecordInterval));
        item.SubItems.Add(String.Format("{0} mins", schedule.PostRecordInterval));

        if (schedule.MaxAirings.ToString() == int.MaxValue.ToString())
          item.SubItems.Add("Keep all");
        else
          item.SubItems.Add(schedule.MaxAirings.ToString());

        if (schedule.IsSerieIsCanceled(schedule.StartTime))
        {
          item.Font = new Font(item.Font, FontStyle.Strikeout);
        }
        listView1.Items.Add(item);
      }
      listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
    }

    private void listView1_ColumnClick(object sender, ColumnClickEventArgs e)
    {
      if (e.Column == lvwColumnSorter.SortColumn)
      {
        // Reverse the current sort direction for this column.
        lvwColumnSorter.Order = lvwColumnSorter.Order == SortOrder.Ascending
                                  ? SortOrder.Descending
                                  : SortOrder.Ascending;
      }
      else
      {
        // Set the column number that is to be sorted; default to ascending.
        lvwColumnSorter.SortColumn = e.Column;
        lvwColumnSorter.Order = SortOrder.Ascending;
      }
      // Perform the sort with these new sort options.
      listView1.Sort();
    }

    private void deleteToolStripMenuItem_Click(object sender, EventArgs e)
    {
      mpButtonDel_Click(null, null);
    }

    private void mpButtonDel_Click(object sender, EventArgs e)
    {
      foreach (ListViewItem item in listView1.SelectedItems)
      {
        Schedule schedule = (Schedule)item.Tag;
        TvServer server = new TvServer();
        server.StopRecordingSchedule(schedule.IdSchedule);
        schedule.Delete();

        listView1.Items.Remove(item);
      }
      listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
    }
  }
}