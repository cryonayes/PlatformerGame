using Networking.Packets;

namespace Networking.GameServer
{
    public class GameClientHandle
    {
        public static void Welcome(Packet packet)
        {
            GameClient.Instance.myId = packet.ReadInt();
            GameServerSend.JoinLobbyRequest(Global.LobbyIdToJoin);
        }

        public static void AddPlayersToGame(Packet packet)
        {
            // Lobby'de olması gereken oyuncuları spawn et.
            var playerID = packet.ReadInt();
            var playerPos = packet.ReadVector3();
            
            GameManager.Instance.SpawnPlayers(playerID, playerPos);
        }

        public static void HandlePlayerMove(Packet packet)
        {
            var movingID = packet.ReadInt();
            var moveVec = packet.ReadVector3();

            GameManager.players[movingID].transform.position = moveVec;
        }
    }
}