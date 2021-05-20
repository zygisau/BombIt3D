using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private static ObjectPool pool;
    public static ObjectPool Instance { get { return pool; } }

    [SerializeField]
    private Dictionary<string, List<GameObject>> pooledObjects;

    [SerializeField]
    private List<PoolElement> objectsToPool;

    private void Awake()
    {
        if (pool != null && pool != this)
        {
            Destroy(this.gameObject);
        } else {
            pool = this;
        }
    }

    private void Start()
    {
        pooledObjects = new Dictionary<string, List<GameObject>>();
        populatePools();
    }

    private void populatePools()
    {
        foreach (var prefab in objectsToPool)
        {
            List<GameObject> prefabPool = getPoolByPrefab(prefab.element.tag);
            populatePool(prefabPool, prefab);
        }
    }

    private void populatePool(List<GameObject> prefabPool, PoolElement prefab)
    {
        for (int i = 0; i < prefab.amountToPool; i++)
        {
            GameObject obj = (GameObject)Instantiate(prefab.element);
            obj.SetActive(false); 
            prefabPool.Add(obj);
        }
    }

    private List<GameObject> getPoolByPrefab(string prefabTag)
    {
        List<GameObject> prefabPool = null;
        if (!pooledObjects.TryGetValue(prefabTag, out prefabPool))
        {
            pooledObjects[prefabTag] = new List<GameObject>();
            prefabPool = pooledObjects[prefabTag];
        }
        return prefabPool;
    }

    public GameObject GetAndPlacePooledObject(string tag, Vector3 position, Quaternion rotation)
    {
        var item = GetPooledObject(tag);
        if (item)
        {
            placeItemInPosition(item, position, rotation);
        }

        return item;
    }

    public GameObject GetPooledObject(string tag)
    {
        List<GameObject> objectPool = null;
        pooledObjects.TryGetValue(tag, out objectPool);
        return objectPool.Find(obj => !obj.activeInHierarchy);
    }

    private void placeItemInPosition(GameObject item, Vector3 position, Quaternion rotation)
    {
        item.transform.position = position;
        item.transform.rotation = rotation;
        item.SetActive(true);
    }
}
