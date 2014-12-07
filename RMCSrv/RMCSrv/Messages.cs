using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization.Formatters.Binary;
using System.Runtime.Serialization;
using System.Diagnostics;
using System.Drawing;

namespace RMCSrv
    
{
    

    public class Messages
    {
        /* Constants */
        private const byte CurrentVersion = 16;
        private const string CorrectHeader = "RMCAPP";



        private const byte VOL_MUTE = 0x01;
        private const byte VOL_UP = 0x02;
        private const byte VOL_DOWN = 0x03;
        private const byte TRACK_NEXT = 0x04;
        private const byte TRACK_PREV = 0x05;
        private const byte TRACK_STOP = 0x06;
        private const byte TRACK_PAUSE = 0x07;
        private const byte TRACK_TURNOFF_SCREEN = 0x08;
        private const byte TRACK_TURNOFF_PC = 0x09;
        private const byte AUTHORIZE = 0x10;
        private const byte DEAUTH = 0x11;
        private const byte MOUSE_MOVE = 0x12;
        private const byte MOUSE_LMB = 0x13;
        private const byte MOUSE_RMB = 0x14;
        private const byte ZOOM_IN = 0x15;
        private const byte ZOOM_OUT = 0x16;
        private const byte TEST_CONNECTION = 0x18;
        private const byte WIN_KEY = 0x19;
        private const byte MY_PC = 0x20;
        private const byte CONTEXT_MENU_KEY = 0x21;
	
		/**
			New commads 
								**/
		private const byte DEFERRED_SHUTDOWN = 0x22;
		private const byte ARROW_UP_KEY = 0x23;
		private const byte ARROW_RIGHT_KEY = 0x24;
		private const byte ARROW_DOWN_KEY = 0x25;
		private const byte ARROW_LEFT_KEY = 0x26;
		private const byte HOME_KEY = 0x27;
		private const byte END_KEY = 0x28;
		private const byte ENTER_KEY = 0x29;
		private const byte MOUSE_LMB_DOUBLE = 0x30;
		private const byte ESC_KEY = 0x31;
		
		/**
		
			New commads 
								**/
								
        private const byte ANSWER_OK = 0x01;
        private const byte ANSWER_NOTAUTHED = 0x02;
        private const byte ANSWER_WRONGPASS = 0x03;

        

        private const int SEQ_COUNT = 64; //Count of packets to save in history
        private const int HOSTS_COUNT = 6; // Max authorized hosts

        public static bool started = false;
        public static int procId = 0;

        class FixedSizeList<T> : List<T> // List with a Limited size
        {
            private int MaxNumber;
            public FixedSizeList(int Limit)
                : base()
            {
                MaxNumber = Limit;
            }

            public void add(T t)
            {

                if (this.Count < MaxNumber)
                {
                    base.Add(t);
                }
                else
                {
                    base.RemoveAt(0);
                    base.Add(t);
                }
            }
        }



        [Serializable] //Used to serialize packets data
        class MSG
        {
            public string header;
            public byte proto_version;
            public int seq;
            public int command;
            public string data;
        }

        static FixedSizeList<int> SeqHistory = new FixedSizeList<int>(SEQ_COUNT); // Limited list with the sequences
        static FixedSizeList<String> Hosts = new FixedSizeList<String>(HOSTS_COUNT);    //and with the authorized hosts

        /*
         * 
         *        
         * 
         * 
         * */


        static byte[] FormatAnswer(int seq, byte answer) 
        {
            using (MemoryStream memStream = new MemoryStream(5))
            {
                memStream.Write(BitConverter.GetBytes(seq), 0, 4); //Copy sequence bytes
                memStream.WriteByte(answer);    //Write an answer
                return memStream.ToArray();
            }

        }


        public static byte[] ParseMessage(byte[] data, string from)
        {

            MSG Message = new MSG();

            BinaryReader br = new BinaryReader(new MemoryStream(data));

            br.BaseStream.Seek(0, SeekOrigin.Begin);

            Message.header = br.ReadString(); //
            Message.proto_version = br.ReadByte();
            Message.seq = br.ReadInt32();
            Message.command = br.ReadByte();
            Message.data = br.ReadString();

            br.Dispose();

            //eventLog1.WriteEntry("GOGO");

            if (!String.Equals(Message.header, CorrectHeader)) // Corrupted packet header
            {
                //LOGGER.LOG("Incorrect Header from " + from);
            //    eventLog1.WriteEntry("Incorrect Header from " + from);
                return null;
            }

            if (Message.proto_version != CurrentVersion) // Wrong protocol version
            {
                //LOGGER.LOG("Wrong protocol version from " + from);
                return null;
            }

                       
            if (!Hosts.Any(x => x.Contains(from)))// if a client is not authorized
            {
                if (Message.command == AUTHORIZE) // Trying to autorize
                {
                    if (String.Equals(Message.data.ToUpper(), Server.PasswordHash.ToUpper())) //Password is okay
                    {
                        Hosts.add(from);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
                    else
                    {
                        //LOGGER.LOG("Wrong password from " + from); // Wrong password
                        return FormatAnswer(Message.seq, ANSWER_WRONGPASS);
                    }
                
                }
                else return FormatAnswer(Message.seq, ANSWER_NOTAUTHED);// if a client is not authorized and not gonna to do that
            }


            if (SeqHistory.Find(x => x == Message.seq) != 0) //Searching for processed packets
                    return FormatAnswer(Message.seq, ANSWER_OK); // this packet has been processed already

            SeqHistory.add(Message.seq);  // this one, has not beed processed 


            switch (Message.command)//Executing a command
            {
                case VOL_MUTE:
                    {
                        KeyControls.send_event(KeyControls.VK_VOLUME_MUTE);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
                case VOL_UP:
                    {
                        KeyControls.send_event(KeyControls.VK_VOLUME_UP);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
                case VOL_DOWN:
                    {
                        KeyControls.send_event(KeyControls.VK_VOLUME_DOWN);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
                case TRACK_NEXT:
                    {
                        KeyControls.send_event(KeyControls.VK_MEDIA_NEXT_TRACK);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
                case TRACK_PREV:
                    {
                        KeyControls.send_event(KeyControls.VK_MEDIA_PREV_TRACK);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }

                case TRACK_STOP:
                    {
                        KeyControls.send_event(KeyControls.VK_MEDIA_STOP);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
                case TRACK_PAUSE:
                    {
                        KeyControls.send_event(KeyControls.VK_MEDIA_PLAY_PAUSE);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
                case TRACK_TURNOFF_SCREEN:
                    {
                        KeyControls.TurnOffScreen();
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }

                case TRACK_TURNOFF_PC:
                    {
                        Process.Start("shutdown", "-s -f -t 0"); //not sure about this, sometimes this fails
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }

                case TEST_CONNECTION:
                    {
                       // Form1.showBalloon("Remote Media Control Server", "It works! Looks like you have configured the application well.");
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
                case MOUSE_MOVE:
                    {

                        string[] words = Message.data.Split(' ');
                       // Cursor.Position = new Point(Cursor.Position.X + Convert.ToInt32(words[0]), Cursor.Position.Y + Convert.ToInt32(words[1]));
                        CursorManager.SmoothMove(Convert.ToInt32(words[0]), Convert.ToInt32(words[1]));
                        return null;
                    }


                    case MOUSE_LMB:
                    {
                        CursorManager.Click();
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }

                    case MOUSE_RMB:
                    {
                        CursorManager.RightClick();
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }


                    case WIN_KEY:
                    {
                        KeyControls.WinKey();
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }

                    case MY_PC:
                    {
                        System.Diagnostics.Process.Start("explorer", Environment.GetFolderPath(Environment.SpecialFolder.MyComputer));
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }


                    case ZOOM_IN:
                    {
                        KeyControls.ZoomIn();
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }

                    case ZOOM_OUT:
                    {
                        KeyControls.ZoomOut();
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }

                    		/**
						New commads 
								**/
								
                    case DEFERRED_SHUTDOWN: // TODO
                    {
                        //KeyControls.ZoomOut(); 
						
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
					
					/* arrows */
                    case ARROW_UP_KEY:
                    {
						KeyControls.send_event(KeyControls.VK_UP);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }

					case ARROW_RIGHT_KEY:
                    {
                        KeyControls.send_event(KeyControls.VK_RIGHT);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
					
					case ARROW_DOWN_KEY:
                    {
                        KeyControls.send_event(KeyControls.VK_DOWN);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
					
					case ARROW_LEFT_KEY:
                    {
                        KeyControls.send_event(KeyControls.VK_LEFT);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
					
					/* new keyboard keys */
					case HOME_KEY:
                    {
                        KeyControls.send_event(KeyControls.VK_HOME);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
					
					case END_KEY:
                    {
                        KeyControls.send_event(KeyControls.VK_END);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
					
					case ENTER_KEY:
                    {
                        KeyControls.send_event(KeyControls.VK_RETURN);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
					
					case MOUSE_LMB_DOUBLE:
                    {
                        CursorManager.DoubleClick();
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
					
					
					case ESC_KEY:
                    {
                        KeyControls.send_event(KeyControls.VK_ESC);
                        return FormatAnswer(Message.seq, ANSWER_OK);
                    }
					

            }
            return null;
        }
    }
}
