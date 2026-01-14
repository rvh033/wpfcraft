using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Security.Policy;

namespace wpfcraft
{
    internal class Client
    {
        public Client(TcpClient client, Server server)
        {
            this.TcpClient = client;
            this.Server = server;
            PReader = new PReader(this.TcpClient.GetStream());
            byte t = PReader.ReadByte();
            string packet = PReader.ReadPacket();
            string[] packetContent = packet.Split('-');
            string s = packetContent[1];
            string[] split = packetContent[1].Split(':');
            this.Name = split[0];
            this.Id = Convert.ToUInt64(split[1]);
            Debug.WriteLine($"OUTPUT FROM CLIENT.CS {this.Name} {this.Id}");
        }

        public string Name;
        public double X;
        public double Y;
        public ulong Id;
        public bool IsInit = false;
        public TcpClient TcpClient;
        public PReader PReader;
        public Server Server;

        public void Init()
        {
            this.IsInit = true;
            if (Id != 0)
            {
                Task.Run(() => this.ReadPackets());
            }
            else
            {
                Task.Run(() => this.ReadPacketsSelf());
            }
        }

        void ReadPackets()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    byte t = PReader.ReadByte();
                    Debug.WriteLine($"Packet type: {t}");
                    string packet = PReader.ReadPacket();
                    string[] packetContent = packet.Split('-');
                    Debug.WriteLine($"Packet content: {packet}");
                    int type = Convert.ToInt32(packetContent[0]);
                    switch (type)
                    {
                        case 0:
                            Debug.WriteLine("A connection was made");
                            string s = packetContent[1];
                            string[] split = packetContent[1].Split(':');
                            string name = split[0];
                            ulong id = Convert.ToUInt64(split[1]);
                            //this.name = name;
                            //this.id = id;
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                        case 100:
                            string posPacket = packetContent[1];
                            this.Server.SendPosUpdated(posPacket);
                            break;
                    }
                }
            });
        }

        void ReadPacketsSelf()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    byte t = PReader.ReadByte();
                    Debug.WriteLine($"Packet type: {t}");
                    string packet = PReader.ReadPacket();
                    string[] packetContent = packet.Split('-');
                    Debug.WriteLine($"Packet content: {packet}");
                    int type = Convert.ToInt32(packetContent[0]);
                    switch (type)
                    {
                        case 0:
                            Debug.WriteLine("A connection was made");
                            string s = packetContent[1];
                            string[] split = packetContent[1].Split(':');
                            string name = split[0];
                            ulong id = Convert.ToUInt64(split[1]);
                            //Client client = new Client(name, id, this.server.listener.AcceptTcpClient(), this.server);
                            //if (!client.isInit)
                            //{
                            //    client.init(name, id);
                            //}
                            //this.server.players.Add(client);
                            //this.server.sendConnection($"{name}:{id}");
                            break;
                        case 1:
                            break;
                        case 2:
                            break;
                    }
                }
            });
        }

        public void SetPos(double x, double y)
        {
            X = x;
            Y = y;
        }

        public void SetX(double x)
        {
            X = x;
        }

        public void SetY(double y)
        {
            Y = y;
        }
    }
}
