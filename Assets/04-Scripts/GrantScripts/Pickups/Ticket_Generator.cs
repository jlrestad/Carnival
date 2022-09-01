using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket_Generator : MonoBehaviour
{
    /*
     * -This script provides an easy way for breakables to produce tickets with only certain odds.
     * -Relies on object pooling, so an object pooler must be present in the scene.
     * -Should be attached to the breakable object or any other gameobject that produces pickups.
     * Grant Hargraves 7/2022
     */
    //==================================================
    //=========================|FIELDS|
    //==================================================
    [Header("STATS")]
    [Tooltip("The likelyhood in terms of 'one out of _' that this object will produce a pickup when broken.")]
    public int dropChance = 1;
    [Tooltip("The type of pickup that will be pulled from an object pool when a pickup is produced. Options are 'RedTicket' 'BlueTicket' and 'TicketString'.")]
    public string dropType = "RedTicket";

    [Header("INTERNAL/DEBUG")]
    [Tooltip("A reference to the objectpooler in the scene. Should be assigned automatically if one is present.")]
    public ObjectPooler myPooler;
    [Tooltip("The random number generated in an attempt to drop a pickup. Pickups are spawned if this number = 0.")]
    [SerializeField] int dropPicker = 0;
    //==================================================
    //=========================|BUILT-IN METHODS|
    //==================================================
    public void Start()
    {
         myPooler = ObjectPooler.PoolInstance;
        attemptDrop();
    }

    private void OnEnable()
    {
        attemptDrop();
    }
    //==================================================
    //=========================|CUSTOM METHODS|
    //==================================================
    public void attemptDrop()
    {
        dropPicker = Random.Range(0, dropChance);
        if(dropPicker == 0)
        {
            dropItem();
        }
    }
    
    //WARNING: Will throw errors if run at Start, since the Object Pooler has not been assigned yet.
    public void dropItem()
    {
        GameObject obj = myPooler.SpawnFromPool(dropType, transform.position, Quaternion.identity); //pull the required object from the pool and spawn it here.
        obj.transform.parent = null;
    }
}
