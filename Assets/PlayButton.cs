using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayButton : MonoBehaviour
{
    private const int MINIMUM_PLAYERS_COUNT = 2;
    private Button button;
    private RoundManager roundManager;


    // Start is called before the first frame update
    void Start()
    {
        roundManager = RoundManager.Instance;
        button = gameObject.GetComponent<Button>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        button.enabled = roundManager.playingPlayersCount() >= MINIMUM_PLAYERS_COUNT;
    }
}
