using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Xml;
namespace TGUS
{
    public partial class HardWare : Form
    {
        public struct HardWare_str
        {
            public string R1;
            public string R3;
            public string R2;
            public string R5;
            public string R6;
            public string R7;
            public string R8;
            public string R9;
            public string RA;
        }
        public static HardWare_str hardware_str;
        public static byte R2;
        public static HardWare hardwaresingle;
        private static string Language;
        int r2 = 0;
        public static HardWare GetSingle()
        {
            if(hardwaresingle == null)
            {
                hardwaresingle = new HardWare();
            }
            hardwaresingle.LoadText();
            return hardwaresingle;
        }
        private HardWare()
        {
            InitializeComponent();
            try
            {
                r2 = Convert.ToByte(hardware_str.R2, 16);
            }
            catch
            {
                r2 = 0;
            }
            Init_Data();
            Control[] tempControls = new Control[this.Controls.Count];
            this.Controls.CopyTo(tempControls, 0);
            MapControls(tempControls);
            R1_CONF_cb.SelectedIndexChanged += R1_CONF_cb_SelectedIndexChanged;
            R3_COMF_tb.TextChanged += Text_TextChanged;
            RA_CONF_tb.TextChanged += Text_TextChanged;
            R5_CONF_tb.TextChanged += Text_TextChanged;
            R9_CONF_tb.TextChanged += Text_TextChanged;
            R6_CONF_tb.TextChanged += Text_TextChanged;
            R8_CONF_tb.TextChanged += Text_TextChanged;
            R7_CONF_tb.TextChanged += Text_TextChanged;
        }
        private void Init_Data()
        {
            if(R1_CONF_cb.Text == null)
            {
                R1_CONF_cb.SelectedIndex = 7;
            }
            if(R3_COMF_tb.Text == "")
            {
                R3_COMF_tb.Text = "5A";
            }
            if(RA_CONF_tb.Text == "")
            {
                RA_CONF_tb.Text = "A5";
            }
            if(R5_CONF_tb.Text == "")
            {
                R5_CONF_tb.Text = "00";
            }
            if(R6_CONF_tb.Text == "")
            {
                R6_CONF_tb.Text = "00";
            }
            if (R7_CONF_tb.Text == "")
            {
                R7_CONF_tb.Text = "00";
            }
            if (R8_CONF_tb.Text == "")
            {
                R8_CONF_tb.Text = "01";
            }
            if (R9_CONF_tb.Text == "")
            {
                R9_CONF_tb.Text = "00";
            }
        }
        private void HardWare_Load(object sender, EventArgs e)
        {
            R1_CONF_cb.SelectedIndex = Convert.ToInt16(hardware_str.R1, 16);
            if(hardware_str.R3 != null)
                R3_COMF_tb.Text = hardware_str.R3;
            if (hardware_str.RA != null)
                RA_CONF_tb.Text = hardware_str.RA;
            if (hardware_str.R5 != null)
                R5_CONF_tb.Text = hardware_str.R5;
            if (hardware_str.R9 != null)
                R9_CONF_tb.Text = hardware_str.R9;
            if (hardware_str.R6 != null)
                R6_CONF_tb.Text = hardware_str.R6;
            if (hardware_str.R8 != null)
                R8_CONF_tb.Text = hardware_str.R8;
            if (hardware_str.R7 != null)
                R7_CONF_tb.Text = hardware_str.R7;
        }

        void Text_TextChanged(object sender, EventArgs e)
        {
            string str = ((TextBox)sender).Text;
            switch(((TextBox)sender).Name)
            {
                case "R3_COMF_tb":
                    hardware_str.R3 = str;
                    break;
                case "RA_CONF_tb":
                    hardware_str.RA = str;
                    break;
                case "R5_CONF_tb":
                    hardware_str.R5 = str;
                    break;
                case "R9_CONF_tb":
                    hardware_str.R9 = str;
                    break;
                case "R6_CONF_tb":
                    hardware_str.R6 = str;
                    break;
                case "R8_CONF_tb":
                    hardware_str.R8 = str;
                    break;
                case "R7_CONF_tb":
                    hardware_str.R7 = str;
                    break;
            }
        }

        void R1_CONF_cb_SelectedIndexChanged(object sender, EventArgs e)
        {
            hardware_str.R1 = ((ComboBox)sender).SelectedIndex.ToString("X2");
        }

        private void HardWare_FormClosed(object sender, FormClosedEventArgs e)
        {
            //this.Dispose();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            hardware_str.R1 = R1_CONF_cb.SelectedIndex.ToString();
            hardware_str.R2 = R2.ToString();
            hardware_str.R3 = R3_COMF_tb.Text;
            hardware_str.R5 = R5_CONF_tb.Text;
            hardware_str.R6 = R6_CONF_tb.Text;
            hardware_str.R7 = R7_CONF_tb.Text;
            hardware_str.R8 = R8_CONF_tb.Text;
            hardware_str.R9 = R9_CONF_tb.Text;
            hardware_str.RA = RA_CONF_tb.Text;
        }

        private void MapControls(Control[] controls)
        {
            foreach(Control control in controls)
            {
                //控件里面还有别的控件
                if(control.Controls.Count > 0)
                {
                    Control[] tempControls = new Control[control.Controls.Count];
                    control.Controls.CopyTo(tempControls, 0);
                    MapControls(tempControls);
                }
                if(control is RadioButton)
                {
                    int flag = 0;
                    int temp = Convert.ToInt32(control.Tag);
                    if (temp < 0) flag = 1;
                    temp = Math.Abs(temp);
                    if(temp <= 3)
                    {
                        if ((r2 & 0x03) == temp)
                        {
                            ((RadioButton)control).Checked = true;
                        }
                    }
                    else if ((temp & r2) == 0&&flag == 1)
                    {
                        
                        ((RadioButton)control).Checked = true;
                        
                    }
                    else if ((temp & r2) == temp && flag == 0)
                    {
                        ((RadioButton)control).Checked = true;
                    }
                }
            }
        }
        private void RadioButton_CheckedChange(object sender, EventArgs e)
        {
            RadioButton radiobutton = sender as RadioButton;
            int temp = Convert.ToInt32(radiobutton.Tag);
            if (temp < 0)
            {
                R2 = (byte)(R2&(~(byte)System.Math.Abs(temp)));
            }
            else
            {
                if(temp >= 4)
                {
                    R2 = (byte)(R2 | temp);
                }
                else
                {
                    R2 &= 0xFC;
                    R2 |= (byte)temp;
                }
            }
            hardware_str.R2 = R2.ToString("X2");
        }
        private void LoadText()
        {
            if (Language != Main_Form.LanguageType)
            {
                Language = Main_Form.LanguageType;
                FileStream fs = null;
                try  //对文件进行安全检查
                {
                    if (Main_Form.LanguageType == "English")
                    {
                        fs = File.Open(System.Windows.Forms.Application.StartupPath + "\\" + "Languages" + "\\US_EN.xml", FileMode.Open);
                    }
                    else
                    {
                        fs = File.Open(System.Windows.Forms.Application.StartupPath + "\\" + "Languages" + "\\ZH_CN.xml", FileMode.Open);
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                using (StreamReader sr = new StreamReader(fs))
                {
                    string xmlContent = sr.ReadToEnd();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    XmlNode Lan = doc.SelectSingleNode("/TGUS/HardWare_Form");
                    this.Text = Lan.Attributes["name"].Value;
                    Control[] tempControls = new Control[this.Controls.Count];
                    this.Controls.CopyTo(tempControls, 0);

                    SearchControl(tempControls);
                    /*************************保留********************/
                    //XmlDeclaration dec = doc.CreateXmlDeclaration("1.0","utf-8",null);
                    //doc.AppendChild(dec);
                    //XmlElement TGUS = doc.CreateElement("TGUS");
                    //doc.AppendChild(TGUS);
                    //XmlElement Screen = doc.CreateElement("Screen_Attribute");
                    //Screen.SetAttribute("name", "Screen_Attribute");
                    //TGUS.AppendChild(Screen);
                    //foreach(Control ctr in controltype)
                    //{
                    //    XmlElement s = doc.CreateElement(ctr.Name);
                    //    s.InnerText = ctr.Text;
                    //    Screen.AppendChild(s);
                    //}
                    //doc.Save("Screen_Attribute.xml");
                    /*************************保留********************/
                    for (int i = 0; i < controltype.Count; i++)
                    {
                        if (Lan.SelectSingleNode(controltype[i].Name) != null)
                        {
                            if (Lan.SelectSingleNode(controltype[i].Name) != null)
                            {
                                XmlNode s = Lan.SelectSingleNode(controltype[i].Name);
                                controltype[i].Text = s.InnerText;
                            }
                        }
                    }
                }
                controltype.Clear();  //最后进行清空操作
                fs.Close();
            }
        }
        List<Control> controltype = new List<Control> { };
        public void SearchControl(Control[] controls)
        {
            foreach (Control control in controls)
            {
                if (control is NumericUpDown ||
                    control is System.Windows.Forms.TextBox ||
                    control is PictureBox ||
                    control is HexNumberTextBox ||
                    control is DecNumberTextBox ||
                    control is ForbidDecimaNumericUpDownl ||
                    control is MaskedTextBox ||
                    control is HScrollBar ||
                    control is VScrollBar)
                    continue;
                if (control.Controls.Count > 0)
                {
                    Control[] tempControls = new Control[control.Controls.Count];
                    control.Controls.CopyTo(tempControls, 0);
                    SearchControl(tempControls);
                }
                controltype.Add(control);
            }
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            this.Visible = false;
        }
    }

}
