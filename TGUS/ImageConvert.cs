using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;
using System.IO;
using System.Threading;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Xml;
namespace TGUS
{
    public partial class ImageConvert : Skin_VS
    {
        string[] path1 = null;                 //用于存储选择的文件列表
        string path2 = null;                    //用于存储保存的路径
        Thread td;                          //声明一个线程
        int Com_SelectIndex;
        public struct BmpINFO
        {
            public byte width;          //图片的宽度
            public byte height;         //图片的高度
            public UInt16 Transparent;  //透明色  左上角的REG
            public UInt32 Location;     //位图信息的位置   (width * height)*2 + fs.Length   
        }
        public ImageConvert()
        {
            InitializeComponent();
            
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
                XmlNode Lan = doc.SelectSingleNode("/TGUS/ImageConvert");
                Control[] tempControls = new Control[this.Controls.Count];
                this.Controls.CopyTo(tempControls, 0);
                foreach (Control ctr in tempControls)
                {
                    if (ctr is CCWin.SkinControl.SkinToolStrip)
                    {
                        CCWin.SkinControl.SkinToolStrip ctr1 = ctr as CCWin.SkinControl.SkinToolStrip;
                        foreach (object obj in ctr1.Items)
                        {
                            if (obj is ToolStripButton || obj is ToolStripLabel)
                            {
                                XmlNode s = null;
                                if(obj is ToolStripButton)
                                    s = Lan.SelectSingleNode(((ToolStripButton)obj).Name);
                                else if(obj is ToolStripLabel)
                                    s = Lan.SelectSingleNode(((ToolStripLabel)obj).Name);
                                if (s != null)
                                {
                                    string str = s.InnerText;
                                    if (obj is ToolStripButton)
                                        ((ToolStripButton)obj).Text = str;
                                    else if(obj is ToolStripLabel)
                                        ((ToolStripLabel)obj).Text = str;
                                }
                            }
                            else if(obj is ToolStripComboBox)
                            {
                                int i = 0;
                                XmlNode s = Lan.SelectSingleNode(((ToolStripComboBox)obj).Name);
                                for(i = 0;i <= 5;i++)
                                {
                                    ((ToolStripComboBox)obj).Items[i] = s.Attributes["Item" + i.ToString()].Value;
                                }
                            } 
                        }
                    }
                    else if (ctr is CCWin.SkinControl.SkinListView)
                    {
                        XmlNode s = Lan.SelectSingleNode(((CCWin.SkinControl.SkinListView)ctr).Name);
                        ((CCWin.SkinControl.SkinListView)ctr).Columns[1].Text = s.Attributes["filename"].Value;
                        ((CCWin.SkinControl.SkinListView)ctr).Columns[2].Text = s.Attributes["extension_name"].Value;
                        ((CCWin.SkinControl.SkinListView)ctr).Columns[3].Text = s.Attributes["date"].Value;
                        ((CCWin.SkinControl.SkinListView)ctr).Columns[4].Text = s.Attributes["size"].Value;
                        ((CCWin.SkinControl.SkinListView)ctr).Columns[5].Text = s.Attributes["state"].Value;
                    }
                }
            }
            Com_TransItem.SelectedIndexChanged += Com_TransItem_SelectedIndexChanged;
            Com_TransItem.SelectedIndex = 0;

        }

        private void Com_TransItem_SelectedIndexChanged(object sender, EventArgs e)
        {
            Com_SelectIndex = Com_TransItem.SelectedIndex;
        }

        public delegate void MyInvoke(string str1,int i);
        private void 打开OToolStripButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "所有图片|*.jpg;*.jpeg;*.bmp;*.png";
            openFileDialog.AddExtension = true;
            openFileDialog.AutoUpgradeEnabled = true;
            openFileDialog.CheckFileExists = true;
            openFileDialog.CheckPathExists = true;
            openFileDialog.DereferenceLinks = true;
            openFileDialog.FilterIndex = 1;
            openFileDialog.Multiselect = true;
            openFileDialog.ValidateNames = true;
            if(openFileDialog.ShowDialog() == DialogResult.OK)
            {
                skinListView1.Items.Clear();  //清空listView
                string[] info = new string[6];                          //存储每一行数据
                FileInfo fi;                                            //创建一个FileInfo对象，用于获取图片信息
                path1 = openFileDialog.FileNames;                   //获取选取的图片集合
                foreach(string p in path1)              //读取集合中的内容
                {
                    //获取图片名称
                    string ImgName = p.Substring(p.LastIndexOf("\\") + 1, p.Length - p.LastIndexOf("\\") - 1);
                    Image img = System.Drawing.Image.FromFile(p);
                    string ImgType;
                    //获取图片类型
                    System.Drawing.Imaging.ImageFormat _img_format = GetImageFormat(img, out ImgType);
                    fi = new FileInfo(p.ToString());             //实例化FileInfo对象
                    imageList1.Images.Add(ImgName, Properties.Resources.图标23);
                    info[1] = ImgName;
                    info[2] = ImgType;
                    info[3] = fi.LastWriteTime.ToShortDateString();//图片最后修改日期
                    info[4] = fi.Length + "B";                //图片大小
                    if(Main_Form.LanguageType == "English")
                    {
                        info[5] = "Unconverted";
                    }
                    else
                    {
                        info[5] = "未转换"; //图片状态 
                    }                    
                    ListViewItem lvi = new ListViewItem(info, ImgName);  //实例化ListViewItem对象
                    skinListView1.Items.Add(lvi);                              //将信息添加到listView1控件中
                }
                if (Main_Form.LanguageType == "English")
                {
                    toolStripStatusLabel1.Text = "Current total" + path1.Length.ToString() + "Files";//状态栏中显示图片数量
                }
                else
                {
                    toolStripStatusLabel1.Text = "当前共有 " + path1.Length.ToString() + " 个文件";//状态栏中显示图片数量
                }
            }
        }
        private void 保存SToolStripButton_Click(object sender, EventArgs e)
        {
            if (path1 != null)
            {
                
                saveFileDialog1.InitialDirectory = path1[0].Substring(0, path1[0].LastIndexOf("\\")+1);
            }
            if(saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path2 = saveFileDialog1.FileName;
            }
        }
        private System.Drawing.Imaging.ImageFormat GetImageFormat(Image _img, out string ImgType)
        {
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            {
                ImgType = ".jpg";
                return System.Drawing.Imaging.ImageFormat.Jpeg;
            }
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Jpeg))
            {
                ImgType = ".jpeg";
                return System.Drawing.Imaging.ImageFormat.Gif;
            }
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Png))
            {
                ImgType = ".png";
                return System.Drawing.Imaging.ImageFormat.Png;
            }
            if (_img.RawFormat.Equals(System.Drawing.Imaging.ImageFormat.Bmp))
            {
                ImgType = ".bmp";
                return System.Drawing.Imaging.ImageFormat.Bmp;
            }
            ImgType = string.Empty;
            return null;
        }
        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            skinListView1.Items.Clear();
            path2 = null;
            if(path1 == null)
            {
                return;
            }
            for(int i = 0; i < path1.Length; i++)
            {
                path1[i] = string.Empty;
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            td = new Thread(new ThreadStart(ImageConvertThread));//通过线程调用ConvertImage方法进行转换
            td.Start();
        }

        private void ImageConvertThread()
        {
            if (path1 == null)
            {
                MessageBox.Show("Please select images");
                return;
            }
            if (path2 == null)
            {
                MessageBox.Show("please select save path");
                return;
            }
            FileStream fs = new FileStream(path2, FileMode.Create);
            fs.SetLength(0x40000);
            BmpINFO bmpinfo = new BmpINFO();
            for (int i = 0; i < path1.Length; i++)
            {
                byte[] byteArray = new byte[8];
                //1.获取图片信息 
                //FileInfo f = new FileInfo(path1[i]);
                //2将不是bmp格式的图片转换为bmp格式
                string ImgType;
                string ImagePath = path2.Substring(0, path2.LastIndexOf("\\") + 1);
                //获取图片类型
                Image img = System.Drawing.Image.FromFile(path1[i]);


                System.Drawing.Imaging.ImageFormat _img_format = GetImageFormat(img, out ImgType);
                if (ImgType != ".bmp")
                {
                    Bitmap myImage = new Bitmap(img.Width, img.Height, PixelFormat.Format32bppRgb);
                    using (Graphics g = Graphics.FromImage(myImage))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.HighQualityBicubic;
                        g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                        g.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighQuality;
                        g.DrawImage(img, 0, 0);
                    }
                    path1[i] = Path.ChangeExtension(path1[i], ".TGUStemp");
                    img.Save(path1[i], ImageFormat.Bmp);

                    myImage.Dispose();
                }
                Bitmap bitmap;//定义Bitmap类
                bitmap = new Bitmap(path1[i]);
                bitmap = ToBgr565(bitmap);
                Color clr = bitmap.GetPixel(0, 0);
                bmpinfo.Transparent = (UInt16)(((clr.R >> 3) << 11) | ((clr.G >> 2) << 5) | ((clr.B >> 3)));

                bmpinfo.width = (byte)bitmap.Width;
                bmpinfo.height = (byte)bitmap.Height;
                bitmap.RotateFlip(RotateFlipType.Rotate180FlipX);//垂直翻转   十分重要

                switch(Com_SelectIndex)
                {
                    case 0:
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipNone);
                        break;
                    case 1:
                        bitmap.RotateFlip(RotateFlipType.Rotate180FlipNone);//不进行翻转的 180 度旋转。
                        break;
                    case 2:
                        bitmap.RotateFlip(RotateFlipType.Rotate270FlipNone);//指定不进行翻转的 270 度旋转 
                        break;
                    case 3:
                        bitmap.RotateFlip(RotateFlipType.Rotate90FlipNone);
                        break;
                    case 4:
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipX);//X轴镜像
                        break;
                    case 5:
                        bitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);//Y轴镜像
                        break;
                }
                bmpinfo.width = (byte)bitmap.Width;
                bmpinfo.height = (byte)bitmap.Height;
                ImagePath += "\\" + (i + 1) + ".TGUSbmp";//保存临时文件
                bitmap.Save(ImagePath, ImageFormat.Bmp);
                Stream stream = File.OpenRead(ImagePath);

                byte[] buffer = new byte[stream.Length];//将位图数据读到buffer里面
                stream.Read(buffer, 0, (int)stream.Length);

                UInt32 byteoffset = 0;//图片数据的偏移量
                for (int j = 0; j < 4; j++)
                {
                    byteoffset <<= 8;
                    byteoffset |= buffer[13 - j];
                }
                bmpinfo.Location = (UInt32)fs.Length / 2;
                byte[] bmpinfobyte = new byte[8];
                bmpinfobyte = StructToBytes(bmpinfo);
                byte temp;
                temp = bmpinfobyte[2];
                bmpinfobyte[2] = bmpinfobyte[7];
                bmpinfobyte[7] = temp;
                temp = bmpinfobyte[3];
                bmpinfobyte[3] = bmpinfobyte[6];
                bmpinfobyte[6] = temp;
                temp = bmpinfobyte[4];
                bmpinfobyte[4] = bmpinfobyte[5];
                bmpinfobyte[5] = temp;
                //List<byte> array = new List<byte>
                fs.Position = fs.Seek(8 * (i + 1), SeekOrigin.Begin);
                fs.Write(bmpinfobyte, 0, 8);
                fs.Position = fs.Length;
                byte[] fbyte = new byte[buffer.Length - byteoffset];
                Array.Copy(buffer, byteoffset, fbyte, 0, fbyte.Length);
                for (int leng = 0; leng < fbyte.Length; leng += 2)
                {
                    fbyte[leng] = (byte)(fbyte[leng] ^ fbyte[leng + 1]);
                    fbyte[leng + 1] = (byte)(fbyte[leng] ^ fbyte[leng + 1]);
                    fbyte[leng] = (byte)(fbyte[leng] ^ fbyte[leng + 1]);
                }
                if (bmpinfo.width % 2 == 1)
                {
                    int iii = 0;
                    byte[] fnew = new byte[fbyte.Length - bmpinfo.height * 2];
                    for (int ii = 0; ii < bmpinfo.height; ii++)
                    {
                        for (int j = 0; j < (bmpinfo.width * 2 + 2); j++)
                        {
                            if (j < bmpinfo.width * 2)
                            {

                                fnew[iii] = fbyte[ii * (bmpinfo.width * 2 + 2) + j];

                                iii++;
                            }
                        }
                    }
                    fs.Write(fnew, 0, fnew.Length);
                }
                else
                {
                    fs.Write(fbyte, 0, fbyte.Length);
                }


                img.Dispose();
                bitmap.Dispose();
                stream.Close();
                MyInvoke mi = new MyInvoke(UpdateForm);
                string con = string.Empty;
                if(Main_Form.LanguageType == "English")
                {
                    con = "Converted";
                }
                else
                {
                    con = "已转换";
                }
                this.BeginInvoke(mi, con, i);
            }
            fs.Close();
            //文件夹路径
            DirectoryInfo dyInfo = new DirectoryInfo(path2.Substring(0, path2.LastIndexOf("\\") + 1));
            //获取文件夹下所有的文件
            foreach (FileInfo feInfo in dyInfo.GetFiles())
            {
                //判断文件后缀
                if (feInfo.Extension == ".TGUStemp" || feInfo.Extension == ".TGUSbmp")
                    feInfo.Delete();
            }
            dyInfo = new DirectoryInfo(path1[0].Substring(0, path1[0].LastIndexOf("\\") + 1));
            foreach (FileInfo feInfo in dyInfo.GetFiles())
            {
                //判断文件后缀
                if (feInfo.Extension == ".TGUStemp" || feInfo.Extension == ".TGUSbmp")
                    feInfo.Delete();
            }
            if(Main_Form.LanguageType == "English")
            {
                toolStripStatusLabel1.Text = "Picture conversion complete";
            }
            else
            {
                toolStripStatusLabel1.Text = "图片转换全部完成";
            }
            
        }
        public void UpdateForm(string strtemp,int i)
        {
            skinListView1.Items[i].SubItems[5].Text = strtemp;
        }
        /// <summary>
        /// 将原始图像转换成格式为Bgr565的16位图像
        /// </summary>
        /// <param name="bmp">用于转换的原始图像</param>
        /// <returns>转换后格式为Bgr565的16位图像</returns>
        public static Bitmap ToBgr565(Bitmap bmp)
        {
            Int32 PixelHeight = bmp.Height; // 图像高度
            Int32 PixelWidth = bmp.Width;   // 图像宽度
            Int32 Stride = ((PixelWidth * 3 + 3) >> 2) << 2;    // 跨距宽度
            Byte[] Pixels = new Byte[PixelHeight * Stride];

            // 锁定位图到系统内存
            BitmapData bmpData = bmp.LockBits(new Rectangle(0, 0, PixelWidth, PixelHeight), ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(bmpData.Scan0, Pixels, 0, Pixels.Length);  // 从非托管内存拷贝数据到托管内存
            bmp.UnlockBits(bmpData);    // 从系统内存解锁位图
            bmp.Dispose();

            // Bgr565格式为 RRRRR GGGGGG BBBBB
            Int32 TargetStride = ((PixelWidth + 1) >> 1) << 2;  // 每个像素占2字节，且跨距要求4字节对齐
            Byte[] TargetPixels = new Byte[PixelHeight * TargetStride];
            for (Int32 i = 0; i < PixelHeight; i++)
            {
                Int32 Index = i * Stride;
                Int32 Loc = i * TargetStride;
                for (Int32 j = 0; j < PixelWidth; j++)
                {
                    Byte B = Pixels[Index++];
                    Byte G = Pixels[Index++];
                    Byte R = Pixels[Index++];

                    TargetPixels[Loc++] = (Byte)(((G << 3) & 0xe0) | ((B >> 3) & 0x1f));
                    TargetPixels[Loc++] = (Byte)((R & 0xf8) | ((G >> 5) & 7));
                }
            }
            // 创建Bgr565图像
            Bitmap TargetBmp = new Bitmap(PixelWidth, PixelHeight, PixelFormat.Format16bppRgb565);

            // 设置位图图像特性
            BitmapData TargetBmpData = TargetBmp.LockBits(new Rectangle(0, 0, PixelWidth, PixelHeight), ImageLockMode.WriteOnly, PixelFormat.Format16bppRgb565);
            Marshal.Copy(TargetPixels, 0, TargetBmpData.Scan0, TargetPixels.Length);
            TargetBmp.UnlockBits(TargetBmpData);
            return TargetBmp;
        }
        public Byte[] StructToBytes(Object structure)
        {
            //得到结构体的大小
            int size = Marshal.SizeOf(structure);
            //创建byte数组
            byte[] bytes = new byte[size];
            //分配结构体大小的内存空间
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            //将结构体拷到分配好的内存空间
            Marshal.StructureToPtr(structure, structPtr, false);
            //从内存空间拷到byte数组
            Marshal.Copy(structPtr, bytes, 0, size);
            //释放内存空间
            Marshal.FreeHGlobal(structPtr);
            //返回byte数组
            return bytes;

        }
        private void ImageConvert_FormClosed(object sender, FormClosedEventArgs e)//关闭窗口时要关闭线程
        {
            if(td != null)                   //判断是否存在线程
            {
                if(td.ThreadState == ThreadState.Running)  //判断线程是否在运行
                { 
                    td.Abort();                         //如果运行则关闭线程
                }
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
