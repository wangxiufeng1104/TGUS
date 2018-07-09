using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using System.Xml;
namespace TGUS
{

    public partial class Engineering_attribute_Form : Form
    {
        Display Dispaly_form = null;
        Main_Form mainform = Main_Form.GetSingle();
        string StrSave_Path = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);///先设置默认的路径为桌面

        public Engineering_attribute_Form(Main_Form mainform)
        {
            this.mainform = mainform;///这里的用法类似于指针  
            InitializeComponent();
            LoadText();
        }
        /// <summary>
        /// 选择保存和工作路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_FilePath_Click(object sender, EventArgs e)
        {
            folderBrowserDialog1.SelectedPath = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);///先设置默认的路径为桌面
            
            if(folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                TBox_SavePath.Text = folderBrowserDialog1.SelectedPath;                         ///显示已经选择的路径
                try
                {
                    System.IO.Directory.SetCurrentDirectory(folderBrowserDialog1.SelectedPath);     ///设置当前工作目录
                }
                catch 
                {
                    MessageBox.Show("Path Error!","waring",MessageBoxButtons.OK,MessageBoxIcon.Error);
                    return;
                }
                
            }
            StrSave_Path = folderBrowserDialog1.SelectedPath;
        }
        /// <summary>
        /// 点击确认按键返回OK，方便主窗体确认返回值。
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_Confirm_Click(object sender, EventArgs e)
        {
            if(CBox_ResolutionRatio.Text == "")
            {
                if(Main_Form.LanguageType == "English")
                {
                    MessageBox.Show("Please select the resolution", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("请选择分辨率", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            if (TBox_ProName.Text == "")
            {
                if (Main_Form.LanguageType == "English")
                {
                    MessageBox.Show("Please fill in the name of the project", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("请填写工程名称", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            if(TBox_SavePath.Text == "")
            {
                if (Main_Form.LanguageType == "English")
                {
                    MessageBox.Show("Please select the project storage path", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("请选择工程存储路径", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }  
            if(Directory.Exists(StrSave_Path + "\\TGUS" + "\\image") == false)
                Directory.CreateDirectory(StrSave_Path + "\\TGUS" + "\\image");
            if(Directory.Exists(StrSave_Path + "\\TGUS" + "\\TFT") == false)
                Directory.CreateDirectory(StrSave_Path + "\\TGUS" + "\\TFT");
            if (Directory.Exists(StrSave_Path + "\\TGUS" + "\\I") == false)
                Directory.CreateDirectory(StrSave_Path + "\\TGUS" + "\\I");
            if(Directory.Exists(StrSave_Path + "\\TGUS" + "TGUS_SET") == false)
                Directory.CreateDirectory(StrSave_Path + "\\TGUS" + "\\TGUS_SET");
            if (Directory.Exists(StrSave_Path + "\\TGUS" + "\\FONT") == false)
                Directory.CreateDirectory(StrSave_Path + "\\TGUS" + "\\FONT");
            string StrSave_Name = TBox_ProName.Text;
            if((StrSave_Name.Length > 4) &&  (StrSave_Name.Substring(StrSave_Name.Length - 4,4) == ".gxp"))
            {
                Main_Form.Project_name = StrSave_Name;
            }
            else
            {
                Main_Form.Project_name = StrSave_Name + ".gxp";
            }
            Main_Form.prjsavepath = StrSave_Path;
            Main_Form.WindowsSize = CBox_ResolutionRatio.Text;
            this.DialogResult = DialogResult.OK;
            ///mainform.SetStart();
        }
        /// <summary>
        /// Load函数在窗体初始化的之后执行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form_Load(object sender, EventArgs e)
        {
            CBox_ResolutionRatio.Items.Add("480*272");
            CBox_ResolutionRatio.Items.Add("480*320");
            CBox_ResolutionRatio.Items.Add("640*480");
            CBox_ResolutionRatio.Items.Add("320*240");
            CBox_ResolutionRatio.Items.Add("800*480");
            CBox_ResolutionRatio.Items.Add("1024*600");
            CBox_ResolutionRatio.SelectedIndex = 0;     ///设定选择第一项
            TBox_SavePath.Text = StrSave_Path;///默认选择桌面
        }
        /// <summary>
        /// 改变ComBox里的选项触发以改变Main_Form里画板参数
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CBox_SelectedChanged(object sender, EventArgs e)
        {
            this.Dispaly_form = Display.GetSingle();
            string[] str = CBox_ResolutionRatio.Text.Split('*');
            string width = str[0];
            string height = str[1];
            Main_Form.HEIGHT = Convert.ToUInt16(height);
            Main_Form.WIDTH = Convert.ToUInt16(width);
        }
        private void LoadText()
        {
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
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            using (StreamReader sr = new StreamReader(fs))
            {
                string xmlContent = sr.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                XmlNode Lan = doc.SelectSingleNode("/TGUS/New_Setting");
                Control[] tempControls = new Control[this.Controls.Count];
                this.Controls.CopyTo(tempControls, 0);
                
                SearchControl(tempControls);
                for(int i = 0;i < controltype.Count; i ++)
                {
                    if(Lan.SelectSingleNode(controltype[i].Name) != null)
                    {
                        XmlNode s = Lan.SelectSingleNode(controltype[i].Name);
                        controltype[i].Text = s.InnerText;
                    }
                }
            }
            controltype.Clear();  //完事清空
            fs.Close();
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
