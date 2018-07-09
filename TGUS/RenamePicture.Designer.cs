namespace TGUS
{
    partial class RenamePicture
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RenamePicture));
            this.TBox_Rename = new System.Windows.Forms.TextBox();
            this.But_Rename = new System.Windows.Forms.Button();
            this.TBox_Diaplay = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TBox_Rename
            // 
            this.TBox_Rename.Location = new System.Drawing.Point(12, 55);
            this.TBox_Rename.Name = "TBox_Rename";
            this.TBox_Rename.Size = new System.Drawing.Size(100, 21);
            this.TBox_Rename.TabIndex = 1;
            this.TBox_Rename.KeyDown += new System.Windows.Forms.KeyEventHandler(this.TBox_KeyDown);
            // 
            // But_Rename
            // 
            this.But_Rename.Location = new System.Drawing.Point(152, 55);
            this.But_Rename.Name = "But_Rename";
            this.But_Rename.Size = new System.Drawing.Size(75, 23);
            this.But_Rename.TabIndex = 2;
            this.But_Rename.Text = "确认";
            this.But_Rename.UseVisualStyleBackColor = true;
            this.But_Rename.Click += new System.EventHandler(this.But_Rename_Click);
            // 
            // TBox_Diaplay
            // 
            this.TBox_Diaplay.AcceptsReturn = true;
            this.TBox_Diaplay.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.TBox_Diaplay.CausesValidation = false;
            this.TBox_Diaplay.Location = new System.Drawing.Point(13, 3);
            this.TBox_Diaplay.Multiline = true;
            this.TBox_Diaplay.Name = "TBox_Diaplay";
            this.TBox_Diaplay.ReadOnly = true;
            this.TBox_Diaplay.Size = new System.Drawing.Size(214, 46);
            this.TBox_Diaplay.TabIndex = 3;
            // 
            // RenamePicture
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(239, 103);
            this.Controls.Add(this.TBox_Diaplay);
            this.Controls.Add(this.But_Rename);
            this.Controls.Add(this.TBox_Rename);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RenamePicture";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "RenamePicture";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TBox_Rename;
        private System.Windows.Forms.Button But_Rename;
        private System.Windows.Forms.TextBox TBox_Diaplay;
    }
}