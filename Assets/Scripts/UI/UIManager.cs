using Networking.ClientLogin;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
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
        public InputField lobbyNameField;

        private void Awake()
        {
            if (instance == null)
                instance = this;
            else if (instance != this)
                Destroy(this);
        }

        private void Start()
        {
            LoginClient.Instance.ConnectToServer();
        }
        
        public void LoginButton()
        {
            LoginClientSend.Login();
        }

        public void LoginSuccess()
        {
            loginScreen.SetActive(false);
            lobbyScreen.SetActive(true);
            Debug.Log($"Login Success. GameServerIP: {Global.GameServerIp} Port: {Global.GameServerPort.ToString()}");
        }

        public void LobbyButton()
        {
            LoginClientSend.EnterLobby();
        }
    }
}