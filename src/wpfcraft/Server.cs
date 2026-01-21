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
using System.Windows.Threading;
using wpfcraft.PlayerData;

namespace wpfcraft
{
    internal class Server
    {
        public Server(WPFCraft main, Player player, bool isInternal, string ip, int port)
        {
            this.Player = player;
            if (isInternal)
            {
                Players = new List<Client>();
                IP = ip;
                Port = port;
                StartListening(IP, Port);
            }
            else
            {
                this.Client = new TcpClient();
                this.Main = main;
                PlayersToClient = new List<string[]>();
            }
            Init();
        }

        WPFCraft Main;
        Player Player;
        public TcpClient Client;
        public TcpListener Listener;
        public PReader PReader;
        public List<Client> Players;
        public List<string[]> PlayersToClient;
        MemoryStream PBuilderMemoryStream;
        Random Rand = new Random();
        string IP;
        int Port;
        int Pvn = 1;
        public bool IsReady = true;

        void Init()
        {
            
        }

        public void ConnectToServer(string ip, int port)
        {
            if (!Client.Connected)
            {
                Debug.WriteLine(ip);
                Client.Connect(ip, port);
                string playername = Player.Name;
                PReader = new PReader(Client.GetStream());
                Client.Client.Send(BuildPacketConnecting(0, $"{playername}:{Player.Id}"));
                Task.Run(() => this.ReadPacketsForClient());
            }
        }

        byte[] BuildPacketString(byte type, string s)
        {
            string s1 = $"{type}-{s}";
            PBuilderMemoryStream = new MemoryStream();
            PBuilderMemoryStream.WriteByte(type);
            int length = s1.Length;
            PBuilderMemoryStream.Write(BitConverter.GetBytes(length));
            PBuilderMemoryStream.Write(Encoding.ASCII.GetBytes(s1));
            return PBuilderMemoryStream.ToArray();
        }

        byte[] BuildPacketConnecting(byte type, string s)
        {
            string s1 = $"{Pvn}/{type}/{s}";
            PBuilderMemoryStream = new MemoryStream();
            PBuilderMemoryStream.WriteByte(type);
            int length = s1.Length;
            PBuilderMemoryStream.Write(BitConverter.GetBytes(length));
            PBuilderMemoryStream.Write(Encoding.ASCII.GetBytes(s1));
            return PBuilderMemoryStream.ToArray();
        }

        void BuildPacketInt()
        {
        }

        void BuildPacketChunkData()
        {

        }

        public byte[] BuildPacketPlayerPos(byte type, string ixy)
        {
            string s = $"{Pvn}/{type}/{ixy}";
            PBuilderMemoryStream = new MemoryStream();
            PBuilderMemoryStream.WriteByte(type);
            int length = s.Length;
            PBuilderMemoryStream.Write(BitConverter.GetBytes(length));
            PBuilderMemoryStream.Write(Encoding.ASCII.GetBytes(s));
            return PBuilderMemoryStream.ToArray();
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
                    player.TcpClient.Client.Send(BuildPacketConnecting(0, content));
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
                    player.TcpClient.Client.Send(BuildPacketPlayerPos(100, content));
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

        void ReadPacketsForClient()
        {
            Task.Run(() =>
            {
                while (true)
                {
                    try
                    {
                        byte t = PReader.ReadByte();
                        Debug.WriteLine($"Packet type: {t}");
                        string packet = PReader.ReadPacket();
                        int pvn = Convert.ToInt32(packet.Split('/')[0]);
                        if (pvn != Pvn)
                        {
                            Client.Close();
                        }
                        string[] packetContent = packet.Split('/');
                        Debug.WriteLine($"Packet content: {packet}");
                        int type = Convert.ToInt32(packetContent[1]);
                        switch (type)
                        {
                            case 0:
                                Debug.WriteLine("Connection was made");
                                string s = packetContent[2];
                                string[] split = packetContent[2].Split(':');
                                string name = split[0];
                                ulong id = Convert.ToUInt64(split[1]);
                                string[] playerData = new string[4];
                                playerData[0] = name;
                                playerData[1] = id.ToString();
                                playerData[2] = "0";
                                playerData[3] = "0";
                                this.PlayersToClient.Add(playerData);
                                break;
                            case 1:
                                break;
                            case 2:
                                break;
                            case 100:
                                string posPacket = packetContent[2];
                                string[] posPacketSplit = packetContent[2].Split(':');
                                ulong pid = Convert.ToUInt64(posPacketSplit[0]);
                                double x = Convert.ToDouble(posPacketSplit[1]);
                                double y = Convert.ToDouble(posPacketSplit[2]);
                                this.Main.Dispatcher.Invoke(() =>
                                {
                                    this.Main.PlayerMPPosUpdated(pid, x, y);
                                });
                                break;
                        }
                    }
                    catch (Exception ex)
                    {
                        Client.Close();
                        IsReady = false;
                        Debug.WriteLine($"The connection has been terminated\n{ex.Message}");
                    }
                }
            });
        }
    }
}
