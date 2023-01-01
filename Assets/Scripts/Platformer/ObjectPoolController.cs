using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class ObjectPoolController : MonoBehaviour
{
    [SerializeField] private List<GameObject> poolList;
    private Queue<GameObject> pool;

    private void Start()
    {
        foreach (var pObject in poolList)
        {
            pool.Enqueue(pObject);
        }
    }


    private GameObject Dequeue()
    {
        return pool.Count == 0 ? null : pool.Dequeue();
    }

    public void Enqueue(GameObject poolObject)
    {
        pool?.Enqueue(poolObject);
    }
}
