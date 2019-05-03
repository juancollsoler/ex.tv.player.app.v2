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
using MediaPortal.ExtensionMethods;
using MediaPortal.UserInterface.Controls;

namespace Yeti.MMedia
{
  /// <summary>
  /// Summary description for EditWaveWriter.
  /// </summary>
  public class EditWaveWriter : UserControl, IEditAudioWriterConfig
  {
    private MPGroupBox groupBox1;
    private EditFormat editFormat1;

    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private Container components = null;

    public EditWaveWriter()
    {
      // This call is required by the Windows.Forms Form Designer.
      InitializeComponent();

      // TODO: Add any initialization after the InitializeComponent call
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
          components.SafeDispose();
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
      this.editFormat1 = new Yeti.MMedia.EditFormat();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.editFormat1);
      this.groupBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.groupBox1.Location = new System.Drawing.Point(0, 0);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(312, 208);
      this.groupBox1.TabIndex = 0;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Audio format";
      // 
      // editFormat1
      // 
      this.editFormat1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.editFormat1.Location = new System.Drawing.Point(3, 16);
      this.editFormat1.Name = "editFormat1";
      this.editFormat1.ReadOnly = false;
      this.editFormat1.Size = new System.Drawing.Size(306, 189);
      this.editFormat1.TabIndex = 0;
      this.editFormat1.ConfigChange += new System.EventHandler(this.editFormat1_ConfigChange);
      // 
      // EditWaveWriter
      // 
      this.Controls.Add(this.groupBox1);
      this.Name = "EditWaveWriter";
      this.Size = new System.Drawing.Size(312, 208);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);
    }

    #endregion

    #region IEditAudioWriterConfig Members

    [Browsable(false)]
    public AudioWriterConfig Config
    {
      get { return new AudioWriterConfig(editFormat1.Format); }
      set { editFormat1.Format = value.Format; }
    }

    #endregion

    #region IConfigControl Members

    public void DoApply()
    {
      editFormat1.DoApply();
    }

    public void DoSetInitialValues()
    {
      editFormat1.DoSetInitialValues();
    }

    public Control ConfigControl
    {
      get { return this; }
    }

    public string ControlName
    {
      get { return "Wave writer config"; }
    }

    public event EventHandler ConfigChange;

    #endregion

    private void editFormat1_ConfigChange(object sender, EventArgs e)
    {
      if (ConfigChange != null)
      {
        ConfigChange(sender, e);
      }
    }
  }
}