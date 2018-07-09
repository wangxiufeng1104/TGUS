using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGUS
{
   
    public partial class Display : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        //private Main_Form fmain_form = null;
        public DrawRectangle DrawRec = new DrawRectangle();
        public DrawSelect DrawSec = new DrawSelect();
        public static Display DisplaySingle = null;
        private Display()
        { 
            InitializeComponent();
            designer1.IsDrawSelectRectangle = false;
            designer1.ActiveControl = DrawSec;
        }
        public static Display GetSingle()
        {
            if(DisplaySingle == null)
            {
                DisplaySingle = new Display();
            }
            return DisplaySingle;
        }
        private void Display_Load(object sender, EventArgs e)
        {
            this.Controls.Add(designer1);
            this.Pan_Main.Width = 800;
            this.Pan_Main.Height = 600;
            this.Pan_Main.Location = new Point(30, 30);
            this.designer1.Width = Main_Form.WIDTH;
            this.designer1.Height = Main_Form.HEIGHT;
            this.designer1.Location = new Point(30, 30);
            this.ShowInTaskbar = false;
        }
        
    }
}
