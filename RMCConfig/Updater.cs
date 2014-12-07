using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Diagnostics;
using System.Net;
using System.IO;
using System.Windows.Forms;
using System.Threading; 


namespace ConfigTool
{
    static class Updater
    {
        private static string Address = "http://remotemediacontrol.tk/";
        private static string UPDATEREXENAME = @"RemoteMediaControlUpdater.exe";

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



        public static void CheckUpdate()
        {
            new Thread(
                delegate()
                {
                    try
                    {
                        Thread.Sleep(35000);
                        Version Currentversion = Assembly.GetEntryAssembly().GetName().Version;
                        Version LatestVersion;
                        string getVersion = GET(Address + "version.txt");

                        Version.TryParse(getVersion, out LatestVersion);
                        if (Currentversion < LatestVersion)
                        {
                            LOGGER.LOG("Got a new version, " + getVersion + " starting update...");
                            string TempName = LatestVersion.ToString() + ".zip";
                            Process.Start(UPDATEREXENAME, TempName);
                            Environment.Exit(0);
                        }
                    }
                    catch (Exception)
                    {

                    }
                }).Start();
        }


    }
}
