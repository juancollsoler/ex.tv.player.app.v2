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
using System.ComponentModel;
using System.Drawing;
using System.Web;
using System.IO;
using System.Text;
using System.Windows.Forms;
using MpeCore;
using MpeCore.Classes;

namespace MpeMaker.Sections
{
  public partial class ToolsUpdateXml : UserControl, ISectionControl
  {
    public PackageClass Package { get; set; }
    private bool loading = false;

    public ToolsUpdateXml()
    {
      Package = new PackageClass();
      InitializeComponent();
      webBrowser.StatusTextChanged += webBrowser_StatusTextChanged;
    }

    #region Browse buttons

    private void button1_Click(object sender, EventArgs e)
    {
      SetupSaveFileDialog(textBox1.Text);
      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        textBox1.Text = saveFileDialog.FileName;
      }
    }

    private void btn_browse1_Click(object sender, EventArgs e)
    {
      SetupSaveFileDialog(txt_list1.Text);
      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        txt_list1.Text = saveFileDialog.FileName;
      }
    }

    private void btn_browse2_Click(object sender, EventArgs e)
    {
      SetupSaveFileDialog(txt_list2.Text);
      if (saveFileDialog.ShowDialog() == DialogResult.OK)
      {
        txt_list2.Text = saveFileDialog.FileName;
      }
    }

    private void SetupSaveFileDialog(string oldPath)
    {
      saveFileDialog.FileName = oldPath;
      if (String.IsNullOrEmpty(oldPath)) return;

      string directory;
      string file = Path.GetFileName(oldPath);

      // set initial dir to old one
      directory = Path.GetDirectoryName(oldPath);
      if (Directory.Exists(directory))
      {
        saveFileDialog.InitialDirectory = directory;
        saveFileDialog.FileName = file;
        return;
      }

      // set initial dir to the xmp2 package dir
      directory = Path.GetFileName(Package.ProjectSettings.ProjectFilename);
      if (Directory.Exists(directory))
      {
        saveFileDialog.InitialDirectory = directory;
        saveFileDialog.FileName = file;
        return;
      }
    }

    #endregion

    /// <summary>
    /// Handles the Click event of the btn_gen control.
    /// </summary>
    /// <param name="sender">The source of the event.</param>
    /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
    private void btn_gen_Click(object sender, EventArgs e)
    {
      Package.WriteUpdateXml(textBox1.Text);
    }

    public void Set(PackageClass pak)
    {
      Package = pak;

      loading = true;
      textBox1.Text = Package.ProjectSettings.UpdatePath1;
      txt_list1.Text = Package.ProjectSettings.UpdatePath2;
      txt_list2.Text = Package.ProjectSettings.UpdatePath3;
      loading = false;

      textBox_TextChanged(null, null);
    }

    public PackageClass Get()
    {
      throw new NotImplementedException();
    }

    private void add_list_Click(object sender, EventArgs e)
    {
      string xmlFile = txt_list1.Text;
      ExtensionCollection list = new ExtensionCollection();
      ExtensionCollection list2 = new ExtensionCollection();
      if (File.Exists(xmlFile))
        list = ExtensionCollection.Load(xmlFile);
      if (File.Exists(txt_list2.Text))
        list2 = ExtensionCollection.Load(txt_list2.Text);
      list.Add(list2);
      list.Save(xmlFile);
    }

    private void textBox_TextChanged(object sender, EventArgs e)
    {
      if (loading) return;

      Package.ProjectSettings.UpdatePath1 = textBox1.Text;
      Package.ProjectSettings.UpdatePath2 = txt_list1.Text;
      Package.ProjectSettings.UpdatePath3 = txt_list2.Text;

      btn_gen.Enabled = !String.IsNullOrEmpty(textBox1.Text);
      add_list.Enabled = !(String.IsNullOrEmpty(txt_list1.Text) || String.IsNullOrEmpty(txt_list2.Text));
    }

    private void PublishClick(object sender, EventArgs e)
    {
      if (string.IsNullOrEmpty(Package.GeneralInfo.UpdateUrl))
      {
        MessageBox.Show("No update url is specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      Uri parsedUri = null;
      if (!Uri.TryCreate(Package.GeneralInfo.UpdateUrl, UriKind.Absolute, out parsedUri))
      {
        MessageBox.Show("Invalid update url specified.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      if (!parsedUri.Scheme.StartsWith("http"))
      {
        MessageBox.Show("Update url must starte with http.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }
      if (Package.ValidatePackage().Count > 0)
      {
        MessageBox.Show("Package contains error(s). First solve it!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        return;
      }

      string url =
        string.Format("http://install.team-mediaportal.com/MPEI/submit.php?url={0}&name={1}&version={2}&id={3}",
                      HttpUtility.UrlEncode(Package.GeneralInfo.UpdateUrl), 
                      Package.GeneralInfo.Name,
                      Package.GeneralInfo.Version,
                      Package.GeneralInfo.Id);
      if (webBrowser.Url != null && webBrowser.Url.OriginalString == url)
        webBrowser.Refresh(WebBrowserRefreshOption.Completely);
      else
        webBrowser.Navigate(url);
    }

    private void webBrowser_StatusTextChanged(object sender, EventArgs e)
    {
      toolStripStatusLabel1.Text = webBrowser.StatusText;
    }

    private void webBrowser_ProgressChanged(object sender, WebBrowserProgressChangedEventArgs e)
    {
      toolStripProgressBar1.Maximum = (int)e.MaximumProgress;
      toolStripProgressBar1.Value = Math.Max(0, (int)e.CurrentProgress);
    }
  }
}