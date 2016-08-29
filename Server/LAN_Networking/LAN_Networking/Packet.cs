using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

namespace LAN_Networking
{
    [Serializable]
    public class Packet
    {
        public List<string> Gdata;
        public int packetInt;
        public bool packetBool;
        public string senderID;
        public PacketType packetType;

        public Packet(PacketType type, string senderID)
        {
            Gdata = new List<string>();
            this.senderID = senderID;
            this.packetType = type;
        }

        public Packet(byte[] packetBytes)
        {
            try
            {
                BinaryFormatter bf = new BinaryFormatter();
                MemoryStream ms = new MemoryStream(packetBytes);
                Packet p = (Packet)bf.Deserialize(ms);
                ms.Close();
                this.Gdata = p.Gdata;
                this.packetInt = p.packetInt;
                this.packetBool = p.packetBool;
                this.senderID = p.senderID;
                this.packetType = p.packetType;
            }
            catch (Exception x)
            {
                Console.WriteLine("ERROR!: " + x.Message   );
            }
        }

        //save package - turn data into bytes
        public byte[] ToBytes()
        {
            BinaryFormatter bf = new BinaryFormatter();
            MemoryStream ms = new MemoryStream();

            bf.Serialize(ms, this);
            byte[] bytes = ms.ToArray();
            ms.Close();
            return bytes;
        }

        public static string getIP4Address()
        {
            IPAddress[] ips = Dns.GetHostAddresses(Dns.GetHostName());
            foreach (IPAddress i in ips)
            {
                if (i.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    return i.ToString();
            }

            return "127.0.0.1";
        }
        public enum PacketType
        {
            Registration,
            Chat
        }
    }
}
