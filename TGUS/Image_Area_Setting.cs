using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TGUS
{
    
    public partial class Image_Area_Setting : Form
    {
        public DrawRectangle DrawRec = new DrawRectangle();
        public DrawSelect DrawSec = new DrawSelect();
        public static Image_Area_Setting imageareasettingsingle = null;
        private static Image backimage = null;
        Bitmap bitmap = null;
        Image backgroundbitmap = null;
        /// <summary>
        /// 用于确定是哪个按键激发窗口，需要的时候设置这个字符串，窗体关闭的时候字符串清空
        /// </summary>
        public static string ButName = string.Empty;
        public static Image_Area_Setting GetSingle()
        {
            if (imageareasettingsingle == null)
            {
                imageareasettingsingle = new Image_Area_Setting();
            }
            return imageareasettingsingle;
        }
        private Image_Area_Setting()
        {
            InitializeComponent();
            SelectPosition_Designer.IsDrawSelectRectangle = false;
            SelectPosition_Designer.ActiveControl = DrawRec;
            Check_keyboard.CheckedChanged += Check_keyboard_CheckedChanged;
        }

        private void Check_keyboard_CheckedChanged(object sender, EventArgs e)
        {
            Graphics graphics = Graphics.FromImage(SelectPosition_Designer.BackgroundImage);
            Touch ftouch = Touch.GetSingle();
            Image image = null;
            int x0, y0, x1, y1, showX = 0, showY = 0;
            switch (ButName)
            {
                case "DataInput_DisplayLocationSelect":
                    if(ftouch.DataInput_KeyBoardPosition.SelectedIndex == 1 && ftouch.DataInput_KeyBoardAtPage.Value >= 0)
                    {
                        //第一步：获取图片要截取的图片
                        image = Images_Form.picname[(int)ftouch.DataInput_KeyBoardAtPage.Value].image;
                        //第二步：获取图片的坐标
                        Image image1 = image.Clone() as Image;
                        string[] str = ftouch.DataInput_KeyboardLeftup.Text.Split(',');
                        x0 = Convert.ToUInt16(str[0]);
                        y0 = Convert.ToUInt16(str[1]);
                        str = ftouch.DataInput_KeyboardRightDown.Text.Split(',');
                        x1 = Convert.ToUInt16(str[0]);
                        y1 = Convert.ToUInt16(str[1]);
                        str = ftouch.DataInput_KeyboardShowLocation.Text.Split(',');
                        showX = Convert.ToUInt16(str[0]);
                        showY = Convert.ToUInt16(str[1]);
                        //第三步：根据坐标取出图片
                        bitmap = GetPart(image1, 0,0,x1 - x0,y1 - y0,x0,y0);
                        backgroundbitmap = GetPart(backimage, 0, 0, x1 - x0, y1 - y0, showX, showY);
                    }
                    break;
                case "GBK_InputDisplayAreaSet":
                case "GBK_PinyinDisplayPointSet":
                    if (ftouch.GBK_KeyBoardPosition.SelectedIndex == 1 && ftouch.GBK_KeyBoardAtPage.Value >= 0)
                    {
                        //第一步：获取图片要截取的图片
                        image = Images_Form.picname[(int)ftouch.GBK_KeyBoardAtPage.Value].image;
                        //第二步：获取图片的坐标
                        Image image1 = image.Clone() as Image;
                        string[] str = ftouch.GBK_KeyboardLeftup.Text.Split(',');
                        x0 = Convert.ToUInt16(str[0]);
                        y0 = Convert.ToUInt16(str[1]);
                        str = ftouch.GBK_KeyboardRightDown.Text.Split(',');
                        x1 = Convert.ToUInt16(str[0]);
                        y1 = Convert.ToUInt16(str[1]);
                        str = ftouch.GBK_KeyboardShowLocation.Text.Split(',');
                        showX = Convert.ToUInt16(str[0]);
                        showY = Convert.ToUInt16(str[1]);
                        //第三步：根据坐标取出图片
                        bitmap = GetPart(image1, 0, 0, x1 - x0, y1 - y0, x0, y0);
                        backgroundbitmap = GetPart(backimage, 0, 0, x1 - x0, y1 - y0, showX, showY);
                    }
                    break;
                case "ASCII_InputDisplayAreaSet":
                    if (ftouch.ASCII_KeyBoardPosition.SelectedIndex == 1 && ftouch.ASCII_KeyBoardAtPage.Value >= 0)
                    {
                        //第一步：获取图片要截取的图片
                        image = Images_Form.picname[(int)ftouch.ASCII_KeyBoardAtPage.Value].image;
                        //第二步：获取图片的坐标
                        Image image1 = image.Clone() as Image;
                        string[] str = ftouch.ASCII_KeyboardLeftup.Text.Split(',');
                        x0 = Convert.ToUInt16(str[0]);
                        y0 = Convert.ToUInt16(str[1]);
                        str = ftouch.ASCII_KeyboardRightDown.Text.Split(',');
                        x1 = Convert.ToUInt16(str[0]);
                        y1 = Convert.ToUInt16(str[1]);
                        str = ftouch.ASCII_KeyboardShowLocation.Text.Split(',');
                        showX = Convert.ToUInt16(str[0]);
                        showY = Convert.ToUInt16(str[1]);
                        //第三步：根据坐标取出图片
                        bitmap = GetPart(image1, 0, 0, x1 - x0, y1 - y0, x0, y0);
                        backgroundbitmap = GetPart(backimage, 0, 0, x1 - x0, y1 - y0, showX, showY);
                    }
                    break;
                case "RTCset_Locationset":
                case "RTCset_KeyboardPointaSet":
                    if (ftouch.RTCset_KeyBoardAtPage.Value >= 0)
                    {
                        //第一步：获取图片要截取的图片
                        image = Images_Form.picname[(int)ftouch.RTCset_KeyBoardAtPage.Value].image;
                        //第二步：获取图片的坐标
                        Image image1 = image.Clone() as Image;
                        string[] str = ftouch.RTCset_KeyArea_Left.Text.Split(',');
                        x0 = Convert.ToUInt16(str[0]);
                        y0 = Convert.ToUInt16(str[1]);
                        str = ftouch.RTCset_KeyBoardRight.Text.Split(',');
                        x1 = Convert.ToUInt16(str[0]);
                        y1 = Convert.ToUInt16(str[1]);
                        str = ftouch.RTCset_DisplayPoint.Text.Split(',');
                        showX = Convert.ToUInt16(str[0]);
                        showY = Convert.ToUInt16(str[1]);
                        //第三步：根据坐标取出图片
                        bitmap = GetPart(image1, 0, 0, x1 - x0, y1 - y0, x0, y0);
                        backgroundbitmap = GetPart(backimage, 0, 0, x1 - x0, y1 - y0, showX, showY);
                    }
                    break;
            }
            //第四步：绘制图片
            if (Check_keyboard.Checked == true)
            {
                if(bitmap != null)
                    graphics.DrawImage(bitmap, new Point(showX, showY));
            }
            else
            {
                if(backgroundbitmap != null)
                    graphics.DrawImage(backgroundbitmap, new Point(showX, showY));
            }
            SelectPosition_Designer.Refresh();
        }
        /// <summary>
        /// 截取图片
        /// </summary>
        /// <param name="image">要进行截图的原始图片</param>
        /// <param name="pPartStartPointX">目标图片开始绘制处的坐标X值(通常为0)</param>
        /// <param name="pPartStartPointY">目标图片开始绘制处的坐标Y值(通常为0)</param>
        /// <param name="pPartWidth">目标图片的宽度</param>
        /// <param name="pPartHeight">目标图片的高度</param>
        /// <param name="pOrigStartPointX">原始图片开始截取处的坐标X值</param>
        /// <param name="pOrigStartPointY">原始图片开始截取处的坐标Y值</param>
        /// <returns></returns>
        static Bitmap GetPart(Image image, int pPartStartPointX, int pPartStartPointY, int pPartWidth, int pPartHeight, int pOrigStartPointX, int pOrigStartPointY)
        {
            Image originalImg = image;
            Bitmap partImg = new Bitmap(pPartWidth, pPartHeight);
            Graphics graphics = Graphics.FromImage(partImg);
            Rectangle destRect = new Rectangle(new Point(pPartStartPointX, pPartStartPointY), new Size(pPartWidth, pPartHeight));//目标位置
            Rectangle origRect = new Rectangle(new Point(pOrigStartPointX, pOrigStartPointY), new Size(pPartWidth, pPartHeight));//原图位置（默认从原图中截取的图片大小等于目标图片的大小）
            graphics.DrawImage(originalImg, destRect, origRect, GraphicsUnit.Pixel);
        
            return partImg;
        }
        private void Image_Area_Setting_Load(object sender, EventArgs e)
        {
            if (ButName == "DataInput_DisplayLocationSelect"
                || ButName == "GBK_InputDisplayAreaSet"
                || ButName == "GBK_PinyinDisplayPointSet"
                || ButName == "ASCII_InputDisplayAreaSet"
                || ButName == "RTCset_Locationset"
                || ButName == "RTCset_KeyboardPointaSet")
            {
                Check_keyboard.Visible = true;
            }
            else
            {
                Check_keyboard.Visible = false;
            }
        }
        public void ShowPic(int width,int height,Image image)
        {
            Image ima = image.Clone() as Image;
            backimage = image.Clone() as Image;
            this.SelectPosition_Designer.Size = new System.Drawing.Size(width, height);
            this.SelectPosition_Designer.BackgroundImageLayout = ImageLayout.Stretch;
            this.SelectPosition_Designer.BackgroundImage = ima;
            Designer.DrawMode = Designer.Mode.Selection; 
        }

        private void Image_Area_Setting_FormClosing(object sender, FormClosingEventArgs e)
        {
            Designer.DrawMode = Designer.Mode.Draw;  //这个值自动维护，不需要手动修改
            ButName = string.Empty;                  //这个值在需要的时候进行设置
            imageareasettingsingle = null;
            Check_keyboard.Checked = false;
        }
        public void ShowPositionInformation(Rectangle rec)
        {
            Lab_Xs.Text = rec.X.ToString();
            Lab_Ys.Text = rec.Y.ToString();
            Lab_Xe.Text = (rec.X + rec.Width).ToString();
            Lab_Ye.Text = (rec.Y + rec.Height).ToString();
            Touch ftouch = Touch.GetSingle();
            switch(ButName)
            {
                case "DataInput_DisplayLocationSelect":
                    ftouch.DataInput_KeyBoardLocation.Clear();
                    ftouch.DataInput_KeyBoardLocation.AppendText(rec.X.ToString("D4"));
                    ftouch.DataInput_KeyBoardLocation.AppendText(rec.Y.ToString("D4"));
                    ftouch.DataInput_KeyBoardLocation.Refresh();
                    break;
                case "DataInput_KeyBoardSet":
                    ftouch.DataInput_KeyboardLeftup.Clear();
                    ftouch.DataInput_KeyboardLeftup.AppendText(rec.X.ToString("D4"));
                    ftouch.DataInput_KeyboardLeftup.AppendText(rec.Y.ToString("D4"));
                    ftouch.DataInput_KeyboardLeftup.Refresh();
                    ftouch.DataInput_KeyboardRightDown.Clear();
                    ftouch.DataInput_KeyboardRightDown.AppendText((rec.X + rec.Width).ToString("D4"));
                    ftouch.DataInput_KeyboardRightDown.AppendText((rec.Y + rec.Height).ToString("D4"));
                    ftouch.DataInput_KeyboardRightDown.Refresh();
                    break;
                case "DataInput_KeyboardShowLocationSet":
                    ftouch.DataInput_KeyboardShowLocation.Clear();
                    ftouch.DataInput_KeyboardShowLocation.AppendText(rec.X.ToString("D4"));
                    ftouch.DataInput_KeyboardShowLocation.AppendText(rec.Y.ToString("D4"));
                    ftouch.DataInput_KeyboardShowLocation.Refresh();
                    break;
                case "PopupMenu_MenuSet":
                    ftouch.PopupMenu_MenuLeftUp.Clear();
                    ftouch.PopupMenu_MenuLeftUp.AppendText(rec.X.ToString("D4"));
                    ftouch.PopupMenu_MenuLeftUp.AppendText(rec.Y.ToString("D4"));
                    ftouch.PopupMenu_MenuLeftUp.Refresh();
                    ftouch.PopupMenu_MenuRightDown.Clear();
                    ftouch.PopupMenu_MenuRightDown.AppendText((rec.X + rec.Width).ToString("D4"));
                    ftouch.PopupMenu_MenuRightDown.AppendText((rec.Y + rec.Height).ToString("D4"));
                    ftouch.PopupMenu_MenuRightDown.Refresh();
                    break;
                case "PopupMenu_MenuPositionSet":
                    ftouch.PopupMenu_MenuDisPosition.Clear();
                    ftouch.PopupMenu_MenuDisPosition.AppendText(rec.X.ToString("D4"));
                    ftouch.PopupMenu_MenuDisPosition.AppendText(rec.Y.ToString("D4"));
                    ftouch.PopupMenu_MenuDisPosition.Refresh();
                    break;
                case "IconRotation_IconIDSelect":
                    ftouch.IconRotation_Xc.Value = rec.X;
                    ftouch.IconRotation_Yc.Value = rec.Y;
                    break;
                case "ClockDisplay_HourAddButton":
                    ftouch.ClockDisplay_HourPosition.Clear();
                    ftouch.ClockDisplay_HourPosition.AppendText(rec.X.ToString("D4"));
                    ftouch.ClockDisplay_HourPosition.AppendText(rec.Y.ToString("D4"));
                    ftouch.ClockDisplay_HourPosition.Refresh();
                    break;
                case "ClockDisplay_MinuteAddButton":
                    ftouch.ClockDisplay_MinutePosition.Clear();
                    ftouch.ClockDisplay_MinutePosition.AppendText(rec.X.ToString("D4"));
                    ftouch.ClockDisplay_MinutePosition.AppendText(rec.Y.ToString("D4"));
                    ftouch.ClockDisplay_MinutePosition.Refresh();
                    break;
                case "ClockDisplay_SecAddButton":
                    ftouch.ClockDisplay_SecondPosition.Clear();
                    ftouch.ClockDisplay_SecondPosition.AppendText(rec.X.ToString("D4"));
                    ftouch.ClockDisplay_SecondPosition.AppendText(rec.Y.ToString("D4"));
                    ftouch.ClockDisplay_SecondPosition.Refresh();
                    break;
                case "GBK_KeyBoardPic":
                case "GBK_KeyBoardSet":
                    ftouch.GBK_KeyboardLeftup.Clear();
                    ftouch.GBK_KeyboardLeftup.AppendText(rec.X.ToString("D4"));
                    ftouch.GBK_KeyboardLeftup.AppendText(rec.Y.ToString("D4"));
                    ftouch.GBK_KeyboardLeftup.Refresh();
                    ftouch.GBK_KeyboardRightDown.Clear();
                    ftouch.GBK_KeyboardRightDown.AppendText((rec.X + rec.Width).ToString("D4"));
                    ftouch.GBK_KeyboardRightDown.AppendText((rec.Y + rec.Height).ToString("D4"));
                    ftouch.GBK_KeyboardRightDown.Refresh();
                    break;
                case "GBK_KeyboardShowLocationSet":
                    ftouch.GBK_KeyboardShowLocation.Clear();
                    ftouch.GBK_KeyboardShowLocation.AppendText(rec.X.ToString("D4"));
                    ftouch.GBK_KeyboardShowLocation.AppendText(rec.Y.ToString("D4"));
                    ftouch.GBK_KeyboardShowLocation.Refresh();
                    break;
                case "GBK_InputDisplayAreaSet":
                    ftouch.GBK_InputDisplayAreaLeft.Clear();
                    ftouch.GBK_InputDisplayAreaLeft.AppendText(rec.X.ToString("D4"));
                    ftouch.GBK_InputDisplayAreaLeft.AppendText(rec.Y.ToString("D4"));
                    ftouch.GBK_InputDisplayAreaLeft.Refresh();
                    ftouch.GBK_InputDisplayAreaRight.Clear();
                    ftouch.GBK_InputDisplayAreaRight.AppendText((rec.X + rec.Width).ToString("D4"));
                    ftouch.GBK_InputDisplayAreaRight.AppendText((rec.Y + rec.Height).ToString("D4"));
                    ftouch.GBK_InputDisplayAreaRight.Refresh();
                    break;
                case "GBK_PinyinDisplayPointSet":
                    ftouch.GBK_PinyinDisplayPoint.Clear();
                    ftouch.GBK_PinyinDisplayPoint.AppendText(rec.X.ToString("D4"));
                    ftouch.GBK_PinyinDisplayPoint.AppendText(rec.Y.ToString("D4"));
                    ftouch.GBK_PinyinDisplayPoint.Refresh();
                    break;
                case "ASCII_InputDisplayAreaSet":
                    ftouch.ASCII_InputDisplayAreaLeft.Clear();
                    ftouch.ASCII_InputDisplayAreaLeft.AppendText(rec.X.ToString("D4"));
                    ftouch.ASCII_InputDisplayAreaLeft.AppendText(rec.Y.ToString("D4"));
                    ftouch.ASCII_InputDisplayAreaLeft.Refresh();
                    ftouch.ASCII_InputDisplayAreaRight.Clear();
                    ftouch.ASCII_InputDisplayAreaRight.AppendText((rec.X + rec.Width).ToString("D4"));
                    ftouch.ASCII_InputDisplayAreaRight.AppendText((rec.Y + rec.Height).ToString("D4"));
                    ftouch.ASCII_InputDisplayAreaRight.Refresh();
                    break;
                case "ASCII_KeyBoardPic":
                case "ASCII_KeyBoardSet":
                    ftouch.ASCII_KeyboardLeftup.Clear();
                    ftouch.ASCII_KeyboardLeftup.AppendText(rec.X.ToString("D4"));
                    ftouch.ASCII_KeyboardLeftup.AppendText(rec.Y.ToString("D4"));
                    ftouch.ASCII_KeyboardLeftup.Refresh();
                    ftouch.ASCII_KeyboardRightDown.Clear();
                    ftouch.ASCII_KeyboardRightDown.AppendText((rec.X + rec.Width).ToString("D4"));
                    ftouch.ASCII_KeyboardRightDown.AppendText((rec.Y + rec.Height).ToString("D4"));
                    ftouch.ASCII_KeyboardRightDown.Refresh();
                    break;
                case "ASCII_KeyboardShowLocationSet":
                    ftouch.ASCII_KeyboardShowLocation.Clear();
                    ftouch.ASCII_KeyboardShowLocation.AppendText(rec.X.ToString("D4"));
                    ftouch.ASCII_KeyboardShowLocation.AppendText(rec.Y.ToString("D4"));
                    ftouch.ASCII_KeyboardShowLocation.Refresh();
                    break;
                case "RTCset_Locationset":
                    ftouch.RTCset_Location.Clear();
                    ftouch.RTCset_Location.AppendText((rec.X + rec.Width).ToString("D4"));
                    ftouch.RTCset_Location.AppendText(rec.Y.ToString("D4"));
                    ftouch.RTCset_Location.Refresh();
                    break;
                case "RTCset_KeyBoardSet":
                case "RTCset_KeyBoardPic":
                    ftouch.RTCset_KeyArea_Left.Clear();
                    ftouch.RTCset_KeyArea_Left.AppendText(rec.X.ToString("D4"));
                    ftouch.RTCset_KeyArea_Left.AppendText(rec.Y.ToString("D4"));
                    ftouch.RTCset_KeyArea_Left.Refresh();
                    ftouch.RTCset_KeyBoardRight.Clear();
                    ftouch.RTCset_KeyBoardRight.AppendText((rec.X + rec.Width).ToString("D4"));
                    ftouch.RTCset_KeyBoardRight.AppendText((rec.Y + rec.Height).ToString("D4"));
                    ftouch.RTCset_KeyBoardRight.Refresh();
                    break;
                case "RTCset_KeyboardPointaSet":
                    ftouch.RTCset_DisplayPoint.Clear();
                    ftouch.RTCset_DisplayPoint.AppendText(rec.X.ToString("D4"));
                    ftouch.RTCset_DisplayPoint.AppendText(rec.Y.ToString("D4"));
                    ftouch.RTCset_DisplayPoint.Refresh();
                    break;
            }
            
        }
        private void Button_Confirm_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
        }
    }   

}
