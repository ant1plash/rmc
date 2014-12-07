using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;
using System.Runtime.InteropServices;
using System.Net;
using System.Net.Sockets;
using System.Xml;
using System.Security.Cryptography;
using Gma.QrCodeNet.Encoding.Windows.Render;
using Gma.QrCodeNet.Encoding;
using System.Timers;
using System.Net.NetworkInformation;
using System.Reflection; 
using System.IO;
using System.Diagnostics;


namespace ConfigTool
{
 

    public partial class ConfigForm : Form
    {
        private string RMC_URL = "http://ant1plash.tk";
		private string QR_PREFIX = "rmcapplication";

        /*
         * 
         * */

        public ConfigForm()
        {
            InitializeComponent();
        }

        public bool HideONStart = false;
        public static string _ConfigHash;
        private int _ConfigPort;

        public static string CONF_FILE = Path.Combine(Environment.GetFolderPath(
        Environment.SpecialFolder.ApplicationData), "RMC_conf.xml"); // Application config file


        public string CalculateMD5Hash(string input) // Calculates MD5
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

        private readonly Random _rng = new Random();
        private const string _chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        private string RandomString(int size)
        {
            char[] buffer = new char[size];

            for (int i = 0; i < size; i++)
            {
                buffer[i] = _chars[_rng.Next(_chars.Length)];
            }
            return new string(buffer);
        }

        public class NetAdapter
        {
            public IPAddress IP;
            public string name;
            public string mac;


            public override string ToString()
            {
                return string.Format(IP.ToString());
            }
        }

        public List<NetAdapter> Networks = new List<NetAdapter>();

       /***********************************
        ***********************************
        ************************************/




        private bool LoadConfig()
        {
            XmlDocument xmlDoc = new XmlDocument();

            // Загружаем XML-документ из файла
            try
            {
                xmlDoc.Load(CONF_FILE);
            }
            catch (Exception)
            {
                LOGGER.LOG("No config found! Generating new one");
                _ConfigHash = RandomString(8);
                _ConfigPort = 9050;

                passBox.Text = _ConfigHash;
                portNum.Value = _ConfigPort;
                SaveConfig();
                startButton_Click(null, null);
                return true;
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
                                _ConfigHash = ch.InnerText;
                                passBox.Text = _ConfigHash;
                            }
                            else passBox.Text = "";
                            
                            break;
                        case "ServerPort":
                            int temp = Convert.ToInt32(ch.InnerText);
                            if (temp > 0 && temp < 65535)
                            {
                                portNum.Value = temp;
                                _ConfigPort = temp;
                            }
                            else
                            {
                                portNum.Value = 9050;
                                _ConfigPort = 9050;
                            }

                            break;
                    }
                }
 
            }
            return true;

        }

        public void LockForm()// Locks the form
        {
            saveButton.Enabled = false;
          //  button4.Enabled = false;
            portNum.Enabled = false;
            passBox.Enabled = false;
            infoLabel.Text = "Server is working!";
            infoLabel.ForeColor = Color.LimeGreen;
            startButton.Text = "Stop Server";
        }

        public  void unLockForm()
        {
            saveButton.Enabled = true;
         //   button4.Enabled = true;
            portNum.Enabled = true;
            passBox.Enabled = true;
            infoLabel.Text = "Server not started";
            infoLabel.ForeColor = Color.Red;
            startButton.Text = "Start Server";
        }

        private void SaveConfig()
        {
            XmlTextWriter textWritter = new XmlTextWriter(CONF_FILE, Encoding.UTF8);

            textWritter.WriteStartDocument();
            textWritter.WriteStartElement("config");
            textWritter.WriteEndElement();
            textWritter.Close();

            XmlDocument document = new XmlDocument();


            document.Load(CONF_FILE);

            /*
             * 
             * 
             * */

            XmlNode element = document.CreateElement("configuration");
            document.DocumentElement.AppendChild(element); 


            XmlNode subElement1 = document.CreateElement("Password");
            subElement1.InnerText = passBox.Text;
            element.AppendChild(subElement1);

            XmlNode subElement2 = document.CreateElement("ServerPort");
            subElement2.InnerText = portNum.Value.ToString();
            element.AppendChild(subElement2);

            document.Save(CONF_FILE);

            LoadConfig();
        }





        public void GetInterfaces()
        {                          
            NetworkInterface[] interfaces = NetworkInterface.GetAllNetworkInterfaces();
            IPHostEntry host;
            host = Dns.GetHostEntry(Dns.GetHostName());

            Networks.Clear();
            foreach (NetworkInterface adapter in interfaces)
            {
                var ipProps = adapter.GetIPProperties();

                foreach (var ip in ipProps.UnicastAddresses)
                {
                    if ((adapter.OperationalStatus == OperationalStatus.Up)
                        && (ip.Address.AddressFamily == AddressFamily.InterNetwork
                        && (!string.Equals(ip.Address.ToString(), "127.0.0.1"))))
                    {
                        Networks.Add(new NetAdapter
                        {
                            IP = ip.Address,
                            name = adapter.Description.ToString(),
                            mac = adapter.GetPhysicalAddress().ToString()
                        });
                    }
                }
   
            }


        }



       Process srvProcess;
        private bool isRunning()
        {
            if (srvProcess != null && !srvProcess.HasExited) return true;

            bool running = false;

            Process[] pname = Process.GetProcessesByName("RMCSrv");
            if (pname.Length == 0)
                running = false;
            else
            {
                running = true;
                srvProcess = pname[0];
                srvProcess.EnableRaisingEvents = true;
                srvProcess.Exited += new EventHandler(onProcessExited);
                srvProcess.Refresh();
            }


            return running;
        }

        private void onProcessExited(object sender, System.EventArgs e)
        {
            unLockForm();
        }

        private void RunWithUAC(string ExecutableFileName, string Args)
        {
            ProcessStartInfo info = new ProcessStartInfo(ExecutableFileName, Args);

            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
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

        private void KillServer()
        {
            if (srvProcess != null && !srvProcess.HasExited)
            {
                srvProcess.Kill();
                srvProcess.WaitForExit();
            }
        }



        private void ConfigForm_Load(object sender, EventArgs e)
        {
            this.Visible = false;
            

            LOGGER.CleanUP();
            LoadConfig();

            if (isRunning())
            {
                LockForm();
            }

            GetInterfaces();
            adaptersBox.DataSource = Networks;
            adaptersBox.DrawMode = DrawMode.OwnerDrawFixed;

            this.Text += " " + Assembly.GetEntryAssembly().GetName().Version.ToString();
            
        }

   


        private void startButton_Click(object sender, EventArgs e)
        {
            if (isRunning())
            {
                unLockForm();
                KillServer();
            }
            else
            {
                LockForm();
                RunWithUAC(Path.GetDirectoryName(Application.ExecutablePath)+"/"+"RMCSrv", "");
            }
        }

		private void saveButton_Click(object sender, EventArgs e)
        {
            SaveConfig();
        }
		
 




        private void notifyIcon1_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Normal;
            this.Activate();
            this.Show();
            this.BringToFront();
        }





        private void GenerateConfigQR(object sender, EventArgs e)
        {
         //   MessageBox.Show(GetMacAddress());

            if (_ConfigHash == null || _ConfigHash.Length <= 0)
                passBox.Text = RandomString(8);

            if (Networks.Count < 1) return;

            string txtEncodeData = QR_PREFIX +" " + Networks[adaptersBox.SelectedIndex].IP + " " + portNum.Value + " " + passBox.Text + " " + Networks[adaptersBox.SelectedIndex].mac;

            QrEncoder encoder = new QrEncoder(Gma.QrCodeNet.Encoding.ErrorCorrectionLevel.M);
            QrCode qrCode;

            encoder.TryEncode(txtEncodeData, out qrCode);

            GraphicsRenderer gRenderer = new GraphicsRenderer(
                new FixedModuleSize(4, QuietZoneModules.Two),
                Brushes.Black, Brushes.White);

            MemoryStream ms = new MemoryStream();
            gRenderer.WriteToStream(qrCode.Matrix, ImageFormat.Png, ms);
            var image = Image.FromStream(ms);

            pictureBox1.Image = image;

            SaveConfig();
        }





        private void adaptersBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            GenerateConfigQR(sender, e);
        }

        private void ConfigForm_Shown(object sender, EventArgs e)
        {
            if (HideONStart)
            {
                this.Hide();
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start(RMC_URL);
        }

        private void passBox_TextChanged(object sender, EventArgs e)
        {
            GenerateConfigQR(sender, e);
        }

        private void adaptersBox_DrawItem(object sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) { return; } // added this line thanks to Andrew's comment
            string text = adaptersBox.GetItemText(adaptersBox.Items[e.Index]);
            string hint = Networks[e.Index].IP + " " + Networks[e.Index].name + " MAC: " + Networks[e.Index].mac;

            e.DrawBackground();
            //  e.DrawFocusRectangle();
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
                e.Graphics.FillRectangle(Brushes.LightGreen, e.Bounds);
            else
                e.Graphics.FillRectangle(Brushes.White, e.Bounds);

            using (SolidBrush br = new SolidBrush(Color.Black))
            { e.Graphics.DrawString(text, e.Font, br, e.Bounds); }
            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected)
            { adapterTip.Show(hint, adaptersBox, e.Bounds.Right, e.Bounds.Bottom); }
            else { e.DrawFocusRectangle(); }
        }

        private void adaptersBox_DropDownClosed(object sender, EventArgs e)
        {
            adapterTip.Hide(adaptersBox);
        }


    }
}
