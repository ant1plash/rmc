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


namespace RMCSrv
{
    static class Updater
    {
        private static string VERSION_URL = "http://ant1plash.tk/rmc_version.txt";
        private static string UPDATER_EXENAME = @"RMCUpdateTool.exe";

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
					LOGGER.LOG("Checking for updates...");
					int tries = 0;
					check_update: //Label to check
					
                    try
                    {
						tries++;
						
                        Thread.Sleep(15000);
                        Version Currentversion = Assembly.GetEntryAssembly().GetName().Version;
                        Version LatestVersion;
                        string getVersion = GET(VERSION_URL);

                        Version.TryParse(getVersion, out LatestVersion);
                        if (Currentversion < LatestVersion)
                        {
                            LOGGER.LOG("Got a new version, " + getVersion + " starting update...");
                            Process.Start(UPDATER_EXENAME, "startupdate"); //passing download
                            Environment.Exit(0);
                        }
                    }
                    catch (Exception)
                    {
						Thread.Sleep(15000);
						goto check_update; //Yup, goto :3
                    }
                }).Start();
        }


    }
}
