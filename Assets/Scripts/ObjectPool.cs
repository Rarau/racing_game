using UnityEngine;
using System.Collections.Generic;
using System.Collections;


/// <summary>
/// A simple class for pooling game objects based on their active state in the hierarchy.
/// </summary>
public class ObjectPool : MonoBehaviour 
{
    public GameObject objectPrefab;
    public int initialSize = 10;
    public bool autoGrow = false;

    private List<GameObject> objectPool;

	void Start () 
    {
        if (objectPrefab == null)
            return;
        Initialize();
	}
	
    public void Initialize()
    {
        objectPool = new List<GameObject>();

        for (int i = 0; i < initialSize; i++)
        {
            GameObject newObj = (GameObject)Instantiate(objectPrefab);
            newObj.SetActive(false);
            objectPool.Add(newObj);
        }
    }

    public void DisableAll()
    {
        for (int i = 0; i < objectPool.Count; i++)
        {
            objectPool[i].SetActive(false);
        }
    }

    public GameObject Get()
    {
        for(int i = 0; i < objectPool.Count; i++)
        {
            if(!objectPool[i].activeInHierarchy)
            {
                return objectPool[i];
            }
        }

        if(autoGrow)
        {
            GameObject newObj = (GameObject)Instantiate(objectPrefab);
            newObj.SetActive(false);
            objectPool.Add(newObj);
            return newObj;
        }

        return null;
    }
}
