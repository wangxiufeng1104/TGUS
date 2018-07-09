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
    public partial class Serial_Setup : Form
    {
        public static Serial_Setup serialsingle;
        public static Serial_Setup Getsingle()
        {
            if(serialsingle == null)
            {
                serialsingle = new Serial_Setup();
            }
            return serialsingle;
        }
        public Serial_Setup()
        {
            InitializeComponent();
            Com_Baudrate.SelectedIndex = 0;
            Com_Databits.SelectedIndex = 3;
            Com_Stopbits.SelectedIndex = 0;
            Com_Parity.SelectedIndex = 0;
            Time_OutTextBox.Text = "500";
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
