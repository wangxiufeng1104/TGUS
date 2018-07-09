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
    public partial class Note : Form
    {
        public delegate void EventHandle(string str,Button button);
        public event EventHandle ChangeText;
        Button button;
        public Note(Button but)
        {
            InitializeComponent();
            button = but;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (ChangeText != null)
            {
                ChangeText(NoteText.Text,button);
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
        }
    }
}
