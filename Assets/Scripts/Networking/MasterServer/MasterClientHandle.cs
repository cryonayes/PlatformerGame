using System.Collections.Generic;
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
        
        public static void RegisterSuccess(Packet packet)
        {
            UIManager.instance.RegisterSuccess();
        }

        public static void RegisterFailed(Packet packet)
        {
            UIManager.instance.RegisterFailed();
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

        public static void ScoreTable(Packet packet)
        {
            var scoreList = new List<(string player, int score)>();
            var userCount = packet.ReadInt();
            
            for (var i = 0; i < userCount; i++)
                scoreList.Add((packet.ReadString(), packet.ReadInt()));
            GameManager.Instance.HandleScoreTable(scoreList);
        }
    }
}