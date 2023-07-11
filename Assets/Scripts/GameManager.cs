using System;
using System.Collections.Generic;
using Camera;
using Networking.GameServer;
using Player;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public GameObject ClientManager;
    public GameObject PlayerPrefab;
    public CameraFollow Camera;

    public static GameManager Instance;
    public static Dictionary<int, PlayerManager> players = new();


    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else if (Instance != this)
            Destroy(this);
    }

    private void Start()
    {
        ClientManager.SetActive(true);
        while (!GameClient.Instance) {}

        Camera.player = Instantiate(PlayerPrefab, Vector3.zero, Quaternion.identity).transform;
    }
    
    
}