using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGUS
{
    public partial class RenamePicture : Form
    {
        Images_Form form1 = Images_Form.GetSingle();
        public string Lab_String = null;
        public RenamePicture(Images_Form form, string str)
        {
            this.form1 = form;
            InitializeComponent();
            Lab_String = str;
            TBox_Diaplay.Text = str;
            //Console.WriteLine(str);    
        }
        private void But_Rename_Click(object sender, EventArgs e)
        {
            this.form1.StrRename_bmp = TBox_Rename.Text;
            this.DialogResult = DialogResult.OK;  
        }
        /// <summary>
        /// 焦点在TextBox的时候点击回车按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TBox_KeyDown(object sender, KeyEventArgs e)
        {
            if(e.KeyCode == Keys.Enter)
            {
                this.form1.StrRename_bmp = TBox_Rename.Text;
                this.DialogResult = DialogResult.OK;
            }
            else if(e.KeyCode == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }
        }
    }
}
