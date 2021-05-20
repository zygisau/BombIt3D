using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerSpawner : MonoBehaviour
{
    private Transform[] playerSpawns;
    private RoundManager roundManager;

    [SerializeField]
    private GameObject playerPrefab;
    // Start is called before the first frame update
    void Start()
    {
        playerSpawns = gameObject.GetComponentsInChildren<Transform>().Skip(1).ToArray();
        roundManager = RoundManager.Instance;
        spawnPlayers();
    }

    private void spawnPlayers()
    {
        var players = roundManager.getPlayersMap();
        foreach (var playerIdx in players)
        {
            trySpawn(playerIdx);
        }
        despawnPoints();
    }

    private void despawnPoints()
    {
        foreach (var point in playerSpawns)
        {
            point.gameObject.SetActive(false);
        }
    }

    private void trySpawn(int playerIdx)
    {
        try 
        {
            var player = Instantiate(playerPrefab, playerSpawns[playerIdx].position, Quaternion.identity);
            playerSpawns[playerIdx].gameObject.SetActive(false);
            player.GetComponent<Player>().AssignPlayerProperties(playerIdx);
        }
        catch
        {
            // go to menu
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
