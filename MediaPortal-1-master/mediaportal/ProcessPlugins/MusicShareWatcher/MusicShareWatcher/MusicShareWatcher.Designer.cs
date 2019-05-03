namespace MediaPortal.MusicShareWatcher
{
    partial class MusicShareWatcher
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
          this.components = new System.ComponentModel.Container();
          System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MusicShareWatcher));
          this.notifyIcon1 = new System.Windows.Forms.NotifyIcon(this.components);
          this.contextMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
          this.monitoringEnabledMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.closeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
          this.contextMenu.SuspendLayout();
          this.SuspendLayout();
          // 
          // notifyIcon1
          // 
          this.notifyIcon1.ContextMenuStrip = this.contextMenu;
          this.notifyIcon1.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.notifyIcon1.Text = "MediaPortal Music Share Watcher";
          this.notifyIcon1.Visible = true;
          // 
          // contextMenu
          // 
          this.contextMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.monitoringEnabledMenuItem,
            this.closeMenuItem});
          this.contextMenu.Name = "contextMenuStrip1";
          this.contextMenu.Size = new System.Drawing.Size(205, 48);
          // 
          // monitoringEnabledMenuItem
          // 
          this.monitoringEnabledMenuItem.Checked = true;
          this.monitoringEnabledMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
          this.monitoringEnabledMenuItem.Name = "monitoringEnabledMenuItem";
          this.monitoringEnabledMenuItem.Size = new System.Drawing.Size(204, 22);
          this.monitoringEnabledMenuItem.Text = "Monitoring Enabled";
          this.monitoringEnabledMenuItem.Click += new System.EventHandler(this.monitoringEnabledMenuItem_Click);
          // 
          // closeMenuItem
          // 
          this.closeMenuItem.Name = "closeMenuItem";
          this.closeMenuItem.Size = new System.Drawing.Size(204, 22);
          this.closeMenuItem.Text = "Close Music Share Watcher";
          this.closeMenuItem.Click += new System.EventHandler(this.closeMenuItem_Click);
          // 
          // MusicShareWatcher
          // 
          this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
          this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
          this.ClientSize = new System.Drawing.Size(131, 77);
          this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
          this.Name = "MusicShareWatcher";
          this.ShowInTaskbar = false;
          this.Text = "MP Music Share Watcher";
          this.WindowState = System.Windows.Forms.FormWindowState.Minimized;
          this.Resize += new System.EventHandler(this.OnResize);
          this.contextMenu.ResumeLayout(false);
          this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.NotifyIcon notifyIcon1;
        private System.Windows.Forms.ContextMenuStrip contextMenu;
        private System.Windows.Forms.ToolStripMenuItem closeMenuItem;
        private System.Windows.Forms.ToolStripMenuItem monitoringEnabledMenuItem;
    }
}

