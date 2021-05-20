using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public List<PlayerControl> players;

    public string ToJson()
    {
        return JsonConvert.SerializeObject(this);
    }

    public void LoadFromJson(string json)
    {
        JsonConvert.PopulateObject(json, this);
    }

    public void LoadDefaults()
    {
        var player1Defaults = new PlayerControl
        {
            Up = KeyCode.W,
            Down = KeyCode.S,
            Left = KeyCode.A,
            Right = KeyCode.D,
            Fire = KeyCode.Space
        };
        var player2Defaults = new PlayerControl
        {
            Up = KeyCode.T,
            Down = KeyCode.G,
            Left = KeyCode.F,
            Right = KeyCode.H,
            Fire = KeyCode.Comma
        };
        var player3Defaults = new PlayerControl
        {
            Up = KeyCode.I,
            Down = KeyCode.K,
            Left = KeyCode.J,
            Right = KeyCode.L,
            Fire = KeyCode.Slash
        };
        var player4Defaults = new PlayerControl
        {
            Up = KeyCode.UpArrow,
            Down = KeyCode.DownArrow,
            Left = KeyCode.LeftArrow,
            Right = KeyCode.RightArrow,
            Fire = KeyCode.Keypad1
        };

        players = new List<PlayerControl>
        {
            player1Defaults,
            player2Defaults,
            player3Defaults,
            player4Defaults
        };
    }
}

public interface ISaveable
{
    void PopulateSaveData(SaveData saveData);
    void LoadFromSaveData();
}