using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace RMCSrv
{
    class LOGGER
    {
        public static string LOG_FILE = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData), "RMC_log.txt");
        public static int LOG_MAX_SIZE = 1024*1024; // bytes


        public static void CleanUP()
        {
            if (FileSize(LOG_FILE) > LOG_MAX_SIZE)
            {
                File.Delete(LOG_FILE);
            }
            return;
        }

        public static void LOG(string text)
        {
            using (StreamWriter w = File.AppendText(LOG_FILE))
            {
                w.WriteLine(text);
                w.Close();
            }
            return;
        }

        private static long FileSize(string FileName)
        {
            if (File.Exists(FileName))
            {
                try
                {
                    FileInfo fInfo = new FileInfo(FileName);


                    return fInfo.Length;
                }
                catch (Exception)
                {
                    return 0;
                }
            }
            return 0;
        }
    }
}
