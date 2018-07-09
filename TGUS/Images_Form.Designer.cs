namespace TGUS
{
    partial class Images_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Images_Form));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.But_AddPic = new System.Windows.Forms.ToolStripButton();
            this.toolStripDropDownButton1 = new System.Windows.Forms.ToolStripDropDownButton();
            this.删除选中项ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.删除全部ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.GBox_Images1 = new System.Windows.Forms.GroupBox();
            this.DGV_PictureList = new System.Windows.Forms.DataGridView();
            this.Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Coumn2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.openImagesDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.toolStrip1.SuspendLayout();
            this.GBox_Images1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.DGV_PictureList)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.But_AddPic,
            this.toolStripDropDownButton1,
            this.toolStripButton2,
            this.toolStripButton3});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(212, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // But_AddPic
            // 
            this.But_AddPic.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.But_AddPic.Image = ((System.Drawing.Image)(resources.GetObject("But_AddPic.Image")));
            this.But_AddPic.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.But_AddPic.Name = "But_AddPic";
            this.But_AddPic.Size = new System.Drawing.Size(36, 22);
            this.But_AddPic.Text = "添加";
            this.But_AddPic.Click += new System.EventHandler(this.But_AddPic_Click);
            // 
            // toolStripDropDownButton1
            // 
            this.toolStripDropDownButton1.BackColor = System.Drawing.SystemColors.Control;
            this.toolStripDropDownButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripDropDownButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.删除选中项ToolStripMenuItem,
            this.删除全部ToolStripMenuItem});
            this.toolStripDropDownButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripDropDownButton1.Image")));
            this.toolStripDropDownButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDropDownButton1.Name = "toolStripDropDownButton1";
            this.toolStripDropDownButton1.Size = new System.Drawing.Size(45, 22);
            this.toolStripDropDownButton1.Text = "删除";
            // 
            // 删除选中项ToolStripMenuItem
            // 
            this.删除选中项ToolStripMenuItem.Name = "删除选中项ToolStripMenuItem";
            this.删除选中项ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除选中项ToolStripMenuItem.Text = "删除选中";
            this.删除选中项ToolStripMenuItem.Click += new System.EventHandler(this.Del_Select);
            // 
            // 删除全部ToolStripMenuItem
            // 
            this.删除全部ToolStripMenuItem.Name = "删除全部ToolStripMenuItem";
            this.删除全部ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.删除全部ToolStripMenuItem.Text = "删除全部";
            this.删除全部ToolStripMenuItem.Click += new System.EventHandler(this.Del_ALL);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton2.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton2.Image")));
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton2.Text = "上移";
            this.toolStripButton2.Click += new System.EventHandler(this.But_UpPictre_Click);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.toolStripButton3.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton3.Image")));
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(36, 22);
            this.toolStripButton3.Text = "下移";
            this.toolStripButton3.Click += new System.EventHandler(this.But_DownPicture_Click);
            // 
            // GBox_Images1
            // 
            this.GBox_Images1.Controls.Add(this.DGV_PictureList);
            this.GBox_Images1.Location = new System.Drawing.Point(0, 28);
            this.GBox_Images1.Name = "GBox_Images1";
            this.GBox_Images1.Size = new System.Drawing.Size(211, 497);
            this.GBox_Images1.TabIndex = 1;
            this.GBox_Images1.TabStop = false;
            this.GBox_Images1.Text = "Images";
            // 
            // DGV_PictureList
            // 
            this.DGV_PictureList.AllowUserToAddRows = false;
            this.DGV_PictureList.AllowUserToDeleteRows = false;
            this.DGV_PictureList.AllowUserToResizeColumns = false;
            this.DGV_PictureList.AllowUserToResizeRows = false;
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.DGV_PictureList.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.DGV_PictureList.BackgroundColor = System.Drawing.Color.LightSteelBlue;
            this.DGV_PictureList.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.DGV_PictureList.ColumnHeadersHeight = 30;
            this.DGV_PictureList.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing;
            this.DGV_PictureList.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Column1,
            this.Coumn2});
            this.DGV_PictureList.Cursor = System.Windows.Forms.Cursors.Default;
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle4.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle4.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            dataGridViewCellStyle4.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle4.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle4.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.DGV_PictureList.DefaultCellStyle = dataGridViewCellStyle4;
            this.DGV_PictureList.GridColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.DGV_PictureList.Location = new System.Drawing.Point(3, 20);
            this.DGV_PictureList.Name = "DGV_PictureList";
            this.DGV_PictureList.ReadOnly = true;
            this.DGV_PictureList.RowHeadersVisible = false;
            this.DGV_PictureList.RowHeadersWidth = 12;
            this.DGV_PictureList.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.DGV_PictureList.RowTemplate.Height = 23;
            this.DGV_PictureList.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.DGV_PictureList.ShowCellToolTips = false;
            this.DGV_PictureList.Size = new System.Drawing.Size(202, 461);
            this.DGV_PictureList.StandardTab = true;
            this.DGV_PictureList.TabIndex = 4;
            this.DGV_PictureList.TabStop = false;
            this.DGV_PictureList.CellMouseDown += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.DGV_MouseDown_Click);
            // 
            // Column1
            // 
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.Teal;
            this.Column1.DefaultCellStyle = dataGridViewCellStyle2;
            this.Column1.HeaderText = "位置";
            this.Column1.Name = "Column1";
            this.Column1.ReadOnly = true;
            this.Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Column1.Width = 70;
            // 
            // Coumn2
            // 
            dataGridViewCellStyle3.ForeColor = System.Drawing.Color.Black;
            dataGridViewCellStyle3.SelectionBackColor = System.Drawing.SystemColors.MenuHighlight;
            this.Coumn2.DefaultCellStyle = dataGridViewCellStyle3;
            this.Coumn2.FillWeight = 60F;
            this.Coumn2.HeaderText = "文件";
            this.Coumn2.Name = "Coumn2";
            this.Coumn2.ReadOnly = true;
            this.Coumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.Coumn2.Width = 129;
            // 
            // openImagesDialog1
            // 
            this.openImagesDialog1.FileName = "openFileDialog1";
            this.openImagesDialog1.Multiselect = true;
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imageList1.ImageSize = new System.Drawing.Size(256, 256);
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // Images_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.LightSteelBlue;
            this.ClientSize = new System.Drawing.Size(212, 555);
            this.CloseButton = false;
            this.CloseButtonVisible = false;
            this.Controls.Add(this.GBox_Images1);
            this.Controls.Add(this.toolStrip1);
            this.DockAreas = ((WeifenLuo.WinFormsUI.Docking.DockAreas)(((WeifenLuo.WinFormsUI.Docking.DockAreas.Float | WeifenLuo.WinFormsUI.Docking.DockAreas.DockLeft) 
            | WeifenLuo.WinFormsUI.Docking.DockAreas.DockRight)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Images_Form";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.DockLeft;
            this.Text = "Images";
            this.Load += new System.EventHandler(this.Images_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.GBox_Images1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.DGV_PictureList)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton But_AddPic;
        private System.Windows.Forms.ToolStripDropDownButton toolStripDropDownButton1;
        private System.Windows.Forms.ToolStripMenuItem 删除选中项ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 删除全部ToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Coumn2;
        private System.Windows.Forms.OpenFileDialog openImagesDialog1;
        public System.Windows.Forms.DataGridView DGV_PictureList;
        private System.Windows.Forms.ImageList imageList1;
        public System.Windows.Forms.GroupBox GBox_Images1;
    }
}