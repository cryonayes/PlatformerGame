namespace Networking.Common
{
    public enum MasterToClient
    {
        Welcome = 1,
        LoginSuccess,
        LoginFailed,
        RegisterSuccess,
        RegisterFailed,
        GoJoinLobby,
        ScoreTable
    }

    public enum ClientToMaster
    {
        Login = 10,
        Register,
        LobbyRequest,
        OnFinishLine,
    }

    public enum GameServerToClient
    {
        Welcome = 20,
        AddPlayers,
        PlayerMove
    }
    
    public enum ClientToGameServer
    {
        JoinLobby = 30,
        PlayerMove,
    }
    
    public enum GameServerToMaster
    {
        Welcome = 40,
        LobbyReady
    }

    public enum MasterToGameServer
    {
        LobbyInfo = 50
    }
}