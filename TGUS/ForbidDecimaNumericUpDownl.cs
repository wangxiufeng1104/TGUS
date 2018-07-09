using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
namespace TGUS
{
    public partial class ForbidDecimaNumericUpDownl : NumericUpDown
    {
        public ForbidDecimaNumericUpDownl()
        {
            InitializeComponent();
            this.KeyPress += ForbidDecimal_KeyPress;
        }

        public ForbidDecimaNumericUpDownl(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
            this.KeyPress += ForbidDecimal_KeyPress;
        }
        private void ForbidDecimal_KeyPress(object sender,KeyPressEventArgs e)
        {
            e.Handled = (e.KeyChar == '.') ? (true) : (false);
        }
    }
}
