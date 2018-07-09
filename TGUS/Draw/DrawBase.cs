using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication2
{
    public class DrawBase
    {
        public DrawBase()
        {
            Console.WriteLine("我是base");
        }
        protected void AddNewObject(Designer designer, ItemBase drawitem)
        {
            //在设计器对像列表中加入新的对像
            designer.Items.Add(drawitem);
            //将原来选中的对像设计为未选中
            designer.Items.UnSelectAll();
            //设置新添加的对像为选中状态
            drawitem.Selected = true;

            designer.Capture = true;
            designer.Refresh();

            //designer.ChangeFlage = true;
        }
        public virtual void OnMouseDown(Designer designer, MouseEventArgs e)
        {
        }

        public virtual void OnMouseMove(Designer designer, MouseEventArgs e)
        {
        }

        public virtual void OnMouseUp(Designer designer, MouseEventArgs e)
        {

        }
    }

}
