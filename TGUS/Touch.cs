using System;
using System.Drawing;
using System.Windows.Forms;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

namespace TGUS
{
    public struct iconfileinfo
    {
        public int iconfile_num;
        public string iconfile_name;
        public int iconfile_lenth;
        public int iconfile_addr;
    }
    
    
    public delegate void ChangeImage(Image ima);
    public partial class Touch : WeifenLuo.WinFormsUI.Docking.DockContent
    {
        public static Touch TouchSingle = null;
        public iconfileinfo[] iconfileinfos = new iconfileinfo[128];
        public static PIC_Obj pic_types = PIC_Obj.basictouch;    //当前位图类型
        public static PIC_Obj present_obj;                       ///即将显示的
        public static PIC_Obj Current_Obj = PIC_Obj.NONE;        ///现在正在显示的
        private Main_Form fmain_form = null;
        private Display fDisplay = null;
        private Touch fTouch = null;
        /// <summary>
        /// 临时存储图片
        /// </summary>
        public static Image box_Image = null;
        public static ChoicePicture_Form choicepicture_form = null;
        
        public static Touch GetSingle()
        {
            if (TouchSingle == null)
            {
                TouchSingle = new Touch();

            }
            return TouchSingle;
        }
        private Touch()
        {
            InitializeComponent();
            this.DoubleBuffered = true;//设置本窗体
            SetStyle(ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true); // 禁止擦除背景.
            SetStyle(ControlStyles.DoubleBuffer, true); // 双缓冲

            #region 所有事件
            BaseTouch_X.ValueChanged += NumericUpDown_NumberChange;
            BaseTouch_Y.ValueChanged += NumericUpDown_NumberChange;
            BaseTouch_W.ValueChanged += NumericUpDown_NumberChange;
            BaseTouch_H.ValueChanged += NumericUpDown_NumberChange;
            BaseTouch_ButtonEffectSelect.Click += BaseTouch_ButtonEffectSelect_Click;
            BaseTouch_ButtonEffectnum.ValueChanged += NumericUpDown_NumberChange;
            BaseTouch_ButtonChangePageSelect.Click += BaseTouch_ButtonChangePageSelect_Click;
            BaseTouch_ButtonChangePageNum.ValueChanged += NumericUpDown_NumberChange;
            BaseTouch_ButtonEffectCheckBox.CheckedChanged += BaseTouch_ButtonEffectCheckBox_CheckedChanged;
            BaseTouch_ButtonChangePageCheckBox.CheckedChanged += BaseTouch_ButtonChangePageCheckBox_CheckedChanged;
            BaseTouch_KeyValueCheck.CheckedChanged += BaseTouch_KeyValueCheck_CheckedChanged;
            BaseTouch_KeyValueSet.TextChanged += TextBox_TextChange;
            BaseTouch_KeyValueSet.KeyPress += TextBox_HexOnly;
            BaseTouch_NameDefine.TextChanged += BaseTouch_NameDefine_TextChanged;
            BaseTouch_KeyValueSetButton.Click += BaseTouch_KeyValueSetButton_Click;
            //变量显示
            DataVar_X.ValueChanged += NumericUpDown_NumberChange;
            DataVar_X.KeyPress += NumericUpDown_ForbidDecimal;
            DataVar_Y.ValueChanged += NumericUpDown_NumberChange;
            DataVar_Y.KeyPress += NumericUpDown_ForbidDecimal;
            DataVar_W.ValueChanged += NumericUpDown_NumberChange;
            DataVar_W.KeyPress += NumericUpDown_ForbidDecimal;
            DataVar_H.ValueChanged += NumericUpDown_NumberChange;
            DataVar_H.KeyPress += NumericUpDown_ForbidDecimal;
            DataVar_NameDefine.TextChanged += DataVar_NameDefine_TextChanged;
            DataVar_DescripPoint.TextChanged +=TextBox_TextChange;
            DataVar_DescripPoint.KeyPress += TextBox_HexOnly;
            DataVar_DispalyColorPic.Click += DataVar_DispalyColorPic_Click;
            DataVar_DisplayColor.TextChanged += DataVar_DisplayColor_TextChanged;
            DataVar_DisplayColor.KeyPress += TextBox_HexOnly;
            DataVar_VarAdress.TextChanged += TextBox_TextChange;
            DataVar_VarAdress.KeyPress += TextBox_HexOnly;
            DataVar_FontLib.TextChanged += TextBox_TextChange;
            DataVar_FontLib.KeyPress += TextBox_DecOnly;

            DataVar_FontSize.ValueChanged += NumericUpDown_NumberChange;
            DataVar_Integer_Length.ValueChanged += NumericUpDown_NumberChange;
            DataVar_Integer_Length.KeyPress += NumericUpDown_ForbidDecimal;
            DataVar_Decimal_Length.ValueChanged +=NumericUpDown_NumberChange;
            DataVar_Decimal_Length.KeyPress += NumericUpDown_ForbidDecimal;
            DataVar_Unit_Length.ValueChanged += NumericUpDown_NumberChange;

            DataVar_Display_Unit.TextChanged += TextBox_TextChange;
            
            DataVar_InitialValue.ValueChanged += NumericUpDown_NumberChange;
            DataVar_AlignStyle.SelectedIndexChanged += ComBox_SelectChange;
            DataVar_VarType.SelectedIndexChanged += ComBox_SelectChange;

            //图标变量
            Iconvar_X.ValueChanged += NumericUpDown_NumberChange;
            Iconvar_Y.ValueChanged += NumericUpDown_NumberChange;
            Iconvar_W.ValueChanged += NumericUpDown_NumberChange;
            Iconvar_H.ValueChanged += NumericUpDown_NumberChange;
            Iconvar_NameDefine.TextChanged += TextBox_TextChange;
            Iconvar_DescripPointer.TextChanged += TextBox_TextChange;
            Iconvar_DescripPointer.KeyPress += TextBox_HexOnly;
            Iconvar_VarAdress.TextChanged += TextBox_TextChange;
            Iconvar_VarAdress.KeyPress += TextBox_HexOnly;
            Iconvar_IconFile.Click += Combox_MouseClick;
            Iconvar_IconFile.SelectedIndexChanged += ComBox_SelectChange;
            
            Iconvar_VarMax.TextChanged += TextBox_TextChange;
            Iconvar_VarMaxNum.ValueChanged += NumericUpDown_NumberChange;
            Iconvar_VarMaxPicSelect.Click += Iconvar_VarMaxPicSelect_Click;

            Iconvar_VarMin.TextChanged += TextBox_TextChange;
            Iconvar_VarMinNum.ValueChanged += NumericUpDown_NumberChange;
            Iconvar_VarMinPicSelect.Click += Iconvar_VarMinPicSelect_Click;
            Iconvar_Mode.SelectedIndexChanged += ComBox_SelectChange;
            Iconvar_InitialValue.ValueChanged += NumericUpDown_NumberChange;
            //文本显示
            TxtDisplay_X.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_X.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_Y.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_Y.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_W.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_W.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_H.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_H.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_NameDefine.TextChanged += TextBox_TextChange;
            TxtDisplay_DescripPoint.TextChanged += TextBox_TextChange;
            TxtDisplay_DescripPoint.KeyPress += TextBox_HexOnly;
            TxtDisplay_VarAdress.TextChanged += TextBox_TextChange;
            TxtDisplay_ShowColor.TextChanged += TextBox_TextChange;
            TxtDisplay_ShowColorPic.Click +=TxtDisplay_ShowColorPic_Click;
            TxtDisplay_CodeingMode.SelectedIndexChanged += ComBox_SelectChange;
            TxtDisplay_CharacterNotAdj.CheckedChanged += CheckBox_CheckedChanged;
            TxtDisplay_TxtLength.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_TxtLength.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_Font0_ID.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_Font0_ID.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_Font1_ID.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_Font1_ID.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_Xdirectionnum.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_Xdirectionnum.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_Ydirectionnum.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_Ydirectionnum.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_Horispacing.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_Horispacing.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_verticalspacing.ValueChanged += NumericUpDown_NumberChange;
            TxtDisplay_verticalspacing.KeyPress += NumericUpDown_ForbidDecimal;
            TxtDisplay_Initvalue.TextChanged += TextBox_TextChange;
            /*************RTC_Display***********************/
            RTC_X.ValueChanged += NumericUpDown_NumberChange;
            RTC_Y.ValueChanged += NumericUpDown_NumberChange;
            RTC_W.ValueChanged += NumericUpDown_NumberChange;
            RTC_H.ValueChanged += NumericUpDown_NumberChange;

            RTC_NameDefien.TextChanged += TextBox_TextChange;
            RTC_DescripPointer.TextChanged += TextBox_TextChange;
            RTC_DescripPointer.KeyPress += TextBox_HexOnly;

            RTC_DisplayColor.TextChanged += TextBox_TextChange;
            RTC_DisplayColor.KeyPress += TextBox_HexOnly;

            RTC_DisplayColorPic.Click +=  RTC_DisplayColorPic_Click;
            RTC_FontLib.ValueChanged += NumericUpDown_NumberChange;
            RTC_Xdirectionnum.ValueChanged += NumericUpDown_NumberChange;
            RTC_DataFormat.TextChanged += TextBox_TextChange;
            /******************DataInput*********************/
            DataInput_X.ValueChanged += NumericUpDown_NumberChange;
            DataInput_Y.ValueChanged += NumericUpDown_NumberChange;
            DataInput_W.ValueChanged += NumericUpDown_NumberChange;
            DataInput_H.ValueChanged += NumericUpDown_NumberChange;
            

            DataInput_Namedefine.TextChanged += TextBox_TextChange;

            DataInput_DataAutoUpload.CheckedChanged += CheckBox_CheckedChanged;
            DataInput_ButtonEffectNum.ValueChanged += NumericUpDown_NumberChange;
            DataInput_ButtonEffectSelect.Click += DataInput_ButtonEffectSelect_Click;
            DataInput_ButtonEffectCheck.CheckedChanged += CheckBox_CheckedChanged;
            DataInput_ButtonEffectPic.Click += DataInput_ButtonEffectPic_Click;

            DataInput_PageChangeNum.ValueChanged += NumericUpDown_NumberChange;
            DataInput_PageChangeCheck.CheckedChanged += CheckBox_CheckedChanged;
            DataInput_PageChangeSelect.Click += DataInput_PageChangeSelect_Click;
            DataInput_PageChangePic.Click += DataInput_PageChangePic_Click;

            DataInput_VarAdress.TextChanged += TextBox_TextChange;
            DataInput_VarType.SelectedIndexChanged += ComBox_SelectChange;
            DataInput_IntrgerLength.ValueChanged += NumericUpDown_NumberChange;
            DataInput_DecimalLength.ValueChanged += NumericUpDown_NumberChange;

            DataInput_DisplayLocationSelect.Click += DataInput_DisplayLocationSelect_Click;
            DataInput_DisplayColor.TextChanged += TextBox_TextChange;
            DataInput_DisplayColorPic.Click += DataInput_DisplayColorPic_Click;
            DataInput_FontID.ValueChanged += NumericUpDown_NumberChange;
            DataInput_FontSize.ValueChanged += NumericUpDown_NumberChange;

            DataInput_CursorColor.SelectedIndexChanged += ComBox_SelectChange;
            DataInput_DisplayStyle.SelectedIndexChanged += ComBox_SelectChange;
            DataInput_KeyBoardPosition.SelectedIndexChanged += ComBox_SelectChange;
            
            DataInput_KeyBoardSet.Click += DataInput_KeyBoardSet_Click;
            DataInput_KeyBoardAtPage.ValueChanged += NumericUpDown_NumberChange;
            DataInput_KeyBoardPic.Click += DataInput_KeyBoardPic_Click;
            DataInput_KeyboardShowLocationSet.Click += DataInput_KeyboardShowLocationSet_Click;
            
            DataInput_DataLimiteCheck.CheckedChanged += CheckBox_CheckedChanged;
            DataInput_LimitedMin.ValueChanged += NumericUpDown_NumberChange;
            DataInput_LimitedMax.ValueChanged += NumericUpDown_NumberChange;

            DataInput_InputLoadDataCheck.CheckedChanged += CheckBox_CheckedChanged;
            DataInput_InputVarAddress.TextChanged += TextBox_TextChange;
            DataInput_InputLoadData.ValueChanged += NumericUpDown_NumberChange;
            /************************************************/
            /******************KeyReturn*********************/
            /************************************************/
            KeyRetuen_X.ValueChanged += NumericUpDown_NumberChange;
            KeyRetuen_Y.ValueChanged += NumericUpDown_NumberChange;
            KeyRetuen_W.ValueChanged += NumericUpDown_NumberChange;
            KeyRetuen_H.ValueChanged += NumericUpDown_NumberChange;

            KeyReturn_NameDefine.TextChanged += TextBox_TextChange;
            KeyReturn_ButtonEffrctNum.ValueChanged += NumericUpDown_NumberChange;
            KeyReturn_ButtonEffrctSelect.Click += KeyReturn_ButtonEffrctSelect_Click;
            KeyReturn_ButtonEffrctPic.Click += KeyReturn_ButtonEffrctPic_Click;
            KeyReturn_ButtonEffrctCheck.CheckedChanged += CheckBox_CheckedChanged;

            KeyReturn_ButtonChangePageNum.ValueChanged += NumericUpDown_NumberChange;
            KeyReturn_ButtonChangePageSelect.Click += KeyReturn_ButtonChangePageSelect_Click;
            KeyReturn_ButtonChangePagePic.Click += KeyReturn_ButtonChangePagePic_Click;
            KeyReturn_ButtonChangePageCheck.CheckedChanged += CheckBox_CheckedChanged;

            KeyReturn_KeyValueSet.Click += KeyReturn_KeyValueSet_Click;
            KeyReturn_KeyValue.TextChanged += TextBox_TextChange;
            KeyReturn_TouchKeyValue.TextChanged += TextBox_TextChange;
            KeyReturn_TouchKeyValueSet.Click += KeyReturn_TouchKeyValueSet_Click;
            KeyReturn_KeeppressingText.TextChanged += TextBox_TextChange;
            KeyReturn_Keeppressing.Click += KeyReturn_Keeppressing_Click;
            KeyReturn_VarAddress.TextChanged += TextBox_TextChange;
            
            KeyReturn_BitNum.ValueChanged += NumericUpDown_NumberChange;
            KeyReturn_CheckList.ItemCheck += CheckList_ItemCheck;
            KeyReturn_CheckList.SelectedIndexChanged += CheckList_CheckChange;
            /************************************************/
            /******************QR Code*********************/
            /************************************************/
            QR_Code_X.ValueChanged += NumericUpDown_NumberChange;
            QR_Code_Y.ValueChanged += NumericUpDown_NumberChange;
            QR_Code_W.ValueChanged += NumericUpDown_NumberChange;
            QR_Code_H.ValueChanged += NumericUpDown_NumberChange;

            QR_Code_Unit_Pixels.ValueChanged += NumericUpDown_NumberChange;
            QR_Code_NameDefine.TextChanged += TextBox_TextChange;
            QR_Code_DescripPoint.TextChanged += TextBox_TextChange;
            QR_Code_VarAddress.TextChanged += TextBox_TextChange;
            /************************************************/
            /******************PopupMenu*********************/
            /************************************************/
            PopupMenu_X.ValueChanged += NumericUpDown_NumberChange;
            PopupMenu_Y.ValueChanged += NumericUpDown_NumberChange;
            PopupMenu_W.ValueChanged += NumericUpDown_NumberChange;
            PopupMenu_H.ValueChanged += NumericUpDown_NumberChange;

            PopupMenu_NameDefine.TextChanged += TextBox_TextChange;
            PopupMenu_VarAddress.TextChanged += TextBox_TextChange;

            PopupMenu_BitNum.ValueChanged += NumericUpDown_NumberChange;
            PopupMenu_DataAutoUpLoadCheck.CheckedChanged += CheckBox_CheckedChanged;
            PopupMenu_ButtonEffectCheck.CheckedChanged += CheckBox_CheckedChanged;
            PopupMenu_CheckList.ItemCheck += CheckList_ItemCheck;
            PopupMenu_CheckList.SelectedIndexChanged += CheckList_CheckChange;
            
            PopupMenu_ButtonEffectSelect.Click += PopupMenu_ButtonEffectSelect_Click;
            PopupMenu_ButtonEffectPic.Click += PopupMenu_ButtonEffectPic_Click;
            PopupMenu_ButtonEffectNum.ValueChanged += NumericUpDown_NumberChange;

            PopupMenu_MenuAtPage.ValueChanged += NumericUpDown_NumberChange;
            PopupMenu_MenuSet.Click += PopupMenu_MenuSet_Click;
            PopupMenu_MenuPic.Click += PopupMenu_MenuPic_Click;
            PopupMenu_MenuPositionSet.Click += PopupMenu_MenuPositionSet_Click;
            /************************************************/
            /******************ActionIcon*********************/
            /************************************************/
            ActionIcon_X.ValueChanged += NumericUpDown_NumberChange;
            ActionIcon_Y.ValueChanged += NumericUpDown_NumberChange;
            ActionIcon_W.ValueChanged += NumericUpDown_NumberChange;
            ActionIcon_H.ValueChanged += NumericUpDown_NumberChange;

            ActionIcon_NameDefine.TextChanged += TextBox_TextChange;
            ActionIcon_DescripPoint.TextChanged += TextBox_TextChange;
            ActionIcon_VarAddress.TextChanged += TextBox_TextChange;

            ActionIcon_VSTOP.ValueChanged += NumericUpDown_NumberChange;
            ActionIcon_VSTART.ValueChanged += NumericUpDown_NumberChange;
            ActionIcon_InitValue.ValueChanged += NumericUpDown_NumberChange;

            ActionIcon_IconFile.Click += Combox_MouseClick;
            ActionIcon_IconFile.SelectedIndexChanged += ComBox_SelectChange;
            ActionIcon_StopIDSelect.Click += ActionIcon_ICONIDSelect_Click;
            ActionIcon_StartIDSelect.Click += ActionIcon_ICONIDSelect_Click;
            ActionIcon_EndIDSelect.Click += ActionIcon_ICONIDSelect_Click;
            ActionIcon_ShowMode.SelectedIndexChanged += ComBox_SelectChange;
            /*************************************************/
            /******************IncreaseAdj********************/
            /*************************************************/
            IncreaseAdj_X.ValueChanged += NumericUpDown_NumberChange;
            IncreaseAdj_Y.ValueChanged += NumericUpDown_NumberChange;
            IncreaseAdj_W.ValueChanged += NumericUpDown_NumberChange;
            IncreaseAdj_H.ValueChanged += NumericUpDown_NumberChange;

            IncreaseAdj_NameDefine.TextChanged += TextBox_TextChange;
            IncreaseAdj_VarAddress.TextChanged += TextBox_TextChange;
            IncreaseAdj_AdjStep.ValueChanged += NumericUpDown_NumberChange;
            IncreaseAdj_VMin.ValueChanged += NumericUpDown_NumberChange;
            IncreaseAdj_VMax.ValueChanged += NumericUpDown_NumberChange;

            IncreaseAdj_ButtonEffectSelect.Click += IncreaseAdj_ButtonEffectSelect_Click;
            IncreaseAdj_ButtonEffectPic.Click += IncreaseAdj_ButtonEffectPic_Click;
            IncreaseAdj_ButtonEffectCheck.CheckedChanged += CheckBox_CheckedChanged;
            IncreaseAdj_ButtonEffectNum.ValueChanged += NumericUpDown_NumberChange;
            IncreaseAdj_DataAutoUpLoad.CheckedChanged += CheckBox_CheckedChanged;
            IncreaseAdj_CheckList.ItemCheck += CheckList_ItemCheck;
            IncreaseAdj_CheckList.SelectedIndexChanged += CheckList_CheckChange;
            IncreaseAdj_AdjMode.SelectedIndexChanged += ComBox_SelectChange;
            IncreaseAdj_ReturnMode.SelectedIndexChanged += ComBox_SelectChange;
            IncreaseAdj_BitNum.ValueChanged += NumericUpDown_NumberChange;
            IncreaseAdj_KeyMode.SelectedIndexChanged += ComBox_SelectChange;
            /*************************************************/
            /******************IncreaseAdj********************/
            /*************************************************/
            SlideAdj_X.ValueChanged += NumericUpDown_NumberChange;
            SlideAdj_Y.ValueChanged += NumericUpDown_NumberChange;
            SlideAdj_W.ValueChanged += NumericUpDown_NumberChange;
            SlideAdj_H.ValueChanged += NumericUpDown_NumberChange;
            SlideAdj_VBegin.ValueChanged += NumericUpDown_NumberChange;
            SlideAdj_VEnd.ValueChanged += NumericUpDown_NumberChange;
            SlideAdj_NameDefine.TextChanged += TextBox_TextChange;
            SlideAdj_VarAddress.TextChanged += TextBox_TextChange;
            SlideAdj_DataAutoUpLoad.CheckedChanged += CheckBox_CheckedChanged;
            SlideAdj_DataReturnMode.SelectedIndexChanged += ComBox_SelectChange;
            SlideAdj_AdjMode.SelectedIndexChanged += ComBox_SelectChange;
            /*************************************************/
            /******************ArtFont********************/
            /*************************************************/
            ArtFont_X.ValueChanged += NumericUpDown_NumberChange;
            ArtFont_Y.ValueChanged += NumericUpDown_NumberChange;
            ArtFont_W.ValueChanged += NumericUpDown_NumberChange;
            ArtFont_H.ValueChanged += NumericUpDown_NumberChange;
            ArtFont_IntgetLength.ValueChanged += NumericUpDown_NumberChange;
            ArtFont_DecimalLength.ValueChanged += NumericUpDown_NumberChange;
            ArtFont_InitValue.ValueChanged += NumericUpDown_NumberChange;
            ArtFont_NameDefine.TextChanged += TextBox_TextChange;
            ArtFont_DescripPoint.TextChanged += TextBox_TextChange;
            ArtFont_VarAddress.TextChanged += TextBox_TextChange;
            ArtFont_IconFile.Click += Combox_MouseClick;
            ArtFont_IconFile.SelectedIndexChanged += ComBox_SelectChange;
            ArtFont_BeginIconSelect.Click += ArtFont_BeginIconSelect_Click;
            ArtFont_ShowMode.SelectedIndexChanged += ComBox_SelectChange;
            ArtFont_VarType.SelectedIndexChanged += ComBox_SelectChange;
            ArtFont_Align.SelectedIndexChanged += ComBox_SelectChange;
            /**************************************************/
            /******************SlideDisplay********************/
            /**************************************************/
            SlideDisplay_X.ValueChanged += NumericUpDown_NumberChange;
            SlideDisplay_Y.ValueChanged += NumericUpDown_NumberChange;
            SlideDisplay_W.ValueChanged += NumericUpDown_NumberChange;
            SlideDisplay_H.ValueChanged += NumericUpDown_NumberChange;
            SlideDisplay_NameDefine.TextChanged += TextBox_TextChange;
            SlideDisplay_DescripPoint.TextChanged += TextBox_TextChange;
            SlideDisplay_VarAddress.TextChanged += TextBox_TextChange;
            SlideDisplay_Vbegin.ValueChanged += NumericUpDown_NumberChange;
            SlideDisplay_Vend.ValueChanged += NumericUpDown_NumberChange;
            SlideDisplay_X_Adj.ValueChanged += NumericUpDown_NumberChange;
            SlideDisplay_InitValue.ValueChanged += NumericUpDown_NumberChange;

            SlideDisplay_Mode.SelectedIndexChanged += ComBox_SelectChange;
            SlideDisplay_IconMode.SelectedIndexChanged += ComBox_SelectChange;
            SlideDisplay_VPDataMode.SelectedIndexChanged += ComBox_SelectChange;
            SlideDisplay_IconLib.SelectedIndexChanged += ComBox_SelectChange;
            SlideDisplay_IconLib.Click += Combox_MouseClick;
            SlideDisplay_IconIDSelect.Click += SlideDisplay_IconIDSelect_Click;
            /**************************************************/
            /******************IconRotation********************/
            /**************************************************/
            IconRotation_X.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_Y.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_W.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_H.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_NameDefine.TextChanged += TextBox_TextChange;
            IconRotation_DescripPoint.TextChanged += TextBox_TextChange;
            IconRotation_VarAddress.TextChanged += TextBox_TextChange;
            IconRotation_IconFile.Click += Combox_MouseClick;
            IconRotation_IconFile.SelectedIndexChanged += ComBox_SelectChange;
            IconRotation_IconIDSelect.Click += IconRotation_IconIDSelect_Click;
            IconRotation_Xc.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_Yc.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_Vbegin.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_Vend.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_Albegin.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_Alend.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_InitValue.ValueChanged += NumericUpDown_NumberChange;
            IconRotation_DisplayMode.SelectedIndexChanged += ComBox_SelectChange;
            IconRotation_VPMode.SelectedIndexChanged += ComBox_SelectChange;
            /**************************************************/
            /******************ColckDisplay********************/
            /**************************************************/
            ClockDisplay_X.ValueChanged += NumericUpDown_NumberChange;
            ClockDisplay_Y.ValueChanged += NumericUpDown_NumberChange;
            ClockDisplay_W.ValueChanged += NumericUpDown_NumberChange;
            ClockDisplay_H.ValueChanged += NumericUpDown_NumberChange;
            ClockDisplay_NameDefine.TextChanged += TextBox_TextChange;
            ClockDisplay_DescripPoint.TextChanged += TextBox_TextChange;
            ClockDisplay_IconFile.Click += Combox_MouseClick;
            ClockDisplay_IconFile.SelectedIndexChanged += ComBox_SelectChange;

            ClockDisplay_HourCheck.CheckedChanged += CheckBox_CheckedChanged;
            ClockDisplay_MinuteCheck.CheckedChanged += CheckBox_CheckedChanged;
            ClockDisplay_SecCheck.CheckedChanged += CheckBox_CheckedChanged;
            ClockDisplay_HourAddButton.Click +=ClockDisplay_ClockAddButton_Click;
            ClockDisplay_MinuteAddButton.Click +=ClockDisplay_ClockAddButton_Click;
            ClockDisplay_SecAddButton.Click += ClockDisplay_ClockAddButton_Click;
            /**************************************************/
            /******************GBK*****************************/
            /**************************************************/
            GBK_X.ValueChanged += NumericUpDown_NumberChange;
            GBK_Y.ValueChanged += NumericUpDown_NumberChange;
            GBK_W.ValueChanged += NumericUpDown_NumberChange;
            GBK_H.ValueChanged += NumericUpDown_NumberChange;
            GBK_Namedefine.TextChanged += TextBox_TextChange;
            GBK_DataAutoUpload.CheckedChanged += CheckBox_CheckedChanged;
            GBK_ButtonEffectNum.ValueChanged += NumericUpDown_NumberChange;
            GBK_PageChangeNum.ValueChanged += NumericUpDown_NumberChange;
            GBK_TextLength.ValueChanged += NumericUpDown_NumberChange;
            GBK_DisplayFont.ValueChanged += NumericUpDown_NumberChange;
            GBK_DisplayFontSize.ValueChanged += NumericUpDown_NumberChange;
            GBK_InputProcessFont.ValueChanged += NumericUpDown_NumberChange;
            GBK_InputProcessFontSize.ValueChanged += NumericUpDown_NumberChange;
            GBK_DisplaySpacing.ValueChanged += NumericUpDown_NumberChange;
            GBK_KeyBoardAtPage.ValueChanged += NumericUpDown_NumberChange;
            GBK_ButtonEffectSelect.Click += ButtonEffectSelect_Click;
            GBK_PageChangeSelect.Click += PageChangeSelect_Click;
            GBK_ButtonEffectPic.Click += ButtonEffectSelect_Click;
            GBK_PageChangePic.Click += PageChangeSelect_Click;
            GBK_ButtonEffectCheck.CheckedChanged += CheckBox_CheckedChanged;
            GBK_PageChangeCheck.CheckedChanged += CheckBox_CheckedChanged;
            GKB_VarAdress.TextChanged += TextBox_TextChange;
            GBK_TextColor.TextChanged += TextBox_TextChange;
            GBK_TextProcessColor.TextChanged += TextBox_TextChange;
            GBK_TextColorPic.Click += TextColorPic_Click;
            GBK_TextProcessColorPic.Click += GBK_TextProcessColorPic_Click;
            GBK_InputDisplayAreaSet.Click += InputDisplayAreaSet_Click;
            GBK_PinyinDisplayPointSet.Click += GBK_PinyinDisplayPointSet_Click;
            GBK_KeyboardShowLocationSet.Click += KeyboardShowLocationSet_Click;
            GBK_KeyBoardSet.Click += KeyBoardSet_Click;
            GBK_KeyBoardPic.Click += KeyBoardSet_Click;
            GBK_InputStateReturn.CheckedChanged += CheckBox_CheckedChanged;
            GBK_InputMode.SelectedIndexChanged += ComBox_SelectChange;
            GBK_DispalyMode.SelectedIndexChanged += ComBox_SelectChange;
            GBK_CursorColor.SelectedIndexChanged += ComBox_SelectChange;
            GBK_KeyBoardPosition.SelectedIndexChanged += ComBox_SelectChange;
            /***********************************************************************/
            /****************************ASCII**************************************/
            /***********************************************************************/
            ASCII_X.ValueChanged += NumericUpDown_NumberChange;
            ASCII_Y.ValueChanged += NumericUpDown_NumberChange;
            ASCII_W.ValueChanged += NumericUpDown_NumberChange;
            ASCII_H.ValueChanged += NumericUpDown_NumberChange;
            ASCII_ButtonEffectNum.ValueChanged += NumericUpDown_NumberChange;
            ASCII_PageChangeNum.ValueChanged += NumericUpDown_NumberChange;
            ASCII_TextLength.ValueChanged += NumericUpDown_NumberChange;
            ASCII_DisplayFont.ValueChanged += NumericUpDown_NumberChange;
            ASCII_KeyBoardAtPage.ValueChanged += NumericUpDown_NumberChange;
            ASCII_VarAdress.TextChanged += TextBox_TextChange;
            ASCII_TextColor.TextChanged += TextBox_TextChange;
            ASCII_Namedefine.TextChanged += TextBox_TextChange;
            ASCII_DataAutoUpload.CheckedChanged += CheckBox_CheckedChanged;
            ASCII_ButtonEffectCheck.CheckedChanged += CheckBox_CheckedChanged;
            ASCII_PageChangeCheck.CheckedChanged += CheckBox_CheckedChanged;
            ASCII_InputStateReturn.CheckedChanged += CheckBox_CheckedChanged;
            ASCII_ButtonEffectSelect.Click += ButtonEffectSelect_Click;
            ASCII_PageChangeSelect.Click += PageChangeSelect_Click;
            ASCII_ButtonEffectPic.Click += ButtonEffectSelect_Click;
            ASCII_PageChangePic.Click += PageChangeSelect_Click;
            ASCII_TextColorPic.Click += TextColorPic_Click;
            ASCII_InputDisplayAreaSet.Click += InputDisplayAreaSet_Click;
            ASCII_KeyboardShowLocationSet.Click += KeyboardShowLocationSet_Click;
            ASCII_KeyBoardSet.Click += KeyBoardSet_Click;
            ASCII_KeyBoardPic.Click += KeyBoardSet_Click;
            ASCII_InputMode.SelectedIndexChanged += ComBox_SelectChange;
            ASCII_CursorColor.SelectedIndexChanged += ComBox_SelectChange;
            ASCII_InputDisMode.SelectedIndexChanged += ComBox_SelectChange;
            ASCII_KeyBoardPosition.SelectedIndexChanged += ComBox_SelectChange;
            ASCII_XDirection.ValueChanged += NumericUpDown_NumberChange;
            ASCII_YDirection.ValueChanged += NumericUpDown_NumberChange;
            /***********************************************************************/
            /****************************TouchState*********************************/
            /***********************************************************************/
            TouchState_X.ValueChanged += NumericUpDown_NumberChange;
            TouchState_Y.ValueChanged += NumericUpDown_NumberChange;
            TouchState_W.ValueChanged += NumericUpDown_NumberChange;
            TouchState_H.ValueChanged += NumericUpDown_NumberChange;
            TouchState_NameDefine.TextChanged += TextBox_TextChange;
            TouchState_ButtonEffectNum.ValueChanged += NumericUpDown_NumberChange;
            TouchState_PageSwitchNum.ValueChanged += NumericUpDown_NumberChange;
            TouchState_FirstLength.ValueChanged += NumericUpDown_NumberChange;
            TouchState_ContinueLength.ValueChanged += NumericUpDown_NumberChange;
            TouchState_LoseLength.ValueChanged += NumericUpDown_NumberChange;
            TouchState_ButtonEffectCheck.CheckedChanged += CheckBox_CheckedChanged;
            TouchState_PageChangeCheck.CheckedChanged += CheckBox_CheckedChanged;
            TouchState_FirstVP1S.TextChanged += TextBox_TextChange;
            TouchState_FirstVP1T.TextChanged += TextBox_TextChange;
            TouchState_ContinueVP2S.TextChanged += TextBox_TextChange;
            TouchState_ContinueVP2T.TextChanged += TextBox_TextChange;
            TouchState_LoseVP3S.TextChanged += TextBox_TextChange;
            TouchState_LoseVP3T.TextChanged += TextBox_TextChange;
            TouchState_FirstMode.SelectedIndexChanged += ComBox_SelectChange;
            TouchState_ContinueMode.SelectedIndexChanged += ComBox_SelectChange;
            TouchState_LoseMode.SelectedIndexChanged += ComBox_SelectChange;
            TouchState_ButtonEffectSet.Click += ButtonEffectSelect_Click;
            TouchState_ButtonEffectPic.Click += ButtonEffectSelect_Click;
            TouchState_PageSwitchSet.Click += PageChangeSelect_Click;
            TouchState_PageSwitchPic.Click += PageChangeSelect_Click;
            /***********************************************************************/
            /****************************RTCset*************************************/
            /***********************************************************************/
            RTCset_X.ValueChanged += NumericUpDown_NumberChange;
            RTCset_Y.ValueChanged += NumericUpDown_NumberChange;
            RTCset_W.ValueChanged += NumericUpDown_NumberChange;
            RTCset_H.ValueChanged += NumericUpDown_NumberChange;
            RTCset_ButtonEffectNum.ValueChanged += NumericUpDown_NumberChange;
            RTCset_FontID.ValueChanged += NumericUpDown_NumberChange;
            RTCset_FontSize.ValueChanged += NumericUpDown_NumberChange;
            RTCset_KeyBoardAtPage.ValueChanged += NumericUpDown_NumberChange;
            RTCset_DataAutoUpload.CheckedChanged += CheckBox_CheckedChanged;
            RTCset_NameDefine.TextChanged += TextBox_TextChange;
            RTCset_ButtonEffectSelect.Click += ButtonEffectSelect_Click;
            RTCset_ButtonEffectPic.Click += ButtonEffectSelect_Click;
            RTCset_Locationset.Click += InputDisplayAreaSet_Click;
            RTCset_KeyBoardSet.Click += KeyBoardSet_Click;
            RTCset_KeyBoardPic.Click += KeyBoardSet_Click;
            RTCset_KeyboardPointaSet.Click += KeyboardShowLocationSet_Click;
            RTCset_DisColor.TextChanged += TextBox_TextChange;
            RTCset_DisColorpic.Click += TextColorPic_Click;
            RTCset_CursorColor.SelectedIndexChanged += ComBox_SelectChange;
            /***********************************************************************/
            /****************************BasicGra***********************************/
            /***********************************************************************/
            BasicGra_X.ValueChanged += NumericUpDown_NumberChange;
            BasicGra_Y.ValueChanged += NumericUpDown_NumberChange;
            BasicGra_W.ValueChanged += NumericUpDown_NumberChange;
            BasicGra_H.ValueChanged += NumericUpDown_NumberChange;
            BasicGra_NameDefine.TextChanged += TextBox_TextChange;
            BasicGra_SP.TextChanged += TextBox_TextChange;
            BasicGra_VP.TextChanged += TextBox_TextChange;
            BasicGra_DashSet1.ValueChanged += NumericUpDown_NumberChange;
            BasicGra_DashSet2.ValueChanged += NumericUpDown_NumberChange;
            BasicGra_DashSet3.ValueChanged += NumericUpDown_NumberChange;
            BasicGra_DashSet4.ValueChanged += NumericUpDown_NumberChange;
            BasicGra_DashedLine.CheckedChanged += CheckBox_CheckedChanged;
            #endregion
        }
        private void Touch_Load(object sender, EventArgs e)
        {
            GBox_PageProperties.Visible = true;
            GBox_PageProperties.Location = new Point(10, 20);

            GBox_BaseTouch.Visible = false;
            GBox_BaseTouch.Location = new Point(10, 20);
            
            GBox_DataVar.Visible = false;
            GBox_DataVar.Location = new Point(10, 20);

            GBox_IconVar.Visible = false;
            GBox_IconVar.Location = new Point(10, 20);

            GBox_TxtDisplay.Visible = false;
            GBox_TxtDisplay.Location = new Point(10, 20);

            GBox_RTC.Visible = false;
            GBox_RTC.Location = new Point(10, 20);

            GBox_VarInput.Visible = false;
            GBox_VarInput.Location = new Point(10, 20);

            GBox_KeyReturn.Visible = false;
            GBox_KeyReturn.Location = new Point(10, 20);

            GBox_QR.Visible = false;
            GBox_QR.Location = new Point(10, 20);

            GBox_MeunDis.Visible = false;
            GBox_MeunDis.Location = new Point(10, 20);

            GBox_ActionIcon.Visible = false;
            GBox_ActionIcon.Location = new Point(10, 20);

            GBox_IncreaseAdj.Visible = false;
            GBox_IncreaseAdj.Location = new Point(10, 20);

            GBox_SlideAdj.Visible = false;
            GBox_SlideAdj.Location = new Point(10, 20);

            GBox_ArtFont.Visible = false;
            GBox_ArtFont.Location = new Point(10, 20);

            GBox_SlideDisplay.Visible = false;
            GBox_SlideDisplay.Location = new Point(10, 20);

            GBox_IconSpin.Visible = false;
            GBox_IconSpin.Location = new Point(10, 20);

            GBox_clock.Visible = false;
            GBox_clock.Location = new Point(10, 20);

            GBox_GBK.Visible = false;
            GBox_GBK.Location = new Point(10, 20);

            GBox_ASCII.Visible = false;
            GBox_ASCII.Location = new Point(10, 20);

            GBox_TouchState.Visible = false;
            GBox_TouchState.Location = new Point(10, 20);

            GBox_RTCset.Visible = false;
            GBox_RTCset.Location = new Point(10, 20);

            GBox_BasicGra.Visible = false;
            GBox_BasicGra.Location = new Point(10,20);
        }
        public static void change_Box_Image(Image ima)
        {
            box_Image = ima;
        }
        void CheckList_CheckChange(object sender,EventArgs e)
        {
            CheckedListBox checklistbox = sender as CheckedListBox;
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    switch (checklistbox.Name)
                    {
                        case "KeyReturn_CheckList":
                            Lab_WriteBit.Visible = KeyReturn_BitNum.Visible = (checklistbox.SelectedIndex == 3) ? (true) : (false);
                            if (KeyReturn_CheckList.SelectedIndex <= 2)
                            {
                                list.KeyReturnInformation.VP_Mode = (byte)KeyReturn_CheckList.SelectedIndex;
                            }
                            else
                            {
                                list.KeyReturnInformation.VP_Mode = (byte)(((byte)(KeyReturn_BitNum.Value) & 0x0f) | 0x10);
                            }
                            break;
                        case "PopupMenu_CheckList":
                            PopupMenu_BitNum.Visible = PopupMenu_WriteBit.Visible = (checklistbox.SelectedIndex == 3) ? (true) : (false);
                            if (PopupMenu_CheckList.SelectedIndex <= 2)
                            {
                                list.PopupMenuInformation.VP_Mode = (byte)PopupMenu_CheckList.SelectedIndex;
                            }
                            else
                            {
                                list.PopupMenuInformation.VP_Mode = (byte)(((byte)(PopupMenu_BitNum.Value) & 0x0f) | 0x10);
                            }
                            break;
                        case "IncreaseAdj_CheckList":
                            IncreaseAdj_LabBItNum.Visible = IncreaseAdj_BitNum.Visible = (checklistbox.SelectedIndex == 3) ? (true) : (false);
                            if (IncreaseAdj_CheckList.SelectedIndex <= 2)
                            {
                                list.IncreaseAdjInformation.VP_Mode = (byte)IncreaseAdj_CheckList.SelectedIndex;
                            }
                            else
                            {
                                list.IncreaseAdjInformation.VP_Mode = (byte)(((byte)(IncreaseAdj_BitNum.Value) & 0x0f) | 0x10);
                            }
                            break;
                    }
                }
            }
        }
        void CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox checkbox = sender as CheckBox;
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    switch(checkbox.Name)
                    {
                        case "TxtDisplay_CharacterNotAdj":
                            list.TextDisplayInformation.IsCharacternotadj = (checkbox.Checked) ? (true) : (false);
                            if (checkbox.Checked == true)
                            {
                                list.TextDisplayInformation.Encode_Mode |= 0x80;
                            }
                            else
                            {
                                list.TextDisplayInformation.Encode_Mode &= 0x7F;
                            }
                            break;
                        case "DataInput_DataAutoUpload":
                            list.DataInputInformation.IsDataAutoUpLoad = (checkbox.Checked) ? (true) : (false);
                            break;
                        case "DataInput_ButtonEffectCheck":
                            if(checkbox.Checked == true)
                            {
                                list.DataInputInformation.Pic_On |= 0xFF00;
                                DataInput_ButtonEffectNum.Value = -1;
                                DataInput_ButtonEffectPic.Image = null;
                                list.DataInputInformation.ButtonEffectPic = null;
                            }
                            break;
                        case "DataInput_PageChangeCheck":
                            if (checkbox.Checked == true)
                            {
                                list.DataInputInformation.Pic_Next |= 0xFF00;
                                DataInput_PageChangeNum.Value = -1;
                                DataInput_PageChangePic.Image = null;
                                list.DataInputInformation.ButtonChangePagePic = null;
                            }
                            break;
                        case "DataInput_DataLimiteCheck":
                            list.DataInputInformation.Limits_En = (byte)((checkbox.Checked) ? (0xFF) : (0));
                            break;
                        case "DataInput_InputLoadDataCheck":
                            list.DataInputInformation.Return_Set = (byte)((checkbox.Checked) ? (0x5A) : (0));
                            break;
                        case "KeyReturn_ButtonEffrctCheck":
                            if (checkbox.Checked == true)
                            {
                                list.KeyReturnInformation.Pic_On |= 0xFF00;
                                KeyReturn_ButtonEffrctNum.Value = -1;
                                KeyReturn_ButtonEffrctPic.Image = null;
                                list.KeyReturnInformation.ButtonEffectPic = null;
                            }
                            break;
                        case "KeyReturn_ButtonChangePageCheck":
                            if (checkbox.Checked == true)
                            {  
                                list.KeyReturnInformation.Pic_Next |= 0xFF00;
                                KeyReturn_ButtonChangePageNum.Value = -1;
                                KeyReturn_ButtonChangePagePic.Image = null;
                                list.KeyReturnInformation.ButtonChangePagePic = null;
                            }
                            break;
                        case "PopupMenu_DataAutoUpLoadCheck":
                            list.PopupMenuInformation.IsDataAutoUpLoad = (checkbox.Checked) ? (true) : (false);
                            break;
                        case "PopupMenu_ButtonEffectCheck":
                            if (checkbox.Checked == true)
                            {
                                list.PopupMenuInformation.Pic_On |= 0xFF00;
                                PopupMenu_ButtonEffectNum.Value = -1;
                                PopupMenu_MenuPic.Image = null;
                                list.PopupMenuInformation.ButtonEffectPic = null;

                            }
                            break;
                        case "IncreaseAdj_DataAutoUpLoad":
                            list.IncreaseAdjInformation.IsDataAutoUpLoad = (checkbox.Checked) ? (true) : (false);
                            break;
                        case "IncreaseAdj_ButtonEffectCheck":
                            if (checkbox.Checked == true)
                            {
                                list.IncreaseAdjInformation.Pic_On |= 0xFF00;
                                IncreaseAdj_ButtonEffectNum.Value = -1;
                                IncreaseAdj_ButtonEffectPic.Image = null;
                                list.IncreaseAdjInformation.Pic_OnImage = null;
                            }
                            break;
                        case "SlideAdj_DataAutoUpLoad":
                            list.SlideAdjInformation.IsDataAutoUpLoad = (checkbox.Checked) ? (true) : (false);
                            break;
                        case "ClockDisplay_HourCheck":
                            list.ClockDisplayInformation.IsDiaplayHour = (checkbox.Checked) ? (true) : (false);
                            if (list.ClockDisplayInformation.IsDiaplayHour == true)
                            {
                                ClockDisplay_HourIconNum.Hexadecimal = true;
                                ClockDisplay_HourIconNum.Value = 0xFFFF;
                                list.ClockDisplayInformation.Icon_Hour = 0xFFFF;
                                list.ClockDisplayInformation.Icon_HourPic = null;
                            }
                            break;
                        case "ClockDisplay_MinuteCheck":
                            list.ClockDisplayInformation.IsDiaplayMinute = (checkbox.Checked) ? (true) : (false);
                            if (list.ClockDisplayInformation.IsDiaplayMinute == true)
                            {
                                ClockDisplay_MinuteIconNum.Hexadecimal = true;
                                //ClockDisplay_MinuteIconNum.Refresh();
                                ClockDisplay_MinuteIconNum.Value = 0xFFFF;
                                list.ClockDisplayInformation.Icon_Minute = 0xFFFF;
                                list.ClockDisplayInformation.Icon_MinutePic = null;
                            }
                            break;
                        case "ClockDisplay_SecCheck":
                            list.ClockDisplayInformation.IsDiaplaySecond = (checkbox.Checked) ? (true) : (false);
                            if (list.ClockDisplayInformation.IsDiaplaySecond == true)
                            {
                                ClockDisplay_SecondIconNum.Hexadecimal = true;
                                ClockDisplay_SecondIconNum.Value = 0xFFFF;
                                list.ClockDisplayInformation.Icon_Second = 0xFFFF;
                                list.ClockDisplayInformation.Icon_SecondPic = null;
                            }
                            break;
                        case "GBK_DataAutoUpload":
                            list.GBKInformation.IsDataAutoUpLoad = (byte)((checkbox.Checked) ? (0xFD) : (0xFE));
                            break;
                        case "GBK_ButtonEffectCheck":
                            if (checkbox.Checked == true)
                            {
                                list.GBKInformation.Pic_On |= 0xFF00;
                                GBK_ButtonEffectNum.Value = -1;
                                GBK_ButtonEffectPic.Image = null;
                                list.GBKInformation.Pic_OnPic = null;
                            }
                            break;
                        case "GBK_PageChangeCheck":
                            if (checkbox.Checked == true)
                            {
                                list.GBKInformation.Pic_Next |= 0xFF00;
                                GBK_PageChangeNum.Value = -1;
                                GBK_PageChangePic.Image = null;
                                list.GBKInformation.Pic_NextPic = null;
                            }
                            break;
                        case "GBK_InputStateReturn":
                            list.GBKInformation.Scan_Return_Mode = (byte)((checkbox.Checked) ? (0xAA) : (0xFF));
                            break;
                        case "ASCII_DataAutoUpload":
                            list.ASCIIInformation.IsDataAutoUpLoad = (byte)((checkbox.Checked) ? (0xFD) : (0xFE));
                            break;
                        case "ASCII_ButtonEffectCheck":
                            if (checkbox.Checked == true)
                            {
                                list.ASCIIInformation.Pic_On |= 0xFF00;
                                ASCII_ButtonEffectNum.Value = -1;
                                ASCII_ButtonEffectPic.Image = null;
                                list.ASCIIInformation.Pic_OnPic = null;
                            }
                            break;
                        case "ASCII_PageChangeCheck":
                            if (checkbox.Checked == true)
                            {
                                list.ASCIIInformation.Pic_Next |= 0xFF00;
                                ASCII_PageChangeNum.Value = -1;
                                ASCII_PageChangePic.Image = null;
                                list.ASCIIInformation.Pic_NextPic = null;
                            }
                            break;
                        case "ASCII_InputStateReturn":
                            list.ASCIIInformation.Scan_Return_Mode = (byte)((checkbox.Checked) ? (0x55) : (0x00));
                            break;
                        case "TouchState_ButtonEffectCheck":
                            if (checkbox.Checked == true)
                            {
                                list.TouchStateInformation.Pic_On |= 0xFF00;
                                TouchState_ButtonEffectNum.Value = -1;
                                TouchState_ButtonEffectPic.Image = null;
                                list.TouchStateInformation.Pic_OnPic = null;
                            }
                            break;
                        case "TouchState_PageChangeCheck":
                            if (checkbox.Checked == true)
                            {
                                list.TouchStateInformation.Pic_Next |= 0xFF00;
                                TouchState_PageSwitchNum.Value = -1;
                                TouchState_PageSwitchPic.Image = null;
                                list.TouchStateInformation.Pic_NextPic = null;
                            }
                            break;
                        case "RTCset_DataAutoUpload":
                            list.RTCsetInformation.TP_Code = (UInt16)((checkbox.Checked) ? (0xFD04) : (0xFE04));
                            break;
                        case "BasicGra_DashedLine":
                            list.BasicGraInformation.Dashed_Line_En = (byte)((checkbox.Checked) ? (0x5A) : (0));
                            break;
                    }
                }
            }
        }
        /// <summary>
        /// 文本改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextBox_TextChange(object sender, EventArgs e)
        {
            TextBox textbox = sender as TextBox;

            if (textbox.Text == "" && textbox.Name != "TxtDisplay_Initvalue")
            {
                return;
            }
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    switch (textbox.Name)
                    {
                        #region case BaseTouch
                        case "BaseTouch_KeyValueSet":
                            list.BaseTouchInfo.TP_Code = UInt16.Parse(UInt16.Parse(textbox.Text, System.Globalization.NumberStyles.HexNumber).ToString("X4"), 
                                System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case DataVar
                        case "DataVar_DescripPoint":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text, System.Globalization.NumberStyles.HexNumber).ToString("X4"), 
                                System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "DataVar_VarAdress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text, System.Globalization.NumberStyles.HexNumber).ToString("X4"), 
                                System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "DataVar_FontLib":
                            list.DataVarInfo.Lib_ID = Convert.ToByte(textbox.Text);
                            break;
                        case "DataVar_Display_Unit":
                            list.DataVarInfo.String_Uint = textbox.Text;
                            if(textbox.Text != null)
                            {
                                DataVar_Unit_Length.Value = textbox.Text.Length;
                            }
                            break;
                        #endregion
                        #region case Iconvar
                        case "Iconvar_NameDefine":
                            list.Name_define = Iconvar_NameDefine.Text;
                            break;
                        case "Iconvar_DescripPointer":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "Iconvar_VarAdress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "Iconvar_VarMax":
                            list.IconVarInformation.V_Max = Convert.ToUInt16(textbox.Text);
                            break;
                        case "Iconvar_VarMin":
                            list.IconVarInformation.V_Min = Convert.ToUInt16(textbox.Text);
                            break;
                        #endregion
                        #region case TextDisplay
                        /*********************************文本显示**********************************/
                        case "TxtDisplay_NameDefine":
                            
                            list.Name_define = TxtDisplay_NameDefine.Text;
                            break;
                        case "TxtDisplay_DescripPoint":
                            list.SP = UInt16.Parse(UInt16.Parse(TxtDisplay_DescripPoint.Text, System.Globalization.NumberStyles.HexNumber).ToString("X4"),
                                System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "TxtDisplay_ShowColor":
                            UInt16 color_R;
                            UInt16 color_G;
                            UInt16 color_B;
                            UInt16 color_Num;
                            color_Num = UInt16.Parse(UInt16.Parse(textbox.Text, System.Globalization.NumberStyles.HexNumber).ToString("X4"), 
                                System.Globalization.NumberStyles.HexNumber);
                            color_R = (UInt16)((((color_Num & 0XF800) >> 11) << 3) | ((color_Num & 0X1800) >> 11));
                            color_G = (UInt16)((((color_Num & 0x07E0) >> 5) << 2) | (color_Num & 0x0060)>>5);
                            color_B = (UInt16)(((color_Num & 0x001F) << 3) | (color_Num & 0x0007));
                            TxtDisplay_ShowColorPic.BackColor = Color.FromArgb(color_R, color_G, color_B);
                            list.TextDisplayInformation.Display_Color = TxtDisplay_ShowColorPic.BackColor;
                            list.TextDisplayInformation.COLOR = color_Num;
                            break;
                        case "TxtDisplay_Initvalue":
                           
                           list.TextDisplayInformation.initial_value = textbox.Text;
                            break;
                        case "TxtDisplay_VarAdress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case RTCDisplay
                        case "RTC_NameDefien":
                            list.Name_define = textbox.Text;
                            break;
                        case "RTC_DescripPointer":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "RTC_DisplayColor":
                            UInt16 RTC_color_R;
                            UInt16 RTC_color_G;
                            UInt16 RTC_color_B;
                            UInt16 RTC_color_Num;
                            RTC_color_Num = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            
                            RTC_color_R = (UInt16)((((RTC_color_Num & 0XF800) >> 11) << 3) | ((RTC_color_Num & 0X1800) >> 11));
                            RTC_color_G = (UInt16)((((RTC_color_Num & 0x07E0) >> 5) << 2) | (RTC_color_Num & 0x0060) >> 5);
                            RTC_color_B = (UInt16)(((RTC_color_Num & 0x001F) << 3) | (RTC_color_Num & 0x0007));
                            RTC_DisplayColorPic.BackColor = Color.FromArgb(RTC_color_R, RTC_color_G, RTC_color_B);
                            list.RTCDisplayInformatin.Display_Color = RTC_DisplayColorPic.BackColor;
                            list.RTCDisplayInformatin.COLOR = RTC_color_Num;
                            break;
                        case "RTC_DataFormat":
                            list.RTCDisplayInformatin.String_Code = RTC_DataFormat.Text;
                            break;
                        #endregion
                        #region case DataInput
                        case "DataInput_DisplayColor":
                            UInt16 DataInput_color_R;
                            UInt16 DataInput_color_G;
                            UInt16 DataInput_color_B;
                            UInt16 DataInput_color_Num;
                            DataInput_color_Num = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            list.DataInputInformation.COLOR = DataInput_color_Num;
                            DataInput_color_R = (UInt16)((((DataInput_color_Num & 0XF800) >> 11) << 3) | ((DataInput_color_Num & 0X1800) >> 11));
                            DataInput_color_G = (UInt16)((((DataInput_color_Num & 0x07E0) >> 5) << 2) | (DataInput_color_Num & 0x0060) >> 5);
                            DataInput_color_B = (UInt16)(((DataInput_color_Num & 0x001F) << 3) | (DataInput_color_Num & 0x0007));
                            DataInput_DisplayColorPic.BackColor = Color.FromArgb(DataInput_color_R, DataInput_color_G, DataInput_color_B);
                            list.DataInputInformation.Display_Color = DataInput_DisplayColorPic.BackColor;
                            break;
                        case "DataInput_VarAdress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "DataInput_Namedefine":
                            list.Name_define = DataInput_Namedefine.Text;
                            break;
                        case "DataInput_InputVarAddress":
                            list.DataInputInformation.Return_VP = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case keyreturn
                        case "KeyReturn_NameDefine":
                            list.Name_define = KeyReturn_NameDefine.Text;
                            break;
                        case "KeyReturn_KeyValue":
                            list.KeyReturnInformation.Key_Code = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "KeyReturn_TouchKeyValue":
                            list.KeyReturnInformation.Touch_Key_Code = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "KeyReturn_VarAddress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "KeyReturn_KeeppressingText":
                            list.KeyReturnInformation.Touch_KeyPressing_Code = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case QR Code
                        case "QR_Code_NameDefine":
                            list.Name_define = QR_Code_NameDefine.Text;
                            break;
                        case "QR_Code_DescripPoint":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "QR_Code_VarAddress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text, 
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case PopupMenu
                        case "PopupMenu_NameDefine":
                            list.Name_define = PopupMenu_NameDefine.Text;
                            break;
                        case "PopupMenu_VarAddress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case ActionIcon
                        case "ActionIcon_NameDefine":
                            list.Name_define = ActionIcon_NameDefine.Text;
                            break;
                        case "ActionIcon_DescripPoint":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "ActionIcon_VarAddress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case IncreaseAdj
                        case "IncreaseAdj_NameDefine":
                            list.Name_define = IncreaseAdj_NameDefine.Text;
                            break;
                        case "IncreaseAdj_VarAddress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case SlideAdj
                        case "SlideAdj_NameDefine":
                            list.Name_define = SlideAdj_NameDefine.Text;
                            break;
                        case "SlideAdj_VarAddress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case ArtFont
                        case "ArtFont_NameDefine":
                            list.Name_define = textbox.Text;
                            break;
                        case "ArtFont_DescripPoint":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "ArtFont_VarAddress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case SliderDisplay
                        case "SlideDisplay_NameDefine":
                            list.Name_define = textbox.Text;
                            break;
                        case "SlideDisplay_DescripPoint":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "SlideDisplay_VarAddress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case IconRotation
                        case "IconRotation_NameDefine":
                            list.Name_define = textbox.Text;
                            break;
                        case "IconRotation_DescripPoint":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "IconRotation_VarAddress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case ClockDisplay
                        case "ClockDisplay_NameDefine":
                            list.Name_define = textbox.Text;
                            break;
                        case "ClockDisplay_DescripPoint":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case GBK
                        case "GBK_Namedefine":
                            list.Name_define = textbox.Text;
                            break;
                        case "GKB_VarAdress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "GBK_TextColor":
                            UInt16 GBK_TextColor_R;
                            UInt16 GBK_TextColor_G;
                            UInt16 GBK_TextColor_B;
                            UInt16 GBK_TextColor_Num;
                            GBK_TextColor_Num = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            list.GBKInformation.ColorNum1 = GBK_TextColor_Num;
                            GBK_TextColor_R = (UInt16)((((GBK_TextColor_Num & 0XF800) >> 11) << 3) | ((GBK_TextColor_Num & 0X1800) >> 11));
                            GBK_TextColor_G = (UInt16)((((GBK_TextColor_Num & 0x07E0) >> 5) << 2) | (GBK_TextColor_Num & 0x0060) >> 5);
                            GBK_TextColor_B = (UInt16)(((GBK_TextColor_Num & 0x001F) << 3) | (GBK_TextColor_Num & 0x0007));
                            GBK_TextColorPic.BackColor = Color.FromArgb(GBK_TextColor_R, GBK_TextColor_G, GBK_TextColor_B);
                            list.GBKInformation.Color1 = GBK_TextColorPic.BackColor;
                            break;
                        case "GBK_TextProcessColor":
                            UInt16 GBK_TextProcessColor_R;
                            UInt16 GBK_TextProcessColor_G;
                            UInt16 GBK_TextProcessColor_B;
                            UInt16 GBK_TextProcessColor_Num;
                            GBK_TextProcessColor_Num = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            list.GBKInformation.ColorNum2 = GBK_TextProcessColor_Num;
                            GBK_TextProcessColor_R = (UInt16)((((GBK_TextProcessColor_Num & 0XF800) >> 11) << 3) | ((GBK_TextProcessColor_Num & 0X1800) >> 11));
                            GBK_TextProcessColor_G = (UInt16)((((GBK_TextProcessColor_Num & 0x07E0) >> 5) << 2) | (GBK_TextProcessColor_Num & 0x0060) >> 5);
                            GBK_TextProcessColor_B = (UInt16)(((GBK_TextProcessColor_Num & 0x001F) << 3) | (GBK_TextProcessColor_Num & 0x0007));
                            GBK_TextProcessColorPic.BackColor = Color.FromArgb(GBK_TextProcessColor_R, GBK_TextProcessColor_G, GBK_TextProcessColor_B);
                            list.GBKInformation.Color2 = GBK_TextProcessColorPic.BackColor;
                            break;
                        #endregion
                        #region case ASCII
                        case "ASCII_Namedefine":
                            list.Name_define = textbox.Text;
                            break;
                        case "ASCII_VarAdress":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "ASCII_TextColor":
                            UInt16 ASCII_TextColor_R;
                            UInt16 ASCII_TextColor_G;
                            UInt16 ASCII_TextColor_B;
                            UInt16 ASCII_TextColor_Num;
                            ASCII_TextColor_Num = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            list.ASCIIInformation.ColorNum = ASCII_TextColor_Num;
                            ASCII_TextColor_R = (UInt16)((((ASCII_TextColor_Num & 0XF800) >> 11) << 3) | ((ASCII_TextColor_Num & 0X1800) >> 11));
                            ASCII_TextColor_G = (UInt16)((((ASCII_TextColor_Num & 0x07E0) >> 5) << 2) | (ASCII_TextColor_Num & 0x0060) >> 5);
                            ASCII_TextColor_B = (UInt16)(((ASCII_TextColor_Num & 0x001F) << 3) | (ASCII_TextColor_Num & 0x0007));
                            ASCII_TextColorPic.BackColor = Color.FromArgb(ASCII_TextColor_R, ASCII_TextColor_G, ASCII_TextColor_B);
                            list.ASCIIInformation.Color = ASCII_TextColorPic.BackColor;
                            break;
                        #endregion
                        #region case TouchState
                        case "TouchState_NameDefine":
                            list.Name_define = textbox.Text;
                            break;
                        case "TouchState_FirstVP1S":
                            list.TouchStateInformation.VP1S = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "TouchState_FirstVP1T":
                            list.TouchStateInformation.VP1T = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "TouchState_ContinueVP2S":
                            list.TouchStateInformation.VP2S = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "TouchState_ContinueVP2T":
                            list.TouchStateInformation.VP2T = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "TouchState_LoseVP3S":
                            list.TouchStateInformation.VP3S = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "TouchState_LoseVP3T":
                            list.TouchStateInformation.VP3T = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        #endregion
                        #region case RTCset
                        case "RTCset_DisColor":
                            UInt16 RTCset_TextColor_R;
                            UInt16 RTCset_TextColor_G;
                            UInt16 RTCset_TextColor_B;
                            UInt16 RTCset_TextColor_Num;
                            RTCset_TextColor_Num = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            list.RTCsetInformation.ColorNum = RTCset_TextColor_Num;
                            RTCset_TextColor_R = (UInt16)((((RTCset_TextColor_Num & 0XF800) >> 11) << 3) | ((RTCset_TextColor_Num & 0X1800) >> 11));
                            RTCset_TextColor_G = (UInt16)((((RTCset_TextColor_Num & 0x07E0) >> 5) << 2) | (RTCset_TextColor_Num & 0x0060) >> 5);
                            RTCset_TextColor_B = (UInt16)(((RTCset_TextColor_Num & 0x001F) << 3) | (RTCset_TextColor_Num & 0x0007));
                            RTCset_DisColorpic.BackColor = Color.FromArgb(RTCset_TextColor_R, RTCset_TextColor_G, RTCset_TextColor_B);
                            list.RTCsetInformation.Color = RTCset_DisColorpic.BackColor;
                            break;
                        case "RTCset_NameDefine":
                            list.Name_define = textbox.Text;
                            break;
                        #endregion
                        #region case BasicGra
                        case "BasicGra_NameDefine":
                            list.Name_define = textbox.Text;
                            break;
                        case "BasicGra_SP":
                            list.SP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                        case "BasicGra_VP":
                            list.VP = UInt16.Parse(UInt16.Parse(textbox.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                            break;
                            #endregion
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// NumericUpDown的内容改变
        /// </summary>
        void NumericUpDown_NumberChange(object sender, EventArgs e)
        {
            NumericUpDown numberupdown = sender as NumericUpDown;
            
            UpDownBase upDownBsae = (UpDownBase)numberupdown;
            if(upDownBsae.Text == "")
            {
                return ;
            }
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    switch (numberupdown.Name)
                    {
                        #region case BaseTouch
                        case "BaseTouch_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle BaseTouch_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = BaseTouch_X_Rec;
                            break;
                        case "BaseTouch_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle BaseTouch_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = BaseTouch_Y_Rec;
                            break;
                        case "BaseTouch_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle BaseTouch_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = BaseTouch_W_Rec;
                            break;
                        case "BaseTouch_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle BaseTouch_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = BaseTouch_H_Rec;
                            break;               
                        case "BaseTouch_ButtonEffectnum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if(numberupdown.Value > -1)
                            {
                                list.BaseTouchInfo.ButtonEffect_Image = BaseTouch_ButtonEffectPicture.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.BaseTouchInfo.Pic_On = (int)numberupdown.Value;
                            }
                            else
                                list.BaseTouchInfo.ButtonEffect_Image = BaseTouch_ButtonEffectPicture.Image = null;
                            break;
                        case "BaseTouch_ButtonChangePageNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            
                            if(numberupdown.Value > -1)
                            {
                                list.BaseTouchInfo.PageChange_Image = BaseTouch_ButtonChangePagePic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.BaseTouchInfo.Pic_Next = (int)numberupdown.Value;
                            }
                            else
                                list.BaseTouchInfo.PageChange_Image = BaseTouch_ButtonChangePagePic.Image = null;
                            break;
                        #endregion
                        #region case DataVar
                        /******************************数据变量显示*********************************/
                        case "DataVar_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle DataVar_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = DataVar_X_Rec;
                            break;
                        case "DataVar_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle DataVar_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = DataVar_Y_Rec;
                            break;
                        case "DataVar_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle DataVar_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = DataVar_W_Rec;
                            break;
                        case "DataVar_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle DataVar_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = DataVar_H_Rec;
                            break;
                        case "DataVar_FontSize":
                            list.DataVarInfo.Font_Size = (byte)numberupdown.Value; 
                            break;
                        case "DataVar_Integer_Length":
                            list.DataVarInfo.Integer_Length = (byte)numberupdown.Value;
                            break;
                        case "DataVar_Decimal_Length":
                            list.DataVarInfo.Decimal_Length = (byte)numberupdown.Value;
                            break;
                        case "DataVar_Unit_Length":
                            list.DataVarInfo.Len_unit = (byte)numberupdown.Value;
                            break;
                        case "DataVar_InitialValue":
                            list.DataVarInfo.Initial_Value = (long)numberupdown.Value;
                            break;
                        #endregion
                        #region case Iconvar
                        /***************************************图标变量************************************/
                        case "Iconvar_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle Iconvar_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = Iconvar_X_Rec;
                            break;
                        case "Iconvar_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle Iconvar_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = Iconvar_Y_Rec;
                            break;
                        case "Iconvar_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle Iconvar_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = Iconvar_W_Rec;
                            break;
                        case "Iconvar_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle Iconvar_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = Iconvar_H_Rec;
                            break;
                        case "Iconvar_VarMaxNum":
                            list.IconVarInformation.Icon_Max = (int)numberupdown.Value;
                            if(list.IconVarInformation.Icon_Max >= 1)
                            {
                                if(Iconvar_VarMaxPic.Image == null && list.IconVarInformation.Icon_MaxPic != null)
                                {
                                    Iconvar_VarMaxPic.Image = list.IconVarInformation.Icon_MaxPic;
                                }
                            }
                            break;
                        case "Iconvar_VarMinNum":
                            list.IconVarInformation.Icon_Min = (int)numberupdown.Value;
                            if(list.IconVarInformation.Icon_Min >= 1)
                            {
                                if(Iconvar_VarMinPic.Image == null && list.IconVarInformation.Icon_MinPic != null)
                                {
                                    Iconvar_VarMinPic.Image = list.IconVarInformation.Icon_MinPic;
                                }
                            }
                            break;
                        case "Iconvar_InitialValue":
                            list.IconVarInformation.InitialValue = (UInt16)numberupdown.Value;
                            break;
                        #endregion
                        #region case TextDisplay
                        /*************************************文本显示**********************************/
                        case "TxtDisplay_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle TxtDisplay_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = TxtDisplay_X_Rec;
                            break;
                        case "TxtDisplay_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle TxtDisplay_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = TxtDisplay_Y_Rec;
                            break;
                        case "TxtDisplay_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle TxtDisplay_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = TxtDisplay_W_Rec;
                            break;
                        case "TxtDisplay_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle TxtDisplay_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = TxtDisplay_H_Rec;
                            break;
                        case "TxtDisplay_TxtLength":
                            list.TextDisplayInformation.Text_length = (UInt16)numberupdown.Value;
                            if ((int)TxtDisplay_TxtLength.Value == 0)
                            {
                                TxtDisplay_Initvalue.MaxLength = 100;
                            }
                            else
                            {
                                TxtDisplay_Initvalue.MaxLength = (int)TxtDisplay_TxtLength.Value;
                            }
                            break;
                        case "TxtDisplay_Font0_ID":
                            if(numberupdown.Value > 0 && numberupdown.Value < 23)
                            {
                                numberupdown.Value = 23;
                                list.TextDisplayInformation.Font0_ID = 23;
                            }
                            else
                            {
                                list.TextDisplayInformation.Font0_ID = (byte)numberupdown.Value;
                            }
                            break;
                        case "TxtDisplay_Font1_ID":
                            if (numberupdown.Value > 0 && numberupdown.Value < 23)
                            {
                                numberupdown.Value = 23;
                                list.TextDisplayInformation.Font1_ID = 23;
                            }
                            else
                            {
                                list.TextDisplayInformation.Font1_ID = (byte)numberupdown.Value;
                            }
                            break;
                        case "TxtDisplay_Xdirectionnum":
                            list.TextDisplayInformation.Font_X_Dots = (byte)numberupdown.Value;
                            break;
                        case "TxtDisplay_Ydirectionnum":
                            list.TextDisplayInformation.Font_Y_Dots = (byte)numberupdown.Value;
                            break;
                        case "TxtDisplay_Horispacing":
                            list.TextDisplayInformation.HOR_Dis = (byte)numberupdown.Value;
                            break;
                        case "TxtDisplay_verticalspacing":
                            list.TextDisplayInformation.VER_Dis = (byte)numberupdown.Value;
                            break;
                        #endregion
                        #region case RTC
                        /*************************************RTC显示**********************************/
                        case "RTC_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle RTC_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = RTC_X_Rec;
                            break;
                        case "RTC_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle RTC_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = RTC_Y_Rec;
                            break;
                        case "RTC_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle RTC_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = RTC_W_Rec;
                            break;
                        case "RTC_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle RTC_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = RTC_H_Rec;
                            break;
                        case "RTC_FontLib":
                            if (numberupdown.Value > 0 && numberupdown.Value < 23)
                            {
                                numberupdown.Value = 23;
                                list.RTCDisplayInformatin.Lib_ID = 23;
                            }
                            else
                            {
                                list.RTCDisplayInformatin.Lib_ID = (byte)numberupdown.Value;
                            }
                            break;
                        case "RTC_Xdirectionnum":
                            list.RTCDisplayInformatin.Font_X_Dots = (byte)numberupdown.Value;
                            break;
                        #endregion
                        #region case DataInput
                        case "DataInput_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle DataInput_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = DataInput_X_Rec;
                            break;
                        case "DataInput_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle DataInput_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = DataInput_Y_Rec;
                            break;
                        case "DataInput_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle DataInput_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = DataInput_W_Rec;
                            break;
                        case "DataInput_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle DataInput_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = DataInput_H_Rec;
                            break;
                        case "DataInput_ButtonEffectNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if (numberupdown.Value > -1)
                            {
                                list.DataInputInformation.ButtonEffectPic = DataInput_ButtonEffectPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.DataInputInformation.Pic_On = (int)numberupdown.Value;
                            }
                            else
                                list.DataInputInformation.ButtonEffectPic = DataInput_ButtonEffectPic.Image = null;
                            break;
                        case "DataInput_PageChangeNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if(numberupdown.Value > -1)
                            {
                                list.DataInputInformation.ButtonChangePagePic = DataInput_PageChangePic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.DataInputInformation.Pic_Next = (int)numberupdown.Value;
                            }
                            else
                                list.DataInputInformation.ButtonChangePagePic = DataInput_PageChangePic.Image = null;
                            break;
                        case "DataInput_IntrgerLength":
                            list.DataInputInformation.N_Int = (byte)numberupdown.Value;
                            break;
                        case "DataInput_DecimalLength":
                            list.DataInputInformation.N_Dot = (byte)numberupdown.Value;
                            break;
                        case "DataInput_FontID":
                            if (numberupdown.Value > 0 && numberupdown.Value < 23)
                            {
                                numberupdown.Value = 23;
                                list.DataInputInformation.Lib_ID = 23;
                            }
                            else
                            {
                                list.DataInputInformation.Lib_ID = (byte)numberupdown.Value;
                            }
                            break;
                        case "DataInput_FontSize":
                            list.DataInputInformation.Font_Hor = (byte)numberupdown.Value; 
                            break;
                        case "DataInput_KeyBoardAtPage":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if(numberupdown.Value > -1)
                            {
                                list.DataInputInformation.KeyBoardPic = DataInput_KeyBoardPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.DataInputInformation.PIC_KB = (int)numberupdown.Value;
                            }
                            else
                                list.DataInputInformation.KeyBoardPic = DataInput_KeyBoardPic.Image = null;
                            break;
                        case "DataInput_LimitedMax":
                            list.DataInputInformation.V_max = (long)numberupdown.Value;
                            break;
                        case "DataInput_LimitedMin":
                            list.DataInputInformation.V_min = (long)numberupdown.Value;
                            break;
                        case "DataInput_InputLoadData":
                            list.DataInputInformation.Return_DATA = (UInt16)numberupdown.Value;
                            break;
                        #endregion
                        #region case keyreturn
                        case "KeyRetuen_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle KeyRetuen_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = KeyRetuen_X_Rec;
                            break;
                        case "KeyRetuen_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle KeyRetuen_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = KeyRetuen_Y_Rec;
                            break;
                        case "KeyRetuen_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle KeyRetuen_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = KeyRetuen_W_Rec;
                            break;
                        case "KeyRetuen_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle KeyRetuen_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = KeyRetuen_H_Rec;
                            break;
                        case "KeyReturn_ButtonEffrctNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if(numberupdown.Value > -1)
                            {
                                list.KeyReturnInformation.ButtonEffectPic = KeyReturn_ButtonEffrctPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.KeyReturnInformation.Pic_On = (int)numberupdown.Value;
                            }
                            else
                                list.KeyReturnInformation.ButtonEffectPic = KeyReturn_ButtonEffrctPic.Image = null;
                            break;
                        case "KeyReturn_ButtonChangePageNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if(numberupdown.Value > -1)
                            {
                                list.KeyReturnInformation.ButtonChangePagePic = KeyReturn_ButtonChangePagePic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.KeyReturnInformation.Pic_Next = (int)numberupdown.Value;
                            }
                            else
                                list.KeyReturnInformation.ButtonChangePagePic = KeyReturn_ButtonChangePagePic.Image = null;
                            break;
                        case "KeyReturn_BitNum":
                            list.KeyReturnInformation.VP_Mode = (byte)(((byte)(numberupdown.Value) & 0x0f) | 0x10);
                            break;
                        #endregion
                        #region case QR Code
                         case "QR_Code_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle QR_Code_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = QR_Code_X_Rec;
                            break;
                        case "QR_Code_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle QR_Code_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = QR_Code_Y_Rec;
                            break;
                        case "QR_Code_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle QR_Code_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = QR_Code_W_Rec;
                            break;
                        case "QR_Code_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle QR_Code_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = QR_Code_H_Rec;
                            break;
                        case "QR_Code_Unit_Pixels":
                            list.QRCodeInformation.Unit_Pixels = (UInt16)numberupdown.Value;
                            break;
                          
                        #endregion
                        #region case PopupMenu
                        case "PopupMenu_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle PopupMenu_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = PopupMenu_X_Rec;
                            break;
                        case "PopupMenu_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle PopupMenu_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = PopupMenu_Y_Rec;
                            break;
                        case "PopupMenu_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle PopupMenu_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = PopupMenu_W_Rec;
                            break;
                        case "PopupMenu_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle PopupMenu_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = PopupMenu_H_Rec;
                            break;
                        case "PopupMenu_ButtonEffectNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if(numberupdown.Value > -1)
                            {
                                list.PopupMenuInformation.ButtonEffectPic = PopupMenu_ButtonEffectPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.PopupMenuInformation.Pic_On = (int)numberupdown.Value;
                            }
                            else
                                list.PopupMenuInformation.ButtonEffectPic = PopupMenu_ButtonEffectPic.Image = null;
                            break;
                        case "PopupMenu_MenuAtPage":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if(numberupdown.Value > -1)
                            {
                                list.PopupMenuInformation.PopupMenuPic = PopupMenu_MenuPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.PopupMenuInformation.Pic_Menu = (int)numberupdown.Value;
                            }
                            else
                                list.PopupMenuInformation.PopupMenuPic = PopupMenu_MenuPic.Image = null;
                            break;
                        case "PopupMenu_BitNum":
                            list.PopupMenuInformation.VP_Mode = (byte)(((byte)(numberupdown.Value) & 0x0f) | 0x10);
                            break;
                        #endregion
                        #region case ActionIcon
                        case "ActionIcon_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle ActionIcon_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = ActionIcon_X_Rec;
                            break;
                        case "ActionIcon_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle ActionIcon_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = ActionIcon_Y_Rec;
                            break;
                        case "ActionIcon_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle ActionIcon_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = ActionIcon_W_Rec;
                            break;
                        case "ActionIcon_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle ActionIcon_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = ActionIcon_H_Rec;
                            break;
                        case "ActionIcon_VSTOP":
                            list.ActionIconInforamtion.V_Stop = (UInt16)numberupdown.Value;
                            break;
                        case "ActionIcon_VSTART":
                            list.ActionIconInforamtion.V_Start = (UInt16)numberupdown.Value;
                            break;
                        case "ActionIcon_InitValue":
                            list.ActionIconInforamtion.InitlizValue = (UInt16)numberupdown.Value;
                            break;
                        #endregion
                        #region case IncreaseAdj
                        case "IncreaseAdj_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle IncreaseAdj_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = IncreaseAdj_X_Rec;
                            break;
                        case "IncreaseAdj_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle IncreaseAdj_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = IncreaseAdj_Y_Rec;
                            break;
                        case "IncreaseAdj_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle IncreaseAdj_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = IncreaseAdj_W_Rec;
                            break;
                        case "IncreaseAdj_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle IncreaseAdj_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = IncreaseAdj_H_Rec;
                            break;
                        case "IncreaseAdj_ButtonEffectNum":
                            numberupdown.Maximum = Images_Form.Pic_Number -1;
                            if(numberupdown.Value > -1)
                            {
                                list.IncreaseAdjInformation.Pic_OnImage = IncreaseAdj_ButtonEffectPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.IncreaseAdjInformation.Pic_On = (int)numberupdown.Value;
                            }                                
                            else
                                list.IncreaseAdjInformation.Pic_OnImage = IncreaseAdj_ButtonEffectPic.Image = null;
                            break;
                        case "IncreaseAdj_AdjStep":
                            list.IncreaseAdjInformation.Adj_Step = (UInt16)numberupdown.Value;
                            break;
                        case "IncreaseAdj_VMin":
                            list.IncreaseAdjInformation.V_Min = (UInt16)numberupdown.Value;
                            break;
                        case "IncreaseAdj_VMax":
                            list.IncreaseAdjInformation.V_Max = (UInt16)numberupdown.Value;
                            break;
                        case "IncreaseAdj_BitNum":
                            list.IncreaseAdjInformation.VP_Mode =  (byte)(((byte)(numberupdown.Value) & 0x0f) | 0x10);
                            break;
                        #endregion
                        #region case SlideAdj
                        case "SlideAdj_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle SlideAdj_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = SlideAdj_X_Rec;
                            break;
                        case "SlideAdj_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle SlideAdj_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = SlideAdj_Y_Rec;
                            break;
                        case "SlideAdj_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle SlideAdj_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = SlideAdj_W_Rec;
                            break;
                        case "SlideAdj_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle SlideAdj_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = SlideAdj_H_Rec;
                            break;
                        case "SlideAdj_VBegin":
                            list.SlideAdjInformation.V_begin = (UInt16)numberupdown.Value;
                            break;
                        case "SlideAdj_VEnd":
                            list.SlideAdjInformation.V_end = (UInt16)numberupdown.Value;
                            break;
                        #endregion
                        #region case ArtFont
                        case "ArtFont_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle ArtFont_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = ArtFont_X_Rec;
                            break;
                        case "ArtFont_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle ArtFont_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = ArtFont_Y_Rec;
                            break;
                        case "ArtFont_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle ArtFont_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = ArtFont_W_Rec;
                            break;
                        case "ArtFont_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle ArtFont_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = ArtFont_H_Rec;
                            break;
                        case "ArtFont_IntgetLength":
                            list.ArtFontInformation.Integer_Length = (byte)numberupdown.Value;
                            break;
                        case "ArtFont_DecimalLength":
                            list.ArtFontInformation.Decimal_Length = (byte)numberupdown.Value;
                            break;
                        case "ArtFont_InitValue":
                            list.ArtFontInformation.Init_Value = (long)numberupdown.Value;
                            break;
                        #endregion
                        #region case sliderdisplay
                        case "SlideDisplay_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle SlideDisplay_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = SlideDisplay_X_Rec;
                            if(list.SlideDisplayInformation.Mode == 0) //横向调节
                            {
                                list.SlideDisplayInformation.X_begain = (UInt16)numberupdown.Value;
                                list.SlideDisplayInformation.X_end = (UInt16)(numberupdown.Value + list.Rectangle.Width);
                            }
                            break;
                        case "SlideDisplay_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle SlideDisplay_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = SlideDisplay_Y_Rec;
                            if(list.SlideDisplayInformation.Mode == 1)
                            {
                                list.SlideDisplayInformation.X_begain = (UInt16)numberupdown.Value;
                                list.SlideDisplayInformation.X_end = (UInt16)(numberupdown.Value + list.Rectangle.Height);
                            }
                            break;
                        case "SlideDisplay_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle SlideDisplay_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = SlideDisplay_W_Rec;
                            if (list.SlideDisplayInformation.Mode == 0) //横向调节
                            {
                                list.SlideDisplayInformation.X_end = (UInt16)(numberupdown.Value + list.Rectangle.X);
                            }
                            break;
                        case "SlideDisplay_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle SlideDisplay_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = SlideDisplay_H_Rec;
                            if (list.SlideDisplayInformation.Mode == 1)
                            {
                                list.SlideDisplayInformation.X_end = (UInt16)(numberupdown.Value + list.Rectangle.Y);
                            }
                            break;
                        case "SlideDisplay_Vbegin":
                            list.SlideDisplayInformation.V_begain = (byte)numberupdown.Value;
                            break;
                        case "SlideDisplay_Vend":
                            list.SlideDisplayInformation.V_end = (byte)numberupdown.Value;
                            break;
                        case "SlideDisplay_X_Adj":
                            list.SlideDisplayInformation.X_adj = (byte)numberupdown.Value;
                            break;
                        case "SlideDisplay_InitValue":
                            list.SlideDisplayInformation.InitVal = (short)numberupdown.Value;
                            break;
                        #endregion
                        #region case IconRotation
                        case "IconRotation_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle IconRotation_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = IconRotation_X_Rec;
                            list.IconRotationInformation.Xc = (UInt16)list.Rectangle.X;
                            break;
                        case "IconRotation_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle IconRotation_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = IconRotation_Y_Rec;
                            list.IconRotationInformation.Yc = (UInt16)list.Rectangle.Y;
                            break;
                        case "IconRotation_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle IconRotation_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = IconRotation_W_Rec;
                            break;
                        case "IconRotation_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle IconRotation_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = IconRotation_H_Rec;
                            break;
                        case "IconRotation_Xc":
                            list.IconRotationInformation.Icon_Xc = (UInt16)numberupdown.Value;
                            break;
                        case "IconRotation_Yc":
                            list.IconRotationInformation.Icon_Yc = (UInt16)numberupdown.Value;
                            break;
                        case "IconRotation_Vbegin":
                            list.IconRotationInformation.V_begain = (UInt16)numberupdown.Value;
                            break;
                        case "IconRotation_Vend":
                            list.IconRotationInformation.V_end = (UInt16)numberupdown.Value;
                            break;
                        case "IconRotation_Albegin":
                            list.IconRotationInformation.AL_begain = (UInt16)numberupdown.Value;
                            break;
                        case "IconRotation_Alend":
                            list.IconRotationInformation.AL_end = (UInt16)numberupdown.Value;
                            break;
                        case "IconRotation_InitValue":
                            list.IconRotationInformation.Init_Value = (int)numberupdown.Value;
                            break;
                        #endregion 
                        #region case ClockDisplay
                        case "ClockDisplay_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle ClockDisplay_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = ClockDisplay_X_Rec;
                            break;
                        case "ClockDisplay_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle ClockDisplay_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = ClockDisplay_Y_Rec;
                            break;
                        case "ClockDisplay_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle ClockDisplay_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = ClockDisplay_W_Rec;
                            break;
                        case "ClockDisplay_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle ClockDisplay_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = ClockDisplay_H_Rec;
                            break;
                        #endregion
                        #region case GBK 
                        case "GBK_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle GBK_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = GBK_X_Rec;
                            break;
                        case "GBK_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle GBK_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = GBK_Y_Rec;
                            break;
                        case "GBK_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle GBK_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = GBK_W_Rec;
                            break;
                        case "GBK_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle GBK_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = GBK_H_Rec;
                            break;
                        case "GBK_ButtonEffectNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if (numberupdown.Value > -1)
                            {
                                list.GBKInformation.Pic_OnPic = GBK_ButtonEffectPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.GBKInformation.Pic_On = (int)numberupdown.Value;
                            }
                            else
                                list.GBKInformation.Pic_OnPic = GBK_ButtonEffectPic.Image = null;
                            break;
                        case "GBK_PageChangeNum":
                            numberupdown.Maximum = Images_Form.Pic_Number;
                            if (numberupdown.Value > -1)
                            {
                                list.GBKInformation.Pic_NextPic = GBK_PageChangePic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.GBKInformation.Pic_Next = (int)numberupdown.Value;
                            }
                            else
                                list.GBKInformation.Pic_NextPic = GBK_PageChangePic.Image = null;
                            break;
                        case "GBK_TextLength":
                            list.GBKInformation.VP_Len_Max = (int)GBK_TextLength.Value;
                            break;
                        case "GBK_DisplayFont":
                            if ((byte)numberupdown.Value >= 1 && (byte)numberupdown.Value <= 22)
                                list.GBKInformation.Lib_GBK1 = 23;
                            else
                                list.GBKInformation.Lib_GBK1 = (byte)numberupdown.Value;
                            break;
                        case "GBK_DisplayFontSize":
                            list.GBKInformation.Font_Scale1 = (byte)GBK_DisplayFontSize.Value;
                            break;
                        case "GBK_InputProcessFont":
                            if ((byte)numberupdown.Value >= 1 && (byte)numberupdown.Value <= 22)
                                list.GBKInformation.Lib_GBK2 = 23;
                            else
                                list.GBKInformation.Lib_GBK2 = (byte)numberupdown.Value;
                            break;
                        case "GBK_InputProcessFontSize":
                            list.GBKInformation.Font_Scale2 = (byte)GBK_InputProcessFontSize.Value;
                            break;
                        case "GBK_DisplaySpacing":
                            list.GBKInformation.Scan_Dis = (byte)GBK_DisplaySpacing.Value;
                            break;
                        case "GBK_KeyBoardAtPage":
                            numberupdown.Maximum = Images_Form.Pic_Number;
                            if (numberupdown.Value > -1)
                            {
                                list.GBKInformation.PIC_KBPic = GBK_KeyBoardPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.GBKInformation.PIC_KB = (int)GBK_KeyBoardAtPage.Value;
                            }
                            break;
                        #endregion
                        #region case ASCII
                        case "ASCII_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle ASCII_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = ASCII_X_Rec;
                            break;
                        case "ASCII_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle ASCII_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = ASCII_Y_Rec;
                            break;
                        case "ASCII_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle ASCII_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = ASCII_W_Rec;
                            break;
                        case "ASCII_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle ASCII_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = ASCII_H_Rec;
                            break;
                        case "ASCII_ButtonEffectNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if (numberupdown.Value > -1)
                            {
                                list.ASCIIInformation.Pic_OnPic = ASCII_ButtonEffectPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.ASCIIInformation.Pic_On = (int)numberupdown.Value;
                            }
                            else
                                list.ASCIIInformation.Pic_OnPic = ASCII_ButtonEffectPic.Image = null;
                            break;
                        case "ASCII_PageChangeNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if (numberupdown.Value > -1)
                            {
                                list.ASCIIInformation.Pic_NextPic = ASCII_PageChangePic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.ASCIIInformation.Pic_Next = (int)numberupdown.Value;
                            }
                            else
                                list.GBKInformation.Pic_NextPic = ASCII_PageChangePic.Image = null;
                            break;
                        case "ASCII_TextLength":
                            list.ASCIIInformation.VP_Len_Max = (int)ASCII_TextLength.Value;
                            break;
                        case "ASCII_DisplayFont":
                            if ((byte)ASCII_DisplayFont.Value >= 1 && (byte)ASCII_DisplayFont.Value <= 22)
                                list.ASCIIInformation.Lib_ID = 23;
                            else
                                list.ASCIIInformation.Lib_ID = (byte)ASCII_DisplayFont.Value;
                            
                            break;
                        case "ASCII_XDirection":
                            list.ASCIIInformation.Font_Hor = (byte)ASCII_XDirection.Value;
                            break;
                        case "ASCII_YDirection":
                            list.ASCIIInformation.Font_Ver = (byte)ASCII_YDirection.Value;
                            break;
                        case "ASCII_KeyBoardAtPage":
                            numberupdown.Maximum = Images_Form.Pic_Number;
                            if (numberupdown.Value > -1)
                            {
                                list.ASCIIInformation.PIC_KBPic = ASCII_KeyBoardPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.ASCIIInformation.PIC_KB = (int)numberupdown.Value;
                            }
                            else
                                list.ASCIIInformation.PIC_KBPic = ASCII_KeyBoardPic.Image = null;
                            break;
                        #endregion
                        #region case TouchState
                        case "TouchState_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle TouchState_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = TouchState_X_Rec;
                            break;
                        case "TouchState_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle TouchState_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = TouchState_Y_Rec;
                            break;
                        case "TouchState_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle TouchState_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = TouchState_W_Rec;
                            break;
                        case "TouchState_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle TouchState_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = TouchState_H_Rec;
                            break;
                        case "TouchState_ButtonEffectNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if (numberupdown.Value > -1)
                            {
                                list.TouchStateInformation.Pic_OnPic = TouchState_ButtonEffectPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.TouchStateInformation.Pic_On = (int)numberupdown.Value;
                            }
                            else
                                list.TouchStateInformation.Pic_OnPic = TouchState_ButtonEffectPic.Image = null;
                            break;
                        case "TouchState_PageSwitchNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if (numberupdown.Value > -1)
                            {
                                list.TouchStateInformation.Pic_NextPic = TouchState_PageSwitchPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.TouchStateInformation.Pic_Next = (int)numberupdown.Value;
                            }
                            else
                                list.TouchStateInformation.Pic_NextPic = TouchState_PageSwitchPic.Image = null;
                            break;
                        case "TouchState_FirstLength":
                            list.TouchStateInformation.LEN1 = (byte)numberupdown.Value;
                            break;
                        case "TouchState_ContinueLength":
                            list.TouchStateInformation.LEN2 = (byte)numberupdown.Value;
                            break;
                        case "TouchState_LoseLength":
                            list.TouchStateInformation.LEN3 = (byte)numberupdown.Value;
                            break;
                        #endregion
                        #region  case RTCset
                        case "RTCset_X":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.Width;
                            Rectangle RTCset_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = RTCset_X_Rec;
                            break;
                        case "RTCset_Y":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Height;
                            Rectangle RTCset_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = RTCset_Y_Rec;
                            break;
                        case "RTCset_W":
                            //numberupdown.Maximum = fDisplay.designer1.Width - list.Rectangle.X;
                            Rectangle RTCset_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = RTCset_W_Rec;
                            break;
                        case "RTCset_H":
                            //numberupdown.Maximum = fDisplay.designer1.Height - list.Rectangle.Y;
                            Rectangle RTCset_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = RTCset_H_Rec;
                            break;
                        case "RTCset_ButtonEffectNum":
                            numberupdown.Maximum = Images_Form.Pic_Number - 1;
                            if (numberupdown.Value > -1)
                            {
                                list.RTCsetInformation.Pic_OnPic = RTCset_ButtonEffectPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.RTCsetInformation.Pic_On = (int)numberupdown.Value;
                            }
                            else
                                list.RTCsetInformation.Pic_OnPic = RTCset_ButtonEffectPic.Image = null;
                            break;
                        case "RTCset_KeyBoardAtPage":
                            numberupdown.Maximum = Images_Form.Pic_Number;
                            if (numberupdown.Value > -1)
                            {
                                list.RTCsetInformation.PIC_KBPic = RTCset_KeyBoardPic.Image = Images_Form.picname[((int)numberupdown.Value)].image;
                                list.RTCsetInformation.PIC_KB = (int)numberupdown.Value;
                                list.RTCsetInformation.KB_Source =
                                    (byte)((list.RTCsetInformation.PIC_KB == Main_Form.presentpage_num) ? (0) : (list.RTCsetInformation.PIC_KB));
                            }
                            else
                                list.RTCsetInformation.PIC_KBPic = RTCset_KeyBoardPic.Image = null;
                            break;
                        case "RTCset_FontID":
                            list.RTCsetInformation.Lib_ID = (byte)numberupdown.Value;
                            break;
                        case "RTCset_FontSize":
                            list.RTCsetInformation.Font_Hor = (byte)numberupdown.Value;
                            break;
                        #endregion
                        #region case BasicGra
                        case "BasicGra_X":
                            Rectangle BasicGra_X_Rec = new Rectangle((int)numberupdown.Value, list.Rectangle.Y, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = BasicGra_X_Rec;
                            break;
                        case "BasicGra_Y":
                            Rectangle BasicGra_Y_Rec = new Rectangle(list.Rectangle.X, (int)numberupdown.Value, list.Rectangle.Width, list.Rectangle.Height);
                            list.Rectangle = BasicGra_Y_Rec;
                            break;
                        case "BasicGra_W":
                            Rectangle BasicGra_W_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, (int)numberupdown.Value, list.Rectangle.Height);
                            list.Rectangle = BasicGra_W_Rec;
                            break;
                        case "BasicGra_H":
                            Rectangle BasicGra_H_Rec = new Rectangle(list.Rectangle.X, list.Rectangle.Y, list.Rectangle.Width, (int)numberupdown.Value);
                            list.Rectangle = BasicGra_H_Rec;
                            break;
                        case "BasicGra_DashSet1":
                           list.BasicGraInformation.Dash_Set_1 = (byte)numberupdown.Value;
                            break;
                        case "BasicGra_DashSet2":
                            list.BasicGraInformation.Dash_Set_2 = (byte)numberupdown.Value;
                            break;
                        case "BasicGra_DashSet3":
                            list.BasicGraInformation.Dash_Set_3 = (byte)numberupdown.Value;
                            break;
                        case "BasicGra_DashSet4":
                            list.BasicGraInformation.Dash_Set_4 = (byte)numberupdown.Value;
                            break;
                            #endregion
                    }
                    fDisplay.designer1.Refresh();
                    break;
                }
            }

        }
        /// <summary>
        /// ComBox内容改变
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void ComBox_SelectChange(object sender, EventArgs e)
        {
            ComboBox combox = sender as ComboBox;
            if(combox.Text == "")
            {
                return;
            }
            fDisplay = Display.GetSingle();
            byte temp;
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    switch (combox.Name)
                    {
                        case "TxtDisplay_CodeingMode":
                            temp = list.TextDisplayInformation.Encode_Mode;
                            temp &= 0x80;
                            temp |= (byte)combox.SelectedIndex;
                            list.TextDisplayInformation.Encode_Mode = temp;
                            break;
                        case "DataVar_AlignStyle":
                            list.DataVarInfo.Font_Align = (byte)combox.SelectedIndex;
                            break;
                        case "DataVar_VarType":
                            list.DataVarInfo.Var_Type = (byte)combox.SelectedIndex;
                            switch (list.DataVarInfo.Var_Type)
                            {
                                case 0:
                                    DataVar_InitialValue.Maximum = short.MaxValue;
                                    DataVar_InitialValue.Minimum = short.MinValue;
                                    break;
                                case 1:
                                    DataVar_InitialValue.Maximum = int.MaxValue;
                                    DataVar_InitialValue.Minimum = int.MinValue;
                                    break;
                                case 2:
                                case 3:
                                    DataVar_InitialValue.Maximum = byte.MaxValue;
                                    DataVar_InitialValue.Minimum = sbyte.MinValue;
                                    break;
                                case 4:
                                    DataVar_InitialValue.Maximum = long.MaxValue;
                                    DataVar_InitialValue.Minimum = long.MinValue;
                                    break;
                                case 5:
                                    DataVar_InitialValue.Maximum = UInt16.MaxValue;
                                    DataVar_InitialValue.Minimum = UInt16.MinValue;
                                    break;
                                case 6:
                                    DataVar_InitialValue.Maximum = UInt32.MaxValue;
                                    DataVar_InitialValue.Minimum = UInt32.MinValue;
                                    break;
                            }
                            break;
                        case "Iconvar_IconFile":
                            list.IconVarInformation.Icon_FileName = Iconvar_IconFile.Text;
                            list.IconVarInformation.Icon_Lib = ReturnNum(Iconvar_IconFile.Text);
                            list.IconVarInformation.Iconfileselect = Iconvar_IconFile.SelectedIndex;
                            break;
                        case "Iconvar_Mode":
                            list.IconVarInformation.Mode = (byte)combox.SelectedIndex;
                            break;
                        #region case DataInput
                        case "DataInput_VarType":
                            list.DataInputInformation.V_Type = (byte)combox.SelectedIndex;
                            switch (list.DataInputInformation.V_Type)
                            {
                                case 0:
                                    DataInput_LimitedMin.Maximum = short.MaxValue;
                                    DataInput_LimitedMin.Minimum = short.MinValue;
                                    DataInput_LimitedMax.Maximum = short.MaxValue;
                                    DataInput_LimitedMax.Minimum = short.MinValue;
                                    break;
                                case 1:
                                    DataInput_LimitedMin.Maximum = int.MaxValue;
                                    DataInput_LimitedMin.Minimum = int.MinValue;
                                    DataInput_LimitedMax.Maximum = int.MaxValue;
                                    DataInput_LimitedMax.Minimum = int.MinValue;
                                    break;
                                case 2:
                                case 3:
                                    DataInput_LimitedMin.Maximum = byte.MaxValue;
                                    DataInput_LimitedMin.Minimum = sbyte.MinValue;
                                    DataInput_LimitedMax.Maximum = byte.MaxValue;
                                    DataInput_LimitedMax.Minimum = sbyte.MinValue;

                                    break;
                                case 4:
                                    DataInput_LimitedMin.Maximum = long.MaxValue;
                                    DataInput_LimitedMin.Minimum = long.MinValue;
                                    DataInput_LimitedMax.Maximum = long.MaxValue;
                                    DataInput_LimitedMax.Minimum = long.MinValue;
                                    break;
                            }
                            break;
                        case "DataInput_CursorColor":
                            list.DataInputInformation.CurousColor = (byte)combox.SelectedIndex;
                            break;
                        case "DataInput_DisplayStyle":
                            list.DataInputInformation.Hide_En = (byte)combox.SelectedIndex;
                            break;
                        case "DataInput_KeyBoardPosition":
                            list.DataInputInformation.KB_Source = (byte)combox.SelectedIndex;
                            break;
                        #endregion
                        #region case ActionIcon
                        case "ActionIcon_IconFile":
                            if (ActionIcon_IconFile.Text != "")
                            {
                                list.ActionIconInforamtion.Icon_FileName = ActionIcon_IconFile.Text;
                                list.ActionIconInforamtion.Iconfileselect = ActionIcon_IconFile.SelectedIndex;
                                list.ActionIconInforamtion.Icon_Lib = ReturnNum(ActionIcon_IconFile.Text);
                            }  
                            else
                            {
                                list.ActionIconInforamtion.Icon_FileName = "";
                                list.ActionIconInforamtion.Iconfileselect = -1;
                            }
                            break;
                        case "ActionIcon_ShowMode":
                            if (ActionIcon_ShowMode.Text != "")
                            {
                                list.ActionIconInforamtion.strMode = ActionIcon_ShowMode.Text;
                               
                                list.ActionIconInforamtion.Mode = (byte)ActionIcon_ShowMode.SelectedIndex;
                            }
                            break;
                        #endregion
                        #region case increaseAdj
                        case "IncreaseAdj_AdjMode":
                            list.IncreaseAdjInformation.Adj_Mode = (byte)IncreaseAdj_AdjMode.SelectedIndex;
                            break;
                        case "IncreaseAdj_ReturnMode":
                            list.IncreaseAdjInformation.Return_Mode = (byte)IncreaseAdj_ReturnMode.SelectedIndex;
                            break;
                        case "IncreaseAdj_KeyMode":
                            list.IncreaseAdjInformation.Key_Mode = (byte)IncreaseAdj_KeyMode.SelectedIndex;
                            break;
                        #endregion
                        #region case SlideAdj
                        case "SlideAdj_DataReturnMode":
                            temp = list.SlideAdjInformation.Adj_Mode;
                            temp = (byte)(temp & 0x0F);
                            temp |= (byte)(SlideAdj_DataReturnMode.SelectedIndex << 4);
                            list.SlideAdjInformation.Adj_Mode = temp;
                            break;
                        case "SlideAdj_AdjMode":
                            temp = list.SlideAdjInformation.Adj_Mode;
                            temp = (byte)(temp & 0xF0);
                            temp |= (byte)(SlideAdj_AdjMode.SelectedIndex);
                            list.SlideAdjInformation.Adj_Mode = temp;
                            break;
                        #endregion
                        #region case ArtFile
                        case "ArtFont_IconFile":
                            list.ArtFontInformation.Icon_FileName = combox.Text;
                            list.ArtFontInformation.Icon_Lib = ReturnNum(combox.Text);
                            list.ArtFontInformation.Icon_SelectIndex = combox.SelectedIndex;
                            break;
                        case "ArtFont_ShowMode":
                            list.ArtFontInformation.Icon_Mode = (byte)combox.SelectedIndex;
                            break;
                        case "ArtFont_VarType":
                            list.ArtFontInformation.Var_Type = (byte)combox.SelectedIndex;
                            switch (list.ArtFontInformation.Var_Type)
                            {
                                case 0:
                                    ArtFont_InitValue.Maximum = short.MaxValue;
                                    ArtFont_InitValue.Minimum = short.MinValue;
                                    break;
                                case 1:
                                    ArtFont_InitValue.Maximum = int.MaxValue;
                                    ArtFont_InitValue.Minimum = int.MinValue;
                                    break;
                                case 2:
                                case 3:
                                    ArtFont_InitValue.Maximum = byte.MaxValue;
                                    ArtFont_InitValue.Minimum = sbyte.MinValue;
                                    break;
                                case 4:
                                    ArtFont_InitValue.Maximum = long.MaxValue;
                                    ArtFont_InitValue.Minimum = long.MinValue;
                                    break;
                                case 5:
                                    ArtFont_InitValue.Maximum = UInt16.MaxValue;
                                    ArtFont_InitValue.Minimum = UInt16.MinValue;
                                    break;
                                case 6:
                                    ArtFont_InitValue.Maximum = UInt32.MaxValue;
                                    ArtFont_InitValue.Minimum = UInt32.MinValue;
                                    break;
                            }
                            break;
                        case "ArtFont_Align":
                            list.ArtFontInformation.Align_Mode = (byte)combox.SelectedIndex;
                            break;
                        #endregion
                        #region case SliderDispaly
                        case "SlideDisplay_Mode":
                            list.SlideDisplayInformation.Mode = (byte)combox.SelectedIndex;
                            if(list.SlideDisplayInformation.Mode == 0)
                            {
                                list.SlideDisplayInformation.X_begain = (UInt16)list.Rectangle.X;
                                list.SlideDisplayInformation.X_end = (UInt16)(list.Rectangle.X + list.Rectangle.Width);
                            }
                            else
                            {
                                list.SlideDisplayInformation.X_begain = (UInt16)list.Rectangle.Y;
                                list.SlideDisplayInformation.X_end = (UInt16)(list.Rectangle.Y + list.Rectangle.Height);
                            }
                            break;
                        case "SlideDisplay_IconMode":
                            list.SlideDisplayInformation.Icon_Mode = (byte)combox.SelectedIndex;
                            break;
                        case "SlideDisplay_VPDataMode":
                            list.SlideDisplayInformation.VP_DATA_Mode = (byte)combox.SelectedIndex;
                            switch(list.SlideDisplayInformation.VP_DATA_Mode)
                            {
                                case 0:
                                    SlideDisplay_InitValue.Maximum = short.MaxValue;
                                    SlideDisplay_InitValue.Minimum = short.MinValue;
                                    break;
                                case 1:
                                case 2:
                                    SlideDisplay_InitValue.Maximum = byte.MaxValue;
                                    SlideDisplay_InitValue.Minimum = sbyte.MinValue;
                                    break;
                            }
                            break;
                        case "SlideDisplay_IconLib":

                            list.SlideDisplayInformation.Icon_FileName = combox.Text;
                            list.SlideDisplayInformation.Icon_Lib = ReturnNum(combox.Text);
                            list.SlideDisplayInformation.Icon_SelectIndex = combox.SelectedIndex;
                            break;
                        #endregion
                        #region case IconRotation
                        case "IconRotation_IconFile":
                            list.IconRotationInformation.Icon_FileName = IconRotation_IconFile.Text;
                            list.IconRotationInformation.Lib_ID = ReturnNum(IconRotation_IconFile.Text);
                            list.IconRotationInformation.Icon_SelectIndex = combox.SelectedIndex;
                            break;
                        case "IconRotation_DisplayMode":
                            list.IconRotationInformation.Mode = (byte)combox.SelectedIndex;
                            break;
                        case "IconRotation_VPMode":
                            list.IconRotationInformation.VP_Mode = (byte)combox.SelectedIndex;
                            switch (list.IconRotationInformation.VP_Mode)
                            {
                                case 0:
                                    IconRotation_InitValue.Maximum = short.MaxValue;
                                    IconRotation_InitValue.Minimum = short.MinValue;
                                    break;
                                case 1:
                                case 2:
                                    IconRotation_InitValue.Maximum = byte.MaxValue;
                                    IconRotation_InitValue.Minimum = sbyte.MinValue;
                                    break;
                            }
                            break;
                        #endregion
                        #region case ClockDisplay
                        case "ClockDisplay_IconFile":
                            list.ClockDisplayInformation.Icon_FileName = combox.Text;
                            list.ClockDisplayInformation.Icon_Lib = ReturnNum(combox.Text);
                            list.ClockDisplayInformation.Icon_SelectIndex = combox.SelectedIndex;
                            break;
                        #endregion
                        #region case GBK
                        case "GBK_InputMode":
                            list.GBKInformation.Scan_Mode = (byte)combox.SelectedIndex;
                            break;
                        case "GBK_DispalyMode":
                            list.GBKInformation.PY_Disp_Mode = (byte)combox.SelectedIndex;
                            break;
                        case "GBK_CursorColor":
                            list.GBKInformation.Cusor_Color = (byte)combox.SelectedIndex;
                            break;
                        case "GBK_KeyBoardPosition":
                            list.GBKInformation.KB_Source = (byte)combox.SelectedIndex;
                            break;
                        #endregion
                        #region case ASCII
                        case "ASCII_InputMode":
                            list.ASCIIInformation.Scan_Mode = (byte)combox.SelectedIndex;
                            break;
                        case "ASCII_CursorColor":
                            list.ASCIIInformation.Cusor_Color = (byte)combox.SelectedIndex;
                            break;
                        case "ASCII_InputDisMode":
                            list.ASCIIInformation.DISPLAY_EN = (byte)combox.SelectedIndex;
                            break;
                        case "ASCII_KeyBoardPosition":
                            list.ASCIIInformation.KB_Source = (byte)combox.SelectedIndex;
                            break;
                        #endregion
                        #region case TouchState
                        case "TouchState_FirstMode":
                            list.TouchStateInformation.TP_ON_Mode = (byte)combox.SelectedIndex;
                            break;
                        case "TouchState_ContinueMode":
                            list.TouchStateInformation.TP_ON_Continue_Mode = (byte)combox.SelectedIndex;
                            break;
                        case "TouchState_LoseMode":
                            list.TouchStateInformation.TP_OFF_Mode = (byte)combox.SelectedIndex;
                            break;
                        #endregion
                        case "RTCset_CursorColor":
                            list.RTCsetInformation.Cusor_Color = (byte)combox.SelectedIndex;
                            break;
                    }
                    break;
                }
            }

        }
        private byte ReturnNum(string str)//返回前面几个数字
        {
            byte Num = 0;
            Regex r = new Regex(@"[^\d]");
            int index = r.Match(str).Index;
            try
            {
                Num = Convert.ToByte(str.Substring(0, index));
            }
            catch
            {
                MessageBox.Show("Icon Lib position too high");
                return 0;
            }
            return Num;
        }
        void Combox_MouseClick(object sender, EventArgs e)
        {
            ComboBox combox = sender as ComboBox;
            fDisplay = Display.GetSingle();
            GetIconFile();
            combox.Items.Clear();  //清空内容
            foreach (iconfileinfo ifi in iconfileinfos)
            {
                if (ifi.iconfile_name == null)
                {
                    break;
                }
                combox.Items.Add(ifi.iconfile_name);
            }
        }
        /// <summary>
        /// 不能键入小数点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void NumericUpDown_ForbidDecimal(object sender, KeyPressEventArgs e)
        {
            if(e.KeyChar == '.')
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// 只能输入十进制数字的TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextBox_DecOnly(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || (e.KeyChar == (char)8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        /// <summary>
        /// 只能输入十六进制的TextBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void TextBox_HexOnly(object sender, KeyPressEventArgs e)
        {
            if (Char.IsNumber(e.KeyChar) || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F') || (e.KeyChar == (char)8))
            {
                e.Handled = false;
            }
            else
            {
                e.Handled = true;
            }
        }
        public void PIC_GBoxShow(PIC_Obj obj)
        {
            present_obj = obj;
            switch (Current_Obj)
            {
                case PIC_Obj.NONE:
                    GBox_PageProperties.Visible = false;
                    break;
                case PIC_Obj.basictouch:
                    GBox_BaseTouch.Visible = false;
                    break;
                case PIC_Obj.data_display:
                    GBox_DataVar.Visible = false;
                    break;
                case PIC_Obj.icon_display:
                    GBox_IconVar.Visible = false;
                    break;
                case PIC_Obj.text_dispaly:
                    GBox_TxtDisplay.Visible = false;
                    break;
                case PIC_Obj.rtc_display:
                    GBox_RTC.Visible = false;
                    break;
                case PIC_Obj.QR_display:
                    GBox_QR.Visible = false;
                    break;
                case PIC_Obj.datainput:
                    GBox_VarInput.Visible = false;
                    break;
                case PIC_Obj.menu_display:
                    GBox_MeunDis.Visible = false;
                    break;
                case PIC_Obj.aniicon_display:
                    GBox_ActionIcon.Visible = false;
                    break;
                case PIC_Obj.keyreturn:
                    GBox_KeyReturn.Visible = false;
                    break;
                case PIC_Obj.increadj:
                    GBox_IncreaseAdj.Visible = false;
                    break;
                case PIC_Obj.sliadj:
                    GBox_SlideAdj.Visible = false;
                    break;
                case PIC_Obj.artfont:
                    GBox_ArtFont.Visible = false;
                    break;
                case PIC_Obj.slidis:
                    GBox_SlideDisplay.Visible = false;
                    break;
                case PIC_Obj.iconrota:
                    GBox_IconSpin.Visible = false;
                    break;
                case PIC_Obj.clockdisplay:
                    GBox_clock.Visible = false;
                    break;
                case PIC_Obj.GBK:
                    GBox_GBK.Visible = false;
                    break;
                case PIC_Obj.ASCII:
                    GBox_ASCII.Visible = false;
                    break;
                case PIC_Obj.TouchState:
                    GBox_TouchState.Visible = false;
                    break;
                case PIC_Obj.RTC_Set:
                    GBox_RTCset.Visible = false;
                    break;
                case PIC_Obj.BasicGra:
                    GBox_BasicGra.Visible = false;
                    break;
            }
            switch (obj)
            {
                case PIC_Obj.NONE:
                    GBox_PageProperties.Visible = true;
                    break;
                case PIC_Obj.basictouch:
                    GBox_BaseTouch.Visible = true;
                    break;
                case PIC_Obj.data_display:
                    GBox_DataVar.Visible = true;
                    break;
                case PIC_Obj.icon_display:
                    GBox_IconVar.Visible = true;
                    break;
                case PIC_Obj.text_dispaly:
                    GBox_TxtDisplay.Visible = true;
                    break;
                case PIC_Obj.rtc_display:
                    GBox_RTC.Visible = true;
                    break;
                case PIC_Obj.QR_display:
                    GBox_QR.Visible = true;
                    break;
                case PIC_Obj.datainput:
                    GBox_VarInput.Visible = true;
                    break;
                case PIC_Obj.menu_display:
                    GBox_MeunDis.Visible = true;
                    break;
                case PIC_Obj.aniicon_display:
                    GBox_ActionIcon.Visible = true;
                    break;
                case PIC_Obj.keyreturn:
                    GBox_KeyReturn.Visible = true;
                    break;
                case PIC_Obj.increadj:
                    GBox_IncreaseAdj.Visible = true;
                    break;
                case PIC_Obj.sliadj:
                    GBox_SlideAdj.Visible = true;
                    break;
                case PIC_Obj.artfont:
                    GBox_ArtFont.Visible = true;
                    break;
                case PIC_Obj.slidis:
                    GBox_SlideDisplay.Visible = true;
                    break;
                case PIC_Obj.iconrota:
                    GBox_IconSpin.Visible = true;
                    break;
                case PIC_Obj.clockdisplay:
                    GBox_clock.Visible = true;
                    break;
                case PIC_Obj.GBK:
                    GBox_GBK.Visible = true;
                    break;
                case PIC_Obj.ASCII:
                    GBox_ASCII.Visible = true;
                    break;
                case PIC_Obj.TouchState:
                    GBox_TouchState.Visible = true;
                    break;
                case PIC_Obj.RTC_Set:
                    GBox_RTCset.Visible = true;
                    break;
                case PIC_Obj.BasicGra:
                    GBox_BasicGra.Visible = true;
                    break;
            }
            Current_Obj = obj;
        }
        #region BsseTouch
        /// <summary>
        /// 名称定义
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseTouch_NameDefine_TextChanged(object sender, EventArgs e)
        {
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    if (BaseTouch_NameDefine.Text != null)
                    {
                        list.Name_define = BaseTouch_NameDefine.Text;
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 切换页面选项
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseTouch_ButtonChangePageSelect_Click(object sender,EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this,2);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        list.BaseTouchInfo.PageChange_Image = BaseTouch_ButtonChangePagePic.Image;
                        list.BaseTouchInfo.Pic_Next = (int)BaseTouch_ButtonChangePageNum.Value;
                        BaseTouch_ButtonChangePageCheckBox.Checked = false;
                        break;
                    }
                }
            }
        }
        /// <summary>
        /// 是否进行页面切换的checkBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseTouch_ButtonChangePageCheckBox_CheckedChanged(object sender, EventArgs e)
        {

            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    if(((CheckBox)sender).Checked)
                    {
                        list.BaseTouchInfo.Pic_Next |= 0xFF00;
                        BaseTouch_ButtonChangePageNum.Value = -1;
                        BaseTouch_ButtonChangePagePic.Image = null;
                        list.BaseTouchInfo.PageChange_Image = null;
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 点击是否有按键效果的checkBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseTouch_ButtonEffectCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    if (((CheckBox)sender).Checked)
                    {
                        list.BaseTouchInfo.Pic_On |= 0xFF00;
                        BaseTouch_ButtonEffectnum.Value = -1;
                        BaseTouch_ButtonEffectPicture.Image = null;
                        list.BaseTouchInfo.ButtonEffect_Image = null;
                    }
                    break;
                }
            }
        }
        /// <summary>
        /// 是否自定义键值
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseTouch_KeyValueCheck_CheckedChanged(object sender, EventArgs e)
        {
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    list.BaseTouchInfo.IsKey_Value = BaseTouch_KeyValueCheck.Checked;
                    BaseTouch_KeyValueSetButton.Visible = BaseTouch_KeyValueCheck.Checked;
                    break;
                }
            }
        }
        /// <summary>
        /// 按键效果选择按键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void BaseTouch_ButtonEffectSelect_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form,this,1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                fTouch = Touch.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        BaseTouch_ButtonEffectCheckBox.Checked = false;
                        list.BaseTouchInfo.ButtonEffect_Image = box_Image;
                        list.BaseTouchInfo.Pic_On = (int)fTouch.BaseTouch_ButtonEffectnum.Value;
                        break;
                    }
                }
            }
        }
       
        void BaseTouch_KeyValueSetButton_Click(object sender, EventArgs e)
        {
            KeyCode keycode = new KeyCode(BaseTouch_KeyValueSet);
            keycode.Show();
        }

        #endregion
        #region Data_Var
        /// <summary>
        /// 名称定义
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataVar_NameDefine_TextChanged(object sender, EventArgs e)
        {
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    list.Name_define = DataVar_NameDefine.Text;
                    break;
                }
            }

        }
       
      
        /// <summary>
        /// 颜色文本框改变颜色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataVar_DisplayColor_TextChanged(object sender,EventArgs e)
        {
            UInt16 color_R;
            UInt16 color_G;
            UInt16 color_B;
            UInt16 color_Num; 
            if(DataVar_DisplayColor.Text == "")
            {
                return;
            }
            color_Num = UInt16.Parse(UInt16.Parse(DataVar_DisplayColor.Text, System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
           
            color_R = (UInt16)((((color_Num & 0XF800) >> 11) << 3) | ((color_Num & 0X1800) >> 11));
            color_G = (UInt16)((((color_Num & 0x07E0) >> 5) << 2) | (color_Num & 0x0060)>>5);
            color_B = (UInt16)(((color_Num & 0x001F) << 3) | (color_Num & 0x0007));
            DataVar_DispalyColorPic.BackColor = Color.FromArgb(color_R, color_G, color_B);
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    list.DataVarInfo.Display_Color = DataVar_DispalyColorPic.BackColor;
                    list.DataVarInfo.COLOR = color_Num;
                    break;
                }
            }
        }
      
        /// <summary>
        /// 颜色选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void DataVar_DispalyColorPic_Click(object sender,EventArgs e)
        {
            ColorDialog col = new ColorDialog();
            if(col.ShowDialog() == DialogResult.OK)
            {
                DataVar_DispalyColorPic.BackColor = col.Color;
                //Console.WriteLine(col.Color);
                //Console.WriteLine("R = {0}, G = {1},B = {2}",col.Color.R,col.Color.G,col.Color.B);

                DataVar_DisplayColor.Text = (((col.Color.R >> 3) << 11) | ((col.Color.G >> 2) << 5) | ((col.Color.B >> 3))).ToString("X4");
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        list.DataVarInfo.Display_Color = col.Color;
                        list.DataVarInfo.COLOR = UInt16.Parse(UInt16.Parse(DataVar_DisplayColor.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                        break;
                    }
                }
            }
        }
        #endregion
        #region Icon_display 
        void Iconvar_VarMaxPicSelect_Click(object sender,EventArgs e)
        {
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                   if(list.IconVarInformation.Icon_FileName != "")
                   {
                       DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\I");
                       ChoicePicture_Form choicepicture = new ChoicePicture_Form(dir, fmain_form,this,1);
                       choicepicture.iconshow(list.IconVarInformation.Icon_FileName, 1);
                       if(choicepicture.ShowDialog() == DialogResult.OK)
                       {
                           list.IconVarInformation.Icon_IsMaxTransparent = ChoicePicture_Form.IsImageTrasparent;
                           if(Iconvar_VarMaxPic.Image != null)
                           {
                               list.IconVarInformation.Icon_MaxPic = Iconvar_VarMaxPic.Image;
                           }
                       }
                   }
                   else
                   {
                       if(Main_Form.LanguageType == "English")
                       {
                           MessageBox.Show("Please select the icon file set file stored in the TGUS / I / folder");
                       }
                       else
                       {
                           MessageBox.Show("请选择图标文件集，文件存放在 TGUS / I / 文件夹中");
                       }
                   }
                }
            }
        }
        void Iconvar_VarMinPicSelect_Click(object sender, EventArgs e)
        {
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    if (list.IconVarInformation.Icon_FileName != "")
                    {
                        DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\I");
                        ChoicePicture_Form choicepicture = new ChoicePicture_Form(dir, fmain_form, this, 2);
                        choicepicture.iconshow(list.IconVarInformation.Icon_FileName, 2);
                        if (choicepicture.ShowDialog() == DialogResult.OK)
                        {
                            list.IconVarInformation.Icon_IsMinTransparent = ChoicePicture_Form.IsImageTrasparent;
                            if(Iconvar_VarMinPic.Image != null)
                            {
                                list.IconVarInformation.Icon_MinPic = Iconvar_VarMinPic.Image;
                            } 
                        }
                    }
                    else
                    {
                        if (Main_Form.LanguageType == "English")
                        {
                            MessageBox.Show("Please select the icon file set file stored in the TGUS / I / folder");
                        }
                        else
                        {
                            MessageBox.Show("请选择图标文件集，文件存放在 TGUS / I / 文件夹中");
                        }
                    }
                }
            }
        }
        #endregion
        #region Text_Display
        void TxtDisplay_ShowColorPic_Click(object sender, EventArgs e)
        {
            ColorDialog col = new ColorDialog();
            if (col.ShowDialog() == DialogResult.OK)
            {
                TxtDisplay_ShowColorPic.BackColor = col.Color;
                TxtDisplay_ShowColor.Text = (((col.Color.R >> 3) << 11) | ((col.Color.G >> 2) << 5) | ((col.Color.B >> 3))).ToString("X4");
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        list.TextDisplayInformation.Display_Color = col.Color;
                        list.TextDisplayInformation.COLOR = UInt16.Parse(UInt16.Parse(TxtDisplay_ShowColor.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                        break;
                    }
                }
            }
        }
        #endregion
        #region RTC_Display
        void RTC_DisplayColorPic_Click(object sender, EventArgs e)
        {
            ColorDialog col = new ColorDialog();
            if (col.ShowDialog() == DialogResult.OK)
            {
                RTC_DisplayColorPic.BackColor = col.Color;
                RTC_DisplayColor.Text = (((col.Color.R >> 3) << 11) | ((col.Color.G >> 2) << 5) | ((col.Color.B >> 3))).ToString("X4");
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        list.RTCDisplayInformatin.Display_Color = col.Color;
                        list.RTCDisplayInformatin.COLOR = UInt16.Parse(UInt16.Parse(RTC_DisplayColor.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                        break;
                    }
                }
            }
        }
        #endregion
        #region DataInput
        void DataInput_ButtonEffectSelect_Click (object sender,EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        DataInput_ButtonEffectCheck.Checked = false;
                        list.DataInputInformation.ButtonEffectPic = DataInput_ButtonEffectPic.Image;
                        list.DataInputInformation.Pic_On = (int)DataInput_ButtonEffectNum.Value;
                        break;
                    }
                }
            }
        }
        void DataInput_ButtonEffectPic_Click(object sender,EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        DataInput_ButtonEffectCheck.Checked = false;
                        list.DataInputInformation.ButtonEffectPic = DataInput_ButtonEffectPic.Image;
                        list.DataInputInformation.Pic_On = (int)DataInput_ButtonEffectNum.Value;
                        break;
                    }
                }
            }
        }
        void  DataInput_PageChangeSelect_Click(object sender,EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 2);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.DataInputInformation.ButtonChangePagePic != null)
                        {
                            list.DataInputInformation.ButtonChangePagePic = null;
                        }
                        DataInput_PageChangeCheck.Checked = false;
                        list.DataInputInformation.ButtonChangePagePic = DataInput_PageChangePic.Image;
                        list.DataInputInformation.Pic_Next = (int)DataInput_PageChangeNum.Value;
                        break;
                    }
                }
            }
        }
        void DataInput_PageChangePic_Click(object sender,EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 2);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();              
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.DataInputInformation.ButtonChangePagePic != null)
                        {
                            list.DataInputInformation.ButtonChangePagePic = null;
                        }
                        DataInput_PageChangeCheck.Checked = false;
                        list.DataInputInformation.ButtonChangePagePic = DataInput_PageChangePic.Image;
                        list.DataInputInformation.Pic_Next = (int)DataInput_PageChangeNum.Value;
                        break;
                    }
                }
            }
        }
        void DataInput_DisplayLocationSelect_Click(object sender,EventArgs e)
        {
            Display dis = Display.GetSingle();
            if (dis.designer1.BackgroundImage == null)
            {
                MessageBox.Show("Please Choice e picture as backgroundpicture", "promp", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            Image_Area_Setting ImageAreaForm = Image_Area_Setting.GetSingle();
            Image bakimage = dis.designer1.BackgroundImage;
            ImageAreaForm.ShowPic(dis.designer1.Width,dis.designer1.Height,dis.designer1.BackgroundImage);
            Button but = sender as Button;
            Image_Area_Setting.ButName = but.Name;
            if(ImageAreaForm.ShowDialog() == DialogResult.OK)
            {
                dis.designer1.BackgroundImage = bakimage;
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        string[] str = DataInput_KeyBoardLocation.Text.Split(',');
                        list.DataInputInformation.KeyShowPosition_X = Convert.ToUInt16(str[0]);
                        list.DataInputInformation.KeyShowPosition_Y = Convert.ToUInt16(str[1]);
                        break;
                    }
                }
            }
        }
        void DataInput_KeyBoardSet_Click(object sender,EventArgs e)
        {  
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 3);
            choicepicture_form.touchpicshow();
            Button but = sender as Button;  
            Image_Area_Setting.ButName = but.Name;
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.DataInputInformation.KeyBoardPic != null)
                        {
                            list.DataInputInformation.KeyBoardPic = null;
                        }
                        list.DataInputInformation.KeyBoardPic = DataInput_KeyBoardPic.Image;
                        list.DataInputInformation.PIC_KB = (int)DataInput_KeyBoardAtPage.Value;
                        string[] str = DataInput_KeyboardLeftup.Text.Split(',');
                        list.DataInputInformation.AREA_KB_Xs = Convert.ToUInt16(str[0]);
                        list.DataInputInformation.AREA_KB_Ys = Convert.ToUInt16(str[1]);
                        str = DataInput_KeyboardRightDown.Text.Split(',');
                        list.DataInputInformation.AREA_KB_Xe = Convert.ToUInt16(str[0]);
                        list.DataInputInformation.AREA_KB_Ye = Convert.ToUInt16(str[1]);
                        break;
                    }
                }
            }
        }
            
        void DataInput_KeyBoardPic_Click(object sender,EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 3);
            choicepicture_form.touchpicshow();
            Button but = sender as Button;
            Image_Area_Setting.ButName = "DataInput_KeyBoardSet";
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.DataInputInformation.KeyBoardPic != null)
                        {
                            list.DataInputInformation.KeyBoardPic = null;
                        }
                        list.DataInputInformation.KeyBoardPic = DataInput_KeyBoardPic.Image;
                        list.DataInputInformation.PIC_KB = (int)DataInput_KeyBoardAtPage.Value;
                        string[] str = DataInput_KeyboardLeftup.Text.Split(',');
                        list.DataInputInformation.AREA_KB_Xs = Convert.ToUInt16(str[0]);
                        list.DataInputInformation.AREA_KB_Ys = Convert.ToUInt16(str[1]);
                        str = DataInput_KeyboardRightDown.Text.Split(',');
                        list.DataInputInformation.AREA_KB_Xe = Convert.ToUInt16(str[0]);
                        list.DataInputInformation.AREA_KB_Ye = Convert.ToUInt16(str[1]);
                        break;
                    }
                }
            }
        }
        void DataInput_KeyboardShowLocationSet_Click(object sender,EventArgs e)
        {
            Display dis = Display.GetSingle();
            if (dis.designer1.BackgroundImage == null)
            {
                MessageBox.Show("Please Choice e picture as backgroundpicture", "promp", MessageBoxButtons.OK, MessageBoxIcon.Information, 
                    MessageBoxDefaultButton.Button1);
                return;
            }
            Image_Area_Setting ImageAreaForm = Image_Area_Setting.GetSingle();
            ImageAreaForm.ShowPic(dis.designer1.Width, dis.designer1.Height, dis.designer1.BackgroundImage);
            Button but = sender as Button;
            Image_Area_Setting.ButName = but.Name;
            if (ImageAreaForm.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        string[] str = DataInput_KeyboardShowLocation.Text.Split(',');
                        list.DataInputInformation.AREA_KB_Posation_X = Convert.ToUInt16(str[0]);
                        list.DataInputInformation.AREA_KB_Posation_Y = Convert.ToUInt16(str[1]);
                        break;
                    }
                }
            }
        }
        void DataInput_DisplayColorPic_Click(object sender, EventArgs e)
        {
            ColorDialog col = new ColorDialog();
            if (col.ShowDialog() == DialogResult.OK)
            {
                DataInput_DisplayColorPic.BackColor = col.Color;
                DataInput_DisplayColor.Text = (((col.Color.R >> 3) << 11) | ((col.Color.G >> 2) << 5) | ((col.Color.B >> 3))).ToString("X4");
                
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        list.DataInputInformation.Display_Color = col.Color;
                        list.DataInputInformation.COLOR = UInt16.Parse(UInt16.Parse(DataInput_DisplayColor.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                        break;
                    }
                }
            }
        }
        #endregion
        #region KeyReturn
        void CheckList_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            if (e.CurrentValue == CheckState.Checked) return;//取消选中就不用进行以下操作  
            for (int i = 0; i < ((CheckedListBox)sender).Items.Count; i++)
            {
                ((CheckedListBox)sender).SetItemChecked(i, false);//将所有选项设为不选中  
            }
            e.NewValue = CheckState.Checked;//刷新 
        }
        void KeyReturn_ButtonChangePagePic_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 2);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.KeyReturnInformation.ButtonChangePagePic != null)
                        {
                            list.KeyReturnInformation.ButtonChangePagePic = null;
                        }
                        KeyReturn_ButtonChangePageCheck.Checked = false;
                        list.KeyReturnInformation.ButtonChangePagePic = KeyReturn_ButtonChangePagePic.Image;
                        list.KeyReturnInformation.Pic_Next = (int)KeyReturn_ButtonChangePageNum.Value;
                        break;
                    }
                }
            }
        }

        void KeyReturn_KeyValueSet_Click(object sender, EventArgs e)
        {
            KeyCode keycode = new KeyCode(KeyReturn_KeyValue);
            if(keycode.ShowDialog() == DialogResult.OK)
            {

            }
        }
        void KeyReturn_TouchKeyValueSet_Click(object sender, EventArgs e)
        {
            KeyCode keycode = new KeyCode(KeyReturn_TouchKeyValue);
            if (keycode.ShowDialog() == DialogResult.OK)
            {

            }
        }
        void KeyReturn_Keeppressing_Click(object sender, EventArgs e)
        {
            KeyCode keycode = new KeyCode(KeyReturn_KeeppressingText);
            if (keycode.ShowDialog() == DialogResult.OK)
            {

            }

        }
        void KeyReturn_ButtonChangePageSelect_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 2);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.KeyReturnInformation.ButtonChangePagePic != null)
                        {
                            list.KeyReturnInformation.ButtonChangePagePic = null;
                        }
                        KeyReturn_ButtonChangePageCheck.Checked = false;
                        list.KeyReturnInformation.ButtonChangePagePic = KeyReturn_ButtonChangePagePic.Image;
                        list.KeyReturnInformation.Pic_Next = (int)KeyReturn_ButtonChangePageNum.Value;
                        break;
                    }
                }
            }
        }

        void KeyReturn_ButtonEffrctPic_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.KeyReturnInformation.ButtonEffectPic != null)
                        {
                            list.KeyReturnInformation.ButtonEffectPic = null;
                        }
                        KeyReturn_ButtonEffrctCheck.Checked = false;
                        list.KeyReturnInformation.ButtonEffectPic = KeyReturn_ButtonEffrctPic.Image;
                        list.KeyReturnInformation.Pic_On = (int)KeyReturn_ButtonEffrctNum.Value;
                        break;
                    }
                }
            }
        }

        void KeyReturn_ButtonEffrctSelect_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.KeyReturnInformation.ButtonEffectPic != null)
                        {
                            list.KeyReturnInformation.ButtonEffectPic = null;
                        }
                        KeyReturn_ButtonEffrctCheck.Checked = false;
                        list.KeyReturnInformation.ButtonEffectPic = KeyReturn_ButtonEffrctPic.Image;
                        list.KeyReturnInformation.Pic_On = (int)KeyReturn_ButtonEffrctNum.Value;
                        break;
                    }
                }
            }
        }

        #endregion
        #region PopupMenu
        void PopupMenu_MenuPositionSet_Click(object sender, EventArgs e)
        {
            Display dis = Display.GetSingle();
            if (dis.designer1.BackgroundImage == null)
            {
                MessageBox.Show("Please Choice e picture as backgroundpicture", "promp", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            Image_Area_Setting ImageAreaForm = Image_Area_Setting.GetSingle();
            ImageAreaForm.ShowPic(dis.designer1.Width, dis.designer1.Height, dis.designer1.BackgroundImage);
            Button but = sender as Button;
            Image_Area_Setting.ButName = but.Name;
            if (ImageAreaForm.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        string[] str = PopupMenu_MenuDisPosition.Text.Split(',');
                        list.PopupMenuInformation.Menu_Position_X = Convert.ToUInt16(str[0]);
                        list.PopupMenuInformation.Menu_Position_Y = Convert.ToUInt16(str[1]);
                        break;
                    }
                }
            }
        }
        void PopupMenu_MenuPic_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 3);
            choicepicture_form.touchpicshow();
            Button but = sender as Button;
            Image_Area_Setting.ButName = "PopupMenu_MenuPositionSet";
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.PopupMenuInformation.PopupMenuPic != null)
                        {
                            list.PopupMenuInformation.PopupMenuPic = null;
                        }
                        list.PopupMenuInformation.PopupMenuPic = PopupMenu_MenuPic.Image;
                        list.PopupMenuInformation.Pic_Menu = (int)PopupMenu_MenuAtPage.Value;
                        string[] str = PopupMenu_MenuLeftUp.Text.Split(',');
                        list.PopupMenuInformation.AREA_Menu_Xs = Convert.ToUInt16(str[0]);
                        list.PopupMenuInformation.AREA_Menu_Ys = Convert.ToUInt16(str[1]);
                        str = PopupMenu_MenuRightDown.Text.Split(',');
                        list.PopupMenuInformation.AREA_Menu_Xe = Convert.ToUInt16(str[0]);
                        list.PopupMenuInformation.AREA_Menu_Ye = Convert.ToUInt16(str[1]);
                        break;
                    }
                }
            }
        }

        void PopupMenu_MenuSet_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 3);
            choicepicture_form.touchpicshow();
            Button but = sender as Button;
            Image_Area_Setting.ButName = but.Name;
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.PopupMenuInformation.PopupMenuPic != null)
                        {
                            list.PopupMenuInformation.PopupMenuPic = null;
                        }
                        list.PopupMenuInformation.PopupMenuPic = PopupMenu_MenuPic.Image;
                        list.PopupMenuInformation.Pic_Menu = (int)PopupMenu_MenuAtPage.Value;
                        string[] str = PopupMenu_MenuLeftUp.Text.Split(',');
                        list.PopupMenuInformation.AREA_Menu_Xs = Convert.ToUInt16(str[0]);
                        list.PopupMenuInformation.AREA_Menu_Ys = Convert.ToUInt16(str[1]);
                        str = PopupMenu_MenuRightDown.Text.Split(',');
                        list.PopupMenuInformation.AREA_Menu_Xe = Convert.ToUInt16(str[0]);
                        list.PopupMenuInformation.AREA_Menu_Ye = Convert.ToUInt16(str[1]);
                        break;
                    }
                }
            }
        }

        void PopupMenu_ButtonEffectPic_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.PopupMenuInformation.ButtonEffectPic != null)
                        {
                            list.PopupMenuInformation.ButtonEffectPic = null;
                        }
                        PopupMenu_ButtonEffectCheck.Checked = false;
                        list.PopupMenuInformation.ButtonEffectPic = PopupMenu_ButtonEffectPic.Image;
                        list.PopupMenuInformation.Pic_On = (int)PopupMenu_ButtonEffectNum.Value;
                        break;
                    }
                }
            }
        }

        void PopupMenu_ButtonEffectSelect_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.PopupMenuInformation.ButtonEffectPic != null)
                        {
                            list.PopupMenuInformation.ButtonEffectPic = null;
                        }
                        PopupMenu_ButtonEffectCheck.Checked = false;
                        list.PopupMenuInformation.ButtonEffectPic = PopupMenu_ButtonEffectPic.Image;
                        list.PopupMenuInformation.Pic_On = (int)PopupMenu_ButtonEffectNum.Value;
                        break;
                    }
                }
            }
        }

        #endregion
        #region ActionIcon
        void ActionIcon_ICONIDSelect_Click(object sender, EventArgs e)
        {
            Button but = sender as Button;
            fDisplay = Display.GetSingle();
            int i = 0;
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    if (list.ActionIconInforamtion.Icon_FileName != "")
                    {
                        switch (but.Name)
                        {
                            case "ActionIcon_StopIDSelect":
                                i = 1;
                                break;
                            case "ActionIcon_StartIDSelect":
                                i = 2;
                                break;
                            case "ActionIcon_EndIDSelect":
                                i = 3;
                                break;
                        }
                        DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\I");
                        ChoicePicture_Form choicepicture = new ChoicePicture_Form(dir, fmain_form, this, i);
                        choicepicture.iconshow(list.ActionIconInforamtion.Icon_FileName, 1);
                        if (choicepicture.ShowDialog() == DialogResult.OK)
                        {
                            switch (but.Name)
                            {
                                case "ActionIcon_StopIDSelect":
                                    list.ActionIconInforamtion.ISIcon_Stop = ChoicePicture_Form.IsImageTrasparent;
                                    list.ActionIconInforamtion.Icon_Stop = (UInt16)ActionIcon_StopID.Value;
                                    break;
                                case "ActionIcon_StartIDSelect":
                                    list.ActionIconInforamtion.ISIcon_Start = ChoicePicture_Form.IsImageTrasparent;
                                    list.ActionIconInforamtion.Icon_Start = (UInt16)ActionIcon_StartID.Value;
                                    if(list.ActionIconInforamtion.Icon_Start >= 1)
                                    {
                                        GetIconFiles.Geticon(list.ActionIconInforamtion.Icon_FileName);
                                        list.ActionIconInforamtion.Icon_StartPic =
                                            GetIconFiles.Icon_List[list.ActionIconInforamtion.Icon_Start - 1].image;
                                    }
                                    break;
                                case "ActionIcon_EndIDSelect":
                                    list.ActionIconInforamtion.ISIcon_End = ChoicePicture_Form.IsImageTrasparent;
                                    list.ActionIconInforamtion.Icon_End = (UInt16)ActionIcon_EndID.Value;
                                    break;
                            }
                        }
                    }
                    else
                    {
                        if (Main_Form.LanguageType == "English")
                        {
                            MessageBox.Show("Please select the icon file set file stored in the TGUS / I / folder");
                        }
                        else
                        {
                            MessageBox.Show("请选择图标文件集，文件存放在 TGUS / I / 文件夹中");
                        }
                    }
                }
            }
        }

        #endregion
        #region IncreaseAdj
        void IncreaseAdj_ButtonEffectPic_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.IncreaseAdjInformation.Pic_OnImage != null)
                        {
                            list.IncreaseAdjInformation.Pic_OnImage = null;
                        }
                        IncreaseAdj_ButtonEffectCheck.Checked = false;
                        list.IncreaseAdjInformation.Pic_OnImage = IncreaseAdj_ButtonEffectPic.Image;
                        list.IncreaseAdjInformation.Pic_On = (UInt16)IncreaseAdj_ButtonEffectNum.Value;
                        break;
                    }
                }
            }
        }

        void IncreaseAdj_ButtonEffectSelect_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        if (list.IncreaseAdjInformation.Pic_OnImage != null)
                        {
                            list.IncreaseAdjInformation.Pic_OnImage = null;
                        }
                        IncreaseAdj_ButtonEffectCheck.Checked = false;
                        list.IncreaseAdjInformation.Pic_OnImage = IncreaseAdj_ButtonEffectPic.Image;
                        list.IncreaseAdjInformation.Pic_On = (UInt16)IncreaseAdj_ButtonEffectNum.Value;
                        break;
                    }
                }
            }
        }
        #endregion
        #region ArtFont
        void ArtFont_BeginIconSelect_Click(object sender, EventArgs e)
        {
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    if (list.ArtFontInformation.Icon_FileName != null)
                    {
                        DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\I");
                        ChoicePicture_Form choicepicture = new ChoicePicture_Form(dir, fmain_form, this, 1);
                        choicepicture.iconshow(list.ArtFontInformation.Icon_FileName, 1);
                        if (choicepicture.ShowDialog() == DialogResult.OK)
                        {
                            list.ArtFontInformation.Icon_IsTransparent = ChoicePicture_Form.IsImageTrasparent;
                            list.ArtFontInformation.Icon_0 = (UInt16)ArtFont_BeginIconNum.Value;
                            if(list.ArtFontInformation.Icon_0 >= 1)
                            {
                                GetIconFiles.Geticon(list.ArtFontInformation.Icon_FileName);
                                list.ArtFontInformation.Icon_Pic =
                                    GetIconFiles.Icon_List[list.ArtFontInformation.Icon_0 - 1].image;
                            }
                        }
                    }
                    else
                    {
                        if (Main_Form.LanguageType == "English")
                        {
                            MessageBox.Show("Please select the icon file set file stored in the TGUS / I / folder");
                        }
                        else
                        {
                            MessageBox.Show("请选择图标文件集，文件存放在 TGUS / I / 文件夹中");
                        }
                    }
                }
            }
        }
        #endregion
        #region SliderDisplay
        void SlideDisplay_IconIDSelect_Click(object sender, EventArgs e)
        {
            fDisplay = Display.GetSingle();
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    if (list.SlideDisplayInformation.Icon_FileName != "")
                    {
                        DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\I");
                        ChoicePicture_Form choicepicture = new ChoicePicture_Form(dir, fmain_form, this, 1);
                        choicepicture.iconshow(list.SlideDisplayInformation.Icon_FileName, 1);
                        if (choicepicture.ShowDialog() == DialogResult.OK)
                        {
                            list.SlideDisplayInformation.Icon_IsTransparent = ChoicePicture_Form.IsImageTrasparent;
                            list.SlideDisplayInformation.Icon_ID = (UInt16)SlideDisplay_IconIDNum.Value;
                            if (list.SlideDisplayInformation.Icon_ID >= 1)
                            {
                                GetIconFiles.Geticon(list.SlideDisplayInformation.Icon_FileName);
                                list.SlideDisplayInformation.Icon_Pic =
                                    GetIconFiles.Icon_List[list.SlideDisplayInformation.Icon_ID - 1].image;
                            }
                        }
                    }
                    else
                    {
                        if (Main_Form.LanguageType == "English")
                        {
                            MessageBox.Show("Please select the icon file set file stored in the TGUS / I / folder");
                        }
                        else
                        {
                            MessageBox.Show("请选择图标文件集，文件存放在 TGUS / I / 文件夹中");
                        }
                    }
                }
            }
        }
        #endregion 
        #region IconRotation
        void IconRotation_IconIDSelect_Click(object sender, EventArgs e)
        {
            fDisplay = Display.GetSingle();
            Button but = sender as Button; 
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    if (list.IconRotationInformation.Icon_FileName != "")
                    {
                        DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\I");
                        Image_Area_Setting.ButName = but.Name;
                        ChoicePicture_Form choicepicture = new ChoicePicture_Form(dir, fmain_form, this, 1);
                        choicepicture.iconshow(list.IconRotationInformation.Icon_FileName, 1);
                        if (choicepicture.ShowDialog() == DialogResult.OK)
                        {
                            list.IconRotationInformation.Icon_IsTransparent = ChoicePicture_Form.IsImageTrasparent;
                            list.IconRotationInformation.Icon_ID = (UInt16)IconRotation_IconIDNum.Value;
                            if (list.IconRotationInformation.Icon_ID >= 1)
                            {
                                GetIconFiles.Geticon(list.IconRotationInformation.Icon_FileName);
                                list.IconRotationInformation.Icon_Pic =
                                    GetIconFiles.Icon_List[list.IconRotationInformation.Icon_ID - 1].image;
                            }
                        }
                    }
                    else
                    {
                        if (Main_Form.LanguageType == "English")
                        {
                            MessageBox.Show("Please select the icon file set file stored in the TGUS / I / folder");
                        }
                        else
                        {
                            MessageBox.Show("请选择图标文件集，文件存放在 TGUS / I / 文件夹中");
                        }
                    }
                }
            }
        }
        #endregion
        #region ClockDisplay
        void ClockDisplay_ClockAddButton_Click(object sender, EventArgs e)
        {
            fDisplay = Display.GetSingle();
            Button but = sender as Button;
            string[] str;
            int i = 0;
            foreach (ItemRectangle list in fDisplay.designer1.Items)
            {
                if (list.visibility == true && list.Selected == true)
                {
                    if (list.ClockDisplayInformation.Icon_FileName != "")
                    {
                        switch(((Button)sender).Name)
                        {
                            case "ClockDisplay_HourAddButton":
                                i = 1;
                                break;
                            case "ClockDisplay_MinuteAddButton":
                                i = 2;
                                break;
                            case "ClockDisplay_SecAddButton":
                                i = 3;
                                break;
                        }
                        DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\I");
                        Image_Area_Setting.ButName = but.Name;
                        ChoicePicture_Form choicepicture = new ChoicePicture_Form(dir, fmain_form, this, i);
                        choicepicture.iconshow(list.ClockDisplayInformation.Icon_FileName, i);
                        
                        if (choicepicture.ShowDialog() == DialogResult.OK)
                        {
                            if(i == 1)
                            {
                                list.ClockDisplayInformation.Icon_Hour = (UInt16)ClockDisplay_HourIconNum.Value;
                                list.ClockDisplayInformation.Icon_IsHourTransparent = ChoicePicture_Form.IsImageTrasparent;
                                str = ClockDisplay_HourPosition.Text.Split(',');
                                list.ClockDisplayInformation.Icon_Hour_Central_X = Convert.ToUInt16(str[0]);
                                list.ClockDisplayInformation.Icon_Hour_Central_Y = Convert.ToUInt16(str[1]);
                                ClockDisplay_HourCheck.Checked = false;
                                if (list.ClockDisplayInformation.Icon_Hour >= 1 && list.ClockDisplayInformation.Icon_Hour != 0xFFFF)
                                {
                                    GetIconFiles.Geticon(list.ClockDisplayInformation.Icon_FileName);
                                    list.ClockDisplayInformation.Icon_HourPic =
                                        GetIconFiles.Icon_List[list.ClockDisplayInformation.Icon_Hour - 1].image;
                                }
                                else if(list.ClockDisplayInformation.Icon_Hour == 0xFFFF)
                                {
                                    list.ClockDisplayInformation.Icon_HourPic = null;
                                }
                            }
                            else if(i == 2)
                            {
                                list.ClockDisplayInformation.Icon_Minute = (UInt16)ClockDisplay_MinuteIconNum.Value;
                                list.ClockDisplayInformation.Icon_IsMinuteTransparent = ChoicePicture_Form.IsImageTrasparent;
                                str = ClockDisplay_MinutePosition.Text.Split(',');
                                list.ClockDisplayInformation.Icon_Minute_Central_X = Convert.ToUInt16(str[0]);
                                list.ClockDisplayInformation.Icon_Minute_Central_Y = Convert.ToUInt16(str[1]);
                                ClockDisplay_MinuteCheck.Checked = false;
                                if (list.ClockDisplayInformation.Icon_Minute >= 1 && list.ClockDisplayInformation.Icon_Minute != 0xFFFF)
                                {
                                    GetIconFiles.Geticon(list.ClockDisplayInformation.Icon_FileName);
                                    list.ClockDisplayInformation.Icon_MinutePic =
                                        GetIconFiles.Icon_List[list.ClockDisplayInformation.Icon_Minute - 1].image;
                                }
                                else if(list.ClockDisplayInformation.Icon_Minute == 0xFFFF)
                                {
                                    list.ClockDisplayInformation.Icon_MinutePic = null;
                                }
                            }
                            else if(i == 3)
                            {
                                
                                list.ClockDisplayInformation.Icon_Second = (UInt16)ClockDisplay_SecondIconNum.Value;
                                list.ClockDisplayInformation.Icon_IsSecondTransparent = ChoicePicture_Form.IsImageTrasparent;
                                str = ClockDisplay_SecondPosition.Text.Split(',');
                                list.ClockDisplayInformation.Icon_Second_Central_X = Convert.ToUInt16(str[0]);
                                list.ClockDisplayInformation.Icon_Second_Central_Y = Convert.ToUInt16(str[1]);
                                ClockDisplay_SecCheck.Checked = false;
                                if (list.ClockDisplayInformation.Icon_Second >= 1 && list.ClockDisplayInformation.Icon_Second != 0xFFFF)
                                {
                                    GetIconFiles.Geticon(list.ClockDisplayInformation.Icon_FileName);
                                    list.ClockDisplayInformation.Icon_SecondPic =
                                        GetIconFiles.Icon_List[list.ClockDisplayInformation.Icon_Second - 1].image;
                                }
                                else if(list.ClockDisplayInformation.Icon_Second == 0xFFFF)
                                {
                                    list.ClockDisplayInformation.Icon_SecondPic = null;
                                }
                            }
                        }
                    }
                    else
                    {
                        if (Main_Form.LanguageType == "English")
                        {
                            MessageBox.Show("Please select the icon file set file stored in the TGUS / I / folder");
                        }
                        else
                        {
                            MessageBox.Show("请选择图标文件集，文件存放在 TGUS / I / 文件夹中");
                        }
                    }
                }
            }
        }
        #endregion
        private void ButtonEffectSelect_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 1);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        switch (((Control)sender).Name)
                        {
                            case "ASCII_ButtonEffectPic":
                            case "ASCII_ButtonEffectSelect":
                                if (list.ASCIIInformation.Pic_OnPic != null)
                                {
                                    list.ASCIIInformation.Pic_OnPic = null;
                                }
                                ASCII_ButtonEffectCheck.Checked = false;
                                list.ASCIIInformation.Pic_OnPic = ASCII_ButtonEffectPic.Image;
                                list.ASCIIInformation.Pic_On = (UInt16)ASCII_ButtonEffectNum.Value;
                                break;
                            case "GBK_ButtonEffectPic":
                            case "GBK_ButtonEffectSelect":
                                if (list.GBKInformation.Pic_OnPic != null)
                                {
                                    list.GBKInformation.Pic_OnPic = null;
                                }
                                GBK_ButtonEffectCheck.Checked = false;
                                list.GBKInformation.Pic_OnPic = GBK_ButtonEffectPic.Image;
                                list.GBKInformation.Pic_On = (UInt16)GBK_ButtonEffectNum.Value;
                                break;
                            case "TouchState_ButtonEffectSet":
                            case "TouchState_ButtonEffectPic":
                                if (list.TouchStateInformation.Pic_OnPic != null)
                                {
                                    list.TouchStateInformation.Pic_OnPic = null;
                                }
                                TouchState_ButtonEffectCheck.Checked = false;
                                list.TouchStateInformation.Pic_OnPic = TouchState_ButtonEffectPic.Image;
                                list.TouchStateInformation.Pic_On = (UInt16)TouchState_ButtonEffectNum.Value;
                                break;
                            case "RTCset_ButtonEffectSelect":
                            case "RTCset_ButtonEffectPic":
                                if (list.RTCsetInformation.Pic_OnPic != null)
                                {
                                    list.RTCsetInformation.Pic_OnPic = null;
                                }
                                list.RTCsetInformation.Pic_OnPic = RTCset_ButtonEffectPic.Image;
                                list.RTCsetInformation.Pic_On = (UInt16)RTCset_ButtonEffectNum.Value;
                                break;
                        }
                        break;
                    }
                }
            }
        }
        private void PageChangeSelect_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 2);
            choicepicture_form.touchpicshow();
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        switch (((Control)sender).Name)
                        {
                            case "GBK_PageChangePic":
                            case "GBK_PageChangeSelect":
                                if (list.GBKInformation.Pic_NextPic != null)
                                {
                                    list.GBKInformation.Pic_NextPic = null;
                                }
                                GBK_PageChangeCheck.Checked = false;
                                list.GBKInformation.Pic_NextPic = GBK_PageChangePic.Image;
                                list.GBKInformation.Pic_Next = (int)GBK_PageChangeNum.Value;
                                break;
                            case "ASCII_PageChangeSelect":
                            case "ASCII_PageChangePic":
                                if (list.ASCIIInformation.Pic_NextPic != null)
                                {
                                    list.ASCIIInformation.Pic_NextPic = null;
                                }
                                ASCII_PageChangeCheck.Checked = false;
                                list.ASCIIInformation.Pic_NextPic = ASCII_PageChangePic.Image;
                                list.ASCIIInformation.Pic_Next = (int)ASCII_PageChangeNum.Value;
                                break;
                            case "TouchState_PageSwitchSet":
                            case "TouchState_PageSwitchPic":
                                if (list.TouchStateInformation.Pic_NextPic != null)
                                {
                                    list.TouchStateInformation.Pic_NextPic = null;
                                }
                                TouchState_PageChangeCheck.Checked = false;
                                list.TouchStateInformation.Pic_NextPic = TouchState_PageSwitchPic.Image;
                                list.TouchStateInformation.Pic_Next = (int)TouchState_PageSwitchNum.Value;
                                break;
                        }
                        break;
                    }
                }
            }
        }
        #region GBK
        private void KeyboardShowLocationSet_Click(object sender, EventArgs e)
        {
            Display dis = Display.GetSingle();
            if (dis.designer1.BackgroundImage == null)
            {
                MessageBox.Show("Please Choice e picture as backgroundpicture", "promp", MessageBoxButtons.OK, MessageBoxIcon.Information,
                    MessageBoxDefaultButton.Button1);
                return;
            }
            Image_Area_Setting ImageAreaForm = Image_Area_Setting.GetSingle();
            ImageAreaForm.ShowPic(dis.designer1.Width, dis.designer1.Height, dis.designer1.BackgroundImage);
            Button but = sender as Button;
            Image_Area_Setting.ButName = but.Name;
            if (ImageAreaForm.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        string[] str = new string[2];
                        switch (but.Name)
                        {
                            case "GBK_KeyboardShowLocationSet":
                                str = GBK_KeyboardShowLocation.Text.Split(',');
                                list.GBKInformation.AREA_KB_Position_Xs = Convert.ToUInt16(str[0]);
                                list.GBKInformation.AREA_KB_Position_Ys = Convert.ToUInt16(str[1]);
                                break;
                            case "ASCII_KeyboardShowLocationSet":
                                str = ASCII_KeyboardShowLocation.Text.Split(',');
                                list.ASCIIInformation.AREA_KB_Position_Xs = Convert.ToUInt16(str[0]);
                                list.ASCIIInformation.AREA_KB_Position_Ys = Convert.ToUInt16(str[1]);
                                break;
                            case "RTCset_KeyboardPointaSet":
                                str = RTCset_DisplayPoint.Text.Split(',');
                                list.RTCsetInformation.AREA_KB_Position_Xs = Convert.ToUInt16(str[0]);
                                list.RTCsetInformation.AREA_KB_Position_Ys = Convert.ToUInt16(str[1]);
                                break;
                        }
                        break;
                    }
                }
            }
        }
        private void GBK_PinyinDisplayPointSet_Click(object sender, EventArgs e)
        {
            Display dis = Display.GetSingle();
            if (dis.designer1.BackgroundImage == null)
            {
                MessageBox.Show("Please Choice e picture as backgroundpicture", "promp", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            Image_Area_Setting ImageAreaForm = Image_Area_Setting.GetSingle();
            Image bakimage = dis.designer1.BackgroundImage;
            ImageAreaForm.ShowPic(dis.designer1.Width, dis.designer1.Height, dis.designer1.BackgroundImage);
            Button but = sender as Button;
            Image_Area_Setting.ButName = but.Name;
            if (ImageAreaForm.ShowDialog() == DialogResult.OK)
            {
                dis.designer1.BackgroundImage = bakimage;
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        string[] str = GBK_PinyinDisplayPoint.Text.Split(',');
                        list.GBKInformation.Scan1_Area_Start_Xs = Convert.ToUInt16(str[0]);
                        list.GBKInformation.Scan1_Area_Start_Ys = Convert.ToUInt16(str[1]);
                        break;
                    }
                }
            }
        }

        private void InputDisplayAreaSet_Click(object sender, EventArgs e)
        {
            Display dis = Display.GetSingle();
            if (dis.designer1.BackgroundImage == null)
            {
                MessageBox.Show("Please Choice e picture as backgroundpicture", "promp", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1);
                return;
            }
            Image_Area_Setting ImageAreaForm = Image_Area_Setting.GetSingle();
            Image bakimage = dis.designer1.BackgroundImage;
            ImageAreaForm.ShowPic(dis.designer1.Width, dis.designer1.Height, dis.designer1.BackgroundImage);
            Control but = sender as Control;
            Image_Area_Setting.ButName = but.Name;
            if (ImageAreaForm.ShowDialog() == DialogResult.OK)
            {
                dis.designer1.BackgroundImage = bakimage;
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        string[] str = new string[2];
                        switch(but.Name)
                        {
                            case "GBK_InputDisplayAreaSet":
                                str = GBK_InputDisplayAreaLeft.Text.Split(',');
                                list.GBKInformation.Scan0_Area_Start_Xs = Convert.ToUInt16(str[0]);
                                list.GBKInformation.Scan0_Area_Start_Ys = Convert.ToUInt16(str[1]);
                                str = GBK_InputDisplayAreaRight.Text.Split(',');
                                list.GBKInformation.Scan0_Area_End_Xe = Convert.ToUInt16(str[0]);
                                list.GBKInformation.Scan0_Area_End_Ye = Convert.ToUInt16(str[1]);
                                break;
                            case "ASCII_InputDisplayAreaSet":
                                str = ASCII_InputDisplayAreaLeft.Text.Split(',');
                                list.ASCIIInformation.Scan_Area_Start_Xs = Convert.ToUInt16(str[0]);
                                list.ASCIIInformation.Scan_Area_Start_Ys = Convert.ToUInt16(str[1]);
                                str = ASCII_InputDisplayAreaRight.Text.Split(',');
                                list.ASCIIInformation.Scan_Area_End_Xe = Convert.ToUInt16(str[0]);
                                list.ASCIIInformation.Scan_Area_End_Ye = Convert.ToUInt16(str[1]);
                                break;
                            case "RTCset_Locationset":
                                str = RTCset_Location.Text.Split(',');
                                list.RTCsetInformation.DisplayPoint_X = Convert.ToUInt16(str[0]);
                                list.RTCsetInformation.DisplayPoint_Y = Convert.ToUInt16(str[1]);
                                break;
                        }
                        break;
                    }
                }
            }
        }
        private void KeyBoardSet_Click(object sender, EventArgs e)
        {
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\image");
            choicepicture_form = new ChoicePicture_Form(dir, fmain_form, this, 3);
            choicepicture_form.touchpicshow();
            Control but = sender as Control;
            Image_Area_Setting.ButName = but.Name;
            if (choicepicture_form.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        string[] str = new string[2];
                        switch (but.Name)
                        {
                            case "ASCII_KeyBoardPic":
                            case "ASCII_KeyBoardSet":
                                if (list.ASCIIInformation.PIC_KBPic != null)
                                {
                                    list.ASCIIInformation.PIC_KBPic = null;
                                }
                                list.ASCIIInformation.PIC_KBPic = ASCII_KeyBoardPic.Image;
                                list.ASCIIInformation.PIC_KB = (int)ASCII_KeyBoardAtPage.Value;
                                str = ASCII_KeyboardLeftup.Text.Split(',');
                                list.ASCIIInformation.AREA_KB_Xs = Convert.ToUInt16(str[0]);
                                list.ASCIIInformation.AREA_KB_Ys = Convert.ToUInt16(str[1]);
                                str = ASCII_KeyboardRightDown.Text.Split(',');
                                list.ASCIIInformation.AREA_KB_Xe = Convert.ToUInt16(str[0]);
                                list.ASCIIInformation.AREA_KB_Ye = Convert.ToUInt16(str[1]);
                                break;
                            case "GBK_KeyBoardPic":
                            case "GBK_KeyBoardSet":
                                if (list.GBKInformation.PIC_KBPic != null)
                                {
                                    list.GBKInformation.PIC_KBPic = null;
                                }
                                list.GBKInformation.PIC_KBPic = GBK_KeyBoardPic.Image;
                                list.GBKInformation.PIC_KB = (int)GBK_KeyBoardAtPage.Value;
                                str = GBK_KeyboardLeftup.Text.Split(',');
                                list.GBKInformation.AREA_KB_Xs = Convert.ToUInt16(str[0]);
                                list.GBKInformation.AREA_KB_Ys = Convert.ToUInt16(str[1]);
                                str = GBK_KeyboardRightDown.Text.Split(',');
                                list.GBKInformation.AREA_KB_Xe = Convert.ToUInt16(str[0]);
                                list.GBKInformation.AREA_KB_Ye = Convert.ToUInt16(str[1]);
                                break;
                            case "RTCset_KeyBoardSet":
                            case "RTCset_KeyBoardPic":
                                if (list.RTCsetInformation.PIC_KBPic != null)
                                {
                                    list.RTCsetInformation.PIC_KBPic = null;
                                }
                                list.RTCsetInformation.PIC_KBPic = RTCset_KeyBoardPic.Image;
                                list.RTCsetInformation.PIC_KB = (int)RTCset_KeyBoardAtPage.Value;
                                list.RTCsetInformation.KB_Source = 
                                    (byte)((list.RTCsetInformation.PIC_KB == Main_Form.presentpage_num) ?(0) : (list.RTCsetInformation.PIC_KB));
                                str = RTCset_KeyArea_Left.Text.Split(',');
                                list.RTCsetInformation.AREA_KB_Xs = Convert.ToUInt16(str[0]);
                                list.RTCsetInformation.AREA_KB_Ys = Convert.ToUInt16(str[1]);
                                str = RTCset_KeyBoardRight.Text.Split(',');
                                list.RTCsetInformation.AREA_KB_Xe = Convert.ToUInt16(str[0]);
                                list.RTCsetInformation.AREA_KB_Ye = Convert.ToUInt16(str[1]);
                                break;
                        }
                        
                        break;
                    }
                }
            }
        }
        private void GBK_TextProcessColorPic_Click(object sender, EventArgs e)
        {
            ColorDialog col = new ColorDialog();
            if (col.ShowDialog() == DialogResult.OK)
            {
                GBK_TextProcessColorPic.BackColor = col.Color;
                GBK_TextProcessColor.Text = (((col.Color.R >> 3) << 11) | ((col.Color.G >> 2) << 5) | ((col.Color.B >> 3))).ToString("X4");

                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        list.GBKInformation.Color2 = col.Color;
                        list.GBKInformation.ColorNum2 = UInt16.Parse(UInt16.Parse(GBK_TextProcessColor.Text,
                                System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                        break;
                    }
                }
            }
        }
        private void TextColorPic_Click(object sender, EventArgs e)
        {
            ColorDialog col = new ColorDialog();
            if (col.ShowDialog() == DialogResult.OK)
            {
                fDisplay = Display.GetSingle();
                foreach (ItemRectangle list in fDisplay.designer1.Items)
                {
                    if (list.visibility == true && list.Selected == true)
                    {
                        switch(((Control)sender).Name)
                        {
                            case "ASCII_TextColorPic":
                                ASCII_TextColorPic.BackColor = col.Color;
                                ASCII_TextColor.Text = (((col.Color.R >> 3) << 11) | ((col.Color.G >> 2) << 5) | ((col.Color.B >> 3))).ToString("X4");
                                list.ASCIIInformation.Color = col.Color;
                                list.ASCIIInformation.ColorNum = UInt16.Parse(UInt16.Parse(ASCII_TextColor.Text,
                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                break;
                            case "GBK_TextColorPic":
                                GBK_TextColorPic.BackColor = col.Color;
                                GBK_TextColor.Text = (((col.Color.R >> 3) << 11) | ((col.Color.G >> 2) << 5) | ((col.Color.B >> 3))).ToString("X4");
                                list.GBKInformation.Color1 = col.Color;
                                list.GBKInformation.ColorNum1 = UInt16.Parse(UInt16.Parse(GBK_TextColor.Text,
                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                break;
                            case "RTCset_DisColorpic":
                                RTCset_DisColorpic.BackColor = col.Color;
                                RTCset_DisColor.Text = (((col.Color.R >> 3) << 11) | ((col.Color.G >> 2) << 5) | ((col.Color.B >> 3))).ToString("X4");
                                list.RTCsetInformation.Color = col.Color;
                                list.RTCsetInformation.ColorNum = UInt16.Parse(UInt16.Parse(RTCset_DisColor.Text,
                                        System.Globalization.NumberStyles.HexNumber).ToString("X4"), System.Globalization.NumberStyles.HexNumber);
                                break;
                        }
                        break;
                    }
                }
            }
        }

        #endregion
     
        public void GetIconFile()
        {
            int iconfilenum = 0;
            long iconfileaddr = 0;
            Array.Clear(iconfileinfos, 0, 10);
            
            DirectoryInfo dir = new DirectoryInfo(System.Environment.CurrentDirectory + "\\TGUS" + "\\I");
            FileInfo[] iconfiles = dir.GetFiles();
            foreach (FileInfo iconfile in iconfiles)
            {
                string str = iconfile.Name;
                if(str.StartsWith("0")||char.IsLetter(str[0]))
                {
                    continue;
                }
                
                if (iconfile.Name.Contains(".ICO"))
                {
                    ActionIcon_IconFile.Items.Add(iconfile.Name);
                    iconfileinfos[iconfilenum].iconfile_name = iconfile.Name;
                    iconfileinfos[iconfilenum].iconfile_num = iconfilenum;
                    iconfileinfos[iconfilenum].iconfile_lenth = (int)iconfile.Length;
                    iconfileinfos[iconfilenum].iconfile_addr = (int)iconfileaddr;
                    iconfilenum++;
                    iconfileaddr = ((iconfileaddr + iconfile.Length) / 512);
                    iconfileaddr = iconfileaddr * 512 + 512;
                }
            }
        }
    }
}
