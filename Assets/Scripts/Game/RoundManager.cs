using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoundManager : MonoBehaviour
{
    private static RoundManager _instance;

    public static RoundManager Instance { get { return _instance; } }

    private BitArray players;

    [SerializeField]
    private GameObject gameOver;

    public static int PLAYER_1 = 0;
    public static int PLAYER_2 = 1;
    public static int PLAYER_3 = 2;
    public static int PLAYER_4 = 3;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        players = new BitArray(4);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setPlayer(int player, bool willPlay)
    {
        if (player >= players.Count || player < 0) {
            throw new InvalidPlayerException("Invalid player was selected");
        }

        players.Set(player, willPlay);
    }

    public bool isPlayerSelected(int player)
    {
        if (players.Count >= player || player < 0) {
            throw new InvalidPlayerException("Invalid player was selected");
        }

        return players.Get(player);
    }

    internal void notifyAboutDeath(int playerId)
    {
        this.setPlayer(playerId, false);
        checkForGameOver();
    }

    private void checkForGameOver()
    {
        var playersList = players.Cast<bool>();
        if (playersList.Count(player => player) == 1)
        {
            var overScreen = Instantiate(gameOver);
            overScreen.transform.SetParent(FindObjectOfType<Canvas>().transform, false);
            // TODO: Instantiate instead of setting as active
            // gameOver.SetActive(true);
            var playerIdx = playersList
                .Select((p, index) => (p, index))
                .First((tuple) => tuple.p).index;
            overScreen.gameObject.GetComponent<GameOver>().SetWinText(playerIdx);
        }
    }

    public bool isAnyPlayerSelected()
    {
        return players.Cast<bool>().Contains(true);
    }

    public List<int> getPlayersMap()
    {
        return players
            .Cast<bool>()
            .Select((player, index) => (player ? index : -1))
            .Where(player => player != -1)
            .ToList();
    }

    public int playingPlayersCount()
    {
        return players
            .Cast<bool>()
            .Count(p => p);
    }
}

public class InvalidPlayerException : System.Exception
{
    public InvalidPlayerException()
    {
    }

    public InvalidPlayerException(string message)
        : base(message)
    {
    }

    public InvalidPlayerException(string message, System.Exception inner)
        : base(message, inner)
    {
    }
}