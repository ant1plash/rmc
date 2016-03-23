using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.IO;
using System.Xml;



namespace RMCSrv
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        /// 
        private static string CurDir = (new FileInfo(System.Reflection.Assembly.GetEntryAssembly().Location)).Directory.ToString() + "\\";
        

        public static string CONF_FILE = Path.Combine(Environment.GetFolderPath(
            Environment.SpecialFolder.ApplicationData), "RMC_conf.xml"); // Application config file

        [STAThread]
        static void Main(string[] args)
        {

            
            LOGGER.LOG("-----------------------------------------");
            LOGGER.LOG("STARING");

            bool createdNew = true;
            using (Mutex mutex = new Mutex(true, "RMCserver", out createdNew))
            {
                if (!createdNew)
                {
                    LOGGER.LOG("Duplicate process, please kill another one");
                    Environment.Exit(0);
                }

            }

            if (args.Count() <= 0)
            {
                Updater.CheckUpdate();
            }



            



            Cfg config = new Cfg(CONF_FILE);




            if (!config.isCorrect)
            {
                LOGGER.LOG("Warning! Incorrect config! Starting config tool...");
                RunWithUAC("RMCConfig", "");
                Environment.Exit(0);
            }

            Server.StartServer(config.port, CalculateMD5Hash(config.password));
            while (0 == 0)
            {
                Thread.Sleep(1000);
            }
        }


        public static void RunWithUAC(string ExecutableFileName, string Args)
        {
            ProcessStartInfo info = new ProcessStartInfo(CurDir+ExecutableFileName, Args);

            info.WindowStyle = ProcessWindowStyle.Normal;
            info.Verb = "runas";

            try
            {
                Process.Start(info);
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public static string CalculateMD5Hash(string input) // Calculates MD5
        {
            // step 1, calculate MD5 hash from input
            MD5 md5 = System.Security.Cryptography.MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hash = md5.ComputeHash(inputBytes);

            // step 2, convert byte array to hex string
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }
            return sb.ToString();
        }

        class Cfg
        {
            public int port;
            public string password;
            public bool isCorrect = false;
            public Cfg(string fileName)
            {

                XmlDocument xmlDoc = new XmlDocument();

                // Загружаем XML-документ из файла
                try
                {
                    xmlDoc.Load(fileName);
                }
                catch (Exception)
                {
                    LOGGER.LOG("No config found!");
                    return;
                }

                foreach (XmlNode table in xmlDoc.DocumentElement.ChildNodes)
                {
                    foreach (XmlNode ch in table.ChildNodes)
                    {
                        switch (ch.Name)
                        {
                            case "Password":
                                if (ch.InnerText.Length > 0)
                                {
                                    this.password = ch.InnerText;
                                }
                                else {
                                    isCorrect = false;
                                    return;
                                };

                                break;
                            case "ServerPort":
                                int temp = 9050;
                                try
                                {
                                    temp = Convert.ToInt32(ch.InnerText);
                                }
                                catch (Exception ex)
                                {
                                    LOGGER.LOG(ex.Message);
                                }

                                if (temp > 0 && temp < 65535)
                                {
                                    this.port = temp;
                                }
                                else
                                {
                                    this.port = 9050;
                                }

                                break;
                        }


                    }


                }//foreach

                isCorrect = true;
            }
        }
    }
}
