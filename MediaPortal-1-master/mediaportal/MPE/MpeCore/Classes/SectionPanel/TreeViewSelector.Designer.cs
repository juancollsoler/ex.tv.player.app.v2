﻿namespace MpeCore.Classes.SectionPanel
{
    partial class TreeViewSelector
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
      this.lbl_description = new System.Windows.Forms.Label();
      this.groupBox1 = new System.Windows.Forms.GroupBox();
      this.treeView1 = new System.Windows.Forms.TreeView();
      this.label1 = new System.Windows.Forms.Label();
      this.groupBox1.SuspendLayout();
      this.SuspendLayout();
      // 
      // button_next
      // 
      this.button_next.Text = "Next >";
      // 
      // lbl_description
      // 
      this.lbl_description.Dock = System.Windows.Forms.DockStyle.Fill;
      this.lbl_description.Location = new System.Drawing.Point(3, 16);
      this.lbl_description.Margin = new System.Windows.Forms.Padding(3);
      this.lbl_description.Name = "lbl_description";
      this.lbl_description.Padding = new System.Windows.Forms.Padding(3);
      this.lbl_description.Size = new System.Drawing.Size(164, 182);
      this.lbl_description.TabIndex = 0;
      // 
      // groupBox1
      // 
      this.groupBox1.Controls.Add(this.lbl_description);
      this.groupBox1.Location = new System.Drawing.Point(312, 99);
      this.groupBox1.Name = "groupBox1";
      this.groupBox1.Size = new System.Drawing.Size(170, 201);
      this.groupBox1.TabIndex = 25;
      this.groupBox1.TabStop = false;
      this.groupBox1.Text = "Description";
      // 
      // treeView1
      // 
      this.treeView1.CheckBoxes = true;
      this.treeView1.Location = new System.Drawing.Point(6, 106);
      this.treeView1.Name = "treeView1";
      this.treeView1.Size = new System.Drawing.Size(300, 194);
      this.treeView1.TabIndex = 24;
      this.treeView1.AfterCheck += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterCheck);
      this.treeView1.AfterSelect += new System.Windows.Forms.TreeViewEventHandler(this.treeView1_AfterSelect);
      // 
      // label1
      // 
      this.label1.Location = new System.Drawing.Point(6, 73);
      this.label1.Name = "label1";
      this.label1.Size = new System.Drawing.Size(476, 30);
      this.label1.TabIndex = 23;
      this.label1.Text = "label1";
      // 
      // TreeViewSelector
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
      this.ClientSize = new System.Drawing.Size(494, 350);
      this.Controls.Add(this.groupBox1);
      this.Controls.Add(this.treeView1);
      this.Controls.Add(this.label1);
      this.Name = "TreeViewSelector";
      this.Text = "Extension Installer for   - 0.0.0.0";
      this.Controls.SetChildIndex(this.label1, 0);
      this.Controls.SetChildIndex(this.treeView1, 0);
      this.Controls.SetChildIndex(this.groupBox1, 0);
      this.groupBox1.ResumeLayout(false);
      this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Label lbl_description;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TreeView treeView1;
        private System.Windows.Forms.Label label1;

    }
}