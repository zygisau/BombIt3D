using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour, IDestructable
{
    private HealthManager healthManager;
    [SerializeField]
    private float invincibilityTime;
    private int playerId;
    private bool invincible;
    private Renderer rend;
    private RoundManager roundManager;
    // Start is called before the first frame update
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.enabled = true;
        invincible = false;
        healthManager = HealthManager.Instance;
        roundManager = RoundManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void AssignPlayerProperties(int playerId)
    {
        this.playerId = playerId;
    }

    public int GetId()
    {
        return playerId;
    }

    public void HandleDestruction()
    {
        if (!invincible)
        {
            healthManager.SetHealth(this.playerId, healthManager.GetHealth(this.playerId) - 1);
            invincible = true;
            if (healthManager.GetHealth(this.playerId) == 0)
            {
                roundManager.notifyAboutDeath(this.playerId);
                gameObject.SetActive(false);
            }
            else
            {
                StartCoroutine(Flash());
                Invoke("resetInvulnerability", invincibilityTime);   
            }
        }
    }

    private IEnumerator Flash()
    {
        float elapsedTime = 0f;
        float waitTime = 0.2f;

        while(elapsedTime <= invincibilityTime)
        {
            rend.enabled = !rend.enabled;
            elapsedTime += Time.deltaTime + waitTime;
            yield return new WaitForSeconds(waitTime);
        }
    }

    private void resetInvulnerability()
    {
        StopCoroutine(Flash());
        rend.enabled = true;
        invincible = false;
    }
}
