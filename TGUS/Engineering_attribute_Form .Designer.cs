namespace TGUS
{
    partial class Engineering_attribute_Form
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Engineering_attribute_Form));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBox_ResolutionRatio = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.TBox_ProName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.But_FilePath = new System.Windows.Forms.Button();
            this.TBox_SavePath = new System.Windows.Forms.TextBox();
            this.But_Confirm = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBox_ResolutionRatio);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(31, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(347, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "图形属性";
            // 
            // CBox_ResolutionRatio
            // 
            this.CBox_ResolutionRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBox_ResolutionRatio.FormattingEnabled = true;
            this.CBox_ResolutionRatio.Location = new System.Drawing.Point(141, 30);
            this.CBox_ResolutionRatio.Name = "CBox_ResolutionRatio";
            this.CBox_ResolutionRatio.Size = new System.Drawing.Size(121, 20);
            this.CBox_ResolutionRatio.TabIndex = 1;
            this.CBox_ResolutionRatio.SelectedIndexChanged += new System.EventHandler(this.CBox_SelectedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.Location = new System.Drawing.Point(33, 33);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(41, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "分辨率";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.TBox_ProName);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.But_FilePath);
            this.groupBox2.Controls.Add(this.TBox_SavePath);
            this.groupBox2.Location = new System.Drawing.Point(31, 118);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(347, 126);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "存储位置";
            // 
            // TBox_ProName
            // 
            this.TBox_ProName.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TBox_ProName.Location = new System.Drawing.Point(33, 41);
            this.TBox_ProName.Name = "TBox_ProName";
            this.TBox_ProName.Size = new System.Drawing.Size(220, 21);
            this.TBox_ProName.TabIndex = 4;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 26);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "名称：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(41, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "路径：";
            // 
            // But_FilePath
            // 
            this.But_FilePath.Location = new System.Drawing.Point(255, 79);
            this.But_FilePath.Name = "But_FilePath";
            this.But_FilePath.Size = new System.Drawing.Size(48, 23);
            this.But_FilePath.TabIndex = 1;
            this.But_FilePath.Text = "...";
            this.But_FilePath.UseVisualStyleBackColor = true;
            this.But_FilePath.Click += new System.EventHandler(this.But_FilePath_Click);
            // 
            // TBox_SavePath
            // 
            this.TBox_SavePath.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.TBox_SavePath.Location = new System.Drawing.Point(33, 80);
            this.TBox_SavePath.Name = "TBox_SavePath";
            this.TBox_SavePath.Size = new System.Drawing.Size(220, 21);
            this.TBox_SavePath.TabIndex = 0;
            // 
            // But_Confirm
            // 
            this.But_Confirm.Location = new System.Drawing.Point(303, 250);
            this.But_Confirm.Name = "But_Confirm";
            this.But_Confirm.Size = new System.Drawing.Size(75, 23);
            this.But_Confirm.TabIndex = 2;
            this.But_Confirm.Text = "确认";
            this.But_Confirm.UseVisualStyleBackColor = true;
            this.But_Confirm.Click += new System.EventHandler(this.But_Confirm_Click);
            // 
            // Engineering_attribute_Form
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(409, 280);
            this.Controls.Add(this.But_Confirm);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Engineering_attribute_Form";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "工程属性";
            this.Load += new System.EventHandler(this.Form_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox CBox_ResolutionRatio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button But_FilePath;
        private System.Windows.Forms.TextBox TBox_SavePath;
        private System.Windows.Forms.Button But_Confirm;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.TextBox TBox_ProName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
    }
}