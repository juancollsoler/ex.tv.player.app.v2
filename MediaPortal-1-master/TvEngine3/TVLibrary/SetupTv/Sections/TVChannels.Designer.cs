namespace SetupTv.Sections
{
  partial class TvChannels
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TvChannels));
      this.mpListView1 = new MediaPortal.UserInterface.Controls.MPListView();
      this.hdrhekje = new System.Windows.Forms.ColumnHeader();
      this.hdrGroup = new System.Windows.Forms.ColumnHeader();
      this.hdrProvider = new System.Windows.Forms.ColumnHeader();
      this.hdrTypes = new System.Windows.Forms.ColumnHeader();
      this.hdrNumberOfDetails = new System.Windows.Forms.ColumnHeader();
      this.channelListContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.addToFavoritesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteThisChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.editChannelToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
      this.renameMarkedChannelsBySIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.addSIDInFrontOfNameToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.renumberChannelsBySIDToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.imageList1 = new System.Windows.Forms.ImageList(this.components);
      this.mpLabelChannelCount = new MediaPortal.UserInterface.Controls.MPLabel();
      this.tabControl1 = new MediaPortal.UserInterface.Controls.MPTabControl();
      this.tabPage1 = new System.Windows.Forms.TabPage();
      this.lblFilterLabel = new System.Windows.Forms.Label();
      this.txtFilterString = new System.Windows.Forms.TextBox();
      this.label2 = new System.Windows.Forms.Label();
      this.mpGroupBox1 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.mpButtonDown = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonUp = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonTestAvailable = new System.Windows.Forms.Button();
      this.mpButtonAdd = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonEdit = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonDel = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonPreview = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonClear = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpGroupBox3 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.mpButtonDeleteEncrypted = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonUncheckEncrypted = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpGroupBox2 = new MediaPortal.UserInterface.Controls.MPGroupBox();
      this.mpButtonDelGroup = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonAddGroup = new MediaPortal.UserInterface.Controls.MPButton();
      this.mpButtonRenameGroup = new MediaPortal.UserInterface.Controls.MPButton();
      this.tableLayoutPanel4 = new System.Windows.Forms.TableLayoutPanel();
      this.groupTabContextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
      this.renameGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.deleteGroupToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
      this.hdrChannelNumber = new System.Windows.Forms.ColumnHeader();
      this.channelListContextMenuStrip.SuspendLayout();
      this.tabControl1.SuspendLayout();
      this.tabPage1.SuspendLayout();
      this.mpGroupBox1.SuspendLayout();
      this.mpGroupBox3.SuspendLayout();
      this.mpGroupBox2.SuspendLayout();
      this.groupTabContextMenuStrip.SuspendLayout();
      this.SuspendLayout();
      // 
      // mpListView1
      // 
      this.mpListView1.AllowDrop = true;
      this.mpListView1.AllowRowReorder = true;
      this.mpListView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.mpListView1.CheckBoxes = true;
      this.mpListView1.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.hdrhekje,
            this.hdrChannelNumber,
            this.hdrGroup,
            this.hdrProvider,
            this.hdrTypes,
            this.hdrNumberOfDetails});
      this.mpListView1.ContextMenuStrip = this.channelListContextMenuStrip;
      this.mpListView1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8F);
      this.mpListView1.FullRowSelect = true;
      this.mpListView1.IsChannelListView = false;
      this.mpListView1.LabelEdit = true;
      this.mpListView1.LargeImageList = this.imageList1;
      this.mpListView1.Location = new System.Drawing.Point(9, 75);
      this.mpListView1.Name = "mpListView1";
      this.mpListView1.Size = new System.Drawing.Size(438, 189);
      this.mpListView1.SmallImageList = this.imageList1;
      this.mpListView1.TabIndex = 0;
      this.mpListView1.UseCompatibleStateImageBehavior = false;
      this.mpListView1.View = System.Windows.Forms.View.Details;
      this.mpListView1.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.mpListView1_MouseDoubleClick);
      this.mpListView1.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.mpListView1_ItemChecked);
      this.mpListView1.AfterLabelEdit += new System.Windows.Forms.LabelEditEventHandler(this.mpListView1_AfterLabelEdit);
      this.mpListView1.ColumnClick += new System.Windows.Forms.ColumnClickEventHandler(this.mpListView1_ColumnClick);
      this.mpListView1.ItemDrag += new System.Windows.Forms.ItemDragEventHandler(this.mpListView1_ItemDrag);
      // 
      // hdrhekje
      // 
      this.hdrhekje.Text = "Name";
      this.hdrhekje.Width = 120;
      // 
      // hdrGroup
      // 
      this.hdrGroup.DisplayIndex = 1;
      this.hdrGroup.Text = "Groups";
      // 
      // hdrProvider
      // 
      this.hdrProvider.DisplayIndex = 2;
      this.hdrProvider.Text = "Provider";
      // 
      // hdrTypes
      // 
      this.hdrTypes.DisplayIndex = 3;
      this.hdrTypes.Text = "Mapped Cardtypes";
      this.hdrTypes.Width = 50;
      // 
      // hdrNumberOfDetails
      // 
      this.hdrNumberOfDetails.DisplayIndex = 4;
      this.hdrNumberOfDetails.Text = "Tuningdetails";
      // 
      // channelListContextMenuStrip
      // 
      this.channelListContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addToFavoritesToolStripMenuItem,
            this.deleteThisChannelToolStripMenuItem,
            this.editChannelToolStripMenuItem,
            this.toolStripMenuItem1,
            this.renameMarkedChannelsBySIDToolStripMenuItem,
            this.addSIDInFrontOfNameToolStripMenuItem,
            this.renumberChannelsBySIDToolStripMenuItem});
      this.channelListContextMenuStrip.Name = "contextMenuStrip1";
      this.channelListContextMenuStrip.Size = new System.Drawing.Size(258, 142);
      // 
      // addToFavoritesToolStripMenuItem
      // 
      this.addToFavoritesToolStripMenuItem.Name = "addToFavoritesToolStripMenuItem";
      this.addToFavoritesToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
      this.addToFavoritesToolStripMenuItem.Text = "Add to group";
      this.addToFavoritesToolStripMenuItem.Click += new System.EventHandler(this.OnAddToFavoritesMenuItem_Click);
      // 
      // deleteThisChannelToolStripMenuItem
      // 
      this.deleteThisChannelToolStripMenuItem.Name = "deleteThisChannelToolStripMenuItem";
      this.deleteThisChannelToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
      this.deleteThisChannelToolStripMenuItem.Text = "Delete this channel";
      this.deleteThisChannelToolStripMenuItem.Click += new System.EventHandler(this.deleteThisChannelToolStripMenuItem_Click);
      // 
      // editChannelToolStripMenuItem
      // 
      this.editChannelToolStripMenuItem.Name = "editChannelToolStripMenuItem";
      this.editChannelToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
      this.editChannelToolStripMenuItem.Text = "Edit channel";
      this.editChannelToolStripMenuItem.Click += new System.EventHandler(this.editChannelToolStripMenuItem_Click);
      // 
      // toolStripMenuItem1
      // 
      this.toolStripMenuItem1.Name = "toolStripMenuItem1";
      this.toolStripMenuItem1.Size = new System.Drawing.Size(254, 6);
      // 
      // renameMarkedChannelsBySIDToolStripMenuItem
      // 
      this.renameMarkedChannelsBySIDToolStripMenuItem.Name = "renameMarkedChannelsBySIDToolStripMenuItem";
      this.renameMarkedChannelsBySIDToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
      this.renameMarkedChannelsBySIDToolStripMenuItem.Text = "Rename selected channel(s) by SID";
      this.renameMarkedChannelsBySIDToolStripMenuItem.Click += new System.EventHandler(this.renameMarkedChannelsBySIDToolStripMenuItem_Click);
      // 
      // addSIDInFrontOfNameToolStripMenuItem
      // 
      this.addSIDInFrontOfNameToolStripMenuItem.Name = "addSIDInFrontOfNameToolStripMenuItem";
      this.addSIDInFrontOfNameToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
      this.addSIDInFrontOfNameToolStripMenuItem.Text = "Add SID in front of name";
      this.addSIDInFrontOfNameToolStripMenuItem.Click += new System.EventHandler(this.addSIDInFrontOfNameToolStripMenuItem_Click);
      // 
      // renumberChannelsBySIDToolStripMenuItem
      // 
      this.renumberChannelsBySIDToolStripMenuItem.Name = "renumberChannelsBySIDToolStripMenuItem";
      this.renumberChannelsBySIDToolStripMenuItem.Size = new System.Drawing.Size(257, 22);
      this.renumberChannelsBySIDToolStripMenuItem.Text = "Renumber channels by SID";
      this.renumberChannelsBySIDToolStripMenuItem.Click += new System.EventHandler(this.renumberChannelsBySIDToolStripMenuItem_Click);
      // 
      // imageList1
      // 
      this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
      this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
      this.imageList1.Images.SetKeyName(0, "radio_fta_.png");
      this.imageList1.Images.SetKeyName(1, "radio_scrambled.png");
      this.imageList1.Images.SetKeyName(2, "icon.radio_scrambled_and_fta.png");
      this.imageList1.Images.SetKeyName(3, "tv_fta_.png");
      this.imageList1.Images.SetKeyName(4, "tv_scrambled.png");
      this.imageList1.Images.SetKeyName(5, "icon.tv_scrambled_and_fta.png");
      // 
      // mpLabelChannelCount
      // 
      this.mpLabelChannelCount.AutoSize = true;
      this.mpLabelChannelCount.Location = new System.Drawing.Point(380, 7);
      this.mpLabelChannelCount.Name = "mpLabelChannelCount";
      this.mpLabelChannelCount.Size = new System.Drawing.Size(0, 13);
      this.mpLabelChannelCount.TabIndex = 2;
      // 
      // tabControl1
      // 
      this.tabControl1.AllowDrop = true;
      this.tabControl1.AllowReorderTabs = false;
      this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                  | System.Windows.Forms.AnchorStyles.Left)
                  | System.Windows.Forms.AnchorStyles.Right)));
      this.tabControl1.Controls.Add(this.tabPage1);
      this.tabControl1.Location = new System.Drawing.Point(3, 3);
      this.tabControl1.Name = "tabControl1";
      this.tabControl1.SelectedIndex = 0;
      this.tabControl1.Size = new System.Drawing.Size(465, 400);
      this.tabControl1.TabIndex = 8;
      this.tabControl1.DragOver += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragOver);
      this.tabControl1.DragDrop += new System.Windows.Forms.DragEventHandler(this.tabControl1_DragDrop);
      this.tabControl1.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tabControl1_MouseClick);
      this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
      // 
      // tabPage1
      // 
      this.tabPage1.Controls.Add(this.lblFilterLabel);
      this.tabPage1.Controls.Add(this.txtFilterString);
      this.tabPage1.Controls.Add(this.label2);
      this.tabPage1.Controls.Add(this.mpListView1);
      this.tabPage1.Controls.Add(this.mpGroupBox1);
      this.tabPage1.Controls.Add(this.mpGroupBox3);
      this.tabPage1.Controls.Add(this.mpGroupBox2);
      this.tabPage1.Controls.Add(this.mpLabelChannelCount);
      this.tabPage1.Location = new System.Drawing.Point(4, 22);
      this.tabPage1.Name = "tabPage1";
      this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
      this.tabPage1.Size = new System.Drawing.Size(457, 374);
      this.tabPage1.TabIndex = 0;
      this.tabPage1.Text = "Channels";
      this.tabPage1.UseVisualStyleBackColor = true;
      // 
      // lblFilterLabel
      // 
      this.lblFilterLabel.AutoSize = true;
      this.lblFilterLabel.Location = new System.Drawing.Point(6, 52);
      this.lblFilterLabel.Name = "lblFilterLabel";
      this.lblFilterLabel.Size = new System.Drawing.Size(116, 13);
      this.lblFilterLabel.TabIndex = 34;
      this.lblFilterLabel.Text = "Filter by Channel Name";
      // 
      // txtFilterString
      // 
      this.txtFilterString.Location = new System.Drawing.Point(126, 49);
      this.txtFilterString.Name = "txtFilterString";
      this.txtFilterString.Size = new System.Drawing.Size(321, 20);
      this.txtFilterString.TabIndex = 33;
      this.txtFilterString.TextChanged += new System.EventHandler(this.txtFilterString_TextChanged);
      // 
      // label2
      // 
      this.label2.AutoSize = true;
      this.label2.Location = new System.Drawing.Point(6, 2);
      this.label2.Name = "label2";
      this.label2.Size = new System.Drawing.Size(414, 39);
      this.label2.TabIndex = 32;
      this.label2.Text = resources.GetString("label2.Text");
      this.label2.UseMnemonic = false;
      // 
      // mpGroupBox1
      // 
      this.mpGroupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.mpGroupBox1.Controls.Add(this.mpButtonDown);
      this.mpGroupBox1.Controls.Add(this.mpButtonUp);
      this.mpGroupBox1.Controls.Add(this.mpButtonTestAvailable);
      this.mpGroupBox1.Controls.Add(this.mpButtonAdd);
      this.mpGroupBox1.Controls.Add(this.mpButtonEdit);
      this.mpGroupBox1.Controls.Add(this.mpButtonDel);
      this.mpGroupBox1.Controls.Add(this.mpButtonPreview);
      this.mpGroupBox1.Controls.Add(this.mpButtonClear);
      this.mpGroupBox1.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox1.Location = new System.Drawing.Point(9, 270);
      this.mpGroupBox1.Name = "mpGroupBox1";
      this.mpGroupBox1.Size = new System.Drawing.Size(234, 97);
      this.mpGroupBox1.TabIndex = 25;
      this.mpGroupBox1.TabStop = false;
      this.mpGroupBox1.Text = "Channels";
      // 
      // mpButtonDown
      // 
      this.mpButtonDown.Image = global::SetupTv.Properties.Resources.icon_down;
      this.mpButtonDown.Location = new System.Drawing.Point(197, 42);
      this.mpButtonDown.Name = "mpButtonDown";
      this.mpButtonDown.Size = new System.Drawing.Size(30, 23);
      this.mpButtonDown.TabIndex = 25;
      this.mpButtonDown.UseVisualStyleBackColor = true;
      this.mpButtonDown.Click += new System.EventHandler(this.mpButtonDown_Click);
      // 
      // mpButtonUp
      // 
      this.mpButtonUp.Image = global::SetupTv.Properties.Resources.icon_up;
      this.mpButtonUp.Location = new System.Drawing.Point(197, 16);
      this.mpButtonUp.Name = "mpButtonUp";
      this.mpButtonUp.Size = new System.Drawing.Size(30, 23);
      this.mpButtonUp.TabIndex = 24;
      this.mpButtonUp.UseVisualStyleBackColor = true;
      this.mpButtonUp.Click += new System.EventHandler(this.mpButtonUp_Click);
      // 
      // mpButtonTestAvailable
      // 
      this.mpButtonTestAvailable.Location = new System.Drawing.Point(69, 68);
      this.mpButtonTestAvailable.Margin = new System.Windows.Forms.Padding(2);
      this.mpButtonTestAvailable.Name = "mpButtonTestAvailable";
      this.mpButtonTestAvailable.Size = new System.Drawing.Size(125, 23);
      this.mpButtonTestAvailable.TabIndex = 27;
      this.mpButtonTestAvailable.Text = "Test timeshifting";
      this.mpButtonTestAvailable.UseVisualStyleBackColor = true;
      this.mpButtonTestAvailable.Click += new System.EventHandler(this.mpButtonTestAvailable_Click);
      // 
      // mpButtonAdd
      // 
      this.mpButtonAdd.Location = new System.Drawing.Point(68, 16);
      this.mpButtonAdd.Name = "mpButtonAdd";
      this.mpButtonAdd.Size = new System.Drawing.Size(60, 23);
      this.mpButtonAdd.TabIndex = 21;
      this.mpButtonAdd.Text = "&Add";
      this.mpButtonAdd.UseVisualStyleBackColor = true;
      this.mpButtonAdd.Click += new System.EventHandler(this.mpButtonAdd_Click);
      // 
      // mpButtonEdit
      // 
      this.mpButtonEdit.Location = new System.Drawing.Point(68, 42);
      this.mpButtonEdit.Name = "mpButtonEdit";
      this.mpButtonEdit.Size = new System.Drawing.Size(60, 23);
      this.mpButtonEdit.TabIndex = 22;
      this.mpButtonEdit.Text = "&Edit";
      this.mpButtonEdit.UseVisualStyleBackColor = true;
      this.mpButtonEdit.Click += new System.EventHandler(this.mpButtonEdit_Click);
      // 
      // mpButtonDel
      // 
      this.mpButtonDel.Location = new System.Drawing.Point(134, 16);
      this.mpButtonDel.Name = "mpButtonDel";
      this.mpButtonDel.Size = new System.Drawing.Size(60, 23);
      this.mpButtonDel.TabIndex = 23;
      this.mpButtonDel.Text = "&Delete";
      this.mpButtonDel.UseVisualStyleBackColor = true;
      this.mpButtonDel.Click += new System.EventHandler(this.mpButtonDel_Click);
      // 
      // mpButtonPreview
      // 
      this.mpButtonPreview.Location = new System.Drawing.Point(6, 16);
      this.mpButtonPreview.Name = "mpButtonPreview";
      this.mpButtonPreview.Size = new System.Drawing.Size(56, 75);
      this.mpButtonPreview.TabIndex = 16;
      this.mpButtonPreview.Text = "Pre&view";
      this.mpButtonPreview.UseVisualStyleBackColor = true;
      this.mpButtonPreview.Click += new System.EventHandler(this.mpButtonPreview_Click);
      // 
      // mpButtonClear
      // 
      this.mpButtonClear.Location = new System.Drawing.Point(134, 42);
      this.mpButtonClear.Name = "mpButtonClear";
      this.mpButtonClear.Size = new System.Drawing.Size(60, 23);
      this.mpButtonClear.TabIndex = 18;
      this.mpButtonClear.Text = "Clear";
      this.mpButtonClear.UseVisualStyleBackColor = true;
      this.mpButtonClear.Click += new System.EventHandler(this.mpButtonClear_Click);
      // 
      // mpGroupBox3
      // 
      this.mpGroupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.mpGroupBox3.Controls.Add(this.mpButtonDeleteEncrypted);
      this.mpGroupBox3.Controls.Add(this.mpButtonUncheckEncrypted);
      this.mpGroupBox3.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox3.Location = new System.Drawing.Point(249, 321);
      this.mpGroupBox3.Name = "mpGroupBox3";
      this.mpGroupBox3.Size = new System.Drawing.Size(202, 46);
      this.mpGroupBox3.TabIndex = 24;
      this.mpGroupBox3.TabStop = false;
      this.mpGroupBox3.Text = "Scrambled/encrypted channels";
      // 
      // mpButtonDeleteEncrypted
      // 
      this.mpButtonDeleteEncrypted.Location = new System.Drawing.Point(74, 17);
      this.mpButtonDeleteEncrypted.Name = "mpButtonDeleteEncrypted";
      this.mpButtonDeleteEncrypted.Size = new System.Drawing.Size(60, 23);
      this.mpButtonDeleteEncrypted.TabIndex = 29;
      this.mpButtonDeleteEncrypted.Text = "Delete";
      this.mpButtonDeleteEncrypted.UseVisualStyleBackColor = true;
      this.mpButtonDeleteEncrypted.Click += new System.EventHandler(this.mpButtonDeleteEncrypted_Click);
      // 
      // mpButtonUncheckEncrypted
      // 
      this.mpButtonUncheckEncrypted.Location = new System.Drawing.Point(8, 17);
      this.mpButtonUncheckEncrypted.Name = "mpButtonUncheckEncrypted";
      this.mpButtonUncheckEncrypted.Size = new System.Drawing.Size(60, 23);
      this.mpButtonUncheckEncrypted.TabIndex = 28;
      this.mpButtonUncheckEncrypted.Text = "Uncheck";
      this.mpButtonUncheckEncrypted.UseVisualStyleBackColor = true;
      this.mpButtonUncheckEncrypted.Click += new System.EventHandler(this.mpButtonUncheckEncrypted_Click);
      // 
      // mpGroupBox2
      // 
      this.mpGroupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
      this.mpGroupBox2.Controls.Add(this.mpButtonDelGroup);
      this.mpGroupBox2.Controls.Add(this.mpButtonAddGroup);
      this.mpGroupBox2.Controls.Add(this.mpButtonRenameGroup);
      this.mpGroupBox2.FlatStyle = System.Windows.Forms.FlatStyle.Popup;
      this.mpGroupBox2.Location = new System.Drawing.Point(249, 270);
      this.mpGroupBox2.Name = "mpGroupBox2";
      this.mpGroupBox2.Size = new System.Drawing.Size(202, 46);
      this.mpGroupBox2.TabIndex = 23;
      this.mpGroupBox2.TabStop = false;
      this.mpGroupBox2.Text = "Channel groups";
      // 
      // mpButtonDelGroup
      // 
      this.mpButtonDelGroup.Location = new System.Drawing.Point(134, 16);
      this.mpButtonDelGroup.Name = "mpButtonDelGroup";
      this.mpButtonDelGroup.Size = new System.Drawing.Size(60, 23);
      this.mpButtonDelGroup.TabIndex = 31;
      this.mpButtonDelGroup.Text = "Delete";
      this.mpButtonDelGroup.UseVisualStyleBackColor = true;
      this.mpButtonDelGroup.Click += new System.EventHandler(this.mpButtonDelGroup_Click);
      // 
      // mpButtonAddGroup
      // 
      this.mpButtonAddGroup.Location = new System.Drawing.Point(8, 16);
      this.mpButtonAddGroup.Name = "mpButtonAddGroup";
      this.mpButtonAddGroup.Size = new System.Drawing.Size(60, 23);
      this.mpButtonAddGroup.TabIndex = 29;
      this.mpButtonAddGroup.Text = "Add";
      this.mpButtonAddGroup.UseVisualStyleBackColor = true;
      this.mpButtonAddGroup.Click += new System.EventHandler(this.mpButtonAddGroup_Click);
      // 
      // mpButtonRenameGroup
      // 
      this.mpButtonRenameGroup.Location = new System.Drawing.Point(71, 16);
      this.mpButtonRenameGroup.Name = "mpButtonRenameGroup";
      this.mpButtonRenameGroup.Size = new System.Drawing.Size(60, 23);
      this.mpButtonRenameGroup.TabIndex = 30;
      this.mpButtonRenameGroup.Text = "Rename";
      this.mpButtonRenameGroup.UseVisualStyleBackColor = true;
      this.mpButtonRenameGroup.Click += new System.EventHandler(this.mpButtonRenameGroup_Click);
      // 
      // tableLayoutPanel4
      // 
      this.tableLayoutPanel4.ColumnCount = 3;
      this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 33.33333F));
      this.tableLayoutPanel4.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel4.Location = new System.Drawing.Point(0, 0);
      this.tableLayoutPanel4.Name = "tableLayoutPanel4";
      this.tableLayoutPanel4.RowCount = 1;
      this.tableLayoutPanel4.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
      this.tableLayoutPanel4.Size = new System.Drawing.Size(200, 100);
      this.tableLayoutPanel4.TabIndex = 0;
      // 
      // groupTabContextMenuStrip
      // 
      this.groupTabContextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.renameGroupToolStripMenuItem,
            this.deleteGroupToolStripMenuItem});
      this.groupTabContextMenuStrip.Name = "groupTabContextMenuStrip";
      this.groupTabContextMenuStrip.Size = new System.Drawing.Size(154, 48);
      // 
      // renameGroupToolStripMenuItem
      // 
      this.renameGroupToolStripMenuItem.Name = "renameGroupToolStripMenuItem";
      this.renameGroupToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
      this.renameGroupToolStripMenuItem.Text = "Rename Group";
      this.renameGroupToolStripMenuItem.Click += new System.EventHandler(this.renameGroupToolStripMenuItem_Click);
      // 
      // deleteGroupToolStripMenuItem
      // 
      this.deleteGroupToolStripMenuItem.Name = "deleteGroupToolStripMenuItem";
      this.deleteGroupToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
      this.deleteGroupToolStripMenuItem.Text = "Delete Group";
      this.deleteGroupToolStripMenuItem.Click += new System.EventHandler(this.deleteGroupToolStripMenuItem_Click);
      // 
      // hdrChannelNumber
      // 
      this.hdrChannelNumber.Text = "Channel number";
      // 
      // TvChannels
      // 
      this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.Controls.Add(this.tabControl1);
      this.Name = "TvChannels";
      this.Size = new System.Drawing.Size(474, 412);
      this.channelListContextMenuStrip.ResumeLayout(false);
      this.tabControl1.ResumeLayout(false);
      this.tabPage1.ResumeLayout(false);
      this.tabPage1.PerformLayout();
      this.mpGroupBox1.ResumeLayout(false);
      this.mpGroupBox3.ResumeLayout(false);
      this.mpGroupBox2.ResumeLayout(false);
      this.groupTabContextMenuStrip.ResumeLayout(false);
      this.ResumeLayout(false);

    }

    #endregion

    private MediaPortal.UserInterface.Controls.MPListView mpListView1;
    private System.Windows.Forms.ColumnHeader hdrTypes;
	private MediaPortal.UserInterface.Controls.MPLabel mpLabelChannelCount;
	private System.Windows.Forms.ColumnHeader hdrhekje;
  private MediaPortal.UserInterface.Controls.MPTabControl tabControl1;
	private System.Windows.Forms.TabPage tabPage1;
    private System.Windows.Forms.ContextMenuStrip channelListContextMenuStrip;
    private System.Windows.Forms.ToolStripMenuItem addToFavoritesToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem deleteThisChannelToolStripMenuItem;
    private System.Windows.Forms.ToolStripMenuItem editChannelToolStripMenuItem;
	private System.Windows.Forms.ImageList imageList1;
	private System.Windows.Forms.ColumnHeader hdrProvider;
    private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
    private System.Windows.Forms.ToolStripMenuItem renameMarkedChannelsBySIDToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem addSIDInFrontOfNameToolStripMenuItem;
	private System.Windows.Forms.ToolStripMenuItem renumberChannelsBySIDToolStripMenuItem;
	private System.Windows.Forms.TableLayoutPanel tableLayoutPanel4;
	private MediaPortal.UserInterface.Controls.MPGroupBox mpGroupBox2;
	private MediaPortal.UserInterface.Controls.MPGroupBox mpGroupBox1;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonAdd;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonEdit;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonDel;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonPreview;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonClear;
	private MediaPortal.UserInterface.Controls.MPGroupBox mpGroupBox3;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonDeleteEncrypted;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonUncheckEncrypted;
	private System.Windows.Forms.Button mpButtonTestAvailable;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonDelGroup;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonAddGroup;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonRenameGroup;
	private MediaPortal.UserInterface.Controls.MPButton mpButtonUp;
  private MediaPortal.UserInterface.Controls.MPButton mpButtonDown;
	private System.Windows.Forms.Label label2;
  private System.Windows.Forms.ColumnHeader hdrGroup;
  private System.Windows.Forms.ContextMenuStrip groupTabContextMenuStrip;
  private System.Windows.Forms.ToolStripMenuItem renameGroupToolStripMenuItem;
  private System.Windows.Forms.ToolStripMenuItem deleteGroupToolStripMenuItem;
  private System.Windows.Forms.ColumnHeader hdrNumberOfDetails;
  private System.Windows.Forms.TextBox txtFilterString;
  private System.Windows.Forms.Label lblFilterLabel;
  private System.Windows.Forms.ColumnHeader hdrChannelNumber;
  }
}
