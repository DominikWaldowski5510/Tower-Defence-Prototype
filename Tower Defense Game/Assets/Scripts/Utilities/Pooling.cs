using System.Collections.Generic;
using UnityEngine;

//Class that creates objects that appear frequently such as bullets, enemies or towers
public class Pooling : MonoBehaviour
{
    [SerializeField] private GameObject pooledObject = null;           //Object that we plan to reuse many times
    [SerializeField] private int amountOfPooledObj = 10;             //How many copies of the pulled object we want at one time
    [SerializeField] private bool canGrow = true;                                //If the pooling can extend if it hits limit

    private List<GameObject> pooledObjects;

    //Creates pooled objects and adds them to a list so we can use them later
    private void Start()
    {
        pooledObjects = new List<GameObject>();
        for (int i = 0; i < amountOfPooledObj; i++)
        {
            GameObject go = Instantiate(pooledObject);
            go.SetActive(false);
            go.transform.SetParent(this.transform);
            pooledObjects.Add(go);
        }
    }

    public GameObject GetPooledObject()
    {
        for (int i = 0; i < pooledObjects.Count; i++)
        {
            if (!pooledObjects[i].activeInHierarchy)
            {
                return pooledObjects[i];
            }
        }
        if (canGrow)
        {
            GameObject go = Instantiate(pooledObject);
            pooledObjects.Add(go);
            go.transform.SetParent(this.transform);
            return go;
        }
        return null;
    }
}
