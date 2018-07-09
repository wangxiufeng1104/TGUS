using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
namespace TGUS
{
    [Serializable()]
    public partial class Designer : UserControl
    {
        private Point mouseXs = Point.Empty, mouseXe = Point.Empty, mouseYs = Point.Empty, mouseYe = Point.Empty;

        private Point mouseStartPos = Point.Empty;
        /// <summary>
        /// 没有控件的选择矩形
        /// </summary>
        private Rectangle selectRectangle;
        private Point lastPoint = new Point(0, 0), startPoint = new Point(0, 0);
        private int cursor_x;
        private int cursor_y;
        public Touch fTouch = null;   //指向Touch引用
        public static bool ChangeFlag;//界面刷新的标志
        public enum Mode
        {
            Draw,
            Selection
        }
        /// <summary>
        /// 切换画图和选择键盘的时候使用
        /// 画图：DrawMode = Mode.Draw
        /// 选择键盘：DrawMoe = Mode.Selrction
        /// </summary>
        public static Mode DrawMode = Mode.Draw;
        
        /// <summary>
        /// 获取系统默认的鼠标工作区域
        /// </summary>
        //private static Rectangle tempCursorPos = Cursor.Clip;
        private Rectangle tempCursorPos = Cursor.Clip;
        //设计器中所包含的对像
        private ItemList _items;
        //选择的坐标的时候使用的集合
        private ItemList _itemSelection;
        /// <summary>
        /// 对像集合
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public ItemList Items
        {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
            }
        }
        public ItemList ItemSelection
        {
            get
            {
                return _itemSelection;
            }
            set
            {
                _itemSelection = value;
            }
        }
        public Rectangle  SelectRectangle
        {
            get
            {
                return selectRectangle;
            }
            set
            {
                selectRectangle = value;
            }
        }
        //是否画选择区域
        private bool isdrawSelectRectangel = false;
        /// <summary>
        /// 是否画选择区域
        /// </summary>
        public bool IsDrawSelectRectangle
        {
            get
            {
                return isdrawSelectRectangel;
            }
            set
            {
                isdrawSelectRectangel = value;
            }
        }
        //是否画出鼠标的位置
        private bool isdrawMousePosition = true;
        /// <summary>
        /// 是否画出鼠标在设计器的当前位置
        /// </summary>
        public bool IsDrawMousePosition
        {
            get
            {
                return isdrawMousePosition;
            }
            set
            {
                isdrawMousePosition = value;
            }
        }
        private DrawBase activeControl;//通过这个来调用抽象类DrawBase的派生类DrawSelect和DrawRectangle
        public DrawBase ActiveControl
        {
            get
            {
                return activeControl;
            }
            set
            {
                activeControl = value;
                //换了控件则取清空起始位置
                mouseStartPos = Point.Empty;
            }
        }
        public Designer()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.ResizeRedraw, true);
            this.BorderStyle = BorderStyle.FixedSingle;
            InitializeComponent();
            activeControl = new DrawBase();
            _items = new ItemList();
            fTouch = Touch.GetSingle();
        }
        private void designer_Load(object sender, EventArgs e)
        {
            
        }
        private void Designer_MouseDown(object sender, MouseEventArgs e)
        {
           // Console.WriteLine(Main_Form.SelectType);
            if (e.Button == MouseButtons.Left && activeControl != null)
            {
                activeControl.OnMouseDown(this, e);

            }
            if (isdrawMousePosition)
            {
                //拖拽时记录起始位置
                mouseStartPos = new Point(e.X, e.Y);
                mouseXs = new Point(0, e.Y);
                mouseXe = new Point(e.X, e.Y);
                mouseYs = new Point(e.X, 0);
                mouseYe = new Point(e.X, e.Y);
            }
        }
        public MouseEventHandler DesignerMouseMove;
        private void Designer_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button == MouseButtons.Left || e.Button == MouseButtons.None) && activeControl != null)
            {
                activeControl.OnMouseMove(this, e);
            }

            if (DesignerMouseMove != null)
            {
                DesignerMouseMove(sender, e);
            }
            if (isdrawMousePosition)
            {

                mouseXs = new Point(0, e.Y);
                mouseXe = new Point(e.X, e.Y);
                mouseYs = new Point(e.X, 0);
                mouseYe = new Point(e.X, e.Y);
                //lastPoint.X = e.X;
                //lastPoint.Y = e.Y;
                if (cursor_x != e.X || cursor_y != e.Y)
                {
                    toolTip1.SetToolTip(this, Main_Form.StrMouseName + " " + e.X + "," + e.Y);
                    cursor_x = e.X;
                    cursor_y = e.Y;
                }
                this.Refresh();
            }
        }
        private void Designer_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left && ActiveControl != null)
            {
                ActiveControl.OnMouseUp(this, e);
                //恢复鼠标的工作区域
                Cursor.Clip = tempCursorPos;
                //拖拽完成后设置这个值为空,程序将不会在画出鼠标坐标线
                mouseXs = Point.Empty;
                activeControl = new DrawSelect();//这里把类型重新绑定为选择
            }
            if(DrawMode == Mode.Draw)
            {
                updateinfo();
            }
            else
            {
                if(Image_Area_Setting.imageareasettingsingle != null)
                {
                    Image_Area_Setting imageArea = Image_Area_Setting.GetSingle();
                    imageArea.ShowPositionInformation(Items[0].Rectangle);
                }
            }
            Main_Form.StrMouseName = "";
        }
        private void updateinfo()
        {
            if (Items.SelectionCount == 1)
            {
                foreach (ItemRectangle list in Items)
                {
                    if (list.Selected)
                    {
                        list.times++;
                        //Console.WriteLine("list.times = {0}",list.times);
                        switch(list.ControlType)
                        {
                            #region case basictouch
                            case PIC_Obj.basictouch:
                                //fTouch.BaseTouch_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.BaseTouch_X.Value = list.Rectangle.X;
                                //fTouch.BaseTouch_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.BaseTouch_Y.Value = list.Rectangle.Y;
                                //fTouch.BaseTouch_W.Maximum = ((this.Width - list.Rectangle.X) < 0)?(this.Width):();
                                fTouch.BaseTouch_W.Value = list.Rectangle.Width;
                                //fTouch.BaseTouch_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.BaseTouch_H.Value = list.Rectangle.Height;
                                //名称定义
                                fTouch.BaseTouch_NameDefine.Text = list.Name_define;
                                ///按键效果
                                fTouch.BaseTouch_ButtonEffectnum.Maximum = Images_Form.Pic_Number;
                                if (list.BaseTouchInfo.ButtonEffect_Image != null)
                                {
                                    fTouch.BaseTouch_ButtonEffectPicture.Image = list.BaseTouchInfo.ButtonEffect_Image;
                                }
                                else
                                {
                                   
                                    fTouch.BaseTouch_ButtonEffectPicture.Image = null;
                                  
                                }
                                if (((list.BaseTouchInfo.Pic_On + 1) & 0xFF00) == 0)
                                {
                                    fTouch.BaseTouch_ButtonEffectCheckBox.Checked = false;
                                    fTouch.BaseTouch_ButtonEffectnum.Value = list.BaseTouchInfo.Pic_On;
                                }
                                else
                                {
                                    fTouch.BaseTouch_ButtonEffectnum.Value = -1;
                                }
                                ///切换界面
                                fTouch.BaseTouch_ButtonChangePageNum.Maximum = Images_Form.Pic_Number;
                                if (list.BaseTouchInfo.PageChange_Image != null)
                                {
                                    fTouch.BaseTouch_ButtonChangePagePic.Image = list.BaseTouchInfo.PageChange_Image;
                                }
                                else
                                {
                                    fTouch.BaseTouch_ButtonChangePagePic.Image = null;
                                }
                                if (((list.BaseTouchInfo.Pic_Next + 1) & 0xFF00) == 0)
                                {
                                    fTouch.BaseTouch_ButtonChangePageCheckBox.Checked = false;
                                    fTouch.BaseTouch_ButtonChangePageNum.Value = list.BaseTouchInfo.Pic_Next;
                                }
                                else
                                {
                                    fTouch.BaseTouch_ButtonChangePageNum.Value = -1;
                                }
                                
                                //键值定义
                                fTouch.BaseTouch_KeyValueCheck.Checked = list.BaseTouchInfo.IsKey_Value;
                                fTouch.BaseTouch_KeyValueSetButton.Visible = list.BaseTouchInfo.IsKey_Value;
                                fTouch.BaseTouch_KeyValueSet.Text = list.BaseTouchInfo.TP_Code.ToString("X4");
                                break;
                            #endregion
                            #region case data_display
                            case PIC_Obj.data_display:
                                //fTouch.DataVar_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.DataVar_X.Value = list.Rectangle.X;
                                //fTouch.DataVar_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.DataVar_Y.Value = list.Rectangle.Y;
                                //fTouch.DataVar_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.DataVar_W.Value = list.Rectangle.Width;
                               // fTouch.DataVar_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.DataVar_H.Value = list.Rectangle.Height;
                                fTouch.DataVar_NameDefine.Text = list.Name_define;
                                fTouch.DataVar_DescripPoint.Text = list.SP.ToString("X4");
                                fTouch.DataVar_VarAdress.Text = list.VP.ToString("X4");
                                fTouch.DataVar_DisplayColor.Text = (((list.DataVarInfo.Display_Color.R >> 3) << 11) 
                                                                              | ((list.DataVarInfo.Display_Color.G >> 2) << 5)
                                                                              | ((list.DataVarInfo.Display_Color.B >> 3))).ToString("X4");
                                fTouch.DataVar_DispalyColorPic.BackColor = list.DataVarInfo.Display_Color;
                                fTouch.DataVar_FontLib.Text = list.DataVarInfo.Lib_ID.ToString();
                                //字号
                                fTouch.DataVar_FontSize.Value = list.DataVarInfo.Font_Size;
                                //对齐方式
                                fTouch.DataVar_AlignStyle.SelectedIndex = list.DataVarInfo.Font_Align;
                                //变量类型
                                fTouch.DataVar_VarType.SelectedIndex = list.DataVarInfo.Var_Type;
                                fTouch.DataVar_Integer_Length.Value = list.DataVarInfo.Integer_Length;
                                fTouch.DataVar_Decimal_Length.Value = list.DataVarInfo.Decimal_Length;
                                fTouch.DataVar_Unit_Length.Value = list.DataVarInfo.Len_unit;
                                fTouch.DataVar_Display_Unit.Text = list.DataVarInfo.String_Uint;
                                fTouch.DataVar_InitialValue.Value = list.DataVarInfo.Initial_Value;
                                break;
                            #endregion
                            #region case icon_display
                            case PIC_Obj.icon_display:
                                //fTouch.Iconvar_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.Iconvar_X.Value = list.Rectangle.X;
                                //fTouch.Iconvar_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.Iconvar_Y.Value = list.Rectangle.Y;
                                //fTouch.Iconvar_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.Iconvar_W.Value = list.Rectangle.Width;
                                //fTouch.Iconvar_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.Iconvar_H.Value = list.Rectangle.Height;
                                //名称定义
                                fTouch.Iconvar_NameDefine.Text = list.Name_define;
                                fTouch.Iconvar_DescripPointer.Text = list.SP.ToString("X4");
                                fTouch.Iconvar_VarAdress.Text = list.VP.ToString("X4");
                                if(list.IconVarInformation.Icon_FileName != "" && !fTouch.Iconvar_IconFile.Items.Contains(list.IconVarInformation.Icon_FileName))
                                {
                                    fTouch.Iconvar_IconFile.Items.Add(list.IconVarInformation.Icon_FileName);
                                    fTouch.Iconvar_IconFile.Text = list.IconVarInformation.Icon_FileName;
                                }
                                else if(fTouch.Iconvar_IconFile.Items.Count != 0 && list.IconVarInformation.Icon_FileName != "")
                                {
                                    if (list.IconVarInformation.Iconfileselect != -1)
                                    {
                                        //fTouch.Iconvar_IconFile.SelectedIndex = list.IconVarInformation.Iconfileselect;
                                        fTouch.Iconvar_IconFile.Text = list.IconVarInformation.Icon_FileName;
                                    }
                                    else
                                    {
                                        fTouch.Iconvar_IconFile.Items.Clear();
                                    }
                                    fTouch.Iconvar_IconFile.Refresh();
                                }
                                else if(list.IconVarInformation.Icon_FileName == "")
                                {
                                    fTouch.Iconvar_IconFile.Items.Clear();
                                    fTouch.Iconvar_IconFile.Refresh();
                                }
                               
                                fTouch.Iconvar_VarMax.Text = list.IconVarInformation.V_Max.ToString();
                                if(list.IconVarInformation.Icon_MaxPic != null)
                                {
                                    fTouch.Iconvar_VarMaxPic.Image = list.IconVarInformation.Icon_MaxPic;
                                }
                                else
                                {
                                    fTouch.Iconvar_VarMaxPic.Image = null;
                                }
                                fTouch.Iconvar_VarMaxNum.Value = list.IconVarInformation.Icon_Max;
                                fTouch.Iconvar_VarMin.Text = list.IconVarInformation.V_Min.ToString();
                                if(list.IconVarInformation.Icon_MinPic != null)
                                {
                                    fTouch.Iconvar_VarMinPic.Image = list.IconVarInformation.Icon_MinPic;
                                }
                                else
                                {
                                    fTouch.Iconvar_VarMinPic.Image = null;
                                }
                                fTouch.Iconvar_VarMinNum.Value = list.IconVarInformation.Icon_Min;
                                fTouch.Iconvar_Mode.SelectedIndex = list.IconVarInformation.Mode;
                                fTouch.Iconvar_InitialValue.Text = list.IconVarInformation.InitialValue.ToString();
                                break;
                            #endregion
                            #region case text_dispaly
                            case PIC_Obj.text_dispaly:
                                //fTouch.TxtDisplay_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.TxtDisplay_X.Value = list.Rectangle.X;
                                //fTouch.TxtDisplay_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.TxtDisplay_Y.Value = list.Rectangle.Y;
                                //fTouch.TxtDisplay_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.TxtDisplay_W.Value = list.Rectangle.Width;
                                //fTouch.TxtDisplay_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.TxtDisplay_H.Value = list.Rectangle.Height;
                                fTouch.TxtDisplay_NameDefine.Text = list.Name_define;
                                fTouch.TxtDisplay_DescripPoint.Text = list.SP.ToString("X4");
                                fTouch.TxtDisplay_VarAdress.Text = list.VP.ToString("X4");
                                fTouch.TxtDisplay_ShowColor.Text = (((list.TextDisplayInformation.Display_Color.R >> 3) << 11)
                                                                   | ((list.TextDisplayInformation.Display_Color.G >> 2) << 5)
                                                                   | ((list.TextDisplayInformation.Display_Color.B >> 3))).ToString("X4");
                                fTouch.TxtDisplay_ShowColorPic.BackColor = list.TextDisplayInformation.Display_Color;
                                fTouch.TxtDisplay_CharacterNotAdj.Checked = list.TextDisplayInformation.IsCharacternotadj;
                                fTouch.TxtDisplay_CodeingMode.SelectedIndex = list.TextDisplayInformation.Encode_Mode & 0x7F;
                                fTouch.TxtDisplay_CharacterNotAdj.Checked = ((list.TextDisplayInformation.Encode_Mode & 0x80) != 0);
                                fTouch.TxtDisplay_TxtLength.Value = list.TextDisplayInformation.Text_length;
                                fTouch.TxtDisplay_Font0_ID.Value = list.TextDisplayInformation.Font0_ID;
                                fTouch.TxtDisplay_Font1_ID.Value = list.TextDisplayInformation.Font1_ID;
                                fTouch.TxtDisplay_Xdirectionnum.Value = list.TextDisplayInformation.Font_X_Dots;
                                fTouch.TxtDisplay_Ydirectionnum.Value = list.TextDisplayInformation.Font_Y_Dots;
                                fTouch.TxtDisplay_Horispacing.Value = list.TextDisplayInformation.HOR_Dis;
                                fTouch.TxtDisplay_verticalspacing.Value = list.TextDisplayInformation.VER_Dis;
                                fTouch.TxtDisplay_Initvalue.Text = list.TextDisplayInformation.initial_value;
                                break;
                            #endregion
                            #region case rtc_display
                            
                            case PIC_Obj.rtc_display:
                                //fTouch.RTC_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.RTC_X.Value = list.Rectangle.X;
                                //fTouch.RTC_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.RTC_Y.Value = list.Rectangle.Y;
                                //fTouch.RTC_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.RTC_W.Value = list.Rectangle.Width;
                                //fTouch.RTC_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.RTC_H.Value = list.Rectangle.Height;
                                fTouch.RTC_NameDefien.Text = list.Name_define;
                                fTouch.RTC_DescripPointer.Text = list.SP.ToString("X4");
                                fTouch.RTC_DisplayColor.Text = (((list.RTCDisplayInformatin.Display_Color.R >> 3) << 11)
                                                                   | ((list.RTCDisplayInformatin.Display_Color.G >> 2) << 5)
                                                                   | ((list.RTCDisplayInformatin.Display_Color.B >> 3))).ToString("X4");
                                fTouch.RTC_DisplayColorPic.BackColor = list.RTCDisplayInformatin.Display_Color;
                                fTouch.RTC_FontLib.Value = list.RTCDisplayInformatin.Lib_ID;
                                fTouch.RTC_Xdirectionnum.Value = list.RTCDisplayInformatin.Font_X_Dots;
                                fTouch.RTC_DataFormat.Text = list.RTCDisplayInformatin.String_Code;
                                break;
                            #endregion
                            #region case datainput
                            case PIC_Obj.datainput:
                                //fTouch.DataInput_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.DataInput_X.Value = list.Rectangle.X;
                                //fTouch.DataInput_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.DataInput_Y.Value = list.Rectangle.Y;
                                //fTouch.DataInput_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.DataInput_W.Value = list.Rectangle.Width;
                                //fTouch.DataInput_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.DataInput_H.Value = list.Rectangle.Height;

                                fTouch.DataInput_Namedefine.Text = list.Name_define;
                                fTouch.DataInput_DataAutoUpload.Checked = list.DataInputInformation.IsDataAutoUpLoad;
                                if (((list.DataInputInformation.Pic_On + 1) & 0xFF00) == 0)
                                {
                                    fTouch.DataInput_ButtonEffectCheck.Checked = false;
                                    fTouch.DataInput_ButtonEffectNum.Value = list.DataInputInformation.Pic_On;
                                }
                                else
                                {
                                    fTouch.DataInput_ButtonEffectNum.Value = -1;
                                }
                                fTouch.DataInput_ButtonEffectPic.Image = list.DataInputInformation.ButtonEffectPic;
                                if (((list.DataInputInformation.Pic_Next + 1) & 0xFF00) == 0)
                                {
                                    fTouch.DataInput_PageChangeCheck.Checked = false;
                                    fTouch.DataInput_PageChangeNum.Value = list.DataInputInformation.Pic_Next;
                                }
                                else
                                {
                                    fTouch.DataInput_PageChangeNum.Value = -1;
                                }
                                fTouch.DataInput_PageChangePic.Image = list.DataInputInformation.ButtonChangePagePic;
                                fTouch.DataInput_VarAdress.Text = list.VP.ToString("X4");
                                fTouch.DataInput_VarType.SelectedIndex = list.DataInputInformation.V_Type;
                                fTouch.DataInput_IntrgerLength.Value = list.DataInputInformation.N_Int;
                                fTouch.DataInput_DecimalLength.Value = list.DataInputInformation.N_Dot;
                                fTouch.DataInput_KeyBoardLocation.Clear();
                                fTouch.DataInput_KeyBoardLocation.AppendText(list.DataInputInformation.KeyShowPosition_X.ToString("D4"));
                                fTouch.DataInput_KeyBoardLocation.AppendText(list.DataInputInformation.KeyShowPosition_Y.ToString("D4"));
                                fTouch.DataInput_DisplayColorPic.BackColor = list.DataInputInformation.Display_Color; 
                                fTouch.DataInput_DisplayColor.Text = (((list.DataInputInformation.Display_Color.R >> 3) << 11)
                                                                   | ((list.DataInputInformation.Display_Color.G >> 2) << 5)
                                                                   | ((list.DataInputInformation.Display_Color.B >> 3))).ToString("X4");
                                fTouch.DataInput_FontID.Value = list.DataInputInformation.Lib_ID;
                                fTouch.DataInput_FontSize.Value = list.DataInputInformation.Font_Hor;
                                fTouch.DataInput_CursorColor.SelectedIndex = list.DataInputInformation.CurousColor;
                                fTouch.DataInput_DisplayStyle.SelectedIndex = list.DataInputInformation.Hide_En;
                                fTouch.DataInput_KeyBoardPosition.SelectedIndex = list.DataInputInformation.KB_Source;
                                fTouch.DataInput_KeyBoardAtPage.Value = list.DataInputInformation.PIC_KB;
                                fTouch.DataInput_KeyBoardPic.Image = list.DataInputInformation.KeyBoardPic;
                                fTouch.DataInput_KeyboardLeftup.Clear();
                                fTouch.DataInput_KeyboardLeftup.AppendText(list.DataInputInformation.AREA_KB_Xs.ToString("D4"));
                                fTouch.DataInput_KeyboardLeftup.AppendText(list.DataInputInformation.AREA_KB_Ys.ToString("D4"));
                                fTouch.DataInput_KeyboardRightDown.Clear();
                                fTouch.DataInput_KeyboardRightDown.AppendText(list.DataInputInformation.AREA_KB_Xe.ToString("D4"));
                                fTouch.DataInput_KeyboardRightDown.AppendText(list.DataInputInformation.AREA_KB_Ye.ToString("D4"));
                                fTouch.DataInput_KeyboardShowLocation.Clear();
                                fTouch.DataInput_KeyboardShowLocation.AppendText(list.DataInputInformation.AREA_KB_Posation_X.ToString("D4"));
                                fTouch.DataInput_KeyboardShowLocation.AppendText(list.DataInputInformation.AREA_KB_Posation_Y.ToString("D4"));
                                fTouch.DataInput_DataLimiteCheck.Checked = (list.DataInputInformation.Limits_En == 0xFF)?(true):(false);
                                fTouch.DataInput_LimitedMin.Value = list.DataInputInformation.V_min;
                                fTouch.DataInput_LimitedMax.Value = list.DataInputInformation.V_max;
                                fTouch.DataInput_InputLoadDataCheck.Checked = (list.DataInputInformation.Return_Set == 0x5A) ? (true) : (false);
                                fTouch.DataInput_InputVarAddress.Text = list.DataInputInformation.Return_VP.ToString("X4");
                                fTouch.DataInput_InputLoadData.Value = list.DataInputInformation.Return_DATA;
                                break;
                            #endregion
                            #region case key_return
                            case PIC_Obj.keyreturn:
                                //fTouch.KeyRetuen_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.KeyRetuen_X.Value = list.Rectangle.X;
                                //fTouch.KeyRetuen_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.KeyRetuen_Y.Value = list.Rectangle.Y;
                                //fTouch.KeyRetuen_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.KeyRetuen_W.Value = list.Rectangle.Width;
                                //fTouch.KeyRetuen_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.KeyRetuen_H.Value = list.Rectangle.Height;
                                fTouch.KeyReturn_NameDefine.Text = list.Name_define;
                                fTouch.KeyReturn_ButtonEffrctPic.Image = list.KeyReturnInformation.ButtonEffectPic;
                                if (((list.KeyReturnInformation.Pic_On + 1) & 0xFF00) == 0)
                                {
                                    fTouch.KeyReturn_ButtonEffrctCheck.Checked = false;
                                    fTouch.KeyReturn_ButtonEffrctNum.Value = list.KeyReturnInformation.Pic_On;
                                }
                                else
                                    fTouch.KeyReturn_ButtonEffrctNum.Value = -1;
                                fTouch.KeyReturn_ButtonChangePagePic.Image = list.KeyReturnInformation.ButtonChangePagePic;
                                if (((list.KeyReturnInformation.Pic_Next + 1) & 0xFF00) == 0)
                                {
                                    fTouch.KeyReturn_ButtonChangePageCheck.Checked = false;
                                    fTouch.KeyReturn_ButtonChangePageNum.Value = list.KeyReturnInformation.Pic_Next;
                                }
                                else
                                    fTouch.KeyReturn_ButtonChangePageNum.Value = -1;
                                fTouch.KeyReturn_KeyValue.Text = list.KeyReturnInformation.Key_Code.ToString("X4");
                                fTouch.KeyReturn_TouchKeyValue.Text = list.KeyReturnInformation.Touch_Key_Code.ToString("X4");
                                fTouch.KeyReturn_KeeppressingText.Text = list.KeyReturnInformation.Touch_KeyPressing_Code.ToString("X4");
                                fTouch.KeyReturn_VarAddress.Text = list.VP.ToString("X4");
                                if (list.KeyReturnInformation.VP_Mode <= 2)
                                {
                                    fTouch.KeyReturn_CheckList.SetItemChecked(list.KeyReturnInformation.VP_Mode, true);
                                    fTouch.Lab_WriteBit.Visible = fTouch.KeyReturn_BitNum.Visible = false;
                                }
                                else
                                {
                                    fTouch.KeyReturn_CheckList.SetItemChecked(3,true);
                                    fTouch.KeyReturn_BitNum.Value = (Byte)(list.KeyReturnInformation.VP_Mode & 0x0F);
                                    fTouch.Lab_WriteBit.Visible = fTouch.KeyReturn_BitNum.Visible = true;
                                }
                                break;
                            #endregion
                            #region case QR_code
                            case PIC_Obj.QR_display:
                                //fTouch.QR_Code_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.QR_Code_X.Value = list.Rectangle.X;
                                //fTouch.QR_Code_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.QR_Code_Y.Value = list.Rectangle.Y;
                                //fTouch.QR_Code_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.QR_Code_W.Value = list.Rectangle.Width;
                                //fTouch.QR_Code_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.QR_Code_H.Value = list.Rectangle.Height;

                                fTouch.QR_Code_NameDefine.Text = list.Name_define;
                                fTouch.QR_Code_DescripPoint.Text = list.SP.ToString("X4");
                                fTouch.QR_Code_VarAddress.Text = list.VP.ToString("X4");
                                fTouch.QR_Code_Unit_Pixels.Value = list.QRCodeInformation.Unit_Pixels;
                                break;
                            #endregion
                            #region case PopupMenu
                            case PIC_Obj.menu_display:
                                //fTouch.PopupMenu_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.PopupMenu_X.Value = list.Rectangle.X;
                                //fTouch.PopupMenu_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.PopupMenu_Y.Value = list.Rectangle.Y;
                                //fTouch.PopupMenu_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.PopupMenu_W.Value = list.Rectangle.Width;
                                //fTouch.PopupMenu_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.PopupMenu_H.Value = list.Rectangle.Height;

                                fTouch.PopupMenu_NameDefine.Text = list.Name_define;
                                fTouch.PopupMenu_DataAutoUpLoadCheck.Checked = list.PopupMenuInformation.IsDataAutoUpLoad;
                                if (((list.PopupMenuInformation.Pic_On + 1) & 0xFF00) == 0)
                                {
                                    fTouch.PopupMenu_ButtonEffectCheck.Checked = false;
                                    fTouch.PopupMenu_ButtonEffectNum.Value = list.PopupMenuInformation.Pic_On;
                                }
                                else
                                {
                                    fTouch.PopupMenu_ButtonEffectNum.Value = -1;
                                }
                                fTouch.PopupMenu_ButtonEffectPic.Image = list.PopupMenuInformation.ButtonEffectPic;
                                fTouch.PopupMenu_VarAddress.Text = list.VP.ToString("X4");
                                if (list.PopupMenuInformation.VP_Mode <= 2)
                                {
                                    fTouch.PopupMenu_CheckList.SetItemChecked(list.PopupMenuInformation.VP_Mode, true);
                                    fTouch.PopupMenu_WriteBit.Visible = fTouch.PopupMenu_BitNum.Visible = false;
                                }
                                else
                                {
                                    fTouch.PopupMenu_CheckList.SetItemChecked(3, true);
                                    fTouch.PopupMenu_BitNum.Value = (Byte)(list.PopupMenuInformation.VP_Mode & 0x0F);
                                    fTouch.PopupMenu_WriteBit.Visible = fTouch.PopupMenu_BitNum.Visible = true;
                                }
                                fTouch.PopupMenu_MenuAtPage.Value = list.PopupMenuInformation.Pic_Menu;
                                fTouch.PopupMenu_MenuPic.Image = list.PopupMenuInformation.PopupMenuPic;
                                fTouch.PopupMenu_MenuLeftUp.Clear();
                                fTouch.PopupMenu_MenuLeftUp.AppendText(list.PopupMenuInformation.AREA_Menu_Xs.ToString("D4"));
                                fTouch.PopupMenu_MenuLeftUp.AppendText(list.PopupMenuInformation.AREA_Menu_Ys.ToString("D4"));

                                fTouch.PopupMenu_MenuRightDown.Clear();
                                fTouch.PopupMenu_MenuRightDown.AppendText(list.PopupMenuInformation.AREA_Menu_Xe.ToString("D4"));
                                fTouch.PopupMenu_MenuRightDown.AppendText(list.PopupMenuInformation.AREA_Menu_Ye.ToString("D4"));
                                fTouch.PopupMenu_MenuDisPosition.Clear();
                                fTouch.PopupMenu_MenuDisPosition.AppendText(list.PopupMenuInformation.Menu_Position_X.ToString("D4"));
                                fTouch.PopupMenu_MenuDisPosition.AppendText(list.PopupMenuInformation.Menu_Position_Y.ToString("D4"));
                                break;
                            #endregion
                            #region case ActionIcon
                            case PIC_Obj.aniicon_display:
                                //fTouch.ActionIcon_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.ActionIcon_X.Value = list.Rectangle.X;
                                //fTouch.ActionIcon_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.ActionIcon_Y.Value = list.Rectangle.Y;
                                //fTouch.ActionIcon_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.ActionIcon_W.Value = list.Rectangle.Width;
                                //fTouch.ActionIcon_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.ActionIcon_H.Value = list.Rectangle.Height;

                                fTouch.ActionIcon_NameDefine.Text = list.Name_define;
                                fTouch.ActionIcon_DescripPoint.Text = list.SP.ToString("X4");
                                fTouch.ActionIcon_VarAddress.Text = list.VP.ToString("X4");
                                fTouch.ActionIcon_IconFile.Text = list.ActionIconInforamtion.Icon_FileName;
                                if (list.ActionIconInforamtion.Icon_FileName != "" && !fTouch.ActionIcon_IconFile.Items.Contains(list.ActionIconInforamtion.Icon_FileName))
                                {
                                    fTouch.ActionIcon_IconFile.Items.Add(list.ActionIconInforamtion.Icon_FileName);
                                    fTouch.ActionIcon_IconFile.Text = list.ActionIconInforamtion.Icon_FileName;
                                }
                                else if (fTouch.ActionIcon_IconFile.Items.Count != 0 && list.ActionIconInforamtion.Icon_FileName != "")
                                {
                                    if (list.ActionIconInforamtion.Iconfileselect != -1)
                                    {
                                        //fTouch.ActionIcon_IconFile.SelectedIndex = list.ActionIconInforamtion.Iconfileselect;
                                        fTouch.ActionIcon_IconFile.Text = list.ActionIconInforamtion.Icon_FileName;

                                    }
                                    else
                                    {
                                        fTouch.ActionIcon_IconFile.Items.Clear();
                                    }
                                    fTouch.ActionIcon_IconFile.Refresh();
                                }
                                else if(list.ActionIconInforamtion.Icon_FileName == "")
                                {
                                    fTouch.ActionIcon_IconFile.Items.Clear();
                                    fTouch.ActionIcon_IconFile.Refresh();
                                }
                                fTouch.ActionIcon_VSTOP.Value = list.ActionIconInforamtion.V_Stop;
                                fTouch.ActionIcon_VSTART.Value = list.ActionIconInforamtion.V_Start;
                                fTouch.ActionIcon_StopID.Value = list.ActionIconInforamtion.Icon_Stop;
                                fTouch.ActionIcon_StartID.Value = list.ActionIconInforamtion.Icon_Start;
                                fTouch.ActionIcon_EndID.Value = list.ActionIconInforamtion.Icon_End;
                                fTouch.ActionIcon_ShowMode.Text = list.ActionIconInforamtion.strMode;
                                fTouch.ActionIcon_InitValue.Value = list.ActionIconInforamtion.InitlizValue;
                                break;
                            #endregion
                            #region case Incremental Adjustment
                            case PIC_Obj.increadj:
                                //fTouch.IncreaseAdj_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.IncreaseAdj_X.Value = list.Rectangle.X;
                                //fTouch.IncreaseAdj_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.IncreaseAdj_Y.Value = list.Rectangle.Y;
                                //fTouch.IncreaseAdj_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.IncreaseAdj_W.Value = list.Rectangle.Width;
                                //fTouch.IncreaseAdj_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.IncreaseAdj_H.Value = list.Rectangle.Height;

                                fTouch.IncreaseAdj_NameDefine.Text = list.Name_define;
                                fTouch.IncreaseAdj_DataAutoUpLoad.Checked = list.IncreaseAdjInformation.IsDataAutoUpLoad;
                                fTouch.IncreaseAdj_ButtonEffectPic.Image = list.IncreaseAdjInformation.Pic_OnImage;
                                if (((list.IncreaseAdjInformation.Pic_On + 1) & 0xFF00) == 0)
                                {
                                    fTouch.IncreaseAdj_ButtonEffectCheck.Checked = false;
                                    fTouch.IncreaseAdj_ButtonEffectNum.Value = list.IncreaseAdjInformation.Pic_On;
                                }
                                else
                                {
                                    fTouch.IncreaseAdj_ButtonEffectNum.Value = -1;
                                }
                                //fTouch.IncreaseAdj_ButtonEffectNum.Value = list.IncreaseAdjInformation.Pic_On;
                                fTouch.IncreaseAdj_VarAddress.Text = list.VP.ToString("X4");
                                if (list.IncreaseAdjInformation.VP_Mode <= 2)
                                {
                                    fTouch.IncreaseAdj_CheckList.SetItemChecked(list.IncreaseAdjInformation.VP_Mode, true);
                                    fTouch.IncreaseAdj_LabBItNum.Visible = fTouch.IncreaseAdj_BitNum.Visible = false;
                                }
                                else
                                {
                                    fTouch.IncreaseAdj_CheckList.SetItemChecked(3, true);
                                    fTouch.IncreaseAdj_BitNum.Value = (Byte)(list.IncreaseAdjInformation.VP_Mode & 0x0F);
                                    fTouch.IncreaseAdj_LabBItNum.Visible = fTouch.IncreaseAdj_BitNum.Visible = true;
                                }
                                fTouch.IncreaseAdj_AdjMode.SelectedIndex = list.IncreaseAdjInformation.Adj_Mode;
                                fTouch.IncreaseAdj_ReturnMode.SelectedIndex = list.IncreaseAdjInformation.Return_Mode;
                                fTouch.IncreaseAdj_AdjStep.Value = list.IncreaseAdjInformation.Adj_Step;
                                fTouch.IncreaseAdj_VMin.Value = list.IncreaseAdjInformation.V_Min;
                                fTouch.IncreaseAdj_VMax.Value = list.IncreaseAdjInformation.V_Max;
                                fTouch.IncreaseAdj_KeyMode.SelectedIndex = list.IncreaseAdjInformation.Key_Mode;
                                break;
                            #endregion
                            #region case Slider Adjustment
                            case PIC_Obj.sliadj:
                                //fTouch.SlideAdj_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.SlideAdj_X.Value = list.Rectangle.X;
                                //fTouch.SlideAdj_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.SlideAdj_Y.Value = list.Rectangle.Y;
                                //fTouch.SlideAdj_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.SlideAdj_W.Value = list.Rectangle.Width;
                                //fTouch.SlideAdj_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.SlideAdj_H.Value = list.Rectangle.Height;

                                fTouch.SlideAdj_NameDefine.Text = list.Name_define;
                                fTouch.SlideAdj_DataAutoUpLoad.Checked = list.SlideAdjInformation.IsDataAutoUpLoad;
                                fTouch.SlideAdj_VarAddress.Text = list.VP.ToString("X4");
                                fTouch.SlideAdj_DataReturnMode.SelectedIndex = (list.SlideAdjInformation.Adj_Mode >> 4);
                                fTouch.SlideAdj_AdjMode.SelectedIndex = list.SlideAdjInformation.Adj_Mode & 0x0f;
                                fTouch.SlideAdj_VBegin.Value = list.SlideAdjInformation.V_begin;
                                fTouch.SlideAdj_VEnd.Value = list.SlideAdjInformation.V_end;
                                break;
                            #endregion
                            #region case ArtFont
                            case PIC_Obj.artfont:
                                //fTouch.ArtFont_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.ArtFont_X.Value = list.Rectangle.X;
                                //fTouch.ArtFont_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.ArtFont_Y.Value = list.Rectangle.Y;
                                //fTouch.ArtFont_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.ArtFont_W.Value = list.Rectangle.Width;
                                //fTouch.ArtFont_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.ArtFont_H.Value = list.Rectangle.Height;
                                fTouch.ArtFont_NameDefine.Text = list.Name_define;
                                fTouch.ArtFont_DescripPoint.Text = list.SP.ToString("X4");
                                fTouch.ArtFont_VarAddress.Text = list.VP.ToString("X4");
                                if (list.ArtFontInformation.Icon_FileName != "" && !fTouch.ArtFont_IconFile.Items.Contains(list.ArtFontInformation.Icon_FileName))
                                {
                                    fTouch.ArtFont_IconFile.Items.Add(list.ArtFontInformation.Icon_FileName);
                                    fTouch.ArtFont_IconFile.Text = list.ArtFontInformation.Icon_FileName;
                                }
                                else if (fTouch.ArtFont_IconFile.Items.Count != 0 && list.ArtFontInformation.Icon_FileName != "")
                                {
                                    if (list.ActionIconInforamtion.Iconfileselect != -1)
                                    {
                                        //fTouch.ArtFont_IconFile.SelectedIndex = list.ArtFontInformation.Icon_SelectIndex;
                                        fTouch.ArtFont_IconFile.Text = list.ArtFontInformation.Icon_FileName;

                                    }
                                    else
                                    {
                                        fTouch.ArtFont_IconFile.Items.Clear();
                                    }
                                    fTouch.ArtFont_IconFile.Refresh();
                                }
                                else if(list.ArtFontInformation.Icon_FileName == "")
                                {
                                    fTouch.ArtFont_IconFile.Items.Clear();
                                    fTouch.ArtFont_IconFile.Refresh();
                                }
                                fTouch.ArtFont_BeginIconNum.Value = list.ArtFontInformation.Icon_0;
                                fTouch.ArtFont_ShowMode.SelectedIndex = list.ArtFontInformation.Icon_Mode;
                                fTouch.ArtFont_VarType.SelectedIndex = list.ArtFontInformation.Var_Type;
                                fTouch.ArtFont_Align.SelectedIndex = list.ArtFontInformation.Align_Mode;
                                fTouch.ArtFont_IntgetLength.Value = list.ArtFontInformation.Integer_Length;
                                fTouch.ArtFont_DecimalLength.Value = list.ArtFontInformation.Decimal_Length;
                                fTouch.ArtFont_InitValue.Value = list.ArtFontInformation.Init_Value;
                                break;
                            #endregion
                            #region case slider Display
                            case PIC_Obj.slidis:
                                //fTouch.SlideDisplay_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.SlideDisplay_X.Value = list.Rectangle.X;
                               // fTouch.SlideDisplay_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.SlideDisplay_Y.Value = list.Rectangle.Y;
                                //fTouch.SlideDisplay_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.SlideDisplay_W.Value = list.Rectangle.Width;
                                //fTouch.SlideDisplay_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.SlideDisplay_H.Value = list.Rectangle.Height;
                                fTouch.SlideDisplay_NameDefine.Text = list.Name_define;
                                fTouch.SlideDisplay_DescripPoint.Text = list.SP.ToString("X4");
                                fTouch.SlideDisplay_VarAddress.Text = list.VP.ToString("X4");
                                fTouch.SlideDisplay_Vbegin.Value = list.SlideDisplayInformation.V_begain;
                                fTouch.SlideDisplay_Vend.Value = list.SlideDisplayInformation.V_end;
                                fTouch.SlideDisplay_Mode.SelectedIndex = list.SlideDisplayInformation.Mode;
                                if (list.SlideDisplayInformation.Icon_FileName != "" && !fTouch.SlideDisplay_IconLib.Items.Contains(list.SlideDisplayInformation.Icon_FileName))
                                {
                                    fTouch.SlideDisplay_IconLib.Items.Add(list.SlideDisplayInformation.Icon_FileName);
                                    fTouch.SlideDisplay_IconLib.Text = list.SlideDisplayInformation.Icon_FileName;
                                }
                                else if (list.SlideDisplayInformation.Icon_FileName != "" && fTouch.SlideDisplay_IconLib.Items.Count != 0)
                                {
                                    if (list.SlideDisplayInformation.Icon_SelectIndex != -1)
                                    {
                                        // fTouch.SlideDisplay_IconLib.SelectedIndex = list.SlideDisplayInformation.Icon_SelectIndex;
                                        fTouch.SlideDisplay_IconLib.Text = list.SlideDisplayInformation.Icon_FileName;
                                    }
                                    else
                                    {
                                        fTouch.SlideDisplay_IconLib.Items.Clear();
                                    }
                                    fTouch.SlideDisplay_IconLib.Refresh();
                                }
                                else if(list.SlideDisplayInformation.Icon_FileName == "")
                                {
                                    fTouch.SlideDisplay_IconLib.Items.Clear();
                                    fTouch.SlideDisplay_IconLib.Refresh();
                                }
                                fTouch.SlideDisplay_IconIDNum.Value = list.SlideDisplayInformation.Icon_ID;
                                fTouch.SlideDisplay_IconMode.SelectedIndex = list.SlideDisplayInformation.Icon_Mode;
                                fTouch.SlideDisplay_X_Adj.Value = list.SlideDisplayInformation.X_adj;
                                fTouch.SlideDisplay_VPDataMode.SelectedIndex = list.SlideDisplayInformation.VP_DATA_Mode;
                                fTouch.SlideDisplay_InitValue.Value = list.SlideDisplayInformation.InitVal;
                                break;
                            #endregion
                            #region case Icon Rotation
                            case PIC_Obj.iconrota:
                                //fTouch.IconRotation_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.IconRotation_X.Value = list.Rectangle.X;
                                //fTouch.IconRotation_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.IconRotation_Y.Value = list.Rectangle.Y;
                                //fTouch.IconRotation_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.IconRotation_W.Value = list.Rectangle.Width;
                                //fTouch.IconRotation_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.IconRotation_H.Value = list.Rectangle.Height;
                                fTouch.IconRotation_NameDefine.Text = list.Name_define;
                                fTouch.IconRotation_DescripPoint.Text = list.SP.ToString("X4");
                                fTouch.IconRotation_VarAddress.Text = list.VP.ToString("X4");


                                if (list.IconRotationInformation.Icon_FileName != "" && !fTouch.IconRotation_IconFile.Items.Contains(list.IconRotationInformation.Icon_FileName))
                                {
                                    fTouch.IconRotation_IconFile.Items.Add(list.IconRotationInformation.Icon_FileName);
                                    fTouch.IconRotation_IconFile.Text = list.IconRotationInformation.Icon_FileName;
                                }
                                else if (list.IconRotationInformation.Icon_FileName != "" && fTouch.IconRotation_IconFile.Items.Count != 0)
                                {
                                    if (list.IconRotationInformation.Icon_SelectIndex != -1)
                                    {
                                        //fTouch.IconRotation_IconFile.SelectedIndex = list.IconRotationInformation.Icon_SelectIndex;
                                        fTouch.IconRotation_IconFile.Text = list.IconRotationInformation.Icon_FileName;
                                    }
                                    else
                                    {
                                        fTouch.IconRotation_IconFile.Items.Clear();
                                    }
                                    fTouch.IconRotation_IconFile.Refresh();
                                }
                                else if(list.IconRotationInformation.Icon_FileName == "")
                                {
                                    fTouch.IconRotation_IconFile.Items.Clear();
                                    fTouch.IconRotation_IconFile.Refresh();
                                }
                                fTouch.IconRotation_IconIDNum.Value = list.IconRotationInformation.Icon_ID;
                                fTouch.IconRotation_Xc.Value = list.IconRotationInformation.Icon_Xc;
                                fTouch.IconRotation_Yc.Value = list.IconRotationInformation.Icon_Yc;
                                fTouch.IconRotation_Vbegin.Value = list.IconRotationInformation.V_begain;
                                fTouch.IconRotation_Vend.Value = list.IconRotationInformation.V_end;
                                fTouch.IconRotation_Albegin.Value = list.IconRotationInformation.AL_begain;
                                fTouch.IconRotation_Alend.Value = list.IconRotationInformation.AL_end;
                                fTouch.IconRotation_DisplayMode.SelectedIndex = list.IconRotationInformation.Mode;
                                fTouch.IconRotation_VPMode.SelectedIndex = list.IconRotationInformation.VP_Mode;
                                fTouch.IconRotation_InitValue.Value = list.IconRotationInformation.Init_Value;
                                break;
                            #endregion
                            #region case Clock Display
                            case PIC_Obj.clockdisplay:
                                //fTouch.ClockDisplay_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.ClockDisplay_X.Value = list.Rectangle.X;
                                //fTouch.ClockDisplay_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.ClockDisplay_Y.Value = list.Rectangle.Y;
                                //fTouch.ClockDisplay_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.ClockDisplay_W.Value = list.Rectangle.Width;
                                //fTouch.ClockDisplay_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.ClockDisplay_H.Value = list.Rectangle.Height;
                                fTouch.ClockDisplay_NameDefine.Text = list.Name_define;
                                fTouch.ClockDisplay_DescripPoint.Text = list.SP.ToString("X4");


                                if (list.ClockDisplayInformation.Icon_FileName != "" && !fTouch.ClockDisplay_IconFile.Items.Contains(list.ClockDisplayInformation.Icon_FileName))
                                {
                                    fTouch.ClockDisplay_IconFile.Items.Add(list.ClockDisplayInformation.Icon_FileName);
                                    fTouch.ClockDisplay_IconFile.Text = list.ClockDisplayInformation.Icon_FileName;
                                }
                                else if (list.ClockDisplayInformation.Icon_FileName != "" && fTouch.ClockDisplay_IconFile.Items.Count != 0)
                                {
                                    if (list.ClockDisplayInformation.Icon_SelectIndex != -1)
                                    {
                                        //fTouch.ClockDisplay_IconFile.SelectedIndex = list.ClockDisplayInformation.Icon_SelectIndex;
                                        fTouch.ClockDisplay_IconFile.Text = list.ClockDisplayInformation.Icon_FileName;
                                    }
                                    else
                                    {
                                        fTouch.ClockDisplay_IconFile.Items.Clear();
                                    }
                                    fTouch.ClockDisplay_IconFile.Refresh();
                                }
                                else if (list.ClockDisplayInformation.Icon_FileName == "")
                                {
                                    fTouch.ClockDisplay_IconFile.Items.Clear();
                                    fTouch.ClockDisplay_IconFile.Refresh();
                                }
                                fTouch.ClockDisplay_HourCheck.Checked = list.ClockDisplayInformation.IsDiaplayHour;
                                if (fTouch.ClockDisplay_HourCheck.Checked == false)
                                {
                                    fTouch.ClockDisplay_HourIconNum.Value = list.ClockDisplayInformation.Icon_Hour;
                                }
                                fTouch.ClockDisplay_HourPosition.Clear();
                                fTouch.ClockDisplay_HourPosition.AppendText(list.ClockDisplayInformation.Icon_Hour_Central_X.ToString("D4"));
                                fTouch.ClockDisplay_HourPosition.AppendText(list.ClockDisplayInformation.Icon_Hour_Central_Y.ToString("D4"));
                                fTouch.ClockDisplay_MinuteCheck.Checked = list.ClockDisplayInformation.IsDiaplayMinute;
                                if (fTouch.ClockDisplay_MinuteCheck.Checked == false)
                                {
                                    fTouch.ClockDisplay_MinuteIconNum.Value = list.ClockDisplayInformation.Icon_Minute;
                                }
                                fTouch.ClockDisplay_MinutePosition.Clear();
                                fTouch.ClockDisplay_MinutePosition.AppendText(list.ClockDisplayInformation.Icon_Minute_Central_X.ToString("D4"));
                                fTouch.ClockDisplay_MinutePosition.AppendText(list.ClockDisplayInformation.Icon_Minute_Central_Y.ToString("D4"));
                                fTouch.ClockDisplay_SecCheck.Checked = list.ClockDisplayInformation.IsDiaplaySecond;
                                if (fTouch.ClockDisplay_SecCheck.Checked == false)
                                {
                                    fTouch.ClockDisplay_SecondIconNum.Value = list.ClockDisplayInformation.Icon_Second;
                                }
                                fTouch.ClockDisplay_SecondPosition.Clear();
                                fTouch.ClockDisplay_SecondPosition.AppendText(list.ClockDisplayInformation.Icon_Second_Central_X.ToString("D4"));
                                fTouch.ClockDisplay_SecondPosition.AppendText(list.ClockDisplayInformation.Icon_Second_Central_Y.ToString("D4"));
                                break;
                            #endregion
                            #region case GBK
                            case PIC_Obj.GBK:
                                //fTouch.ClockDisplay_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.GBK_X.Value = list.Rectangle.X;
                                //fTouch.ClockDisplay_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.GBK_Y.Value = list.Rectangle.Y;
                                //fTouch.ClockDisplay_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.GBK_W.Value = list.Rectangle.Width;
                                //fTouch.ClockDisplay_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.GBK_H.Value = list.Rectangle.Height;
                                fTouch.GBK_Namedefine.Text = list.Name_define;
                                fTouch.GBK_DataAutoUpload.Checked = (list.GBKInformation.IsDataAutoUpLoad == 0xFD)?(true):(false);
                                fTouch.GBK_ButtonEffectNum.Maximum = Images_Form.Pic_Number;
                                if (list.GBKInformation.Pic_OnPic != null)
                                {
                                    fTouch.GBK_ButtonEffectPic.Image = list.GBKInformation.Pic_OnPic;
                                }
                                else
                                {
                                    fTouch.GBK_ButtonEffectPic.Image = null;
                                }
                                if (((list.GBKInformation.Pic_On + 1) & 0xFF00) == 0)
                                {
                                    fTouch.GBK_ButtonEffectCheck.Checked = false;
                                    fTouch.GBK_ButtonEffectNum.Value = list.GBKInformation.Pic_On;
                                }
                                else
                                {
                                    fTouch.GBK_ButtonEffectNum.Value = -1;
                                }
                                ///切换界面
                                fTouch.GBK_PageChangeNum.Maximum = Images_Form.Pic_Number;
                                if (list.GBKInformation.Pic_NextPic != null)
                                {
                                    fTouch.GBK_PageChangePic.Image = list.GBKInformation.Pic_NextPic;
                                }
                                else
                                {
                                    fTouch.GBK_PageChangePic.Image = null;
                                }
                                if (((list.GBKInformation.Pic_Next + 1) & 0xFF00) == 0)
                                {
                                    fTouch.GBK_PageChangeCheck.Checked = false;
                                    fTouch.GBK_PageChangeNum.Value = list.GBKInformation.Pic_Next;
                                }
                                else
                                {
                                    fTouch.GBK_PageChangeNum.Value = -1;
                                }
                                fTouch.GKB_VarAdress.Text = list.VP.ToString("X4");
                                fTouch.GBK_TextLength.Value = list.GBKInformation.VP_Len_Max;
                                fTouch.GBK_InputMode.SelectedIndex = list.GBKInformation.Scan_Mode;
                                fTouch.GBK_DisplayFont.Value = list.GBKInformation.Lib_GBK1;
                                fTouch.GBK_DisplayFontSize.Value = list.GBKInformation.Font_Scale1;
                                fTouch.GBK_TextColor.Text = list.GBKInformation.ColorNum1.ToString("X4");
                                fTouch.GBK_TextColorPic.BackColor = list.GBKInformation.Color1;
                                fTouch.GBK_InputProcessFont.Value = list.GBKInformation.Lib_GBK2;
                                
                                fTouch.GBK_InputProcessFontSize.Value = list.GBKInformation.Font_Scale2;
                                fTouch.GBK_TextProcessColor.Text = list.GBKInformation.ColorNum2.ToString("X4");
                                fTouch.GBK_TextProcessColorPic.BackColor = list.GBKInformation.Color2;
                                fTouch.GBK_DispalyMode.SelectedIndex = list.GBKInformation.PY_Disp_Mode;
                                fTouch.GBK_CursorColor.SelectedIndex = list.GBKInformation.Cusor_Color;

                                fTouch.GBK_InputStateReturn.Checked = ((list.GBKInformation.Scan_Return_Mode == 0xFF) ? (false) : (true));
                                fTouch.GBK_InputDisplayAreaLeft.Clear();
                                fTouch.GBK_InputDisplayAreaLeft.AppendText(list.GBKInformation.Scan0_Area_Start_Xs.ToString("D4"));
                                fTouch.GBK_InputDisplayAreaLeft.AppendText(list.GBKInformation.Scan0_Area_Start_Ys.ToString("D4"));
                                fTouch.GBK_InputDisplayAreaRight.Clear();
                                fTouch.GBK_InputDisplayAreaRight.AppendText(list.GBKInformation.Scan0_Area_End_Xe.ToString("D4"));
                                fTouch.GBK_InputDisplayAreaRight.AppendText(list.GBKInformation.Scan0_Area_End_Ye.ToString("D4"));
                                fTouch.GBK_PinyinDisplayPoint.Clear();
                                fTouch.GBK_PinyinDisplayPoint.AppendText(list.GBKInformation.Scan1_Area_Start_Xs.ToString("D4"));
                                fTouch.GBK_PinyinDisplayPoint.AppendText(list.GBKInformation.Scan1_Area_Start_Ys.ToString("D4"));

                                fTouch.GBK_DisplaySpacing.Value = list.GBKInformation.Scan_Dis;
                                fTouch.GBK_KeyBoardPosition.SelectedIndex = list.GBKInformation.KB_Source;
                                fTouch.GBK_KeyBoardAtPage.Value = list.GBKInformation.PIC_KB;
                                fTouch.GBK_KeyBoardPic.BackgroundImage = list.GBKInformation.PIC_KBPic;
                                fTouch.GBK_KeyboardLeftup.Clear();
                                fTouch.GBK_KeyboardLeftup.AppendText(list.GBKInformation.AREA_KB_Xs.ToString("D4"));
                                fTouch.GBK_KeyboardLeftup.AppendText(list.GBKInformation.AREA_KB_Ys.ToString("D4"));
                                fTouch.GBK_KeyboardRightDown.Clear();
                                fTouch.GBK_KeyboardRightDown.AppendText(list.GBKInformation.AREA_KB_Xe.ToString("D4"));
                                fTouch.GBK_KeyboardRightDown.AppendText(list.GBKInformation.AREA_KB_Ye.ToString("D4"));
                                fTouch.GBK_KeyboardShowLocation.Clear();
                                fTouch.GBK_KeyboardShowLocation.AppendText(list.GBKInformation.AREA_KB_Position_Xs.ToString("D4"));
                                fTouch.GBK_KeyboardShowLocation.AppendText(list.GBKInformation.AREA_KB_Position_Ys.ToString("D4"));
                                break;
                            #endregion
                            #region case ASCII
                            case PIC_Obj.ASCII:
                                //fTouch.ClockDisplay_X.Maximum = this.Width - list.Rectangle.Width;
                                fTouch.ASCII_X.Value = list.Rectangle.X;
                                //fTouch.ClockDisplay_Y.Maximum = this.Height - list.Rectangle.Height;
                                fTouch.ASCII_Y.Value = list.Rectangle.Y;
                                //fTouch.ClockDisplay_W.Maximum = this.Width - list.Rectangle.X;
                                fTouch.ASCII_W.Value = list.Rectangle.Width;
                                //fTouch.ClockDisplay_H.Maximum = this.Height - list.Rectangle.Y;
                                fTouch.ASCII_H.Value = list.Rectangle.Height;
                                fTouch.ASCII_Namedefine.Text = list.Name_define;
                                fTouch.ASCII_DataAutoUpload.Checked = (list.ASCIIInformation.IsDataAutoUpLoad == 0xFD) ? (true) : (false);
                                fTouch.ASCII_ButtonEffectNum.Maximum = Images_Form.Pic_Number;
                                if (list.ASCIIInformation.Pic_OnPic != null)
                                {
                                    fTouch.ASCII_ButtonEffectPic.Image = list.ASCIIInformation.Pic_OnPic;
                                }
                                else
                                {
                                    fTouch.ASCII_ButtonEffectPic.Image = null;
                                }
                                if (((list.ASCIIInformation.Pic_On + 1) & 0xFF00) == 0)
                                {
                                    fTouch.ASCII_ButtonEffectCheck.Checked = false;
                                    fTouch.ASCII_ButtonEffectNum.Value = list.ASCIIInformation.Pic_On;
                                }
                                else
                                {
                                    fTouch.ASCII_ButtonEffectNum.Value = -1;
                                }
                                ///切换界面
                                fTouch.ASCII_PageChangeNum.Maximum = Images_Form.Pic_Number;
                                if (list.ASCIIInformation.Pic_NextPic != null)
                                {
                                    fTouch.ASCII_PageChangePic.Image = list.ASCIIInformation.Pic_NextPic;
                                }
                                else
                                {
                                    fTouch.ASCII_PageChangePic.Image = null;
                                }
                                if (((list.ASCIIInformation.Pic_Next + 1) & 0xFF00) == 0)
                                {
                                    fTouch.ASCII_PageChangeCheck.Checked = false;
                                    fTouch.ASCII_PageChangeNum.Value = list.ASCIIInformation.Pic_Next;
                                }
                                else
                                {
                                    fTouch.ASCII_PageChangeNum.Value = -1;
                                }
                                fTouch.ASCII_VarAdress.Text = list.VP.ToString("X4");
                                fTouch.ASCII_TextLength.Value = list.ASCIIInformation.VP_Len_Max;
                                fTouch.ASCII_InputMode.SelectedIndex = list.ASCIIInformation.Scan_Mode;
                                fTouch.ASCII_DisplayFont.Value = list.ASCIIInformation.Lib_ID;
                                fTouch.ASCII_XDirection.Value = list.ASCIIInformation.Font_Hor;
                                fTouch.ASCII_YDirection.Value = list.ASCIIInformation.Font_Ver;
                                fTouch.ASCII_TextColor.Text = list.ASCIIInformation.ColorNum.ToString("X4");
                                fTouch.ASCII_TextColorPic.BackColor = list.ASCIIInformation.Color;
                                fTouch.ASCII_CursorColor.SelectedIndex = list.ASCIIInformation.Cusor_Color;
                                       
                                fTouch.ASCII_InputStateReturn.Checked = ((list.ASCIIInformation.Scan_Return_Mode == 0x00) ? (false) : (true));
                                fTouch.ASCII_InputDisplayAreaLeft.Clear();
                                fTouch.ASCII_InputDisplayAreaLeft.AppendText(list.ASCIIInformation.Scan_Area_Start_Xs.ToString("D4"));
                                fTouch.ASCII_InputDisplayAreaLeft.AppendText(list.ASCIIInformation.Scan_Area_Start_Ys.ToString("D4"));
                                fTouch.ASCII_InputDisplayAreaRight.Clear();
                                fTouch.ASCII_InputDisplayAreaRight.AppendText(list.ASCIIInformation.Scan_Area_End_Xe.ToString("D4"));
                                fTouch.ASCII_InputDisplayAreaRight.AppendText(list.ASCIIInformation.Scan_Area_End_Ye.ToString("D4"));

                                fTouch.ASCII_InputDisMode.SelectedIndex = list.ASCIIInformation.DISPLAY_EN;
                                fTouch.ASCII_KeyBoardPosition.SelectedIndex = list.ASCIIInformation.KB_Source;
                                fTouch.ASCII_KeyBoardAtPage.Value = list.ASCIIInformation.PIC_KB;
                                fTouch.ASCII_KeyBoardPic.BackgroundImage = list.ASCIIInformation.PIC_KBPic;
                                fTouch.ASCII_KeyboardLeftup.Clear();
                                fTouch.ASCII_KeyboardLeftup.AppendText(list.ASCIIInformation.AREA_KB_Xs.ToString("D4"));
                                fTouch.ASCII_KeyboardLeftup.AppendText(list.ASCIIInformation.AREA_KB_Ys.ToString("D4"));
                                fTouch.ASCII_KeyboardRightDown.Clear();
                                fTouch.ASCII_KeyboardRightDown.AppendText(list.ASCIIInformation.AREA_KB_Xe.ToString("D4"));
                                fTouch.ASCII_KeyboardRightDown.AppendText(list.ASCIIInformation.AREA_KB_Ye.ToString("D4"));
                                fTouch.ASCII_KeyboardShowLocation.Clear();
                                fTouch.ASCII_KeyboardShowLocation.AppendText(list.ASCIIInformation.AREA_KB_Position_Xs.ToString("D4"));
                                fTouch.ASCII_KeyboardShowLocation.AppendText(list.ASCIIInformation.AREA_KB_Position_Ys.ToString("D4"));
                                break;
                            #endregion
                            #region case TouchState
                            case PIC_Obj.TouchState:
                                fTouch.TouchState_X.Value = list.Rectangle.X;
                                fTouch.TouchState_Y.Value = list.Rectangle.Y;
                                fTouch.TouchState_W.Value = list.Rectangle.Width;
                                fTouch.TouchState_H.Value = list.Rectangle.Height;
                                fTouch.TouchState_NameDefine.Text = list.Name_define;
                                fTouch.TouchState_ButtonEffectNum.Maximum = Images_Form.Pic_Number;
                                if (list.TouchStateInformation.Pic_OnPic != null)
                                {
                                    fTouch.TouchState_ButtonEffectPic.Image = list.TouchStateInformation.Pic_OnPic;
                                }
                                else
                                {
                                    fTouch.TouchState_ButtonEffectPic.Image = null;
                                }
                                if (((list.TouchStateInformation.Pic_On + 1) & 0xFF00) == 0)
                                {
                                    fTouch.TouchState_ButtonEffectCheck.Checked = false;
                                    fTouch.TouchState_ButtonEffectNum.Value = list.TouchStateInformation.Pic_On;
                                }
                                else
                                {
                                    fTouch.TouchState_ButtonEffectNum.Value = -1;
                                }
                                ///切换界面
                                fTouch.TouchState_PageSwitchNum.Maximum = Images_Form.Pic_Number;
                                if (list.TouchStateInformation.Pic_NextPic != null)
                                {
                                    fTouch.TouchState_PageSwitchPic.Image = list.TouchStateInformation.Pic_NextPic;
                                }
                                else
                                {
                                    fTouch.TouchState_PageSwitchPic.Image = null;
                                }
                                if (((list.TouchStateInformation.Pic_Next + 1) & 0xFF00) == 0)
                                {
                                    fTouch.TouchState_PageChangeCheck.Checked = false;
                                    fTouch.TouchState_PageSwitchNum.Value = list.TouchStateInformation.Pic_Next;
                                }
                                else
                                {
                                    fTouch.TouchState_PageSwitchNum.Value = -1;
                                }
                                fTouch.TouchState_FirstMode.SelectedIndex = list.TouchStateInformation.TP_ON_Mode;
                                fTouch.TouchState_FirstVP1S.Text = list.TouchStateInformation.VP1S.ToString("X4");
                                fTouch.TouchState_FirstVP1T.Text = list.TouchStateInformation.VP1T.ToString("X4");
                                fTouch.TouchState_FirstLength.Value = list.TouchStateInformation.LEN1;
                                fTouch.TouchState_ContinueMode.SelectedIndex = list.TouchStateInformation.TP_ON_Continue_Mode;
                                fTouch.TouchState_ContinueVP2S.Text = list.TouchStateInformation.VP2S.ToString("X4");
                                fTouch.TouchState_ContinueVP2T.Text = list.TouchStateInformation.VP2T.ToString("X4");
                                fTouch.TouchState_ContinueLength.Value = list.TouchStateInformation.LEN2;
                                fTouch.TouchState_LoseMode.SelectedIndex = list.TouchStateInformation.TP_OFF_Mode;
                                fTouch.TouchState_LoseVP3S.Text = list.TouchStateInformation.VP3S.ToString("X4");
                                fTouch.TouchState_LoseVP3T.Text = list.TouchStateInformation.VP3T.ToString("X4");
                                fTouch.TouchState_LoseLength.Value = list.TouchStateInformation.LEN3;
                                break;
                            #endregion
                            #region case 
                            case PIC_Obj.RTC_Set:
                                fTouch.RTCset_X.Value = list.Rectangle.X;
                                fTouch.RTCset_Y.Value = list.Rectangle.Y;
                                fTouch.RTCset_W.Value = list.Rectangle.Width;
                                fTouch.RTCset_H.Value = list.Rectangle.Height;
                                fTouch.RTCset_NameDefine.Text = list.Name_define;
                                fTouch.RTCset_DataAutoUpload.Checked = (list.RTCsetInformation.TP_Code == 0xFD04) ? (true) : (false);
                                fTouch.RTCset_ButtonEffectNum.Maximum = Images_Form.Pic_Number;
                                if (list.RTCsetInformation.Pic_OnPic != null)
                                {
                                    fTouch.RTCset_ButtonEffectPic.Image = list.RTCsetInformation.Pic_OnPic;
                                }
                                else
                                {
                                    fTouch.RTCset_ButtonEffectPic.Image = null;
                                }
                                if (((list.RTCsetInformation.Pic_On + 1) & 0xFF00) == 0)
                                {
                                    fTouch.RTCset_ButtonEffectNum.Value = list.RTCsetInformation.Pic_On;
                                }
                                else
                                {
                                    fTouch.RTCset_ButtonEffectNum.Value = -1;
                                }
                                fTouch.RTCset_Location.Clear();
                                fTouch.RTCset_Location.AppendText(list.RTCsetInformation.DisplayPoint_X.ToString("D4"));
                                fTouch.RTCset_Location.AppendText(list.RTCsetInformation.DisplayPoint_Y.ToString("D4"));
                                fTouch.RTCset_DisColor.Text = list.RTCsetInformation.ColorNum.ToString("X4");
                                fTouch.RTCset_DisColorpic.BackColor = list.RTCsetInformation.Color;
                                fTouch.RTCset_FontID.Value = list.RTCsetInformation.Lib_ID;
                                fTouch.RTCset_FontSize.Value = list.RTCsetInformation.Font_Hor;
                                fTouch.RTCset_CursorColor.SelectedIndex = list.RTCsetInformation.Cusor_Color;
                                fTouch.RTCset_KeyBoardAtPage.Value = list.RTCsetInformation.PIC_KB;
                                if(list.RTCsetInformation.PIC_KBPic != null)
                                    fTouch.RTCset_KeyBoardPic.Image = list.RTCsetInformation.PIC_KBPic;
                                fTouch.RTCset_KeyArea_Left.Clear();
                                fTouch.RTCset_KeyArea_Left.AppendText(list.RTCsetInformation.AREA_KB_Xs.ToString("D4"));
                                fTouch.RTCset_KeyArea_Left.AppendText(list.RTCsetInformation.AREA_KB_Ys.ToString("D4"));
                                fTouch.RTCset_KeyBoardRight.Clear();
                                fTouch.RTCset_KeyBoardRight.AppendText(list.RTCsetInformation.AREA_KB_Xe.ToString("D4"));
                                fTouch.RTCset_KeyBoardRight.AppendText(list.RTCsetInformation.AREA_KB_Ye.ToString("D4"));
                                fTouch.RTCset_DisplayPoint.Clear();
                                fTouch.RTCset_DisplayPoint.AppendText(list.RTCsetInformation.AREA_KB_Position_Xs.ToString("D4"));
                                fTouch.RTCset_DisplayPoint.AppendText(list.RTCsetInformation.AREA_KB_Position_Ys.ToString("D4"));
                                break;
                            #endregion
                            #region case BasicGra
                            case PIC_Obj.BasicGra:
                                    fTouch.BasicGra_X.Value = list.Rectangle.X;
                                    fTouch.BasicGra_Y.Value = list.Rectangle.Y;
                                    fTouch.BasicGra_H.Value = list.Rectangle.Height;
                                    fTouch.BasicGra_W.Value = list.Rectangle.Width;
                                    fTouch.BasicGra_NameDefine.Text = list.Name_define;
                                    fTouch.BasicGra_SP.Text = list.SP.ToString("X4");
                                    fTouch.BasicGra_VP.Text = list.VP.ToString("X4");
                                    fTouch.BasicGra_DashedLine.Checked = (list.BasicGraInformation.Dashed_Line_En == 0) ? (false) : (true);
                                    fTouch.BasicGra_DashSet1.Value = list.BasicGraInformation.Dash_Set_1;
                                    fTouch.BasicGra_DashSet2.Value = list.BasicGraInformation.Dash_Set_2;
                                    fTouch.BasicGra_DashSet3.Value = list.BasicGraInformation.Dash_Set_3;
                                    fTouch.BasicGra_DashSet4.Value = list.BasicGraInformation.Dash_Set_4;
                                break;
                                #endregion
                        }
                        fTouch.PIC_GBoxShow(list.ControlType);
                    } 
                }  
            }
            else
            {
                fTouch.PIC_GBoxShow(PIC_Obj.NONE);
            }
        }
        private void Designer_Paint(object sender, PaintEventArgs e)
        {
            Items.Draw(e.Graphics);
            //是否画出选择的矩形
            if (isdrawSelectRectangel)
            {
                DrawSelectorRectangel(e.Graphics);
            }
            //是否画出鼠标的位置线
            if (isdrawMousePosition)
            {
                if (mouseXs != Point.Empty)
                {
                    e.Graphics.DrawLine(Pens.SkyBlue, mouseXs, mouseXe);
                    e.Graphics.DrawLine(Pens.SkyBlue, mouseYs, mouseYe);
                }
            }  
        }
        private void DrawSelectorRectangel(Graphics g)
        {
            using (SolidBrush sb = new SolidBrush(Color.FromArgb(50, Color.SkyBlue)))
            {
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.FillRectangle(sb, selectRectangle);
            }
            //使用完画笔后及时的释放资源
            using (Pen pen = new Pen(Color.SkyBlue))
            {
                pen.DashStyle = DashStyle.Dash;
                pen.DashPattern = new float[] { 3.0f, 3.0f };

                g.DrawRectangle(pen, selectRectangle);
            }
        }
        private void Designer_MouseLeave(object sender, EventArgs e)
        {
            //如果鼠标离开设计器
            //如果是画鼠标位置则将坐标清空
            if (isdrawMousePosition)
            {
                mouseXs = Point.Empty;
                mouseXe = Point.Empty;
                mouseYs = Point.Empty;
                mouseYe = Point.Empty;
                this.Refresh();
            }
        }
        /// <summary>
        /// 删除设计器中的对象
        /// </summary>
        public void DeleteSelectedItem()
        {
            //获取设计器中的对像数量
            ItemRectangle[] drawitems = new ItemRectangle[Items.Count];
            Items.CopyTo(drawitems, 0);//将对像复制一份删除时使用
            foreach (ItemRectangle dib in drawitems)
            {
                //如果对像选择删除
                if (dib.Selected)
                {
                    Items.Remove(dib);
                }
            }
            fTouch.PIC_GBoxShow(PIC_Obj.NONE);
            //更新对像
            this.Refresh();
        }
        private void Designer_KeyDown(object sender, KeyEventArgs e)
        {
            int n = 0;
            switch (e.KeyCode)
            {
                //如果用户按下DELETE键,删除所选对像
                case Keys.Delete:
                    DeleteSelectedItem();
                    
                    break;
                case Keys.Left:
                    n = Items.SelectionCount;
                    for (int i = 0; i < n; i++)
                    {
                        Items.GetSelectItem(i).Move(-1, 0);
                    }
                    Refresh();
                    
                    break;
                case Keys.Up:
                    n = Items.SelectionCount;
                    for (int i = 0; i < n; i++)
                    {
                        Items.GetSelectItem(i).Move(0, -1);
                    }
                    Refresh();
                    break;
                case Keys.Down:
                    n = Items.SelectionCount;
                    for (int i = 0; i < n; i++)
                    {
                        Items.GetSelectItem(i).Move(0, 1);
                    }
                    Refresh();
                    break;
                case Keys.Right:
                    n = Items.SelectionCount;
                    for (int i = 0; i < n; i++)
                    {
                        Items.GetSelectItem(i).Move(1, 0);
                    }
                    Refresh();
                    break;
                case Keys.Escape:
                    Items.UnSelectAll();
                    Refresh();
                    break;
            }
        }
        protected override bool ProcessDialogKey(Keys keyData)
        {
            if (keyData == Keys.Up || keyData == Keys.Down || keyData == Keys.Left || keyData == Keys.Right)
                return false;
            else
                return base.ProcessDialogKey(keyData);
        } 
    }
}
//
//                       .::::.
//                     .::::::::.
//                    :::::::::::
//                 ..:::::::::::'
//              '::::::::::::'
//                .::::::::::
//           '::::::::::::::..
//                ..::::::::::::.
//              ``::::::::::::::::
//               ::::``:::::::::'        .:::.
//              ::::'   ':::::'       .::::::::.
//            .::::'      ::::     .:::::::'::::.
//           .:::'       :::::  .:::::::::' ':::::.
//          .::'        :::::.:::::::::'      ':::::.
//         .::'         ::::::::::::::'         ``::::.
//     ...:::           ::::::::::::'              ``::.
//    ```` ':.          ':::::::::'                  ::::..
//                       '.:::::'                    ':'````..

