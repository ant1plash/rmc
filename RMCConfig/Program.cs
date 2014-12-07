using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Diagnostics;
using System.Text;

namespace ConfigTool
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        ///       
        /// 

        #region Dll Imports
        public const int HWND_BROADCAST = 0xFFFF;

        [DllImport("user32.dll")]
        public static extern bool SetForegroundWindow(IntPtr hWnd);

        [DllImport("user32")]
        public static extern bool PostMessage(IntPtr hwnd, int msg, IntPtr wparam, IntPtr lparam);

        [DllImport("user32")]
        public static extern int RegisterWindowMessage(string message);
        #endregion Dll Imports

        public static readonly int WM_ACTIVATEAPP = RegisterWindowMessage("WM_ACTIVATEAPP");




        static string ConvertStringArrayToString(string[] array)
        {
            //
            // Concatenate all the elements into a StringBuilder.
            //
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(' ');
            }
            return builder.ToString();
        }

        [STAThread]
        static void Main(string[] args)
        {

            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "RMCconfig", out createdNew))
            {
                if (createdNew)
                {
                    Application.EnableVisualStyles();
                    Application.SetCompatibleTextRenderingDefault(false);

                        ConfigForm f = new ConfigForm();
                        Application.Run(f);
                    
                }
            }
        }
    }
}
