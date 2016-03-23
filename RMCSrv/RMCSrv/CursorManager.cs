using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Threading;
using System.Drawing;


namespace RMCSrv
{
    static class CursorManager
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern uint SendInput(uint nInputs, ref INPUT pInputs, int cbSize);

        [StructLayout(LayoutKind.Sequential)]
        struct INPUT
        {
            public SendInputEventType type;
            public MouseKeybdhardwareInputUnion mkhi;
        }

        [StructLayout(LayoutKind.Explicit)]
        struct MouseKeybdhardwareInputUnion
        {
            [FieldOffset(0)]
            public MouseInputData mi;

            [FieldOffset(0)]
            public KEYBDINPUT ki;

            [FieldOffset(0)]
            public HARDWAREINPUT hi;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct KEYBDINPUT
        {
            public ushort wVk;
            public ushort wScan;
            public uint dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [StructLayout(LayoutKind.Sequential)]
        struct HARDWAREINPUT
        {
            public int uMsg;
            public short wParamL;
            public short wParamH;
        }
        struct MouseInputData
        {
            public int dx;
            public int dy;
            public uint mouseData;
            public MouseEventFlags dwFlags;
            public uint time;
            public IntPtr dwExtraInfo;
        }
        [Flags]
        enum MouseEventFlags : uint
        {
            MOUSEEVENTF_MOVE = 0x0001,
            MOUSEEVENTF_LEFTDOWN = 0x0002,
            MOUSEEVENTF_LEFTUP = 0x0004,
            MOUSEEVENTF_RIGHTDOWN = 0x0008,
            MOUSEEVENTF_RIGHTUP = 0x0010,
            MOUSEEVENTF_MIDDLEDOWN = 0x0020,
            MOUSEEVENTF_MIDDLEUP = 0x0040,
            MOUSEEVENTF_XDOWN = 0x0080,
            MOUSEEVENTF_XUP = 0x0100,
            MOUSEEVENTF_WHEEL = 0x0800,
            MOUSEEVENTF_VIRTUALDESK = 0x4000,
            MOUSEEVENTF_ABSOLUTE = 0x8000
        }
        enum SendInputEventType : int
        {
            InputMouse,
            InputKeyboard,
            InputHardware
        }


        private static int dx = 0;//diff
        private static int dy = 0;
        private static int cx = 0;//current
        private static int cy = 0;

        private static bool working = false;
        private static long IdleTime = 0;
        private const long IDLE_TIMEOUT = 20000;


        static Queue<Point> queue = new Queue<Point>();

        public static void SmoothMove(int _dx, int _dy)
        {
            cx = Cursor.Position.X;
            cy = Cursor.Position.Y;
            int steps = 5;
            
          //  for (int i = steps; i > 0; i--)
          //  {
             //   PointF fpoint = new PointF(_dx / i, _dy / i);
                queue.Enqueue(new Point(_dx, _dy));//Point.Round(fpoint));
         //   }
          
            dx = _dx; // (int)Math.Pow(_dx, 3);// (int)Math.Log10(_dx);
            dy = _dy; // (int)Math.Pow(_dy, 3);
            IdleTime = 0;
            if (!working) StartThread();
        }

      /*  public void LinearSmoothMove(Point newPosition, int steps)
        {
            Point start = GetCursorPosition();
            PointF iterPoint = start;

            // Find the slope of the line segment defined by start and newPosition
            PointF slope = new PointF(newPosition.X - start.X, newPosition.Y - start.Y);

            // Divide by the number of steps
            slope.X = slope.X / steps;
            slope.Y = slope.Y / steps;

            // Move the mouse to each iterative point.
            for (int i = 0; i < steps; i++)
            {
                iterPoint = new PointF(iterPoint.X + slope.X, iterPoint.Y + slope.Y);
                SetCursorPosition(Point.Round(iterPoint));
                Thread.Sleep(MouseEventDelayMS);
            }

            // Move the mouse to the final destination.
            SetCursorPosition(newPosition);
        }*/
        public static int Sign(int value)
        {
            if (value < 0)
            {
                return -1;
            }
            if (value > 0)
            {
                return 1;
            }
            return 0;
        }
        
        public static void StartThread()
        {
            

            working = true;
            Thread a = new Thread(
                         delegate()
                         {
                             while (working)
                             {

                                 try
                                 {
                                     if (queue.Count > 0)
                                     {
                                         
                                      /*   TODO: Optimize this*/
                                         Point cur;
                                         lock (queue)
                                         {
                                             cur = queue.Dequeue();
                                         }

                                         INPUT mouseMoveInput = new INPUT();
                                         mouseMoveInput.type = SendInputEventType.InputMouse;
                                         mouseMoveInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_MOVE;
                                         mouseMoveInput.mkhi.mi.time = 0;//.mkhi.mi.dx
                                         mouseMoveInput.mkhi.mi.dx = cur.X;
                                         mouseMoveInput.mkhi.mi.dy = cur.Y;
                                         SendInput(1, ref mouseMoveInput, Marshal.SizeOf(new INPUT()));


                                         Thread.Sleep(0);
                                       //  IdleTime = 0;
                                     }
                                     else Thread.Sleep(5);
                                     
                                  //   IdleTime += 5;
                                //     if (IdleTime > IDLE_TIMEOUT) working = false;
                                 }
                                 catch (Exception ex)
                                 {
                                     MessageBox.Show("smooth move" + ex.Message);
                                     working = false;
                                 }
                             }
                         });
            a.IsBackground = true;
            a.Start();
        }



        static public void Click()
        {
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));

            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref mouseUpInput, Marshal.SizeOf(new INPUT()));
        }

        static public void LeftPress()
        {
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTDOWN;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));
        }

        static public void LeftDepress()
        {
            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_LEFTUP;
            SendInput(1, ref mouseUpInput, Marshal.SizeOf(new INPUT()));
        }

        static public void DoubleClick()
        {
            Click();
			Click();
			/* What a code, lol */
        }

        static public void RightClick()
        {
            INPUT mouseDownInput = new INPUT();
            mouseDownInput.type = SendInputEventType.InputMouse;
            mouseDownInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTDOWN;
            SendInput(1, ref mouseDownInput, Marshal.SizeOf(new INPUT()));

            INPUT mouseUpInput = new INPUT();
            mouseUpInput.type = SendInputEventType.InputMouse;
            mouseUpInput.mkhi.mi.dwFlags = MouseEventFlags.MOUSEEVENTF_RIGHTUP;
            SendInput(1, ref mouseUpInput, Marshal.SizeOf(new INPUT()));
        }

    }
}
