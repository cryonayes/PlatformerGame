using System;
using System.Net;
using Networking.Packets;

namespace Networking.GameServer
{
    public class GameClientHandle
    {
        public static void Welcome(Packet packet)
        {
            Console.WriteLine("GameServer welcome received!");
            GameClient.Instance._myId = packet.ReadInt();
            GameClient.Instance.UdpConn.Connect(((IPEndPoint)GameClient.Instance.TcpConn.TcpSocket.Client.LocalEndPoint).Port);
            GameServerSend.WelcomeResponse();
        }
    }
}