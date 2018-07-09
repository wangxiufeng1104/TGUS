using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Data;

namespace TGUS
{
    [Serializable()]
    public class ItemList : List<ItemRectangle>
    {
        public ItemList()
        {
        }


        //将对像插入列表
        public new void Add(ItemRectangle itemBase)
        {

            base.Insert(0, itemBase);

        }

        public void Draw(Graphics g)
        {
            for (int i = this.Count - 1; i > -1; i--)
            {
                if (this[i].presentpage_num == Main_Form.presentpage_num)//判断是否为当前页，是则画出图形，否则不画出
                {
                    this[i].visibility = true;
                    if (Designer.DrawMode == Designer.Mode.Selection && i == 0)
                    {
                        this[i].Draw(g, "");
                    }
                    else
                    {
                        Bitmap bitmap = null;
                        Point offset = new Point(0,0);
                        bool Isfixed = true;
                        switch (this[i].ControlType)
                        {
                            case PIC_Obj.icon_display:
                                if(this[i].IconVarInformation.Icon_MaxPic!=null)
                                {
                                    bitmap = new Bitmap(this[i].IconVarInformation.Icon_MaxPic);
                                    if(this[i].IconVarInformation.Icon_IsMaxTransparent)
                                    {
                                        bitmap.MakeTransparent();
                                    }
                                }
                                break;
                            case PIC_Obj.aniicon_display:
                                if (this[i].ActionIconInforamtion.Icon_StartPic != null)
                                {
                                    bitmap = new Bitmap(this[i].ActionIconInforamtion.Icon_StartPic);
                                    if(this[i].ActionIconInforamtion.ISIcon_Start)
                                    {
                                        bitmap.MakeTransparent();
                                    }
                                }
                                break;
                            case PIC_Obj.artfont:
                                if(this[i].ArtFontInformation.Icon_Pic != null)
                                {
                                    bitmap = new Bitmap(this[i].ArtFontInformation.Icon_Pic);
                                    if(this[i].ArtFontInformation.Icon_IsTransparent)
                                    {
                                        bitmap.MakeTransparent();
                                    }
                                }
                                break;
                            case PIC_Obj.slidis:
                                if (this[i].SlideDisplayInformation.Icon_Pic != null)
                                {
                                    bitmap = new Bitmap(this[i].SlideDisplayInformation.Icon_Pic);
                                    if (this[i].SlideDisplayInformation.Icon_IsTransparent)
                                    {
                                        bitmap.MakeTransparent();
                                    }
                                }
                                break;
                            case PIC_Obj.iconrota:
                                if(this[i].IconRotationInformation.Icon_Pic != null)
                                {
                                    bitmap = new Bitmap(this[i].IconRotationInformation.Icon_Pic);
                                    Isfixed = false;
                                    offset.X = this[i].IconRotationInformation.Icon_Xc;
                                    offset.Y = this[i].IconRotationInformation.Icon_Yc;
                                    if (this[i].IconRotationInformation.Icon_IsTransparent)
                                    {
                                        bitmap.MakeTransparent();
                                    }
                                }
                                break;
                            case PIC_Obj.clockdisplay:
                                int Left = 0, Right = 0, Top = 0, Bottom = 0,width = 0,height = 0;
                                if (this[i].ClockDisplayInformation.Icon_HourPic != null)
                                {
                                    offset.X = this[i].ClockDisplayInformation.Icon_Hour_Central_X;
                                    offset.Y = this[i].ClockDisplayInformation.Icon_Hour_Central_Y;
                                    Left = (offset.X > Left) ? (offset.X) : (Left);
                                    Top = (offset.Y > Top) ? (offset.Y) : (Top);
                                    Right = ((this[i].ClockDisplayInformation.Icon_HourPic.Width - offset.X) > Right) ? ((this[i].ClockDisplayInformation.Icon_HourPic.Width - offset.X)) : (Right);
                                    Bottom = ((this[i].ClockDisplayInformation.Icon_HourPic.Height - offset.Y) > Bottom) ? ((this[i].ClockDisplayInformation.Icon_HourPic.Height - offset.Y)) : (Bottom);
                                }
                                if (this[i].ClockDisplayInformation.Icon_MinutePic != null)
                                {
                                    offset.X = this[i].ClockDisplayInformation.Icon_Minute_Central_X;
                                    offset.Y = this[i].ClockDisplayInformation.Icon_Minute_Central_Y;
                                    Top = (offset.Y > Top) ? (offset.Y) : (Top);
                                    Left = (offset.X > Left) ? (offset.X) : (Left);
                                    Right = ((this[i].ClockDisplayInformation.Icon_MinutePic.Width - offset.X) > Right) ? ((this[i].ClockDisplayInformation.Icon_MinutePic.Width - offset.X)) : (Right);
                                    Bottom = ((this[i].ClockDisplayInformation.Icon_MinutePic.Height - offset.Y) > Bottom) ? ((this[i].ClockDisplayInformation.Icon_MinutePic.Height - offset.Y)) : (Bottom);
                                }
                                if (this[i].ClockDisplayInformation.Icon_SecondPic != null)
                                {
                                    offset.X = this[i].ClockDisplayInformation.Icon_Second_Central_X;
                                    offset.Y = this[i].ClockDisplayInformation.Icon_Second_Central_Y;
                                    Left = (offset.X > Left) ? (offset.X) : (Left);
                                    Top = (offset.Y > Top) ? (offset.Y) : (Top);
                                    Right = ((this[i].ClockDisplayInformation.Icon_SecondPic.Width - offset.X) > Right) ? ((this[i].ClockDisplayInformation.Icon_SecondPic.Width - offset.X)) : (Right);
                                    Bottom = ((this[i].ClockDisplayInformation.Icon_SecondPic.Height - offset.Y) > Bottom) ? ((this[i].ClockDisplayInformation.Icon_SecondPic.Height - offset.Y)) : (Bottom);
                                }
                                width = Right + Left;
                                height = Top + Bottom;
                                if(width != 0 && height != 0)
                                {
                                    bitmap = new Bitmap(width, height);
                                    bitmap.MakeTransparent();
                                }
                                //Graphics graphics = Graphics.FromImage(bitmap);
                                
                                Bitmap tempBitmap = null;
                                if (this[i].ClockDisplayInformation.Icon_HourPic != null)
                                {

                                    tempBitmap = new Bitmap(this[i].ClockDisplayInformation.Icon_HourPic);
                                    tempBitmap.MakeTransparent();
                                    
                                    for (int ii = 0; ii < this[i].ClockDisplayInformation.Icon_HourPic.Height; ii++)
                                    {
                                        for (int jj = 0; jj < this[i].ClockDisplayInformation.Icon_HourPic.Width; jj++)
                                        {
                                            if (tempBitmap.GetPixel(jj, ii) != Color.FromArgb(0,0,0,0))
                                            {
                                                //Console.WriteLine($"tempBitmap.GetPixel({jj}, {ii}) = {tempBitmap.GetPixel(jj, ii)}\n");
                                                bitmap.SetPixel(Left - this[i].ClockDisplayInformation.Icon_Hour_Central_X + jj, Top - this[i].ClockDisplayInformation.Icon_Hour_Central_Y + ii, tempBitmap.GetPixel(jj, ii));
                                            }
                                        }
                                    }
                                }
                                if (this[i].ClockDisplayInformation.Icon_MinutePic != null)
                                {
                                    tempBitmap = new Bitmap(this[i].ClockDisplayInformation.Icon_MinutePic);
                                    tempBitmap.MakeTransparent();
                                    for (int ii = 0; ii < this[i].ClockDisplayInformation.Icon_MinutePic.Height; ii++)
                                    {
                                        for (int jj = 0; jj < this[i].ClockDisplayInformation.Icon_MinutePic.Width; jj++)
                                        {
                                            if (tempBitmap.GetPixel(jj, ii) != Color.FromArgb(0, 0, 0, 0))
                                            {
                                                bitmap.SetPixel(Left - this[i].ClockDisplayInformation.Icon_Minute_Central_X + jj, Top - this[i].ClockDisplayInformation.Icon_Minute_Central_Y + ii, tempBitmap.GetPixel(jj, ii));
                                            }
                                        }
                                    }
                                }
                                if (this[i].ClockDisplayInformation.Icon_SecondPic != null)
                                {
                                    tempBitmap = new Bitmap(this[i].ClockDisplayInformation.Icon_SecondPic);
                                    tempBitmap.MakeTransparent();
                                    for (int ii = 0; ii < this[i].ClockDisplayInformation.Icon_SecondPic.Height; ii++)
                                    {
                                        for (int jj = 0; jj < this[i].ClockDisplayInformation.Icon_SecondPic.Width; jj++)
                                        {
                                            if (tempBitmap.GetPixel(jj, ii) != Color.FromArgb(0, 0, 0, 0))
                                            {
                                                
                                                bitmap.SetPixel(Left - this[i].ClockDisplayInformation.Icon_Second_Central_X + jj, Top - this[i].ClockDisplayInformation.Icon_Second_Central_Y + ii, tempBitmap.GetPixel(jj, ii));
                                            }
                                        }
                                    }
                                }
                                offset.X = Left;
                                offset.Y = Top;
                                Isfixed = false;
                                break;
                        }
                        if(bitmap != null)
                            this[i].Draw(g, bitmap,offset,Isfixed);
                        this[i].Draw(g, this[i].Name_define);
                    }
                    //如果对像是选中状态,则画出手柄
                    if (this[i].Selected)
                    {
                        this[i].DrawTracker(g);
                    }
                }
                else
                {
                    this[i].Selected = false;
                    this[i].visibility = false;
                }
            }
        }
        /// <summary>
        /// 获取对像列中选定的对像
        /// </summary>
        /// <param name="index">选中的是第几个</param>
        /// <returns></returns>
        public ItemRectangle GetSelectItem(int index)
        {
            int i = -1;

            foreach (ItemRectangle o in this)
            {
                if (o.visibility == true)
                {
                    if (o.Selected)
                    {
                        i++;
                        if (i == index)
                            return o;
                    }
                }
            }

            return null;
        }

        /// <summary>
        /// 选择所有对像
        /// </summary>
        public void SelectAll()
        {
            foreach (ItemRectangle item in this)
            {
                item.Selected = true;
            }
        }

        /// <summary>
        /// 将所选中第一个对像放到最后
        /// </summary>
        /// <param name="selectindex"></param>
        public void SendBlow()
        {
            if (this.Count > 1)
            {
                ItemRectangle dib = GetSelectItem(0);
                this.Remove(dib);
                this.Insert(this.Count, dib);
            }
        }
        /// <summary>
        /// 将选中的第一个对像放到最前面
        /// </summary>
        /// <param name="selectindex"></param>
        public void SendFront()
        {
            if (this.Count > 1)
            {
                ItemRectangle dib = GetSelectItem(0);
                this.Remove(dib);
                this.Insert(0, dib);
            }

        }

        /// <summary>
        /// 取消选择所有对像
        /// </summary>
        public void UnSelectAll()
        {
            foreach (ItemRectangle item in this)
            {
                item.Selected = false;
            }
        }

        /// <summary>
        /// 选择矩形中的对像
        /// </summary>
        /// <param name="rect"></param>
        public void SelectInRectangle(Rectangle rect)
        {
            foreach (ItemRectangle item in this)
            {
                if (item.visibility == true)
                {
                    if (item.IntersectsWith(rect))
                    {
                        item.Selected = true;
                    }
                }
            }
        }
        /// <summary>
        /// 获取有多少个选中的对像
        /// </summary>
        public int SelectionCount
        {
            get
            {
                int n = 0;

                foreach (ItemRectangle o in this)
                {
                    if (o.Selected)
                        n++;
                }

                return n;
            }
        }

        /// <summary>
        /// 获取选中对像列表中第一个对像在列表中的索引号
        /// </summary>
        public int GetSelectedIndex
        {
            get
            {
                int n = -1;
                foreach (ItemRectangle o in this)
                {
                    n++;
                    if (o.Selected)
                    {
                        break;
                    }
                }
                return n;
            }
        }

    }
}
