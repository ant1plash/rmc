using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace RMCSrv
{
    
    public class KeyControls
    {
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        static extern void keybd_event(byte bVk, byte bScan, uint dwFlags, int dwExtraInfo);

        [DllImport("user32.dll")]
        static extern int MapVirtualKey(uint uCode, uint uMapType);
 
        [DllImport("user32.dll")]
        private static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
 
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        private static extern void mouse_event(IntPtr dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [StructLayout(LayoutKind.Sequential)]
        public struct MOUSEINPUT
        {
            public int dx;
            public int dy;
            public int mouseData;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct KEYBDINPUT
        {
            public short wVk;
            public short wScan;
            public int dwFlags;
            public int time;
            public IntPtr dwExtraInfo;
        }

        [StructLayout(LayoutKind.Sequential)]
        public struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }

        [Flags]
        public enum InputType
        {
            INPUT_MOUSE = 0,
            INPUT_KEYBOARD = 1,
            INPUT_HARDWARE = 2
        }

        [Flags]
        public enum MOUSEEVENTF
        {
            MOVE = 0x0001, /* mouse move */
            LEFTDOWN = 0x0002, /* left button down */
            LEFTUP = 0x0004, /* left button up */
            RIGHTDOWN = 0x0008, /* right button down */
            RIGHTUP = 0x0010, /* right button up */
            MIDDLEDOWN = 0x0020, /* middle button down */
            MIDDLEUP = 0x0040, /* middle button up */
            XDOWN = 0x0080, /* x button down */
            XUP = 0x0100, /* x button down */
            WHEEL = 0x0800, /* wheel button rolled */
            MOVE_NOCOALESCE = 0x2000, /* do not coalesce mouse moves */
            VIRTUALDESK = 0x4000, /* map to entire virtual desktop */
            ABSOLUTE = 0x8000 /* absolute move */
        }

        [Flags]
        public enum KEYEVENTF
        {
            KEYDOWN = 0,
            EXTENDEDKEY = 0x0001,
            KEYUP = 0x0002,
            UNICODE = 0x0004,
            SCANCODE = 0x0008,
        }

        [StructLayout(LayoutKind.Explicit)]
        public struct INPUT
        {
            [FieldOffset(4)]
            public HARDWAREINPUT hi;
            [FieldOffset(4)]
            public KEYBDINPUT ki;
            [FieldOffset(4)]
            public MOUSEINPUT mi;
            [FieldOffset(0)]
            public int type;
        }

        [DllImport("user32.dll", SetLastError = true)]
        public static extern IntPtr GetMessageExtraInfo();

        [DllImport("user32.dll", SetLastError = true)]
        public static extern uint SendInput(uint nInputs, INPUT[] pInputs, int cbSize);

        [DllImport("user32.dll")]
        public static extern short VkKeyScan(char ch);



            public const byte VK_VOLUME_MUTE = 0xAD;
            public const byte VK_VOLUME_DOWN = 0xAE;
            public const byte VK_VOLUME_UP = 0xAF;
            public const byte VK_MEDIA_NEXT_TRACK = 0xB0;
            public const byte VK_MEDIA_PREV_TRACK = 0xB1;
            public const byte VK_MEDIA_STOP = 0xB2;
            public const byte VK_MEDIA_PLAY_PAUSE = 0xB3;
            public const byte VK_LWIN = 0x5B;
            public const byte VK_RMENU = 0xA5;
            public const byte VK_LBUTTON = 0x01;
            public const byte VK_OEM_PLUS = 0xBB;
			
			
			/* new keys */
			
			public const byte VK_LEFT = 0x25;
			public const byte VK_UP = 0x26;
			public const byte VK_RIGHT = 0x27;
			public const byte VK_DOWN = 0x28;
			public const byte VK_RETURN = 0x0D; //enter
			public const byte VK_ESCAPE = 0x1B;
			public const byte VK_END = 0x23;
			public const byte VK_ESC= 0x1B;
			public const byte VK_HOME = 0x24;

            private const uint KEYEVENTF_KEYUP = 0x0002;

            static public void send_event(byte key)
            {
                keybd_event(key, 0, 0, 0);
            }

            private const int MOVE = 0x0001;
            private const int HWND_BROADCAST = 0xffff;
            private const int WM_SYSCOMMAND = 0x0112;
            private const int SC_MONITORPOWER = 0xF170;


              /*
               
               **********************************************************
               **********************************************************
               
              */

            static public void TurnOffScreen()
            {
                mouse_event((IntPtr)MOVE, 0, 0, 0, UIntPtr.Zero);
                SendMessage((IntPtr)HWND_BROADCAST, WM_SYSCOMMAND, SC_MONITORPOWER, 2);
            }



            static public void WinKey()
            {
                KeyUse((short)Keys.LWin, false);
                KeyUse((short)Keys.LWin, true);
            }




            [DllImport("kernel32.dll")]
            static extern uint GetLastError();

            static void KeyUse(short Key, bool depress)
            {
                INPUT INPUT1 = new INPUT();
                INPUT1.type = (int)InputType.INPUT_KEYBOARD;
                INPUT1.ki.wVk = Key;
                INPUT1.ki.dwFlags = (int) (depress ? KEYEVENTF.KEYUP : KEYEVENTF.KEYDOWN);
                INPUT1.ki.dwExtraInfo = GetMessageExtraInfo();

                SendInput(1, new INPUT[] { INPUT1 }, Marshal.SizeOf(INPUT1));

            }


            static public void ZoomIn()
            {
                KeyUse((short)Keys.LWin, false);
                KeyUse((short)Keys.Oemplus, false);
                KeyUse((short)Keys.LWin, true);
                KeyUse((short)Keys.Oemplus, true);
            }

            static public void ZoomOut()
            {
                KeyUse((short)Keys.LWin, false);
                KeyUse((short)Keys.OemMinus, false);
                KeyUse((short)Keys.LWin, true);
                KeyUse((short)Keys.OemMinus, true);
            }

        

      }
}
