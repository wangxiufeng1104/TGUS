using System;
using System.Windows.Forms;
using System.Drawing;

namespace TGUS
{
    public class DrawBase
    {
        public DrawBase()
        {
            
        }

       
        protected void AddNewObject(Designer designer, ItemRectangle drawitem)
        {
            
            switch (Main_Form.VarType)
            {
                case Main_Form.myGraphicsType.touch:
                    drawitem.FillColor = Color.FromArgb(60, Color.Yellow);
                    break;
                case Main_Form.myGraphicsType.variable:
                    drawitem.FillColor = Color.FromArgb(60, Color.SkyBlue);
                    break;
                case Main_Form.myGraphicsType.other:
                    drawitem.FillColor = Color.FromArgb(60, Color.Red);
                    break;
                case Main_Form.myGraphicsType.AreaSetting:
                    drawitem.FillColor = Color.FromArgb(60, Color.PaleGreen);
                    break;
            }
            drawitem.presentpage_num = Main_Form.presentpage_num;//绑定图元所在页面
            drawitem.ControlType = Main_Form.SelectType;
            switch(drawitem.ControlType)
            {
                #region case basictouch
                case PIC_Obj.basictouch:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "Base_Touch";
                    drawitem.BaseTouchInfo.ButtonEffect_Image = null;
                    drawitem.BaseTouchInfo.Pic_On = -1;
                    drawitem.BaseTouchInfo.PageChange_Image = null;
                    drawitem.BaseTouchInfo.Pic_Next = -1;
                    drawitem.BaseTouchInfo.TP_Code = 0x00;
                    break;
                #endregion
                #region case data_display
                case PIC_Obj.data_display:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "Data_Display";
                    drawitem.SP = 0xFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.DataVarInfo.Display_Color = Color.Black;
                    drawitem.DataVarInfo.Lib_ID = 0x00;
                    drawitem.DataVarInfo.Font_Size = 8;
                    drawitem.DataVarInfo.Font_Align = 0;
                    drawitem.DataVarInfo.Var_Type = 0;
                    drawitem.DataVarInfo.Integer_Length = 8;
                    
                    drawitem.DataVarInfo.String_Uint = "";
                    drawitem.DataVarInfo.Initial_Value = 0;
                    break;
                #endregion
                #region case icon_display
                case PIC_Obj.icon_display:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "Icon";
                    drawitem.SP = 0xFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.IconVarInformation.Iconfileselect = -1;
                    drawitem.IconVarInformation.Icon_FileName = string.Empty;
                    drawitem.IconVarInformation.V_Max = 0;
                    drawitem.IconVarInformation.Icon_MaxPic = null;
                    drawitem.IconVarInformation.Icon_IsMaxTransparent = false;
                    drawitem.IconVarInformation.Icon_Max = -1;
                    drawitem.IconVarInformation.V_Min = 0;
                    drawitem.IconVarInformation.Icon_MinPic = null;
                    drawitem.IconVarInformation.Icon_IsMinTransparent = false;
                    drawitem.IconVarInformation.Icon_Min = -1;
                    drawitem.IconVarInformation.InitialValue = 0;
                    break;
                #endregion
                #region case text_dispaly
                case PIC_Obj.text_dispaly:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "Text_Display";
                    drawitem.SP = 0xFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.TextDisplayInformation.Display_Color = Color.Black;
                    drawitem.TextDisplayInformation.Encode_Mode = 0;
                    drawitem.TextDisplayInformation.IsCharacternotadj = false;
                    drawitem.TextDisplayInformation.initial_value = string.Empty;
                    drawitem.TextDisplayInformation.Font_X_Dots = 16;
                    drawitem.TextDisplayInformation.Font_Y_Dots = 16;
                    break;
                #endregion
                #region case rtc_display
                case PIC_Obj.rtc_display:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "RTC_Display";
                    drawitem.SP = 0XFFFF;
                    drawitem.RTCDisplayInformatin.Font_X_Dots = 16;
                    drawitem.RTCDisplayInformatin.Display_Color = Color.Black;
                    drawitem.RTCDisplayInformatin.String_Code = "Y-M-D H:Q:S W";
                    break;
                #endregion
                #region case datainput
                case PIC_Obj.datainput:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "DataInput";
                    drawitem.DataInputInformation.Pic_ID = (UInt16)Main_Form.presentpage_num;
                    drawitem.DataInputInformation.IsDataAutoUpLoad = false;
                    drawitem.DataInputInformation.Pic_On = -1;
                    drawitem.DataInputInformation.ButtonEffectPic = null;
                    drawitem.DataInputInformation.Pic_Next = -1;
                    drawitem.DataInputInformation.ButtonChangePagePic = null;
                    drawitem.VP = 0x0000;
                    drawitem.DataInputInformation.Display_Color = Color.Black;
                    drawitem.DataInputInformation.Lib_ID = 0;
                    drawitem.DataInputInformation.Font_Hor = 16;
                    drawitem.DataInputInformation.N_Int = 8;
                    drawitem.DataInputInformation.PIC_KB = -1;
                    drawitem.DataInputInformation.Return_VP = 0x0000;
                    break;
                #endregion
                #region case keyreturn
                case PIC_Obj.keyreturn:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "Key_Return";
                    drawitem.KeyReturnInformation.Pic_ID = (UInt16)Main_Form.presentpage_num;
                    drawitem.KeyReturnInformation.Pic_On = -1;
                    drawitem.KeyReturnInformation.Pic_Next = -1;
                    drawitem.KeyReturnInformation.Key_Code = 0x0000;
                    drawitem.KeyReturnInformation.Touch_Key_Code = 0xFFFF;
                    drawitem.KeyReturnInformation.Touch_KeyPressing_Code = 0xFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.KeyReturnInformation.VP_Mode = 0x0;
                    break;
                #endregion
                #region case QR_code
                case PIC_Obj.QR_display:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "QR_Code";
                    drawitem.SP = 0xFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.QRCodeInformation.Unit_Pixels = 4;
                    break;
                #endregion
                #region case popupmenu
                case PIC_Obj.menu_display:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "PopupMenu";
                    drawitem.PopupMenuInformation.Pic_ID = (UInt16)Main_Form.presentpage_num;
                    drawitem.PopupMenuInformation.IsDataAutoUpLoad = false;
                    drawitem.PopupMenuInformation.Pic_On = -1;
                    drawitem.PopupMenuInformation.ButtonEffectPic = null;
                    drawitem.VP = 0x0000;
                    drawitem.PopupMenuInformation.VP_Mode = 0x00;
                    drawitem.PopupMenuInformation.Pic_Menu = -1;
                    drawitem.PopupMenuInformation.PopupMenuPic = null;
                    drawitem.PopupMenuInformation.AREA_Menu_Xs = 0;
                    drawitem.PopupMenuInformation.AREA_Menu_Ys = 0;
                    drawitem.PopupMenuInformation.AREA_Menu_Xe = 0;
                    drawitem.PopupMenuInformation.AREA_Menu_Ye = 0;
                    drawitem.PopupMenuInformation.Menu_Position_X = 0;
                    drawitem.PopupMenuInformation.Menu_Position_Y = 0;
                    break;
                #endregion
                #region case ActionIcon
                case PIC_Obj.aniicon_display:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "ActionIcon";
                    drawitem.SP = 0XFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.ActionIconInforamtion.Iconfileselect = -1;
                    drawitem.ActionIconInforamtion.Icon_FileName = string.Empty;
                    drawitem.ActionIconInforamtion.V_Stop = 0;
                    drawitem.ActionIconInforamtion.V_Start = 0;
                    drawitem.ActionIconInforamtion.Icon_Start = 0;
                    drawitem.ActionIconInforamtion.Icon_Stop = 0;
                    drawitem.ActionIconInforamtion.Icon_End = 0;
                    drawitem.ActionIconInforamtion.Mode = 0;
                    drawitem.ActionIconInforamtion.InitlizValue = 0;
                    break;
                #endregion
                #region case IncreaseAdj
                case PIC_Obj.increadj:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "Incremental Adjustment";
                    drawitem.IncreaseAdjInformation.Pic_ID = (UInt16)Main_Form.presentpage_num;
                    drawitem.IncreaseAdjInformation.Pic_On = -1;
                    break;
                #endregion
                #region case SlideAdj
                case PIC_Obj.sliadj:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "Slider Adjustment";
                    drawitem.VP = 0x0000;
                    break;
                #endregion
                #region case ArtFont
                case PIC_Obj.artfont:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "Art Word";
                    drawitem.SP = 0xFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.ArtFontInformation.Icon_SelectIndex = -1;
                    drawitem.ArtFontInformation.Icon_FileName = string.Empty;
                    drawitem.ArtFontInformation.Integer_Length = 8;
                    break;
                #endregion
                #region case SlideDisplay
                case PIC_Obj.slidis:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "Slider Display";
                    drawitem.SP = 0XFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.SlideDisplayInformation.Icon_SelectIndex = -1;
                    drawitem.SlideDisplayInformation.Icon_FileName = string.Empty;
                    break;
                #endregion
                #region case IconRotation
                case PIC_Obj.iconrota:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "Icon Rotation";
                    drawitem.SP = 0xFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.IconRotationInformation.Icon_SelectIndex = -1;
                    drawitem.IconRotationInformation.Icon_FileName = string.Empty;
                    break;
                #endregion
                #region case ClockDisplay
                case PIC_Obj.clockdisplay:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "Clock Display";
                    drawitem.SP = 0xFFFF;
                    drawitem.ClockDisplayInformation.Icon_SelectIndex = -1;
                    drawitem.ClockDisplayInformation.Icon_FileName = string.Empty;
                    break;
                case PIC_Obj.GBK:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "GBK";
                    drawitem.GBKInformation.IsDataAutoUpLoad = 0xFE;
                    drawitem.GBKInformation.Pic_Next = -1;
                    drawitem.GBKInformation.Pic_On = -1;
                    drawitem.GBKInformation.Color1 = Color.Black;
                    drawitem.GBKInformation.Color2 = Color.Black;
                    drawitem.GBKInformation.Font_Scale1 = 16;
                    drawitem.GBKInformation.Font_Scale2 = 16;
                    drawitem.GBKInformation.PIC_KB = -1;
                    drawitem.GBKInformation.Scan_Return_Mode = 0xAA;
                    break;
                #endregion
                #region case ASCII
                case PIC_Obj.ASCII:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "ASCII";
                    drawitem.ASCIIInformation.IsDataAutoUpLoad = 0xFE;
                    drawitem.ASCIIInformation.Pic_Next = -1;
                    drawitem.ASCIIInformation.Pic_On = -1;
                    drawitem.ASCIIInformation.Color = Color.Black;
                    drawitem.ASCIIInformation.Font_Hor = 8;
                    drawitem.ASCIIInformation.Font_Ver = 16;
                    drawitem.ASCIIInformation.PIC_KB = -1;
                    drawitem.ASCIIInformation.Scan_Return_Mode = 0xAA;
                    break;
                #endregion
                #region case TouchState
                case PIC_Obj.TouchState:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "按压数据同步返回";
                    drawitem.TouchStateInformation.Pic_Next = -1;
                    drawitem.TouchStateInformation.Pic_On = -1;
                    drawitem.TouchStateInformation.LEN1 = 2;
                    drawitem.TouchStateInformation.LEN2 = 2;
                    drawitem.TouchStateInformation.LEN3 = 2;
                    break;
                #endregion
                #region case RTCset
                case PIC_Obj.RTC_Set:
                    drawitem.touchvar = ItemBase.TouchOrVar.touch;
                    drawitem.Name_define = "RTC 设置";
                    drawitem.RTCsetInformation.TP_Code = 0xFE04;
                    drawitem.RTCsetInformation.Pic_On = -1;
                    drawitem.RTCsetInformation.PIC_KB = -1;
                    drawitem.RTCsetInformation.Color = Color.Black;
                    drawitem.RTCsetInformation.Font_Hor = 16;
                    break;
                #endregion
                #region case BasicGra
                case PIC_Obj.BasicGra:
                    drawitem.touchvar = ItemBase.TouchOrVar.varable;
                    drawitem.Name_define = "基本图像设置";
                    drawitem.SP = 0xFFFF;
                    drawitem.VP = 0x0000;
                    drawitem.BasicGraInformation.Dash_Set_1 = 1;
                    drawitem.BasicGraInformation.Dash_Set_2 = 1;
                    drawitem.BasicGraInformation.Dash_Set_3 = 1;
                    drawitem.BasicGraInformation.Dash_Set_4 = 1;
                    break;
                #endregion
            }
            //在设计器对像列表中加入新的对像
            designer.Items.Add(drawitem);
            //将原来选中的对像设计为未选中
            designer.Items.UnSelectAll();
            //设置新添加的对像为选中状态
            drawitem.Selected = true;

            designer.Capture = true;
            designer.Refresh();
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
