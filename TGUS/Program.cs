using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TGUS
{
    public enum PIC_Obj
    {
        NONE,
        basictouch,
        keyreturn,
        data_display,
        icon_display,
        text_dispaly,
        rtc_display,
        QR_display,
        datainput,
        menu_display,
        aniicon_display,
        increadj,
        sliadj,
        artfont,
        slidis,
        iconrota,
        clockdisplay,
        GBK,
        ASCII,
        TouchState,
        RTC_Set,
        BasicGra,
        areaset
    }

    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
       {
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Main_Form());
        }
    }
}
