using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour
{
    private SceneChanger sceneChanger;
    private TMPro.TextMeshProUGUI winText;
    void Awake()
    {
        sceneChanger = gameObject.GetComponent<SceneChanger>();
        winText = gameObject.GetComponentInChildren<TMPro.TextMeshProUGUI>(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            sceneChanger.ChangeScene("StartGame");
        }
    }

    public void SetWinText(int player)
    {
        winText.text = $"Player {player} WINS";
    }
}
