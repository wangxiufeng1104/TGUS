﻿using System;
using System.Runtime.Serialization;
using System.Drawing;
using System.Windows.Forms;

namespace TGUS
{
    [Serializable()]
    public class ItemRectangle : ItemBase, ISerializable ,IComparable<ItemRectangle>
    {
        private Rectangle rectangle;
       
        public Rectangle Rectangle
        {
            get
            {
                return GetNormalizedRectangle(rectangle);
            }
            set
            {
                rectangle = GetNormalizedRectangle(value);
            }
        }
        public int CompareTo(ItemRectangle comparePart)
        {
            // A null value means that this object is greater.
            if (comparePart == null)
                return 1;
            else
            {
                return VP.CompareTo(comparePart.VP);
            }
        }
        public override int GetHashCode()
        {
            return VP;
        }
        public bool Equals(ItemRectangle other)
        {
            if (other == null) return false;
            return (VP.Equals(other.VP));
        }

        public ItemRectangle()   //DrawRectangle
        {
            rectangle = new Rectangle(0, 0, 1, 1);
            //IconImage = GetIconImage("rectangle.png");
        }

        public override string Name
        {
            get { return "矩形"; }
        }
        public ItemRectangle(int x, int y, int w, int h)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = w;
            rectangle.Height = h;
        }
        public override void Draw(Graphics g,string Name_Ddefine)
        {
            using (SolidBrush sb = new SolidBrush(FillColor))
            {
                g.FillRectangle(sb, rectangle);
                try
                {
                    g.DrawString(Name_Ddefine, new Font("Times New Roman", (((int)Math.Sqrt(Math.Pow(rectangle.Height, 2) + Math.Pow(rectangle.Width, 2))) + 16) / 16), 
                                                        Brushes.Black, rectangle);
                }
                catch
                {
                    g.DrawString(Name_Ddefine, new Font(FontFamily.GenericSerif, (((int)Math.Sqrt(Math.Pow(rectangle.Height, 2) + Math.Pow(rectangle.Width, 2))) + 16) / 16),
                                                        Brushes.Black, rectangle);
                }  
            }         
        }
        public  void Draw(Graphics g,Image image,Point offset,bool Isfixed)
        {
            Rectangle rec = new Rectangle();
            rec.Location = rectangle.Location;
            rec.X -= offset.X;
            rec.Y -= offset.Y;
            if (rec.X < 0) rec.X = 0;
            if (rec.Y < 0) rec.Y = 0;
            
            rec.Width = image.Width;
            rec.Height = image.Height;
            if(Isfixed == true)
            {
                rectangle.Width = rec.Width;
                rectangle.Height = rec.Height;
            }
            g.DrawImage(image, rec);
        }

        /// <summary>
        /// 设置对像的大小
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        protected void SetRectangle(int x, int y, int width, int height)
        {
            rectangle.X = x;
            rectangle.Y = y;
            rectangle.Width = width;
            rectangle.Height = height;
        }

        /// <summary>
        /// 获取手柄数
        /// </summary>
        public override int HandleCount
        {
            get { return 8; }
        }

        /// <summary>
        /// 获取手柄所在的坐标
        /// 1--2--3
        /// |     |
        /// 4     5
        /// |     |
        /// 6--7--8
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Point GetHandle(int handleNumber)
        {
            int x, y, xCenter, yCenter;
            xCenter = rectangle.X + rectangle.Width / 2;
            yCenter = rectangle.Y + rectangle.Height / 2;
            x = rectangle.X;
            y = rectangle.Y;
            switch (handleNumber)
            {
                case 2:
                    x = xCenter;
                    break;
                case 3:
                    x = rectangle.Right;
                    break;
                case 4:
                    y = yCenter;
                    break;
                case 5:
                    x = rectangle.Right;
                    y = yCenter;
                    break;
                case 6:
                    y = rectangle.Bottom;
                    break;
                case 7:
                    x = xCenter;
                    y = rectangle.Bottom;
                    break;
                case 8:
                    x = rectangle.Right;
                    y = rectangle.Bottom;
                    break;
            }

            return new Point(x, y);
        }

        /// <summary>
        /// 测试鼠标的当前位置有没有在对像上
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public override int HitTest(Point point)
        {
            if (Selected)
            {
                //是否在对像的手柄上
                for (int i = 1; i <= HandleCount; i++)
                {
                    if (GetHandleRectangle(i).Contains(point))
                    {
                        return i;
                    }
                }
            }
            //在对像上但不是在手柄上
            if (PointInObject(point))
            {
                return 0;
            }
            //不在对像上
            return -1;
        }

        /// <summary>
        /// 检查点是否在矩形上
        /// </summary>
        /// <param name="point">要检查的点</param>
        /// <returns></returns>
        protected override bool PointInObject(Point point)
        {
            return rectangle.Contains(point);
        }

        /// <summary>
        /// 获取鼠标把在手柄的光标
        /// 1--2--3
        /// |     |
        /// 4     5
        /// |     |
        /// 6--7--8
        /// </summary>
        /// <param name="handleNumber"></param>
        /// <returns></returns>
        public override Cursor GetHandleCursor(int handleNumber)
        {
            Cursor cursor = Cursors.Default;
            switch (handleNumber)
            {
                case 1:
                    cursor = Cursors.SizeNWSE;
                    break;
                case 2:
                    cursor = Cursors.SizeNS;
                    break;
                case 3:
                    cursor = Cursors.SizeNESW;
                    break;
                case 4:
                    cursor = Cursors.SizeWE;
                    break;
                case 5:
                    cursor = Cursors.SizeWE;
                    break;
                case 6:
                    cursor = Cursors.SizeNESW;
                    break;
                case 7:
                    cursor = Cursors.SizeNS;
                    break;
                case 8:
                    cursor = Cursors.SizeNWSE;
                    break;
            }
            return cursor;
        }

        /// <summary>
        /// 改变对像的大小
        /// 1--2--3
        /// |     |
        /// 4     5
        /// |     |
        /// 6--7--8
        /// </summary>
        /// <param name="point"></param>
        /// <param name="handleIndex"></param>
        public override void MoveHandleTo(Point point, int handleIndex)
        {
            int left = rectangle.Left, right = rectangle.Right, top = rectangle.Top, bottom = rectangle.Bottom;
        
            switch (handleIndex)
            {
                case 1:
                    left = point.X;
                    top = point.Y;
                    break;
                case 2:
                    top = point.Y;
                    break;
                case 3:
                    right = point.X;
                    top = point.Y;
                    break;
                case 4:
                    left = point.X;
                    break;
                case 5:
                    right = point.X;
                    break;
                case 6:
                    left = point.X;
                    bottom = point.Y;
                    break;
                case 7:
                    bottom = point.Y;
                    break;
                case 8:
                    right = point.X;
                    bottom = point.Y;
                    break;
            }
            if(right <= (left + 10))
            {
                right = left + 10;
            }
            if(bottom <= (top + 10))
            {
                bottom = top + 10;
            }
            SetRectangle(left, top, right - left, bottom - top);
            //Console.WriteLine("left = {0},top = {1},width = {2},hight = {3}", left, top, right - left, bottom - top);
        }

        /// <summary>
        /// 选择的矩形是否包含对像区域
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public override bool IntersectsWith(Rectangle selectRect)
        {
            return selectRect.Contains(rectangle);
        }
        /// <summary>
        /// 移动对像到坐标点上
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(int deltaX, int deltaY)
        {
            rectangle.X += deltaX;
            rectangle.Y += deltaY;
        }
        /// <summary>
        /// 移动对像到坐标点上
        /// </summary>
        /// <param name="deltaX"></param>
        /// <param name="deltaY"></param>
        public override void Move(int deltaX, int deltaY,Designer designer)
        {   
            if((rectangle.X + deltaX + rectangle.Width)>designer.Width)//防止出右边界
            {
                rectangle.X += (designer.Width - rectangle.X - rectangle.Width);
            }
            else if((rectangle.X + deltaX) < 0)  //防止出左边界
            {
                rectangle.X = 0;
            }
            else
            {
                rectangle.X += deltaX;
            }
            if((rectangle.Y + deltaY + rectangle.Height)>designer.Height)//防止出下边界
            {
                rectangle.Y += (designer.Height - rectangle.Y - rectangle.Height);
            }
            else if((rectangle.Y + deltaY) < 0) //防止出上边界
            {
                rectangle.Y = 0;
            }
            else
            {
                rectangle.Y += deltaY;
            }
        }

        public override void Normalize()
        {
            GetNormalizedRectangle(rectangle);

        }

        public static Rectangle GetNormalizedRectangle(int x1, int y1, int x2, int y2)
        {
            if (x2 < x1)
            {
                int tmp = x2;
                x2 = x1;
                x1 = tmp;
            }

            if (y2 < y1)
            {
                int tmp = y2;
                y2 = y1;
                y1 = tmp;
            }

            return new Rectangle(x1, y1, x2 - x1, y2 - y1);
        }

        public static Rectangle GetNormalizedRectangle(Point p1, Point p2)
        {
            return GetNormalizedRectangle(p1.X, p1.Y, p2.X, p2.Y);
        }

        public static Rectangle GetNormalizedRectangle(Rectangle r)
        {
            return GetNormalizedRectangle(r.X, r.Y, r.X + r.Width, r.Y + r.Height);
        }


        #region ISerializable Members

        public void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            info.AddValue("Rectangle", this.rectangle);
            info.AddValue("PenWidth", this.LineWidth);
            info.AddValue("LineColor", this.LineColor);
            info.AddValue("FillColor", this.FillColor);

        }
        public ItemRectangle(SerializationInfo info, StreamingContext context)
            : this()
        {
            this.rectangle = (Rectangle)info.GetValue("Rectangle", typeof(Rectangle));
            this.LineWidth = info.GetInt32("PenWidth");
            this.LineColor = (Color)info.GetValue("LineColor", typeof(Color));
            this.FillColor = (Color)info.GetValue("FillColor", typeof(Color));
        }

        #endregion
    }
}
