using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Media;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace RMCSrv
{


    static class Server
    {

        private static Thread SvrThread;
        private static bool working; 
        private static int port;
        public static string PasswordHash;
        private static Socket mysocket;

        public static void StartServer(int _port, string Hash)
        {
            port = _port;
            PasswordHash = Hash;
            working = true;
            SvrThread = new Thread(Listen);
            SvrThread.IsBackground = true;
            SvrThread.Start();
        }



        public static void StopServer()
        {
            
            mysocket.Close();
            working = false;
    //       SvrThread.Abort();
        }

        public static bool IsWorking()
        {
            if (working) return true;
            return false;
        }

        public static void Listen()
        {
	        int recv; 
	        byte[] data = new byte[64];
            start:

	        mysocket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);


            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            try
            {
                mysocket.Bind(ipep);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Can't bind to port " + port + ex.Message, "Remote Media Control");
                LOGGER.LOG("Can't bind to port " + port + ex.Message);
                //RmcService.eventLog1.WriteEntry(ex.Message);
                working = false;
                mysocket.Close();

                return;
            }
	        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
	        EndPoint Remote = (EndPoint)(sender);
           // mysocket.ReceiveTimeout = 100;


            while (working)
            {
                data = new byte[64];
                
                try
                {

                    recv = mysocket.ReceiveFrom(data, ref Remote);

                   // SystemSounds.Beep.Play();

                    byte[] answer = Messages.ParseMessage(data, Remote.ToString());

                    if (answer != null)
                    {
                        if (BitConverter.IsLittleEndian)
                            Array.Reverse(data);//

                        mysocket.SendTo(answer, answer.Length, SocketFlags.None, Remote);
                    }

                }
                catch (Exception ex)
                {
                    //RmcService.eventLog1.WriteEntry(ex.Message);
                    LOGGER.LOG("Unable to RCVFROM, info:" + ex.Message);
                    mysocket.Close();
                    goto start;
                }
            }

            mysocket.Close();
            
        }

        private static EndPoint _getHost(string text)
        {

            string clientport = text.Substring(text.IndexOf(":")+1);

            string host = text.Remove(text.IndexOf(":"), text.Length - text.IndexOf(":"));


            IPAddress hostIPAddress = IPAddress.Parse(host);
            IPEndPoint hostIPEndPoint = new IPEndPoint(hostIPAddress, Convert.ToInt16(clientport));
            EndPoint To = (EndPoint)(hostIPEndPoint);
            return To;
        }
    }
}
