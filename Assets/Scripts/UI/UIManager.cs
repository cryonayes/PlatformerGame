using Networking.MasterServer;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class UIManager : MonoBehaviour
    {
        public static UIManager instance;

        public GameObject loginScreen;
        public GameObject lobbyScreen;
        
        public InputField usernameField;
        public InputField passwordField;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);
        }

        private void Start()
        {
            MasterClient.Instance.ConnectToServer();
        }
        
        public void LoginButton()
        {
            MasterClientSend.Login();
        }
        
        public void RegisterButton()
        {
            MasterClientSend.Register();
        }
        
        public void LoginSuccess()
        {
            loginScreen.SetActive(false);
            lobbyScreen.SetActive(true);
            Debug.Log($"Login Success. GameServerIP: {Global.GameServerIp} Port: {Global.GameServerPort.ToString()}");
        }

        public void RegisterSuccess()
        {
            // Register success yaz
        }

        public void RegisterFailed()
        {
            // Register failed yaz
        }

        public void LobbyButton()
        {
            MasterClientSend.EnterLobby();
        }
        
    }
}