using Networking.Common;
using Networking.Packets;
using UI;

namespace Networking.ClientLogin
{
    public abstract class LoginClientSend
    {
        private static void SendTcpData(Packet packet)
        {
            packet.WriteLength();
            LoginClient.Instance.TcpConn.SendData(packet);
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

        public static void EnterLobby()
        {
            using var packet = new Packet((int)ClientToMaster.LobbyRequest);
            SendTcpData(packet);
        }
        
        #endregion
    }
}