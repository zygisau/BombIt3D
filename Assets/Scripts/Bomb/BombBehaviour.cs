using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBehaviour : MonoBehaviour
{
    private List<int> ignoreCollisionList;

    [SerializeField]
    private LayerMask playerMask;
    private CapsuleCollider bombCollider;

    void Awake()
    {
        ignoreCollisionList = new List<int>();
        bombCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    void OnEnable()
    {
        resetState();
        registerCollisions();
    }

    private void registerCollisions()
    {
        Collider[] hitColliders = Physics.OverlapSphere(gameObject.transform.position, 0.5f, playerMask);
        foreach (var hitCollider in hitColliders)
        {
            ignoreCollisionList.Add(hitCollider.gameObject.GetInstanceID());
            setCollision(hitCollider.gameObject, false);
        }
    }

    private void setCollision(GameObject instance, bool collide = false)
    {
        Physics.IgnoreCollision(instance.GetComponent<CharacterController>().GetComponent<Collider>(), bombCollider, !collide);
    }

    private void resetState()
    {
        ignoreCollisionList.Clear();
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && ignoreCollisionList.Contains(other.gameObject.GetInstanceID()))
        {
            setCollision(other.gameObject, true);
            deregisterCollision(other.gameObject.GetInstanceID());
        }
    }

    private void deregisterCollision(int id)
    {
        ignoreCollisionList.Remove(id);
    }

    void Update()
    {
        
    }

    public bool isBombPushable()
    {
        return ignoreCollisionList.Count == 0;
    }
}
