using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace TGUS
{
    public class ASCII
    {
        public struct ASCII_struct
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
            /// TP_Code
            /// </summary>
            public UInt16 TP_Code;
            /// <summary>
            /// 文本变量最大长度
            /// </summary>
            public int VP_Len_Max;
            /// <summary>
            /// 录入模式控制
            /// </summary>
            public byte Scan_Mode;
            /// <summary>
            /// 显示使用的是ASCII字符
            /// </summary>
            public byte Lib_ID;
            /// <summary>
            /// 字体大小，X方向点阵数
            /// </summary>
            public byte Font_Hor;
            /// <summary>
            /// 字体大小，Y方向点阵数
            /// </summary>
            public byte Font_Ver;
            /// <summary>
            /// 光标颜色
            /// </summary>
            public byte Cusor_Color;
            /// <summary>
            /// 文本显示颜色数
            /// </summary>
            public UInt16 ColorNum;
            /// <summary>
            /// 文本显示颜色
            /// </summary>
            public Color Color;
            /// <summary>
            /// 录入文本显示区域左上角坐标（Xs）
            /// </summary>
            public UInt16 Scan_Area_Start_Xs;
            /// <summary>
            /// 录入文本显示区域左上角坐标（Ys）
            /// </summary>
            public UInt16 Scan_Area_Start_Ys;
            /// <summary>
            /// 0x55或者0x00
            /// </summary>
            public byte Scan_Return_Mode;
            /// <summary>
            /// 录入文本显示区域右下角坐标（Xe）
            /// </summary>
            public UInt16 Scan_Area_End_Xe;
            /// <summary>
            /// 录入文本显示区域右下角坐标（Ye）
            /// </summary>
            public UInt16 Scan_Area_End_Ye;
            /// <summary>
            /// 键盘页面位置选择：0x00 = 键盘在当前页面，其他 = 键盘不在当前页面
            /// </summary>
            public byte KB_Source;
            /// <summary>
            /// 键盘所在页面ID
            /// </summary>
            public int PIC_KB;
            /// <summary>
            /// 键盘所在页面的图片
            /// </summary>
            public Image PIC_KBPic;
            /// <summary>
            /// 键盘页面上键盘区域坐标
            /// </summary>
            public UInt16 AREA_KB_Xs;
            public UInt16 AREA_KB_Ys;
            public UInt16 AREA_KB_Xe;
            public UInt16 AREA_KB_Ye;
            /// <summary>
            /// 键盘区域粘贴在当前页面显示的位置，左上角坐标
            /// </summary>
            public UInt16 AREA_KB_Position_Xs;
            public UInt16 AREA_KB_Position_Ys;
            /// <summary>
            /// 输入可见使能
            /// </summary>
            public byte DISPLAY_EN;
        }
    }
    public class TouchState
    {
        public struct TouchSatte_struct
        {
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
            public UInt16 TP_code;
            /// <summary>
            /// 触摸屏第一次按压数据返回格式
            /// </summary>
            public byte TP_ON_Mode;
            /// <summary>
            /// 触摸屏第一次按压时，读取数据的地址
            /// </summary>
            public UInt16 VP1S;
            /// <summary>
            /// 触摸屏第一次按压时，写入数据的地址
            /// </summary>
            public UInt16 VP1T;
            /// <summary>
            /// 返回数据长度，字节数。TP_ON_Mode = 0x01时，LEN1必须为偶数
            /// </summary>
            public byte LEN1;
            /// <summary>
            /// 触摸屏第一次按压后，持续按下时，数据返回格式
            /// </summary>
            public byte TP_ON_Continue_Mode;
            /// <summary>
            /// 触摸屏持续按压时，读取数据的地址
            /// </summary>
            public UInt16 VP2S;
            /// <summary>
            /// 触摸屏持续按压时，写入数据的地址
            /// </summary>
            public UInt16 VP2T;
            /// <summary>
            /// 返回数据长度，字节数。TP_Continue_ON_Mode = 0x01时，LEN2必须为偶数
            /// </summary>
            public byte LEN2;
            /// <summary>
            /// 触摸屏松开时，数据的返回格式
            /// </summary>
            public byte TP_OFF_Mode;
            /// <summary>
            /// 触摸屏松开时，读取数据的格式
            /// </summary>
            public UInt16 VP3S;
            /// <summary>
            /// 触摸屏松开时，写入数据的地址
            /// </summary>
            public UInt16 VP3T;
            /// <summary>
            /// 返回数据长度，字节数。TP_OFF_Mode = 0x01时，LEN3必须为偶数
            /// </summary>
            public byte LEN3;
        }
    }
    public class RTCset
    {
        public struct RTCset_struct
        {
            /// <summary>
            ///  按键效果图序
            /// </summary>
            public int Pic_On;
            /// <summary>
            /// 按键效果图
            /// </summary>
            public Image Pic_OnPic;
            public UInt16 TP_Code;
            /// <summary>
            /// 输入过程显示位置，右对齐方式，（x,y）是字符串右上角坐标
            /// </summary>
            public UInt16 DisplayPoint_X;
            public UInt16 DisplayPoint_Y;
            /// <summary>
            /// 输入字体显示颜色
            /// </summary>
            public UInt16 ColorNum;
            public Color Color;
            /// <summary>
            /// 显示使用的ASCII字库的位置
            /// </summary>
            public byte Lib_ID;
            /// <summary>
            /// 字体大小，X方向点阵数目
            /// </summary>
            public byte Font_Hor;
            /// <summary>
            /// 光标颜色
            /// </summary>
            public byte Cusor_Color;
            /// <summary>
            /// 00 = 键盘显示在当前页面，其他 = 键盘不在当前页面
            /// </summary>
            public byte KB_Source;
            /// <summary>
            /// 键盘所在页面ID，仅当KB_Source不等于0x00时有效
            /// </summary>
            public int PIC_KB;
            public Image PIC_KBPic;
            /// <summary>
            /// 键盘页面上键盘区域坐标
            /// </summary>
            public UInt16 AREA_KB_Xs;
            public UInt16 AREA_KB_Ys;
            public UInt16 AREA_KB_Xe;
            public UInt16 AREA_KB_Ye;
            /// <summary>
            /// 键盘区域粘贴在当前页面显示的位置，左上角坐标
            /// </summary>
            public UInt16 AREA_KB_Position_Xs;
            public UInt16 AREA_KB_Position_Ys;
        }
    }
    public class BasicGra
    {
        public struct BasicGra_struct
        {
            /// <summary>
            /// 虚线或者实线开关
            /// </summary>
            public byte Dashed_Line_En;
            /// <summary>
            /// 设置虚线（点划线）格式
            /// </summary>
            public byte Dash_Set_1;
            public byte Dash_Set_2;
            public byte Dash_Set_3;
            public byte Dash_Set_4;
        }
    }
}
