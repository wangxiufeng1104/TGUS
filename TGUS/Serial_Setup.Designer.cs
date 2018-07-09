namespace TGUS
{
    partial class Serial_Setup
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Time_OutTextBox = new TGUS.DecNumberTextBox(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.Com_Parity = new System.Windows.Forms.ComboBox();
            this.Com_Stopbits = new System.Windows.Forms.ComboBox();
            this.Com_Databits = new System.Windows.Forms.ComboBox();
            this.Com_Baudrate = new System.Windows.Forms.ComboBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.Time_OutTextBox);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.Com_Parity);
            this.groupBox1.Controls.Add(this.Com_Stopbits);
            this.groupBox1.Controls.Add(this.Com_Databits);
            this.groupBox1.Controls.Add(this.Com_Baudrate);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Location = new System.Drawing.Point(13, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(225, 182);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Settings";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(202, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(17, 12);
            this.label6.TabIndex = 13;
            this.label6.Text = "ms";
            // 
            // Time_OutTextBox
            // 
            this.Time_OutTextBox.Location = new System.Drawing.Point(98, 148);
            this.Time_OutTextBox.Name = "Time_OutTextBox";
            this.Time_OutTextBox.Size = new System.Drawing.Size(98, 21);
            this.Time_OutTextBox.TabIndex = 12;
            this.Time_OutTextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 152);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(53, 12);
            this.label1.TabIndex = 11;
            this.label1.Text = "Time Out";
            // 
            // Com_Parity
            // 
            this.Com_Parity.FormattingEnabled = true;
            this.Com_Parity.Items.AddRange(new object[] {
            "None",
            "Odd",
            "Even",
            "Mark",
            "Space"});
            this.Com_Parity.Location = new System.Drawing.Point(98, 118);
            this.Com_Parity.Name = "Com_Parity";
            this.Com_Parity.Size = new System.Drawing.Size(121, 20);
            this.Com_Parity.TabIndex = 10;
            this.Com_Parity.Tag = "4";
            // 
            // Com_Stopbits
            // 
            this.Com_Stopbits.FormattingEnabled = true;
            this.Com_Stopbits.Items.AddRange(new object[] {
            "1",
            "1.5",
            "2"});
            this.Com_Stopbits.Location = new System.Drawing.Point(98, 85);
            this.Com_Stopbits.Name = "Com_Stopbits";
            this.Com_Stopbits.Size = new System.Drawing.Size(121, 20);
            this.Com_Stopbits.TabIndex = 9;
            this.Com_Stopbits.Tag = "3";
            // 
            // Com_Databits
            // 
            this.Com_Databits.FormattingEnabled = true;
            this.Com_Databits.Items.AddRange(new object[] {
            "5",
            "6",
            "7",
            "8"});
            this.Com_Databits.Location = new System.Drawing.Point(98, 52);
            this.Com_Databits.Name = "Com_Databits";
            this.Com_Databits.Size = new System.Drawing.Size(121, 20);
            this.Com_Databits.TabIndex = 8;
            this.Com_Databits.Tag = "2";
            // 
            // Com_Baudrate
            // 
            this.Com_Baudrate.FormattingEnabled = true;
            this.Com_Baudrate.Items.AddRange(new object[] {
            "Custom",
            "110",
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "19200",
            "38400",
            "56000",
            "57600",
            "115200",
            "128000",
            "230400",
            "256000",
            "460800",
            "500000",
            "512000",
            "600000",
            "750000",
            "921600",
            "1000000",
            "1500000",
            "2000000"});
            this.Com_Baudrate.Location = new System.Drawing.Point(98, 22);
            this.Com_Baudrate.Name = "Com_Baudrate";
            this.Com_Baudrate.Size = new System.Drawing.Size(121, 20);
            this.Com_Baudrate.TabIndex = 7;
            this.Com_Baudrate.Tag = "1";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(15, 121);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 4;
            this.label5.Text = "Parity";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(15, 88);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 12);
            this.label4.TabIndex = 3;
            this.label4.Text = "Stop bits";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(15, 55);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 12);
            this.label3.TabIndex = 2;
            this.label3.Text = "Data bits";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(15, 22);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(59, 12);
            this.label2.TabIndex = 1;
            this.label2.Text = "Baud rate";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(13, 216);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 1;
            this.button1.Text = "OK";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(163, 216);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 2;
            this.button2.Text = "Cancel";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Serial_Setup
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(249, 251);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.groupBox1);
            this.DoubleBuffered = true;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Serial_Setup";
            this.ShowIcon = false;
            this.Text = "Setup";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        public System.Windows.Forms.ComboBox Com_Parity;
        public System.Windows.Forms.ComboBox Com_Stopbits;
        public System.Windows.Forms.ComboBox Com_Databits;
        public System.Windows.Forms.ComboBox Com_Baudrate;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label6;
        public DecNumberTextBox Time_OutTextBox;
    }
}