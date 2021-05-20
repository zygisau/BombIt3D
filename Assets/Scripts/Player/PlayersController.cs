using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class PlayersController : MonoBehaviour
{
    [SerializeField]
    private int playerIdx;
    private PlayerControl player;
    private Toggle toggle;
    private RoundManager roundManager;

    // Start is called before the first frame update
    void Start()
    {
        player = SettingsManager.Instance.GetPlayerControlByIdx(playerIdx);
        roundManager = RoundManager.Instance;
        toggle = gameObject.GetComponent<Toggle>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(player.Fire))
        {
            toggle.isOn = !toggle.isOn;
            roundManager.setPlayer(playerIdx, toggle.isOn);
        }
    }
}
