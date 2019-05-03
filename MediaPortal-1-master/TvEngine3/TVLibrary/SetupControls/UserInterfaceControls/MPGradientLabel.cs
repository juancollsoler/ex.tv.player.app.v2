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

using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace MediaPortal.UserInterface.Controls
{
  /// <summary>
  /// Summary description for GradientLabel.
  /// </summary>
  [ToolboxBitmap(typeof (Label))]
  public class MPGradientLabel : UserControl
  {
    private Color firstColor;
    private Color lastColor;

    private int paddingTop, paddingLeft;

    private Label workingLabel;

    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private Container components = null;

    public MPGradientLabel()
    {
      SetStyle(ControlStyles.DoubleBuffer, true);

      //
      // This call is required by the Windows.Forms Form Designer.
      //
      InitializeComponent();

      firstColor = SystemColors.InactiveCaption;
      lastColor = Color.White;

      workingLabel.Paint += workingLabel_Paint;
    }

    private void DrawBackground(Graphics graphics)
    {
      //
      // Create gradient brush
      //
      Brush gradientBrush = new LinearGradientBrush(workingLabel.ClientRectangle,
                                                    firstColor,
                                                    lastColor,
                                                    LinearGradientMode.Horizontal);

      //
      // Draw brush
      //
      graphics.FillRectangle(gradientBrush, ClientRectangle);
      gradientBrush.Dispose();
    }

    private void DrawForeground(Graphics graphics)
    {
      //
      // Draw bevelbox
      //
      Pen grayPen = new Pen(Color.FromArgb(200, 200, 200));
      graphics.DrawLine(grayPen, 0, 0, Width - 1, 0);
      graphics.DrawLine(Pens.WhiteSmoke, 0, Height - 1, Width - 1, Height - 1);
      grayPen.Dispose();

      //
      // Draw caption
      //
      graphics.DrawString(Caption,
                          TextFont,
                          new SolidBrush(TextColor),
                          paddingLeft, paddingTop);
    }

    [Browsable(true), Category("Gradient")]
    public Color FirstColor
    {
      get { return firstColor; }
      set
      {
        firstColor = value;
        workingLabel.Invalidate();
      }
    }

    [Browsable(true), Category("Gradient")]
    public Color LastColor
    {
      get { return lastColor; }
      set
      {
        lastColor = value;
        workingLabel.Invalidate();
      }
    }

    [Browsable(true), Category("Gradient")]
    public string Caption
    {
      get { return workingLabel.Text; }
      set
      {
        workingLabel.Text = value;
        workingLabel.Invalidate();
      }
    }

    [Browsable(true), Category("Gradient")]
    public Color TextColor
    {
      get { return workingLabel.ForeColor; }
      set
      {
        workingLabel.ForeColor = value;
        workingLabel.Invalidate();
      }
    }

    [Browsable(true), Category("Gradient")]
    public Font TextFont
    {
      get { return workingLabel.Font; }
      set
      {
        workingLabel.Font = value;
        workingLabel.Invalidate();
      }
    }

    [Browsable(true), Category("Gradient"), DefaultValue(0)]
    public int PaddingTop
    {
      get { return paddingTop; }
      set
      {
        paddingTop = value;
        workingLabel.Invalidate();
      }
    }

    [Browsable(true), Category("Gradient"), DefaultValue(0)]
    public int PaddingLeft
    {
      get { return paddingLeft; }
      set
      {
        paddingLeft = value;
        workingLabel.Invalidate();
      }
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      this.workingLabel = new System.Windows.Forms.Label();
      this.SuspendLayout();
      // 
      // workingLabel
      // 
      this.workingLabel.BackColor = System.Drawing.SystemColors.Control;
      this.workingLabel.Dock = System.Windows.Forms.DockStyle.Fill;
      this.workingLabel.Location = new System.Drawing.Point(0, 0);
      this.workingLabel.Name = "workingLabel";
      this.workingLabel.Size = new System.Drawing.Size(150, 150);
      this.workingLabel.TabIndex = 0;
      // 
      // GradientLabel
      // 
      this.Controls.Add(this.workingLabel);
      this.Name = "GradientLabel";
      this.ResumeLayout(false);
    }

    #endregion

    private void workingLabel_Paint(object sender, PaintEventArgs e)
    {
      e.Graphics.Clear(System.Drawing.SystemColors.Control);

      DrawBackground(e.Graphics);
      DrawForeground(e.Graphics);
    }
  }
}