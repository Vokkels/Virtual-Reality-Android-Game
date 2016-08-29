using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;

namespace LAN_Networking
{
    
    public class NetworkClient
    {
        public static Socket master;
        public static string name;
        public static string id;

        public NetworkClient()
        {

            Console.Write("Enter your name: ");
            name = Console.ReadLine();

            A: Console.Clear();
            Console.Write("Enter host IP address: ");
            string ip = Console.ReadLine();
            master = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            IPEndPoint ipe = new IPEndPoint(IPAddress.Parse(ip), 4242);

            try
            {
                master.Connect(ipe);
            }

            catch
            {
                Console.WriteLine("Could not connect to host!");
                Thread.Sleep(1000);
                goto A;
            }

            Thread t = new Thread(Data_IN);
            t.Start();

            for (;;)
            {
                Console.Write("::>");
                string input = Console.ReadLine();

                Packet p = new Packet(Packet.PacketType.Chat, id);
                p.Gdata.Add(name);
                p.Gdata.Add(input);
                master.Send(p.ToBytes());
                    
            }
        }

        static void Data_IN()
        {
            byte[] Buffer;
            int readBytes;

            for (;;)
            {
                Buffer = new byte[master.SendBufferSize];
                readBytes = master.Receive(Buffer);
                if (readBytes > 0)
                {
                    DataManager(new Packet(Buffer));
                }
            }
        }

        static void DataManager(Packet p)
        {
            switch (p.packetType)
            {
                case Packet.PacketType.Registration:
                    //Console.WriteLine("Recieve a packtet for registration! Responding...");
                    id = p.Gdata[0];
                    break;

                case Packet.PacketType.Chat:
                    ConsoleColor c = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Cyan;
                    Console.WriteLine(p.Gdata[0] + ": " + p.Gdata[1]);
                    Console.ForegroundColor = c;
                    break;
            }
        }
    }
}