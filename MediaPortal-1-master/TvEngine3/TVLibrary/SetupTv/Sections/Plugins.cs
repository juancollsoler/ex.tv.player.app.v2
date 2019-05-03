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
using TvDatabase;
using TvEngine;

namespace SetupTv.Sections
{
  public partial class Plugins : SectionSettings
  {
    private readonly PluginLoader _loader;
    private bool _needRestart;
    private bool _ignoreEvents;

    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public event ChangedEventHandler ChangedActivePlugins;


    public Plugins(string name, PluginLoader loader)
      : base(name)
    {
      _loader = loader;
      InitializeComponent();
    }

    public override void OnSectionActivated()
    {
      _needRestart = false;
      _ignoreEvents = true;
      TvBusinessLayer layer = new TvBusinessLayer();
      base.OnSectionActivated();
      listView1.Items.Clear();
      ListViewGroup listGroup = listView1.Groups["listViewGroupAvailable"];
      foreach (ITvServerPlugin plugin in _loader.Plugins)
      {
        ListViewItem item = listView1.Items.Add("");
        item.Group = listGroup;
        item.SubItems.Add(plugin.Name);
        item.SubItems.Add(plugin.Author);
        item.SubItems.Add(plugin.Version);
        Setting setting = layer.GetSetting(String.Format("plugin{0}", plugin.Name), "false");
        item.Checked = setting.Value == "true";
        item.Tag = setting;
      }
      listGroup = listView1.Groups["listViewGroupIncompatible"];
      foreach (Type plugin in _loader.IncompatiblePlugins)
      {
        ListViewItem item = listView1.Items.Add("");
        item.Group = listGroup;
        item.SubItems.Add(plugin.Name);
        item.SubItems.Add("Unknown");
        item.SubItems.Add(plugin.Assembly.GetName().Version.ToString());
        item.Checked = false;
      }
      listView1.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
      _ignoreEvents = false;
    }

    public override void OnSectionDeActivated()
    {
      if (_needRestart)
      {
        MessageBox.Show(this, "The activated plugins will be started after you restart the TVService manually",
          "Plugin activation", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
      }
      base.OnSectionDeActivated();
    }

    private void listView1_ItemChecked(object sender, ItemCheckedEventArgs e)
    {
      if (_ignoreEvents)
        return;
      Setting setting = e.Item.Tag as Setting;
      if (setting == null)
      {
        e.Item.Checked = false;
        return;
      }

      setting.Value = e.Item.Checked ? "true" : "false";
      setting.Persist();
      _needRestart = true;

      OnChanged(setting, EventArgs.Empty);
    }

    //Pass on the information for the plugin that was changed
    protected virtual void OnChanged(object sender, EventArgs e)
    {
      if (ChangedActivePlugins != null)
        ChangedActivePlugins(sender, e);
    }
  }
}