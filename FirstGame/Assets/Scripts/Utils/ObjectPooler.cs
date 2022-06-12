using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    [System.Serializable]
    public class PoolItem
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public List<PoolItem> pools;

    public bool isDictionaryLoaded;

    #region Singleton
    public static ObjectPooler Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            if(Instance != this)
            {
                Destroy(gameObject);
            }
        }
        isDictionaryLoaded = false;
        GeneratePoolDictionary();

    }
    #endregion

    
    public Dictionary<string, Queue<GameObject>> poolDictionary;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void GeneratePoolDictionary()
    {
        if (isDictionaryLoaded)
            return;
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (PoolItem poolItem in pools)
        {
            Queue<GameObject> objectPools = new Queue<GameObject>();

            for (int i = 0; i < poolItem.size; i++)
            {
                GameObject obj = Instantiate(poolItem.prefab);
                obj.SetActive(false);
                objectPools.Enqueue(obj);
                obj.transform.parent = transform;
            }

            poolDictionary.Add(poolItem.tag, objectPools);
        }

        isDictionaryLoaded = true;
    }

    public GameObject SpawnFormPool(string tag, Transform stickObject, Vector3 offset, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            return null;
        }

        GameObject objToSpawn = poolDictionary[tag].Dequeue();

        objToSpawn.transform.localPosition = position;
        objToSpawn.transform.rotation = rotation;

        IObjectPooler objectPooler = objToSpawn.GetComponent<IObjectPooler>();

        if (objectPooler != null)
            objectPooler.OnObjectSpawn(stickObject, offset);

        poolDictionary[tag].Enqueue(objToSpawn);

        return objToSpawn;
    }

}