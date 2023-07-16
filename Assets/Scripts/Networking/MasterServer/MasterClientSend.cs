using Networking.Common;
using Networking.Packets;
using UI;

namespace Networking.MasterServer
{
    public abstract class MasterClientSend
    {
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            MasterClient.Instance.TcpConn.SendData(packet);
        }

        #region Packets
        
        public static void Login()
        {
            var username = UIManager.instance.usernameField.text;
            var password = UIManager.instance.passwordField.text;

            using var packet = new Packet((int)ClientToMaster.Login);
            packet.Write(username);
            packet.Write(password);
            
            SendTcpData(packet);
        }

        public static void Register()
        {
            var username = UIManager.instance.usernameField.text;
            var password = UIManager.instance.passwordField.text;

            using var packet = new Packet((int)ClientToMaster.Register);
            packet.Write(username);
            packet.Write(password);
            SendTcpData(packet);
        }

        public static void EnterLobby()
        {
            using var packet = new Packet((int)ClientToMaster.LobbyRequest);
            SendTcpData(packet);
        }
        
        public static void PlayerFinished()
        {
            using var packet = new Packet((int)ClientToMaster.OnFinishLine);
            packet.Write(Global.Token);
            SendTcpData(packet);
        }

        #endregion
    }
}