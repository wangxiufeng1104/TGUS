namespace TGUS
{
    partial class Display
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
            TGUS.DrawBase drawBase1 = new TGUS.DrawBase();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Display));
            this.Pan_Main = new System.Windows.Forms.Panel();
            this.designer1 = new TGUS.Designer();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Pan_Main.SuspendLayout();
            this.SuspendLayout();
            // 
            // Pan_Main
            // 
            this.Pan_Main.AutoScroll = true;
            this.Pan_Main.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.Pan_Main.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Pan_Main.Controls.Add(this.designer1);
            this.Pan_Main.ImeMode = System.Windows.Forms.ImeMode.NoControl;
            this.Pan_Main.Location = new System.Drawing.Point(30, 30);
            this.Pan_Main.Name = "Pan_Main";
            this.Pan_Main.Size = new System.Drawing.Size(322, 212);
            this.Pan_Main.TabIndex = 3;
            this.Pan_Main.Visible = false;
            // 
            // designer1
            // 
            this.designer1.ActiveControl = drawBase1;
            this.designer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.designer1.IsDrawMousePosition = true;
            this.designer1.IsDrawSelectRectangle = false;
            this.designer1.ItemSelection = null;
            this.designer1.Location = new System.Drawing.Point(0, -1);
            this.designer1.Name = "designer1";
            this.designer1.SelectRectangle = new System.Drawing.Rectangle(0, 0, 0, 0);
            this.designer1.Size = new System.Drawing.Size(479, 271);
            this.designer1.TabIndex = 3;
            // 
            // Display
            // 
            this.AllowEndUserDocking = false;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.GradientInactiveCaption;
            this.ClientSize = new System.Drawing.Size(622, 426);
            this.ControlBox = false;
            this.Controls.Add(this.Pan_Main);
            this.DockAreas = WeifenLuo.WinFormsUI.Docking.DockAreas.Document;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.HideOnClose = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Display";
            this.ShowHint = WeifenLuo.WinFormsUI.Docking.DockState.Document;
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "触控及变量配置";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.Display_Load);
            this.Pan_Main.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Panel Pan_Main;
        private System.Windows.Forms.ToolTip toolTip1;
        public Designer designer1;

    }
}