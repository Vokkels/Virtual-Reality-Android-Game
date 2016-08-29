using System;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.IO;
using System.Collections.Generic;

namespace LAN_Networking
{
    public class NetworkServer
    {
        static Socket listenerSocket;
        static List<ClientData> _clients;

        public NetworkServer()
        {
            Console.WriteLine("Starting server on " + Packet.getIP4Address());
            listenerSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _clients = new List<ClientData>();

            IPEndPoint ip = new IPEndPoint(IPAddress.Parse(Packet.getIP4Address()), 4242);
            listenerSocket.Bind(ip);

            Thread listenThread = new Thread(ListenThread);
            listenThread.Start();
        }

        //Listens for new clients to connect
        public void ListenThread()
        {
            for (;;)
            {
                listenerSocket.Listen(0);
                _clients.Add(new ClientData(listenerSocket.Accept()));

            }
        }

        //Incoming Data
        public static void Data_IN(object cSocket)
        {
            Socket clientSocket = (Socket)cSocket;
            byte[] Buffer;
            int readBytes;

            for (;;)
            {
                Buffer = new byte[clientSocket.SendBufferSize];
                readBytes = clientSocket.Receive(Buffer);
                if (readBytes > 0)
                {
                    //handle incoming data
                    Packet packet = new Packet(Buffer);
                    DataManager(packet);
                }
            }
        }

        public static void DataManager(Packet p)
        {
            switch (p.packetType)
            {
                case Packet.PacketType.Chat:
                    foreach (ClientData c in _clients)
                        c.clientSocket.Send(p.ToBytes());
                    break;
            }
        }
    }

    public class ClientData
    {
        public Socket clientSocket;
        public Thread clientThread;
        public string id;

        public ClientData()
        {
            id = Guid.NewGuid().ToString();
            clientThread = new Thread(NetworkServer.Data_IN);
            clientThread.Start(clientSocket);
            SendResistrationPacket();
        }

        public ClientData(Socket clientSocket)
        {
            this.clientSocket = clientSocket;
            id = Guid.NewGuid().ToString();
            clientThread = new Thread(NetworkServer.Data_IN);
            clientThread.Start(clientSocket);
            SendResistrationPacket();
        }

        public void SendResistrationPacket()
        {
            Packet p = new Packet(Packet.PacketType.Registration, "server");
            p.Gdata.Add(id);
            clientSocket.Send(p.ToBytes());

        }
    }
}

