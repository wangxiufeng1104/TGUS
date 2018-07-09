using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WeifenLuo.WinFormsUI.Docking;
namespace TGUS
{
    
    public partial class Serial_Input : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public struct SerialParameter
        {
            public string comName;
            public int comBaudrate;
            public int comDatabits;
            public System.IO.Ports.StopBits comStopbits;
            public System.IO.Ports.Parity comParity;
            public int Timeout;
        }
        public static SerialParameter serialpara;
        public static Serial_Input serial_InputSingle = null;
        Comm com = new Comm();
        public static Serial_Input GetSingle()
        {
            if(serial_InputSingle == null)
            {
                serial_InputSingle = new Serial_Input();
            }
            return serial_InputSingle;
        }
        public Serial_Input()
        {
            InitializeComponent();
        }
        private void Serial_Input_Load(object sender, EventArgs e)
        {
            com.Add_Serials();
        }
        private bool flag = false;
        private void Serial_Set_Click(object sender, EventArgs e)
        {
            Serial_Setup serialsetup = Serial_Setup.Getsingle();
            DialogResult result = serialsetup.ShowDialog();
            if(result == System.Windows.Forms.DialogResult.OK)
            {
                flag = true;
                if(serialsetup.Com_Baudrate.SelectedIndex != 0)
                    serialpara.comBaudrate = Convert.ToInt32(serialsetup.Com_Baudrate.Text);
                else
                {
                    Custom_baudrateForm custom = new Custom_baudrateForm();
                    result = custom.ShowDialog();
                    {
                        if(result != System.Windows.Forms.DialogResult.OK || serialpara.comBaudrate == 0)
                        {
                            MessageBox.Show("Please int a number in Baudrate","TGUS",MessageBoxButtons.OK);
                        }
                    }
                }
                serialpara.comDatabits = Convert.ToInt32(serialsetup.Com_Databits.Text);
                switch(serialsetup.Com_Stopbits.SelectedIndex)
                {
                    case 0:
                        serialpara.comStopbits = System.IO.Ports.StopBits.One;
                        break;
                    case 1:
                        serialpara.comStopbits = System.IO.Ports.StopBits.OnePointFive;
                        break;
                    case 2:
                        serialpara.comStopbits = System.IO.Ports.StopBits.Two;
                        break;
                }
                switch(serialsetup.Com_Parity.SelectedIndex)
                {
                    case 0:
                        serialpara.comParity = System.IO.Ports.Parity.None;
                        break;
                    case 1:
                        serialpara.comParity = System.IO.Ports.Parity.Odd;
                        break;
                    case 2:
                        serialpara.comParity = System.IO.Ports.Parity.Even;
                        break;
                    case 3:
                        serialpara.comParity = System.IO.Ports.Parity.Mark;
                        break;
                    case 4:
                        serialpara.comParity = System.IO.Ports.Parity.Space;
                        break;
                }
                serialpara.Timeout = Convert.ToInt32(serialsetup.Time_OutTextBox.Text);
            }
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            com.Add_Serials();
        }
        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if(com.IsOpen == false)
            {
                try
                {
                    com.serialPort.PortName = Serial_Names.Text;
                    if(flag == false)
                    {
                        com.serialPort.BaudRate = 115200;
                        com.serialPort.DataBits = 8;
                        com.serialPort.StopBits = System.IO.Ports.StopBits.One;
                        com.serialPort.Parity = System.IO.Ports.Parity.None;
                        com.serialPort.ReadTimeout = 500;
                    }
                    else
                    {
                        com.serialPort.BaudRate = serialpara.comBaudrate;
                        com.serialPort.DataBits = serialpara.comDatabits;
                        com.serialPort.StopBits = serialpara.comStopbits;
                        com.serialPort.Parity = serialpara.comParity;
                        com.serialPort.ReadTimeout = serialpara.Timeout;
                    }
                    com.Open();
                    Open_Serial.Text = "Close";
                    toolStripLabel2.Image = global::TGUS.Properties.Resources.Aqua_Ball_Green;
                    if(com.IsOpen)
                    {
                        com.DataReceived += new Comm.EventHandle(Receive_Data);
                    }
                }
                catch(Exception ex)
                {
                    MessageBox.Show("Error:" + ex.Message, "Error");
                    return;
                }
            }
            else
            {
                com.Close();
                Open_Serial.Text = "Open";
                toolStripLabel2.Image = global::TGUS.Properties.Resources.Aqua_Ball_Red;
            }
        }
        private void Send_Data_Click(object sender, EventArgs e)
        {
            if (!com.IsOpen) return;
            Button but = sender as Button;
            string Send_Data = null;
            bool hexsend = false;
            switch(Convert.ToInt32( but.Tag))
            {
                case 1:
                    Send_Data = Data_TextBox1.Text;
                    if (Hex1.Checked) hexsend = true;
                    break;
                case 2:
                    Send_Data = Data_TextBox2.Text;
                    if (Hex2.Checked) hexsend = true;
                    break;
                case 3:
                    Send_Data = Data_TextBox3.Text;
                    if (Hex3.Checked) hexsend = true;
                    break;
                case 4:
                    Send_Data = Data_TextBox4.Text;
                    if (Hex4.Checked) hexsend = true;
                    break;
                case 5:
                    Send_Data = Data_TextBox5.Text;
                    if (Hex5.Checked) hexsend = true;
                    break;
            }
            
            if(Send_Data != null && Send_Data != "")
            {
                string Header = string.Format("[{0}]Send:-> ", DateTime.Now.ToString("hh:mm:ss.fff"));
                Receive_DataTextBox.AppendText(Header + Send_Data + "\r\n");

                if(hexsend == true)
                {
                    string[] str = Send_Data.Trim().Split(' ');
                    List<byte> bytes = new List<byte>();
                    foreach(string st in str)
                    {
                        for(int n = 0; n < st.Length; n += 2)
                        {
                            byte a = byte.Parse(st.Substring(n, 2), System.Globalization.NumberStyles.HexNumber);
                            bytes.Add(a);
                        }
                    }
                    byte[] bytearr = bytes.ToArray();
                    com.WritePort(bytearr, 0, bytearr.Length);

                    
                }
                else
                {
                    com.serialPort.WriteLine(Send_Data);
                }
            }
        }
        private void numberTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar == (char)8) || e.KeyChar == (char)32)
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        private void Hex_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox check = sender as CheckBox;
            switch(Convert.ToInt32(check.Tag))
            {
                case 1:
                    Hex_Ascii(check,Data_TextBox1);
                    break;
                case 2:
                    Hex_Ascii(check, Data_TextBox2);
                    break;
                case 3:
                    Hex_Ascii(check, Data_TextBox3);
                    break;
                case 4:
                    Hex_Ascii(check, Data_TextBox4);
                    break;
                case 5:
                    Hex_Ascii(check, Data_TextBox5);
                    break;
            }
        }
        private static string[] TextBoxText = new string[5];
        private void Hex_Ascii(CheckBox check,TextBox textbox)
        {
            if (check.Checked == true)
            {
                if (textbox.Text != null)
                {
                    TextBoxText[Convert.ToInt32(check.Tag) - 1] = textbox.Text;
                    string s = textbox.Text;
                    textbox.Clear();
                    byte[] by = System.Text.Encoding.ASCII.GetBytes(s);
                    foreach (byte b in by)
                    {
                        textbox.AppendText(b.ToString("X2"));
                        textbox.AppendText(" ");
                    }
                }
                textbox.KeyPress += numberTextBox_KeyPress;
            }
            else
            {
                //if (textbox.Text != null)
                //{
                //    string[] str = textbox.Text.Trim().Split(' ');
                    
                //    textbox.Clear();
                //    foreach (string ascii in str)
                //    {
                //        if (ascii.Length <= 2)
                //        {
                //            if(ascii != "")
                //            {
                //                int value = Convert.ToInt32(ascii, 16);
                //                string stringValue = char.ConvertFromUtf32(value);
                //                textbox.AppendText(stringValue);
                //            }
                            
                //        }
                //        else
                //        {
                //            string ascii1 = ascii;
                //            if(ascii.Length%2 == 1)  //奇数长度
                //            {
                //                ascii1 = ascii1 + "0";
                //            }
                //            for(int i = 0;i < ascii1.Length;i+=2)
                //            {
                //                int value = Convert.ToInt32(ascii1.Substring(i, 2), 16);
                //                string stringValue = char.ConvertFromUtf32(value);
                //                textbox.AppendText(stringValue);
                //            }
                //        }   
                //    }
                //}
                if (TextBoxText[Convert.ToInt32(check.Tag) - 1] != null)
                {
                    textbox.Text = TextBoxText[Convert.ToInt32(check.Tag) - 1];
                }
                textbox.KeyPress -= numberTextBox_KeyPress;
            }
        }
        public delegate void MyInvoke(byte[] ReceiveData);
        public void Receive_Data(byte[] ReceiveData)
        {

            MyInvoke mi = new MyInvoke(Updata);
            this.BeginInvoke(mi, new Object[] { ReceiveData });
            
        }
        public void Updata(byte[] ReceiveData)
        {
            string Header = string.Format("[{0}]Receive:<- ", DateTime.Now.ToString("hh:mm:ss.fff"));
            Receive_DataTextBox.AppendText(Header);
            if(Show_HexCheckBox.Checked == false)
            {
                Receive_DataTextBox.AppendText(System.Text.Encoding.ASCII.GetString(ReceiveData));
            }
            else
            {
                foreach(byte b in ReceiveData)
                {
                    Receive_DataTextBox.AppendText(b.ToString("X2"));
                    Receive_DataTextBox.AppendText(" ");
                }
            }
            Receive_DataTextBox.AppendText("\r\n");
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Receive_DataTextBox.Clear();
        }
        private void Data_TextBox_DoubleClick(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;
            switch(Convert.ToInt32(textbox.Tag))
            {
                case 1:
                    Change_Text(But_Send1);
                    break;
                case 2:
                    Change_Text(But_Send2);
                    break;
                case 3:
                    Change_Text(But_Send3);
                    break;
                case 4:
                    Change_Text(But_Send4);
                    break;
                case 5:
                    Change_Text(But_Send5);
                    break;
            }
        }
        private void Change_Text(Button button)
        {
            Note note = new Note(button);
            note.ChangeText += note_ChangeText;
            note.ShowDialog();
        }

        void note_ChangeText(string str,Button button)
        {
            button.Text = str;
        }
    }
}
