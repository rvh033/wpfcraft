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
using wpfcraftserver.Packet;

namespace wpfcraftserver
{
    internal class Server
    {
        public Server(WPFCraftServer main, string ip, int port)
        {
            Players = new List<Client>();
            IP = ip;
            Port = port;
            StartListening(IP, Port);
            Init();
        }

        public TcpClient Client;
        public TcpListener Listener;
        public PacketReader PReader;
        public List<Client> Players;
        Random Rand = new Random();
        string IP;
        int Port;
        public static int Pvn = 1;

        void Init()
        {
            
        }

        void StartListening(string ip, int port)
        {
            this.Listener = new TcpListener(IPAddress.Parse(ip), port);
            this.Listener.Start();
            while (true)
            {
                Client client = new Client(this.Listener.AcceptTcpClient(), this);
                if (!client.IsInit)
                {
                    client.Init();
                }
                Players.Add(client);
                SendConnection($"{client.Name}:{client.Id}");
                Debug.WriteLine(Players.Count);
            }
        }

        public void SendConnection(string content)
        {
            string[] split = content.Split(':');
            ulong id = Convert.ToUInt64(split[1]);
            foreach(Client player in Players)
            {
                if (player.Id != id)
                {
                    player.TcpClient.Client.Send(PacketBuilder.BuildPacketConnecting(0, content));
                }
            }
        }

        public void SendPosUpdated(string content)
        {
            string[] split = content.Split(':');
            ulong id = Convert.ToUInt64(split[0]);
            foreach (Client player in Players)
            {
                if (player.Id != id)
                {
                    player.TcpClient.Client.Send(PacketBuilder.BuildPacketPlayerPos(100, content));
                }
            }
        }

        void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    string[] packetContent = PReader.ReadPacket().Split('-');
                    int type = Convert.ToInt32(packetContent[0]);
                    switch (type)
                    {
                        case 0:
                            Debug.WriteLine("Connection received");
                            string s = packetContent[1];
                            string[] split = packetContent[1].Split(':');
                            string name = split[0];
                            ulong id = Convert.ToUInt64(split[1]);
                            SendConnection(name);
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                    }
                }
            });
        }
    }
}
