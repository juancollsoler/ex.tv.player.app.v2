namespace MediaPortal.DeployTool
{
  partial class DeployTool
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DeployTool));
      this.splitContainer1 = new System.Windows.Forms.SplitContainer();
      this.bHelp = new System.Windows.Forms.Button();
      this.bExit = new System.Windows.Forms.Button();
      this.pictureBox1 = new System.Windows.Forms.PictureBox();
      this.splitContainer2 = new System.Windows.Forms.SplitContainer();
      this.nextButton = new System.Windows.Forms.Button();
      this.backButton = new System.Windows.Forms.Button();
      this.splitContainer1.Panel1.SuspendLayout();
      this.splitContainer1.Panel2.SuspendLayout();
      this.splitContainer1.SuspendLayout();
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
      this.splitContainer2.Panel2.SuspendLayout();
      this.splitContainer2.SuspendLayout();
      this.SuspendLayout();
      // 
      // splitContainer1
      // 
      this.splitContainer1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(31)))), ((int)(((byte)(73)))));
      this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
      this.splitContainer1.IsSplitterFixed = true;
      this.splitContainer1.Location = new System.Drawing.Point(0, 0);
      this.splitContainer1.Margin = new System.Windows.Forms.Padding(0);
      this.splitContainer1.Name = "splitContainer1";
      this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer1.Panel1
      // 
      this.splitContainer1.Panel1.BackColor = System.Drawing.Color.Transparent;
      this.splitContainer1.Panel1.Controls.Add(this.bHelp);
      this.splitContainer1.Panel1.Controls.Add(this.bExit);
      this.splitContainer1.Panel1.Controls.Add(this.pictureBox1);
      this.splitContainer1.Panel1.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      // 
      // splitContainer1.Panel2
      // 
      this.splitContainer1.Panel2.BackColor = System.Drawing.Color.Transparent;
      this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
      this.splitContainer1.Size = new System.Drawing.Size(666, 416);
      this.splitContainer1.SplitterDistance = 122;
      this.splitContainer1.SplitterWidth = 1;
      this.splitContainer1.TabIndex = 0;
      // 
      // bHelp
      // 
      this.bHelp.BackColor = System.Drawing.Color.Transparent;
      this.bHelp.BackgroundImage = global::MediaPortal.DeployTool.Images.Background_help_button;
      this.bHelp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.bHelp.Cursor = System.Windows.Forms.Cursors.Hand;
      this.bHelp.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(31)))), ((int)(((byte)(73)))));
      this.bHelp.FlatAppearance.BorderSize = 0;
      this.bHelp.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(31)))), ((int)(((byte)(73)))));
      this.bHelp.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(31)))), ((int)(((byte)(73)))));
      this.bHelp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.bHelp.Image = global::MediaPortal.DeployTool.Images.helpIcon;
      this.bHelp.Location = new System.Drawing.Point(579, 17);
      this.bHelp.Name = "bHelp";
      this.bHelp.Size = new System.Drawing.Size(31, 30);
      this.bHelp.TabIndex = 21;
      this.bHelp.UseVisualStyleBackColor = false;
      this.bHelp.Click += new System.EventHandler(this.bHelp_Click);
      // 
      // bExit
      // 
      this.bExit.BackColor = System.Drawing.Color.Transparent;
      this.bExit.BackgroundImage = global::MediaPortal.DeployTool.Images.Background_exit_button;
      this.bExit.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.bExit.Cursor = System.Windows.Forms.Cursors.Hand;
      this.bExit.FlatAppearance.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(31)))), ((int)(((byte)(73)))));
      this.bExit.FlatAppearance.BorderSize = 0;
      this.bExit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(31)))), ((int)(((byte)(73)))));
      this.bExit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(31)))), ((int)(((byte)(73)))));
      this.bExit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
      this.bExit.Image = global::MediaPortal.DeployTool.Images.exitIcon;
      this.bExit.Location = new System.Drawing.Point(616, 16);
      this.bExit.Margin = new System.Windows.Forms.Padding(0);
      this.bExit.Name = "bExit";
      this.bExit.Size = new System.Drawing.Size(34, 34);
      this.bExit.TabIndex = 0;
      this.bExit.UseVisualStyleBackColor = false;
      this.bExit.Click += new System.EventHandler(this.bExit_Click);
      // 
      // pictureBox1
      // 
      this.pictureBox1.BackColor = System.Drawing.Color.Transparent;
      this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.pictureBox1.Image = global::MediaPortal.DeployTool.Images.Background_top;
      this.pictureBox1.Location = new System.Drawing.Point(0, 0);
      this.pictureBox1.Name = "pictureBox1";
      this.pictureBox1.Size = new System.Drawing.Size(666, 122);
      this.pictureBox1.TabIndex = 20;
      this.pictureBox1.TabStop = false;
      // 
      // splitContainer2
      // 
      this.splitContainer2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(31)))), ((int)(((byte)(73)))));
      this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
      this.splitContainer2.IsSplitterFixed = true;
      this.splitContainer2.Location = new System.Drawing.Point(0, 0);
      this.splitContainer2.Margin = new System.Windows.Forms.Padding(0);
      this.splitContainer2.Name = "splitContainer2";
      this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
      // 
      // splitContainer2.Panel1
      // 
      this.splitContainer2.Panel1.BackgroundImage = global::MediaPortal.DeployTool.Images.Background_middle_empty;
      // 
      // splitContainer2.Panel2
      // 
      this.splitContainer2.Panel2.BackColor = System.Drawing.Color.WhiteSmoke;
      this.splitContainer2.Panel2.BackgroundImage = global::MediaPortal.DeployTool.Images.Background_bottom;
      this.splitContainer2.Panel2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
      this.splitContainer2.Panel2.Controls.Add(this.nextButton);
      this.splitContainer2.Panel2.Controls.Add(this.backButton);
      this.splitContainer2.Size = new System.Drawing.Size(666, 293);
      this.splitContainer2.SplitterDistance = 250;
      this.splitContainer2.SplitterWidth = 1;
      this.splitContainer2.TabIndex = 0;
      // 
      // nextButton
      // 
      this.nextButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.nextButton.AutoSize = true;
      this.nextButton.Cursor = System.Windows.Forms.Cursors.Hand;
      this.nextButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.nextButton.Location = new System.Drawing.Point(561, 8);
      this.nextButton.Name = "nextButton";
      this.nextButton.Size = new System.Drawing.Size(75, 25);
      this.nextButton.TabIndex = 2;
      this.nextButton.Text = "next";
      this.nextButton.UseVisualStyleBackColor = true;
      this.nextButton.Click += new System.EventHandler(this.nextButton_Click);
      // 
      // backButton
      // 
      this.backButton.AutoSize = true;
      this.backButton.Cursor = System.Windows.Forms.Cursors.Hand;
      this.backButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.backButton.Location = new System.Drawing.Point(446, 8);
      this.backButton.Name = "backButton";
      this.backButton.Size = new System.Drawing.Size(78, 24);
      this.backButton.TabIndex = 1;
      this.backButton.Text = "previous";
      this.backButton.UseVisualStyleBackColor = true;
      this.backButton.Click += new System.EventHandler(this.backButton_Click);
      // 
      // DeployTool
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.None;
      this.BackColor = System.Drawing.Color.White;
      this.ClientSize = new System.Drawing.Size(666, 416);
      this.Controls.Add(this.splitContainer1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
      this.MaximizeBox = false;
      this.Name = "DeployTool";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
      this.Text = "MediaPortal Deploy Tool";
      this.splitContainer1.Panel1.ResumeLayout(false);
      this.splitContainer1.Panel2.ResumeLayout(false);
      this.splitContainer1.ResumeLayout(false);
      ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
      this.splitContainer2.Panel2.ResumeLayout(false);
      this.splitContainer2.Panel2.PerformLayout();
      this.splitContainer2.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.SplitContainer splitContainer1;
    private System.Windows.Forms.PictureBox pictureBox1;
    private System.Windows.Forms.SplitContainer splitContainer2;
    private System.Windows.Forms.Button nextButton;
    private System.Windows.Forms.Button backButton;
    private System.Windows.Forms.Button bExit;
    private System.Windows.Forms.Button bHelp;


  }
}

