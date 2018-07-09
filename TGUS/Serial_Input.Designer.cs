namespace TGUS
{
    partial class Serial_Input
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Serial_Input));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.Serial_Set = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripLabel1 = new System.Windows.Forms.ToolStripLabel();
            this.Serial_Names = new System.Windows.Forms.ToolStripComboBox();
            this.Refresh_serial_port = new System.Windows.Forms.ToolStripButton();
            this.Open_Serial = new System.Windows.Forms.ToolStripButton();
            this.toolStripLabel2 = new System.Windows.Forms.ToolStripLabel();
            this.Receive_DataTextBox = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.But_Send5 = new System.Windows.Forms.Button();
            this.But_Send4 = new System.Windows.Forms.Button();
            this.But_Send3 = new System.Windows.Forms.Button();
            this.But_Send2 = new System.Windows.Forms.Button();
            this.But_Send1 = new System.Windows.Forms.Button();
            this.Hex5 = new System.Windows.Forms.CheckBox();
            this.Data_TextBox5 = new System.Windows.Forms.TextBox();
            this.Hex4 = new System.Windows.Forms.CheckBox();
            this.Data_TextBox4 = new System.Windows.Forms.TextBox();
            this.Hex3 = new System.Windows.Forms.CheckBox();
            this.Data_TextBox3 = new System.Windows.Forms.TextBox();
            this.Hex2 = new System.Windows.Forms.CheckBox();
            this.Data_TextBox2 = new System.Windows.Forms.TextBox();
            this.Hex1 = new System.Windows.Forms.CheckBox();
            this.Data_TextBox1 = new System.Windows.Forms.TextBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.Show_HexCheckBox = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.Serial_Set,
            this.toolStripSeparator1,
            this.toolStripLabel1,
            this.Serial_Names,
            this.Refresh_serial_port,
            this.Open_Serial,
            this.toolStripLabel2});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(871, 25);
            this.toolStrip1.TabIndex = 0;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // Serial_Set
            // 
            this.Serial_Set.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Serial_Set.Image = ((System.Drawing.Image)(resources.GetObject("Serial_Set.Image")));
            this.Serial_Set.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Serial_Set.Name = "Serial_Set";
            this.Serial_Set.Size = new System.Drawing.Size(66, 22);
            this.Serial_Set.Text = "Serial Set";
            this.Serial_Set.Click += new System.EventHandler(this.Serial_Set_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripLabel1
            // 
            this.toolStripLabel1.Name = "toolStripLabel1";
            this.toolStripLabel1.Size = new System.Drawing.Size(72, 22);
            this.toolStripLabel1.Text = "Serial Num";
            // 
            // Serial_Names
            // 
            this.Serial_Names.Name = "Serial_Names";
            this.Serial_Names.Size = new System.Drawing.Size(121, 25);
            // 
            // Refresh_serial_port
            // 
            this.Refresh_serial_port.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.Refresh_serial_port.Image = global::TGUS.Properties.Resources.refersh_已去底_;
            this.Refresh_serial_port.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Refresh_serial_port.Name = "Refresh_serial_port";
            this.Refresh_serial_port.Size = new System.Drawing.Size(23, 22);
            this.Refresh_serial_port.Text = "刷新";
            this.Refresh_serial_port.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // Open_Serial
            // 
            this.Open_Serial.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.Open_Serial.Image = ((System.Drawing.Image)(resources.GetObject("Open_Serial.Image")));
            this.Open_Serial.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.Open_Serial.Name = "Open_Serial";
            this.Open_Serial.Size = new System.Drawing.Size(44, 22);
            this.Open_Serial.Text = "Open";
            this.Open_Serial.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripLabel2
            // 
            this.toolStripLabel2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripLabel2.Image = global::TGUS.Properties.Resources.Aqua_Ball_Red;
            this.toolStripLabel2.Name = "toolStripLabel2";
            this.toolStripLabel2.Size = new System.Drawing.Size(16, 22);
            this.toolStripLabel2.Text = "toolStripLabel2";
            // 
            // Receive_DataTextBox
            // 
            this.Receive_DataTextBox.BackColor = System.Drawing.Color.White;
            this.Receive_DataTextBox.Location = new System.Drawing.Point(18, 20);
            this.Receive_DataTextBox.Multiline = true;
            this.Receive_DataTextBox.Name = "Receive_DataTextBox";
            this.Receive_DataTextBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.Receive_DataTextBox.Size = new System.Drawing.Size(388, 382);
            this.Receive_DataTextBox.TabIndex = 1;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.But_Send5);
            this.groupBox1.Controls.Add(this.But_Send4);
            this.groupBox1.Controls.Add(this.But_Send3);
            this.groupBox1.Controls.Add(this.But_Send2);
            this.groupBox1.Controls.Add(this.But_Send1);
            this.groupBox1.Controls.Add(this.Hex5);
            this.groupBox1.Controls.Add(this.Data_TextBox5);
            this.groupBox1.Controls.Add(this.Hex4);
            this.groupBox1.Controls.Add(this.Data_TextBox4);
            this.groupBox1.Controls.Add(this.Hex3);
            this.groupBox1.Controls.Add(this.Data_TextBox3);
            this.groupBox1.Controls.Add(this.Hex2);
            this.groupBox1.Controls.Add(this.Data_TextBox2);
            this.groupBox1.Controls.Add(this.Hex1);
            this.groupBox1.Controls.Add(this.Data_TextBox1);
            this.groupBox1.Location = new System.Drawing.Point(12, 28);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(418, 432);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Send";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(325, 43);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(83, 12);
            this.label5.TabIndex = 37;
            this.label5.Text = "Click to Send";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(84, 43);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(179, 12);
            this.label4.TabIndex = 36;
            this.label4.Text = "string[Double click to Notes]";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(152, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(0, 12);
            this.label3.TabIndex = 35;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 43);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(23, 12);
            this.label2.TabIndex = 34;
            this.label2.Text = "Hex";
            // 
            // But_Send5
            // 
            this.But_Send5.Location = new System.Drawing.Point(320, 363);
            this.But_Send5.Name = "But_Send5";
            this.But_Send5.Size = new System.Drawing.Size(92, 23);
            this.But_Send5.TabIndex = 32;
            this.But_Send5.Tag = "5";
            this.But_Send5.Text = "send";
            this.But_Send5.UseVisualStyleBackColor = true;
            this.But_Send5.Click += new System.EventHandler(this.Send_Data_Click);
            // 
            // But_Send4
            // 
            this.But_Send4.Location = new System.Drawing.Point(320, 293);
            this.But_Send4.Name = "But_Send4";
            this.But_Send4.Size = new System.Drawing.Size(92, 23);
            this.But_Send4.TabIndex = 30;
            this.But_Send4.Tag = "4";
            this.But_Send4.Text = "send";
            this.But_Send4.UseVisualStyleBackColor = true;
            this.But_Send4.Click += new System.EventHandler(this.Send_Data_Click);
            // 
            // But_Send3
            // 
            this.But_Send3.Location = new System.Drawing.Point(320, 223);
            this.But_Send3.Name = "But_Send3";
            this.But_Send3.Size = new System.Drawing.Size(92, 23);
            this.But_Send3.TabIndex = 28;
            this.But_Send3.Tag = "3";
            this.But_Send3.Text = "send";
            this.But_Send3.UseVisualStyleBackColor = true;
            this.But_Send3.Click += new System.EventHandler(this.Send_Data_Click);
            // 
            // But_Send2
            // 
            this.But_Send2.Location = new System.Drawing.Point(320, 153);
            this.But_Send2.Name = "But_Send2";
            this.But_Send2.Size = new System.Drawing.Size(92, 23);
            this.But_Send2.TabIndex = 27;
            this.But_Send2.Tag = "2";
            this.But_Send2.Text = "send";
            this.But_Send2.UseVisualStyleBackColor = true;
            this.But_Send2.Click += new System.EventHandler(this.Send_Data_Click);
            // 
            // But_Send1
            // 
            this.But_Send1.Location = new System.Drawing.Point(320, 83);
            this.But_Send1.Name = "But_Send1";
            this.But_Send1.Size = new System.Drawing.Size(92, 23);
            this.But_Send1.TabIndex = 24;
            this.But_Send1.Tag = "1";
            this.But_Send1.Text = "send";
            this.But_Send1.UseVisualStyleBackColor = true;
            this.But_Send1.Click += new System.EventHandler(this.Send_Data_Click);
            // 
            // Hex5
            // 
            this.Hex5.AutoSize = true;
            this.Hex5.Location = new System.Drawing.Point(16, 367);
            this.Hex5.Name = "Hex5";
            this.Hex5.Size = new System.Drawing.Size(15, 14);
            this.Hex5.TabIndex = 21;
            this.Hex5.Tag = "5";
            this.Hex5.UseVisualStyleBackColor = true;
            this.Hex5.CheckedChanged += new System.EventHandler(this.Hex_CheckedChanged);
            // 
            // Data_TextBox5
            // 
            this.Data_TextBox5.Location = new System.Drawing.Point(42, 346);
            this.Data_TextBox5.Multiline = true;
            this.Data_TextBox5.Name = "Data_TextBox5";
            this.Data_TextBox5.Size = new System.Drawing.Size(272, 57);
            this.Data_TextBox5.TabIndex = 20;
            this.Data_TextBox5.Tag = "5";
            this.Data_TextBox5.DoubleClick += new System.EventHandler(this.Data_TextBox_DoubleClick);
            // 
            // Hex4
            // 
            this.Hex4.AutoSize = true;
            this.Hex4.Location = new System.Drawing.Point(16, 297);
            this.Hex4.Name = "Hex4";
            this.Hex4.Size = new System.Drawing.Size(15, 14);
            this.Hex4.TabIndex = 17;
            this.Hex4.Tag = "4";
            this.Hex4.UseVisualStyleBackColor = true;
            this.Hex4.CheckedChanged += new System.EventHandler(this.Hex_CheckedChanged);
            // 
            // Data_TextBox4
            // 
            this.Data_TextBox4.Location = new System.Drawing.Point(42, 276);
            this.Data_TextBox4.Multiline = true;
            this.Data_TextBox4.Name = "Data_TextBox4";
            this.Data_TextBox4.Size = new System.Drawing.Size(272, 57);
            this.Data_TextBox4.TabIndex = 16;
            this.Data_TextBox4.Tag = "4";
            this.Data_TextBox4.DoubleClick += new System.EventHandler(this.Data_TextBox_DoubleClick);
            // 
            // Hex3
            // 
            this.Hex3.AutoSize = true;
            this.Hex3.Location = new System.Drawing.Point(16, 227);
            this.Hex3.Name = "Hex3";
            this.Hex3.Size = new System.Drawing.Size(15, 14);
            this.Hex3.TabIndex = 13;
            this.Hex3.Tag = "3";
            this.Hex3.UseVisualStyleBackColor = true;
            this.Hex3.CheckedChanged += new System.EventHandler(this.Hex_CheckedChanged);
            // 
            // Data_TextBox3
            // 
            this.Data_TextBox3.Location = new System.Drawing.Point(42, 206);
            this.Data_TextBox3.Multiline = true;
            this.Data_TextBox3.Name = "Data_TextBox3";
            this.Data_TextBox3.Size = new System.Drawing.Size(272, 57);
            this.Data_TextBox3.TabIndex = 12;
            this.Data_TextBox3.Tag = "3";
            this.Data_TextBox3.DoubleClick += new System.EventHandler(this.Data_TextBox_DoubleClick);
            // 
            // Hex2
            // 
            this.Hex2.AutoSize = true;
            this.Hex2.Location = new System.Drawing.Point(16, 157);
            this.Hex2.Name = "Hex2";
            this.Hex2.Size = new System.Drawing.Size(15, 14);
            this.Hex2.TabIndex = 9;
            this.Hex2.Tag = "2";
            this.Hex2.UseVisualStyleBackColor = true;
            this.Hex2.CheckedChanged += new System.EventHandler(this.Hex_CheckedChanged);
            // 
            // Data_TextBox2
            // 
            this.Data_TextBox2.Location = new System.Drawing.Point(42, 136);
            this.Data_TextBox2.Multiline = true;
            this.Data_TextBox2.Name = "Data_TextBox2";
            this.Data_TextBox2.Size = new System.Drawing.Size(272, 57);
            this.Data_TextBox2.TabIndex = 8;
            this.Data_TextBox2.Tag = "2";
            this.Data_TextBox2.DoubleClick += new System.EventHandler(this.Data_TextBox_DoubleClick);
            // 
            // Hex1
            // 
            this.Hex1.AutoSize = true;
            this.Hex1.Location = new System.Drawing.Point(16, 87);
            this.Hex1.Name = "Hex1";
            this.Hex1.Size = new System.Drawing.Size(15, 14);
            this.Hex1.TabIndex = 5;
            this.Hex1.Tag = "1";
            this.Hex1.UseVisualStyleBackColor = true;
            this.Hex1.CheckedChanged += new System.EventHandler(this.Hex_CheckedChanged);
            // 
            // Data_TextBox1
            // 
            this.Data_TextBox1.Location = new System.Drawing.Point(42, 66);
            this.Data_TextBox1.Multiline = true;
            this.Data_TextBox1.Name = "Data_TextBox1";
            this.Data_TextBox1.Size = new System.Drawing.Size(272, 57);
            this.Data_TextBox1.TabIndex = 4;
            this.Data_TextBox1.Tag = "1";
            this.Data_TextBox1.DoubleClick += new System.EventHandler(this.Data_TextBox_DoubleClick);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.Show_HexCheckBox);
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.Receive_DataTextBox);
            this.groupBox2.Location = new System.Drawing.Point(436, 28);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(423, 436);
            this.groupBox2.TabIndex = 3;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Receive";
            // 
            // Show_HexCheckBox
            // 
            this.Show_HexCheckBox.AutoSize = true;
            this.Show_HexCheckBox.Location = new System.Drawing.Point(129, 412);
            this.Show_HexCheckBox.Name = "Show_HexCheckBox";
            this.Show_HexCheckBox.Size = new System.Drawing.Size(72, 16);
            this.Show_HexCheckBox.TabIndex = 1;
            this.Show_HexCheckBox.Text = "Show Hex";
            this.Show_HexCheckBox.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(18, 408);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 2;
            this.button1.Text = "clean";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Serial_Input
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ButtonFace;
            this.ClientSize = new System.Drawing.Size(871, 501);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Serial_Input";
            this.Text = "Serial_assistant";
            this.Load += new System.EventHandler(this.Serial_Input_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripLabel toolStripLabel1;
        private System.Windows.Forms.ToolStripButton Serial_Set;
        private System.Windows.Forms.ToolStripButton Open_Serial;
        private System.Windows.Forms.TextBox Receive_DataTextBox;
        private System.Windows.Forms.ToolStripLabel toolStripLabel2;
        public System.Windows.Forms.ToolStripComboBox Serial_Names;
        private System.Windows.Forms.ToolStripButton Refresh_serial_port;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button But_Send5;
        private System.Windows.Forms.Button But_Send4;
        private System.Windows.Forms.Button But_Send3;
        private System.Windows.Forms.Button But_Send2;
        private System.Windows.Forms.Button But_Send1;
        private System.Windows.Forms.CheckBox Hex5;
        private System.Windows.Forms.TextBox Data_TextBox5;
        private System.Windows.Forms.CheckBox Hex4;
        private System.Windows.Forms.TextBox Data_TextBox4;
        private System.Windows.Forms.CheckBox Hex3;
        private System.Windows.Forms.TextBox Data_TextBox3;
        private System.Windows.Forms.CheckBox Hex2;
        private System.Windows.Forms.TextBox Data_TextBox2;
        private System.Windows.Forms.CheckBox Hex1;
        private System.Windows.Forms.TextBox Data_TextBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox Show_HexCheckBox;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
    }
}