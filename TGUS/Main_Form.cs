using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;
using System.IO;
using System.Threading;
using WeifenLuo.WinFormsUI.Docking;
using System.Xml;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using Microsoft.Office.Interop.Excel;
using System.Collections;

/*
 *----------Dragon be here!----------/
 * 　　　┏┓　　　┏┓
 * 　　┏┛┻━━━┛┻┓
 * 　　┃　　　　　　　┃
 * 　　┃　　　━　　　┃
 * 　　┃　┳┛　┗┳　┃
 * 　　┃　　　　　　　┃me
 * 　　┃　　　┻　　　┃
 * 　　┃　　　　　　　┃
 * 　　┗━┓　　　┏━┛
 * 　　　　┃　　　┃神兽保佑
 * 　　　　┃　　　┃代码无BUG！
 * 　　　　┃　　　┗━━━┓
 * 　　　　┃　　　　　　　┣┓
 * 　　　　┃　　　　　　　┏┛
 * 　　　　┗┓┓┏━┳┓┏┛
 * 　　　　　┃┫┫　┃┫┫
 * 　　　　　┗┻┛　┗┻┛
 * ━━━━━━神兽出没━━━━━━
 */
namespace TGUS
{
    public delegate void NewProject(object sender, EventArgs e);
    public delegate void OpenProject(object sender, EventArgs e);
    public partial class Main_Form : Form
    {
        public const string Version = "1.7"; 
        public struct ICONINFORMATION   ///存储图标信息的结构体
        {
            public int iconfile_num;
            public string iconfile_name;
            public int iconfile_lenth;
            public int iconfile_addr;
        }
        public enum FORMVALUE
        {
            display,
            image_form,
            touch,
            welcome,
            serial_input,
            serial_out
        }
        

        public enum myGraphicsType
        {
            None,
            touch,
            variable,
            other,
            AreaSetting
        }
        public static bool setstart_flag = false;    ///开始
        public string[] StrDriver = new string[10];
        public static bool SD_reflash_flag = false;   ///USB设备更新标志
        public static string Project_name;  //存储名称                                             //
        public static string WindowsSize = "";
        public static UInt16 WIDTH = 480;    ///画板的宽度    创建变量的时候使用了static就无需再创建实例，可以直接使用
        public static UInt16 HEIGHT = 272;   ///画板的高度
        public static string prjsavepath;          ///工程存储路径
        public static string PicSavePath;          ///图片存储路径
        public static int picture_count;
        private double fTouch_Width = 320.0;
        public static myGraphicsType VarType = myGraphicsType.None;     //图元类型
        private const int max_Page = 255;
        
        Image[] ima_temp = new Image[max_Page];
        public string StrRename_bmp = "";
        
        ///public  PICINFORMATION[] picname = new PICINFORMATION[30];///存储图片信息的结构体数组
        public int DGV_ClickIndex = 0;              ///点击DataGridView的索引
        [Description("图元所在页面")]
        public static int presentpage_num;          ///当前页面
        public static int presentstr_num;      
        
        public static string mouse_name;            ///鼠标显示
        private bool drawstr_flag = false;          //开始画位图标志
        //private Point pictureloc;                   ///图片上的坐标
        public static int  Show_Percent = 100;      ///显示百分比

        private Display fDisplay;           ///指向主显示窗体
        private Images_Form fImages_Form;   ///指向加载图片窗体
        //private Welcome fWelcome;           ///指向欢迎窗体
        private Touch fTouch;               ///指向控件属性窗体
        private Serial_Input fSerial_Input; ///指向串口输入的窗体
       // private Serial_out fSerial_out;     ///指向串口输出的窗体
        //public event Display OnActiveToolChange;
         [Description("当前控件类型")]
        public static PIC_Obj SelectType = PIC_Obj.NONE;
        public static string StrMouseName = string.Empty;  //鼠标光标的标签
        private Thread td;   // 保存工程的线程
        public static string LanguageType = "English";
        private static string Main_FormLanguage = string.Empty;
        private static int DisplayscaleNumSelect = 0;

        public static Main_Form Main_FormSindle = null;
        public static Main_Form GetSingle()
        {
            if(Main_FormSindle == null)
            {
                Main_FormSindle = new Main_Form();
                
            }
            return Main_FormSindle;
        }
        public Main_Form()
        {
            Main_FormSindle = this;
            System.Windows.Forms.Control.CheckForIllegalCrossThreadCalls = true;///这个开关用来决定是否可以使用多线程来访问控件的时候
                                                                                ///是否抛出异常
            InitializeComponent();
            ChangeLanguage();
            LoadText();
            CBox_SDChoice.Items.Clear();///When you remove items from the list, 
            ///all information about the deleted items is lost
            StrDriver = GetMobileDiskList();///得到盘符,这个函数直接调用，最大识别10个USB设备
            ///Console.WriteLine("盘符:{0}",strtemp);
            Displayscale.SelectedIndex = 0;
            foreach (string str in StrDriver)
            {
                if (str != null)
                {
                    ///在ComBox控件里面显示所有的挂在的USB设备的盘符
                    CBox_SDChoice.Items.Add(str);
                    CBox_SDChoice.SelectedIndex = 0;
                }
            }
        }
        public void LoadText()
        {
            if(Main_FormLanguage != Main_Form.LanguageType)
            {
                Main_FormLanguage = Main_Form.LanguageType;
                FileStream fs;
                if (LanguageType == "English")
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
                    XmlNode Lan = doc.SelectSingleNode("/TGUS/Main_Form");
                    Control[] tempControls = new Control[this.Controls.Count];
                    this.Controls.CopyTo(tempControls, 0);
                    foreach (Control ctr in tempControls)
                    {
                        if (ctr is ToolStrip)
                        {
                            ToolStrip ctr1 = ctr as ToolStrip;
                            foreach (object obj in ctr1.Items)
                            {
                                if (obj is ToolStripButton)
                                {
                                    XmlNode s = Lan.SelectSingleNode(((ToolStripButton)obj).Name);
                                    string str = s.InnerText;
                                    str = str.Replace("&amp;", "&");
                                    ((ToolStripButton)obj).Text = str;
                                }
                            }
                        }
                    }
                    /**************************Touch_Form********************/
                    Touch t = Touch.GetSingle();
                    tempControls = new Control[t.Controls.Count];
                    t.Controls.CopyTo(tempControls, 0);
                    controltype.Clear();//先清空原有的控件数据
                    SearchControl(tempControls);//填充控件信息
                    Lan = doc.SelectSingleNode("/TGUS/Touch_Form");//寻找到Touch节点

                    for (int i = 0; i < controltype.Count; i++)
                    {
                        XmlNode touchnode = Lan.SelectSingleNode(controltype[i].Name);//定位到控件信息所在的节点
                        if (touchnode == null) continue;
                        //判断需要的是名称还是节点属性信息
                        if (controltype[i] is ComboBox)//需要的是节点属性信息
                        {
                            for (int j = 0; j < ((ComboBox)controltype[i]).Items.Count; j++)
                            {
                                if (touchnode.Attributes["num" + j.ToString()] != null)
                                {
                                    ((ComboBox)controltype[i]).Items[j] = touchnode.Attributes["num" + j.ToString()].Value;
                                }
                            }
                        }
                        else if (controltype[i] is CheckedListBox)
                        {
                            for (int j = 0; j < ((CheckedListBox)controltype[i]).Items.Count; j++)
                            {
                                if (touchnode.Attributes["num" + j.ToString()] != null)
                                {
                                    ((CheckedListBox)controltype[i]).Items[j] = touchnode.Attributes["num" + j.ToString()].Value;
                                }
                            }
                        }
                        else
                        {
                            if (touchnode.InnerText != null)
                            {
                                controltype[i].Text = touchnode.InnerText;//将节点的信息取出赋值
                            }
                        }
                    }
                    if (LanguageType == "English")
                    {
                        t.RTC_Picture.BackgroundImage = Properties.Resources.RTCEN2;
                    }
                    else
                    {
                        t.RTC_Picture.BackgroundImage = Properties.Resources.RTCZH;
                    }
                    /*****************************Images_Form*******************************/
                    Images_Form Ima = Images_Form.GetSingle();
                    tempControls = new Control[Ima.Controls.Count];
                    Ima.Controls.CopyTo(tempControls, 0);
                    controltype.Clear();//先清空原有的控件数据
                    SearchControl(tempControls);//填充控件信息
                    Lan = doc.SelectSingleNode("/TGUS/Images_Form");//寻找到Images节点
                    for (int i = 0; i < controltype.Count; i++)
                    {
                        //XmlNode imagenode = Lan.SelectSingleNode(controltype[i].Name);//定位到控件信息所在的节点
                        if (controltype[i] is ToolStrip)
                        {
                            foreach (object obj in ((ToolStrip)controltype[i]).Items)
                            {
                                if (obj is ToolStripButton)
                                {
                                    XmlNode s = Lan.SelectSingleNode(((ToolStripButton)obj).Name);
                                    ((ToolStripButton)obj).Text = s.InnerText;
                                }
                                else
                                {
                                    XmlNode s = Lan.SelectSingleNode(((ToolStripDropDownButton)obj).Name);
                                    ((ToolStripDropDownButton)obj).Text = s.InnerText;
                                    foreach (object ob in ((ToolStripDropDownButton)obj).DropDownItems)
                                    {
                                        XmlNode s1 = Lan.SelectSingleNode(((ToolStripMenuItem)ob).Name);
                                        ((ToolStripMenuItem)ob).Text = s1.InnerText;
                                    }
                                }
                            }
                        }
                        else if (controltype[i] is DataGridView)
                        {
                            XmlNode s = Lan.SelectSingleNode(controltype[i].Name);
                            ((DataGridView)controltype[i]).Columns[0].HeaderCell.Value = s.Attributes["Location"].Value;
                            ((DataGridView)controltype[i]).Columns[1].HeaderCell.Value = s.Attributes["Image"].Value;
                        }
                    }
                    /*****************************Display*******************************/
                    Display d = Display.GetSingle();
                    Lan = doc.SelectSingleNode("/TGUS/Display");//寻找到Images节点
                    d.Text = Lan.Attributes["name"].Value;
                    ///*************************保留 用作更新语言文件********************/
                    //fTouch = Touch.GetSingle();
                    //tempControls = new Control[fTouch.Controls.Count];
                    //fTouch.Controls.CopyTo(tempControls, 0);
                    //controltype.Clear();//先清空原有的控件数据
                    //SearchControl(tempControls);//填充控件信息
                    //XmlDocument xx = new XmlDocument();
                    //XmlDeclaration dec = xx.CreateXmlDeclaration("1.0", "utf-8", null);
                    //xx.AppendChild(dec);
                    //XmlElement TGUS = xx.CreateElement("TGUS");
                    //xx.AppendChild(TGUS);
                    //XmlElement Screen = xx.CreateElement("Screen_Attribute");
                    //Screen.SetAttribute("name", "Screen_Attribute");
                    //TGUS.AppendChild(Screen);
                    //for (int tt = 0; tt < controltype.Count; tt++)
                    //{
                    //    if (controltype[tt].Text != "")
                    //    {
                    //        XmlElement s = xx.CreateElement(controltype[tt].Name);
                    //        s.InnerText = controltype[tt].Text;
                    //        Screen.AppendChild(s);
                    //    }
                    //    else if (controltype[tt] is ComboBox)
                    //    {
                    //        ComboBox com = controltype[tt] as ComboBox;
                    //        XmlElement s = xx.CreateElement(controltype[tt].Name);
                    //        for (int yy = 0; yy < com.Items.Count; yy++)
                    //        {
                    //            s.SetAttribute($"num{yy}", com.Items[yy].ToString());
                    //            Screen.AppendChild(s);
                    //        }
                    //    }

                    //}
                    //xx.Save("Screen_Attribute.xml");
                    /*************************保留********************/
                }
                controltype.Clear();//完事清空
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
                    control is MaskedTextBox||
                    control is HScrollBar||
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
        private void TranslateXml(ref string str)
        {
            str = str.Replace("&amp;", "&");
            str = str.Replace("&lt;", "<");
            str = str.Replace("&gt;", ">");
            str = str.Replace("&apos;", "'");
            str = str.Replace("&quot;", "\"");
        }
        private void ChangeLanguage()
        {
            FileStream fs = File.Open(System.Windows.Forms.Application.StartupPath + "\\Settings.xml", FileMode.Open);
            using (StreamReader sr = new StreamReader(fs))
            {
                string xmlContent = sr.ReadToEnd();
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);
                XmlNode Lan = doc.SelectSingleNode("/root/Language");
                LanguageType = Lan.InnerText;
            }
            fs.Close();
        }
        public bool Drawstr_flag
        {
            get { return drawstr_flag; }
            set { drawstr_flag = value; }
        }
        /// <summary>
        /// WndProc：windows调用的回调函数
        /// </summary>
        /// <param name="sysm"></param>
        protected override void WndProc(ref Message sysm)
        {
            const int DBT_DEVICEARRIVAL = 0x8000;  
            const int DBT_DEVICEREMOVECOMPLETE = 0x8004; 
            const int WM_DEVICECHANGE = 0x219;///U盘插入的时候windows消息值
            string appname = System.Windows.Forms.Application.ExecutablePath;
            try
            {
                
                if (sysm.Msg == WM_DEVICECHANGE)
                {
                    switch(sysm.WParam.ToInt32())
                    {
                        case WM_DEVICECHANGE:
                            break;
                        case DBT_DEVICEARRIVAL:///USB插入事件
                            {
                                Thread SDcard_Evevt = new Thread(new ThreadStart(this.ThreadMethod));
                                SDcard_Evevt.Start(); 
                            }
                            break;
                        case DBT_DEVICEREMOVECOMPLETE:///USB设备拔出事件
                            {
                                Thread SDcard_Evevt = new Thread(new ThreadStart(this.ThreadMethod));
                                SDcard_Evevt.Start();
                            }
                            break;
                        default:
                            break;
                    }
                }
                base.WndProc(ref sysm);
            }
            catch
            {
            }
        }
        private string[] GetMobileDiskList()
        {
            System.Management.ManagementClass mc = new System.Management.ManagementClass("Win32_DiskDrive");
            ///使用Win32_DiskDrive获取磁盘信息
            ManagementObjectCollection moc = mc.GetInstances();
            List<string> drs = new List<string>();  ///使用集合
            foreach (ManagementObject mo in moc)
            {
                if (mo.Properties["InterfaceType"].Value.ToString() != "USB")
                    continue;
                foreach (ManagementObject mo1 in mo.GetRelated("Win32_DiskPartition"))///获取与对（联系） 相关联的对象的集合。
                {
                    foreach (ManagementBaseObject mo2 in mo1.GetRelated("Win32_LogicalDisk"))
                    {
                        drs.Add(mo2.Properties["Name"].Value.ToString());
                    }
                }
            }
            return drs.ToArray();///把集合复制到新的数组中
        }
        private void But_Click(object sender, EventArgs e)
        {
            if(setstart_flag)
            {
                string SD_Path = CBox_SDChoice.Text;
                PBar_DownLoadProgress.Value = 0;
                if(Directory.Exists(SD_Path))
                {
                    try
                    {
                        Directory.Delete(SD_Path + "\\TGUS_SET", true);
                    }
                    catch(Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                    CopyDirectory(System.Environment.CurrentDirectory + "\\TGUS" + "\\TGUS_SET", SD_Path);
                }
                else
                {
                    CopyDirectory(System.Environment.CurrentDirectory + "\\TGUS" + "\\TGUS_SET", SD_Path);
                }
            }
        }
        private void CopyDirectory(string srcdir, string desdir)
        {
            int file_num = 0; ;
            string folderName = srcdir.Substring(srcdir.LastIndexOf("\\") + 1);

            string desfolderdir = desdir + "\\" + folderName;

            if (desdir.LastIndexOf("\\") == (desdir.Length - 1))
            {
                desfolderdir = desdir + folderName;
            }
            string[] filenames = Directory.GetFileSystemEntries(srcdir);


            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                file_num++;
            }
            PBar_DownLoadProgress.Maximum = file_num;
            PBar_DownLoadProgress.Value = 0;
            foreach (string file in filenames)// 遍历所有的文件和目录
            {
                if (Directory.Exists(file))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
                {
                    string currentdir = desfolderdir + "\\" + file.Substring(file.LastIndexOf("\\") + 1);
                    if (!Directory.Exists(currentdir))
                    {
                        Directory.CreateDirectory(currentdir);
                    }
                    CopyDirectory(file, desfolderdir);
                }

                else // 否则直接copy文件
                {
                    string srcfileName = file.Substring(file.LastIndexOf("\\") + 1);
                    srcfileName = desfolderdir + "\\" + srcfileName;
                    if (!Directory.Exists(desfolderdir))
                    {
                        Directory.CreateDirectory(desfolderdir);
                    }
                    try
                    {
                        File.Copy(file, srcfileName);
                    }
                    catch
                    {
                        if(LanguageType == "English")
                        {
                            MessageBox.Show("Insufficient disk space");
                        }
                        else
                        {
                            MessageBox.Show("磁盘空间不足");
                        }
                        return;
                    }
                    PBar_DownLoadProgress.Value = PBar_DownLoadProgress.Value + 1;
                }
            }
            if (LanguageType == "English")
            {
                MessageBox.Show("   Download complete");
            }
            else
            {
                MessageBox.Show("   下载完成");
            }
            PBar_DownLoadProgress.Value = 0;
        }
        /// <summary>
        /// 新建工程
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_new_Click(object sender, EventArgs e)
        {
            if(setstart_flag)
            {
                return;
            }
            Engineering_attribute_Form Engineering_attribute_Form1 = new Engineering_attribute_Form(this);
            if (Engineering_attribute_Form1.ShowDialog() == DialogResult.OK)
            {
                SetStart();
                System.Environment.CurrentDirectory = prjsavepath;                  ///设置工作路径    *重要*   
                this.Text = prjsavepath + "\\" + Project_name;
                System.Windows.Forms.Application.UserAppDataRegistry.SetValue("prjsavepath", Main_Form.prjsavepath + "\\" + Project_name);    ///注册表中写当前工程的路径
            }
        }
        private void But_Save_Click(object sender, EventArgs e)
        {
            
            if (!setstart_flag || Images_Form.Pic_Number <= 0)   //工程没有建立或者没有添加图片的时候直接退出
                return;
            SaveHistoryPath();
            But_Save.Enabled = false;
            td = new Thread(new ThreadStart(SaveProject));
            td.Start();
        }
         /// 清除字符串数组中的重复项 
         /// /// 字符串数组 
         /// 字符串数组中单个元素的最大长度 
         ///  /// 
         public static string[] DistinctStringArray(string[] strArray, int maxElementLength) 
         {
            Hashtable h = new Hashtable();
            int length = strArray.Length;
            foreach (string s in strArray)
            {
                string k = s;
                if (maxElementLength > 0 && k.Length > maxElementLength)
                {
                    k = k.Substring(0, maxElementLength);
                }
                h[k.Trim()] = s;
            }
            string[] result = new string[length];
            h.Keys.CopyTo(result, 0);
            return result;
        } 
        private static void SaveHistoryPath()
        {
            if (Welcome.history_path[0] != (Main_Form.prjsavepath + @"\TGUS\" + Project_name))//如果当前工程是新建的
            {
                if (!Welcome.history_path.Contains(Main_Form.prjsavepath + @"\TGUS\" + Project_name))
                    Welcome.history_path.Insert(0, Main_Form.prjsavepath + @"\TGUS\" + Project_name);
            }
            for(int i = 0;i < 4;i++)
            {
                if(i < Welcome.history_path.Count)
                {
                    System.Windows.Forms.Application.UserAppDataRegistry.SetValue($"proj_path{i}", Welcome.history_path[i]);
                }
                else
                {
                    System.Windows.Forms.Application.UserAppDataRegistry.SetValue($"proj_path{i}", "");
                }
            }
            if(Welcome.history_path.Count >= 5)
            {
                Welcome.history_path.RemoveRange(4, Welcome.history_path.Count - 4);
            }
        }
        public delegate void SaveButDel(bool flag);
        private void SaveProject()
        {
            CheckFilePath();  //检查文件路径
            DeleteDir(Environment.CurrentDirectory + @"\TGUS\TFT"); //清除文件夹下的文件
            //DeleteDir(Environment.CurrentDirectory + @"\TGUS\TGUS_SET"); //清除文件夹下的文件
            //for (int i = 0; i < Images_Form.Pic_Number; i++)
            //{
            //    if (Directory.Exists(Environment.CurrentDirectory + "\\TGUS" + "\\TGUS_SET"))
            //    {
            //        try
            //        {
            //            File.Copy(Images_Form.picname[i].name, Environment.CurrentDirectory + "\\TGUS" + "\\TGUS_SET" +
            //                "\\" + (i).ToString().TrimStart() + ".bmp");
            //        }
            //        catch (Exception ex)
            //        {
            //            MessageBox.Show(ex.Message);
            //        }
            //    }
            //}
            SetProjectProperty();//填写工程属性文件
            fDisplay = Display.GetSingle();

            for (int i = 0; i < Images_Form.Pic_Number; i++)
            {
                SaveInformation(i);
            }
            MessageBox.Show("Save Successed!");
            if(td != null)
            {
                if(td.ThreadState == ThreadState.Running)
                { 
                    SaveButDel sdel = new SaveButDel(But_Save_enable);
                    this.BeginInvoke(sdel,true);
                    td.Abort();//关闭线程
                }
            }
        }
        
        void But_Save_enable(bool flag)
        {
            But_Save.Enabled = flag;
        }

        private void SaveInformation(int i)
        {
            //创建xml文件
            XmlDocument doc = new XmlDocument();
            //创建文件描述
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //创建根节点
            XmlElement root = doc.CreateElement("root");//创建根节点
            doc.AppendChild(root);
            //创建图片节点
            XmlElement bmp = doc.CreateElement("bmp");
            root.AppendChild(bmp);
            XmlElement bmpname = doc.CreateElement("BmpName");
            bmpname.InnerText = Images_Form.picname[i].safename;
            bmp.AppendChild(bmpname);
            //创建图元节点
            XmlElement graphics = doc.CreateElement("Graphics");
            root.AppendChild(graphics);

            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.presentpage_num == i)
                {

                    //创建图元节点下的图元
                    XmlElement graphic = doc.CreateElement("Graphic");
                    graphic.SetAttribute("touchvar",list.touchvar.ToString());
                    graphic.SetAttribute("ControlType", list.ControlType.ToString());
                    graphic.SetAttribute("Page", list.presentpage_num.ToString());
                    graphic.SetAttribute("Name", list.Name_define);
                    graphic.SetAttribute("Xs", list.Rectangle.X.ToString());
                    graphic.SetAttribute("Ys", list.Rectangle.Y.ToString());
                    graphic.SetAttribute("Width", list.Rectangle.Width.ToString());
                    graphic.SetAttribute("Height", list.Rectangle.Height.ToString());

                    switch (list.ControlType)
                    {
                        #region case basetouch
                        case PIC_Obj.basictouch:
                            graphic.SetAttribute("Pic_On", list.BaseTouchInfo.Pic_On.ToString());
                            graphic.SetAttribute("Pic_Next", list.BaseTouchInfo.Pic_Next.ToString());
                            graphic.SetAttribute("TP_Code", list.BaseTouchInfo.TP_Code.ToString("X4"));
                            graphic.SetAttribute("IsKey_Value", list.BaseTouchInfo.IsKey_Value.ToString());
                            break;
                        #endregion
                        #region case data_displaty
                        case PIC_Obj.data_display:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("R", list.DataVarInfo.Display_Color.R.ToString());
                            graphic.SetAttribute("G", list.DataVarInfo.Display_Color.G.ToString());
                            graphic.SetAttribute("B", list.DataVarInfo.Display_Color.B.ToString());
                            graphic.SetAttribute("Font_ID", list.DataVarInfo.Lib_ID.ToString());
                            graphic.SetAttribute("Font_Size", list.DataVarInfo.Font_Size.ToString());
                            graphic.SetAttribute("Font_Align", list.DataVarInfo.Font_Align.ToString());
                            graphic.SetAttribute("Var_Type", list.DataVarInfo.Var_Type.ToString());
                            graphic.SetAttribute("Integer_Length", list.DataVarInfo.Integer_Length.ToString());
                            graphic.SetAttribute("Decimal_Length", list.DataVarInfo.Decimal_Length.ToString());
                            graphic.SetAttribute("Len_unit", list.DataVarInfo.Len_unit.ToString());
                            graphic.SetAttribute("String_Uint", list.DataVarInfo.String_Uint);
                            graphic.SetAttribute("Initial_Value", list.DataVarInfo.Initial_Value.ToString());
                            break;
                        #endregion
                        #region case icon_display
                        case PIC_Obj.icon_display:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("Icon_FileName", list.IconVarInformation.Icon_FileName);
                            graphic.SetAttribute("V_Min", list.IconVarInformation.V_Min.ToString());
                            graphic.SetAttribute("V_Max", list.IconVarInformation.V_Max.ToString());
                            graphic.SetAttribute("Icon_Min", list.IconVarInformation.Icon_Min.ToString());
                            graphic.SetAttribute("Icon_IsMinTtasparnet", list.IconVarInformation.Icon_IsMinTransparent.ToString());
                            graphic.SetAttribute("Icon_Max", list.IconVarInformation.Icon_Max.ToString());
                            graphic.SetAttribute("Icon_IsMaxTrasparent", list.IconVarInformation.Icon_IsMaxTransparent.ToString());
                            graphic.SetAttribute("Icon_Lib", list.IconVarInformation.Icon_Lib.ToString());
                            graphic.SetAttribute("Mode", list.IconVarInformation.Mode.ToString());
                            graphic.SetAttribute("InitialValue", list.IconVarInformation.InitialValue.ToString());
                            break;
                        #endregion
                        #region case text_dispaly
                        case PIC_Obj.text_dispaly:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("R", list.TextDisplayInformation.Display_Color.R.ToString());
                            graphic.SetAttribute("G", list.TextDisplayInformation.Display_Color.G.ToString());
                            graphic.SetAttribute("B", list.TextDisplayInformation.Display_Color.B.ToString());
                            graphic.SetAttribute("Text_length", list.TextDisplayInformation.Text_length.ToString());
                            graphic.SetAttribute("Font0_ID", list.TextDisplayInformation.Font0_ID.ToString());
                            graphic.SetAttribute("Font1_ID", list.TextDisplayInformation.Font1_ID.ToString());
                            graphic.SetAttribute("Font_X_Dots", list.TextDisplayInformation.Font_X_Dots.ToString());
                            graphic.SetAttribute("Font_Y_Dots", list.TextDisplayInformation.Font_Y_Dots.ToString());
                            graphic.SetAttribute("Encode_Mode", list.TextDisplayInformation.Encode_Mode.ToString());
                            graphic.SetAttribute("HOR_Dis", list.TextDisplayInformation.HOR_Dis.ToString());
                            graphic.SetAttribute("VER_Dis", list.TextDisplayInformation.VER_Dis.ToString());
                            graphic.SetAttribute("initial_value", list.TextDisplayInformation.initial_value);
                            break;
                        #endregion
                        #region case rtc_display
                        case PIC_Obj.rtc_display:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("R", list.RTCDisplayInformatin.Display_Color.R.ToString());
                            graphic.SetAttribute("G", list.RTCDisplayInformatin.Display_Color.G.ToString());
                            graphic.SetAttribute("B", list.RTCDisplayInformatin.Display_Color.B.ToString());
                            graphic.SetAttribute("Lib_ID", list.RTCDisplayInformatin.Lib_ID.ToString());
                            graphic.SetAttribute("Font_X_Dots", list.RTCDisplayInformatin.Font_X_Dots.ToString());
                            graphic.SetAttribute("String_Code", list.RTCDisplayInformatin.String_Code);
                            break;
                        #endregion
                        #region case datainput
                        case PIC_Obj.datainput:
                            graphic.SetAttribute("Pic_ID", list.DataInputInformation.Pic_ID.ToString());
                            graphic.SetAttribute("IsDataAutoUpLoad", list.DataInputInformation.IsDataAutoUpLoad.ToString());
                            graphic.SetAttribute("Pic_Next", list.DataInputInformation.Pic_Next.ToString());
                            graphic.SetAttribute("Pic_On", list.DataInputInformation.Pic_On.ToString());
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("V_Type", list.DataInputInformation.V_Type.ToString());
                            graphic.SetAttribute("N_Int", list.DataInputInformation.N_Int.ToString());
                            graphic.SetAttribute("N_Dot", list.DataInputInformation.N_Dot.ToString());

                            graphic.SetAttribute("KeyShowPosition_X", list.DataInputInformation.KeyShowPosition_X.ToString());
                            graphic.SetAttribute("KeyShowPosition_Y", list.DataInputInformation.KeyShowPosition_Y.ToString());
                            graphic.SetAttribute("R", list.DataInputInformation.Display_Color.R.ToString());
                            graphic.SetAttribute("G", list.DataInputInformation.Display_Color.G.ToString());
                            graphic.SetAttribute("B", list.DataInputInformation.Display_Color.B.ToString());
                            graphic.SetAttribute("Lib_ID", list.DataInputInformation.Lib_ID.ToString());
                            graphic.SetAttribute("Font_Hor", list.DataInputInformation.Font_Hor.ToString());
                            graphic.SetAttribute("CurousColor", list.DataInputInformation.CurousColor.ToString());
                            graphic.SetAttribute("Hide_En", list.DataInputInformation.Hide_En.ToString());

                            graphic.SetAttribute("KB_Source", list.DataInputInformation.KB_Source.ToString());
                            graphic.SetAttribute("PIC_KB", list.DataInputInformation.PIC_KB.ToString());
                            graphic.SetAttribute("AREA_KB_Xs", list.DataInputInformation.AREA_KB_Xs.ToString());
                            graphic.SetAttribute("AREA_KB_Ys", list.DataInputInformation.AREA_KB_Ys.ToString());
                            graphic.SetAttribute("AREA_KB_Xe", list.DataInputInformation.AREA_KB_Xe.ToString());
                            graphic.SetAttribute("AREA_KB_Ye", list.DataInputInformation.AREA_KB_Ye.ToString());
                            graphic.SetAttribute("AREA_KB_Posation_X", list.DataInputInformation.AREA_KB_Posation_X.ToString());
                            graphic.SetAttribute("AREA_KB_Posation_Y", list.DataInputInformation.AREA_KB_Posation_Y.ToString());
                            graphic.SetAttribute("V_min", list.DataInputInformation.V_min.ToString());

                            graphic.SetAttribute("V_max", list.DataInputInformation.V_max.ToString());
                            graphic.SetAttribute("Limits_En", list.DataInputInformation.Limits_En.ToString("X2"));
                            graphic.SetAttribute("Return_Set", list.DataInputInformation.Return_Set.ToString());
                            graphic.SetAttribute("Return_VP", list.DataInputInformation.Return_VP.ToString("X4"));
                            graphic.SetAttribute("Return_DATA", list.DataInputInformation.Return_DATA.ToString());
                           
                            break;
                        #endregion
                        #region case key_return
                        case PIC_Obj.keyreturn:
                            graphic.SetAttribute("Pic_ID", list.KeyReturnInformation.Pic_ID.ToString());
                            graphic.SetAttribute("Pic_Next", list.KeyReturnInformation.Pic_Next.ToString());
                            graphic.SetAttribute("Pic_On", list.KeyReturnInformation.Pic_On.ToString());
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("VP_Mode", list.KeyReturnInformation.VP_Mode.ToString());
                            graphic.SetAttribute("Key_Code", list.KeyReturnInformation.Key_Code.ToString("X4"));
                            graphic.SetAttribute("Touch_Key_Code", list.KeyReturnInformation.Touch_Key_Code.ToString("X4"));
                            graphic.SetAttribute("Touch_KeyPressing_Code", list.KeyReturnInformation.Touch_KeyPressing_Code.ToString("X4"));
                            break;
                        #endregion
                        #region case QR_code
                        case PIC_Obj.QR_display:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("Unit_Pixels", list.QRCodeInformation.Unit_Pixels.ToString());
                            break;
                        #endregion
                        #region cae PopupMenu
                        case PIC_Obj.menu_display:
                            graphic.SetAttribute("Pic_ID", list.PopupMenuInformation.Pic_ID.ToString());
                            graphic.SetAttribute("IsDataAutoUpLoad", list.PopupMenuInformation.IsDataAutoUpLoad.ToString());
                            graphic.SetAttribute("Pic_On", list.PopupMenuInformation.Pic_On.ToString());
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("VP_Mode", list.PopupMenuInformation.VP_Mode.ToString());

                            graphic.SetAttribute("Pic_Menu", list.PopupMenuInformation.Pic_Menu.ToString());
                            graphic.SetAttribute("AREA_Menu_Xs", list.PopupMenuInformation.AREA_Menu_Xs.ToString());
                            graphic.SetAttribute("AREA_Menu_Ys", list.PopupMenuInformation.AREA_Menu_Ys.ToString());
                            graphic.SetAttribute("AREA_Menu_Xe", list.PopupMenuInformation.AREA_Menu_Xe.ToString());
                            graphic.SetAttribute("AREA_Menu_Ye", list.PopupMenuInformation.AREA_Menu_Ye.ToString());
                            graphic.SetAttribute("Menu_Position_X", list.PopupMenuInformation.Menu_Position_X.ToString());
                            graphic.SetAttribute("Menu_Position_Y", list.PopupMenuInformation.Menu_Position_Y.ToString());
                            break;
                        #endregion
                        #region case ActionIcon
                        case PIC_Obj.aniicon_display:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("V_Start", list.ActionIconInforamtion.V_Start.ToString());
                            graphic.SetAttribute("V_Stop", list.ActionIconInforamtion.V_Stop.ToString());
                            graphic.SetAttribute("Icon_FileName", list.ActionIconInforamtion.Icon_FileName);
                            graphic.SetAttribute("Icon_Lib", list.ActionIconInforamtion.Icon_Lib.ToString());
                            graphic.SetAttribute("Iconfileselect", list.ActionIconInforamtion.Iconfileselect.ToString());
                            graphic.SetAttribute("Icon_Stop", list.ActionIconInforamtion.Icon_Stop.ToString());
                            graphic.SetAttribute("ISIcon_Stop", list.ActionIconInforamtion.ISIcon_Stop.ToString());
                            graphic.SetAttribute("Icon_Start", list.ActionIconInforamtion.Icon_Start.ToString());
                            graphic.SetAttribute("ISIcon_Start", list.ActionIconInforamtion.ISIcon_Start.ToString());
                            
                            graphic.SetAttribute("Icon_End", list.ActionIconInforamtion.Icon_End.ToString());
                            graphic.SetAttribute("ISIcon_End", list.ActionIconInforamtion.ISIcon_End.ToString());
                            graphic.SetAttribute("Mode", list.ActionIconInforamtion.Mode.ToString());
                            graphic.SetAttribute("strMode", list.ActionIconInforamtion.strMode);
                            graphic.SetAttribute("Reset_Icon_En", list.ActionIconInforamtion.Reset_Icon_En.ToString());
                            graphic.SetAttribute("InitlizValue", list.ActionIconInforamtion.InitlizValue.ToString());
                            break;
                        #endregion
                        #region case Incremental Adjustment
                        case PIC_Obj.increadj:
                            graphic.SetAttribute("Pic_ID", list.IncreaseAdjInformation.Pic_ID.ToString());
                            graphic.SetAttribute("IsDataAutoUpLoad", list.IncreaseAdjInformation.IsDataAutoUpLoad.ToString());
                            graphic.SetAttribute("Pic_On", list.IncreaseAdjInformation.Pic_On.ToString());
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("VP_Mode", list.IncreaseAdjInformation.VP_Mode.ToString());

                            graphic.SetAttribute("Adj_Mode", list.IncreaseAdjInformation.Adj_Mode.ToString());
                            graphic.SetAttribute("Return_Mode", list.IncreaseAdjInformation.Return_Mode.ToString());
                            graphic.SetAttribute("Adj_Step", list.IncreaseAdjInformation.Adj_Step.ToString());
                            graphic.SetAttribute("V_Max", list.IncreaseAdjInformation.V_Max.ToString());
                            graphic.SetAttribute("V_Min", list.IncreaseAdjInformation.V_Min.ToString());
                            graphic.SetAttribute("Key_Mode", list.IncreaseAdjInformation.Key_Mode.ToString());
                            break;
                        #endregion
                        #region case Slider Adjustment
                        case PIC_Obj.sliadj:
                            graphic.SetAttribute("Pic_ID", list.SlideAdjInformation.Pic_ID.ToString());
                            graphic.SetAttribute("IsDataAutoUpLoad", list.SlideAdjInformation.IsDataAutoUpLoad.ToString());
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("Adj_Mode", list.SlideAdjInformation.Adj_Mode.ToString());
                            graphic.SetAttribute("V_begin", list.SlideAdjInformation.V_begin.ToString());
                            graphic.SetAttribute("V_end", list.SlideAdjInformation.V_end.ToString());
                            break;
                        #endregion
                        #region case ArtFont
                        case PIC_Obj.artfont:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("Icon_FileName", list.ArtFontInformation.Icon_FileName);
                            graphic.SetAttribute("Icon_SelectIndex", list.ArtFontInformation.Icon_SelectIndex.ToString());
                            graphic.SetAttribute("Icon_0", list.ArtFontInformation.Icon_0.ToString());
                            graphic.SetAttribute("Icon_IsTransparent", list.ArtFontInformation.Icon_IsTransparent.ToString());
                            graphic.SetAttribute("Icon_Lib", list.ArtFontInformation.Icon_Lib.ToString());
                            graphic.SetAttribute("Icon_Mode", list.ArtFontInformation.Icon_Mode.ToString());

                            graphic.SetAttribute("Integer_Length", list.ArtFontInformation.Integer_Length.ToString());
                            graphic.SetAttribute("Decimal_Length", list.ArtFontInformation.Decimal_Length.ToString());
                            graphic.SetAttribute("Var_Type", list.ArtFontInformation.Var_Type.ToString());
                            graphic.SetAttribute("Align_Mode", list.ArtFontInformation.Align_Mode.ToString());
                            graphic.SetAttribute("Init_Value", list.ArtFontInformation.Init_Value.ToString());
                            break;
                        #endregion
                        #region case slider Display
                        case PIC_Obj.slidis:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("V_begain", list.SlideDisplayInformation.V_begain.ToString());
                            graphic.SetAttribute("V_end", list.SlideDisplayInformation.V_end.ToString());
                            graphic.SetAttribute("X_begain", list.SlideDisplayInformation.X_begain.ToString());
                            graphic.SetAttribute("X_end", list.SlideDisplayInformation.X_end.ToString());

                            graphic.SetAttribute("Icon_ID", list.SlideDisplayInformation.Icon_ID.ToString());
                            graphic.SetAttribute("Icon_FileName", list.SlideDisplayInformation.Icon_FileName);
                            graphic.SetAttribute("Icon_SelectIndex", list.SlideDisplayInformation.Icon_SelectIndex.ToString());
                            graphic.SetAttribute("Icon_IsTransparent", list.SlideDisplayInformation.Icon_IsTransparent.ToString());
                            graphic.SetAttribute("Icon_Lib", list.SlideDisplayInformation.Icon_Lib.ToString());
                            graphic.SetAttribute("Icon_Mode", list.SlideDisplayInformation.Icon_Mode.ToString());
                            graphic.SetAttribute("X_adj", list.SlideDisplayInformation.X_adj.ToString());

                            graphic.SetAttribute("Mode", list.SlideDisplayInformation.Mode.ToString());
                            graphic.SetAttribute("VP_DATA_Mode", list.SlideDisplayInformation.VP_DATA_Mode.ToString());
                            graphic.SetAttribute("InitVal", list.SlideDisplayInformation.InitVal.ToString());
                            break;
                        #endregion
                        #region case Icon Rotation
                        case PIC_Obj.iconrota:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("Icon_FileName", list.IconRotationInformation.Icon_FileName);
                            graphic.SetAttribute("Lib_ID", list.IconRotationInformation.Lib_ID.ToString());
                            graphic.SetAttribute("Icon_SelectIndex", list.IconRotationInformation.Icon_SelectIndex.ToString());
                            graphic.SetAttribute("Icon_IsTransparent", list.IconRotationInformation.Icon_IsTransparent.ToString());
                            graphic.SetAttribute("Icon_ID", list.IconRotationInformation.Icon_ID.ToString());
                            graphic.SetAttribute("Icon_Xc", list.IconRotationInformation.Icon_Xc.ToString());
                            graphic.SetAttribute("Icon_Yc", list.IconRotationInformation.Icon_Yc.ToString());
                            graphic.SetAttribute("Xc", list.IconRotationInformation.Xc.ToString());
                            graphic.SetAttribute("Yc", list.IconRotationInformation.Yc.ToString());
                            graphic.SetAttribute("V_begain", list.IconRotationInformation.V_begain.ToString());
                            graphic.SetAttribute("V_end", list.IconRotationInformation.V_end.ToString());
                            graphic.SetAttribute("AL_begain", list.IconRotationInformation.AL_begain.ToString());
                            graphic.SetAttribute("AL_end", list.IconRotationInformation.AL_end.ToString());
                            graphic.SetAttribute("VP_Mode", list.IconRotationInformation.VP_Mode.ToString());
                            graphic.SetAttribute("Mode", list.IconRotationInformation.Mode.ToString());
                            graphic.SetAttribute("Init_Value", list.IconRotationInformation.Init_Value.ToString());
                            break;
                        #endregion
                        #region case Clock Display
                        case PIC_Obj.clockdisplay:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("Icon_FileName", list.ClockDisplayInformation.Icon_FileName);
                            graphic.SetAttribute("Icon_SelectIndex", list.ClockDisplayInformation.Icon_SelectIndex.ToString());
                            graphic.SetAttribute("Icon_Lib", list.ClockDisplayInformation.Icon_Lib.ToString());
                            graphic.SetAttribute("X", list.ClockDisplayInformation.X.ToString());
                            graphic.SetAttribute("Y", list.ClockDisplayInformation.Y.ToString());
                            graphic.SetAttribute("IsDiaplayHour", list.ClockDisplayInformation.IsDiaplayHour.ToString());
                            graphic.SetAttribute("IsDiaplayMinute", list.ClockDisplayInformation.IsDiaplayMinute.ToString());
                            graphic.SetAttribute("IsDiaplaySecond", list.ClockDisplayInformation.IsDiaplaySecond.ToString());

                            graphic.SetAttribute("Icon_Hour", list.ClockDisplayInformation.Icon_Hour.ToString());
                            graphic.SetAttribute("Icon_IsHourTransparent", list.ClockDisplayInformation.Icon_IsHourTransparent.ToString());
                            graphic.SetAttribute("Icon_Minute", list.ClockDisplayInformation.Icon_Minute.ToString());
                            graphic.SetAttribute("Icon_IsMinuteTransparent", list.ClockDisplayInformation.Icon_IsMinuteTransparent.ToString());
                            graphic.SetAttribute("Icon_Second", list.ClockDisplayInformation.Icon_Second.ToString());
                            graphic.SetAttribute("Icon_IsSecondTransparent", list.ClockDisplayInformation.Icon_IsSecondTransparent.ToString());
                            graphic.SetAttribute("Icon_Hour_Central_X", list.ClockDisplayInformation.Icon_Hour_Central_X.ToString());
                            graphic.SetAttribute("Icon_Hour_Central_Y", list.ClockDisplayInformation.Icon_Hour_Central_Y.ToString());
                            graphic.SetAttribute("Icon_Minute_Central_X", list.ClockDisplayInformation.Icon_Minute_Central_X.ToString());
                            graphic.SetAttribute("Icon_Minute_Central_Y", list.ClockDisplayInformation.Icon_Minute_Central_Y.ToString());
                            graphic.SetAttribute("Icon_Second_Central_X", list.ClockDisplayInformation.Icon_Second_Central_X.ToString());
                            graphic.SetAttribute("Icon_Second_Central_Y", list.ClockDisplayInformation.Icon_Second_Central_Y.ToString());
                            break;
                        #endregion
                        #region case GBK
                        case PIC_Obj.GBK:
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("IsDataAutoUpLoad", list.GBKInformation.IsDataAutoUpLoad.ToString());
                            graphic.SetAttribute("Pic_Next", list.GBKInformation.Pic_Next.ToString());
                            graphic.SetAttribute("Pic_On", list.GBKInformation.Pic_On.ToString());
                            graphic.SetAttribute("VP_Len_Max", list.GBKInformation.VP_Len_Max.ToString());
                            graphic.SetAttribute("Scan_Mode", list.GBKInformation.Scan_Mode.ToString());
                            graphic.SetAttribute("Lib_GBK1", list.GBKInformation.Lib_GBK1.ToString());
                            graphic.SetAttribute("Lib_GBK2", list.GBKInformation.Lib_GBK2.ToString());
                            graphic.SetAttribute("Font_Scale1", list.GBKInformation.Font_Scale1.ToString());

                            graphic.SetAttribute("Font_Scale2", list.GBKInformation.Font_Scale2.ToString());
                            graphic.SetAttribute("Cusor_Color", list.GBKInformation.Cusor_Color.ToString());
                            graphic.SetAttribute("ColorNum1", list.GBKInformation.ColorNum1.ToString("X4"));
                            graphic.SetAttribute("ColorNum2", list.GBKInformation.ColorNum2.ToString("X4"));
                            graphic.SetAttribute("PY_Disp_Mode", list.GBKInformation.PY_Disp_Mode.ToString());
                            graphic.SetAttribute("Scan_Return_Mode", list.GBKInformation.Scan_Return_Mode.ToString());
                            graphic.SetAttribute("Scan0_Area_Start_Xs", list.GBKInformation.Scan0_Area_Start_Xs.ToString());
                            graphic.SetAttribute("Scan0_Area_Start_Ys", list.GBKInformation.Scan0_Area_Start_Ys.ToString());
                            graphic.SetAttribute("Scan0_Area_End_Xe", list.GBKInformation.Scan0_Area_End_Xe.ToString());
                            graphic.SetAttribute("Scan0_Area_End_Ye", list.GBKInformation.Scan0_Area_End_Ye.ToString());
                            graphic.SetAttribute("Scan1_Area_Start_Xs", list.GBKInformation.Scan1_Area_Start_Xs.ToString());
                            graphic.SetAttribute("Scan1_Area_Start_Ys", list.GBKInformation.Scan1_Area_Start_Ys.ToString());


                            graphic.SetAttribute("Scan_Dis", list.GBKInformation.Scan_Dis.ToString());
                            graphic.SetAttribute("KB_Source", list.GBKInformation.KB_Source.ToString());
                            graphic.SetAttribute("PIC_KB", list.GBKInformation.PIC_KB.ToString());
                            graphic.SetAttribute("AREA_KB_Xs", list.GBKInformation.AREA_KB_Xs.ToString());
                            graphic.SetAttribute("AREA_KB_Ys", list.GBKInformation.AREA_KB_Ys.ToString());
                            graphic.SetAttribute("AREA_KB_Xe", list.GBKInformation.AREA_KB_Xe.ToString());
                            graphic.SetAttribute("AREA_KB_Ye", list.GBKInformation.AREA_KB_Ye.ToString());
                            graphic.SetAttribute("AREA_KB_Position_Xs", list.GBKInformation.AREA_KB_Position_Xs.ToString());
                            graphic.SetAttribute("AREA_KB_Position_Ys", list.GBKInformation.AREA_KB_Position_Ys.ToString());
                            graphic.SetAttribute("SCAN_MODE", list.GBKInformation.SCAN_MODE.ToString());
                            break;

                        #endregion
                        #region ASCII
                        case PIC_Obj.ASCII:
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("IsDataAutoUpLoad", list.ASCIIInformation.IsDataAutoUpLoad.ToString());
                            graphic.SetAttribute("Pic_Next", list.ASCIIInformation.Pic_Next.ToString());
                            graphic.SetAttribute("Pic_On", list.ASCIIInformation.Pic_On.ToString());
                            graphic.SetAttribute("VP_Len_Max", list.ASCIIInformation.VP_Len_Max.ToString());
                            graphic.SetAttribute("Scan_Mode", list.ASCIIInformation.Scan_Mode.ToString());
                            graphic.SetAttribute("Lib_ID", list.ASCIIInformation.Lib_ID.ToString());
                            graphic.SetAttribute("Font_Hor", list.ASCIIInformation.Font_Hor.ToString());

                            graphic.SetAttribute("Font_Ver", list.ASCIIInformation.Font_Ver.ToString());
                            graphic.SetAttribute("Cusor_Color", list.ASCIIInformation.Cusor_Color.ToString());
                            graphic.SetAttribute("ColorNum", list.ASCIIInformation.ColorNum.ToString("X4"));
                            
                            graphic.SetAttribute("Scan_Return_Mode", list.ASCIIInformation.Scan_Return_Mode.ToString());
                            graphic.SetAttribute("Scan_Area_Start_Xs", list.ASCIIInformation.Scan_Area_Start_Xs.ToString());
                            graphic.SetAttribute("Scan_Area_Start_Ys", list.ASCIIInformation.Scan_Area_Start_Ys.ToString());
                            graphic.SetAttribute("Scan_Area_End_Xe", list.ASCIIInformation.Scan_Area_End_Xe.ToString());
                            graphic.SetAttribute("Scan_Area_End_Ye", list.ASCIIInformation.Scan_Area_End_Ye.ToString());
                            
                            graphic.SetAttribute("KB_Source", list.ASCIIInformation.KB_Source.ToString());
                            graphic.SetAttribute("PIC_KB", list.ASCIIInformation.PIC_KB.ToString());
                            graphic.SetAttribute("AREA_KB_Xs", list.ASCIIInformation.AREA_KB_Xs.ToString());
                            graphic.SetAttribute("AREA_KB_Ys", list.ASCIIInformation.AREA_KB_Ys.ToString());
                            graphic.SetAttribute("AREA_KB_Xe", list.ASCIIInformation.AREA_KB_Xe.ToString());
                            graphic.SetAttribute("AREA_KB_Ye", list.ASCIIInformation.AREA_KB_Ye.ToString());
                            graphic.SetAttribute("AREA_KB_Position_Xs", list.ASCIIInformation.AREA_KB_Position_Xs.ToString());
                            graphic.SetAttribute("AREA_KB_Position_Ys", list.ASCIIInformation.AREA_KB_Position_Ys.ToString());
                            graphic.SetAttribute("DISPLAY_EN", list.ASCIIInformation.DISPLAY_EN.ToString());
                            break;
                        #endregion
                        case PIC_Obj.TouchState:
                            graphic.SetAttribute("Pic_Next", list.TouchStateInformation.Pic_Next.ToString());
                            graphic.SetAttribute("Pic_On", list.TouchStateInformation.Pic_On.ToString());
                            graphic.SetAttribute("TP_ON_Mode", list.TouchStateInformation.TP_ON_Mode.ToString());
                            graphic.SetAttribute("VP1S", list.TouchStateInformation.VP1S.ToString("X4"));
                            graphic.SetAttribute("VP1T", list.TouchStateInformation.VP1T.ToString("X4"));
                            graphic.SetAttribute("LEN1", list.TouchStateInformation.LEN1.ToString());

                            graphic.SetAttribute("TP_ON_Continue_Mode", list.TouchStateInformation.TP_ON_Continue_Mode.ToString());
                            graphic.SetAttribute("VP2S", list.TouchStateInformation.VP2S.ToString("X4"));
                            graphic.SetAttribute("VP2T", list.TouchStateInformation.VP2T.ToString("X4"));

                            graphic.SetAttribute("LEN2", list.TouchStateInformation.LEN2.ToString());
                            graphic.SetAttribute("TP_OFF_Mode", list.TouchStateInformation.TP_OFF_Mode.ToString());
                            graphic.SetAttribute("VP3S", list.TouchStateInformation.VP3S.ToString("X4"));
                            graphic.SetAttribute("VP3T", list.TouchStateInformation.VP3T.ToString("X4"));
                            graphic.SetAttribute("LEN3", list.TouchStateInformation.LEN3.ToString());
                            break;
                        #region case RTCset
                        case PIC_Obj.RTC_Set:
                            graphic.SetAttribute("Pic_On", list.RTCsetInformation.Pic_On.ToString());
                            graphic.SetAttribute("TP_Code", list.RTCsetInformation.TP_Code.ToString("X4"));
                            graphic.SetAttribute("DisplayPoint_X", list.RTCsetInformation.DisplayPoint_X.ToString());
                            graphic.SetAttribute("DisplayPoint_Y", list.RTCsetInformation.DisplayPoint_Y.ToString());
                            graphic.SetAttribute("ColorNum", list.RTCsetInformation.ColorNum.ToString("X4"));
                            graphic.SetAttribute("Lib_ID", list.RTCsetInformation.Lib_ID.ToString());
                            graphic.SetAttribute("Font_Hor", list.RTCsetInformation.Font_Hor.ToString());
                            graphic.SetAttribute("Cusor_Color", list.RTCsetInformation.Cusor_Color.ToString());
                            graphic.SetAttribute("KB_Source", list.RTCsetInformation.KB_Source.ToString());
                            graphic.SetAttribute("PIC_KB", list.RTCsetInformation.PIC_KB.ToString());
                            graphic.SetAttribute("AREA_KB_Xs", list.RTCsetInformation.AREA_KB_Xs.ToString());
                            graphic.SetAttribute("AREA_KB_Ys", list.RTCsetInformation.AREA_KB_Ys.ToString());
                            graphic.SetAttribute("AREA_KB_Xe", list.RTCsetInformation.AREA_KB_Xe.ToString());
                            graphic.SetAttribute("AREA_KB_Ye", list.RTCsetInformation.AREA_KB_Ye.ToString());
                            graphic.SetAttribute("AREA_KB_Position_Xs", list.RTCsetInformation.AREA_KB_Position_Xs.ToString());
                            graphic.SetAttribute("AREA_KB_Position_Ys", list.RTCsetInformation.AREA_KB_Position_Ys.ToString());
                            break;
                        #endregion
                        #region case BasicGra
                        case PIC_Obj.BasicGra:
                            graphic.SetAttribute("SP", list.SP.ToString("X4"));
                            graphic.SetAttribute("VP", list.VP.ToString("X4"));
                            graphic.SetAttribute("Dashed_Line_En", list.BasicGraInformation.Dashed_Line_En.ToString("X2"));
                            graphic.SetAttribute("Dash_Set_1", list.BasicGraInformation.Dash_Set_1.ToString());
                            graphic.SetAttribute("Dash_Set_2", list.BasicGraInformation.Dash_Set_2.ToString());
                            graphic.SetAttribute("Dash_Set_3", list.BasicGraInformation.Dash_Set_3.ToString());
                            graphic.SetAttribute("Dash_Set_4", list.BasicGraInformation.Dash_Set_4.ToString());
                            break;
                        #endregion
                    }
                    graphics.AppendChild(graphic);
                }
            }
            doc.Save(Environment.CurrentDirectory + "\\TGUS" + "\\TFT"
               + "\\" + Images_Form.picname[i].safename + ".TFT");
        }
        private void DeleteDir(string strPath)
        {
            try
            {
                strPath = @strPath.Trim().ToString();// 清除空格
                if (Directory.Exists(strPath))// 判断文件夹是否存在
                {
                    string[] strDirs = Directory.GetDirectories(strPath);// 获得文件夹数组
                    string[] strFiles = Directory.GetFiles(strPath);// 获得文件数组
                    foreach (string strFile in strFiles)// 遍历所有文件
                    {
                        System.Diagnostics.Debug.Write(strFile + "-deleted");
                        string extension = Path.GetExtension(strFile);//扩展名 “.aspx”
                        if(extension == ".bmp" || extension == ".TFT"|| extension == ".ico"||extension == ".ICO")
                        {
                            File.Delete(strFile);// 删除文件
                        }  
                    }
                    foreach (string strdir in strDirs)// 遍历所有子文件夹
                    {
                        System.Diagnostics.Debug.Write(strdir + "-deleted");
                        Directory.Delete(strdir, true);// 删除文件夹
                    }
                }
            }
            catch (Exception Exp) // 异常处理
            {
                System.Diagnostics.Debug.Write(Exp.Message.ToString());// 异常信息
            }
        }
        private void SaveAsBinaryFormat(object obj,string fileName)
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            using(Stream fstream = new FileStream(fileName,
                FileMode.Create,FileAccess.Write,FileShare.None))
            {
                binFormat.Serialize(fstream,obj);
            }
            //Console.WriteLine("=>Saved car in binary format");
        }
        private void LoadFromBinaryFile(string fileName)
        {
            BinaryFormatter binFormat = new BinaryFormatter();
            fDisplay = Display.GetSingle();
            //从二进制文件读取对象
            using (Stream fStream = File.OpenRead(fileName))
            {
                ItemRectangle list = (ItemRectangle)binFormat.Deserialize(fStream);
                //Console.WriteLine("list{0}",list);
            }
        }
        private void SaveAsXmlFormat(object obj,string fileName)
        {
            XmlSerializer xmlFormat = new XmlSerializer(typeof(ItemRectangle));
            using (Stream fStream = new FileStream(fileName,
                FileMode.Create,FileAccess.Write,FileShare.None))
            {
                xmlFormat.Serialize(fStream,obj);
            }
        }
        private void SetProjectProperty()
        {
            //将图片信息写入工程属性文件
            //创建工程属性文件
            XmlDocument prodoc = new XmlDocument();
            XmlDeclaration prodec = prodoc.CreateXmlDeclaration("1.0", "utf-8", null);
            prodoc.AppendChild(prodec);
            XmlElement TGUS = prodoc.CreateElement("TGUS");
            prodoc.AppendChild(TGUS);
            XmlElement HardWarexml = prodoc.CreateElement("Hardware_Setup");

            HardWarexml.SetAttribute("ProjectName", Project_name);
            HardWarexml.SetAttribute("Version", $"{Version}");
            HardWarexml.SetAttribute("StartupPage", presentpage_num.ToString());
            HardWarexml.SetAttribute("WIDTH", WIDTH.ToString());
            HardWarexml.SetAttribute("HEIGHT", HEIGHT.ToString());
            HardWarexml.SetAttribute("DisplayscaleNumSelect", DisplayscaleNumSelect.ToString());
            HardWarexml.SetAttribute("R1", HardWare.hardware_str.R1);
            HardWarexml.SetAttribute("R2", HardWare.hardware_str.R2);
            HardWarexml.SetAttribute("R3", HardWare.hardware_str.R3);
            HardWarexml.SetAttribute("R5", HardWare.hardware_str.R5);
            HardWarexml.SetAttribute("R6", HardWare.hardware_str.R6);
            HardWarexml.SetAttribute("R7", HardWare.hardware_str.R7);
            HardWarexml.SetAttribute("R8", HardWare.hardware_str.R8);
            HardWarexml.SetAttribute("R9", HardWare.hardware_str.R9);
            HardWarexml.SetAttribute("RA", HardWare.hardware_str.RA);
            TGUS.AppendChild(HardWarexml);
            XmlElement Imagexml = prodoc.CreateElement("Images");
            for (int i = 0; i < Images_Form.Pic_Number; i++)
            {

                Imagexml.SetAttribute("Image" + i.ToString(), Images_Form.picname[i].safename);
            }
            TGUS.AppendChild(Imagexml);
            prodoc.Save(Environment.CurrentDirectory + "\\TGUS" + "\\" + Project_name);
        }

        private void CheckFilePath()
        {
            if (Directory.Exists(Environment.CurrentDirectory + "\\TGUS" + "\\TGUS_SET") == false)
            {
                //try
                //{
                //    Directory.Delete(System.Environment.CurrentDirectory + "\\TGUS" + "\\TGUS_SET", true);//重新生成的时候直接删除这个文件夹，方便所有的文件重新生成
                //    //删除指定的目录并（如果指示）删除该目录中的所有子目录和文件。
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
                Directory.CreateDirectory(Environment.CurrentDirectory + "\\TGUS" + "\\TGUS_SET");//重新创建TGUS_SET文件夹
            }
        }
        OpenFileDialog openproj = new OpenFileDialog();///打开文件夹对话框
                                                       ///

        List<string> ControlName = new List<string> { };
        List<string> ControlText = new List<string> { };
        private void MapControls(Control[] controls)
        {
            foreach (Control control in controls)
            {
                if(control is LinkLabel|
                    control is NumericUpDown|
                    control is System.Windows.Forms.TextBox|
                    control is DockPanel|
                    control is Panel)
                {
                    continue;
                }
                if(control is ToolStrip)
                {
                    for (int i = 0; i < ((ToolStrip)control).Items.Count;i++ )
                    {
                        if(((ToolStrip)control).Items[i] is ToolStripButton)
                        {
                            ControlName.Add(((ToolStrip)control).Items[i].Name);
                            ControlText.Add(((ToolStrip)control).Items[i].Text);
                        }
                    }
                        continue;
                }
                //控件里面还有别的控件
                if (control.Controls.Count > 0)
                {
                    Control[] tempControls = new Control[control.Controls.Count];
                    control.Controls.CopyTo(tempControls, 0);
                    MapControls(tempControls);
                }
                ControlName.Add(control.Name);
                ControlText.Add(control.Text);
            }
        }
        private void Creat_Control_inf()
        {
            XmlDocument doc = new XmlDocument();
            //创建文件描述
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            doc.AppendChild(dec);
            //创建根节点
            XmlElement root = doc.CreateElement("TGUS");//创建根节点

            doc.AppendChild(root);
            XmlElement xMain_Form = doc.CreateElement("welcome_From");
            xMain_Form.SetAttribute("Form", "welcome_From");
            root.AppendChild(xMain_Form);
            for (int i = 0; i < ControlName.Count;i++ )
            {
                //Console.WriteLine(ControlText[i]);
                if(ControlName[i] != "")
                {
                    XmlElement con = doc.CreateElement(ControlName[i]);
                    con.InnerText = ControlText[i];
                    xMain_Form.AppendChild(con);
                }
            }
            doc.Save(Environment.CurrentDirectory +"\\Conrol.xml");
        }
        /// <summary>
        /// 窗体载入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Load(object sender, EventArgs e)
        {
            Environment.CurrentDirectory = Environment.GetFolderPath(Environment.SpecialFolder.DesktopDirectory);///设置路径

            openproj.InitialDirectory = Environment.CurrentDirectory;///设置打开文件目录->桌面
            /*************************控件属性窗口不可见**********************************/
            try
            {
                DockPlan1.Dock = DockStyle.Fill;
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            Welcome welcome_From = Welcome.GetSingle(But_ProOpen_Click, But_new_Click);
            welcome_From.Show(DockPlan1);
            welcome_From.CloseButtonVisible = false;
            fDisplay = Display.GetSingle();
            fTouch = Touch.GetSingle();
            fSerial_Input = Serial_Input.GetSingle();
            fImages_Form = Images_Form.GetSingle();
        }
        /****************************************************************************/
        /*****************************SD卡相关***************************************/
        /****************************************************************************/
      
        public delegate void MyInvoke(string[] str1);
        protected void ThreadMethod()
        {
            string[] strtemp = GetMobileDiskList();
            MyInvoke mi = new MyInvoke(UpdateForm);
            this.BeginInvoke(mi, new Object[] { strtemp });
            
        }
        public void UpdateForm(string[] strtemp)
        {
            CBox_SDChoice.Items.Clear();
            CBox_SDChoice.Text = "";
            foreach (string str in strtemp)
            {
                if (str != null)
                {
                    CBox_SDChoice.Items.Add(str);
                    try
                    {
                        CBox_SDChoice.SelectedIndex = 0;
                    }
                    catch { };
                }
            }   
        }
        /****************************************************************************/
        /*****************************SD卡相关***************************************/
        /****************************************************************************/
        private void LoadHardInformatioin(string FileName)
        {
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(FileName);
            }
            catch
            {
                MessageBox.Show("File select error OR File Corrupted");
                return ;
            }
            XmlNode node = doc.SelectSingleNode("/TGUS/Hardware_Setup");          
            Project_name = node.Attributes["ProjectName"].Value;
            string ver = node.Attributes["Version"].Value;
            if (ver != Version)
            {
                if(Main_Form.LanguageType == "English")
                {
                    MessageBox.Show($"Soft Version is not {Version}", "Waring", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                    
                }
                else
                {
                    MessageBox.Show($"软件的版本不是{Version}", "警告", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
                }
            }
            presentpage_num = Convert.ToInt32(node.Attributes["StartupPage"].Value);
            
            WIDTH = Convert.ToUInt16(node.Attributes["WIDTH"].Value);
            HEIGHT = Convert.ToUInt16(node.Attributes["HEIGHT"].Value);
            DisplayscaleNumSelect = Convert.ToInt32(node.Attributes["DisplayscaleNumSelect"].Value);
            if((node.Attributes["R1"].Value).Length >0)
            {
                HardWare.hardware_str.R1 = node.Attributes["R1"].Value;
            }
            if ((node.Attributes["R2"].Value).Length > 0)
            {
                HardWare.hardware_str.R2 = node.Attributes["R2"].Value;
            }
            if((node.Attributes["R3"].Value).Length >0)
            {
                HardWare.hardware_str.R3 = node.Attributes["R3"].Value;
            }
            if((node.Attributes["R5"].Value).Length >0)
            {
                HardWare.hardware_str.R5 = node.Attributes["R5"].Value;
            }
            if((node.Attributes["R6"].Value).Length >0)
            {
                HardWare.hardware_str.R6 = node.Attributes["R6"].Value;
            }
            if((node.Attributes["R7"].Value).Length >0)
            {
                HardWare.hardware_str.R7 = node.Attributes["R7"].Value;
            }
            if((node.Attributes["R8"].Value).Length >0)
            {
                HardWare.hardware_str.R8 = node.Attributes["R8"].Value;
            }
            if((node.Attributes["R9"].Value).Length >0)
            {
                HardWare.hardware_str.R9 = node.Attributes["R9"].Value;
            }
            if((node.Attributes["RA"].Value).Length >0)
            {
                HardWare.hardware_str.RA = node.Attributes["RA"].Value;
            }
        }
        public static string LinkText = null;
        private void But_ProOpen_Click(object sender, EventArgs e)
        {
            //先读取工程信息文件
            XmlDocument doc = new XmlDocument();
            string openproject = string.Empty;
            if(LinkText == null)
            {
                OpenFileDialog openFileDialog = new OpenFileDialog();
                openFileDialog.Filter = "*.gxp|*.gxp";
                openFileDialog.Multiselect = false;
                openFileDialog.InitialDirectory = System.Environment.CurrentDirectory;
                openFileDialog.CheckFileExists = true;
                openFileDialog.CheckPathExists = true;
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    try
                    {
                        Project_name = openFileDialog.FileName.Substring(openFileDialog.FileName.LastIndexOf("\\") + 1,
                            openFileDialog.FileName.Length - openFileDialog.FileName.LastIndexOf("\\") - 1);
                        System.Windows.Forms.Application.UserAppDataRegistry.SetValue("prjsavepath", openFileDialog.FileName);    ///注册表中写当前工程的路径
                        // Main_Form.prjsavepath = openFileDialog.FileName;
                        openproject = openFileDialog.FileName;
                    }
                    catch 
                    {
                        MessageBox.Show("File select error OR File Corrupted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }
                else
                {
                    return;
                }
            }
            else
            {
                openproject = LinkText;
                LinkText = null;
                if (!File.Exists(openproject))
                {
                    MessageBox.Show("File select error OR File Corrupted", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
            }
            try
            {
                doc.Load(openproject);
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
                return;
            }
            //加载硬件信息
            LoadHardInformatioin(openproject);
            SetStart();    //打开画面
            prjsavepath = openproject.Substring(0, openproject.LastIndexOf("\\") - 5);
            System.Environment.CurrentDirectory = prjsavepath;                  ///设置工作路径    *重要*  
            this.Text = openproject;
            //Console.WriteLine("save_path = {0}", openproject);
            //先加载图片
            XmlNode xn = doc.SelectSingleNode("/TGUS/Images");
            XmlAttributeCollection xmlA = xn.Attributes;
            int PicNum = 0;
            Images_Form imageform = Images_Form.GetSingle();
           
            foreach(XmlAttribute x in xmlA)
            {
                if (x.Value == "")
                    continue;
                Images_Form.picname[PicNum].name = System.Environment.CurrentDirectory + "\\TGUS" + "\\image" + "\\" + x.Value;
                Images_Form.picname[PicNum].safename = x.Value;
                Images_Form.picname[PicNum].image = Image.FromFile(Images_Form.picname[PicNum].name);
                Images_Form.picname[PicNum].num = PicNum;
                imageform.DGV_PictureList.Rows.Add(1);   
                imageform.DGV_PictureList.Rows[PicNum].Cells[1].Value = x.Value;///获取图片文件名 
                imageform.DGV_PictureList.Rows[PicNum].Cells[0].Value = PicNum.ToString();
                PicNum++;
            }
            Display dis = Display.GetSingle();
            dis.designer1.BackgroundImage = Images_Form.picname[presentpage_num].image;
            Images_Form.Pic_Number = imageform.DGV_PictureList.RowCount;
            //在加载图元
           // try
            //{
                if (System.IO.Directory.Exists(System.Environment.CurrentDirectory + "\\TGUS" + "\\TFT"))// 判断文件夹是否存在
                {
                    string[] strFiles = System.IO.Directory.GetFiles(System.Environment.CurrentDirectory + "\\TGUS" + "\\TFT");// 获得文件数组
                    fDisplay = Display.GetSingle();
                    
                    foreach (string strFile in strFiles)// 遍历所有子文件夹
                    {
                        XmlDocument Gdoc = new XmlDocument();
                        Gdoc.Load(strFile);
                        XmlNode bmp = Gdoc.SelectSingleNode("/root/bmp");
                        XmlNodeList GraphicsInform = Gdoc.SelectNodes("/root/Graphics/Graphic");
                        UInt16 R = 0;
                        UInt16 G = 0;
                        UInt16 B = 0;
                        foreach (XmlNode node in GraphicsInform)
                        {
                            PIC_Obj GrahicsType = (PIC_Obj)(Enum.Parse(typeof(PIC_Obj), node.Attributes["ControlType"].Value));
                            ItemRectangle itemrectangle = new ItemRectangle();
                            System.Drawing.Rectangle rectangle = new System.Drawing.Rectangle(Convert.ToUInt16(node.Attributes["Xs"].Value),Convert.ToUInt16(node.Attributes["Ys"].Value),
                                Convert.ToUInt16(node.Attributes["Width"].Value),Convert.ToUInt16(node.Attributes["Height"].Value));
                            itemrectangle.Rectangle = rectangle;
                            if (node.Attributes["touchvar"].Value == "touch")
                            {
                                itemrectangle.touchvar = ItemBase.TouchOrVar.touch;
                            }
                            else
                            {
                                itemrectangle.touchvar = ItemBase.TouchOrVar.varable;
                            }
                            itemrectangle.presentpage_num = Convert.ToUInt16(node.Attributes["Page"].Value);
                            itemrectangle.ControlType = GrahicsType;
                            itemrectangle.Name_define = node.Attributes["Name"].Value;
                            switch(GrahicsType)
                            {
                                #region case basetouch    
                                case PIC_Obj.basictouch:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                    itemrectangle.BaseTouchInfo.Pic_On = Convert.ToInt32(node.Attributes["Pic_On"].Value);
                                    itemrectangle.BaseTouchInfo.Pic_Next = Convert.ToInt32(node.Attributes["Pic_Next"].Value);
                                    try
                                    {
                                        itemrectangle.BaseTouchInfo.TP_Code = UInt16.Parse(UInt16.Parse(node.Attributes["TP_Code"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    }
                                    catch(Exception ex)
                                    {
                                        MessageBox.Show(ex.Message);
                                        itemrectangle.BaseTouchInfo.TP_Code = 0;
                                    }
                                    break;
                                #endregion
                                #region case data_displaty  
                                case PIC_Obj.data_display:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Red);
                                    itemrectangle.SP = UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    R = Convert.ToUInt16(node.Attributes["R"].Value);
                                    G = Convert.ToUInt16(node.Attributes["G"].Value);
                                    B = Convert.ToUInt16(node.Attributes["B"].Value);
                                    itemrectangle.DataVarInfo.Display_Color = Color.FromArgb(R,G,B);
                                    itemrectangle.DataVarInfo.COLOR = (UInt16)(((itemrectangle.DataVarInfo.Display_Color.R >> 3) << 11)
                                                                   | ((itemrectangle.DataVarInfo.Display_Color.G >> 2) << 5)
                                                                   | ((itemrectangle.DataVarInfo.Display_Color.B >> 3)));
                                    itemrectangle.DataVarInfo.Lib_ID = Convert.ToByte(node.Attributes["Font_ID"].Value);
                                    itemrectangle.DataVarInfo.Font_Size = Convert.ToByte(node.Attributes["Font_Size"].Value);
                                    itemrectangle.DataVarInfo.Font_Align = Convert.ToByte(node.Attributes["Font_Align"].Value);
                                    itemrectangle.DataVarInfo.Var_Type = Convert.ToByte(node.Attributes["Var_Type"].Value);
                                    itemrectangle.DataVarInfo.Integer_Length = Convert.ToByte(node.Attributes["Integer_Length"].Value);
                                    itemrectangle.DataVarInfo.Decimal_Length = Convert.ToByte(node.Attributes["Decimal_Length"].Value);
                                    itemrectangle.DataVarInfo.Len_unit = Convert.ToByte(node.Attributes["Len_unit"].Value);
                                    itemrectangle.DataVarInfo.String_Uint = node.Attributes["String_Uint"].Value;
                                    itemrectangle.DataVarInfo.Initial_Value = Convert.ToInt64(node.Attributes["Initial_Value"].Value);
                                    break;
                                #endregion
                                #region case icon_display  
                                case PIC_Obj.icon_display:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Red);
                                    itemrectangle.SP =  UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.IconVarInformation.Icon_FileName = node.Attributes["Icon_FileName"].Value;
                                    itemrectangle.IconVarInformation.V_Min = Convert.ToUInt16(node.Attributes["V_Min"].Value);
                                    itemrectangle.IconVarInformation.V_Max = Convert.ToUInt16(node.Attributes["V_Max"].Value);
                                    try
                                    {
                                        itemrectangle.IconVarInformation.Icon_IsMinTransparent = Convert.ToBoolean(node.Attributes["Icon_IsMinTtasparnet"].Value);
                                        itemrectangle.IconVarInformation.Icon_IsMaxTransparent = Convert.ToBoolean(node.Attributes["Icon_IsMaxTrasparent"].Value);
                                    }
                                    catch { };
                                    if (itemrectangle.IconVarInformation.Icon_FileName != "")
                                    {
                                        GetIconFiles.Geticon(itemrectangle.IconVarInformation.Icon_FileName);
                                    }
                                    itemrectangle.IconVarInformation.Icon_Min = Convert.ToInt32(node.Attributes["Icon_Min"].Value);
                                    if (itemrectangle.IconVarInformation.Icon_Min >= 1)
                                    {
                                        itemrectangle.IconVarInformation.Icon_MinPic = GetIconFiles.Icon_List[itemrectangle.IconVarInformation.Icon_Min - 1].image;
                                    }
                                    itemrectangle.IconVarInformation.Icon_Max = Convert.ToInt32(node.Attributes["Icon_Max"].Value);
                                    if(itemrectangle.IconVarInformation.Icon_Max >= 1)
                                    {
                                        itemrectangle.IconVarInformation.Icon_MaxPic = GetIconFiles.Icon_List[itemrectangle.IconVarInformation.Icon_Max - 1].image;
                                    }
                                    itemrectangle.IconVarInformation.Icon_Lib = Convert.ToByte(node.Attributes["Icon_Lib"].Value);
                                    itemrectangle.IconVarInformation.Mode = Convert.ToByte(node.Attributes["Mode"].Value);
                                    itemrectangle.IconVarInformation.InitialValue = Convert.ToUInt16(node.Attributes["InitialValue"].Value);
                                    break;
                                #endregion
                                #region case text_dispaly    
                                case PIC_Obj.text_dispaly:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.SkyBlue);
                                    itemrectangle.SP =  UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    R = Convert.ToUInt16(node.Attributes["R"].Value);
                                    G = Convert.ToUInt16(node.Attributes["G"].Value);
                                    B = Convert.ToUInt16(node.Attributes["B"].Value);
                                    itemrectangle.TextDisplayInformation.Display_Color = Color.FromArgb(R, G, B);
                                    itemrectangle.TextDisplayInformation.COLOR = (UInt16)(((itemrectangle.TextDisplayInformation.Display_Color.R >> 3) << 11)
                                                                   | ((itemrectangle.TextDisplayInformation.Display_Color.G >> 2) << 5)
                                                                   | ((itemrectangle.TextDisplayInformation.Display_Color.B >> 3)));
                                    itemrectangle.TextDisplayInformation.Text_length = Convert.ToUInt16(node.Attributes["Text_length"].Value);
                                    itemrectangle.TextDisplayInformation.Font0_ID = Convert.ToByte(node.Attributes["Font0_ID"].Value);
                                    itemrectangle.TextDisplayInformation.Font1_ID = Convert.ToByte(node.Attributes["Font1_ID"].Value);
                                    itemrectangle.TextDisplayInformation.Font_X_Dots = Convert.ToByte(node.Attributes["Font_X_Dots"].Value);
                                    itemrectangle.TextDisplayInformation.Font_Y_Dots = Convert.ToByte(node.Attributes["Font_Y_Dots"].Value);
                                    itemrectangle.TextDisplayInformation.Encode_Mode = Convert.ToByte(node.Attributes["Encode_Mode"].Value);
                                    itemrectangle.TextDisplayInformation.HOR_Dis = Convert.ToByte(node.Attributes["HOR_Dis"].Value);
                                    itemrectangle.TextDisplayInformation.VER_Dis = Convert.ToByte(node.Attributes["VER_Dis"].Value);
                                    itemrectangle.TextDisplayInformation.initial_value = node.Attributes["initial_value"].Value;
                                    break;
                                #endregion
                                #region case rtc_display 
                                case PIC_Obj.rtc_display:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.SkyBlue);
                                    itemrectangle.SP = UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    R = Convert.ToUInt16(node.Attributes["R"].Value);
                                    G = Convert.ToUInt16(node.Attributes["G"].Value);
                                    B = Convert.ToUInt16(node.Attributes["B"].Value);
                                    itemrectangle.RTCDisplayInformatin.Display_Color = Color.FromArgb(R,G,B);
                                    itemrectangle.RTCDisplayInformatin.COLOR = (UInt16)(((itemrectangle.RTCDisplayInformatin.Display_Color.R >> 3) << 11)
                                                                   | ((itemrectangle.RTCDisplayInformatin.Display_Color.G >> 2) << 5)
                                                                   | ((itemrectangle.RTCDisplayInformatin.Display_Color.B >> 3)));
                                    itemrectangle.RTCDisplayInformatin.Lib_ID = Convert.ToByte(node.Attributes["Lib_ID"].Value);
                                    itemrectangle.RTCDisplayInformatin.Font_X_Dots = Convert.ToByte(node.Attributes["Font_X_Dots"].Value);
                                    itemrectangle.RTCDisplayInformatin.String_Code = node.Attributes["String_Code"].Value;
                                    break;
                                #endregion
                                #region case datainput    
                                case PIC_Obj.datainput:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                    itemrectangle.DataInputInformation.Pic_ID = Convert.ToUInt16(node.Attributes["Pic_ID"].Value);
                                    if (node.Attributes["IsDataAutoUpLoad"].Value == "false")
                                        itemrectangle.DataInputInformation.IsDataAutoUpLoad = false;
                                    else
                                        itemrectangle.DataInputInformation.IsDataAutoUpLoad = true;
                                    itemrectangle.DataInputInformation.Pic_Next = Convert.ToInt32(node.Attributes["Pic_Next"].Value);
                                    itemrectangle.DataInputInformation.Pic_On = Convert.ToInt32(node.Attributes["Pic_On"].Value);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.DataInputInformation.V_Type = Convert.ToByte(node.Attributes["V_Type"].Value);
                                    itemrectangle.DataInputInformation.N_Int = Convert.ToByte(node.Attributes["N_Int"].Value);
                                    itemrectangle.DataInputInformation.N_Dot = Convert.ToByte(node.Attributes["N_Dot"].Value);
                                    itemrectangle.DataInputInformation.KeyShowPosition_X = Convert.ToInt32(node.Attributes["KeyShowPosition_X"].Value);
                                    itemrectangle.DataInputInformation.KeyShowPosition_Y = Convert.ToInt32(node.Attributes["KeyShowPosition_Y"].Value);
                                    R = Convert.ToUInt16(node.Attributes["R"].Value);
                                    G = Convert.ToUInt16(node.Attributes["G"].Value);
                                    B = Convert.ToUInt16(node.Attributes["B"].Value);
                                    itemrectangle.DataInputInformation.Display_Color = Color.FromArgb(R, G, B);
                                    itemrectangle.DataInputInformation.COLOR = (UInt16)(((itemrectangle.DataInputInformation.Display_Color.R >> 3) << 11)
                                                                   | ((itemrectangle.DataInputInformation.Display_Color.G >> 2) << 5)
                                                                   | ((itemrectangle.DataInputInformation.Display_Color.B >> 3)));
                                    itemrectangle.DataInputInformation.Lib_ID = Convert.ToByte(node.Attributes["Lib_ID"].Value);
                                    itemrectangle.DataInputInformation.Font_Hor = Convert.ToByte(node.Attributes["Font_Hor"].Value);
                                    itemrectangle.DataInputInformation.CurousColor = Convert.ToByte(node.Attributes["CurousColor"].Value);
                                    itemrectangle.DataInputInformation.Hide_En = Convert.ToByte(node.Attributes["Hide_En"].Value);
                                    itemrectangle.DataInputInformation.KB_Source = Convert.ToByte(node.Attributes["KB_Source"].Value);
                                    itemrectangle.DataInputInformation.PIC_KB = Convert.ToInt32(node.Attributes["PIC_KB"].Value);
                                    itemrectangle.DataInputInformation.AREA_KB_Xs = Convert.ToUInt16(node.Attributes["AREA_KB_Xs"].Value);
                                    itemrectangle.DataInputInformation.AREA_KB_Ys = Convert.ToUInt16(node.Attributes["AREA_KB_Ys"].Value);
                                    itemrectangle.DataInputInformation.AREA_KB_Xe = Convert.ToUInt16(node.Attributes["AREA_KB_Xe"].Value);
                                    itemrectangle.DataInputInformation.AREA_KB_Ye = Convert.ToUInt16(node.Attributes["AREA_KB_Ye"].Value);
                                    itemrectangle.DataInputInformation.AREA_KB_Posation_X = Convert.ToUInt16(node.Attributes["AREA_KB_Posation_X"].Value);
                                    itemrectangle.DataInputInformation.AREA_KB_Posation_Y = Convert.ToUInt16(node.Attributes["AREA_KB_Posation_Y"].Value);
                                    itemrectangle.DataInputInformation.V_min = Convert.ToInt32(node.Attributes["V_min"].Value);
                                    itemrectangle.DataInputInformation.V_max = Convert.ToInt32(node.Attributes["V_max"].Value);
                                    itemrectangle.DataInputInformation.Limits_En = Byte.Parse(Byte.Parse(node.Attributes["Limits_En"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X2"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.DataInputInformation.Return_Set = Convert.ToByte(node.Attributes["Return_Set"].Value);
                                    itemrectangle.DataInputInformation.Return_VP = UInt16.Parse(UInt16.Parse(node.Attributes["Return_VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.DataInputInformation.Return_DATA = Convert.ToUInt16(node.Attributes["Return_DATA"].Value);
                                    break;
                                #endregion
                                #region case key_return    
                                case PIC_Obj.keyreturn:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                    itemrectangle.KeyReturnInformation.Pic_ID = Convert.ToUInt16(node.Attributes["Pic_ID"].Value);
                                    itemrectangle.KeyReturnInformation.Pic_Next = Convert.ToInt32(node.Attributes["Pic_Next"].Value);
                                    itemrectangle.KeyReturnInformation.Pic_On = Convert.ToInt32(node.Attributes["Pic_On"].Value);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.KeyReturnInformation.VP_Mode = Convert.ToByte(node.Attributes["VP_Mode"].Value);
                                    itemrectangle.KeyReturnInformation.Key_Code = UInt16.Parse(UInt16.Parse(node.Attributes["Key_Code"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.KeyReturnInformation.Touch_Key_Code = UInt16.Parse(UInt16.Parse(node.Attributes["Touch_Key_Code"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.KeyReturnInformation.Touch_KeyPressing_Code = UInt16.Parse(UInt16.Parse(node.Attributes["Touch_KeyPressing_Code"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    break;
                                #endregion
                                #region case QR_code
                                case PIC_Obj.QR_display:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.SkyBlue);
                                    itemrectangle.SP = UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.QRCodeInformation.Unit_Pixels = Convert.ToUInt16(node.Attributes["Unit_Pixels"].Value);
                                    break;
                                #endregion
                                #region cae PopupMenu    
                                case PIC_Obj.menu_display:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                    itemrectangle.PopupMenuInformation.Pic_ID = Convert.ToUInt16(node.Attributes["Pic_ID"].Value);
                                    if (node.Attributes["IsDataAutoUpLoad"].Value == "False")
                                    {
                                        itemrectangle.PopupMenuInformation.IsDataAutoUpLoad = false;
                                    }
                                    else
                                    {
                                        itemrectangle.PopupMenuInformation.IsDataAutoUpLoad = true;
                                    }
                                    itemrectangle.PopupMenuInformation.Pic_On = Convert.ToInt32(node.Attributes["Pic_On"].Value);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.PopupMenuInformation.VP_Mode = Convert.ToByte(node.Attributes["VP_Mode"].Value);
                                    itemrectangle.PopupMenuInformation.Pic_Menu = Convert.ToInt32(node.Attributes["Pic_Menu"].Value);
                                    itemrectangle.PopupMenuInformation.AREA_Menu_Xs = Convert.ToUInt16(node.Attributes["AREA_Menu_Xs"].Value);
                                    itemrectangle.PopupMenuInformation.AREA_Menu_Ys = Convert.ToUInt16(node.Attributes["AREA_Menu_Ys"].Value);
                                    itemrectangle.PopupMenuInformation.AREA_Menu_Xe = Convert.ToUInt16(node.Attributes["AREA_Menu_Xe"].Value);
                                    itemrectangle.PopupMenuInformation.AREA_Menu_Ye = Convert.ToUInt16(node.Attributes["AREA_Menu_Ye"].Value);
                                    itemrectangle.PopupMenuInformation.Menu_Position_X = Convert.ToUInt16(node.Attributes["Menu_Position_X"].Value);
                                    itemrectangle.PopupMenuInformation.Menu_Position_Y = Convert.ToUInt16(node.Attributes["Menu_Position_Y"].Value);
                                    break;
                                #endregion
                                #region case ActionIcon    
                                case PIC_Obj.aniicon_display:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.SkyBlue);
                                    itemrectangle.SP = UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.ActionIconInforamtion.V_Start = Convert.ToUInt16(node.Attributes["V_Start"].Value);
                                    itemrectangle.ActionIconInforamtion.V_Stop = Convert.ToUInt16(node.Attributes["V_Stop"].Value);
                                    itemrectangle.ActionIconInforamtion.Icon_FileName = node.Attributes["Icon_FileName"].Value;
                                    itemrectangle.ActionIconInforamtion.Icon_Lib = Convert.ToByte(node.Attributes["Icon_Lib"].Value);

                                    itemrectangle.ActionIconInforamtion.Icon_Stop = Convert.ToUInt16(node.Attributes["Icon_Stop"].Value);
                                    try
                                    {
                                        itemrectangle.ActionIconInforamtion.ISIcon_Stop = Convert.ToBoolean(node.Attributes["ISIcon_Stop"].Value);
                                        itemrectangle.ActionIconInforamtion.ISIcon_Start = Convert.ToBoolean(node.Attributes["ISIcon_Start"].Value);
                                        itemrectangle.ActionIconInforamtion.ISIcon_End = Convert.ToBoolean(node.Attributes["ISIcon_End"].Value);
                                    }
                                    catch { };
                                    itemrectangle.ActionIconInforamtion.Icon_Start = Convert.ToUInt16(node.Attributes["Icon_Start"].Value);
                                    if(itemrectangle.ActionIconInforamtion.Icon_FileName != ""&& itemrectangle.ActionIconInforamtion.Icon_Start >=1)
                                    {
                                        GetIconFiles.Geticon(itemrectangle.ActionIconInforamtion.Icon_FileName);
                                        itemrectangle.ActionIconInforamtion.Icon_StartPic = GetIconFiles.Icon_List[itemrectangle.ActionIconInforamtion.Icon_Start - 1].image;
                                    }
                                    itemrectangle.ActionIconInforamtion.Iconfileselect = Convert.ToInt32(node.Attributes["Iconfileselect"].Value);
                                    itemrectangle.ActionIconInforamtion.Icon_End = Convert.ToUInt16(node.Attributes["Icon_End"].Value);
                                    itemrectangle.ActionIconInforamtion.Mode = Convert.ToByte(node.Attributes["Mode"].Value);
                                    itemrectangle.ActionIconInforamtion.strMode = node.Attributes["strMode"].Value;
                                    itemrectangle.ActionIconInforamtion.Reset_Icon_En = Convert.ToUInt16(node.Attributes["Reset_Icon_En"].Value);
                                    itemrectangle.ActionIconInforamtion.InitlizValue = Convert.ToUInt16(node.Attributes["InitlizValue"].Value);
                                    break;
                                #endregion
                                #region case Incremental Adjustment   
                                case PIC_Obj.increadj:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                    itemrectangle.IncreaseAdjInformation.Pic_ID = Convert.ToUInt16(node.Attributes["Pic_ID"].Value);
                                    itemrectangle.IncreaseAdjInformation.IsDataAutoUpLoad = Convert.ToBoolean(node.Attributes["IsDataAutoUpLoad"].Value);
                                    itemrectangle.IncreaseAdjInformation.Pic_On = Convert.ToInt32(node.Attributes["Pic_On"].Value);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.IncreaseAdjInformation.VP_Mode = Convert.ToByte(node.Attributes["VP_Mode"].Value);
                                    itemrectangle.IncreaseAdjInformation.Adj_Mode = Convert.ToByte(node.Attributes["Adj_Mode"].Value);
                                    itemrectangle.IncreaseAdjInformation.Return_Mode = Convert.ToByte(node.Attributes["Return_Mode"].Value);
                                    itemrectangle.IncreaseAdjInformation.Adj_Step = Convert.ToByte(node.Attributes["Adj_Step"].Value);
                                    itemrectangle.IncreaseAdjInformation.V_Max = Convert.ToByte(node.Attributes["V_Max"].Value);
                                    itemrectangle.IncreaseAdjInformation.V_Min = Convert.ToByte(node.Attributes["V_Min"].Value);
                                    itemrectangle.IncreaseAdjInformation.Key_Mode = Convert.ToByte(node.Attributes["Key_Mode"].Value);
                                    break;
                                #endregion
                                #region case Slider Adjustment   
                                case PIC_Obj.sliadj:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                    itemrectangle.SlideAdjInformation.Pic_ID = Convert.ToUInt16(node.Attributes["Pic_ID"].Value);
                                    itemrectangle.SlideAdjInformation.IsDataAutoUpLoad = Convert.ToBoolean(node.Attributes["IsDataAutoUpLoad"].Value);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.SlideAdjInformation.Adj_Mode = Convert.ToByte(node.Attributes["Adj_Mode"].Value);
                                    itemrectangle.SlideAdjInformation.V_begin = Convert.ToUInt16(node.Attributes["V_begin"].Value);
                                    itemrectangle.SlideAdjInformation.V_end = Convert.ToUInt16(node.Attributes["V_end"].Value);
                                    break;
                                #endregion
                                #region case ArtFont   
                                case PIC_Obj.artfont:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.SkyBlue);
                                    itemrectangle.SP = UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.ArtFontInformation.Icon_FileName = node.Attributes["Icon_FileName"].Value;
                                    itemrectangle.ArtFontInformation.Icon_SelectIndex = Convert.ToInt32(node.Attributes["Icon_SelectIndex"].Value);
                                    itemrectangle.ArtFontInformation.Icon_0 = Convert.ToUInt16(node.Attributes["Icon_0"].Value);
                                    try
                                    {
                                        itemrectangle.ArtFontInformation.Icon_IsTransparent = Convert.ToBoolean(node.Attributes["Icon_IsTransparent"].Value);
                                    }
                                    catch { };
                                    
                                    if(itemrectangle.ArtFontInformation.Icon_FileName != "" && itemrectangle.ArtFontInformation.Icon_0 >= 1)
                                    {
                                        GetIconFiles.Geticon(itemrectangle.ArtFontInformation.Icon_FileName);
                                        itemrectangle.ArtFontInformation.Icon_Pic = GetIconFiles.Icon_List[itemrectangle.ArtFontInformation.Icon_0 - 1].image;
                                    }
                                    itemrectangle.ArtFontInformation.Icon_Lib = Convert.ToByte(node.Attributes["Icon_Lib"].Value);
                                    itemrectangle.ArtFontInformation.Icon_Mode = Convert.ToByte(node.Attributes["Icon_Mode"].Value);
                                    itemrectangle.ArtFontInformation.Integer_Length = Convert.ToByte(node.Attributes["Integer_Length"].Value);
                                    itemrectangle.ArtFontInformation.Decimal_Length = Convert.ToByte(node.Attributes["Decimal_Length"].Value);
                                    itemrectangle.ArtFontInformation.Var_Type = Convert.ToByte(node.Attributes["Var_Type"].Value);
                                    itemrectangle.ArtFontInformation.Align_Mode = Convert.ToByte(node.Attributes["Align_Mode"].Value);
                                    itemrectangle.ArtFontInformation.Init_Value = Convert.ToInt64(node.Attributes["Init_Value"].Value);
                                    break;
                                #endregion
                                #region case slider Display
                                case PIC_Obj.slidis:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.SkyBlue);
                                    itemrectangle.SP = UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.SlideDisplayInformation.V_begain = Convert.ToByte(node.Attributes["V_begain"].Value);
                                    itemrectangle.SlideDisplayInformation.V_end = Convert.ToByte(node.Attributes["V_end"].Value);
                                    itemrectangle.SlideDisplayInformation.X_begain = Convert.ToUInt16(node.Attributes["X_begain"].Value);
                                    itemrectangle.SlideDisplayInformation.X_end = Convert.ToUInt16(node.Attributes["X_end"].Value);
                                    itemrectangle.SlideDisplayInformation.Icon_ID = Convert.ToUInt16(node.Attributes["Icon_ID"].Value);
                                    itemrectangle.SlideDisplayInformation.Icon_FileName = node.Attributes["Icon_FileName"].Value;
                                    itemrectangle.SlideDisplayInformation.Icon_SelectIndex = Convert.ToInt32(node.Attributes["Icon_SelectIndex"].Value);
                                    try
                                    {
                                        itemrectangle.SlideDisplayInformation.Icon_IsTransparent = Convert.ToBoolean(node.Attributes["Icon_IsTransparent"].Value);
                                    }
                                    catch { };
                                    if (itemrectangle.SlideDisplayInformation.Icon_FileName != "" && itemrectangle.SlideDisplayInformation.Icon_ID >= 1)
                                    {
                                        GetIconFiles.Geticon(itemrectangle.SlideDisplayInformation.Icon_FileName);
                                        itemrectangle.SlideDisplayInformation.Icon_Pic = GetIconFiles.Icon_List[itemrectangle.SlideDisplayInformation.Icon_ID - 1].image;
                                    }
                                    itemrectangle.SlideDisplayInformation.Icon_Lib = Convert.ToByte(node.Attributes["Icon_Lib"].Value);
                                    itemrectangle.SlideDisplayInformation.Icon_Mode = Convert.ToByte(node.Attributes["Icon_Mode"].Value);
                                    itemrectangle.SlideDisplayInformation.X_adj = Convert.ToByte(node.Attributes["X_adj"].Value);
                                    itemrectangle.SlideDisplayInformation.Mode = Convert.ToByte(node.Attributes["Mode"].Value);
                                    itemrectangle.SlideDisplayInformation.VP_DATA_Mode = Convert.ToByte(node.Attributes["VP_DATA_Mode"].Value);
                                    itemrectangle.SlideDisplayInformation.InitVal = Convert.ToInt16(node.Attributes["InitVal"].Value);
                                    break;
                                #endregion
                                #region case Icon Rotation
                                case PIC_Obj.iconrota:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.SkyBlue);
                                    itemrectangle.SP = UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.IconRotationInformation.Icon_FileName = node.Attributes["Icon_FileName"].Value;
                                    itemrectangle.IconRotationInformation.Icon_SelectIndex = Convert.ToInt32(node.Attributes["Icon_SelectIndex"].Value);
                                    itemrectangle.IconRotationInformation.Lib_ID = Convert.ToByte(node.Attributes["Lib_ID"].Value);
                                    itemrectangle.IconRotationInformation.Icon_ID = Convert.ToUInt16(node.Attributes["Icon_ID"].Value);
                                    try
                                    {
                                        itemrectangle.IconRotationInformation.Icon_IsTransparent = Convert.ToBoolean(node.Attributes["Icon_IsTransparent"].Value);
                                    }
                                    catch { };
                                    if (itemrectangle.IconRotationInformation.Icon_FileName != "" && itemrectangle.IconRotationInformation.Icon_ID >= 1)
                                    {
                                        GetIconFiles.Geticon(itemrectangle.IconRotationInformation.Icon_FileName);
                                        itemrectangle.IconRotationInformation.Icon_Pic = GetIconFiles.Icon_List[itemrectangle.IconRotationInformation.Icon_ID - 1].image;
                                    }
                                    itemrectangle.IconRotationInformation.Icon_Xc = Convert.ToUInt16(node.Attributes["Icon_Xc"].Value);
                                    itemrectangle.IconRotationInformation.Icon_Yc = Convert.ToUInt16(node.Attributes["Icon_Yc"].Value);

                                    itemrectangle.IconRotationInformation.Xc = Convert.ToUInt16(node.Attributes["Xc"].Value);
                                    itemrectangle.IconRotationInformation.Yc = Convert.ToUInt16(node.Attributes["Yc"].Value);
                                    itemrectangle.IconRotationInformation.V_begain = Convert.ToUInt16(node.Attributes["V_begain"].Value);
                                    itemrectangle.IconRotationInformation.V_end = Convert.ToUInt16(node.Attributes["V_end"].Value);
                                    itemrectangle.IconRotationInformation.AL_begain = Convert.ToUInt16(node.Attributes["AL_begain"].Value);

                                    itemrectangle.IconRotationInformation.AL_end = Convert.ToByte(node.Attributes["AL_end"].Value);
                                    itemrectangle.IconRotationInformation.VP_Mode = Convert.ToByte(node.Attributes["VP_Mode"].Value);
                                    itemrectangle.IconRotationInformation.Lib_ID = Convert.ToByte(node.Attributes["Lib_ID"].Value);
                                    itemrectangle.IconRotationInformation.Mode = Convert.ToByte(node.Attributes["Mode"].Value);
                                    itemrectangle.IconRotationInformation.Init_Value = Convert.ToInt32(node.Attributes["Init_Value"].Value);
                                    break;
                                #endregion
                                #region case Clock Display
                                case PIC_Obj.clockdisplay:
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.SkyBlue);
                                    itemrectangle.SP = UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.ClockDisplayInformation.Icon_FileName = node.Attributes["Icon_FileName"].Value;
                                    itemrectangle.ClockDisplayInformation.Icon_SelectIndex = Convert.ToInt32(node.Attributes["Icon_SelectIndex"].Value);
                                    itemrectangle.ClockDisplayInformation.Icon_Lib = Convert.ToByte(node.Attributes["Icon_Lib"].Value);
                                    itemrectangle.ClockDisplayInformation.X = Convert.ToByte(node.Attributes["X"].Value);
                                    itemrectangle.ClockDisplayInformation.Y = Convert.ToByte(node.Attributes["Y"].Value);
                                    itemrectangle.ClockDisplayInformation.IsDiaplayHour = Convert.ToBoolean(node.Attributes["IsDiaplayHour"].Value);
                                    itemrectangle.ClockDisplayInformation.IsDiaplayMinute = Convert.ToBoolean(node.Attributes["IsDiaplayMinute"].Value);
                                    itemrectangle.ClockDisplayInformation.IsDiaplaySecond = Convert.ToBoolean(node.Attributes["IsDiaplaySecond"].Value);

                                    itemrectangle.ClockDisplayInformation.Icon_Hour = Convert.ToUInt16(node.Attributes["Icon_Hour"].Value);
                                    itemrectangle.ClockDisplayInformation.Icon_Minute = Convert.ToUInt16(node.Attributes["Icon_Minute"].Value);
                                    itemrectangle.ClockDisplayInformation.Icon_Second = Convert.ToUInt16(node.Attributes["Icon_Second"].Value);
                                    if (itemrectangle.ClockDisplayInformation.Icon_FileName != "" && itemrectangle.ClockDisplayInformation.Icon_Hour >= 1)
                                    {
                                        GetIconFiles.Geticon(itemrectangle.ClockDisplayInformation.Icon_FileName);
                                        itemrectangle.ClockDisplayInformation.Icon_HourPic = GetIconFiles.Icon_List[itemrectangle.ClockDisplayInformation.Icon_Hour - 1].image;
                                    }
                                    if (itemrectangle.ClockDisplayInformation.Icon_FileName != "" && itemrectangle.ClockDisplayInformation.Icon_Minute >= 1)
                                    {
                                        GetIconFiles.Geticon(itemrectangle.ClockDisplayInformation.Icon_FileName);
                                        itemrectangle.ClockDisplayInformation.Icon_MinutePic = GetIconFiles.Icon_List[itemrectangle.ClockDisplayInformation.Icon_Minute - 1].image;
                                    }
                                    if (itemrectangle.ClockDisplayInformation.Icon_FileName != "" && itemrectangle.ClockDisplayInformation.Icon_Second >= 1)
                                    {
                                        GetIconFiles.Geticon(itemrectangle.ClockDisplayInformation.Icon_FileName);
                                        itemrectangle.ClockDisplayInformation.Icon_SecondPic = GetIconFiles.Icon_List[itemrectangle.ClockDisplayInformation.Icon_Second - 1].image;
                                    }
                                    itemrectangle.ClockDisplayInformation.Icon_Hour_Central_X = Convert.ToUInt16(node.Attributes["Icon_Hour_Central_X"].Value);
                                    itemrectangle.ClockDisplayInformation.Icon_Hour_Central_Y = Convert.ToUInt16(node.Attributes["Icon_Hour_Central_Y"].Value);
                                    itemrectangle.ClockDisplayInformation.Icon_Minute_Central_X = Convert.ToUInt16(node.Attributes["Icon_Minute_Central_X"].Value);
                                    itemrectangle.ClockDisplayInformation.Icon_Minute_Central_Y = Convert.ToUInt16(node.Attributes["Icon_Minute_Central_Y"].Value);
                                    itemrectangle.ClockDisplayInformation.Icon_Second_Central_X = Convert.ToUInt16(node.Attributes["Icon_Second_Central_X"].Value);
                                    itemrectangle.ClockDisplayInformation.Icon_Second_Central_Y = Convert.ToUInt16(node.Attributes["Icon_Second_Central_Y"].Value);
                                    break;
                                #endregion
                                #region case GBK
                                case PIC_Obj.GBK:
                                    try    //版本兼容1.3、1.4、1.5
                                    {
                                        itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                        itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                        itemrectangle.GBKInformation.IsDataAutoUpLoad = Convert.ToByte(node.Attributes["IsDataAutoUpLoad"].Value);
                                        itemrectangle.GBKInformation.Pic_Next = Convert.ToInt32(node.Attributes["Pic_Next"].Value);
                                        itemrectangle.GBKInformation.Pic_On = Convert.ToInt32(node.Attributes["Pic_On"].Value);
                                        if (itemrectangle.GBKInformation.Pic_Next >= 0 && itemrectangle.GBKInformation.Pic_Next <= max_Page)
                                            itemrectangle.GBKInformation.Pic_NextPic = Images_Form.picname[itemrectangle.GBKInformation.Pic_Next].image;
                                        if (itemrectangle.GBKInformation.Pic_On >= 0 && itemrectangle.GBKInformation.Pic_On <= max_Page)
                                            itemrectangle.GBKInformation.Pic_OnPic = Images_Form.picname[itemrectangle.GBKInformation.Pic_On].image;
                                        itemrectangle.GBKInformation.VP_Len_Max = Convert.ToInt32(node.Attributes["VP_Len_Max"].Value);
                                        itemrectangle.GBKInformation.Scan_Mode = Convert.ToByte(node.Attributes["Scan_Mode"].Value);
                                        itemrectangle.GBKInformation.Lib_GBK1 = Convert.ToByte(node.Attributes["Lib_GBK1"].Value);
                                        itemrectangle.GBKInformation.Lib_GBK2 = Convert.ToByte(node.Attributes["Lib_GBK2"].Value);
                                        itemrectangle.GBKInformation.Font_Scale1 = Convert.ToByte(node.Attributes["Font_Scale1"].Value);
                                        itemrectangle.GBKInformation.Font_Scale2 = Convert.ToByte(node.Attributes["Font_Scale2"].Value);
                                        itemrectangle.GBKInformation.Cusor_Color = Convert.ToByte(node.Attributes["Cusor_Color"].Value);
                                        Console.WriteLine($"Uint16 = {UInt16.MaxValue}");
                                        itemrectangle.GBKInformation.ColorNum1 = UInt16.Parse(UInt16.Parse(node.Attributes["ColorNum1"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                        itemrectangle.GBKInformation.ColorNum2 = UInt16.Parse(UInt16.Parse(node.Attributes["ColorNum2"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);

                                        itemrectangle.GBKInformation.PY_Disp_Mode = Convert.ToByte(node.Attributes["PY_Disp_Mode"].Value);
                                        itemrectangle.GBKInformation.Scan_Return_Mode = Convert.ToByte(node.Attributes["Scan_Return_Mode"].Value);
                                        itemrectangle.GBKInformation.Scan0_Area_Start_Xs = Convert.ToUInt16(node.Attributes["Scan0_Area_Start_Xs"].Value);
                                        itemrectangle.GBKInformation.Scan0_Area_Start_Ys = Convert.ToUInt16(node.Attributes["Scan0_Area_Start_Ys"].Value);
                                        itemrectangle.GBKInformation.Scan0_Area_End_Xe = Convert.ToUInt16(node.Attributes["Scan0_Area_End_Xe"].Value);
                                        itemrectangle.GBKInformation.Scan0_Area_End_Ye = Convert.ToUInt16(node.Attributes["Scan0_Area_End_Ye"].Value);
                                        itemrectangle.GBKInformation.Scan1_Area_Start_Xs = Convert.ToUInt16(node.Attributes["Scan1_Area_Start_Xs"].Value);
                                        itemrectangle.GBKInformation.Scan1_Area_Start_Ys = Convert.ToUInt16(node.Attributes["Scan1_Area_Start_Ys"].Value);
                                        itemrectangle.GBKInformation.Scan_Dis = Convert.ToByte(node.Attributes["Scan_Dis"].Value);
                                        itemrectangle.GBKInformation.KB_Source = Convert.ToByte(node.Attributes["KB_Source"].Value);
                                        itemrectangle.GBKInformation.PIC_KB = Convert.ToInt32(node.Attributes["PIC_KB"].Value);
                                        if (itemrectangle.GBKInformation.PIC_KB >= 0)
                                            itemrectangle.GBKInformation.PIC_KBPic = Images_Form.picname[itemrectangle.GBKInformation.PIC_KB].image;

                                        itemrectangle.GBKInformation.AREA_KB_Xs = Convert.ToUInt16(node.Attributes["AREA_KB_Xs"].Value);
                                        itemrectangle.GBKInformation.AREA_KB_Ys = Convert.ToUInt16(node.Attributes["AREA_KB_Ys"].Value);
                                        itemrectangle.GBKInformation.AREA_KB_Xe = Convert.ToUInt16(node.Attributes["AREA_KB_Xe"].Value);
                                        itemrectangle.GBKInformation.AREA_KB_Ye = Convert.ToUInt16(node.Attributes["AREA_KB_Ye"].Value);
                                        itemrectangle.GBKInformation.AREA_KB_Position_Xs = Convert.ToUInt16(node.Attributes["AREA_KB_Position_Xs"].Value);
                                        itemrectangle.GBKInformation.AREA_KB_Position_Ys = Convert.ToUInt16(node.Attributes["AREA_KB_Position_Ys"].Value);
                                        itemrectangle.GBKInformation.SCAN_MODE = Convert.ToByte(node.Attributes["SCAN_MODE"].Value);
                                    }
                                    catch { };
                                    break;
                                #endregion
                                #region case ASCII
                                case PIC_Obj.ASCII:
                                    try  //版本兼容1.3、1.4、1.5
                                    {
                                        itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                        itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                        itemrectangle.ASCIIInformation.IsDataAutoUpLoad = Convert.ToByte(node.Attributes["IsDataAutoUpLoad"].Value);
                                        itemrectangle.ASCIIInformation.Pic_Next = Convert.ToInt32(node.Attributes["Pic_Next"].Value);
                                        itemrectangle.ASCIIInformation.Pic_On = Convert.ToInt32(node.Attributes["Pic_On"].Value);
                                        if (itemrectangle.ASCIIInformation.Pic_Next >= 0 && itemrectangle.ASCIIInformation.Pic_Next < max_Page)
                                            itemrectangle.ASCIIInformation.Pic_NextPic = Images_Form.picname[itemrectangle.ASCIIInformation.Pic_Next].image;
                                        if (itemrectangle.ASCIIInformation.Pic_On >= 0 && itemrectangle.ASCIIInformation.Pic_On < max_Page)
                                            itemrectangle.ASCIIInformation.Pic_OnPic = Images_Form.picname[itemrectangle.ASCIIInformation.Pic_On].image;
                                        itemrectangle.ASCIIInformation.VP_Len_Max = Convert.ToInt32(node.Attributes["VP_Len_Max"].Value);
                                        itemrectangle.ASCIIInformation.Scan_Mode = Convert.ToByte(node.Attributes["Scan_Mode"].Value);
                                        itemrectangle.ASCIIInformation.Lib_ID = Convert.ToByte(node.Attributes["Lib_ID"].Value);
                                        itemrectangle.ASCIIInformation.Font_Hor = Convert.ToByte(node.Attributes["Font_Hor"].Value);
                                        itemrectangle.ASCIIInformation.Font_Ver = Convert.ToByte(node.Attributes["Font_Ver"].Value);
                                        itemrectangle.ASCIIInformation.Cusor_Color = Convert.ToByte(node.Attributes["Cusor_Color"].Value);
                                        itemrectangle.ASCIIInformation.ColorNum = UInt16.Parse(UInt16.Parse(node.Attributes["ColorNum"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);


                                        itemrectangle.ASCIIInformation.Scan_Return_Mode = Convert.ToByte(node.Attributes["Scan_Return_Mode"].Value);
                                        itemrectangle.ASCIIInformation.Scan_Area_Start_Xs = Convert.ToUInt16(node.Attributes["Scan_Area_Start_Xs"].Value);
                                        itemrectangle.ASCIIInformation.Scan_Area_Start_Ys = Convert.ToUInt16(node.Attributes["Scan_Area_Start_Ys"].Value);
                                        itemrectangle.ASCIIInformation.Scan_Area_End_Xe = Convert.ToUInt16(node.Attributes["Scan_Area_End_Xe"].Value);
                                        itemrectangle.ASCIIInformation.Scan_Area_End_Ye = Convert.ToUInt16(node.Attributes["Scan_Area_End_Ye"].Value);

                                        itemrectangle.ASCIIInformation.KB_Source = Convert.ToByte(node.Attributes["KB_Source"].Value);
                                        itemrectangle.ASCIIInformation.PIC_KB = Convert.ToInt32(node.Attributes["PIC_KB"].Value);
                                        if (itemrectangle.ASCIIInformation.PIC_KB >= 0)
                                            itemrectangle.ASCIIInformation.PIC_KBPic = Images_Form.picname[itemrectangle.ASCIIInformation.PIC_KB].image;

                                        itemrectangle.ASCIIInformation.AREA_KB_Xs = Convert.ToUInt16(node.Attributes["AREA_KB_Xs"].Value);
                                        itemrectangle.ASCIIInformation.AREA_KB_Ys = Convert.ToUInt16(node.Attributes["AREA_KB_Ys"].Value);
                                        itemrectangle.ASCIIInformation.AREA_KB_Xe = Convert.ToUInt16(node.Attributes["AREA_KB_Xe"].Value);
                                        itemrectangle.ASCIIInformation.AREA_KB_Ye = Convert.ToUInt16(node.Attributes["AREA_KB_Ye"].Value);
                                        itemrectangle.ASCIIInformation.AREA_KB_Position_Xs = Convert.ToUInt16(node.Attributes["AREA_KB_Position_Xs"].Value);
                                        itemrectangle.ASCIIInformation.AREA_KB_Position_Ys = Convert.ToUInt16(node.Attributes["AREA_KB_Position_Ys"].Value);
                                        itemrectangle.ASCIIInformation.DISPLAY_EN = Convert.ToByte(node.Attributes["DISPLAY_EN"].Value);
                                    }
                                    catch { };
                                    break;
                            #endregion
                            #region case TouchState
                            case PIC_Obj.TouchState:
                                try  //版本兼容1.3、1.4、1.5
                                {
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                    itemrectangle.TouchStateInformation.Pic_Next = Convert.ToInt32(node.Attributes["Pic_Next"].Value);
                                    itemrectangle.TouchStateInformation.Pic_On = Convert.ToInt32(node.Attributes["Pic_On"].Value);
                                    if (itemrectangle.TouchStateInformation.Pic_Next >= 0 && itemrectangle.ASCIIInformation.Pic_Next < max_Page)
                                        itemrectangle.TouchStateInformation.Pic_NextPic = Images_Form.picname[itemrectangle.ASCIIInformation.Pic_Next].image;
                                    if (itemrectangle.TouchStateInformation.Pic_On >= 0 && itemrectangle.ASCIIInformation.Pic_On < max_Page)
                                        itemrectangle.TouchStateInformation.Pic_OnPic = Images_Form.picname[itemrectangle.ASCIIInformation.Pic_On].image;
                                    itemrectangle.TouchStateInformation.TP_ON_Mode = Convert.ToByte(node.Attributes["TP_ON_Mode"].Value);
                                    itemrectangle.TouchStateInformation.VP1S = UInt16.Parse(UInt16.Parse(node.Attributes["VP1S"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.TouchStateInformation.VP1T = UInt16.Parse(UInt16.Parse(node.Attributes["VP1T"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.TouchStateInformation.LEN1 = Convert.ToByte(node.Attributes["LEN1"].Value);

                                    itemrectangle.TouchStateInformation.TP_ON_Continue_Mode = Convert.ToByte(node.Attributes["TP_ON_Continue_Mode"].Value);
                                    itemrectangle.TouchStateInformation.VP2S = UInt16.Parse(UInt16.Parse(node.Attributes["VP2S"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.TouchStateInformation.VP2T = UInt16.Parse(UInt16.Parse(node.Attributes["VP2T"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.TouchStateInformation.LEN2 = Convert.ToByte(node.Attributes["LEN2"].Value);

                                    itemrectangle.TouchStateInformation.TP_OFF_Mode = Convert.ToByte(node.Attributes["TP_OFF_Mode"].Value);
                                    itemrectangle.TouchStateInformation.VP3S = UInt16.Parse(UInt16.Parse(node.Attributes["VP3S"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.TouchStateInformation.VP3T = UInt16.Parse(UInt16.Parse(node.Attributes["VP3T"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.TouchStateInformation.LEN3 = Convert.ToByte(node.Attributes["LEN3"].Value);
                                }
                                catch { };
                                break;
                            #endregion
                            #region
                            case PIC_Obj.RTC_Set:
                                try  //版本兼容1.3、1.4、1.5
                                {
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.Yellow);
                                    itemrectangle.RTCsetInformation.Pic_On = Convert.ToInt32(node.Attributes["Pic_On"].Value);
                                    if (itemrectangle.RTCsetInformation.Pic_On >= 0 && itemrectangle.ASCIIInformation.Pic_On < max_Page)
                                        itemrectangle.RTCsetInformation.Pic_OnPic = Images_Form.picname[itemrectangle.ASCIIInformation.Pic_On].image;
                                    itemrectangle.RTCsetInformation.TP_Code = UInt16.Parse(UInt16.Parse(node.Attributes["TP_Code"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.RTCsetInformation.DisplayPoint_X = Convert.ToUInt16(node.Attributes["DisplayPoint_X"].Value);
                                    itemrectangle.RTCsetInformation.DisplayPoint_Y = Convert.ToUInt16(node.Attributes["DisplayPoint_Y"].Value);
                                    itemrectangle.RTCsetInformation.ColorNum = UInt16.Parse(UInt16.Parse(node.Attributes["ColorNum"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);

                                    itemrectangle.RTCsetInformation.Lib_ID = Convert.ToByte(node.Attributes["Lib_ID"].Value);
                                    itemrectangle.RTCsetInformation.Font_Hor = Convert.ToByte(node.Attributes["Font_Hor"].Value);
                                    itemrectangle.RTCsetInformation.Cusor_Color = Convert.ToByte(node.Attributes["Cusor_Color"].Value);
                                    itemrectangle.RTCsetInformation.KB_Source = Convert.ToByte(node.Attributes["KB_Source"].Value);
                                    itemrectangle.RTCsetInformation.PIC_KB = Convert.ToInt32(node.Attributes["PIC_KB"].Value);
                                    if (itemrectangle.RTCsetInformation.PIC_KB >= 0 && itemrectangle.RTCsetInformation.PIC_KB < max_Page)
                                        itemrectangle.RTCsetInformation.PIC_KBPic = Images_Form.picname[itemrectangle.RTCsetInformation.PIC_KB].image;
                                    itemrectangle.RTCsetInformation.AREA_KB_Xs = Convert.ToUInt16(node.Attributes["AREA_KB_Xs"].Value);
                                    itemrectangle.RTCsetInformation.AREA_KB_Ys = Convert.ToUInt16(node.Attributes["AREA_KB_Ys"].Value);
                                    itemrectangle.RTCsetInformation.AREA_KB_Xe = Convert.ToUInt16(node.Attributes["AREA_KB_Xe"].Value);
                                    itemrectangle.RTCsetInformation.AREA_KB_Ye = Convert.ToUInt16(node.Attributes["AREA_KB_Ye"].Value);
                                    itemrectangle.RTCsetInformation.AREA_KB_Position_Xs = Convert.ToUInt16(node.Attributes["AREA_KB_Position_Xs"].Value);
                                    itemrectangle.RTCsetInformation.AREA_KB_Position_Ys = Convert.ToUInt16(node.Attributes["AREA_KB_Position_Ys"].Value);
                                }
                                catch { };
                                break;
                            #endregion
                            #region case BasicGra
                            case PIC_Obj.BasicGra:
                                try
                                {
                                    itemrectangle.FillColor = Color.FromArgb(60, Color.SkyBlue);
                                    itemrectangle.SP = UInt16.Parse(UInt16.Parse(node.Attributes["SP"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.VP = UInt16.Parse(UInt16.Parse(node.Attributes["VP"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.BasicGraInformation.Dashed_Line_En = Byte.Parse(Byte.Parse(node.Attributes["Dashed_Line_En"].Value,
                                                            System.Globalization.NumberStyles.HexNumber).ToString("X2"), System.Globalization.NumberStyles.HexNumber);
                                    itemrectangle.BasicGraInformation.Dash_Set_1 = Convert.ToByte(node.Attributes["Dash_Set_1"].Value);
                                    itemrectangle.BasicGraInformation.Dash_Set_2 = Convert.ToByte(node.Attributes["Dash_Set_2"].Value);
                                    itemrectangle.BasicGraInformation.Dash_Set_3 = Convert.ToByte(node.Attributes["Dash_Set_3"].Value);
                                    itemrectangle.BasicGraInformation.Dash_Set_4 = Convert.ToByte(node.Attributes["Dash_Set_4"].Value);
                                }
                                catch { }
                                break;
#endregion

                        }
                            fDisplay.designer1.Items.Add(itemrectangle);
                        }
                    }
                }
            //}
            //catch (Exception Exp) // 异常处理
            //{
            //    //System.Diagnostics.Debug.Write(Exp.Message.ToString());// 异常信息
            //    MessageBox.Show(Exp.Message);
            //}    
        }
        public void SetStart()
        {
            int Displayflag = -1;
            foreach(DockContent from in this.DockPlan1.Contents)
            {
                //Console.WriteLine(from.Name);
                if(from.Name == "Display")
                {
                    Displayflag = 0;
                    break;
                }
            }
            if(Displayflag == -1)
            {
                fDisplay.Show(DockPlan1);
                fDisplay.CloseButtonVisible = false;
                fTouch.Show(DockPlan1);
                fTouch.CloseButtonVisible = false;
                fTouch.DockPanel.DockRightPortion = fTouch_Width / Width;
                fImages_Form.Show(DockPlan1);
                fImages_Form.DockTo(DockPlan1, DockStyle.Left);
                fImages_Form.CloseButton = false;
                fImages_Form.CloseButtonVisible = false;
                fImages_Form.DockPanel.DockLeftPortion = (218.0 / Width);     ///Dock停靠大小不是根据尺寸设置的，而是根据比例！！！！！
                fImages_Form.AutoHidePortion = (218.0 / Width);            ////自动隐藏停靠显示
                fSerial_Input.Show(DockPlan1);
                fSerial_Input.CloseButtonVisible = false;
                
                //fSerial_out.Show(this.DockPlan1);
                //fSerial_out.CloseButtonVisible = false;
                fDisplay.DockTo(DockPlan1, DockStyle.None); 
                fTouch.DockTo(DockPlan1, DockStyle.Right);
                fSerial_Input.DockTo(DockPlan1, DockStyle.Bottom);
                fSerial_Input.VisibleState = DockState.DockBottomAutoHide;
                //fSerial_out.DockTo(this.DockPlan1, DockStyle.Bottom);
                //fSerial_out.VisibleState = DockState.DockBottomAutoHide;
            }
            setstart_flag = true;
            fDisplay.Activate();
          
        }
        /// <summary>
        /// 调节弹窗动态显示比例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Main_Resize(object sender, EventArgs e)
        {
            Main_Form main = this;
            int i = 0;
            foreach (IDockContent content in main.DockPlan1.Contents)
            {
                if (content.DockHandler.TabText == "Images")
                {
                    fImages_Form.DockPanel.DockLeftPortion = (double)(218.0 / Width);
                    fImages_Form.DGV_PictureList.Height = this.Height - 220;
                    fImages_Form.GBox_Images1.Height = this.Height - 200;
                    i++;
                }
                if (content.DockHandler.TabText == "Touch")
                {
                    fTouch.DockPanel.DockRightPortion = fTouch_Width / Width;
                    Change_GBox_Height(fTouch, Height - 180);
                    i++;
                }
                if (i == 2)
                    break;
            }
        }
        void Change_GBox_Height(Touch touch,int Height)
        {
            touch.GBox_PageProperties.Height = Height;
            touch.GBox_BaseTouch.Height = Height;
            touch.GBox_DataVar.Height = Height;
            touch.GBox_IconVar.Height = Height;
            touch.GBox_TxtDisplay.Height = Height;
            touch.GBox_RTC.Height = Height;
            touch.GBox_VarInput.Height = Height;
            touch.GBox_KeyReturn.Height = Height;
            touch.GBox_QR.Height = Height;
            touch.GBox_MeunDis.Height = Height;
            touch.GBox_ActionIcon.Height = Height;
            touch.GBox_IncreaseAdj.Height = Height;
            touch.GBox_SlideAdj.Height = Height;
            touch.GBox_ArtFont.Height = Height;
            touch.GBox_SlideDisplay.Height = Height;
            touch.GBox_IconSpin.Height = Height;
            touch.GBox_clock.Height = Height;
            touch.GBox_GBK.Height = Height;
            touch.GBox_ASCII.Height = Height;
            touch.GBox_BaseTouch.Height = Height;
            touch.GBox_RTCset.Height = Height;
            touch.GBox_BasicGra.Height = Height;
        }
        //mouse_name属性
        public string StrCursor
        {
            get { return mouse_name;}
        }

        public static int Max_Page => max_Page;

        private void But_Control_Click(object sender, EventArgs e)
        {
            if(setstart_flag == false || fImages_Form.DGV_PictureList.RowCount == 0)  //没有建立工程的时候不能打开任何控件
            {
                return;
            }
            string str = sender.ToString();
            int tag = Convert.ToInt32(((ToolStripButton)sender).Tag);
            StrMouseName = str + Environment.NewLine; ;
            switch (tag)
            {
                case 1:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.basictouch, str);
                    break;
                case 2:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.other;
                    ButAll_Click(PIC_Obj.data_display, str);
                    break;
                case 3:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.other;
                    ButAll_Click(PIC_Obj.icon_display, str);
                    break;
                case 4:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.variable;
                    ButAll_Click(PIC_Obj.text_dispaly, str);
                    break;
                case 5:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.variable;
                    ButAll_Click(PIC_Obj.rtc_display, str);
                    break;
                case 6:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.datainput, str);
                    break;
                case 7:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.keyreturn, str);
                    break;
                case 8:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.variable;
                    ButAll_Click(PIC_Obj.QR_display, str);
                    break;
                case 9:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.menu_display, str);
                    break;
                case 10:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.variable;
                    ButAll_Click(PIC_Obj.aniicon_display, str);
                    break;
                case 11:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.increadj, str);
                    break;
                case 12:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.sliadj, str);
                    break;
                case 13:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.variable;
                    ButAll_Click(PIC_Obj.artfont, str);
                    break;
                case 14:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.variable;
                    ButAll_Click(PIC_Obj.slidis, str);
                    break;
                case 15:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.variable;
                    ButAll_Click(PIC_Obj.iconrota, str);
                    break;
                case 16:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.variable;
                    ButAll_Click(PIC_Obj.clockdisplay, str);
                    break;
                case 17:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.GBK, str);
                    break;
                case 18:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.ASCII, str);
                    break;
                case 19:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.TouchState, str);
                    break;
                case 20:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.touch;
                    ButAll_Click(PIC_Obj.RTC_Set, str);
                    break;
                case 21:
                    fDisplay.designer1.ActiveControl = fDisplay.DrawRec;
                    VarType = myGraphicsType.variable;
                    ButAll_Click(PIC_Obj.BasicGra, str);
                    break;
            }
        }
        private void ButAll_Click(PIC_Obj obj, string str)
        {
            if (setstart_flag == true)
            {
                //fTouch.PIC_GBoxShow(obj);
                mouse_name = str + Environment.NewLine;
                SelectType = obj;
                if (drawstr_flag == false)
                {
                    drawstr_flag = true;

                }
            }
        }
        private void But_Resolution_Click(object sender, EventArgs e)
        {
            if (!setstart_flag) return;
            Screen_Attribute scr = Screen_Attribute.GetSingle();
            if(scr.ShowDialog() == DialogResult.OK)
            {
                scr.Visible = false;
            }
        }
        private void But_IconCreat_Click(object sender, EventArgs e)
        {
            ImageConvert imageconvert = new ImageConvert();
            imageconvert.Show();
        }
        private void But_MouseMove(object sender, MouseEventArgs e)
        {
            this.Cursor = Cursors.Hand;
        }
        private void But_MouseLeave(object sender, EventArgs e)
        {
            this.Cursor = Cursors.Default;
        }
        private void But_HardWare_Click(object sender, EventArgs e)
        {
            if (!setstart_flag)
            {
                MessageBox.Show("Please Set a new Project");
                return;
            }
            HardWare hardware = HardWare.GetSingle();
            if(hardware.ShowDialog() == DialogResult.OK)
            {
                hardware.Visible = false;
            }
        }
        private byte[] Num;
        private byte[] Num2;
        private void But_ConfigFile_Click(object sender, EventArgs e)
        {
            DeleteDir(Environment.CurrentDirectory + @"\TGUS\TGUS_SET"); //清除文件夹下的文件
            FileStream projstream  = new FileStream(System.Environment.CurrentDirectory + @"\TGUS\TGUS_SET\13_touch.bin", FileMode.Create);
            for (int i = 0; i < Images_Form.Pic_Number; i++)
            {
                if (Directory.Exists(Environment.CurrentDirectory + "\\TGUS" + "\\TGUS_SET"))
                {
                    try
                    {
                        File.Copy(Images_Form.picname[i].name, Environment.CurrentDirectory + "\\TGUS" + "\\TGUS_SET" +
                            "\\" + (i).ToString().TrimStart() + ".bmp");
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message);
                    }
                }
            }
            projstream.Close();
            projstream = new FileStream(Environment.CurrentDirectory + @"\TGUS\TGUS_SET\22_initdata.bin",FileMode.Create);
            projstream.SetLength(12*1024);   //文件大小固定12k
            projstream.Close();
            projstream = new FileStream(Environment.CurrentDirectory + @"\TGUS\TGUS_SET\14_variable.bin", FileMode.Create);
            projstream.SetLength((Images_Form.Pic_Number * 2) * 1024);
            projstream.Close();
            SaveSys_Conf();
            CopyIcon(System.Environment.CurrentDirectory + @"\TGUS\I", System.Environment.CurrentDirectory + @"\TGUS\TGUS_SET");
            //统计变量显示在每一页的数量
            Num = new byte[Images_Form.Pic_Number];
            Num2 = new byte[Images_Form.Pic_Number];
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if(list.touchvar == ItemBase.TouchOrVar.varable)
                {
                    Num[list.presentpage_num]++;
                    Num2[list.presentpage_num]++;
                }
            }
            fDisplay.designer1.Items.Sort();//按照页进行排序

            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                SaveTouch(list);
                SaveVariable(list);
                SaveInitData(list);
            }
            FileStream touchstream = new FileStream(Environment.CurrentDirectory + @"\TGUS\TGUS_SET\13_touch.bin", FileMode.Append, FileAccess.Write);
            touchstream.WriteByte(0xff);
            touchstream.WriteByte(0xff);
            touchstream.Close();
            MessageBox.Show("Configuration file successfully generated", "Notice", MessageBoxButtons.OK,MessageBoxIcon.Information);
        }
        private void CopyIcon(string iconfilepath,string setfilepath)
        {
            DirectoryInfo TheFolder = new DirectoryInfo(iconfilepath);
            List<string> IconList = new List<string> { };
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                switch(list.ControlType)
                {
                    case PIC_Obj.aniicon_display:
                        if (!IconList.Contains(list.ActionIconInforamtion.Icon_FileName))
                            IconList.Add(list.ActionIconInforamtion.Icon_FileName);
                        break;
                    case PIC_Obj.artfont:
                        if (!IconList.Contains(list.ArtFontInformation.Icon_FileName))
                            IconList.Add(list.ArtFontInformation.Icon_FileName);
                        break;
                    case PIC_Obj.clockdisplay:
                        if (!IconList.Contains(list.ClockDisplayInformation.Icon_FileName))
                            IconList.Add(list.ClockDisplayInformation.Icon_FileName);
                        break;
                    case PIC_Obj.iconrota:
                        if (!IconList.Contains(list.IconRotationInformation.Icon_FileName))
                            IconList.Add(list.IconRotationInformation.Icon_FileName);
                        break;
                    case PIC_Obj.icon_display:
                        if (!IconList.Contains(list.IconVarInformation.Icon_FileName))
                            IconList.Add(list.IconVarInformation.Icon_FileName);
                        break;
                    case PIC_Obj.slidis:
                        if (!IconList.Contains(list.SlideDisplayInformation.Icon_FileName))
                            IconList.Add(list.SlideDisplayInformation.Icon_FileName);
                        break;
                    default:
                        break;

                }
            }
            //遍历文件
            foreach (FileInfo NextFile in TheFolder.GetFiles())
            {
                if(IconList.Contains(NextFile.Name))
                    File.Copy(NextFile.FullName, setfilepath + "\\" + NextFile.Name, true); 
            }
        }
        private void SaveSys_Conf()
        {
            StreamWriter file = new StreamWriter(Environment.CurrentDirectory + @"\TGUS\TGUS_SET\sys_conf.txt", false);
            if (HardWare.hardware_str.R1 == null)
            {
                file.WriteLine("R1=07;");
            }
            else
            {
                if (HardWare.hardware_str.R1.Length == 1)
                {
                    file.WriteLine("R1={0};", "0" + HardWare.hardware_str.R1);
                }
                else
                {
                    file.WriteLine("R1={0};", HardWare.hardware_str.R1);
                }
            }
            if (HardWare.hardware_str.R2 == null)
            {
                file.WriteLine("R2=00;");
            }
            else
            {
                if (HardWare.hardware_str.R2.Length == 1)
                {
                    file.WriteLine("R2={0};", "0" + HardWare.hardware_str.R2);
                }
                else
                {
                    file.WriteLine("R2={0};", HardWare.hardware_str.R2);
                }
            }
            if (HardWare.hardware_str.R3 == null)
            {
                file.WriteLine("R3=5A;");
            }
            else
            {
                if (HardWare.hardware_str.R3.Length == 1)
                {
                    file.WriteLine("R3={0};", "0" + HardWare.hardware_str.R3);
                }
                else
                {
                    file.WriteLine("R3={0};", HardWare.hardware_str.R3);
                }
            }
            file.WriteLine("R4=00;");
            if(HardWare.hardware_str.R5 == null)
            {
                file.WriteLine("R5=00;");
            }
            else
            {
                 if (HardWare.hardware_str.R5.Length == 1)
                {
                    file.WriteLine("R5={0};", "0" + HardWare.hardware_str.R5);
                }
                else
                {
                    file.WriteLine("R5={0};", HardWare.hardware_str.R5);
                }
            }
            if (HardWare.hardware_str.R6 == null)
            {
                file.WriteLine("R6=00;");
            }
            else
            {
                if (HardWare.hardware_str.R6.Length == 1)
                {
                    file.WriteLine("R6={0};", "0" + HardWare.hardware_str.R6);
                }
                else
                {
                    file.WriteLine("R6={0};", HardWare.hardware_str.R6);
                }
            }
            if (HardWare.hardware_str.R7 == null)
            {
                file.WriteLine("R7=00;");
            }
            else
            {
                if (HardWare.hardware_str.R7.Length == 1)
                {
                    file.WriteLine("R7={0};", "0" + HardWare.hardware_str.R7);
                }
                else
                {
                    file.WriteLine("R7={0};", HardWare.hardware_str.R7);
                }
            }
            if(HardWare.hardware_str.R8 == null)
            {
                file.WriteLine("R8=01;");
            }
            else
            {
                if (HardWare.hardware_str.R8.Length == 1)
                {
                    file.WriteLine("R8={0};", "0" + HardWare.hardware_str.R8);
                }
                else
                {
                    file.WriteLine("R8={0};", HardWare.hardware_str.R8);
                }
            }
            if(HardWare.hardware_str.R9 == null)
            {
                file.WriteLine("R9=00;");
            }
            else
            {
                if (HardWare.hardware_str.R9.Length == 1)
                {
                    file.WriteLine("R9={0};", "0" + HardWare.hardware_str.R9);
                }
                else
                {
                    file.WriteLine("R9={0};", HardWare.hardware_str.R9);
                }
            }
            if(HardWare.hardware_str.RA == null)
            {
                file.WriteLine("RA=A5;");
            }
            else
            {
                if (HardWare.hardware_str.RA.Length == 1)
                {
                    file.WriteLine("RA={0};", "0" + HardWare.hardware_str.RA);
                }
                else
                {
                    file.WriteLine("RA={0};", HardWare.hardware_str.RA);
                }
            }
            file.WriteLine("RC=00;");
            file.Close();
        }
        private void SaveInitData(ItemRectangle list)
        {
            List<byte> array = new List<byte> { };  
            int offset = 0;
            switch(list.ControlType)
            {
                case PIC_Obj.data_display:
                    switch(list.DataVarInfo.Var_Type)
                    {
                        case 0:
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 8));
                            array.Add((byte)list.DataVarInfo.Initial_Value);
                            break;
                        case 1:
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 24));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 16));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 8));
                            array.Add((byte)list.DataVarInfo.Initial_Value);
                            break;
                        case 2:
                            array.Add((byte)(list.DataVarInfo.Initial_Value));
                            
                            break;
                        case 3:
                            array.Add(0x00);
                            array.Add((byte)list.DataVarInfo.Initial_Value);
                            break;
                        case 4:
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 56));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 48));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 40));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 32));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 24));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 16));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 8));
                            array.Add((byte)list.DataVarInfo.Initial_Value);
                            break;
                        case 5:
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 8));
                            array.Add((byte)list.DataVarInfo.Initial_Value);
                            break;
                        case 6:
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 24));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 16));
                            array.Add((byte)(list.DataVarInfo.Initial_Value >> 8));
                            array.Add((byte)list.DataVarInfo.Initial_Value);
                            break;
                    }
                    offset = list.VP << 1;
                    break;
                case PIC_Obj.icon_display:
                    array.Add((byte)(list.IconVarInformation.InitialValue >> 8));
                    array.Add((byte)list.IconVarInformation.InitialValue);
                    offset = list.VP << 1;
                    break;
                case PIC_Obj.text_dispaly:
                    byte[] bytes = Encoding.GetEncoding("GB2312").GetBytes(list.TextDisplayInformation.initial_value);
                    for (int i = 0; i < bytes.Length; )
                    {
                        if(bytes[i] < 127)
                        {
                            array.Add(bytes[i]);
                            i++;
                        }
                        else
                        {
                            array.Add(bytes[i]);
                            array.Add(bytes[i + 1]);
                            i += 2;
                        }
                    }
                    offset = list.VP << 1;
                    break;
                case PIC_Obj.aniicon_display:
                    array.Add((byte)list.ActionIconInforamtion.InitlizValue);
                    array.Add((byte)(list.ActionIconInforamtion.InitlizValue >> 8));
                    offset = list.VP << 1;
                    break;
                case PIC_Obj.artfont:
                    switch (list.ArtFontInformation.Var_Type)
                    {
                        case 0:
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 8));
                            array.Add((byte)list.ArtFontInformation.Init_Value);
                            break;
                        case 1:
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 24));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 16));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 8));
                            array.Add((byte)list.ArtFontInformation.Init_Value);
                            break;
                        case 2:
                            
                            array.Add((byte)(list.ArtFontInformation.Init_Value));
                            break;
                        case 3:
                            array.Add(0x00);
                            array.Add((byte)list.ArtFontInformation.Init_Value);
                            break;
                        case 4:
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 56));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 48));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 40));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 32));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 24));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 16));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 8));
                            array.Add((byte)list.ArtFontInformation.Init_Value);
                            break;
                        case 5:
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 8));
                            array.Add((byte)list.ArtFontInformation.Init_Value);
                            break;
                        case 6:
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 24));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 16));
                            array.Add((byte)(list.ArtFontInformation.Init_Value >> 8));
                            array.Add((byte)list.ArtFontInformation.Init_Value);
                            break;
                    }
                    offset = list.VP << 1;
                    break;
                case PIC_Obj.slidis:
                    switch (list.SlideDisplayInformation.VP_DATA_Mode)
                    {
                        case 0:
                            array.Add((byte)(list.SlideDisplayInformation.InitVal >> 24));
                            array.Add((byte)(list.SlideDisplayInformation.InitVal >> 16));
                            array.Add((byte)(list.SlideDisplayInformation.InitVal >> 8));
                            array.Add((byte)list.SlideDisplayInformation.InitVal);
                            break;
                        case 1:
                            array.Add((byte)(list.SlideDisplayInformation.InitVal));
                            break;
                        case 2:
                            array.Add(0x00);
                            array.Add((byte)(list.SlideDisplayInformation.InitVal));
                            break;
                    }
                    offset = list.VP << 1;
                    break;
                case PIC_Obj.iconrota:
                    switch (list.IconRotationInformation.VP_Mode)
                    {
                        case 0:
                            array.Add((byte)(list.IconRotationInformation.Init_Value >> 24));
                            array.Add((byte)(list.IconRotationInformation.Init_Value >> 16));
                            array.Add((byte)(list.IconRotationInformation.Init_Value >> 8));
                            array.Add((byte)list.IconRotationInformation.Init_Value);
                            break;
                        case 1:
                            array.Add((byte)(list.IconRotationInformation.Init_Value));
                            break;
                        case 2:
                            array.Add(0x00);
                            array.Add((byte)(list.IconRotationInformation.Init_Value));
                            break;
                    }
                    offset = list.VP << 1;
                    break;
                default:
                    return;
            }
            FileStream filestream = new FileStream(System.Environment.CurrentDirectory + @"\TGUS\TGUS_SET\22_initdata.bin", FileMode.Open, FileAccess.Write);
            filestream.Seek(offset,SeekOrigin.Begin);
            filestream.Write(array.ToArray(),0,array.Count);
            filestream.Close();
        }
        private void SaveTouch(ItemRectangle list)
        {
            List<byte> array = new List<byte> { };
            array.Add((byte)list.presentpage_num);
            array.Add((byte)(list.presentpage_num >> 8));
            array.Add((byte)(list.Rectangle.X));
            array.Add((byte)(list.Rectangle.X >> 8));
            array.Add((byte)list.Rectangle.Y);
            array.Add((byte)(list.Rectangle.Y >> 8));
            array.Add((byte)(list.Rectangle.Width + list.Rectangle.X));
            array.Add((byte)((list.Rectangle.Width + list.Rectangle.X) >> 8));
            array.Add((byte)(list.Rectangle.Height + list.Rectangle.Y));
            array.Add((byte)((list.Rectangle.Height + list.Rectangle.Y) >> 8));
            switch (list.ControlType)
            {
                case PIC_Obj.basictouch:
                    array.Add((byte)(list.BaseTouchInfo.Pic_Next));
                    array.Add((byte)(list.BaseTouchInfo.Pic_Next >> 8));
                    array.Add((byte)(list.BaseTouchInfo.Pic_On));
                    array.Add((byte)(list.BaseTouchInfo.Pic_On >> 8));
                    array.Add((byte)(list.BaseTouchInfo.TP_Code));
                    array.Add((byte)(list.BaseTouchInfo.TP_Code >> 8));
                    break;
                case PIC_Obj.datainput:
                    array.Add((byte)(list.DataInputInformation.Pic_Next));
                    array.Add((byte)(list.DataInputInformation.Pic_Next >> 8));
                    array.Add((byte)(list.DataInputInformation.Pic_On));
                    array.Add((byte)(list.DataInputInformation.Pic_On >> 8));
                    array.Add((byte)(0x00));
                    array.Add((byte)(0xFE));//TPCode
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.VP));
                    array.Add((byte)(list.VP >> 8));
                    array.Add((byte)(list.DataInputInformation.V_Type));
                    array.Add((byte)(list.DataInputInformation.N_Int));
                    array.Add((byte)(list.DataInputInformation.N_Dot));
                    array.Add((byte)(list.DataInputInformation.KeyShowPosition_X));
                    array.Add((byte)(list.DataInputInformation.KeyShowPosition_X >> 8));
                    array.Add((byte)(list.DataInputInformation.KeyShowPosition_Y));
                    array.Add((byte)(list.DataInputInformation.KeyShowPosition_Y >> 8));
                    array.Add((byte)(list.DataInputInformation.COLOR));
                    array.Add((byte)(list.DataInputInformation.COLOR >> 8));
                    array.Add((byte)(list.DataInputInformation.Lib_ID));
                    array.Add((byte)(list.DataInputInformation.Font_Hor));
                    array.Add((byte)(list.DataInputInformation.CurousColor));
                    array.Add((byte)(list.DataInputInformation.Hide_En));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.DataInputInformation.KB_Source));
                    array.Add((byte)(list.DataInputInformation.PIC_KB));
                    array.Add((byte)(list.DataInputInformation.PIC_KB >> 8));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Xs));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Xs >> 8));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Ys));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Ys >> 8));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Xe));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Xe >> 8));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Ye));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Ye >> 8));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Posation_X));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Posation_X >> 8));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Posation_Y));
                    array.Add((byte)(list.DataInputInformation.AREA_KB_Posation_Y >> 8));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.DataInputInformation.Limits_En));
                    array.Add((byte)(list.DataInputInformation.V_min));
                    array.Add((byte)(list.DataInputInformation.V_min >> 8));
                    array.Add((byte)(list.DataInputInformation.V_min >> 16));
                    array.Add((byte)(list.DataInputInformation.V_min >> 24));
                    array.Add((byte)(list.DataInputInformation.V_max));
                    array.Add((byte)(list.DataInputInformation.V_max >> 8));
                    array.Add((byte)(list.DataInputInformation.V_max >> 16));
                    array.Add((byte)(list.DataInputInformation.V_max >> 24));
                    array.Add((byte)(list.DataInputInformation.Return_Set));
                    array.Add((byte)(list.DataInputInformation.Return_VP));
                    array.Add((byte)(list.DataInputInformation.Return_VP >> 8));
                    array.Add((byte)(list.DataInputInformation.Return_DATA));
                    array.Add((byte)(list.DataInputInformation.Return_DATA >> 8));
                    break;
                case PIC_Obj.menu_display:
                    array.Add((byte)(0xFF));
                    array.Add((byte)(0xFF));
                    array.Add((byte)(list.PopupMenuInformation.Pic_On));
                    array.Add((byte)(list.PopupMenuInformation.Pic_On >> 8));
                    array.Add((byte)(0x01));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.VP));
                    array.Add((byte)(list.VP >> 8));
                    array.Add((byte)(list.PopupMenuInformation.VP_Mode));
                    array.Add((byte)(list.PopupMenuInformation.Pic_Menu));
                    array.Add((byte)(list.PopupMenuInformation.Pic_Menu >> 8));
                    array.Add((byte)(list.PopupMenuInformation.AREA_Menu_Xs));
                    array.Add((byte)(list.PopupMenuInformation.AREA_Menu_Xs >> 8));
                    array.Add((byte)(list.PopupMenuInformation.AREA_Menu_Ys));
                    array.Add((byte)(list.PopupMenuInformation.AREA_Menu_Ys >> 8));
                    array.Add((byte)(list.PopupMenuInformation.AREA_Menu_Xe));
                    array.Add((byte)(list.PopupMenuInformation.AREA_Menu_Xe >> 8));
                    array.Add((byte)(list.PopupMenuInformation.AREA_Menu_Ye));
                    array.Add((byte)(list.PopupMenuInformation.AREA_Menu_Ye >> 8));
                    array.Add((byte)(list.PopupMenuInformation.Menu_Position_X));
                    array.Add((byte)(list.PopupMenuInformation.Menu_Position_X >> 8));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.PopupMenuInformation.Menu_Position_Y));
                    array.Add((byte)(list.PopupMenuInformation.Menu_Position_Y >> 8));
                    break;
                case PIC_Obj.increadj:
                    array.Add((byte)(0xFF));
                    array.Add((byte)(0xFF));
                    array.Add((byte)(list.IncreaseAdjInformation.Pic_On));
                    array.Add((byte)(list.IncreaseAdjInformation.Pic_On >> 8));
                    array.Add((byte)(0x02));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.VP));
                    array.Add((byte)(list.VP >> 8));
                    array.Add((byte)(list.IncreaseAdjInformation.VP_Mode));
                    array.Add((byte)(list.IncreaseAdjInformation.Adj_Mode));
                    array.Add((byte)(list.IncreaseAdjInformation.Return_Mode));
                    array.Add((byte)(list.IncreaseAdjInformation.Adj_Step));
                    array.Add((byte)(list.IncreaseAdjInformation.Adj_Step >> 8));
                    array.Add((byte)(list.IncreaseAdjInformation.V_Min));
                    array.Add((byte)(list.IncreaseAdjInformation.V_Min >> 8));
                    array.Add((byte)(list.IncreaseAdjInformation.V_Max));
                    array.Add((byte)(list.IncreaseAdjInformation.V_Max >> 8));
                    array.Add((byte)(list.IncreaseAdjInformation.Key_Mode));
                    break;
                case PIC_Obj.sliadj:
                    array.Add((byte)(0xFF));
                    array.Add((byte)(0xFF));
                    array.Add((byte)(0xFF));
                    array.Add((byte)(0xFF));
                    array.Add((byte)(0x03));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.VP));
                    array.Add((byte)(list.VP >> 8));
                    array.Add((byte)(list.SlideAdjInformation.Adj_Mode));
                    array.Add((byte)(list.Rectangle.X));
                    array.Add((byte)(list.Rectangle.X >> 8));
                    array.Add((byte)(list.Rectangle.Y));
                    array.Add((byte)(list.Rectangle.Y >> 8));
                    array.Add((byte)(list.Rectangle.Width + list.Rectangle.X));
                    array.Add((byte)((list.Rectangle.Width + list.Rectangle.X) >> 8));
                    array.Add((byte)(list.Rectangle.Height + list.Rectangle.Y));
                    array.Add((byte)((list.Rectangle.Height + list.Rectangle.Y) >> 8));
                    array.Add((byte)(list.SlideAdjInformation.V_begin));
                    array.Add((byte)(list.SlideAdjInformation.V_begin >> 8));
                    array.Add((byte)(list.SlideAdjInformation.V_end));
                    array.Add((byte)(list.SlideAdjInformation.V_end >> 8));
                    break;
                case PIC_Obj.keyreturn:
                    array.Add((byte)(list.KeyReturnInformation.Pic_Next));
                    array.Add((byte)(list.KeyReturnInformation.Pic_Next >> 8));
                    array.Add((byte)(list.KeyReturnInformation.Pic_On));
                    array.Add((byte)(list.KeyReturnInformation.Pic_On >> 8));
                    array.Add((byte)(0x05));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.VP));
                    array.Add((byte)(list.VP >> 8));
                    array.Add((byte)(list.KeyReturnInformation.VP_Mode));
                    array.Add((byte)(list.KeyReturnInformation.Key_Code));
                    array.Add((byte)(list.KeyReturnInformation.Key_Code >> 8));
                    array.Add((byte)(list.KeyReturnInformation.Touch_Key_Code));
                    array.Add((byte)(list.KeyReturnInformation.Touch_Key_Code >> 8));
                    array.Add((byte)(list.KeyReturnInformation.Touch_KeyPressing_Code));
                    array.Add((byte)(list.KeyReturnInformation.Touch_KeyPressing_Code >> 8));
                    break;
                case PIC_Obj.GBK:
                    array.Add((byte)(list.GBKInformation.Pic_Next));
                    array.Add((byte)(list.GBKInformation.Pic_Next >> 8));
                    array.Add((byte)(list.GBKInformation.Pic_On));
                    array.Add((byte)(list.GBKInformation.Pic_On >> 8));
                    array.Add((byte)(0x06));
                    array.Add((byte)(list.GBKInformation.IsDataAutoUpLoad));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.VP));
                    array.Add((byte)(list.VP >> 8));
                    array.Add((byte)(list.GBKInformation.VP_Len_Max));
                    array.Add((byte)(list.GBKInformation.Scan_Mode));
                    array.Add((byte)(list.GBKInformation.Lib_GBK1));
                    array.Add((byte)(list.GBKInformation.Lib_GBK2));
                    array.Add((byte)(list.GBKInformation.Font_Scale1));
                    array.Add((byte)(list.GBKInformation.Font_Scale2));
                    array.Add((byte)(list.GBKInformation.Cusor_Color));
                    array.Add((byte)(list.GBKInformation.ColorNum1));
                    array.Add((byte)(list.GBKInformation.ColorNum1 >> 8));
                    array.Add((byte)(list.GBKInformation.ColorNum2));
                    array.Add((byte)(list.GBKInformation.ColorNum2 >> 8));
                    array.Add((byte)(list.GBKInformation.PY_Disp_Mode));
                    array.Add((byte)(list.GBKInformation.Scan_Return_Mode));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.GBKInformation.Scan0_Area_Start_Xs));
                    array.Add((byte)(list.GBKInformation.Scan0_Area_Start_Xs >> 8));
                    array.Add((byte)(list.GBKInformation.Scan0_Area_Start_Ys));
                    array.Add((byte)(list.GBKInformation.Scan0_Area_Start_Ys >> 8));
                    array.Add((byte)(list.GBKInformation.Scan0_Area_End_Xe));
                    array.Add((byte)(list.GBKInformation.Scan0_Area_End_Xe >> 8));
                    array.Add((byte)(list.GBKInformation.Scan0_Area_End_Ye));
                    array.Add((byte)(list.GBKInformation.Scan0_Area_End_Ye >> 8));
                    array.Add((byte)(list.GBKInformation.Scan1_Area_Start_Xs));
                    array.Add((byte)(list.GBKInformation.Scan1_Area_Start_Xs >> 8));
                    array.Add((byte)(list.GBKInformation.Scan1_Area_Start_Ys));
                    array.Add((byte)(list.GBKInformation.Scan1_Area_Start_Ys >> 8));
                    array.Add((byte)(list.GBKInformation.Scan_Dis));
                    array.Add((byte)(0x00));
                    array.Add((byte)(list.GBKInformation.KB_Source));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.GBKInformation.PIC_KB));
                    array.Add((byte)(list.GBKInformation.PIC_KB >> 8));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Xs));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Xs >> 8));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Ys));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Ys >> 8));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Xe));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Xe >> 8));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Ye));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Ye >> 8));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Position_Xs));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Position_Xs >> 8));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Position_Ys));
                    array.Add((byte)(list.GBKInformation.AREA_KB_Position_Ys >> 8));
                    array.Add((byte)(0x02));
                    break;
                case PIC_Obj.ASCII:
                    array.Add((byte)(list.ASCIIInformation.Pic_Next));
                    array.Add((byte)(list.ASCIIInformation.Pic_Next >> 8));
                    array.Add((byte)(list.ASCIIInformation.Pic_On));
                    array.Add((byte)(list.ASCIIInformation.Pic_On >> 8));
                    array.Add((byte)(0x06));
                    array.Add((byte)(list.ASCIIInformation.IsDataAutoUpLoad));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.VP));
                    array.Add((byte)(list.VP >> 8));
                    array.Add((byte)(list.ASCIIInformation.VP_Len_Max));
                    array.Add((byte)(list.ASCIIInformation.Scan_Mode));
                    array.Add((byte)(list.ASCIIInformation.Lib_ID));
                    array.Add((byte)(list.ASCIIInformation.Font_Hor));
                    array.Add((byte)(list.ASCIIInformation.Font_Ver));
                    array.Add((byte)(list.ASCIIInformation.Cusor_Color));
                    array.Add((byte)(list.ASCIIInformation.ColorNum));
                    array.Add((byte)(list.ASCIIInformation.ColorNum >> 8));
                    array.Add((byte)(list.ASCIIInformation.Scan_Area_Start_Xs));
                    array.Add((byte)(list.ASCIIInformation.Scan_Area_Start_Xs >> 8));
                    array.Add((byte)(list.ASCIIInformation.Scan_Area_Start_Ys));
                    array.Add((byte)(list.ASCIIInformation.Scan_Area_Start_Ys >> 8));
                    array.Add((byte)(list.ASCIIInformation.Scan_Return_Mode));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.ASCIIInformation.Scan_Area_End_Xe));
                    array.Add((byte)(list.ASCIIInformation.Scan_Area_End_Xe >> 8));
                    array.Add((byte)(list.ASCIIInformation.Scan_Area_End_Ye));
                    array.Add((byte)(list.ASCIIInformation.Scan_Area_End_Ye >> 8));
                    array.Add((byte)(list.ASCIIInformation.KB_Source));
                    array.Add((byte)(list.ASCIIInformation.PIC_KB));
                    array.Add((byte)(list.ASCIIInformation.PIC_KB >> 8));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Xs));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Xs >> 8));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Ys));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Ys >> 8));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Xe));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Xe >> 8));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Ye));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Ye >> 8));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Position_Xs));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Position_Xs >> 8));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Position_Ys));
                    array.Add((byte)(list.ASCIIInformation.AREA_KB_Position_Ys >> 8));
                    array.Add((byte)(list.ASCIIInformation.DISPLAY_EN));
                    break;
                case PIC_Obj.TouchState:
                    array.Add((byte)(list.TouchStateInformation.Pic_Next));
                    array.Add((byte)(list.TouchStateInformation.Pic_Next >> 8));
                    array.Add((byte)(list.TouchStateInformation.Pic_On));
                    array.Add((byte)(list.TouchStateInformation.Pic_On >> 8));
                    array.Add((byte)(0x08));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.TouchStateInformation.TP_ON_Mode));
                    array.Add((byte)(list.TouchStateInformation.VP1S));
                    array.Add((byte)(list.TouchStateInformation.VP1S >> 8));
                    array.Add((byte)(list.TouchStateInformation.VP1T));
                    array.Add((byte)(list.TouchStateInformation.VP1T >> 8));
                    array.Add((byte)(0x00));
                    array.Add((byte)(list.TouchStateInformation.LEN1));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.TouchStateInformation.TP_ON_Continue_Mode));
                    array.Add((byte)(list.TouchStateInformation.VP2S));
                    array.Add((byte)(list.TouchStateInformation.VP2S >> 8));
                    array.Add((byte)(list.TouchStateInformation.VP2T));
                    array.Add((byte)(list.TouchStateInformation.VP2T >> 8));
                    array.Add((byte)(0x00));
                    array.Add((byte)(list.TouchStateInformation.LEN2));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.TouchStateInformation.TP_OFF_Mode));
                    array.Add((byte)(list.TouchStateInformation.VP3S));
                    array.Add((byte)(list.TouchStateInformation.VP3S >> 8));
                    array.Add((byte)(list.TouchStateInformation.VP3T));
                    array.Add((byte)(list.TouchStateInformation.VP3T >> 8));
                    array.Add((byte)(0x00));
                    array.Add((byte)(list.TouchStateInformation.LEN3));
                    break;
                case PIC_Obj.RTC_Set:
                    array.Add((byte)(0x00));
                    array.Add((byte)(0xFF));
                    array.Add((byte)(list.RTCsetInformation.Pic_On));
                    array.Add((byte)(list.RTCsetInformation.Pic_On >> 8));
                    array.Add((byte)(list.RTCsetInformation.TP_Code));
                    array.Add((byte)(list.RTCsetInformation.TP_Code >> 8));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(0x00));
                    array.Add((byte)(0x00));
                    array.Add((byte)(0x00));
                    array.Add((byte)(list.RTCsetInformation.DisplayPoint_X));
                    array.Add((byte)(list.RTCsetInformation.DisplayPoint_X >> 8));
                    array.Add((byte)(list.RTCsetInformation.DisplayPoint_Y));
                    array.Add((byte)(list.RTCsetInformation.DisplayPoint_Y >> 8));
                    array.Add((byte)(list.RTCsetInformation.ColorNum));
                    array.Add((byte)(list.RTCsetInformation.ColorNum >> 8));
                    array.Add((byte)(list.RTCsetInformation.Lib_ID));
                    array.Add((byte)(list.RTCsetInformation.Font_Hor));
                    array.Add((byte)(list.RTCsetInformation.Cusor_Color));
                    array.Add((byte)(list.RTCsetInformation.KB_Source));
                    array.Add((byte)(list.RTCsetInformation.PIC_KB));
                    array.Add((byte)(list.RTCsetInformation.PIC_KB >> 8));
                    array.Add((byte)(0xFE));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Xs));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Xs >> 8));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Ys));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Ys >> 8));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Xe));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Xe >> 8));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Ye));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Ye >> 8));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Position_Xs));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Position_Xs >> 8));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Position_Ys));
                    array.Add((byte)(list.RTCsetInformation.AREA_KB_Position_Ys >> 8));
                    break;
                default:
                    return;
            }
            if((array.Count % 16) != 0)
            {
                int temp = (array.Count % 16);
                for(int i = temp; i < 16; i ++)
                {
                    array.Add(0);
                }
            }
            FileStream touchstream = new FileStream(System.Environment.CurrentDirectory + @"\TGUS\TGUS_SET\13_touch.bin", FileMode.Append, FileAccess.Write);
            touchstream.Write(array.ToArray(), 0, array.Count);
            touchstream.Close();
        }
        private void SaveVariable(ItemRectangle list)
        {
            byte[] array = new byte[32];
            array[1] = (byte)(0x5A);
            switch (list.ControlType)
            {
                case PIC_Obj.icon_display:
                    array[0] = (byte)(0x00);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x08);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(list.VP);
                    array[7] = (byte)(list.VP >> 8);
                    array[8] = (byte)(list.Rectangle.X);
                    array[9] = (byte)(list.Rectangle.X >> 8);
                    array[10] = (byte)(list.Rectangle.Y);
                    array[11] = (byte)(list.Rectangle.Y >> 8);
                    array[12] = (byte)(list.IconVarInformation.V_Min);
                    array[13] = (byte)(list.IconVarInformation.V_Min >> 8);
                    array[14] = (byte)(list.IconVarInformation.V_Max);
                    array[15] = (byte)(list.IconVarInformation.V_Max >> 8);
                    array[16] = (byte)(list.IconVarInformation.Icon_Min);
                    array[17] = (byte)(list.IconVarInformation.Icon_Min >> 8);
                    array[18] = (byte)(list.IconVarInformation.Icon_Max);
                    array[19] = (byte)(list.IconVarInformation.Icon_Max >> 8);
                    array[20] = (byte)(list.IconVarInformation.Icon_Lib);
                    array[21] = (byte)(list.IconVarInformation.Mode);
                    break;
                case PIC_Obj.aniicon_display:
                    array[0] = (byte)(0x01);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x0A);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(list.VP);
                    array[7] = (byte)(list.VP >> 8);
                    array[8] = (byte)(list.Rectangle.X);
                    array[9] = (byte)(list.Rectangle.X >> 8);
                    array[10] = (byte)(list.Rectangle.Y);
                    array[11] = (byte)(list.Rectangle.Y >> 8);
                    array[12] = (byte)(list.ActionIconInforamtion.Reset_Icon_En);
                    array[13] = (byte)(list.ActionIconInforamtion.Reset_Icon_En >> 8);
                    array[14] = (byte)(list.ActionIconInforamtion.V_Stop);
                    array[15] = (byte)(list.ActionIconInforamtion.V_Stop >> 8);
                    array[16] = (byte)(list.ActionIconInforamtion.V_Start);
                    array[17] = (byte)(list.ActionIconInforamtion.V_Start >> 8);
                    array[18] = (byte)(list.ActionIconInforamtion.Icon_Stop);
                    array[19] = (byte)(list.ActionIconInforamtion.Icon_Stop >> 8);
                    array[20] = (byte)(list.ActionIconInforamtion.Icon_Start);
                    array[21] = (byte)(list.ActionIconInforamtion.Icon_Start >> 8);
                    array[22] = (byte)(list.ActionIconInforamtion.Icon_End);
                    array[23] = (byte)(list.ActionIconInforamtion.Icon_End >> 8);
                    array[24] = (byte)(list.ActionIconInforamtion.Icon_Lib);
                    array[25] = (byte)(list.ActionIconInforamtion.Mode);
                    break;
                case PIC_Obj.slidis:
                    array[0] = (byte)(0x02);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x0A);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(list.VP);
                    array[7] = (byte)(list.VP >> 8);
                    array[8] = (byte)(list.SlideDisplayInformation.V_begain);
                    array[9] = (byte)(list.SlideDisplayInformation.V_begain >> 8);
                    array[10] = (byte)(list.SlideDisplayInformation.V_end);
                    array[11] = (byte)(list.SlideDisplayInformation.V_end >> 8);
                    array[12] = (byte)(list.SlideDisplayInformation.X_begain);
                    array[13] = (byte)(list.SlideDisplayInformation.X_begain >> 8);
                    array[14] = (byte)(list.SlideDisplayInformation.X_end);
                    array[15] = (byte)(list.SlideDisplayInformation.X_end >> 8);
                    array[16] = (byte)(list.SlideDisplayInformation.Icon_ID);
                    array[17] = (byte)(list.SlideDisplayInformation.Icon_ID >> 8);
                    array[18] = (byte)(list.Rectangle.Y);
                    array[19] = (byte)(list.Rectangle.Y >> 8);
                    array[20] = (byte)(list.SlideDisplayInformation.X_adj);
                    array[21] = (byte)(list.SlideDisplayInformation.Mode);
                    array[22] = (byte)(list.SlideDisplayInformation.Icon_Lib);
                    array[23] = (byte)(list.SlideDisplayInformation.Icon_Mode);
                    array[24] = (byte)(list.SlideDisplayInformation.VP_DATA_Mode);
                    break;
                case PIC_Obj.artfont:
                    array[0] = (byte)(0x03);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x07);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(list.VP);
                    array[7] = (byte)(list.VP >> 8);
                    array[8] = (byte)(list.Rectangle.X);
                    array[9] = (byte)(list.Rectangle.X >> 8);
                    array[10] = (byte)(list.Rectangle.Y);
                    array[11] = (byte)(list.Rectangle.Y >> 8);
                    array[12] = (byte)(list.ArtFontInformation.Icon_0);
                    array[13] = (byte)(list.ArtFontInformation.Icon_0 >> 8);
                    array[14] = (byte)(list.ArtFontInformation.Icon_Lib);
                    array[15] = (byte)(list.ArtFontInformation.Icon_Mode);
                    array[16] = (byte)(list.ArtFontInformation.Integer_Length);
                    array[17] = (byte)(list.ArtFontInformation.Decimal_Length);
                    array[18] = (byte)(list.ArtFontInformation.Var_Type);
                    array[19] = (byte)(list.ArtFontInformation.Align_Mode);
                    break;
                case PIC_Obj.iconrota:
                    array[0] = (byte)(0x05);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x0C);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(list.VP);
                    array[7] = (byte)(list.VP >> 8);
                    array[8] = (byte)(list.IconRotationInformation.Icon_ID);
                    array[9] = (byte)(list.IconRotationInformation.Icon_ID >> 8);
                    array[10] = (byte)(list.IconRotationInformation.Icon_Xc);
                    array[11] = (byte)(list.IconRotationInformation.Icon_Xc >> 8);
                    array[12] = (byte)(list.IconRotationInformation.Icon_Yc);
                    array[13] = (byte)(list.IconRotationInformation.Icon_Yc >> 8);
                    array[14] = (byte)(list.IconRotationInformation.Xc);
                    array[15] = (byte)(list.IconRotationInformation.Xc >> 8);
                    array[16] = (byte)(list.IconRotationInformation.Yc);
                    array[17] = (byte)(list.IconRotationInformation.Yc >> 8);
                    array[18] = (byte)(list.IconRotationInformation.V_begain);
                    array[19] = (byte)(list.IconRotationInformation.V_begain >> 8);
                    array[20] = (byte)(list.IconRotationInformation.V_end);
                    array[21] = (byte)(list.IconRotationInformation.V_end >> 8);
                    array[22] = (byte)(list.IconRotationInformation.AL_begain);
                    array[23] = (byte)(list.IconRotationInformation.AL_begain >> 8);
                    array[24] = (byte)(list.IconRotationInformation.AL_end);
                    array[25] = (byte)(list.IconRotationInformation.AL_end >> 8);
                    array[26] = (byte)(list.IconRotationInformation.VP_Mode);
                    array[27] = (byte)(list.IconRotationInformation.Lib_ID);
                    array[28] = (byte)(list.IconRotationInformation.Mode);
                    break;
                case PIC_Obj.data_display:
                    array[0] = (byte)(0x10);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x0D);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(list.VP);
                    array[7] = (byte)(list.VP >> 8);
                    array[8] = (byte)(list.Rectangle.X);
                    array[9] = (byte)(list.Rectangle.X >> 8);
                    array[10] = (byte)(list.Rectangle.Y);
                    array[11] = (byte)(list.Rectangle.Y >> 8);
                    array[12] = (byte)(list.DataVarInfo.COLOR);
                    array[13] = (byte)(list.DataVarInfo.COLOR >> 8);
                    array[14] = (byte)(list.DataVarInfo.Lib_ID);
                    array[15] = (byte)(list.DataVarInfo.Font_Size);
                    array[16] = (byte)(list.DataVarInfo.Font_Align);
                    array[17] = (byte)(list.DataVarInfo.Integer_Length);
                    array[18] = (byte)(list.DataVarInfo.Decimal_Length);
                    array[19] = (byte)(list.DataVarInfo.Var_Type);
                    array[20] = (byte)(list.DataVarInfo.Len_unit);
                    try
                    {
                        for (int length = 0; length < list.DataVarInfo.String_Uint.Length; length++)
                        {
                            array[21 + length] = Convert.ToByte(list.DataVarInfo.String_Uint[list.DataVarInfo.String_Uint.Length - length - 1]);
                        }
                    }
                    catch { };
                      
                    break;
                case PIC_Obj.rtc_display:
                    array[0] = (byte)(0x12);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x0D);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(0x00);
                    array[7] = (byte)(0x00);
                    array[8] = (byte)(list.Rectangle.X);
                    array[9] = (byte)(list.Rectangle.X >> 8);
                    array[10] = (byte)(list.Rectangle.Y);
                    array[11] = (byte)(list.Rectangle.Y >> 8);
                    array[12] = (byte)(list.RTCDisplayInformatin.COLOR);
                    array[13] = (byte)(list.RTCDisplayInformatin.COLOR >> 8);
                    array[14] = (byte)(list.RTCDisplayInformatin.Lib_ID);
                    array[15] = (byte)(list.RTCDisplayInformatin.Font_X_Dots);
                    try
                    {
                        for (int length = 0; length < list.RTCDisplayInformatin.String_Code.Length; length++)
                        {
                            array[16 + length] = Convert.ToByte(list.RTCDisplayInformatin.String_Code[length]);
                        }
                    }
                    catch { };
                    break;
                case PIC_Obj.QR_display:
                    array[0] = (byte)(0x25);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x04);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(list.VP);
                    array[7] = (byte)(list.VP >> 8);
                    array[8] = (byte)(list.Rectangle.X);
                    array[9] = (byte)(list.Rectangle.X >> 8);
                    array[10] =(byte)(list.Rectangle.Y);
                    array[11] =(byte)(list.Rectangle.Y >> 8);
                    array[12] = (byte)(list.QRCodeInformation.Unit_Pixels); 
                    array[13] = (byte)(list.QRCodeInformation.Unit_Pixels >> 8); 
                    break;
                case PIC_Obj.clockdisplay:
                    array[0] = (byte)(0x12);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x0D);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(0x01);
                    array[7] = (byte)(0x00);
                    array[8] = (byte)(list.Rectangle.X);
                    array[9] = (byte)(list.Rectangle.X >> 8);
                    array[10] = (byte)(list.Rectangle.Y);
                    array[11] = (byte)(list.Rectangle.Y >> 8);
                    array[12] = (byte)(list.ClockDisplayInformation.Icon_Hour);
                    array[13] = (byte)(list.ClockDisplayInformation.Icon_Hour >> 8);
                    array[14] = (byte)(list.ClockDisplayInformation.Icon_Hour_Central_X);
                    array[15] = (byte)(list.ClockDisplayInformation.Icon_Hour_Central_X >> 8);
                    array[16] = (byte)(list.ClockDisplayInformation.Icon_Hour_Central_Y);
                    array[17] = (byte)(list.ClockDisplayInformation.Icon_Hour_Central_Y >> 8);
                    array[18] = (byte)(list.ClockDisplayInformation.Icon_Minute);
                    array[19] = (byte)(list.ClockDisplayInformation.Icon_Minute >> 8);
                    array[20] = (byte)(list.ClockDisplayInformation.Icon_Minute_Central_X);
                    array[21] = (byte)(list.ClockDisplayInformation.Icon_Minute_Central_X >> 8);
                    array[22] = (byte)(list.ClockDisplayInformation.Icon_Minute_Central_Y);
                    array[23] = (byte)(list.ClockDisplayInformation.Icon_Minute_Central_Y >> 8);
                    array[24] = (byte)(list.ClockDisplayInformation.Icon_Second);
                    array[25] = (byte)(list.ClockDisplayInformation.Icon_Second >> 8);
                    array[26] = (byte)(list.ClockDisplayInformation.Icon_Second_Central_X);
                    array[27] = (byte)(list.ClockDisplayInformation.Icon_Second_Central_X >> 8);
                    array[28] = (byte)(list.ClockDisplayInformation.Icon_Second_Central_Y);
                    array[29] = (byte)(list.ClockDisplayInformation.Icon_Second_Central_Y >> 8);
                    array[30] = (byte)(list.ClockDisplayInformation.Icon_Lib);
                    break;
                case PIC_Obj.text_dispaly:
                    array[0] = (byte)(0x11);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x0D);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(list.VP);
                    array[7] = (byte)(list.VP >> 8);
                    array[8] = (byte)(list.Rectangle.X);
                    array[9] = (byte)(list.Rectangle.X >> 8);
                    array[10] = (byte)(list.Rectangle.Y);
                    array[11] = (byte)(list.Rectangle.Y >> 8);
                    array[12] = (byte)(list.TextDisplayInformation.COLOR);
                    array[13] = (byte)(list.TextDisplayInformation.COLOR >> 8);
                    array[14] = (byte)(list.Rectangle.X);
                    array[15] = (byte)(list.Rectangle.X >> 8);
                    array[16] = (byte)(list.Rectangle.Y);
                    array[17] = (byte)(list.Rectangle.Y >> 8);
                    array[18] = (byte)(list.Rectangle.X + list.Rectangle.Width);
                    array[19] = (byte)((list.Rectangle.X + list.Rectangle.Width) >> 8);
                    array[20] = (byte)(list.Rectangle.Y + list.Rectangle.Height);
                    array[21] = (byte)((list.Rectangle.Y + list.Rectangle.Height)>> 8);
                    array[22] = (byte)(list.TextDisplayInformation.Text_length);
                    array[23] = (byte)(list.TextDisplayInformation.Text_length >> 8);
                    array[24] = (byte)(list.TextDisplayInformation.Font0_ID);
                    array[25] = (byte)(list.TextDisplayInformation.Font1_ID);
                    array[26] = (byte)(list.TextDisplayInformation.Font_X_Dots);
                    array[27] = (byte)(list.TextDisplayInformation.Font_Y_Dots);
                    array[28] = (byte)(list.TextDisplayInformation.Encode_Mode);
                    array[29] = (byte)(list.TextDisplayInformation.HOR_Dis);
                    array[30] = (byte)(list.TextDisplayInformation.VER_Dis);
                    break;
                case PIC_Obj.BasicGra:
                    array[0] = (byte)(0x21);
                    array[2] = (byte)(list.SP);
                    array[3] = (byte)(list.SP >> 8);
                    array[4] = (byte)(0x08);
                    array[5] = (byte)(0x00);
                    array[6] = (byte)(list.VP);
                    array[7] = (byte)(list.VP >> 8);
                    array[8] = (byte)(list.Rectangle.X);
                    array[9] = (byte)(list.Rectangle.X >> 8);
                    array[10] = (byte)(list.Rectangle.Y);
                    array[11] = (byte)(list.Rectangle.Y >> 8);
                    array[12] = (byte)(list.Rectangle.X + list.Rectangle.Width);
                    array[13] = (byte)((list.Rectangle.X + list.Rectangle.Width)>> 8);
                    array[14] = (byte)(list.Rectangle.Y + list.Rectangle.Height);
                    array[15] = (byte)((list.Rectangle.Y + list.Rectangle.Height)>> 8);
                    array[16] = (byte)list.BasicGraInformation.Dashed_Line_En;
                    array[17] = (byte)list.BasicGraInformation.Dash_Set_1;
                    array[18] = (byte)list.BasicGraInformation.Dash_Set_2;
                    array[19] = (byte)list.BasicGraInformation.Dash_Set_3;
                    array[20] = (byte)list.BasicGraInformation.Dash_Set_4;
                    break;
                default:
                    return;
            }
            int offset = 0;
            if(list.touchvar == ItemBase.TouchOrVar.varable)
            {
                offset = 2 * 1024 * list.presentpage_num + (Num[list.presentpage_num] - Num2[list.presentpage_num]) * 32;
                Num2[list.presentpage_num]--;
            }
            FileStream filestream = new FileStream(System.Environment.CurrentDirectory + @"\TGUS\TGUS_SET\14_variable.bin", FileMode.Open, FileAccess.Write);
            filestream.Seek(offset, SeekOrigin.Begin);
            filestream.Write(array, 0, 32);
            filestream.Close();
        }

        private void But_ExportInformation_Click(object sender, EventArgs e)
        {

            if (setstart_flag == false)
                return;
            td = new Thread(new ThreadStart(ExportAddressInformation));
            td.Start();
        }

        private void ExportAddressInformation()
        {
            string P_str_path = System.Environment.CurrentDirectory + @"\TGUS";
            //创建Excel对象
            Microsoft.Office.Interop.Excel.Application excel = new Microsoft.Office.Interop.Excel.Application();//实例化Excel对象
            excel.Visible = false;
            Microsoft.Office.Interop.Excel.Workbook newWorkBook;
            Microsoft.Office.Interop.Excel.Worksheet newWorksheet;
            object missing = System.Reflection.Missing.Value;   //获取缺少的object类型值
            //添加新的工作薄

            if (File.Exists(P_str_path + "\\" + Project_name.Substring(0, Project_name.LastIndexOf(".")) + ".xls"))
            {
                newWorkBook = excel.Application.Workbooks.Open(P_str_path + "\\" + Project_name.Substring(0, Project_name.LastIndexOf(".")) + ".xls",
                missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing, missing);
            }
            else
            {
                newWorkBook = excel.Application.Workbooks.Add(true);
            }
            newWorksheet = (Worksheet)newWorkBook.Worksheets[1];
            Display dis = Display.GetSingle();
            newWorksheet.Cells[1, 1] = "Name";
            newWorksheet.Cells[1, 2] = "Address";
            string add;
            int count = 0;
            for (int i = 0; i < dis.designer1.Items.Count; i++)
            {
                add = string.Empty;
                switch (dis.designer1.Items[i].ControlType)
                {
                    case PIC_Obj.data_display:
                    case PIC_Obj.icon_display:
                    case PIC_Obj.text_dispaly:
                    case PIC_Obj.datainput:
                    case PIC_Obj.keyreturn:
                    case PIC_Obj.menu_display:
                    case PIC_Obj.aniicon_display:
                    case PIC_Obj.sliadj:
                    case PIC_Obj.artfont:
                    case PIC_Obj.slidis:
                    case PIC_Obj.iconrota:
                    case PIC_Obj.BasicGra:
                        add = dis.designer1.Items[i].VP.ToString("X4");
                        break;
                    default:
                        break;
                }
                if (add != string.Empty)
                {
                    newWorksheet.Cells[count + 2, 1] = dis.designer1.Items[i].Name_define;
                    newWorksheet.Cells[count + 2, 2] = add;
                    count++;
                }
            }
            try
            {
                newWorkBook.SaveAs(P_str_path + "\\" + Project_name.Substring(0, Project_name.LastIndexOf(".")) + ".xls",
                    missing, missing, missing, missing, missing, Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlShared, 1, true, missing, missing);
                newWorkBook.Close(false, Project_name.Substring(0, Project_name.LastIndexOf(".")) + ".xls", true);
                if(Main_Form.LanguageType == "English")
                {
                    MessageBox.Show("Successfully import into the Excel worksheet!", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);//弹出提示信息
                }
                else
                {
                    MessageBox.Show("成功导入Excel工作表中！", "Prompt", MessageBoxButtons.OK, MessageBoxIcon.Information);//弹出提示信息
                }
                
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            if (td != null)
            {
                if (td.ThreadState == ThreadState.Running)
                {
                    SaveButDel sdel = new SaveButDel(But_Save_enable);
                    this.BeginInvoke(sdel, true);
                    td.Abort();//关闭线程
                }
            }
        }
        private void But_Language_Click(object sender, EventArgs e)
        {
            Language_Set language = Language_Set.GetSingle();
            language.ShowDialog();
        }
        private void notifyIcon1_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == System.Windows.Forms.MouseButtons.Left)
            {
                if(this.WindowState == FormWindowState.Minimized)
                {
                    this.WindowState = FormWindowState.Normal;
                }
                else if(this.WindowState == FormWindowState.Maximized || this.WindowState == FormWindowState.Normal)
                {
                    this.WindowState = FormWindowState.Minimized;
                }
            }
            else if(e.Button == System.Windows.Forms.MouseButtons.Right)
            {
                myMenu.Show();
            }
        }
        private void 退出ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }

        private void But_ZoomIn_Click(object sender, EventArgs e)
        {
            if (Displayscale.SelectedIndex < (Displayscale.Items.Count - 1))
            {
                Displayscale.SelectedIndex++;
            }
        }

        private void But_ZoomOut_Click(object sender, EventArgs e)
        {
           if (Displayscale.SelectedIndex > 0)
           {
               Displayscale.SelectedIndex--;
           }
        }

        private void Displayscale_SelectedIndexChanged(object sender, EventArgs e)
        {
            DisplayscaleNumSelect = Displayscale.SelectedIndex;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            Display d = Display.GetSingle();
            foreach (ItemRectangle list in d.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    var sel = list;
                    d.designer1.Items.Remove(list);
                    d.designer1.Items.Insert(0, sel);
                    break;
                }
            }
        }

        private void toolStripButton7_Click(object sender, EventArgs e)
        {
            Display d = Display.GetSingle();
            foreach (ItemRectangle list in d.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    var sel = list;
                    d.designer1.Items.Remove(list);
                    d.designer1.Items.Insert(d.designer1.Items.Count, sel);
                    break;
                }
            }
        }

        private void Main_Form_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(setstart_flag == true)
            {
                if (e.CloseReason == CloseReason.UserClosing)
                {
                    DialogResult result = MessageBox.Show("Make sure the project has been saved", "Notice", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    if (result != System.Windows.Forms.DialogResult.OK)
                    {
                        
                        e.Cancel = true;
                    }
                }
            }
            try
            {
                myIcon.Visible = false;
                myIcon.Dispose();
            }
            catch { };
        }
        List<ItemRectangle> newlist = new List<ItemRectangle>();
        private void But_replicate_Click(object sender, EventArgs e)
        {
            Display d = Display.GetSingle();
            newlist.Clear();
            foreach (ItemRectangle list in d.designer1.Items)
            {
                if(list.visibility == true && list.Selected == true)
                {
                    ItemRectangle copylist = new ItemRectangle();
                    copylist = list.Clone() as ItemRectangle;
                    copylist.Selected = false;
                    newlist.Add(copylist);
                }
            }
        }

        private void But_Help_Click(object sender, EventArgs e)
        {
            Help h = new Help();
            h.Show();
        }

        private void But_Plaster_Click(object sender, EventArgs e)
        {
            if(newlist.Count != 0)
            {
                Display d = Display.GetSingle();
                for(int i = 0; i < newlist.Count; i ++)
                {
                    ItemRectangle CopyList = new ItemRectangle();
                    CopyList = newlist[i].Clone() as ItemRectangle; ;
                    CopyList.presentpage_num = presentpage_num;
                    d.designer1.Items.Add(CopyList);
                }
                d.designer1.Refresh();
            }
        }  
    }
}
/*    
                   _ooOoo_
                  o8888888o
                  88" . "88
                  (| -_- |)
                  O\  =  /O
               ____/`---'\____
             .'  \\|     |//  `.
            /  \\|||  :  |||//  \
           /  _||||| -:- |||||-  \
           |   | \\\  -  /// |   |
           | \_|  ''\---/''  |   |
           \  .-\__  `-`  ___/-. /
         ___`. .'  /--.--\  `. . __
      ."" '<  `.___\_<|>_/___.'  >'"".
     | | :  ` - `.;`\ _ /`;.`/ - ` : | |
     \  \ `-.   \_ __\ /__ _/   .-` /  /
======`-.____`-.___\_____/___.-`____.-'======
                   `=---='
^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^^
            佛祖保佑       永无BUG
 */








