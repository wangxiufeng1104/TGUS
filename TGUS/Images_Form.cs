using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TGUS
{
    /// <summary>
    /// 这个类是对工程图片的操作
    /// </summary>
    public partial class Images_Form : WeifenLuo.WinFormsUI.Docking.DockContent
    {
         public struct PICINFORMATION   ///存储图片信息的结构体
        {
            public Image image;                        ///图片类，用于获取图片的各种信息
            public string name;                        ///图片的名字，包含路径信息
            public string safename;                    ///图片的名字，不包含路径信息
            public int num;                            ///图片的编号
        }
        public string StrRename_bmp = "";
        public static int picture_count;       ///获取已有的行数
        public static int Pic_Number = 0;              ///已经添加进工程的图片数
        public static PICINFORMATION[] picname = new PICINFORMATION[Main_Form.Max_Page];///存储图片信息的结构体数组    
        private  Image Currentimage = null;     ///PBox_Main  当前图片
        Main_Form main_form = Main_Form.GetSingle();

        public static Images_Form ImagesFormSingle = null;
        public static Images_Form GetSingle()
        {
            if(ImagesFormSingle == null)
            {
                ImagesFormSingle = new Images_Form();
            }
            return ImagesFormSingle;
        }
        private Images_Form( )
        {
            InitializeComponent();
        }
        private void Images_Load(object sender, EventArgs e)
        {
            //main_form.Set_fImages_Form(this);
          
            DGV_PictureList.RowCount = 0;   ///这里设置0的时候对TabCotrol显示产生影响，原因不明7.3 6：05
            ///如果 AllowUserToAddRows 是 true，则您不能将 RowCount 设置为 0。    MSDN上对DataGridView.RowCount 属性 
            ///的描述，解决问题，7.3 23:36
            DGV_PictureList.EnableHeadersVisualStyles = false;///启动了可视样式的时候，BackColor和ForeColor的值会被忽略
            DGV_PictureList.ColumnHeadersDefaultCellStyle.BackColor = Color.SteelBlue;
            DGV_PictureList.Font = new System.Drawing.Font("UTF-8", 8);  ///设置字体
            DGV_PictureList.ColumnHeadersDefaultCellStyle.Alignment = DataGridViewContentAlignment.MiddleCenter;///行居中
            DGV_PictureList.AlternatingRowsDefaultCellStyle.BackColor = Color.SteelBlue;                           ///奇偶行显示颜色不同
            
        }
        private void But_AddPic_Click(object sender, EventArgs e)
        {
            if (Pic_Number >= (Main_Form.Max_Page + 1))
            {
                ShowMessage();
                return;
            }
            if (null == Main_Form.PicSavePath)
                this.openImagesDialog1.InitialDirectory = System.Environment.CurrentDirectory + "\\TGUS";
            else
                this.openImagesDialog1.InitialDirectory = Main_Form.PicSavePath;
            this.openImagesDialog1.Filter = "bmp文件(*.bmp)|*.bmp";///过滤需要的文件类型
            this.openImagesDialog1.AddExtension = true;
            this.openImagesDialog1.FileName = "";
            if (this.openImagesDialog1.ShowDialog() == DialogResult.OK)
            {
                Main_Form.PicSavePath = System.IO.Path.GetDirectoryName(this.openImagesDialog1.FileName);
                Application.UserAppDataRegistry.SetValue("PicSavePath", Main_Form.PicSavePath);    ///注册表中写当前的图片选取的路径
                int i = 0;
                string[] StrFileNames = new string[Main_Form.Max_Page];
                string[] StrFileNames1 = new string[Main_Form.Max_Page + 1];
                ///拷贝图片
                foreach (string s in this.openImagesDialog1.FileNames)
                {
                    Pic_Number++;
                    if (Pic_Number >= Main_Form.Max_Page + 1)
                    {
                        ShowMessage();
                        break;     ///跳出foreach循环
                    }
                    if (!(s.Contains(System.Environment.CurrentDirectory + "\\TGUS" + "image" + "\\")))
                    {
                        try
                        {
                            File.Copy(s, System.Environment.CurrentDirectory + "\\TGUS" + "\\image" + "\\" + this.openImagesDialog1.SafeFileNames[i]);
                            StrFileNames[i] = this.openImagesDialog1.SafeFileNames[i];
                            //imageList1.Images.Add(Image.FromFile(System.Environment.CurrentDirectory + "\\TGUS" + "\\image" + "\\" + StrRename_bmp));
                        }
                        catch (IOException)
                        {
                            ///MessageBox.Show("目标文件已存在项目image文件夹中，请修改文件名后重试 文件名：" + openFileDialog1.SafeFileNames[i]);
                            string strfile = this.openImagesDialog1.SafeFileNames[i];
target1:                    string str = "目标文件已存在项目image文件夹中，请修改文件名后重试 文件名：" + strfile;
                            if (Main_Form.LanguageType == "English")
                            {
                                str = "The destination file already exists in the project image folder. Please change the file name and try again ,file name:" + strfile;
                            }
                            
                            RenamePicture Form_Rename = new RenamePicture(this, str);
                            ///在这里循环检查错误
                            this.Cursor = System.Windows.Forms.Cursors.WaitCursor;
                            if (Form_Rename.ShowDialog() == DialogResult.OK)
                            {

                                if (StrRename_bmp.Length <= 4)    ///名称小于4肯定没有添加后缀或者只有后缀
                                {
                                    StrRename_bmp += ".bmp";
                                }
                                else if (StrRename_bmp.Substring(StrRename_bmp.Length - 4) != (".bmp"))
                                {
                                    StrRename_bmp += ".bmp";
                                }
                                try
                                {
                                    File.Copy(s, System.Environment.CurrentDirectory + "\\TGUS" + "\\image" + "\\" + StrRename_bmp);
                                    //imageList1.Images.Add(Image.FromFile(System.Environment.CurrentDirectory + "\\TGUS" + "\\image" + "\\" + StrRename_bmp));
                                }
                                catch (IOException)
                                {
                                    strfile = StrRename_bmp;
                                    goto target1;
                                }
                                StrFileNames[i] = StrRename_bmp;
                            }
                            else
                            {
                                StrFileNames[i] = "";
                            }
                            this.Cursor = System.Windows.Forms.Cursors.Default;
                        }
                        i++;
                    }
                    else
                    {

                    }
                }
                int k = 0;
                ///筛选掉空的文件名
                foreach (string s in StrFileNames)
                {
                    if (i > 0)
                    {
                        if (s != "")
                        {
                            //Console.WriteLine(s);

                            StrFileNames1[k] = s;
                            //Console.WriteLine("StrFileName1:{0}", StrFileNames1[k]);
                            k++;
                        }
                    }
                    i--;
                }
                if (k != 0)
                {
                    picture_count = DGV_PictureList.Rows.Count;   ///先获取当前已经有的行数
                    ///int Add_Picture_Number = StrFileNames1.Length;   ///本次打开的文件的数量 
                    //Console.WriteLine("k = {0}", k);
                    DGV_PictureList.Rows.Add(k);                ///添加行数
                    //Console.WriteLine("已经有的行数：{0}", picture_count);
                    //Console.WriteLine("打开文件的数量{0}", k);
                    k = 0;
                    foreach (string s in this.openImagesDialog1.FileNames)
                    {
                        //Console.WriteLine("Bmp name:{0}",StrFileNames1[k]);
                        if (StrFileNames1[k] != null)     ///最后一个文件名为空
                        {
                            Images_Form.picname[picture_count + k].name = System.Environment.CurrentDirectory + "\\TGUS" + "\\image" + "\\" + StrFileNames1[k];
                            Images_Form.picname[picture_count + k].safename = StrFileNames1[k];
                            Images_Form.picname[picture_count + k].image = Image.FromFile(Images_Form.picname[picture_count + k].name);
                            Images_Form.picname[picture_count + k].num = picture_count + k;
                            DGV_PictureList.Rows[picture_count + k].Cells[1].Value = StrFileNames1[k];///获取图片文件名 
                            DGV_PictureList.Rows[picture_count + k].Cells[0].Value = (picture_count + k).ToString();
                            k++;
                        }
                    }
                }
            }
        }

        private static void ShowMessage()
        {
            string message = "";
            if (Main_Form.LanguageType == "English")
            {
                message = $"Up to {Main_Form.Max_Page} pictures";
                MessageBox.Show(message, "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                message = $"最多可编辑 {Main_Form.Max_Page} 张背景图片";
                MessageBox.Show(message, "警告", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            Pic_Number = Main_Form.Max_Page;
        }

        /// <summary>
        /// 删除选中的文件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_Select(object sender, EventArgs e)
        {
            string DelFileName = null;
            Display dis = Display.GetSingle();
            try
            {
                int iCount = DGV_PictureList.SelectedRows.Count;
                //Console.WriteLine("选中的行数：{0}", iCount);
                if (iCount < 1)
                {
                    MessageBox.Show("Delete Picture Fail", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (DialogResult.Yes == MessageBox.Show("Delete selected rows?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
                {
                    int[] originalPage_num = new int[dis.designer1.Items.Count];
                    int originalnumber = 0;
                    foreach (ItemRectangle list in dis.designer1.Items)  //删除选中页上面的图元
                    {
                        originalPage_num[originalnumber] = list.presentpage_num;//记录原始数据
                        //Console.WriteLine(originalPage_num[originalnumber]);
                        originalnumber++;
                    }
                    originalnumber = 0;
                    for (int i = DGV_PictureList.SelectedRows.Count - 1; i >= 0; i--)
                    {
                        DelFileName = DGV_PictureList.SelectedRows[i].Cells[1].Value.ToString();
                        DGV_PictureList.Rows.Remove(DGV_PictureList.SelectedRows[i]);
                       
                        int ii = 0;
                        foreach (PICINFORMATION s in Images_Form.picname)
                        {
                            if (DelFileName == Images_Form.picname[ii].safename)
                            {                                                                                                                                         
                                if(Currentimage != null)
                                {
                                    dis.designer1.BackgroundImage.Dispose();
                                    dis.designer1.BackgroundImage = null;
                                    Currentimage.Dispose();
                                    Currentimage = null;
                                }
                                Images_Form.picname[ii].name = null;
                                Images_Form.picname[ii].safename = null;
                                Images_Form.picname[ii].image.Dispose();
                                Images_Form.picname[ii].image = null;
                                Images_Form.picname[ii].num = -1;
                                originalnumber = 0;
                                foreach (ItemRectangle list in dis.designer1.Items)  //删除选中页上面的图元
                                {

                                    if (originalPage_num[originalnumber] == ii)
                                    {
                                        list.Selected = true;
                                    }
                                    else if (originalPage_num[originalnumber] > ii)
                                    {
                                        list.presentpage_num--;
                                    }
                                    originalnumber++;
                                }
                                if(dis.designer1.Items.SelectionCount>0)
                                {
                                    dis.designer1.DeleteSelectedItem();
                                }
                                break;
                            }
                            ii++;
                        }
                        //try
                        //{
                        //    File.Delete(System.Environment.CurrentDirectory + "\\TGUS" + "\\image" + "\\" + DelFileName);
                        //}
                        //catch(Exception ex)
                        //{
                        //    MessageBox.Show(ex.Message);
                        //}
                        Pic_Number--;
                    }
                    PICINFORMATION[] picnametemp = new PICINFORMATION[Main_Form.Max_Page];   ///用来临时存放图片书记        
                    int temp1 = 0;
                    int i2 = 0;
                    foreach (PICINFORMATION s in Images_Form.picname)
                    {
                        //Console.WriteLine("picname{0}.safename = {1}", i2, Images_Form.picname[i2].safename);
                        if (s.safename != null)
                        {
                            picnametemp[temp1].name = s.name;
                            picnametemp[temp1].image = Image.FromFile(s.name);   ///这里需要绑定，图片要时刻保持进程占用，防止意外删除
                            picnametemp[temp1].num = s.num;
                            picnametemp[temp1].safename = s.safename;
                          
                            Images_Form.picname[i2].name = null;
                            Images_Form.picname[i2].safename = null;
                            Images_Form.picname[i2].image.Dispose();
                            Images_Form.picname[i2].num = -1;
                            temp1++;
                        }
                        i2++;
                    }
                    temp1--;
                    for (; temp1 >= 0; temp1--)
                    {
                        Images_Form.picname[temp1].name = picnametemp[temp1].name;
                        Images_Form.picname[temp1].image = Image.FromFile(picnametemp[temp1].name);
                        picnametemp[temp1].image.Dispose();
                        Images_Form.picname[temp1].num = temp1;
                        Images_Form.picname[temp1].safename = picnametemp[temp1].safename;

                    }
                    for (int i = 0; i < DGV_PictureList.Rows.Count; i++)
                    {
                        DGV_PictureList.Rows[i].Cells[0].Value = i;
                    }
                }
            }
            catch (Exception ex)
            {

                MessageBox.Show(ex.Message);
            }
            foreach (PICINFORMATION s in Images_Form.picname)
            {
                if (s.name == null)
                {
                    break;
                }
                if (s.safename == DGV_PictureList.Rows[Convert.ToInt32(DGV_PictureList.CurrentCell.RowIndex.ToString())].Cells[1].Value.ToString())
                {
                    //Console.WriteLine(s.name);
                    dis.designer1.BackgroundImage = Image.FromFile(s.name);
                    dis.designer1.BackgroundImageLayout = ImageLayout.Stretch;
                    Currentimage = dis.designer1.BackgroundImage;
                    break;
                }
            }
            //Console.WriteLine("当前页是{0}",Main_Form.presentpage_num);
            //foreach (ItemRectangle list in dis.designer1.Items)  //删除选中页上面的图元
            //{
            //    Console.Write("page:");
            //    Console.Write(list.presentpage_num);
            //    Console.Write(" ");
            //}
            //Console.WriteLine("当前活动的行是{0}",DGV_PictureList.CurrentRow.Index);
            Main_Form.presentpage_num = DGV_PictureList.CurrentRow.Index;
            dis.designer1.Refresh();
        }
        /// <summary>
        /// 删除全部图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Del_ALL(object sender, EventArgs e)
        {
            Pic_Number = 0;
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image" + "\\");
            
            if ((Directory.Exists(System.Environment.CurrentDirectory + "\\TGUS" + "\\image" + "\\")) == false)   ///先判断文件件是否存在
            {
                if (Main_Form.LanguageType == "English")
                {
                    MessageBox.Show("The image folder does not exist", "warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("image文件夹不存在", "警告", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                return;
            }
            int i = 0;
            if (DialogResult.Yes == MessageBox.Show("   Delete all?", "Notice", MessageBoxButtons.YesNo, MessageBoxIcon.Information))
            {
                DGV_PictureList.Rows.Clear();
                Display dis = Display.GetSingle();
                if(Currentimage != null)
                {
                    dis.designer1.BackgroundImage.Dispose();
                    dis.designer1.BackgroundImage = null;
                    foreach (ItemRectangle list in dis.designer1.Items)     //将全部页面的图元选中
                    {
                        list.Selected = true;  
                    }
                    dis.designer1.DeleteSelectedItem();                     //将全部的图元删除
                    Currentimage.Dispose();
                    Currentimage = null;
                }
               
                foreach (PICINFORMATION s in Images_Form.picname)
                {
                    if (Images_Form.picname[i].name != null)
                    {
                        Images_Form.picname[i].image.Dispose();
                        Images_Form.picname[i].name = null;
                        Images_Form.picname[i].safename = null;
                        Images_Form.picname[i].num = -1;
                    }
                    i++;
                }
                //try
                //{
                //    FileSystemInfo[] fileinfo = dir.GetFileSystemInfos();  //返回目录中所有文件和子目录

                //    foreach (FileSystemInfo fi in fileinfo)   //判断是否文件夹
                //    {
                //        if (fi is DirectoryInfo)
                //        {
                //            DirectoryInfo subdir = new DirectoryInfo(fi.FullName);
                //            subdir.Delete(true);          ///删除子目录和文件
                //        }
                //        else
                //        {
                //            string exname = fi.Name.Substring(fi.Name.LastIndexOf(".") + 1);//得到后缀名
                //            if (exname == "bmp")
                //            {
                //                File.Delete(fi.FullName);      ///删除指定文件
                //            }
                //        }
                //    }
                //}
                //catch (Exception ex)
                //{
                //    MessageBox.Show(ex.Message);
                //}
            }
        }
        /// <summary>
        /// 上移图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_UpPictre_Click(object sender, EventArgs e)
        {
            string strtemp = null;
            int CurrentRow = 0;
            if (this.DGV_PictureList.SelectedRows.Count != 1)
            {
                if (Main_Form.LanguageType == "English")
                {
                    MessageBox.Show("only single image could be moved", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("只能移动单张图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                return;
            }
            CurrentRow = DGV_PictureList.CurrentRow.Index;     ///当前焦点所在行
            if (CurrentRow == 0)
            {
                if (Main_Form.LanguageType == "English")
                {
                    MessageBox.Show("The first row cannot be moved up", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("第一行无法上移", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                return;
            }
           
            strtemp = DGV_PictureList.Rows[CurrentRow].Cells[1].Value.ToString();
            int i = 0;
            foreach (PICINFORMATION s in Images_Form.picname)
            {
                if (s.safename == strtemp)
                {
                    ///释放图片资源
                    Images_Form.picname[i].image.Dispose();    ///释放图片资源
                    Images_Form.picname[i - 1].image.Dispose();///释放图片资源
                    //被选中的图片信息绑定
                    Images_Form.picname[i].image = Image.FromFile(Images_Form.picname[i - 1].name);
                    Images_Form.picname[i].name = Images_Form.picname[i - 1].name;
                    Images_Form.picname[i].safename = Images_Form.picname[i - 1].safename;
                    ///上面一张的信息绑定
                    Images_Form.picname[i - 1].image = Image.FromFile(s.name);
                    Images_Form.picname[i - 1].name = s.name;
                    Images_Form.picname[i - 1].safename = s.safename;
                    break;    ///每次被选中的只有一个图片，之后的循环没有必要
                }
                i++;
            }
            this.DGV_PictureList.Rows[CurrentRow].Cells[1].Value = DGV_PictureList.Rows[CurrentRow - 1].Cells[1].Value.ToString();
            this.DGV_PictureList.Rows[CurrentRow - 1].Cells[1].Value = strtemp;
            this.DGV_PictureList.Rows[CurrentRow - 1].Selected = true;
            this.DGV_PictureList.Rows[CurrentRow].Selected = false;
            this.DGV_PictureList.CurrentCell = DGV_PictureList.Rows[CurrentRow - 1].Cells[0];

            Display dis = Display.GetSingle();
            foreach (ItemRectangle list in dis.designer1.Items)  //删除选中页上面的图元
            {
                
                if (list.presentpage_num == Main_Form.presentpage_num)
                {

                    list.presentpage_num --;

                }
                else if(list.presentpage_num == (Main_Form.presentpage_num - 1))
                {
                    list.presentpage_num ++;
                }
            }
            Main_Form.presentpage_num--;
        }
        /// <summary>
        /// 下移图片
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void But_DownPicture_Click(object sender, EventArgs e)
        {
            string strtemp = null;
            
            
            int CurrentRow = 0;
            if (this.DGV_PictureList.SelectedRows.Count != 1)
            {
                if (Main_Form.LanguageType == "English")
                {
                    MessageBox.Show("only single image could be moved", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("只能移动单张图片", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                return;
            }
            CurrentRow = this.DGV_PictureList.CurrentRow.Index;     ///当前焦点所在行
            if (CurrentRow == (Main_Form.Max_Page - 1))
            {
                if (Main_Form.LanguageType == "English")
                {
                    MessageBox.Show("The last line cannot be moved down", "Notice", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("最后一行无法下移", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                
                return;
            }
            
            strtemp = this.DGV_PictureList.Rows[CurrentRow].Cells[1].Value.ToString();
            int i = 0;
            foreach (PICINFORMATION s in Images_Form.picname)
            {
                if (s.safename == strtemp)
                {
                    ///释放图片资源
                    Images_Form.picname[i].image.Dispose();    ///释放图片资源
                    Images_Form.picname[i + 1].image.Dispose();///释放图片资源
                    //被选中的图片信息绑定
                    Images_Form.picname[i].image = Image.FromFile(Images_Form.picname[i + 1].name);
                    Images_Form.picname[i].name = Images_Form.picname[i + 1].name;
                    Images_Form.picname[i].safename = Images_Form.picname[i + 1].safename;
                    ///上面一张的信息绑定
                    Images_Form.picname[i + 1].image = Image.FromFile(s.name);
                    Images_Form.picname[i + 1].name = s.name;
                    Images_Form.picname[i + 1].safename = s.safename;
                    break;    ///每次被选中的只有一个图片，之后的循环没有必要
                }
                i++;
            }
            this.DGV_PictureList.Rows[CurrentRow].Cells[1].Value = this.DGV_PictureList.Rows[CurrentRow + 1].Cells[1].Value.ToString();
            this.DGV_PictureList.Rows[CurrentRow + 1].Cells[1].Value = strtemp;
            this.DGV_PictureList.Rows[CurrentRow + 1].Selected = true;
            this.DGV_PictureList.Rows[CurrentRow].Selected = false;
            this.DGV_PictureList.CurrentCell = DGV_PictureList.Rows[CurrentRow + 1].Cells[0];
            Display dis = Display.GetSingle();
            foreach (ItemRectangle list in dis.designer1.Items)  
            {
                //Console.WriteLine(list.presentpage_num);
                if (list.presentpage_num == Main_Form.presentpage_num)
                {
                    list.presentpage_num++;
                }
                else if (list.presentpage_num == (Main_Form.presentpage_num + 1))
                {
                    list.presentpage_num--;
                }
            }
            Main_Form.presentpage_num++;
        }
        /// <summary>
        /// 点击图片的时候，上一个图片资源释放，绑定下一个图片资源
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void DGV_MouseDown_Click(object sender, DataGridViewCellMouseEventArgs e)
        {
            if(e.RowIndex < 0)   ///点到了表头
            {
                return;
            }
            if(picname[e.RowIndex].name != null)
            {
                Display dis = Display.GetSingle();    ///获取窗体指针
                if (Currentimage != null)
                {
                    dis.designer1.BackgroundImage.Dispose();
                    dis.designer1.BackgroundImage = null;  
                }
                dis.designer1.BackgroundImage = Image.FromFile(picname[e.RowIndex].name);
                dis.designer1.BackgroundImageLayout = ImageLayout.Stretch;

                Currentimage = dis.designer1.BackgroundImage;///记录当前图片  
                Main_Form.presentpage_num = e.RowIndex;  
            }
        }
    }
}

