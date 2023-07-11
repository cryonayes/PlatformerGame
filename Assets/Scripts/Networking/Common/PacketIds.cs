namespace Networking.Common
{
    // Sent from server to client.
    public enum MasterToClient
    {
        Welcome = 1,
        LoginSuccess,
        LoginFailed,
        GoJoinLobby,
    }

// Sent from client to server.
    public enum ClientToMaster
    {
        Login = 1,
        LobbyRequest
    }

    public enum GameServerToClient
    {
        Welcome = 1,
    }
    
    public enum ClientToGameServer
    {
        WelcomeReceived = 1,
        PlayerMove,
    }
}