namespace TGUS
{
    partial class Welcome
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
            System.Windows.Forms.LinkLabel linkLabel3;
            this.Link_H4 = new System.Windows.Forms.LinkLabel();
            this.Link_H3 = new System.Windows.Forms.LinkLabel();
            this.Link_H2 = new System.Windows.Forms.LinkLabel();
            this.Open_Project = new System.Windows.Forms.LinkLabel();
            this.Link_H1 = new System.Windows.Forms.LinkLabel();
            this.New_Project = new System.Windows.Forms.LinkLabel();
            this.skinGroupBox1 = new CCWin.SkinControl.SkinGroupBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            linkLabel3 = new System.Windows.Forms.LinkLabel();
            this.skinGroupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // linkLabel3
            // 
            linkLabel3.AutoSize = true;
            linkLabel3.Enabled = false;
            linkLabel3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            linkLabel3.ForeColor = System.Drawing.Color.White;
            linkLabel3.LinkBehavior = System.Windows.Forms.LinkBehavior.AlwaysUnderline;
            linkLabel3.LinkColor = System.Drawing.Color.Black;
            linkLabel3.LinkVisited = true;
            linkLabel3.Location = new System.Drawing.Point(27, 71);
            linkLabel3.Name = "linkLabel3";
            linkLabel3.Size = new System.Drawing.Size(32, 17);
            linkLabel3.TabIndex = 2;
            linkLabel3.TabStop = true;
            linkLabel3.Text = "历史";
            // 
            // Link_H4
            // 
            this.Link_H4.AutoSize = true;
            this.Link_H4.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Link_H4.ForeColor = System.Drawing.Color.White;
            this.Link_H4.Location = new System.Drawing.Point(27, 173);
            this.Link_H4.Name = "Link_H4";
            this.Link_H4.Size = new System.Drawing.Size(159, 17);
            this.Link_H4.TabIndex = 6;
            this.Link_H4.TabStop = true;
            this.Link_H4.Tag = "4";
            this.Link_H4.Text = "4 TGUS Project:DWprj.hmi";
            this.Link_H4.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
            this.Link_H4.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Link_MouseMove);
            // 
            // Link_H3
            // 
            this.Link_H3.AutoSize = true;
            this.Link_H3.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Link_H3.ForeColor = System.Drawing.Color.White;
            this.Link_H3.Location = new System.Drawing.Point(27, 151);
            this.Link_H3.Name = "Link_H3";
            this.Link_H3.Size = new System.Drawing.Size(159, 17);
            this.Link_H3.TabIndex = 5;
            this.Link_H3.TabStop = true;
            this.Link_H3.Tag = "3";
            this.Link_H3.Text = "3 TGUS Project:DWprj.hmi";
            this.Link_H3.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
            this.Link_H3.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Link_MouseMove);
            // 
            // Link_H2
            // 
            this.Link_H2.AutoSize = true;
            this.Link_H2.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Link_H2.ForeColor = System.Drawing.Color.White;
            this.Link_H2.Location = new System.Drawing.Point(27, 129);
            this.Link_H2.Name = "Link_H2";
            this.Link_H2.Size = new System.Drawing.Size(159, 17);
            this.Link_H2.TabIndex = 4;
            this.Link_H2.TabStop = true;
            this.Link_H2.Tag = "2";
            this.Link_H2.Text = "2 TGUS Project:DWprj.hmi";
            this.Link_H2.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
            this.Link_H2.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Link_MouseMove);
            // 
            // Open_Project
            // 
            this.Open_Project.AutoSize = true;
            this.Open_Project.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Open_Project.ForeColor = System.Drawing.Color.White;
            this.Open_Project.Location = new System.Drawing.Point(137, 34);
            this.Open_Project.Name = "Open_Project";
            this.Open_Project.Size = new System.Drawing.Size(56, 17);
            this.Open_Project.TabIndex = 1;
            this.Open_Project.TabStop = true;
            this.Open_Project.Text = "打开工程";
            this.Open_Project.Click += new System.EventHandler(this.Open_Project_Click);
            // 
            // Link_H1
            // 
            this.Link_H1.AutoSize = true;
            this.Link_H1.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.Link_H1.ForeColor = System.Drawing.Color.White;
            this.Link_H1.Location = new System.Drawing.Point(27, 107);
            this.Link_H1.Name = "Link_H1";
            this.Link_H1.Size = new System.Drawing.Size(159, 17);
            this.Link_H1.TabIndex = 3;
            this.Link_H1.TabStop = true;
            this.Link_H1.Tag = "1";
            this.Link_H1.Text = "1 TGUS Project:DWprj.hmi";
            this.Link_H1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.Link_LinkClicked);
            this.Link_H1.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Link_MouseMove);
            // 
            // New_Project
            // 
            this.New_Project.AutoSize = true;
            this.New_Project.Font = new System.Drawing.Font("微软雅黑", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.New_Project.ForeColor = System.Drawing.Color.White;
            this.New_Project.Location = new System.Drawing.Point(27, 34);
            this.New_Project.Name = "New_Project";
            this.New_Project.Size = new System.Drawing.Size(56, 17);
            this.New_Project.TabIndex = 0;
            this.New_Project.TabStop = true;
            this.New_Project.Text = "新建工程";
            this.New_Project.Click += new System.EventHandler(this.New_Project_Click);
            // 
            // skinGroupBox1
            // 
            this.skinGroupBox1.BackColor = System.Drawing.Color.Transparent;
            this.skinGroupBox1.BorderColor = System.Drawing.Color.SkyBlue;
            this.skinGroupBox1.Controls.Add(this.Link_H4);
            this.skinGroupBox1.Controls.Add(this.Link_H3);
            this.skinGroupBox1.Controls.Add(this.New_Project);
            this.skinGroupBox1.Controls.Add(this.Link_H2);
            this.skinGroupBox1.Controls.Add(this.Link_H1);
            this.skinGroupBox1.Controls.Add(linkLabel3);
            this.skinGroupBox1.Controls.Add(this.Open_Project);
            this.skinGroupBox1.Font = new System.Drawing.Font("等线", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinGroupBox1.ForeColor = System.Drawing.Color.White;
            this.skinGroupBox1.Location = new System.Drawing.Point(22, 12);
            this.skinGroupBox1.Name = "skinGroupBox1";
            this.skinGroupBox1.RectBackColor = System.Drawing.Color.White;
            this.skinGroupBox1.RoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.Size = new System.Drawing.Size(418, 206);
            this.skinGroupBox1.TabIndex = 2;
            this.skinGroupBox1.TabStop = false;
            this.skinGroupBox1.Text = "工程管理";
            this.skinGroupBox1.TitleBorderColor = System.Drawing.SystemColors.ActiveCaption;
            this.skinGroupBox1.TitleRectBackColor = System.Drawing.Color.SkyBlue;
            this.skinGroupBox1.TitleRoundStyle = CCWin.SkinClass.RoundStyle.All;
            this.skinGroupBox1.UseCompatibleTextRendering = true;
            // 
            // Welcome
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(570, 487);
            this.ControlBox = false;
            this.Controls.Add(this.skinGroupBox1);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.Name = "Welcome";
            this.Text = "欢迎使用";
            this.Load += new System.EventHandler(this.Welcome_Load);
            this.skinGroupBox1.ResumeLayout(false);
            this.skinGroupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.LinkLabel Open_Project;
        private System.Windows.Forms.LinkLabel New_Project;
        private CCWin.SkinControl.SkinGroupBox skinGroupBox1;
        public System.Windows.Forms.LinkLabel Link_H4;
        public System.Windows.Forms.LinkLabel Link_H3;
        public System.Windows.Forms.LinkLabel Link_H2;
        public System.Windows.Forms.LinkLabel Link_H1;
        private System.Windows.Forms.ToolTip toolTip1;
    }
}