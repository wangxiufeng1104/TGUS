using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using CCWin;
namespace TGUS
{
    public partial class KeyCode : Skin_VS
    {
        public static UInt16 keyValue;
        public static KeyCode keyCodeSingle;
        private static TextBox myTextBox;
        //public static KeyCode GetSingle(HexNumberTextBox textbox)
        //{
        //    myTextBox = textbox;
        //    if(keyCodeSingle == null)
        //    {
        //        keyCodeSingle = new KeyCode();
        //    }
        //    return keyCodeSingle;
        //}
        public KeyCode(HexNumberTextBox textbox)
        {
            InitializeComponent();
            myTextBox = textbox;
        }

        private void Buttton_Click(object sender, EventArgs e)
        {
            ReturnKeycode(sender,ref keyValue);
            myTextBox.Text = keyValue.ToString("X4");
        }

        private static void ReturnKeycode(object sender, ref UInt16 KeyValue)
        {
            Button but = sender as Button;
            switch (but.Text)
            {
                case "`":
                    KeyValue = 0x7E60;
                    break;
                case "1":
                    KeyValue = 0x2131;
                    break;
                case "2":
                    KeyValue = 0x4032;
                    break;
                case "3":
                    KeyValue = 0x2333;
                    break;
                case "4":
                    KeyValue = 0x2434;
                    break;
                case "5":
                    KeyValue = 0x2535;
                    break;
                case "6":
                    KeyValue = 0x5E36;
                    break;
                case "7":
                    KeyValue = 0x2637;
                    break;
                case "8":
                    KeyValue = 0x2A38;
                    break;
                case "9":
                    KeyValue = 0x2839;
                    break;
                case "0":
                    KeyValue = 0x2930;
                    break;
                case "a":
                case "b":
                case "c":
                case "d":
                case "e":
                case "f":
                case "g":
                case "h":
                case "i":
                case "j":
                case "k":
                case "l":
                case "m":
                case "n":
                case "o":
                case "p":
                case "q":
                case "r":
                case "s":
                case "t":
                case "u":
                case "v":
                case "w":
                case "x":
                case "y":
                case "z":
                    byte[] array = new byte[1];   //定义一组数组array
                    array = System.Text.Encoding.ASCII.GetBytes(but.Text); //string转换的字母
                    KeyValue = (UInt16)(array[0] & 0x00FF);
                    array = System.Text.Encoding.ASCII.GetBytes(but.Text.ToUpper()); //string转换的字母
                    KeyValue = (UInt16)(array[0] << 8 | KeyValue);
                    break;
                case "[":
                    KeyValue = 0x7B5B;
                    break;
                case "]":
                    KeyValue = 0x7D5D;
                    break;
                case "\\":
                    KeyValue = 0x7C5C;
                    break;
                case ";":
                    KeyValue = 0x3A3B;
                    break;
                case "'":
                    KeyValue = 0x2227;
                    break;
                case ",":
                    KeyValue = 0x3C2C;
                    break;
                case ".":
                    KeyValue = 0x3E2E;
                    break;
                case "/":
                    KeyValue = 0x3F2F;
                    break;
                case "-":
                    KeyValue = 0x5F2D;
                    break;
                case "=":
                    KeyValue = 0x2B3D;
                    break;
                case "Space":
                    KeyValue = 0x2020;
                    break;
                case "Enter":
                    KeyValue = 0x0D0D;
                    break;
                case "Cancel":
                    KeyValue = 0x00F0;
                    break;
                case "Return":
                    KeyValue = 0x00F1;
                    break;
                case "CapsLock":
                    KeyValue = 0x00F4;
                    break;
                case "Delete":
                    KeyValue = 0x00F3;
                    break;
                case "Left":
                    KeyValue = 0x00F7;
                    break;
                case "Right":
                    KeyValue = 0x00F8;
                    break;
                case "Backspace":
                    KeyValue = 0x00F2;
                    break;
            }
        }
    }
}
