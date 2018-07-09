using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
namespace TGUS
{
    static class GetIconFiles
    {
        public static  List<ChoicePicture_Form.iconcell> Icon_List = new List<ChoicePicture_Form.iconcell> { };
        public static void Geticon(string iconfilename)
        {
            try
            {
                FileStream fs1;
                ChoicePicture_Form.iconcell TempIcon;
                string path1 = System.Environment.CurrentDirectory + "\\TGUS" + "\\I" + "\\" + iconfilename;
                fs1 = new FileStream(path1, FileMode.Open);
                byte[] bydata = new byte[fs1.Length];
                fs1.Read(bydata, 0, (int)fs1.Length);
                byte width = 0;
                byte height = 0;
                UInt32 DataLength = 0;
                UInt32 DataLocation = 0;
                int TransCloor = 0;
                Icon_List.Clear();//清空原来的数据，添加新的数据
                for (int i = 0; bydata[8 + i] != 0; i += 8)
                {
                    width = bydata[8 + i];
                    height = bydata[9 + i];
                    DataLength = (UInt32)width * height * 2;
                    DataLocation = 0;
                    for (int j = 0; j < 4; j++)    //读取图片的位置
                    {
                        DataLocation <<= 8;
                        DataLocation |= bydata[10 + i + j];
                    }
                    DataLocation <<= 1;      //乘2才是图片的真正位置
                    TransCloor |= (bydata[14] << 8) & 0xFF00;  //图片的透明色
                    TransCloor |= bydata[15];
                    byte[] ImageData = new byte[DataLength];
                    Array.Copy(bydata, DataLocation, ImageData, 0, DataLength);   //将本次循环的图片的位图数据
                    UInt16[] ImageData1 = new UInt16[DataLength / 2];             //组合16位的RGB的数据为16位的数据，方便进行颜色分解
                    for (int h = 0; h < DataLength; h++)
                    {
                        if (h % 2 == 0)
                        {
                            ImageData1[h / 2] = (UInt16)((ImageData[h] << 8) | ImageData[h + 1]);
                        }
                    }
                    Bitmap pic = new Bitmap(width, height, System.Drawing.Imaging.PixelFormat.Format16bppRgb565);
                    Color c;
                    int[] color_R = new int[ImageData1.Length];
                    int[] cloor_G = new int[ImageData1.Length];
                    int[] color_B = new int[ImageData1.Length];
                    for (int h = 0; h < ImageData1.Length; h++)  //颜色分解
                    {
                        color_R[h] = (int)((((ImageData1[h] & 0XF800) >> 11) << 3) | ((ImageData1[h] & 0X1800) >> 11));
                        cloor_G[h] = (int)((((ImageData1[h] & 0x07E0) >> 5) << 2) | (ImageData1[h] & 0x0060) >> 5);
                        color_B[h] = (int)(((ImageData1[h] & 0x001F) << 3) | (ImageData1[h] & 0x0007));
                    }
                    for (int r = 0; r < ImageData1.Length; r++)   //将颜色与图片的位置一一对应
                    {
                        c = Color.FromArgb(color_R[r], cloor_G[r], color_B[r]);
                        pic.SetPixel(r % width, r / width, c);
                    }
                    TempIcon.image = pic;
                    TempIcon.name = (i / 8).ToString();
                    Icon_List.Add(TempIcon);
                }
                fs1.Dispose();
                fs1.Close();  //关闭，防止内存泄露，编程的时候声明fs1马上就写close以免遗忘
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
