namespace TG.INI.Controls
{
    partial class IniEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(IniEditor));
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.lstSections = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnAdd = new System.Windows.Forms.ToolStripButton();
            this.btnDelete = new System.Windows.Forms.ToolStripButton();
            this.KeyValue = new System.Windows.Forms.DataGridView();
            this.toolStrip2 = new System.Windows.Forms.ToolStrip();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.txtCommentChar = new System.Windows.Forms.ToolStripTextBox();
            this.barDialog = new System.Windows.Forms.Panel();
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.btnOk = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.KeyValueMenu = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.quoteValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.encryptValueToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.barStandalone = new System.Windows.Forms.ToolStrip();
            this.btnNew = new System.Windows.Forms.ToolStripButton();
            this.btnOpen = new System.Windows.Forms.ToolStripButton();
            this.btnSave = new System.Windows.Forms.ToolStripSplitButton();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ColKey = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KeyValue)).BeginInit();
            this.toolStrip2.SuspendLayout();
            this.barDialog.SuspendLayout();
            this.tableLayoutPanel1.SuspendLayout();
            this.KeyValueMenu.SuspendLayout();
            this.barStandalone.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.lstSections);
            this.splitContainer1.Panel1.Controls.Add(this.toolStrip1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.KeyValue);
            this.splitContainer1.Panel2.Controls.Add(this.toolStrip2);
            this.splitContainer1.Size = new System.Drawing.Size(614, 425);
            this.splitContainer1.SplitterDistance = 211;
            this.splitContainer1.TabIndex = 1;
            // 
            // lstSections
            // 
            this.lstSections.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1});
            this.lstSections.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lstSections.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstSections.FullRowSelect = true;
            this.lstSections.HideSelection = false;
            this.lstSections.Location = new System.Drawing.Point(0, 27);
            this.lstSections.MultiSelect = false;
            this.lstSections.Name = "lstSections";
            this.lstSections.Size = new System.Drawing.Size(211, 398);
            this.lstSections.TabIndex = 0;
            this.lstSections.UseCompatibleStateImageBehavior = false;
            this.lstSections.View = System.Windows.Forms.View.Details;
            this.lstSections.SelectedIndexChanged += new System.EventHandler(this.lstSections_SelectedIndexChanged);
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Sections";
            this.columnHeader1.Width = 200;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnAdd,
            this.btnDelete});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(211, 27);
            this.toolStrip1.TabIndex = 1;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnAdd
            // 
            this.btnAdd.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnAdd.Image = ((System.Drawing.Image)(resources.GetObject("btnAdd.Image")));
            this.btnAdd.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(24, 24);
            this.btnAdd.Text = "Add Section";
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // btnDelete
            // 
            this.btnDelete.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnDelete.Enabled = false;
            this.btnDelete.Image = ((System.Drawing.Image)(resources.GetObject("btnDelete.Image")));
            this.btnDelete.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(24, 24);
            this.btnDelete.Text = "Delete Section";
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // KeyValue
            // 
            this.KeyValue.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.AllCells;
            this.KeyValue.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.KeyValue.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColKey,
            this.ColValue});
            this.KeyValue.Dock = System.Windows.Forms.DockStyle.Fill;
            this.KeyValue.Location = new System.Drawing.Point(0, 25);
            this.KeyValue.Name = "KeyValue";
            this.KeyValue.Size = new System.Drawing.Size(399, 400);
            this.KeyValue.TabIndex = 0;
            this.KeyValue.VirtualMode = true;
            this.KeyValue.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.KeyValue_CellBeginEdit);
            this.KeyValue.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.KeyValue_CellContextMenuStripNeeded);
            this.KeyValue.CellEndEdit += new System.Windows.Forms.DataGridViewCellEventHandler(this.KeyValue_CellEndEdit);
            this.KeyValue.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.KeyValue_CellPainting);
            this.KeyValue.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.KeyValue_CellValueChanged);
            this.KeyValue.CellValueNeeded += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.KeyValue_CellValueNeeded);
            this.KeyValue.CellValuePushed += new System.Windows.Forms.DataGridViewCellValueEventHandler(this.KeyValue_CellValuePushed);
            this.KeyValue.RowsRemoved += new System.Windows.Forms.DataGridViewRowsRemovedEventHandler(this.KeyValue_RowsRemoved);
            this.KeyValue.SelectionChanged += new System.EventHandler(this.KeyValue_SelectionChanged);
            // 
            // toolStrip2
            // 
            this.toolStrip2.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.toolStrip2.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripLabel1,
            this.txtCommentChar});
            this.toolStrip2.Location = new System.Drawing.Point(0, 0);
            this.toolStrip2.Name = "toolStrip2";
            this.toolStrip2.Size = new System.Drawing.Size(399, 25);
            this.toolStrip2.TabIndex = 1;
            this.toolStrip2.Text = "toolStrip2";
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(118, 22);
            this.toolStripLabel1.Text = "Comment Character:";
            // 
            // txtCommentChar
            // 
            this.txtCommentChar.Name = "txtCommentChar";
            this.txtCommentChar.Size = new System.Drawing.Size(27, 25);
            // 
            // barDialog
            // 
            this.barDialog.Controls.Add(this.tableLayoutPanel1);
            this.barDialog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.barDialog.Location = new System.Drawing.Point(0, 425);
            this.barDialog.Name = "barDialog";
            this.barDialog.Size = new System.Drawing.Size(614, 61);
            this.barDialog.TabIndex = 2;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Controls.Add(this.btnOk, 0, 0);
            this.tableLayoutPanel1.Controls.Add(this.btnCancel, 1, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(405, 12);
            this.tableLayoutPanel1.Margin = new System.Windows.Forms.Padding(9);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(200, 37);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // btnOk
            // 
            this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOk.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnOk.Location = new System.Drawing.Point(3, 3);
            this.btnOk.Margin = new System.Windows.Forms.Padding(3, 3, 9, 3);
            this.btnOk.Name = "btnOk";
            this.btnOk.Size = new System.Drawing.Size(88, 31);
            this.btnOk.TabIndex = 0;
            this.btnOk.Text = "OK";
            this.btnOk.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.btnCancel.Location = new System.Drawing.Point(109, 3);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(9, 3, 3, 3);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(88, 31);
            this.btnCancel.TabIndex = 1;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // KeyValueMenu
            // 
            this.KeyValueMenu.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.KeyValueMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.quoteValueToolStripMenuItem,
            this.encryptValueToolStripMenuItem});
            this.KeyValueMenu.Name = "KeyValueMenu";
            this.KeyValueMenu.Size = new System.Drawing.Size(146, 48);
            // 
            // quoteValueToolStripMenuItem
            // 
            this.quoteValueToolStripMenuItem.Name = "quoteValueToolStripMenuItem";
            this.quoteValueToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.quoteValueToolStripMenuItem.Text = "Quote Value";
            this.quoteValueToolStripMenuItem.Click += new System.EventHandler(this.quoteValueToolStripMenuItem_Click);
            // 
            // encryptValueToolStripMenuItem
            // 
            this.encryptValueToolStripMenuItem.Name = "encryptValueToolStripMenuItem";
            this.encryptValueToolStripMenuItem.Size = new System.Drawing.Size(145, 22);
            this.encryptValueToolStripMenuItem.Text = "Encrypt Value";
            this.encryptValueToolStripMenuItem.Click += new System.EventHandler(this.encryptValueToolStripMenuItem_Click);
            // 
            // barStandalone
            // 
            this.barStandalone.ImageScalingSize = new System.Drawing.Size(20, 20);
            this.barStandalone.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnNew,
            this.btnOpen,
            this.btnSave});
            this.barStandalone.Location = new System.Drawing.Point(0, 0);
            this.barStandalone.Name = "barStandalone";
            this.barStandalone.Size = new System.Drawing.Size(614, 27);
            this.barStandalone.TabIndex = 3;
            this.barStandalone.Text = "toolStrip3";
            this.barStandalone.Visible = false;
            // 
            // btnNew
            // 
            this.btnNew.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnNew.Image = ((System.Drawing.Image)(resources.GetObject("btnNew.Image")));
            this.btnNew.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnNew.Name = "btnNew";
            this.btnNew.Size = new System.Drawing.Size(24, 24);
            this.btnNew.Text = "New";
            this.btnNew.Click += new System.EventHandler(this.btnNew_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnOpen.Image = ((System.Drawing.Image)(resources.GetObject("btnOpen.Image")));
            this.btnOpen.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(24, 24);
            this.btnOpen.Text = "Open";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // btnSave
            // 
            this.btnSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.btnSave.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem});
            this.btnSave.Image = ((System.Drawing.Image)(resources.GetObject("btnSave.Image")));
            this.btnSave.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(36, 24);
            this.btnSave.Text = "Save";
            this.btnSave.ButtonClick += new System.EventHandler(this.btnSave_ButtonClick);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.btnSave_ButtonClick);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAsToolStripMenuItem.Image")));
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(114, 22);
            this.saveAsToolStripMenuItem.Text = "Save As";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // ColKey
            // 
            this.ColKey.HeaderText = "Key";
            this.ColKey.MinimumWidth = 100;
            this.ColKey.Name = "ColKey";
            // 
            // ColValue
            // 
            this.ColValue.HeaderText = "Value";
            this.ColValue.MinimumWidth = 200;
            this.ColValue.Name = "ColValue";
            this.ColValue.Width = 200;
            // 
            // IniEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(614, 486);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.barDialog);
            this.Controls.Add(this.barStandalone);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "IniEditor";
            this.Text = "Ini Editor";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.IniEditor_Load);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel1.PerformLayout();
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            this.splitContainer1.ResumeLayout(false);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.KeyValue)).EndInit();
            this.toolStrip2.ResumeLayout(false);
            this.toolStrip2.PerformLayout();
            this.barDialog.ResumeLayout(false);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.KeyValueMenu.ResumeLayout(false);
            this.barStandalone.ResumeLayout(false);
            this.barStandalone.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.ListView lstSections;
        private System.Windows.Forms.DataGridView KeyValue;
        private System.Windows.Forms.ColumnHeader columnHeader1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton btnAdd;
        private System.Windows.Forms.ToolStripButton btnDelete;
        private System.Windows.Forms.Panel barDialog;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.Button btnOk;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.ContextMenuStrip KeyValueMenu;
        private System.Windows.Forms.ToolStripMenuItem quoteValueToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem encryptValueToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip2;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripTextBox txtCommentChar;
        private System.Windows.Forms.ToolStrip barStandalone;
        private System.Windows.Forms.ToolStripButton btnNew;
        private System.Windows.Forms.ToolStripButton btnOpen;
        private System.Windows.Forms.ToolStripSplitButton btnSave;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColKey;
        private System.Windows.Forms.DataGridViewTextBoxColumn ColValue;
    }
}