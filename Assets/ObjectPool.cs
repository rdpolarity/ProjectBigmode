using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool
{
    private Queue<GameObject> pool = new Queue<GameObject>();
    private GameObject prefab;

    // A dictionary to keep track of which prefab each GameObject came from
    private Dictionary<GameObject, GameObject> prefabLookup = new Dictionary<GameObject, GameObject>();

    public ObjectPool(GameObject prefab, int initialSize)
    {
        this.prefab = prefab;
        for (int i = 0; i < initialSize; i++)
        {
            CreateObject();
        }
    }

    private GameObject CreateObject()
    {
        var newObj = GameObject.Instantiate(prefab);
        newObj.SetActive(false);
        pool.Enqueue(newObj);
        // When created, associate this object with its prefab
        prefabLookup[newObj] = prefab;
        return newObj;
    }

    public GameObject Get()
    {
        if (pool.Count == 0)
        {
            CreateObject();
        }

        var obj = pool.Dequeue();
        obj.SetActive(true);
        // Ensure the prefab lookup is updated (useful if objects are destroyed accidentally)
        prefabLookup[obj] = prefab;
        return obj;
    }

    public void Return(GameObject obj)
    {
        obj.SetActive(false);
        pool.Enqueue(obj);
        // Remove the tracking reference
        prefabLookup.Remove(obj);
    }

    // Method to get the prefab for a specific GameObject
    public GameObject GetPrefab(GameObject obj)
    {
        if (prefabLookup.TryGetValue(obj, out GameObject prefab))
        {
            return prefab;
        }
        return null;
    }
}
