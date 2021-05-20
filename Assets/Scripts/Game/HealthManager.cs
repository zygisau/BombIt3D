using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    private static HealthManager _instance;

    public static HealthManager Instance { get { return _instance; } }

    [SerializeField]
    private List<HealthBar> playerHealth;
    [SerializeField]
    private int maxHealth;
    private RoundManager roundManager;
    
    // Start is called before the first frame update
    void Start()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
            
            roundManager = RoundManager.Instance;

            var players = roundManager.getPlayersMap();
            for (var i = 0; i < playerHealth.Count; i++)
            {
                var healthBar = playerHealth[i];
                if (players.Contains(i))
                {
                    healthBar.SetMaxHealth(maxHealth);
                }
                else
                {
                    healthBar.gameObject.SetActive(false);
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetHealth(int playerId, int health)
    {
        playerHealth[playerId].SetHealth(health);
    }

    public int GetHealth(int playerId)
    {
        return playerHealth[playerId].GetHealth();
    }
}
