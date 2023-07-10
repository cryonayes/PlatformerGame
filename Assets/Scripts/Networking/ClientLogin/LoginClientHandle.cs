using Networking.Packets;
using UI;
using UnityEngine;

namespace Networking.ClientLogin
{
    public class LoginClientHandle
    {
        public static void Welcome(Packet packet)
        {
            var msg = packet.ReadString();
            Debug.Log($"Message from server: {msg}");
        }

        public static void LoginSuccess(Packet packet)
        {
            Global.Username = packet.ReadString();
            Global.Token = packet.ReadString();
            Global.GameServerIp = packet.ReadString();
            Global.GameServerPort = packet.ReadInt();            
            
            UIManager.instance.LoginSuccess();
        }
        public static void LoginFail(Packet packet)
        {
            Debug.Log("Login failed");
        }
        
        public static void GoJoinLobby(Packet packet)
        {
            Debug.Log("Let's play");
        }
        
    }
}