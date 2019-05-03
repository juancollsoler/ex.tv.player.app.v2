﻿namespace SetupTv.Dialogs
{
  partial class FormSelectListChannel
  {
    /// <summary>
    /// Erforderliche Designervariable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Verwendete Ressourcen bereinigen.
    /// </summary>
    /// <param name="disposing">True, wenn verwaltete Ressourcen gelöscht werden sollen; andernfalls False.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Vom Windows Form-Designer generierter Code

    /// <summary>
    /// Erforderliche Methode für die Designerunterstützung.
    /// Der Inhalt der Methode darf nicht mit dem Code-Editor geändert werden.
    /// </summary>
    private void InitializeComponent()
    {
      this.listViewChannels = new System.Windows.Forms.ListView();
      this.checkBoxGuideChannels = new System.Windows.Forms.CheckBox();
      this.mpButtonCancel = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonOk = new MediaPortal.UserInterface.Controls.MPButton();
      this.SuspendLayout();
      // 
      // listViewChannels
      // 
      this.listViewChannels.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.listViewChannels.CheckBoxes = true;
      this.listViewChannels.FullRowSelect = true;
      this.listViewChannels.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
      this.listViewChannels.LabelWrap = false;
      this.listViewChannels.Location = new System.Drawing.Point(12, 30);
      this.listViewChannels.MultiSelect = false;
      this.listViewChannels.Name = "listViewChannels";
      this.listViewChannels.Size = new System.Drawing.Size(668, 430);
      this.listViewChannels.TabIndex = 53;
      this.listViewChannels.UseCompatibleStateImageBehavior = false;
      this.listViewChannels.View = System.Windows.Forms.View.List;
      this.listViewChannels.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewChannels_ItemChecked);
      this.listViewChannels.ItemSelectionChanged += new System.Windows.Forms.ListViewItemSelectionChangedEventHandler(this.listViewChannels_ItemSelectionChanged);
      // 
      // checkBoxGuideChannels
      // 
      this.checkBoxGuideChannels.AutoSize = true;
      this.checkBoxGuideChannels.Checked = true;
      this.checkBoxGuideChannels.CheckState = System.Windows.Forms.CheckState.Checked;
      this.checkBoxGuideChannels.Location = new System.Drawing.Point(12, 7);
      this.checkBoxGuideChannels.Name = "checkBoxGuideChannels";
      this.checkBoxGuideChannels.Size = new System.Drawing.Size(165, 17);
      this.checkBoxGuideChannels.TabIndex = 54;
      this.checkBoxGuideChannels.Text = "Only channels visible in guide";
      this.checkBoxGuideChannels.UseVisualStyleBackColor = true;
      this.checkBoxGuideChannels.CheckedChanged += new System.EventHandler(this.checkBoxGuideChannels_CheckedChanged);
      // 
      // mpButtonCancel
      // 
      this.mpButtonCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.mpButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.mpButtonCancel.Location = new System.Drawing.Point(605, 466);
      this.mpButtonCancel.Name = "mpButtonCancel";
      this.mpButtonCancel.Size = new System.Drawing.Size(75, 23);
      this.mpButtonCancel.TabIndex = 52;
      this.mpButtonCancel.Text = "&Cancel";
      this.mpButtonCancel.UseVisualStyleBackColor = true;
      this.mpButtonCancel.Click += new System.EventHandler(this.mpButtonCancel_Click);
      // 
      // mpButtonOk
      // 
      this.mpButtonOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
      this.mpButtonOk.Location = new System.Drawing.Point(524, 466);
      this.mpButtonOk.Name = "mpButtonOk";
      this.mpButtonOk.Size = new System.Drawing.Size(75, 23);
      this.mpButtonOk.TabIndex = 51;
      this.mpButtonOk.Text = "&OK";
      this.mpButtonOk.UseVisualStyleBackColor = true;
      this.mpButtonOk.Click += new System.EventHandler(this.mpButtonOk_Click);
      // 
      // FormSelectListChannel
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.ClientSize = new System.Drawing.Size(692, 501);
      this.Controls.Add(this.checkBoxGuideChannels);
      this.Controls.Add(this.listViewChannels);
      this.Controls.Add(this.mpButtonCancel);
      this.Controls.Add(this.mpButtonOk);
      this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
      this.Name = "FormSelectListChannel";
      this.Text = "FormSelectListChannel";
      this.ResumeLayout(false);
      this.PerformLayout();

    }

    #endregion

    private MediaPortal.UserInterface.Controls.MPButton mpButtonCancel;
    private MediaPortal.UserInterface.Controls.MPButton mpButtonOk;
    private System.Windows.Forms.ListView listViewChannels;
    private System.Windows.Forms.CheckBox checkBoxGuideChannels;
  }
}