using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace wpfcraftserver.Packet
{
    internal class PacketBuilder
    {

        static MemoryStream Stream;

        public static byte[] BuildPacketConnecting(byte type, string s)
        {
            string s1 = $"{Server.Pvn}/{type}/{s}";
            Stream = new MemoryStream();
            Stream.WriteByte(type);
            int length = s1.Length;
            Stream.Write(BitConverter.GetBytes(length));
            Stream.Write(Encoding.ASCII.GetBytes(s1));
            return Stream.ToArray();
        }

        public static byte[] BuildPacketPlayerPos(byte type, string ixy)
        {
            string s = $"{Server.Pvn}/{type}/{ixy}";
            Stream = new MemoryStream();
            Stream.WriteByte(type);
            int length = s.Length;
            Stream.Write(BitConverter.GetBytes(length));
            Stream.Write(Encoding.ASCII.GetBytes(s));
            return Stream.ToArray();
        }
    }
}
