using System.Collections.Generic;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public static Dictionary<int, PlayerManager> players = new();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    /*
    public void SpawnPlayer(int _id, string _username, Vector3 _position)
    {
        GameObject player;
        if (_id == Client.instance.myId)
            player = Instantiate(localPlayerPrefab, _position, Quaternion.identity);
        else
            player = Instantiate(playerPrefab, _position, Quaternion.identity);

        player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, player.GetComponent<PlayerManager>());
    }
    */
}