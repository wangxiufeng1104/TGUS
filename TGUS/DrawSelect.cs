using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;
namespace TGUS
{
    public sealed class DrawSelect : DrawBase
    {
        private enum SelectionMode
        {
            None,
            NetSelection,//选择一个区域
            Move,//移动对像
            Size//改变对像尺寸
        }
        private SelectionMode selectmode = SelectionMode.None;
        /// <summary>
        ///  改变大小的对像
        /// </summary>
        private ItemBase resizeObject;
        /// <summary>
        /// 使用的是哪个手柄
        /// </summary>
        private int resizeHandle;
        /// <summary>
        /// 移动开始坐标和最后坐标
        /// </summary>
        private Point lastPoint = new Point(0, 0), startPoint = new Point(0, 0);
        public DrawSelect()
        {
            

        }
        public override void OnMouseDown(Designer designer, MouseEventArgs e)
        {
            selectmode = SelectionMode.None;
            Point point = new Point(e.X, e.Y);
            int selectCount = designer.Items.SelectionCount;
            //如果光标在后柄上,则模式为改变对像的大小
            for (int i = 0; i < selectCount; i++)
            {
                //找到要修改的对像
                ItemBase item = designer.Items.GetSelectItem(i);
                if (item == null)
                    continue;
                int handleNumber = item.HitTest(point);
                if (handleNumber > 0)
                {
                    selectmode = SelectionMode.Size;
                    resizeHandle = handleNumber;
                    resizeObject = item;
                    designer.Items.UnSelectAll();
                    resizeObject.Selected = true;
                    break;
                }
            }

            //如果没有选中对像手柄,则检查是否在对像上
            if (selectmode == SelectionMode.None)
            {
                int itemCount = designer.Items.Count;
                ItemBase item = null;
                for (int i = 0; i < itemCount; i++)
                {
                    if(designer.Items[i].visibility == true)
                    {
                        if (designer.Items[i].HitTest(point) == 0)
                        {
                            item = designer.Items[i];
                            selectmode = SelectionMode.Move;
                            designer.Items.UnSelectAll();
                            item.Selected = true;
                            Main_Form.SelectType = item.ControlType;
                            switch (Main_Form.SelectType)
                            {
                                case PIC_Obj.data_display:
                                case PIC_Obj.icon_display:
                                    Main_Form.VarType = Main_Form.myGraphicsType.other;
                                    break;
                                case PIC_Obj.basictouch:
                                case PIC_Obj.datainput:
                                case PIC_Obj.keyreturn:
                                case PIC_Obj.menu_display:
                                case PIC_Obj.increadj:
                                case PIC_Obj.sliadj:
                                case PIC_Obj.GBK:
                                case PIC_Obj.ASCII:
                                case PIC_Obj.TouchState:
                                case PIC_Obj.RTC_Set:
                                    Main_Form.VarType = Main_Form.myGraphicsType.touch;
                                    break;
                                case PIC_Obj.artfont:
                                case PIC_Obj.slidis:
                                case PIC_Obj.iconrota:
                                case PIC_Obj.clockdisplay:
                                case PIC_Obj.text_dispaly:
                                case PIC_Obj.rtc_display:
                                case PIC_Obj.QR_display:
                                case PIC_Obj.aniicon_display:
                                case PIC_Obj.BasicGra:
                                    Main_Form.VarType = Main_Form.myGraphicsType.variable;
                                    break;
                            }
                            designer.Cursor = Cursors.SizeAll;
                            break;
                        }
                    } 
                }
            }
            // 如果没有选中对像,则是进行区域选择
            if (selectmode == SelectionMode.None)
            {
                selectmode = SelectionMode.NetSelection;
                designer.Items.UnSelectAll();
                designer.IsDrawSelectRectangle = true;
            }
            lastPoint.X = startPoint.X = e.X;
            lastPoint.Y = startPoint.Y = e.Y;
            designer.Capture = true;
            designer.SelectRectangle = GetNormalizedRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y);
            //designer.Refresh();
        }

        public override void OnMouseMove(Designer designer, MouseEventArgs e)  
        {


            int x1, y1;
            if(e.X > designer.Width)
            {
                x1 = designer.Width;
            }
            else if(e.X < 0)
            {
                x1 = 0;
            }
            else
            {
                x1 = e.X;
            }
            if(e.Y > designer.Height)
            {
                y1 = designer.Height;
            }
            else if(e.Y < 0)
            {
                y1 = 0;

            }

            else
            {
                y1 = e.Y;
            }

            //Point point = new Point(e.X, e.Y);
            Point point = new Point(x1, y1);
            //如果是移动鼠标,改变鼠标的指针
            if (e.Button == MouseButtons.None)
            {
                Cursor cursor = Cursors.Default;
                
                for (int i = 0; i < designer.Items.Count; i++)
                {
                    int n = designer.Items[i].HitTest(point);
                    if (n > 0)
                    {
                        cursor = designer.Items[i].GetHandleCursor(n);
                        break;
                    }
                }
                designer.Cursor = cursor;
            }
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.X - lastPoint.X;
                int dy = e.Y - lastPoint.Y;
                //Console.WriteLine("dx = {0},dy = {1}",dx,dy);
                lastPoint.X = e.X;
                lastPoint.Y = e.Y;
               
                //改变对像的尺寸
                if (selectmode == SelectionMode.Size)
                {
                    resizeObject.MoveHandleTo(point, resizeHandle);
                    //designer.ChangeFlage = true;
                    designer.Refresh();
                    //designer.SelectedItem(resizeObject);
                }
                //移动选中的对像
                if (selectmode == SelectionMode.Move)
                {
                    int n = designer.Items.SelectionCount;
                    for (int i = 0; i < n; i++)
                    {
                      
                        designer.Items.GetSelectItem(i).Move(dx, dy, designer);
 
                    }

                    designer.Cursor = Cursors.SizeAll;
                    //designer.ChangeFlage = true;
                    designer.Refresh();
                }
                // 区域选择
                if (selectmode == SelectionMode.NetSelection)
                {
                    designer.SelectRectangle = GetNormalizedRectangle(startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y);
                    //Console.WriteLine("s.X = {0},s.Y = {1},l.X = {2},l.Y = {3}", startPoint.X, startPoint.Y, lastPoint.X, lastPoint.Y);
                    designer.Refresh();
                }
            }
        }

        public override void OnMouseUp(Designer designer, MouseEventArgs e)
        {
           
            if (selectmode == SelectionMode.NetSelection)
            {
                designer.Items.SelectInRectangle(designer.SelectRectangle);
                selectmode = SelectionMode.None;
                designer.IsDrawSelectRectangle = false;
            }
            //如果是改变大小则结束改变
            if (resizeObject != null)
            {
                resizeObject.Normalize();
                resizeObject = null;
            }
            designer.Capture = false;
            designer.Refresh();
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
    }

}
