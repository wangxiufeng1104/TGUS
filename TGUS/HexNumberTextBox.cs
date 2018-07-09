using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace TGUS
{
    public partial class HexNumberTextBox : TextBox
    {
        public HexNumberTextBox()
        {
            InitializeComponent();
            this.KeyPress += numberTextBox_KeyPress;
            this.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.MaxLength = 4;
        }

        public HexNumberTextBox(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.KeyPress += numberTextBox_KeyPress;
            this.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.MaxLength = 4;

        }
        private void numberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar == (char)8))
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
