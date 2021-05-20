using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager _instance;

    public static SettingsManager Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            settingsSaver = gameObject.GetComponent<SettingsSaver>();
        }
    }

    private SettingsSaver settingsSaver;

    public List<PlayerControl> GetPlayersControls()
    {
        return settingsSaver.GetPlayersData();
    }

    public PlayerControl GetPlayerControlByIdx(int idx)
    {
        return settingsSaver.GetPlayersData()[idx];
    }
}
