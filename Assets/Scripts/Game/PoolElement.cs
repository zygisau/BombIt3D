using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PoolElement
{
    public GameObject element
    {
        get { return _element; }
        set { _element = value; }
    }

    [SerializeField]
    private GameObject _element;

    public int amountToPool
    {
        get { return _amountToPool; }
        set { _amountToPool = value; }
    }

    [SerializeField]
    private int _amountToPool;
}
