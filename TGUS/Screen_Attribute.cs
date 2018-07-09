using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.IO;
namespace TGUS
{
    public partial class Screen_Attribute : Form
    {
        private UInt16 width;
        private UInt16 height;
        private string strwidth;
        private string strheight;
        private static int Selectindex;
        Display dis = null;
        private bool isLoad = false;
        private static string Language = string.Empty;

        public static Screen_Attribute Screen_AttributeSingle;
        public static Screen_Attribute GetSingle()
        {
            if(Screen_AttributeSingle == null)
            {
                Screen_AttributeSingle = new Screen_Attribute();
            }
            Screen_AttributeSingle.LoadText();
            return Screen_AttributeSingle;
        }
        private Screen_Attribute()
        {
            InitializeComponent();
            
        }

        private void Screen_Attribute_Load(object sender, EventArgs e)
        {
            CBox_ResolutionRatio.Items.Add("480*272");
            CBox_ResolutionRatio.Items.Add("480*320");
            CBox_ResolutionRatio.Items.Add("640*480");
            CBox_ResolutionRatio.Items.Add("320*240");
            CBox_ResolutionRatio.Items.Add("800*480");
            CBox_ResolutionRatio.Items.Add("1024*600");
            string str = Main_Form.WIDTH.ToString() + '*' + Main_Form.HEIGHT.ToString();
            if(CBox_ResolutionRatio.Items.Contains(str))
            {
                CBox_ResolutionRatio.Text = str;
            }
            //CBox_ResolutionRatio.SelectedIndex = Selectindex;     ///设定选择第一项
            CBox_Pixel.SelectedIndex = 0;
            dis = Display.GetSingle();
            isLoad = true;
           
        }

        private void CBox_ResolutionRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(isLoad)
            {
                string[] str = CBox_ResolutionRatio.Text.Split('*');
                strwidth = str[0];
                strheight = str[1];
                width = Convert.ToUInt16(strwidth);
                height = Convert.ToUInt16(strheight);
                Main_Form.HEIGHT = height;
                Main_Form.WIDTH = width;
                dis.designer1.Width = width;
                dis.designer1.Height = height;
                Selectindex = CBox_ResolutionRatio.SelectedIndex;
            }
          
        }
        private void Screen_Attribute_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
        }
        private void LoadText()
        {
            if(Language != Main_Form.LanguageType)
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
                    XmlNode Lan = doc.SelectSingleNode("/TGUS/Screen_Attribute");
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
                            if(Lan.SelectSingleNode(controltype[i].Name) != null)
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
    }
}
