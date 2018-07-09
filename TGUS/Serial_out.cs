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
    public partial class Serial_out : WeifenLuo.WinFormsUI.Docking.DockContent
    {

        public static Serial_out serial_OutSingle = null;
        public static Serial_out GetSingle()
        {
            if (serial_OutSingle == null)
            {
                serial_OutSingle = new Serial_out();
            }
            return serial_OutSingle;
        }
        public Serial_out()
        {
            InitializeComponent();
            
        }

        private void Serial_out_Load(object sender, EventArgs e)
        {
           
        }
    }
}
