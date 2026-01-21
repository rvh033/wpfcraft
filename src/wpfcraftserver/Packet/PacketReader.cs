using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.Net.Sockets;
using System.Xml.Serialization;
using System.IO;
using System.Diagnostics;

namespace wpfcraftserver.Packet
{
    internal class PacketReader : BinaryReader
    {
        public PacketReader(NetworkStream networkStream) : base(networkStream)
        {
            NetworkStream = networkStream;
            Init();
        }

        NetworkStream NetworkStream;

        void Init()
        {

        }

        public string ReadPacketString()
        {
            string s = null;
            byte[] stringBytes;
            int length = ReadInt32();
            stringBytes = new byte[length];
            NetworkStream.Read(stringBytes, 0, length);
            s = Encoding.ASCII.GetString(stringBytes);
            return s;
        }

        public string ReadPacket()
        {
            if(Process.GetCurrentProcess().PrivateMemorySize64 / (1024 * 1024) > 1000)
            {
                Environment.Exit(0);
            }
            string s;
            byte[] stringBytes;
            int length = ReadInt32();
            stringBytes = new byte[length];
            NetworkStream.Read(stringBytes, 0, length);
            s = Encoding.ASCII.GetString(stringBytes);
            Debug.WriteLine($"===");
            Debug.WriteLine($"{length}");
            Debug.WriteLine($"{s.Length}");
            Debug.WriteLine($"{s}");
            Debug.WriteLine($"===");
            return s;
        }

        int ReadPacketInt()
        {
            int i = 0;
            return i;
        }
    }
}
