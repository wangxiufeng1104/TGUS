using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;
namespace TGUS
{
    [Serializable()]
    public abstract class ItemBase : ICloneable
    {
        public enum TouchOrVar
        {
            touch,
            varable
        }
        #region Base_Touch_struct

        public struct Base_Touch_struct
        {
            public int Pic_On;// 按键效果图序
            public Image ButtonEffect_Image;/// 按键效果图片
            public int Pic_Next;/// 页面切换图序
            public Image PageChange_Image;/// 页面切换图片
            public UInt16 TP_Code;/// 键值
            public bool IsKey_Value;
        }
        #endregion
        #region DataVar_struct
        
        public struct DataVar_struct
        {
            /// <summary>
            /// 显示颜色
            /// </summary>
            public Color Display_Color;
            public UInt16 COLOR;
            /// <summary>
            /// 字库位置
            /// </summary>
            public byte Lib_ID;
            /// <summary>
            /// 字体大小
            /// </summary>
            public byte Font_Size;
            /// <summary>
            /// 字体对齐方式
            /// </summary>
            public byte Font_Align;
            /// <summary>
            /// 变量类型
            /// </summary>
            public byte Var_Type;
            /// <summary>
            /// 整数位数
            /// </summary>
            public byte Integer_Length;
            /// <summary>
            /// 小数位数
            /// </summary>
            public byte Decimal_Length;
            /// <summary>
            /// 变量单位长度
            /// </summary>
            public byte Len_unit;
            /// <summary>
            /// 显示单位
            /// </summary>
            public string String_Uint;
            /// <summary>
            /// 变量初始值
            /// </summary>
            public long Initial_Value;
        }
        #endregion
        #region IconVar_Struct
        
        public struct IconVar_Struct
        {
            /// <summary>
            /// 图标变量
            /// </summary>
            public int Iconfileselect;
            /// <summary>
            /// 选中的图标名称
            /// </summary>
            public string Icon_FileName;
            /// <summary>
            /// 图库的位置
            /// </summary>
            public byte Icon_Lib;
            /// <summary>
            /// 变量上限
            /// </summary>
            public UInt16 V_Max;
            /// <summary>
            /// 变量上限对应图标
            /// </summary>
            public Image Icon_MaxPic;
            /// <summary>
            /// 变量上限图标是否透明
            /// </summary>
            public bool Icon_IsMaxTransparent;
            /// <summary>
            /// 对应图标上限
            /// </summary>
            public int Icon_Max;
            /// <summary>
            /// 变量下限
            /// </summary>
            public UInt16 V_Min;
            /// <summary>
            /// 变量下限对应图标
            /// </summary>
            public Image Icon_MinPic;
            /// <summary>
            /// 变量下限图标是否透明
            /// </summary>
            public bool Icon_IsMinTransparent;
            /// <summary>
            /// 对应图标下限
            /// </summary>
            public int Icon_Min;
            /// <summary>
            /// 初始值
            /// </summary>
            public UInt16 InitialValue;
            /// <summary>
            /// ICON显示模式
            /// </summary>
            public byte Mode;
        }
        #endregion
        #region TextDisplay_struct
        public struct TextDisplay_struct
        {
            /// <summary>
            /// 显示颜色
            /// </summary>
            public Color Display_Color;
            public UInt16 COLOR;
            /// <summary>
            /// 编码方式
            /// </summary>
            public byte Encode_Mode;
            /// <summary>
            /// 字符间距自动调整
            /// </summary>
            public bool IsCharacternotadj;
            /// <summary>
            /// 文本长度
            /// </summary>
            public UInt16 Text_length;
            /// <summary>
            /// Font0_ID
            /// </summary>
            public byte Font0_ID;
            /// <summary>
            /// Font1_ID
            /// </summary>
            public byte Font1_ID;
            /// <summary>
            /// X方向点阵数
            /// </summary>
            public byte Font_X_Dots;
            /// <summary>
            /// Y方向点阵数
            /// </summary>
            public byte Font_Y_Dots;
            /// <summary>
            /// 水平间隔
            /// </summary>
            public byte HOR_Dis;
            /// <summary>
            ///  垂直间隔
            /// </summary>
            public byte VER_Dis;
            /// <summary>
            /// 初始值
            /// </summary>
            public string initial_value;
        }
        #endregion
        #region RTC_struct
        public struct RTC_struct
        {
            /// <summary>
            /// 显示颜色
            /// </summary>
            public Color Display_Color;
            public UInt16 COLOR;
            /// <summary>
            /// 字库位置
            /// </summary>
            public byte Lib_ID;
            /// <summary>
            /// X方向点阵数
            /// </summary>
            public byte Font_X_Dots;
            /// <summary>
            /// 日期格式
            /// </summary>
            public string String_Code;
        }
        #endregion
        #region DataInput_struct
        public struct DataInput_struct
        {
            /// <summary>
            /// 页面ID
            /// </summary>
            public UInt16 Pic_ID;
            /// <summary>
            /// 数据自动上传
            /// </summary>
            public bool IsDataAutoUpLoad;
            /// <summary>
            /// 按键效果
            /// </summary>
            public int Pic_On;
            /// <summary>
            /// 按键效果图
            /// </summary>
            public Image ButtonEffectPic;
            /// <summary>
            /// 页面切换
            /// </summary>
            public int Pic_Next;
            /// <summary>
            /// 页面切换效果图
            /// </summary>
            public Image ButtonChangePagePic;
            /// <summary>
            /// 变量类型
            /// </summary>
            public byte V_Type;
            /// <summary>
            /// 整数位数
            /// </summary>
            public byte N_Int;
            /// <summary>
            /// 小数位数
            /// </summary>
            public byte N_Dot;
            /// <summary>
            /// 显示位置X坐标
            /// </summary>
            public int KeyShowPosition_X;
            /// <summary>
            /// 显示位置Y坐标
            /// </summary>
            public int KeyShowPosition_Y;
            /// <summary>
            /// 显示颜色
            /// </summary>
            public Color Display_Color;
            public UInt16 COLOR;
            /// <summary>
            /// 字库位置
            /// </summary>
            public byte Lib_ID;
            /// <summary>
            /// 字体大小
            /// </summary>
            public byte Font_Hor;
            /// <summary>
            /// 光标颜色
            /// </summary>
            public byte CurousColor;
            /// <summary>
            /// 输入显示方式
            /// </summary>
            public byte Hide_En;
            /// <summary>
            /// 键盘位置
            /// </summary>
            public byte KB_Source;

            /// <summary>
            /// 键盘所在图片
            /// </summary>
            public Image KeyBoardPic;
            /// <summary>
            /// 键盘所在页面
            /// </summary>
            public int PIC_KB;
            /// <summary>
            /// 键盘左上X坐标
            /// </summary>
            public UInt16 AREA_KB_Xs;
            /// <summary>
            /// 键盘左上Y坐标
            /// </summary>
            public UInt16 AREA_KB_Ys;
            /// <summary>
            /// 键盘右下X坐标
            /// </summary>
            public UInt16 AREA_KB_Xe;
            /// <summary>
            /// 键盘右下Y坐标
            /// </summary>
            public UInt16 AREA_KB_Ye;
            /// <summary>
            /// 显示位置X坐标
            /// </summary>
            public UInt16 AREA_KB_Posation_X;
            /// <summary>
            /// 显示位置Y坐标
            /// </summary>
            public UInt16 AREA_KB_Posation_Y;
            /// <summary>
            /// 范围限制
            /// </summary>
            public byte Limits_En;
            /// <summary>
            /// 上限
            /// </summary>
            public long V_min;
            /// <summary>
            /// 下限
            /// </summary>
            public long V_max;
            /// <summary>
            /// 录入过程加载数据
            /// </summary>
            public byte Return_Set;
            /// <summary>
            /// 变量地址
            /// </summary>
            public UInt16 Return_VP;
            /// <summary>
            /// 加载数据
            /// </summary>
            public UInt16 Return_DATA;
        }
        #endregion
        #region KeyReturn_struct
        public struct KeyReturn_struct
        {
            /// <summary>
            /// 页面ID
            /// </summary>
            public UInt16 Pic_ID;
            /// <summary>
            /// 按键效果
            /// </summary>
            public int Pic_On;
            /// <summary>
            /// 按键效果图
            /// </summary>
            public Image ButtonEffectPic;
            /// <summary>
            /// 页面切换
            /// </summary>
            public int Pic_Next;
            /// <summary>
            /// 页面切换效果图
            /// </summary>
            public Image ButtonChangePagePic;
            /// <summary>
            /// 键值（0x）
            /// </summary>
            public UInt16 Key_Code;
            /// <summary>
            /// 按下键值（0x）
            /// </summary>
            public UInt16 Touch_Key_Code;
            /// <summary>
            /// 长按（0x）
            /// </summary>
            public UInt16 Touch_KeyPressing_Code;
            /// <summary>
            /// 返回键值返回模式
            /// </summary>
            public Byte VP_Mode;

        }
        #endregion
        #region QR Code Struct
        public struct QR_Code_Struct
        {
            /// <summary>
            /// 每个二维码单元像素所占用的的物理像素点阵大小
            /// </summary>
            public UInt16 Unit_Pixels;
        }
        #endregion
        #region PopupMenu_struct
        public struct PopupMenu_Struct
        {
            /// <summary>
            /// 页面ID
            /// </summary>
            public UInt16 Pic_ID;
            /// <summary>
            /// 数据自动上传
            /// </summary>
            public bool IsDataAutoUpLoad;
            /// <summary>
            /// 按键效果Num
            /// </summary>
            public int Pic_On;
            /// <summary>
            /// 按键效果图片
            /// </summary>
            public Image ButtonEffectPic;
            /// <summary>
            /// 写入变量方式
            /// </summary>
            public byte VP_Mode;
            /// <summary>
            /// 弹出菜单图片
            /// </summary>
            public Image PopupMenuPic;
            /// <summary>
            /// 弹出菜单所在页面
            /// </summary>
            public int Pic_Menu;
            /// <summary>
            /// 左上角X坐标
            /// </summary>
            public UInt16 AREA_Menu_Xs;
            /// <summary>
            /// 左上角Y坐标
            /// </summary>
            public UInt16 AREA_Menu_Ys;
            /// <summary>
            /// 右下角X坐标
            /// </summary>
            public UInt16 AREA_Menu_Xe;
            /// <summary>
            /// 右下角Y坐标
            /// </summary>
            public UInt16 AREA_Menu_Ye;
            /// <summary>
            /// 菜单显示位置X坐标
            /// </summary>
            public UInt16 Menu_Position_X;
            /// <summary>
            /// 菜单显示位置Y坐标
            /// </summary>
            public UInt16 Menu_Position_Y;
        }
        #endregion
        #region ActionIcon_struct
        public struct ActionIcon_Struct
        {
            /// <summary>
            /// 开始值，变量为该值时自动显示动画图标
            /// </summary>
            public UInt16 V_Start;
            /// <summary>
            /// 停止值，变量为该值时现实固定图标
            /// </summary>
            public UInt16 V_Stop;
            /// <summary>
            /// 选择的图标文件的名称
            /// </summary>
            public string Icon_FileName;
           
            /// <summary>
            /// 动画图标存储位置
            /// </summary>
            public byte Icon_Lib;
            /// <summary>
            /// 停止图标ID
            /// </summary>
            public UInt16 Icon_Stop;
            /// <summary>
            /// Stop是否去掉背景色
            /// </summary>
            public bool ISIcon_Stop;
            /// <summary>
            /// 开始图标ID
            /// </summary>
            public UInt16 Icon_Start;
            /// <summary>
            /// 开始图标图片
            /// </summary>
            public Image Icon_StartPic;
            /// <summary>
            /// Start是否去掉背景色
            /// </summary>
            public bool ISIcon_Start;
            /// <summary>
            /// 选择的图标的序号
            /// </summary>
            public int Iconfileselect;
            /// <summary>
            /// 结束图标ID
            /// </summary>
            public UInt16 Icon_End;
            /// <summary>
            /// End是否去掉背景色
            /// </summary>
            public bool ISIcon_End;
            
            /// <summary>
            /// ICON显示模式
            /// </summary>
            public byte Mode;
            /// <summary>
            /// ICON显示模式显示
            /// </summary>
            public string strMode;
            /// <summary>
            /// 001或者000
            /// </summary>
            public UInt16 Reset_Icon_En;
            /// <summary>
            /// 初始值
            /// </summary>
            public UInt16 InitlizValue;
        }
        #endregion 
        #region IncreaseAdj_struct
        public struct IncreaseAdj_struct
        {
            /// <summary>
            /// 页面ID
            /// </summary>
            public UInt16 Pic_ID;
            /// <summary>
            /// 数据自动上传
            /// </summary>
            public bool IsDataAutoUpLoad;
            /// <summary>
            /// 按键效果
            /// </summary>
            public int Pic_On;
            /// <summary>
            /// 按键效果图片
            /// </summary>
            public Image Pic_OnImage;
            /// <summary>
            /// 变量写入模式
            /// </summary>
            public byte VP_Mode;
            /// <summary>
            /// 调节方式
            /// </summary>
            public byte Adj_Mode;
            /// <summary>
            /// 逾限处理方式
            /// </summary>
            public byte Return_Mode;
            /// <summary>
            /// 调节步长：0-0x7FFF
            /// </summary>
            public UInt16 Adj_Step;
            /// <summary>
            /// 上限
            /// </summary>
            public UInt16 V_Max;
            /// <summary>
            /// 下限
            /// </summary>
            public UInt16 V_Min;
            /// <summary>
            /// 连续调节
            /// </summary>
            public byte Key_Mode;

        }
        #endregion
        #region SlideAdj_struct
        public struct SlideAdj_struct
        {
            /// <summary>
            /// 页面ID
            /// </summary>
            public UInt16 Pic_ID;
            /// <summary>
            /// 数据自动上传
            /// </summary>
            public bool IsDataAutoUpLoad;
            /// <summary>
            /// 调节方式:高四位数据返回格式，低四位：0横向，1纵向
            /// </summary>
            public byte Adj_Mode;
            /// <summary>
            /// 起始位置对应返回值
            /// </summary>
            public UInt16 V_begin;
            /// <summary>
            /// 终止位置对应的返回值
            /// </summary>
            public UInt16 V_end;
        }
        #endregion
        #region ArtFont_struct
        public struct ArtFont_Struct
        {
            /// <summary>
            /// 图标文件名称
            /// </summary>
            public string Icon_FileName;
            /// <summary>
            /// 图标的选择类型
            /// </summary>
            public int Icon_SelectIndex;
            /// <summary>
            /// 起始图标
            /// </summary>
            public UInt16 Icon_0;
            /// <summary>
            /// 起始图标图片
            /// </summary>
            public Image Icon_Pic;
            /// <summary>
            /// 图片是否透明
            /// </summary>
            public bool Icon_IsTransparent;
            /// <summary>
            /// 高四位为Icon库位置
            /// </summary>
            public byte Icon_Lib;
            /// <summary>
            /// ICON显示模式
            /// </summary>
            public byte Icon_Mode;
            /// <summary>
            /// 显示的整数位数
            /// </summary>
            public byte Integer_Length;
            /// <summary>
            /// 显示的小数位数
            /// </summary>
            public byte Decimal_Length;
            /// <summary>
            /// 变量数据类型
            /// </summary>
            public byte Var_Type;
            /// <summary>
            /// 对齐模式
            /// </summary>
            public byte Align_Mode;
            /// <summary>
            /// 初始值
            /// </summary>
            public long Init_Value;
        }
        #endregion
        #region SlideDisplay_struct
        public struct SlideDisplay_struct
        {
            /// <summary>
            /// 对应起止刻度变量值
            /// </summary>
            public UInt16 V_begain;
            /// <summary>
            /// 对应终止刻度变量值
            /// </summary>
            public UInt16 V_end;
            /// <summary>
            /// 起始刻度坐标（纵向为Y坐标）
            /// </summary>
            public UInt16 X_begain;
            /// <summary>
            /// 终止刻度坐标
            /// </summary>
            public UInt16 X_end;
            /// <summary>
            /// 刻度滑动块的图标ID
            /// </summary>
            public UInt16 Icon_ID;
            /// <summary>
            /// 图标文件名称
            /// </summary>
            public string Icon_FileName; 
            /// <summary>
            /// 图标选择序号
            /// </summary>
            public int Icon_SelectIndex;
            /// <summary>
            /// 图标图片
            /// </summary>
            public Image Icon_Pic;
            /// <summary>
            /// 图片是否透明
            /// </summary>
            public bool Icon_IsTransparent;
            /// <summary>
            /// 图标的存储位置
            /// </summary>
            public byte Icon_Lib;
            /// <summary>
            /// ICON显示模式
            /// </summary>
            public byte Icon_Mode;
            /// <summary>
            /// 刻度指示图标显示的X坐标前偏移量0x00-0xff
            /// </summary>
            public byte X_adj;
            /// <summary>
            /// 刻度模式0x00-横向刻度条，0x01-纵向刻度条
            /// </summary>
            public byte Mode;
            /// <summary>
            /// 变量类型
            /// </summary>
            public byte VP_DATA_Mode;
            /// <summary>
            /// 初始值
            /// </summary>
            public short InitVal;
        }
        #endregion
        #region IconRotation_struct
        public struct IconRotation_struct
        {
            /// <summary>
            /// 图标文件的名称
            /// </summary>
            public string Icon_FileName;
            /// <summary>
            /// 图标选择序号
            /// </summary>
            public int Icon_SelectIndex;
            /// <summary>
            /// 指定的图标ID
            /// </summary>
            public UInt16 Icon_ID;
            /// <summary>
            /// 图片透明
            /// </summary>
            public bool Icon_IsTransparent;
            /// <summary>
            /// 图标图片
            /// </summary>
            public Image Icon_Pic;
            /// <summary>
            /// ICON图标上的旋转中心位置：X坐标
            /// </summary>
            public UInt16 Icon_Xc;
            /// <summary>
            /// ICON图标上的旋转中心位置：Y坐标
            /// </summary>
            public UInt16 Icon_Yc;
            /// <summary>
            /// ICON显示到当前屏幕的旋转中心位置：X坐标
            /// </summary>
            public UInt16 Xc;
            /// <summary>
            /// ICON显示到当前屏幕的旋转中心位置：Y坐标
            /// </summary>
            public UInt16 Yc;
            /// <summary>
            /// 对应起始旋转坐标的变量值
            /// </summary>
            public UInt16 V_begain;
            /// <summary>
            /// 对应终止旋转坐标的变量值
            /// </summary>
            public UInt16 V_end;
            /// <summary>
            /// 起始旋转角度
            /// </summary>
            public UInt16 AL_begain;
            /// <summary>
            /// 终止旋转角度
            /// </summary>
            public UInt16 AL_end;
            /// <summary>
            /// VP_Mode
            /// </summary>
            public byte VP_Mode;
            /// <summary>
            /// ICON 图标库ID
            /// </summary>
            public byte Lib_ID;
            /// <summary>
            /// Icon显示模式,0x00 = 透明，其他 = 显示图标背景
            /// </summary>
            public byte Mode;
            public int Init_Value;
        }
        #endregion
        #region ClockDisplay_struct
        public struct ClockDisplay_struct
        {
            /// <summary>
            /// 图标库
            /// </summary>
            public string Icon_FileName;
            /// <summary>
            /// 图标选择序号
            /// </summary>
            public int Icon_SelectIndex;
            /// <summary>
            /// 指针图库所在的图标库的ID
            /// </summary>
            public byte Icon_Lib;
            /// <summary>
            /// 时钟表盘的指针中心X坐标
            /// </summary>
            public UInt16 X;
            /// <summary>
            /// 时钟表盘的指针中心Y坐标
            /// </summary>
            public UInt16 Y;
            /// <summary>
            /// 不显示时钟
            /// </summary>
            public bool IsDiaplayHour;
            /// <summary>
            /// 不显示分钟
            /// </summary>
            public bool IsDiaplayMinute;
            /// <summary>
            /// 不显示秒钟
            /// </summary>
            public bool IsDiaplaySecond;
            /// <summary>
            /// 时钟ICON的ID，0XFFFF表示时钟不显示
            /// </summary>
            public UInt16 Icon_Hour;
            /// <summary>
            /// Hour图标是否透明
            /// </summary>
            public bool Icon_IsHourTransparent;
            /// <summary>
            /// Hour的图标图片
            /// </summary>
            public Image Icon_HourPic;
            /// <summary>
            /// 分钟ICON的ID，0XFFFF表示时分钟不显示
            /// </summary>
            public UInt16 Icon_Minute;
            /// <summary>
            /// Minute图标是否透明
            /// </summary>
            public bool Icon_IsMinuteTransparent;
            /// <summary>
            /// Minute的图标图片
            /// </summary>
            public Image Icon_MinutePic;
            /// <summary>
            /// 秒钟ICON的ID，0XFFFF表示秒钟不显示
            /// </summary>
            public UInt16 Icon_Second;
            /// <summary>
            /// Second图标是否透明
            /// </summary>
            public bool Icon_IsSecondTransparent;
            /// <summary>
            /// Second的图标图片
            /// </summary>
            public Image Icon_SecondPic;
            /// <summary>
            /// 时针ICON的旋转中心X坐标
            /// </summary>
            public UInt16 Icon_Hour_Central_X;
            /// <summary>
            /// 时针ICON的旋转中心Y坐标
            /// </summary>
            public UInt16 Icon_Hour_Central_Y;
            /// <summary>
            /// 分针ICON的旋转中心X坐标
            /// </summary>
            public UInt16 Icon_Minute_Central_X;
            /// <summary>
            /// 分针ICON的旋转中心Y坐标
            /// </summary>
            public UInt16 Icon_Minute_Central_Y;
            /// <summary>
            /// 秒针ICON的旋转中心X坐标
            /// </summary>
            public UInt16 Icon_Second_Central_X;
            /// <summary>
            /// 秒针ICON的旋转中心Y坐标
            /// </summary>
            public UInt16 Icon_Second_Central_Y;
        }
        #endregion

        #region GBK_Struct
        public struct GBK_struct
        {
            /// <summary>
            /// 数据自动上传
            /// </summary>
            public byte IsDataAutoUpLoad;
            /// <summary>
            /// 页面切换图序
            /// </summary>
            public int Pic_Next;
            /// <summary>
            /// 页面切换图
            /// </summary>
            public Image Pic_NextPic;
            /// <summary>
            ///  按键效果图序
            /// </summary>
            public int Pic_On;
            /// <summary>
            /// 按键效果图
            /// </summary>
            public Image Pic_OnPic;
            /// <summary>
            /// 文本变量最大长度
            /// </summary>
            public int VP_Len_Max;
            /// <summary>
            /// 录入模式控制
            /// </summary>
            public byte Scan_Mode;
            /// <summary>
            /// 汉字字符显示字库
            /// </summary>
            public byte Lib_GBK1;
            /// <summary>
            /// 录入过程字符字库
            /// </summary>
            public byte Lib_GBK2;
            /// <summary>
            /// Lib_GBK1字符的大小
            /// </summary>
            public byte Font_Scale1;
            /// <summary>
            /// Lib_GBK2字符的大小
            /// </summary>
            public byte Font_Scale2;
            /// <summary>
            /// 光标颜色
            /// </summary>
            public byte Cusor_Color;
            /// <summary>
            /// 录入文本显示颜色数
            /// </summary>
            public UInt16 ColorNum1;
            /// <summary>
            /// 录入文本显示颜色
            /// </summary>
            public Color Color1;
            /// <summary>
            /// 录入过程中文本显示的颜色数
            /// </summary>
            public UInt16 ColorNum2;
            /// <summary>
            /// 录入过程中文本显示的颜色数
            /// </summary>
            public Color Color2;
            /// <summary>
            /// 录入过程中，拼音提示和对应汉字的显示方式
            /// </summary>
            public byte PY_Disp_Mode;
            /// <summary>
            /// 
            /// </summary>
            public byte Scan_Return_Mode;
            /// <summary>
            /// 录入文本显示区域左上角坐标Xs
            /// </summary>
            public UInt16 Scan0_Area_Start_Xs;
            /// <summary>
            /// 录入文本显示区域左上角坐标Ys
            /// </summary>
            public UInt16 Scan0_Area_Start_Ys;
            /// <summary>
            /// 录入文本显示区域右下角坐标Xe
            /// </summary>
            public UInt16 Scan0_Area_End_Xe;
            /// <summary>
            /// 录入文本显示区域右下角坐标Ye
            /// </summary>
            public UInt16 Scan0_Area_End_Ye;
            /// <summary>
            /// 录入过程中拼音提示文本显示区域左上角坐标Xs
            /// </summary>
            public UInt16 Scan1_Area_Start_Xs;
            /// <summary>
            /// 录入过程中拼音提示文本显示区域左上角坐标Ys
            /// </summary>
            public UInt16 Scan1_Area_Start_Ys;
            /// <summary>
            /// 录入过程中每个汉字显示的间距。每行固定显示最多8个汉字
            /// </summary>
            public byte Scan_Dis;
            /// <summary>
            /// 键盘页面位置选择
            /// </summary>
            public byte KB_Source;
            /// <summary>
            /// 键盘所在页的选择
            /// </summary>
            public int PIC_KB;
            /// <summary>
            /// 键盘页面
            /// </summary>
            public Image PIC_KBPic;
            /// <summary>
            /// 键盘页面上键盘区域坐标Xs
            /// </summary>
            public UInt16 AREA_KB_Xs;
            /// <summary>
            /// 键盘页面上键盘区域坐标Ys
            /// </summary>
            public UInt16 AREA_KB_Ys;
            /// <summary>
            /// 键盘页面上键盘区域坐标Xe
            /// </summary>
            public UInt16 AREA_KB_Xe;
            /// <summary>
            /// 键盘页面上键盘区域坐标XYe
            /// </summary>
            public UInt16 AREA_KB_Ye;
            /// <summary>
            /// 键盘区域粘贴在当前页面的位置Xs
            /// </summary>
            public UInt16 AREA_KB_Position_Xs;
            /// <summary>
            /// 键盘区域粘贴在当前页面的位置Ys
            /// </summary>
            public UInt16 AREA_KB_Position_Ys;
            /// <summary>
            /// 02：拼音输入法 03：注音输入法（台湾地区繁体录入）
            /// </summary>
            public byte SCAN_MODE;
        }
        #endregion
        public TouchOrVar touchvar = TouchOrVar.touch;
        /// <summary>
        /// 叙述指针
        /// </summary>
        public UInt16 SP;/// 
        /// <summary>
        /// 变量地址
        /// </summary>
        public UInt16 VP;
        /// <summary>
        /// 当前图元是否被选中
        /// </summary>
        private bool selected = false;
        /// <summary>
        /// 线宽
        /// </summary>
        private int lineWidth = 1;
        /// <summary>
        /// 线的颜色
        /// </summary>
        private Color color = Color.Black;
        /// <summary>
        /// 图元填充的颜色
        /// </summary>
        private Color fillColor = Color.Transparent;
        /// <summary>
        /// 锯齿模式
        /// </summary>
        //private SmoothingMode smoothingMode = SmoothingMode.HighQuality;
        /// <summary>
        /// 图元类型
        /// </summary>
        //private Main_Form.myGraphicsType gType = Main_Form.myGraphicsType.None;
        /// <summary>
        /// 图元所在的页面
        /// </summary>
        public int presentpage_num = 0;//图元所在页面

        /// <summary>
        /// 图元所代表控件的类型
        /// </summary>
        public PIC_Obj ControlType = PIC_Obj.NONE;
        /// <summary>
        /// 图元的可见性
        /// </summary>
        public bool visibility = false;
        /// <summary>
        /// 名称定义
        /// </summary>
        public string Name_define = string.Empty;
        public int times;
        [NonSerializedAttribute]
        /// <summary>
        /// 基础触控的信息结构体
        /// </summary>
        public Base_Touch_struct BaseTouchInfo;
        [NonSerializedAttribute]
        /// <summary>
        /// 数据变量的信息结构体
        /// </summary>
        public DataVar_struct DataVarInfo;
        [NonSerializedAttribute]
        /// <summary>
        /// 图标变量的信息结构体
        /// </summary>
        public IconVar_Struct IconVarInformation;
        [NonSerializedAttribute]
        /// <summary>
        /// 文本显示的信息结构体
        /// </summary>
        public TextDisplay_struct TextDisplayInformation;
        [NonSerializedAttribute]
        /// <summary>
        /// RTC的信息结构体
        /// </summary>
        public RTC_struct RTCDisplayInformatin;
        [NonSerializedAttribute]
        /// <summary>
        /// DataInput的信息结构体
        /// </summary>
        public DataInput_struct DataInputInformation;
        [NonSerializedAttribute]
        /// <summary>
        /// KeyReturn的信息结构体
        /// </summary>
        public KeyReturn_struct KeyReturnInformation;
        [NonSerializedAttribute]
        /// <summary>
        /// QR_Code的信息结构体
        /// </summary>
        public QR_Code_Struct QRCodeInformation;
        [NonSerializedAttribute]
        /// <summary>
        /// 弹出菜单信息结构体
        /// </summary>
        public PopupMenu_Struct PopupMenuInformation;
        [NonSerializedAttribute]
        /// <summary>
        ///动画图标信息结构图
        /// </summary>
        public ActionIcon_Struct ActionIconInforamtion;
        [NonSerializedAttribute]
        /// <summary>
        /// 增量调节信息结构体
        /// </summary>
        public IncreaseAdj_struct IncreaseAdjInformation;
        [NonSerializedAttribute]
        /// <summary>
        /// 滑动调节信息结构体
        /// </summary>
        public SlideAdj_struct SlideAdjInformation;
        [NonSerializedAttribute]
        /// <summary>
        /// 艺术字的信息结构体
        /// </summary>
        public ArtFont_Struct ArtFontInformation;
        [NonSerializedAttribute]
        /// <summary>
        /// 滑块刻度指示的信息结构体
        /// </summary>
        public SlideDisplay_struct SlideDisplayInformation;
        /// <summary>
        /// 图标旋转指示的信息结构体
        /// </summary>
        public IconRotation_struct IconRotationInformation;
        /// <summary>
        /// 表盘时钟显示
        /// </summary>
        public ClockDisplay_struct ClockDisplayInformation;
        /// <summary>
        /// GBK录入
        /// </summary>
        public GBK_struct GBKInformation;
        /// <summary>
        /// ASCII录入的信息结构体
        /// </summary>
        public ASCII.ASCII_struct ASCIIInformation;
        /// <summary>
        /// 同步按键状态返回的信息结构体
        /// </summary>
        public TouchState.TouchSatte_struct TouchStateInformation;
        /// <summary>
        /// RTC设置的信息结构体
        /// </summary>
        public RTCset.RTCset_struct RTCsetInformation;
        /// <summary>
        /// 基本图形显示的结构体
        /// </summary>
        public BasicGra.BasicGra_struct BasicGraInformation;
        /// <summary>
        /// 对像是否选择
        /// </summary>
        public bool Selected
        {
            get
            {
                return selected;
            }
            set
            {
                //如果选中,则不做操作
                selected = value;
            }
        }

        /// <summary>
        /// 设置对像的外观线的宽度
        /// </summary>
        public virtual int LineWidth
        {
            get
            {
                return lineWidth;
            }
            set
            {
                lineWidth = value;
            }
        }
        /// <summary>
        /// 设置对像颜色
        /// </summary>
        public Color LineColor
        {
            get
            {
                return color;
            }
            set
            {
                color = value;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public Color FillColor
        {
            get
            {
                return fillColor;
            }
            set
            {
                fillColor = value;
            }
        }

        /// <summary>
        /// 鼠标移动到什么地方
        /// </summary>
        /// <param name="point">鼠标移动到的坐标</param>
        /// <param name="handleIndex">鼠标的焦点在哪一个手柄上</param>
        public virtual void MoveHandleTo(Point point, int handleIndex)
        {
        }

        /// <summary>
        /// 获得手柄的坐点
        /// </summary>
        /// <param name="handleNumber">哪一个手柄</param>
        /// <returns></returns>
        public virtual Point GetHandle(int handleNumber)
        {
            return new Point(0, 0);
        }

        /// <summary>
        /// 获得手柄矩形大小
        /// </summary>
        /// <param name="handleNumber">哪一个手柄</param>
        /// <returns></returns>
        public virtual Rectangle GetHandleRectangle(int handleNumber)
        {
            Point point = GetHandle(handleNumber);

            return new Rectangle(point.X - 3, point.Y - 3, 7, 7);
        }

        /// <summary>
        /// 画出手柄
        /// </summary>
        /// <param name="g"></param>
        public virtual void DrawTracker(Graphics g)
        {
            if (!Selected)
                return;

            for (int i = 1; i <= HandleCount; i++)
            {
                g.DrawRectangle(Pens.SkyBlue, GetHandleRectangle(i));
            }
        }

        /// <summary>
        /// 是否被点击
        /// 如返回大于1则是被点击并返回手柄数!
        /// </summary>
        /// <param name="point"></param>
        /// <returns></returns>
        public virtual int HitTest(Point point)
        {
            return -1;
        }

        /// <summary>
        /// 点是否是对像中
        /// </summary>
        /// <param name="point">在返回true</param>
        /// <returns></returns>
        protected virtual bool PointInObject(Point point)
        {
            return false;
        }

        /// <summary>
        /// 是否在传来的矩型中
        /// </summary>
        /// <param name="rectangle"></param>
        /// <returns></returns>
        public virtual bool IntersectsWith(Rectangle rectangle)
        {
            return false;
        }

        /// <summary> 
        /// 将对像移动到哪个点上
        /// </summary>
        /// <param name="deltaX">X坐标点</param>
        /// <param name="deltaY">Y坐标点</param>
        public virtual void Move(int deltaX, int deltaY,Designer designer)
        {
        }
        /// <summary> 
        /// 将对像移动到哪个点上
        /// </summary>
        /// <param name="deltaX">X坐标点</param>
        /// <param name="deltaY">Y坐标点</param>
        public virtual void Move(int deltaX, int deltaY)
        {
        }

        /// <summary>
        /// 调用此功能结束放大或缩小
        /// </summary>
        public virtual void Normalize()
        {
        }

        /// <summary>
        /// 获进手柄光标
        /// </summary>
        /// <param name="handleNumber">哪一个手柄</param>
        /// <returns></returns>
        public virtual Cursor GetHandleCursor(int handleNumber)
        {
            return Cursors.Default;
        }

        /// <summary>
        /// 设置选中后对像有几个调节手柄
        /// 例：线２个，矩型８个等
        /// </summary>
        [Browsable(false)]
        public abstract int HandleCount
        {
            get;
        }

        /// <summary>
        /// 在对像列表中显示的名称
        /// </summary>
        [Browsable(false)]
        public virtual string Name
        {
            get { return "NULL"; }
        }
        public abstract void Draw(Graphics g,string Name_define);

        #region ICloneable Members

        public virtual object Clone()
        {
            return base.MemberwiseClone();
        }
        #endregion
    }
}
