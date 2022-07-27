using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    /*
     * -This script creates a system of object pooling that allows multiple different types of objects to be pooled under a single script.
     * -----HOW TO USE-----
     * -Pools can be created in the inspector for the object this script is attached to.
     * -Use SpawnFromPool() to create objects as needed through code.
     * -Create a reference to this script from any other script using ObjectPooler.Instance
     * 
     * Grant Hargraves 7/2022 (Full credit to Brackeys for providing a tutorial on object pooling)
     */

    [System.Serializable] //allows the following to show up in inspector
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }
    #region Singleton
    public static ObjectPooler PoolInstance;

    private void Awake()
    {
        PoolInstance = this;
    }
    #endregion

    public List<Pool> pools;
    public Dictionary<string, Queue<GameObject>> poolDictionary; //create a type of dictionary

    void Start()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>(); //create an instance of pooldictionary

        foreach(Pool pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>(); //create a new queue to fill up each pool with prefabs

            for(int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab); //instantiate all the gameobjects needed to fill up the pool
                obj.SetActive(false); //sets the object inactive so it isn't visible
                objectPool.Enqueue(obj); //put the object we just created into the queue
            }
            poolDictionary.Add(pool.tag, objectPool);
        }
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if(! poolDictionary.ContainsKey(tag)) //if a tag is called that doesn't exist...
        {
            Debug.LogWarning("Pool with tag " + tag + "doesn't exist."); //give a warning and exit the function
            return null;
        }

        GameObject objectToSpawn = poolDictionary[tag].Dequeue(); //pull a gameobject from the queue so we know it was used
        //-----All below just sets the object active and puts it in the right place-----
        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        Debug.Log("Object Spawned");
        //-----Below activates the OnObjectSpawn method if the object is applicable-----
        IPooledObject pooledObj = objectToSpawn.GetComponent<IPooledObject>(); //check to see if the spawned object inherits from IPooledObject
        if(pooledObj != null) //if it does...
        {
            pooledObj.OnObjectSpawn(); //activate its on-spawn method
        }
        //-----
        poolDictionary[tag].Enqueue(objectToSpawn); //put the gameobject back into the queue so it can be immediately reused if needed
        return objectToSpawn;
    }
}
