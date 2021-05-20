using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public interface BombObserver {
    public void OnNotify(int instanceId);
}

public class BombSpawner : MonoBehaviour, BombObserver
{
    [SerializeField]
    private int MAX_BOMBS_COUNT;
    [SerializeField]
    private GameObject bomb;

    private List<int> bombs;
    private CharacterController characterController;
    private Collider playerCollision;
    private ObjectPool bombsPooler;
    private Transform playerTransform;
    private PlayerControl player;


    void Start()
    {
        bombsPooler = ObjectPool.Instance;
        bombs = new List<int>();
        playerTransform = transform;
        characterController = gameObject.GetComponent<CharacterController>();
        playerCollision = characterController.GetComponent<Collider>();
        player = SettingsManager.Instance.GetPlayerControlByIdx(gameObject.GetComponent<Player>().GetId());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(player.Fire) && isDropBombAvailable())
        {
            dropBomb();
        }
    }

    private bool isDropBombAvailable()
    {
        bool enoughBombsLeft = (bombs.Count + 1) <= MAX_BOMBS_COUNT;
        return enoughBombsLeft && !isBombHere(getNewBombPosition());
    }

    private Vector3 getNewBombPosition()
    {
        int playerGround = (int)(transform.position.y - (playerCollision.bounds.center.y - playerCollision.bounds.extents.y));
        return new Vector3(Mathf.RoundToInt(playerTransform.position.x),
            playerGround - 0.5f, Mathf.RoundToInt(playerTransform.position.z));
    }

    private bool isBombHere(Vector3 position)
    {
        Collider[] colliders = Physics.OverlapSphere(position, 0.01f);
        return Array.Exists(colliders, collider => collider.CompareTag("Bomb"));
    }

    private void dropBomb()
    {
        var newBomb = bombsPooler.GetAndPlacePooledObject(bomb.tag, getNewBombPosition(), playerTransform.rotation);
        newBomb.SendMessage("AddSubscriber", this as BombObserver);
        bombs.Add(newBomb.GetInstanceID());
    }

    public void OnNotify(int instanceId)
    {
        bombs.RemoveAll(id => id == instanceId);
    }
}
