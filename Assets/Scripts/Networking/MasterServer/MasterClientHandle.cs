using Networking.Packets;
using UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Networking.MasterServer
{
    public class MasterClientHandle
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
            Global.LobbyIdToJoin = packet.ReadString();
            SceneManager.LoadScene("MainScene");
        }
    }
}