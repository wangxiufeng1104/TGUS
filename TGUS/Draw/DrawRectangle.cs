using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using System.Drawing;

namespace WindowsFormsApplication2
{
    sealed class DrawRectangle : DrawBase
    {
        private int x, y;
        public DrawRectangle()
        {
            //rectangle = new Rectangle(0, 0, 1, 1);
        }

       

        public override void OnMouseDown(Designer designer, MouseEventArgs e)
        {
            AddNewObject(designer, new ItemRectangle(e.X, e.Y, 1, 1));
            x = e.X;
            y = e.Y;
        }

        public override void OnMouseMove(Designer designer, MouseEventArgs e)
        {
            //designer.Cursor = ToolCursor;
            if (e.Button == MouseButtons.Left)
            {
                /*根据鼠标当前的坐标点与起始坐标点
                 * 确定移动移动的方向获取不同的手柄
                 *1--2--3
                 *|     |                 
                 *4     5
                 *|     |
                 *6--7--8
                 */
                Point point = new Point(e.X, e.Y);
                int handle = 8;
                if (x > e.X && y > e.Y)
                {
                    handle = 1;
                }
                else if (x > e.X && y < e.Y)
                {
                    handle = 6;
                }
                else if (x < e.X && y < e.Y)
                {
                    handle = 8;
                }
                else if (x < e.X && y > e.Y)
                {
                    handle = 3;
                }

                designer.Items[0].MoveHandleTo(point, handle);
                designer.Refresh();
                //designer.SelectedItem(designer.Items[0]);
            }
        }
    }
}
