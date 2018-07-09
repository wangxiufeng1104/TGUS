using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D; 

namespace TGUS
{
    public partial class ChoicePicture_Form : Form
    {

        public struct iconcell
        {
            public Image image;
            public string name;
        }
        public static bool IsImageTrasparent;
        DirectoryInfo dir;
        static button_pic[] buttonpic = new button_pic[31];
        static button_pic[] iconbp = new button_pic[1536];
        iconcell[] ima_icon = new iconcell[1536];
        Main_Form fMain_Form = null;
        Touch fTouch_Form = null;
        int fun_flag = 0;
        static int pic_num = 0;
        ChangeImage ChangeBoxPicture = new ChangeImage(Touch.change_Box_Image);
        /// <summary>
        /// 面板上的两个选择按键，上面是1 下面是2 不用区分写0
        /// </summary>
        private int selectButton;

        public ChoicePicture_Form(DirectoryInfo dir, Main_Form form,Touch touch,int selectbutton)
        {
            this.dir = dir;           //工程路径
            this.fMain_Form = form;       //主窗体的指针
            this.fTouch_Form = touch;
            IsImageTrasparent = false;
            selectButton = selectbutton;
            
            InitializeComponent();
            Check_Backgroundcolor.CheckedChanged += Check_Backgroundcolor_CheckedChanged;
            if(Main_Form.LanguageType == "English")
            {
                confirm.Text = "Confirm";
                cancel.Text = "Cancel";
                Check_Backgroundcolor.Text = "Remove background color";

            }
            else
            {
                confirm.Text = "确认";
                cancel.Text = "取消";
                Check_Backgroundcolor.Text = "去掉背景色";
            }
        }
        void Check_Backgroundcolor_CheckedChanged(object sender,EventArgs e)
        {
            IsImageTrasparent = (Check_Backgroundcolor.Checked)?(true):(false);
        }
        public void touchpicshow()
        {
            if(Main_Form.LanguageType == "English")
            {
                Text = "Picture choice";
            }
            else
            {
                Text = "图形选择";
            }
            //Check_Backgroundcolor.Enabled = false;
            Check_Backgroundcolor.Visible = false;
            fun_flag = 2;
            int i = 0;
            foreach (Images_Form.PICINFORMATION pn in Images_Form.picname)
            {
                if (pn.name != null && pn.image != null)
                {
                    fun_flag = 2;
                    buttonpic[i] = new button_pic(pn.image, pn.safename, pn.num);
                    buttonpic[i].Top = 5 + (128 + 5) * (i / 8);
                    buttonpic[i].Left = 5 + 115 * (i % 8);
                    buttonpic[i].Width = 110;
                    buttonpic[i].Height = 128;
                    panel1.Controls.Add(buttonpic[i]);
                    panel1.Focus();
                    i++;
                }
            }
        }
        public void iconshow(string iconfilename, int index)
        {
            if (Main_Form.LanguageType == "English")
            {
                Text = "Icon choice";
            }
            else
            {
                Text = "图标选择";
            }
            //Check_Backgroundcolor.Enabled = true;
            Check_Backgroundcolor.Visible = true;
            fun_flag = index;
            int int_tmep = 0;
            Geticon(iconfilename);
            FileInfo[] FI = new FileInfo[1536];
            FI = dir.GetFiles();
            foreach (iconcell im in ima_icon)
            {
                if (im.image != null)
                {
                    iconbp[int.Parse(im.name)] = new button_pic(im.image, " ", int.Parse(im.name)+1);
                    iconbp[int.Parse(im.name)].Top = 5 + (128 + 5) * (int_tmep / 8);
                    iconbp[int.Parse(im.name)].Left = 5 + 115 * (int_tmep % 8);
                    iconbp[int.Parse(im.name)].Width = 110;
                    iconbp[int.Parse(im.name)].Height = 128;
                    panel1.Controls.Add(iconbp[int.Parse(im.name)]);
                    panel1.Focus();
                    int_tmep++;
                }
            }
        }
        public static void button_picreflash(int index)
        {
            pic_num = index;
            foreach (button_pic bp in buttonpic)
            {
                if (bp != null)
                {
                    bp.back_color = Color.White;
                    if (bp.index == index)
                    {
                        bp.back_color = Color.FromArgb(60, Color.SkyBlue);
                    }
                }
            }
            foreach (button_pic bp in iconbp)
            {
                pic_num = index;
                if (bp != null)
                {
                    bp.back_color = Color.White;
                    if (bp.index == index)
                    {
                        bp.back_color = Color.FromArgb(60, Color.SkyBlue);
                    }
                }
            }
        }
        private void confirm_Click(object sender, EventArgs e)
        {
            switch(Main_Form.SelectType)
            {
                case PIC_Obj.basictouch:
                    if(selectButton == 1)
                    {
                        fTouch_Form.BaseTouch_ButtonEffectnum.Value = pic_num;
                        fTouch_Form.BaseTouch_ButtonEffectPicture.Image = buttonpic[pic_num].buttonpic.Image;

                        ChangeBoxPicture(buttonpic[pic_num].buttonpic.Image);
                    }
                    else if(selectButton == 2)
                    {
                        fTouch_Form.BaseTouch_ButtonChangePageNum.Value = pic_num;
                        fTouch_Form.BaseTouch_ButtonChangePagePic.Image = buttonpic[pic_num].buttonpic.Image;
                    }
                    break;
                case PIC_Obj.icon_display:
                    if (selectButton == 1)
                    {
                        fTouch_Form.Iconvar_VarMaxPic.Image = iconbp[pic_num - 1].buttonpic.Image;
                        fTouch_Form.Iconvar_VarMaxNum.Value = pic_num;
                    }
                    else if (selectButton == 2)
                    {
                        fTouch_Form.Iconvar_VarMinPic.Image = iconbp[pic_num - 1].buttonpic.Image;
                        fTouch_Form.Iconvar_VarMinNum.Value = pic_num;
                    }
                    break;
                case PIC_Obj.datainput:
                    if(selectButton == 1)   //按键效果选择
                    {
                        fTouch_Form.DataInput_ButtonEffectPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.DataInput_ButtonEffectNum.Value = pic_num;
                    }
                    else if(selectButton == 2)   //页面切换选择
                    {
                        fTouch_Form.DataInput_PageChangePic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.DataInput_PageChangeNum.Value = pic_num;
                    }
                    else if(selectButton == 3)
                    {
                        Image_Area_Setting Image_Area = Image_Area_Setting.GetSingle();
                        Image_Area.ShowPic(buttonpic[pic_num].buttonpic.Image.Width, buttonpic[pic_num].buttonpic.Image.Height, buttonpic[pic_num].buttonpic.Image);
                        fTouch_Form.DataInput_KeyBoardPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.DataInput_KeyBoardAtPage.Value = pic_num;
                        if (Image_Area.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                    break;
                case PIC_Obj.keyreturn:
                    if(selectButton == 1)
                    {
                        fTouch_Form.KeyReturn_ButtonEffrctPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.KeyReturn_ButtonEffrctNum.Value = pic_num;
                    }
                    else
                    {
                        fTouch_Form.KeyReturn_ButtonChangePagePic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.KeyReturn_ButtonChangePageNum.Value = pic_num;
                    }
                    break;
                case PIC_Obj.menu_display:
                    if(selectButton == 1)   //按键效果选择
                    {
                        fTouch_Form.PopupMenu_ButtonEffectPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.PopupMenu_ButtonEffectNum.Value = pic_num;
                    }
                    else if (selectButton == 3)
                    {
                        Image_Area_Setting Image_Area = Image_Area_Setting.GetSingle();
                        Image_Area.ShowPic(buttonpic[pic_num].buttonpic.Image.Width, buttonpic[pic_num].buttonpic.Image.Height, buttonpic[pic_num].buttonpic.Image);
                        fTouch_Form.PopupMenu_MenuPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.PopupMenu_MenuAtPage.Value = pic_num;
                        if (Image_Area.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                    break;
                case PIC_Obj.aniicon_display:
                    if (selectButton == 1)
                    {
                        fTouch_Form.ActionIcon_StopID.Value = pic_num;
                    }
                    else if (selectButton == 2)
                    {
                        fTouch_Form.ActionIcon_StartID.Value = pic_num;
                    }
                    else if (selectButton == 3)
                    {
                        fTouch_Form.ActionIcon_EndID.Value = pic_num;
                    }
                    break;
                case PIC_Obj.increadj:
                    fTouch_Form.IncreaseAdj_ButtonEffectPic.Image = buttonpic[pic_num].buttonpic.Image;
                    fTouch_Form.IncreaseAdj_ButtonEffectNum.Value = pic_num;
                    break;
                case PIC_Obj.artfont:
                    fTouch_Form.ArtFont_BeginIconNum.Value = pic_num;
                    break;
                case PIC_Obj.slidis:
                    fTouch_Form.SlideDisplay_IconIDNum.Value = pic_num;
                    break;
                case PIC_Obj.iconrota:
                    fTouch_Form.IconRotation_IconIDNum.Value = pic_num;
                    Image_Area_Setting IconRotationImage_Area = Image_Area_Setting.GetSingle();
                    IconRotationImage_Area.ShowPic(iconbp[pic_num - 1].buttonpic.Image.Width, iconbp[pic_num - 1].buttonpic.Image.Height, iconbp[pic_num - 1].buttonpic.Image);
                    if (IconRotationImage_Area.ShowDialog() == DialogResult.OK)
                    {
                    }
                    break;
                case PIC_Obj.clockdisplay:
                    if (selectButton == 1)
                    {
                        fTouch_Form.ClockDisplay_HourIconNum.Hexadecimal = false;
                        fTouch_Form.ClockDisplay_HourIconNum.Value = pic_num;
                    }
                    else if (selectButton == 2)
                    {
                        fTouch_Form.ClockDisplay_MinuteIconNum.Hexadecimal = false;
                        fTouch_Form.ClockDisplay_MinuteIconNum.Value = pic_num;
                    }
                    else if (selectButton == 3)
                    {
                        fTouch_Form.ClockDisplay_SecondIconNum.Hexadecimal = false;
                        fTouch_Form.ClockDisplay_SecondIconNum.Value = pic_num;
                    }
                    Image_Area_Setting clockdisplayImage_Area = Image_Area_Setting.GetSingle();
                    clockdisplayImage_Area.ShowPic(iconbp[pic_num - 1].buttonpic.Image.Width, iconbp[pic_num - 1].buttonpic.Image.Height, iconbp[pic_num - 1].buttonpic.Image);
                    if (clockdisplayImage_Area.ShowDialog() == DialogResult.OK)
                    {
                    }
                    break;
                case PIC_Obj.GBK:
                    if (selectButton == 1)
                    {
                        fTouch_Form.GBK_ButtonEffectPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.GBK_ButtonEffectNum.Value = pic_num;
                    }
                    else if(selectButton == 2)
                    {
                        fTouch_Form.GBK_PageChangePic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.GBK_PageChangeNum.Value = pic_num;
                    }
                    else
                    {
                        Image_Area_Setting Image_Area = Image_Area_Setting.GetSingle();
                        Image_Area.ShowPic(buttonpic[pic_num].buttonpic.Image.Width, buttonpic[pic_num].buttonpic.Image.Height, buttonpic[pic_num].buttonpic.Image);
                        fTouch_Form.GBK_KeyBoardPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.GBK_KeyBoardAtPage.Value = pic_num;
                        if (Image_Area.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                    break;
                case PIC_Obj.ASCII:
                    if (selectButton == 1)
                    {
                        fTouch_Form.ASCII_ButtonEffectPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.ASCII_ButtonEffectNum.Value = pic_num;
                    }
                    else if (selectButton == 2)
                    {
                        fTouch_Form.ASCII_PageChangePic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.ASCII_PageChangeNum.Value = pic_num;
                    }
                    else
                    {
                        Image_Area_Setting Image_Area = Image_Area_Setting.GetSingle();
                        Image_Area.ShowPic(buttonpic[pic_num].buttonpic.Image.Width, buttonpic[pic_num].buttonpic.Image.Height, buttonpic[pic_num].buttonpic.Image);
                        fTouch_Form.ASCII_KeyBoardPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.ASCII_KeyBoardAtPage.Value = pic_num;
                        if (Image_Area.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                    break;
                case PIC_Obj.TouchState:
                    if (selectButton == 1)
                    {
                        fTouch_Form.TouchState_ButtonEffectPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.TouchState_ButtonEffectNum.Value = pic_num;
                    }
                    else if (selectButton == 2)
                    {
                        fTouch_Form.TouchState_PageSwitchPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.TouchState_PageSwitchNum.Value = pic_num;
                    }
                    break;
                case PIC_Obj.RTC_Set:
                    if(selectButton == 1)
                    {
                        fTouch_Form.RTCset_ButtonEffectPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.RTCset_ButtonEffectNum.Value = pic_num;
                    }
                    else
                    {
                        Image_Area_Setting Image_Area = Image_Area_Setting.GetSingle();
                        Image_Area.ShowPic(buttonpic[pic_num].buttonpic.Image.Width, buttonpic[pic_num].buttonpic.Image.Height, buttonpic[pic_num].buttonpic.Image);
                        fTouch_Form.RTCset_KeyBoardPic.Image = buttonpic[pic_num].buttonpic.Image;
                        fTouch_Form.RTCset_KeyBoardAtPage.Value = pic_num;
                        if (Image_Area.ShowDialog() == DialogResult.OK)
                        {
                        }
                    }
                    break;

            }
            this.DialogResult = DialogResult.OK;
        }
        private void Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        private void Geticon(string iconfilename)
        {
            try
            {
                FileStream fs1;
                string path1 = System.Environment.CurrentDirectory + "\\TGUS" + "\\I" + "\\" + iconfilename;
                fs1 = new FileStream(path1, FileMode.Open);
                byte[] bydata = new byte[fs1.Length];
                fs1.Read(bydata, 0, (int)fs1.Length);
                byte width = 0;
                byte height = 0;
                UInt32 DataLength = 0;
                UInt32 DataLocation = 0;
                int TransCloor = 0;
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
                    ima_icon[i/8].image = pic;
                    ima_icon[i/8].name = (i/8).ToString();
                }
                fs1.Dispose();
                fs1.Close();  //关闭，防止内存泄露，编程的时候声明fs1马上就写close以免遗忘
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
