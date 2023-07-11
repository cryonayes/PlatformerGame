using Networking.Common;
using Networking.Packets;
using UnityEngine;

namespace Networking.GameServer
{
    public class GameServerSend
    {
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            GameClient.Instance.TcpConn.SendData(packet);
        }
        
        private static void SendUdpData(Packet packet)
        {
            packet.WriteLength();
            GameClient.Instance.UdpConn?.SendData(packet);
        }

        public static void WelcomeResponse()
        {
            using var packet = new Packet((int)ClientToGameServer.WelcomeReceived);
            
            packet.Write(GameClient.Instance._myId);
            packet.Write(Global.Token);
            
            SendTcpData(packet);
        }

        public static void PlayerMove(Vector3 position)
        {
            using var packet = new Packet((int)ClientToGameServer.PlayerMove);
            
            packet.Write(position.x);
            packet.Write(position.y);
            packet.Write(position.z);
            
            SendUdpData(packet);
        }
    }
}