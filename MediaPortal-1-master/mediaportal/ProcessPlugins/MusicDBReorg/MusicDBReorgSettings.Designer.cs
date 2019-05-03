namespace MediaPortal.ProcessPlugins.MusicDBReorg
{
  partial class MusicDBReorgSettings
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
      this.mpGroupBox1 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.minutesTextBox = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.mpLabel2 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.hoursTextBox = new MediaPortal.UserInterface.Controls.MPTextBox();
      this.mpLabel1 = new MediaPortal.UserInterface.Controls.MPLabel();
      this.mpGroupBox2 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.cbSunday = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.cbSaturday = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.cbFriday = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.cbThursday = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.cbWednesday = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.cbTuesday = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.cbMonday = new MediaPortal.UserInterface.Controls.MPCheckBox();
      this.btnOK = new MediaPortal.UserInterface.Controls.MPButton();
      this.btnCancel = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpGroupBox1.SuspendLayout();
      this.mpGroupBox2.SuspendLayout();
      this.SuspendLayout();
      // 
      // mpGroupBox1
      // 
      this.mpGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.mpGroupBox1.Controls.Add(this.minutesTextBox);
      this.mpGroupBox1.Controls.Add(this.mpLabel2);
      this.mpGroupBox1.Controls.Add(this.hoursTextBox);
      this.mpGroupBox1.Controls.Add(this.mpLabel1);
      this.mpGroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox1.Location = new System.Drawing.Point(12, 12);
      this.mpGroupBox1.Name = "mpGroupBox1";
      this.mpGroupBox1.Size = new System.Drawing.Size(273, 48);
      this.mpGroupBox1.TabIndex = 0;
      this.mpGroupBox1.TabStop = false;
      this.mpGroupBox1.Text = "Schedule time";
      // 
      // minutesTextBox
      // 
      this.minutesTextBox.BorderColor = System.Drawing.Color.Empty;
      this.minutesTextBox.Location = new System.Drawing.Point(171, 20);
      this.minutesTextBox.Name = "minutesTextBox";
      this.minutesTextBox.Size = new System.Drawing.Size(27, 20);
      this.minutesTextBox.TabIndex = 3;
      this.minutesTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.minutesTextBox_KeyPress);
      // 
      // mpLabel2
      // 
      this.mpLabel2.AutoSize = true;
      this.mpLabel2.Location = new System.Drawing.Point(162, 23);
      this.mpLabel2.Name = "mpLabel2";
      this.mpLabel2.Size = new System.Drawing.Size(10, 13);
      this.mpLabel2.TabIndex = 2;
      this.mpLabel2.Text = ":";
      // 
      // hoursTextBox
      // 
      this.hoursTextBox.BorderColor = System.Drawing.Color.Empty;
      this.hoursTextBox.Location = new System.Drawing.Point(135, 20);
      this.hoursTextBox.Name = "hoursTextBox";
      this.hoursTextBox.Size = new System.Drawing.Size(27, 20);
      this.hoursTextBox.TabIndex = 1;
      this.hoursTextBox.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.hoursTextBox_KeyPress);
      // 
      // mpLabel1
      // 
      this.mpLabel1.AutoSize = true;
      this.mpLabel1.Location = new System.Drawing.Point(7, 23);
      this.mpLabel1.Name = "mpLabel1";
      this.mpLabel1.Size = new System.Drawing.Size(126, 13);
      this.mpLabel1.TabIndex = 0;
      this.mpLabel1.Text = "Do a Music DB Reorg at:";
      // 
      // mpGroupBox2
      // 
      this.mpGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.mpGroupBox2.Controls.Add(this.cbSunday);
      this.mpGroupBox2.Controls.Add(this.cbSaturday);
      this.mpGroupBox2.Controls.Add(this.cbFriday);
      this.mpGroupBox2.Controls.Add(this.cbThursday);
      this.mpGroupBox2.Controls.Add(this.cbWednesday);
      this.mpGroupBox2.Controls.Add(this.cbTuesday);
      this.mpGroupBox2.Controls.Add(this.cbMonday);
      this.mpGroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox2.Location = new System.Drawing.Point(12, 67);
      this.mpGroupBox2.Name = "mpGroupBox2";
      this.mpGroupBox2.Size = new System.Drawing.Size(273, 130);
      this.mpGroupBox2.TabIndex = 1;
      this.mpGroupBox2.TabStop = false;
      this.mpGroupBox2.Text = "Run on";
      // 
      // cbSunday
      // 
      this.cbSunday.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cbSunday.AutoSize = true;
      this.cbSunday.Checked = true;
      this.cbSunday.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbSunday.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cbSunday.Location = new System.Drawing.Point(165, 65);
      this.cbSunday.Name = "cbSunday";
      this.cbSunday.Size = new System.Drawing.Size(60, 17);
      this.cbSunday.TabIndex = 6;
      this.cbSunday.Text = "Sunday";
      this.cbSunday.UseVisualStyleBackColor = true;
      // 
      // cbSaturday
      // 
      this.cbSaturday.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cbSaturday.AutoSize = true;
      this.cbSaturday.Checked = true;
      this.cbSaturday.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbSaturday.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cbSaturday.Location = new System.Drawing.Point(165, 42);
      this.cbSaturday.Name = "cbSaturday";
      this.cbSaturday.Size = new System.Drawing.Size(66, 17);
      this.cbSaturday.TabIndex = 5;
      this.cbSaturday.Text = "Saturday";
      this.cbSaturday.UseVisualStyleBackColor = true;
      // 
      // cbFriday
      // 
      this.cbFriday.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.cbFriday.AutoSize = true;
      this.cbFriday.Checked = true;
      this.cbFriday.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbFriday.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cbFriday.Location = new System.Drawing.Point(165, 19);
      this.cbFriday.Name = "cbFriday";
      this.cbFriday.Size = new System.Drawing.Size(52, 17);
      this.cbFriday.TabIndex = 4;
      this.cbFriday.Text = "Friday";
      this.cbFriday.UseVisualStyleBackColor = true;
      // 
      // cbThursday
      // 
      this.cbThursday.AutoSize = true;
      this.cbThursday.Checked = true;
      this.cbThursday.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbThursday.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cbThursday.Location = new System.Drawing.Point(10, 92);
      this.cbThursday.Name = "cbThursday";
      this.cbThursday.Size = new System.Drawing.Size(68, 17);
      this.cbThursday.TabIndex = 3;
      this.cbThursday.Text = "Thursday";
      this.cbThursday.UseVisualStyleBackColor = true;
      // 
      // cbWednesday
      // 
      this.cbWednesday.AutoSize = true;
      this.cbWednesday.Checked = true;
      this.cbWednesday.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbWednesday.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cbWednesday.Location = new System.Drawing.Point(10, 67);
      this.cbWednesday.Name = "cbWednesday";
      this.cbWednesday.Size = new System.Drawing.Size(81, 17);
      this.cbWednesday.TabIndex = 2;
      this.cbWednesday.Text = "Wednesday";
      this.cbWednesday.UseVisualStyleBackColor = true;
      // 
      // cbTuesday
      // 
      this.cbTuesday.AutoSize = true;
      this.cbTuesday.Checked = true;
      this.cbTuesday.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbTuesday.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cbTuesday.Location = new System.Drawing.Point(10, 44);
      this.cbTuesday.Name = "cbTuesday";
      this.cbTuesday.Size = new System.Drawing.Size(65, 17);
      this.cbTuesday.TabIndex = 1;
      this.cbTuesday.Text = "Tuesday";
      this.cbTuesday.UseVisualStyleBackColor = true;
      // 
      // cbMonday
      // 
      this.cbMonday.AutoSize = true;
      this.cbMonday.Checked = true;
      this.cbMonday.CheckState = System.Windows.Forms.CheckState.Checked;
      this.cbMonday.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.cbMonday.Location = new System.Drawing.Point(10, 20);
      this.cbMonday.Name = "cbMonday";
      this.cbMonday.Size = new System.Drawing.Size(62, 17);
      this.cbMonday.TabIndex = 0;
      this.cbMonday.Text = "Monday";
      this.cbMonday.UseVisualStyleBackColor = true;
      // 
      // btnOK
      // 
      this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnOK.Location = new System.Drawing.Point(129, 203);
      this.btnOK.Name = "btnOK";
      this.btnOK.Size = new System.Drawing.Size(75, 23);
      this.btnOK.TabIndex = 2;
      this.btnOK.Text = "&OK";
      this.btnOK.UseVisualStyleBackColor = true;
      this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
      // 
      // btnCancel
      // 
      this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.btnCancel.Location = new System.Drawing.Point(210, 203);
      this.btnCancel.Name = "btnCancel";
      this.btnCancel.Size = new System.Drawing.Size(75, 23);
      this.btnCancel.TabIndex = 3;
      this.btnCancel.Text = "&Cancel";
      this.btnCancel.UseVisualStyleBackColor = true;
      this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
      // 
      // MusicDBReorgSettings
      // 
      this.AcceptButton = this.btnOK;
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.CancelButton = this.btnCancel;
      this.ClientSize = new System.Drawing.Size(297, 238);
      this.Controls.Add(this.btnCancel);
      this.Controls.Add(this.btnOK);
      this.Controls.Add(this.mpGroupBox2);
      this.Controls.Add(this.mpGroupBox1);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
      this.Name = "MusicDBReorgSettings";
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "MusicDB Reorganisation - Setup";
      this.mpGroupBox1.ResumeLayout(false);
      this.mpGroupBox1.PerformLayout();
      this.mpGroupBox2.ResumeLayout(false);
      this.mpGroupBox2.PerformLayout();
      this.ResumeLayout(false);

    }

    #endregion

    private MediaPortal.UserInterface.Controls.MPGroupBox mpGroupBox1;
    private MediaPortal.UserInterface.Controls.MPTextBox hoursTextBox;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel1;
    private MediaPortal.UserInterface.Controls.MPTextBox minutesTextBox;
    private MediaPortal.UserInterface.Controls.MPLabel mpLabel2;
    private MediaPortal.UserInterface.Controls.MPGroupBox mpGroupBox2;
    private MediaPortal.UserInterface.Controls.MPCheckBox cbSunday;
    private MediaPortal.UserInterface.Controls.MPCheckBox cbSaturday;
    private MediaPortal.UserInterface.Controls.MPCheckBox cbFriday;
    private MediaPortal.UserInterface.Controls.MPCheckBox cbThursday;
    private MediaPortal.UserInterface.Controls.MPCheckBox cbWednesday;
    private MediaPortal.UserInterface.Controls.MPCheckBox cbTuesday;
    private MediaPortal.UserInterface.Controls.MPCheckBox cbMonday;
    private MediaPortal.UserInterface.Controls.MPButton btnOK;
    private MediaPortal.UserInterface.Controls.MPButton btnCancel;
  }
}