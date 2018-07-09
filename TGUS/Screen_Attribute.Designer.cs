namespace TGUS
{
    partial class Screen_Attribute
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Screen_Attribute));
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CBox_Pixel = new System.Windows.Forms.ComboBox();
            this.CBox_ResolutionRatio = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.CBox_Pixel);
            this.groupBox1.Controls.Add(this.CBox_ResolutionRatio);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(291, 81);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "屏幕属性";
            // 
            // CBox_Pixel
            // 
            this.CBox_Pixel.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBox_Pixel.Enabled = false;
            this.CBox_Pixel.FormattingEnabled = true;
            this.CBox_Pixel.Items.AddRange(new object[] {
            "16 - bit",
            "32 - bit"});
            this.CBox_Pixel.Location = new System.Drawing.Point(140, 43);
            this.CBox_Pixel.Name = "CBox_Pixel";
            this.CBox_Pixel.Size = new System.Drawing.Size(121, 20);
            this.CBox_Pixel.TabIndex = 3;
            // 
            // CBox_ResolutionRatio
            // 
            this.CBox_ResolutionRatio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CBox_ResolutionRatio.FormattingEnabled = true;
            this.CBox_ResolutionRatio.Location = new System.Drawing.Point(140, 17);
            this.CBox_ResolutionRatio.Name = "CBox_ResolutionRatio";
            this.CBox_ResolutionRatio.Size = new System.Drawing.Size(121, 20);
            this.CBox_ResolutionRatio.TabIndex = 2;
            this.CBox_ResolutionRatio.SelectedIndexChanged += new System.EventHandler(this.CBox_ResolutionRatio_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 47);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "图片像素";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(16, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "屏幕尺寸";
            // 
            // Screen_Attribute
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(328, 105);
            this.Controls.Add(this.groupBox1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Screen_Attribute";
            this.Text = "屏幕属性设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Screen_Attribute_FormClosed);
            this.Load += new System.EventHandler(this.Screen_Attribute_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox CBox_ResolutionRatio;
        private System.Windows.Forms.ComboBox CBox_Pixel;
    }
}