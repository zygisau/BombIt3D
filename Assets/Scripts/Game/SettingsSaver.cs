using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class SettingsSaver : MonoBehaviour, ISaveable
{
    private SaveData saveData;
    private string DATA_FILE_NAME = "game_settings.dat";

    void Awake()
    {
        string json = "";
        if (readFromFile(out json))
        {
            loadFromJson(json);    
        }
        else
        {
            loadDefaultData();
        }
    }

    private void loadFromJson(string json)
    {
        SaveData payload = new SaveData();
        payload.LoadFromJson(json);
        saveData = payload;    
    }

    private void loadDefaultData()
    {
        saveData = new SaveData();
        saveData.LoadDefaults();
        writeToFile(saveData.ToJson());
    }

    public void LoadFromSaveData()
    {
        string json = "";
        readFromFile(out json);
        loadFromJson(json);
    }

    public void SaveData()
    {
        SaveData payload = new SaveData();
        PopulateSaveData(payload);
        writeToFile(payload.ToJson());
    }

    private bool writeToFile(string content)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, DATA_FILE_NAME);
        try
        {
            File.WriteAllText(fullPath, content);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to write to {fullPath} with exception {e}");
        }
        return false;
    }

    private bool readFromFile(out string result)
    {
        var fullPath = Path.Combine(Application.persistentDataPath, DATA_FILE_NAME);
        try
        {
            result = File.ReadAllText(fullPath);
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to read from {fullPath} with exception {e}");
            result = "";
        }
        return false;
    }

    public void PopulateSaveData(SaveData payload)
    {
        payload.players = new List<PlayerControl>(saveData.players);
    }

    public List<PlayerControl> GetPlayersData()
    {
        return saveData.players;
    }
}
