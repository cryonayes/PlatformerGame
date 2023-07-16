using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.WebSockets;
using Camera;
using Networking.GameServer;
using Player;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject ClientManager;
    public GameObject PlayerPrefab;
    public GameObject OtherPlayerPrefab;
    public TextMeshProUGUI Countdown;
    public GameObject ScoreTable;
    public GameObject UserScoreHolder;

    
    public CameraFollow Camera;

    public static GameManager Instance;
    public static Dictionary<int, GameObject> players = new();

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
    }

    public void SpawnPlayers(int id, Vector3 positions)
    {
        GameObject player;
        if (id == GameClient.Instance.myId)
        {
            player = Instantiate(PlayerPrefab, positions, Quaternion.identity);
            Camera.player = player.transform;
            StartCoroutine(nameof(StartGame), player);
        }
        else
            player = Instantiate(OtherPlayerPrefab, positions, Quaternion.identity);
        
        players.Add(id, player);
    }

    private IEnumerator StartGame(GameObject player)
    {
        var seconds = 3;
        while (seconds > 0)
        {
            Countdown.text = seconds.ToString();
            seconds--;
            yield return new WaitForSeconds(1);
        }

        Countdown.enabled = false;
        player.GetComponent<Movement>().enabled = true;
        yield return null;
    }

    public void HandleScoreTable(List<(string username, int score)> scores)
    {
        ScoreTable.SetActive(true);
        
        foreach (var score in scores)
        {
            var playerScore = Instantiate(UserScoreHolder, ScoreTable.transform);
            var username = playerScore.transform.Find("Username");
            var points = playerScore.transform.Find("Score");

            username.GetComponent<TextMeshProUGUI>().text = score.username;
            points.GetComponent<TextMeshProUGUI>().text = score.score.ToString();
        }
    }
}