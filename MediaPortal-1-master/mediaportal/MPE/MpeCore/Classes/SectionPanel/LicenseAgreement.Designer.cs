﻿namespace MpeCore.Classes.SectionPanel
{
    partial class LicenseAgreement
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
      this.richTextBox1 = new System.Windows.Forms.RichTextBox();
      this.checkBox1 = new System.Windows.Forms.CheckBox();
      this.SuspendLayout();
      // 
      // richTextBox1
      // 
      this.richTextBox1.BackColor = System.Drawing.SystemColors.Window;
      this.richTextBox1.Dock = System.Windows.Forms.DockStyle.Fill;
      this.richTextBox1.Location = new System.Drawing.Point(0, 65);
      this.richTextBox1.Name = "richTextBox1";
      this.richTextBox1.ReadOnly = true;
      this.richTextBox1.Size = new System.Drawing.Size(494, 241);
      this.richTextBox1.TabIndex = 14;
      this.richTextBox1.Text = "";
      // 
      // checkBox1
      // 
      this.checkBox1.Dock = System.Windows.Forms.DockStyle.Bottom;
      this.checkBox1.Location = new System.Drawing.Point(0, 287);
      this.checkBox1.Name = "checkBox1";
      this.checkBox1.Padding = new System.Windows.Forms.Padding(5, 0, 0, 0);
      this.checkBox1.Size = new System.Drawing.Size(494, 19);
      this.checkBox1.TabIndex = 23;
      this.checkBox1.Text = "I accept the terms of the license agreement.";
      this.checkBox1.UseVisualStyleBackColor = true;
      this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
      // 
      // LicenseAgreement
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(494, 350);
      this.Controls.Add(this.checkBox1);
      this.Controls.Add(this.richTextBox1);
      this.Name = "LicenseAgreement";
      this.Text = "Extension Installer for   - 0.0.0.0";
      this.Controls.SetChildIndex(this.richTextBox1, 0);
      this.Controls.SetChildIndex(this.checkBox1, 0);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}