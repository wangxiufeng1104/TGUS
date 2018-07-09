using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace TGUS
{
    public partial class button_pic : UserControl
    {
        public Color back_color
        {
            set
            {
                buttonpic.BackColor = value;
                Imagename.BackColor = value;
                Imagenumber.BackColor = value;
                this.BackColor = value;
            }
        }
        public PictureBox buttonpic = new PictureBox();
        public string safename;
        private Label Imagenumber = new Label();
        private Label Imagename = new Label();
        private bool click_fg = false;
        public int index = 0;
            
        public button_pic(Image ima, string imagename, int imageindex)
        {
            InitializeComponent();
            buttonpic.Parent = this;
            Imagenumber.Parent = this;
            Imagename.Parent = this;
            this.Height = 128;
            this.Width = 110;
            buttonpic.Image = ima;
            this.index = imageindex;

            buttonpic.SizeMode = PictureBoxSizeMode.Zoom;
            buttonpic.Width = 100;
            buttonpic.Height = 60;
            buttonpic.Top = 18;
            buttonpic.Left = 5;
            Imagenumber.Text = (index).ToString();
            safename = imagename;


            Imagenumber.Top = 80;
            Imagenumber.Left = 5;
            Imagenumber.Width = 100;
            Imagenumber.TextAlign = ContentAlignment.MiddleCenter;

            Imagename.Top = 95;
            Imagename.Left = 5;
            Imagename.Width = 100;
            Imagename.TextAlign = ContentAlignment.MiddleCenter;
            Imagename.Text = imagename;

            this.Controls.Add(buttonpic);
            this.Controls.Add(Imagename);
            this.Controls.Add(Imagenumber);
            this.MouseEnter += new EventHandler(button_pic_MouseEnter);
            this.MouseLeave += new EventHandler(button_pic_MouseLeave);
            this.Imagenumber.MouseEnter += new EventHandler(button_pic_MouseEnter);
            this.Imagename.MouseEnter += new EventHandler(button_pic_MouseEnter);
            this.buttonpic.MouseEnter += new EventHandler(button_pic_MouseEnter);
            this.MouseClick += new MouseEventHandler(button_pic_MouseClick);
            this.Imagenumber.MouseClick += new MouseEventHandler(button_pic_MouseClick);
            this.Imagename.MouseClick += new MouseEventHandler(button_pic_MouseClick);
            this.buttonpic.MouseClick += new MouseEventHandler(button_pic_MouseClick);

        }
        void button_pic_MouseClick(object sender, MouseEventArgs e)
        {
            this.click_fg = true;
            this.back_color = Color.FromArgb(60, Color.SkyBlue);
            ChoicePicture_Form.button_picreflash(index);
        }
        void button_pic_MouseLeave(object sender, EventArgs e)
        {
            if (click_fg == false)
                this.back_color = Color.White;
        }
        void button_pic_MouseEnter(object sender, EventArgs e)
        {
            if (click_fg == false)
                this.back_color = Color.FromArgb(120, Color.SkyBlue);
        }
      
    }
}
