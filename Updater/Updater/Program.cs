using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Reflection;
using System.Diagnostics;

using ICSharpCode.SharpZipLib.Zip;
using ICSharpCode.SharpZipLib.Zip.Compression.Streams;


namespace Updater
{
    class Program
    {
		private static string DOWNLOAD_URL = "http://ant1plash.tk/rmc_update.zip";

        private static string curdir = (new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).Directory.ToString() + "\\";

        public static string SERVER_EXENAME = curdir + "RMCSrv.exe";
                                
       
        static void Main(string[] args)
        {
            if (args.Count() <= 0) return;
            if (args[0] != "startupdate") return;
			
            string tmpFname = "";
            try
            {
                tmpFname = GenerateFilename();

                WebClient Client = new WebClient();
                Client.DownloadFile(DOWNLOAD_URL, tmpFname);
                Client.Dispose();

                /*********************************/
                KillBeforeUpdate("RMCSrv");
                KillBeforeUpdate("RMCConfig");
                /*********************************/

                if(unzip(tmpFname, curdir))
                    File.Delete(tmpFname);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				LOGGER.LOG("Error during update:\n" + ex.ToString());
            }


            Process.Start(SERVER_EXENAME, "updated");

        }

        private static void KillBeforeUpdate(string name) 
        {
            try
            {
                foreach (var process in Process.GetProcessesByName(name))
                {
                    process.Kill();
                }
            }
            catch (Exception)
            {

            }
        }
		
        private static string GET(string Url)
        {
            System.Net.WebRequest req = System.Net.WebRequest.Create(Url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.Stream stream = resp.GetResponseStream();
            System.IO.StreamReader sr = new System.IO.StreamReader(stream);
            string Out = sr.ReadToEnd();
            sr.Close();
            return Out;
        }

        public static bool unzip(string Filename, string DirTo)
        {
            if (!File.Exists(Filename))
            {
                return false;
            }

            using (ZipInputStream s = new ZipInputStream(File.OpenRead(Filename)))
            {

                ZipEntry theEntry;
                while ((theEntry = s.GetNextEntry()) != null)
                {

                    Console.WriteLine(theEntry.Name);

                    string directoryName = Path.GetDirectoryName(theEntry.Name);
                    string fileName = Path.GetFileName(theEntry.Name);

                    // create directory
                    if (directoryName.Length > 0)
                    {
                        Directory.CreateDirectory(directoryName);
                    }

                    if (fileName != String.Empty)
                    {
                        using (FileStream streamWriter = File.Create(DirTo+theEntry.Name))
                        {

                            int size = 2048;
                            byte[] data = new byte[2048];
                            while (true)
                            {
                                size = s.Read(data, 0, data.Length);
                                if (size > 0)
                                {
                                    streamWriter.Write(data, 0, size);
                                }
                                else
                                {
                                    break;
                                }
                            }
                        }
                    }
                }
            }
            return true;
        }


        private static string GenerateFilename()
        {
            string result = curdir+"\\update";

            Random random = new Random();
            random.Next(0, 9);

            while (File.Exists(result + ".zip"))
            {
                result += random.Next(0, 9).ToString();
            }
            return result + ".zip";
        }


        static string ConvertStringArrayToString(string[] array)
        {
            StringBuilder builder = new StringBuilder();
            foreach (string value in array)
            {
                builder.Append(value);
                builder.Append(' ');
            }
            return builder.ToString();
        }


    }
}
