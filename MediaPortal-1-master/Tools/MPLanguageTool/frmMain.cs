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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Data;
using System.Windows.Forms;
using System.Globalization;

namespace MPLanguageTool
{
  public partial class frmMain : Form
  {
    #region Variables

    private NameValueCollection defaultTranslations;
    private DataTable originalTranslations;
    private Dictionary<string, DataRow> originalMapping;
    private CultureInfo culture;

    public static string languagePath;
    public static StringsType LangType;

    #endregion

    public frmMain()
    {
      InitializeComponent();
    }

    private int GetUntranslatedCountDeployTool()
    {
      int count = 0;
      foreach (DataGridViewRow row in gv.Rows)
      {
        if (row.Cells[1].Value == null)
          count++;
      }
      return count;
    }

    private int GetUntranslatedCountMediaPortal()
    {
      int count = 0;
      foreach (DataGridViewRow row in gv2.Rows)
      {
        if (String.IsNullOrEmpty(row.Cells[2].Value.ToString()))
          count++;
      }
      return count;
    }

    private void gv_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      string key = (string)gv.Rows[e.RowIndex].Cells[0].Value;
      string value = (string)gv.Rows[e.RowIndex].Cells[1].Value;
      frmEditDeploy dlg = new frmEditDeploy();
      if (dlg.ShowDialog(key, value, defaultTranslations[key]) == DialogResult.OK)
      {
        string trans = dlg.GetTranslation();
        gv.Rows[e.RowIndex].Cells[1].Value = trans;
        gv.Rows[e.RowIndex].Cells[0].Style.ForeColor = String.IsNullOrEmpty(trans)
                                                         ? System.Drawing.Color.Red
                                                         : System.Drawing.Color.Black;
        gv.Rows[e.RowIndex].Cells[1].Style.ForeColor = String.IsNullOrEmpty(trans)
                                                         ? System.Drawing.Color.Red
                                                         : System.Drawing.Color.Black;
        ToolStripText(GetUntranslatedCountDeployTool());
      }
    }

    private void gv2_CellMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
    {
      string key = (string)gv2.Rows[e.RowIndex].Cells[0].Value;
      string valueOriginal = (string)gv2.Rows[e.RowIndex].Cells[1].Value;
      string valueTranslated = (string)gv2.Rows[e.RowIndex].Cells[2].Value;
      string prefixOriginal = (string)gv2.Rows[e.RowIndex].Cells[3].Value;
      string prefixTranslated = (string)gv2.Rows[e.RowIndex].Cells[4].Value;
      frmEditMP dlg = new frmEditMP();
      if (dlg.ShowDialog(key, valueTranslated, valueOriginal, prefixTranslated, prefixOriginal) == DialogResult.OK)
      {
        string trans = dlg.GetTranslation();
        string prefix = dlg.GetPrefixTranslation();

        gv2.Rows[e.RowIndex].Cells[2].Value = trans;
        gv2.Rows[e.RowIndex].Cells[4].Value = prefix;
        gv2.Rows[e.RowIndex].Cells[0].Style.ForeColor = String.IsNullOrEmpty(trans)
                                                          ? System.Drawing.Color.Red
                                                          : System.Drawing.Color.Black;
        gv2.Rows[e.RowIndex].Cells[1].Style.ForeColor = String.IsNullOrEmpty(trans)
                                                          ? System.Drawing.Color.Red
                                                          : System.Drawing.Color.Black;
        ToolStripText(GetUntranslatedCountMediaPortal());
      }
    }

    #region Menu-events

    private void openDeployToolToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LangType = StringsType.DeployTool;

      folderBrowserDialog1.Description = "Please select a path where [MediaPortal.DeployTool.resx] can be found:";
      folderBrowserDialog1.SelectedPath = Application.StartupPath;
      folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;

      if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
        return;

      languagePath = folderBrowserDialog1.SelectedPath;
      // check if selected path contains the default resx file
      defaultTranslations = ResxHandler.Load(null);

      // if not show folderbrowserdlg until user cancels or selects a path that contains it
      while (defaultTranslations == null)
      {
        MessageBox.Show(
          "The file [MediaPortal.DeployTool.resx] could not be found.\nThe LanguageTool does not work without it.",
          "MPLanguageTool -- Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
          return;
        languagePath = folderBrowserDialog1.SelectedPath;
        defaultTranslations = ResxHandler.Load(null);
      }

      gv.Dock = DockStyle.Fill;
      gv2.Dock = DockStyle.None;
      gv2.Visible = false;

      DisableMenuItems();

      SelectCulture dlg = new SelectCulture();
      if (dlg.ShowDialog() != DialogResult.OK) return;
      culture = dlg.GetSelectedCulture();
      Text = "MPLanguageTool -- Current language: " + culture.NativeName + " -- File: MediaPortal.DeployTool." +
             culture.Name + ".resx";
      ToolStripText("Loading \"MediaPortal.DeployTool." + culture.Name + ".resx\"...");

      Cursor = Cursors.WaitCursor;
      NameValueCollection translations = ResxHandler.Load(culture.Name);
      int untranslated = 0;
      foreach (string key in defaultTranslations.AllKeys)
      {
        gv.Rows.Add(key, translations[key]);
        if (String.IsNullOrEmpty(translations[key]))
        {
          gv.Rows[gv.RowCount - 1].Cells[0].Style.ForeColor = System.Drawing.Color.Red;
          gv.Rows[gv.RowCount - 1].Cells[1].Style.ForeColor = System.Drawing.Color.Red;
          untranslated++;
        }
      }
      ToolStripText(untranslated);
      saveToolStripMenuItem.Enabled = true;
      Cursor = Cursors.Default;
    }

    private void openMpToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LangType = StringsType.MediaPortal_1;
      InvokeXml();
    }

    private void openMpIIToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LangType = StringsType.MediaPortal_II;
      InvokeXml();
    }

    private void openTagThatToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LangType = StringsType.MpTagThat;
      InvokeXml();
    }

    private void openMovingPicturesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LangType = StringsType.MovingPictures;
      InvokeXml();
    }

    private void openTvSeriesToolStripMenuItem_Click(object sender, EventArgs e)
    {
      LangType = StringsType.TvSeries;
      InvokeXml();
    }

    #endregion

    #region XML invoke function

    private void InvokeXml()
    {
      string tmpFileName = XmlHandler.BuildFileName(null, false);

      folderBrowserDialog1.Description = "Please select a path where [" + tmpFileName + "] can be found:";
      folderBrowserDialog1.SelectedPath = Application.StartupPath;
      folderBrowserDialog1.RootFolder = Environment.SpecialFolder.Desktop;

      if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
        return;
      languagePath = folderBrowserDialog1.SelectedPath;

      // check if selected path contains the default translation file
      // if not show folderbrowserdlg until user cancels or selects a path that contains it 
      while (!System.IO.File.Exists(System.IO.Path.Combine(languagePath, tmpFileName)))
      {
        MessageBox.Show(
          "The file [" + tmpFileName + "] could not be found.\nThe LanguageTool does not work without it.",
          "MPLanguageTool -- Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
          return;
        languagePath = folderBrowserDialog1.SelectedPath;
      }

      switch (LangType)
      {
        case StringsType.MpTagThat:
        case StringsType.MediaPortal_II:
          FillSectionsCombbo();
          cbSections.Enabled = true;
          string secs = (string)cbSections.SelectedItem;
          XmlHandler.InitializeXmlValues(secs);
          break;

        default:
          XmlHandler.InitializeXmlValues();
          break;
      }

      // check if selected path contains a valid default translation file
      originalTranslations = LoadSection(null);
      // if not show folderbrowserdlg until user cancels or selects a path with a valid one
      while (originalTranslations == null)
      {
        MessageBox.Show("The file [" + tmpFileName + "] is invalid.\nPlease select a correct working one.",
                        "MPLanguageTool -- Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        if (folderBrowserDialog1.ShowDialog() == DialogResult.Cancel)
          return;
        languagePath = folderBrowserDialog1.SelectedPath;
        originalTranslations = XmlHandler.Load(null, out originalMapping);
      }

      gv.Dock = DockStyle.None;
      gv2.Dock = DockStyle.Fill;
      gv2.Visible = true;

      DisableMenuItems();

      SelectCulture dlg = new SelectCulture();
      if (dlg.ShowDialog() != DialogResult.OK) return;
      culture = dlg.GetSelectedCulture();

      tmpFileName = XmlHandler.BuildFileName(culture.Name, false);

      Text = "MPLanguageTool -- Current language: " + culture.NativeName + " -- File: " + tmpFileName;
      ToolStripText("Loading \"" + tmpFileName + "\"...");

      LoadTranslation();
    }

    /// <summary>
    /// Load the section
    /// </summary>
    /// <param name="languageID"></param>
    /// <returns></returns>
    private DataTable LoadSection(string languageID)
    {
      return XmlHandler.Load(languageID, out originalMapping);
    }

    /// <summary>
    /// Loads the translation for the selected section
    /// </summary>
    private void LoadTranslation()
    {
      Cursor = Cursors.WaitCursor;
      // Modified
      DataTable translations = XmlHandler.Load_Translation(culture.Name, originalTranslations, originalMapping);

      int untranslated = 0;

      DataView dv = new DataView(translations);
      gv2.DataSource = dv;

      // Count Not Translated
      for (int z = 0; z < translations.Rows.Count; z++)
      {
        if (String.IsNullOrEmpty((translations.Rows[z]["Translated"].ToString())))
        {
          gv2.Rows[z].Cells[0].Style.ForeColor = System.Drawing.Color.Red;
          gv2.Rows[z].Cells[1].Style.ForeColor = System.Drawing.Color.Red;
          untranslated++;
        }
      }

      ToolStripText(untranslated);
      saveToolStripMenuItem.Enabled = true;
      Cursor = Cursors.Default;
    }


    /// <summary>
    /// Fill the Sections Combo Box with the sections read from the language file
    /// </summary>
    private void FillSectionsCombbo()
    {
      string strSection = string.Empty;
      string strAttrib = string.Empty;

      switch (LangType)
      {
        case StringsType.MediaPortal_II:
          strAttrib = "Name";
          strSection = "/Language/Section";
          break;
        case StringsType.MpTagThat:
          strAttrib = "name";
          strSection = "/Language/Section";
          break;
      }

      List<string> Sections = XmlHandler.ListSections(strSection, strAttrib);
      foreach (string str in Sections)
      {
        cbSections.Items.Add(str);
      }
      cbSections.SelectedItem = cbSections.Items[0];
    }

    #endregion

    #region Common ToolStrips

    private void saveToolStripMenuItem_Click(object sender, EventArgs e)
    {
      switch (LangType)
      {
        case StringsType.DeployTool:
          {
            NameValueCollection translations = new NameValueCollection();

            foreach (DataGridViewRow row in gv.Rows)
              translations.Add((string)row.Cells[0].Value, (string)row.Cells[1].Value);

            ResxHandler.Save(culture.Name, translations);
          }
          break;
        default:
          {
            DataTable translations = new DataTable();

            DataColumn col0 = new DataColumn("id", Type.GetType("System.String"));
            DataColumn col1 = new DataColumn("Translated", Type.GetType("System.String"));
            DataColumn col2 = new DataColumn("PrefixTranslated", Type.GetType("System.String"));

            translations.Columns.Add(col0);
            translations.Columns.Add(col1);
            translations.Columns.Add(col2);

            foreach (DataGridViewRow row in gv2.Rows)
              translations.Rows.Add((string)row.Cells["id"].Value, (string)row.Cells["Translated"].Value,
                                    (string)row.Cells["PrefixTranslated"].Value);

            XmlHandler.Save(culture.Name, culture.EnglishName, translations);
          }
          break;
      }

      ToolStripText("Your translations have been saved.");
      //MessageBox.Show("Your translations have been saved.", "MPLanguageTool -- Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    private void quitToolStripMenuItem_Click(object sender, EventArgs e)
    {
      if (culture != null)
      {
        if (
          MessageBox.Show("Do you want to save before exiting?", "MPLanguageTool -- Confirmation",
                          MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
          saveToolStripMenuItem_Click(null, new EventArgs());
      }
      Close();
    }

    public void ToolStripText(int lines)
    {
      string AddTxt;
      if (lines != 0)
      {
        toolStripStatusLabel1.ForeColor = System.Drawing.Color.Red;
        AddTxt = ". Double click a row to edit text.";
      }
      else
      {
        toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
        AddTxt = null;
      }
      toolStripStatusLabel1.Text = "Missing translations: " + lines + AddTxt;
    }

    public void ToolStripText(string status)
    {
      toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
      toolStripStatusLabel1.Text = status;
      statusStrip1.Refresh();
    }

    private void DisableMenuItems()
    {
      openMp1ToolStripMenuItem.Enabled = false;
      openMpIIToolStripMenuItem.Enabled = false;
      openDeployToolToolStripMenuItem.Enabled = false;
      openTagThatToolStripMenuItem.Enabled = false;
      openMovingPicturesToolStripMenuItem.Enabled = false;
      openTvSeriesToolStripMenuItem.Enabled = false;
    }

    /// <summary>
    /// A selection has been made in the combo box. Load the section
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void cbSections_SelectedIndexChanged(object sender, EventArgs e)
    {
      // The event gets fired, when we fill the combo and we don't have a culture yet.
      if (culture == null)
      {
        return;
      }

      // Save previous section
      saveToolStripMenuItem_Click(null, new EventArgs());

      string secs = (string)cbSections.SelectedItem;
      XmlHandler.InitializeXmlValues(secs);
      originalTranslations = LoadSection(null);
      LoadTranslation();
    }

    #endregion

    public enum StringsType
    {
      MediaPortal_1,
      MediaPortal_II,
      DeployTool,
      MpTagThat,
      MovingPictures,
      TvSeries
    }
  }
}