
using System.Numerics;

public static class Constants
{
    public const int TicksPerSec = 10;
    public const float MsPerTick = 1000f / TicksPerSec;
}

public static class Global
{
    public const string WALK_ANIM = "Walk";

    public static string Username = "";
    public static string Token = "";
    public static string GameServerIp = "";
    public static int GameServerPort = 0;
    public static string LobbyIdToJoin = "";
}