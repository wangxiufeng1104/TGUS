using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace TGUS
{
    public partial class DecNumberTextBox : TextBox
    {
        public DecNumberTextBox()
        {
            InitializeComponent();
            this.KeyPress += numberTextBox_KeyPress;
        }

        public DecNumberTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.KeyPress += numberTextBox_KeyPress;
        }
        private void numberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar)  || (e.KeyChar == (char)8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
    }
}
