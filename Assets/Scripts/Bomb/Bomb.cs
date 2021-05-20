using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface BombPublisher {
    public void AddSubscriber(BombObserver observer);
    public void Notify();
}

public class Bomb : MonoBehaviour, BombPublisher
{
    private Transform bombTransform;
    private Rigidbody bombRigidBody;

    [SerializeField]
    private float timeUntilExplode;
    [SerializeField]
    private GameObject explosion;
    [SerializeField]
    private LayerMask levelMask;
    private bool isExploded;
    private List<BombObserver> observers;
    private bool firstTime = true;
    private ObjectPool explosionsPooler;

    void OnEnable()
    {
        explosionsPooler = ObjectPool.Instance;
        isExploded = false;
        bombTransform = transform;
        bombRigidBody = gameObject.GetComponent<Rigidbody>();
        observers = new List<BombObserver>();
        initiateExplosionIfAvailable();
    }

    private void initiateExplosionIfAvailable()
    {
        if (!firstTime)
        {
            Invoke("Explode", timeUntilExplode);
        }
        firstTime = false;
    }

    void Update()
    {
    }

    void Explode()
    {
        Notify();
        isExploded = true;
        Vector3 pos = new Vector3(Mathf.RoundToInt(bombTransform.position.x), bombTransform.position.y, Mathf.RoundToInt(bombTransform.position.z));
        explosionsPooler.GetAndPlacePooledObject(explosion.tag, pos, explosion.transform.rotation);
        CreateExplosions(pos, Vector3.forward);
        CreateExplosions(pos, Vector3.right);
        CreateExplosions(pos, Vector3.back);
        CreateExplosions(pos, Vector3.left);
        selfDestruct();
    }

    private void selfDestruct()
    {
        gameObject.SetActive(false);
    }

    private void CreateExplosions(Vector3 pos, Vector3 direction) 
    {
        for (int i = 1; i < 3; i++) 
        {
            RaycastHit hit;
            int layerMask = (~(1 << explosion.layer)); // & ~(1 << gameObject.layer)
            bool isHit = Physics.Raycast(pos, direction, out hit, i, layerMask);

            bool shouldStop = false;
            if (isHit)
            {
                shouldStop = handleHit(hit);
            }
            
            explosionsPooler.GetAndPlacePooledObject(explosion.tag, pos + (i * direction),
                explosion.transform.rotation);

            if (shouldStop)
            {
                break;
            }
        }
    }

    private bool handleHit(RaycastHit hit)
    {
        bool shouldTerminateExplosion = false;

        if (hit.transform.gameObject.CompareTag("Wall"))
        {
            shouldTerminateExplosion = true;
        }

        if (hit.transform.gameObject.CompareTag("Bomb"))
        {
            hit.transform.gameObject.SendMessage("chainExplosionsIfAvailable");
            shouldTerminateExplosion = true;
        }

        if(hit.transform.gameObject.CompareTag("Player"))
        {
            hit.transform.gameObject.GetComponent<IDestructable>().HandleDestruction();
        }
        
        if (hit.transform.gameObject.CompareTag("Destructable"))
        {
            hit.transform.gameObject.GetComponent<IDestructable>().HandleDestruction();
            shouldTerminateExplosion = true;
        }

        return shouldTerminateExplosion;
    }

    public void chainExplosionsIfAvailable()
    {
        if (!isExploded) {
            CancelInvoke("Explode");
            CancelInvoke("selfDestruct");
            Explode();
        }
    }

    public void AddSubscriber(BombObserver observer)
    {
        observers.Add(observer);
    }

    public void Notify()
    {
        observers.ForEach(action => action.OnNotify(gameObject.GetInstanceID()));
    }

    public bool isBombExploded()
    {
        return isExploded;
    }
}
