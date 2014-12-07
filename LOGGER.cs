using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;


namespace ConfigTool
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
			try {		
				using (StreamWriter w = File.AppendText(LOG_FILE))
				{
					w.WriteLine(text);
					w.Close();
				}
			}
			catch (Exception ex) 
			{
			
			}
            return;
        }
		
		private async Task WriteTextAsync(string filePath, string text)
		{
			byte[] encodedText = Encoding.Unicode.GetBytes(text);

			using (FileStream sourceStream = new FileStream(filePath,
				FileMode.Append, FileAccess.Write, FileShare.None,
				bufferSize: 4096, useAsync: true))
			{
				await sourceStream.WriteAsync(encodedText, 0, encodedText.Length);
			};
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
