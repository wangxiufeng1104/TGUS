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
    public partial class Custom_baudrateForm : Form
    {
        public Custom_baudrateForm()
        {
            InitializeComponent();
        }

        private void Custom_baudrate_TextChanged(object sender, EventArgs e)
        {
            if(Custom_baudrate.Text != "")
            {
                Serial_Input.serialpara.comBaudrate = Convert.ToInt32(Custom_baudrate.Text);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
        
    }
}
