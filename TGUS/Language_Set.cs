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
    public partial class Language_Set : Form
    {
        public static Language_Set languagesingle;
        public static Language_Set GetSingle()
        {
            if(languagesingle == null)
            {
                languagesingle = new Language_Set();
            }
            return languagesingle;
        }
        private Language_Set()
        {
            InitializeComponent();
            AddLanguageToCombox();
            LoadText();
            cbLanguages.SelectedItem = Main_Form.LanguageType;
            cbLanguages.SelectedIndexChanged += cbLanguages_SelectedIndexChanged;
            button1.Click += button1_Click;
        }

        private void AddLanguageToCombox()
        {
            FileStream fs = File.Open(Application.StartupPath + "\\" + "Languages" + "\\Configure.xml", FileMode.Open);
            using (StreamReader sr = new StreamReader(fs))
            {
                string xmlContent = sr.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                XmlNode Inform = doc.SelectSingleNode("/root");
                XmlNodeList xlist = Inform.ChildNodes;

                foreach (XmlNode x in xlist)
                {
                    XmlNodeList l = x.ChildNodes;
                    foreach (XmlNode xnl in l)
                    {
                        if (xnl.Name == "Name")
                        {
                            cbLanguages.Items.Add(xnl.InnerText);
                        }
                    }
                }
            }
            fs.Close();
        }
        void button1_Click(object sender, EventArgs e)
        {
            Main_Form m = Main_Form.GetSingle();
            m.LoadText();
            Welcome w = Welcome.GetSingle(null,null);
            w.LoadText();
            this.DialogResult = DialogResult.OK;
        }
        void cbLanguages_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(!cbLanguages.SelectedItem.Equals(Main_Form.LanguageType))
            {
                Main_Form.LanguageType = cbLanguages.SelectedItem.ToString().Trim();
                SettingLanguage();
                LoadText();
            }
        }
        private void LoadText()
        {
            FileStream fs;
            if (Main_Form.LanguageType == "English")
            {
                fs = File.Open(Application.StartupPath + "\\" + "Languages" + "\\US_EN.xml", FileMode.Open);
            }
            else
            {
                fs = File.Open(Application.StartupPath + "\\" + "Languages" + "\\ZH_CN.xml", FileMode.Open);
            }
            using (StreamReader sr = new StreamReader(fs))
            {
                string xmlContent = sr.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                XmlNode Inform = doc.SelectSingleNode("/TGUS/Language_Form");
                this.Text = Inform.Attributes["name"].Value;//窗体名
                foreach(Control ctr in this.Controls)
                {
                    if(ctr is ComboBox) continue;
                    XmlNode s = Inform.SelectSingleNode(ctr.Name);
                    ctr.Text = s.InnerText;
                }
                
                //Touch t = Touch.GetSingle();
                //Control[] tempControls = new Control[t.Controls.Count];
                //t.Controls.CopyTo(tempControls, 0);
                //controltype.Clear();//先清空原有的控件数据
                //SearchControl(tempControls);//填充控件信息
                //Inform = doc.SelectSingleNode("/TGUS/Touch_Form");//寻找到Touch节点
                //for (int i = 0; i < controltype.Count; i++)
                //{
                //    XmlNode touchnode = Inform.SelectSingleNode(controltype[i].Name);//定位到控件信息所在的节点
                //    if (touchnode == null) continue;
                //    //判断需要的是名称还是节点属性信息
                //    if(controltype[i] is ComboBox)//需要的是节点属性信息
                //    {
                //        for(int j = 0; j < ((ComboBox)controltype[i]).Items.Count;j++)
                //        {
                //            if (touchnode.Attributes["num" + j.ToString()] != null)
                //            {
                //                ((ComboBox)controltype[i]).Items[j] = touchnode.Attributes["num" + j.ToString()].Value;
                //            }
                //        }
                //    }
                //    else if(controltype[i] is CheckedListBox)
                //    {
                //        for (int j = 0; j < ((CheckedListBox)controltype[i]).Items.Count; j++)
                //        {
                //            if (touchnode.Attributes["num" + j.ToString()] != null)
                //            {
                //                ((CheckedListBox)controltype[i]).Items[j] = touchnode.Attributes["num" + j.ToString()].Value;
                //            }
                //        }
                //    }
                //    else
                //    {
                //        if(touchnode.InnerText != null)
                //        { 
                //            controltype[i].Text = touchnode.InnerText;//将节点的信息取出赋值
                //        } 
                //    }
                //}
            }
            fs.Close();
        }
        private void SettingLanguage()
        {
            XmlDocument doc = new XmlDocument();
            doc.Load(Application.StartupPath +"\\Settings.xml");
            XmlNode node = doc.SelectSingleNode("/root/Language");
            node.InnerText = Main_Form.LanguageType;
            doc.Save(Application.StartupPath + "\\Settings.xml");
        }




        /// <summary>
        /// 遍历所有空间放入List中输出xml文件，方便多语言
        /// </summary>
        List<Control> controltype = new List<Control> { };
        public void SearchControl(Control[] controls)
        {
            foreach(Control control in controls)
            {
                if (control is NumericUpDown ||
                    control is TextBox ||
                    control is PictureBox ||
                    control is HexNumberTextBox ||
                    control is DecNumberTextBox ||
                    control is ForbidDecimaNumericUpDownl||
                    control is MaskedTextBox)
                    continue;
               
                if(control.Controls.Count > 0)
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
