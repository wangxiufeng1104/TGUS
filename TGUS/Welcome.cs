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
    public partial class Welcome : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public static List<string> history_path = new List<string> { };
        private static NewProject Welcome_NewPro;
        private static OpenProject Welcome_OpenPro;
        static string welcomeLanguage;
        public static int LinkNumber;
        public static Welcome WelcomeSingle = null;
        public static Welcome GetSingle(OpenProject openpro, NewProject newpro)
        {
            if (openpro!= null)
            {
                Welcome_OpenPro = openpro;
            }
            if (newpro != null)
            {
                Welcome_NewPro = newpro;
            }
            if (WelcomeSingle == null)
            {
                WelcomeSingle = new Welcome();
               
            } 
            return WelcomeSingle;
        }
        private Welcome()
        {
            InitializeComponent();
            LoadText();
            
        }

        private void Welcome_Load(object sender, EventArgs e)
        {
            try
            {
                Main_Form.prjsavepath = Application.UserAppDataRegistry.GetValue("prjsavepath") as string;
            }
            catch
            {
            }
            try
            {
                Main_Form.PicSavePath = Application.UserAppDataRegistry.GetValue("PicSavePath") as string;
            }
            catch{}
            try
            {
                for(int i = 0;i <= 3;i ++)
                {
                    Control col = this.skinGroupBox1.Controls.Find($"Link_H{i+1}",true)[0];
                    LinkLabel LinkHis = col as LinkLabel; 
                    history_path.Add(Application.UserAppDataRegistry.GetValue($"proj_path{i}") as string);

                    if (history_path[i] == "")
                        LinkHis.Visible = false;
                    else if (File.Exists(history_path[i]))
                        LinkHis.Text = history_path[i];
                    else
                    {
                        history_path[i] = "";
                        LinkHis.Text = "file does not exist";
                        Application.UserAppDataRegistry.SetValue($"proj_path{i}", "");
                    }
                }
            }
            catch{ }
        }

        private void New_Project_Click(object sender, EventArgs e)
        {
            Welcome_NewPro(sender,e);
        }

        private void Open_Project_Click(object sender, EventArgs e)
        {
            Welcome_OpenPro(sender, e);
        }
        public void LoadText()
        {
            if (welcomeLanguage != Main_Form.LanguageType)
            {
                welcomeLanguage = Main_Form.LanguageType;
                FileStream fs;
                if (Main_Form.LanguageType == "English")
                {
                    fs = File.Open(System.Windows.Forms.Application.StartupPath + "\\" + "Languages" + "\\US_EN.xml", FileMode.Open);
                }
                else
                {
                    fs = File.Open(System.Windows.Forms.Application.StartupPath + "\\" + "Languages" + "\\ZH_CN.xml", FileMode.Open);
                }
                using (StreamReader sr = new StreamReader(fs))
                {
                    string xmlContent = sr.ReadToEnd();
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    XmlNode Lan = doc.SelectSingleNode("/TGUS/Welcome");
                    this.Text = Lan.Attributes["name"].Value;
                    Control[] tempControls = new Control[this.Controls.Count];
                    this.Controls.CopyTo(tempControls, 0);
                    foreach (Control ctr in tempControls)
                    {
                        if(ctr.Controls.Count > 0)
                        {
                            Control[] tempControls1 = new Control[ctr.Controls.Count];
                            ctr.Controls.CopyTo(tempControls1, 0);
                            foreach (Control c in tempControls1)
                            {
                                if(c is LinkLabel)
                                {
                                    XmlNode node1 = Lan.SelectSingleNode(((LinkLabel)c).Name);
                                    if(node1 != null)
                                    {
                                        c.Text = node1.InnerText;
                                    }
                                }
                            }
                        }
                        XmlNode node = Lan.SelectSingleNode(((GroupBox)ctr).Name);
                        if(node != null)
                        {
                            ctr.Text = node.InnerText;
                        }   
                    }
                }
                fs.Close();
            }
        }
        private void Link_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel linklable = sender as LinkLabel;
            Main_Form.LinkText = linklable.Text;
            LinkNumber = Convert.ToInt32(linklable.Tag);
            Welcome_OpenPro(sender, e);
        }
        private string HisString;
        private void Link_MouseMove(object sender, MouseEventArgs e)
        {
            if(HisString != ((Control)sender).Text)
            {
                toolTip1.SetToolTip(((Control)sender), ((Control)sender).Text);
                HisString = ((Control)sender).Text;
            }
            
        }
    }
}
