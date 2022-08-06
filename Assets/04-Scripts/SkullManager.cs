using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullManager : MonoBehaviour
{
    public static SkullManager Instance;

    [SerializeField] GameObject skullPrefab;

    [Space (15)]
    public int poolAmount;
    public List<GameObject> pooledSkullsList;
    private Rigidbody rb;

    private void Awake()
    {
        Instance = this;

        PoolSkulls(skullPrefab, pooledSkullsList, poolAmount, transform);
    }

    //
    // POOL SKULLS THAT THE PLAYER HOLDS - Called once in Awake to begin the Pool
    public void PoolSkulls(GameObject skullPrefab, List<GameObject> pooledSkulls, int poolAmount, Transform skullParent)
    {
        GameObject skullHeld;

        //Pool the amount of targets needed and hold them in a list.
        while (pooledSkulls.Count -1 < poolAmount)
        {
            skullHeld = Instantiate(skullPrefab, skullParent, instantiateInWorldSpace: false) as GameObject; //Instantiate the skull
            skullHeld.SetActive(false); //Hide the instantiated skull
            skullHeld.transform.parent = skullParent; //Parent the skulls to the skullParent transform
            pooledSkulls.Add(skullHeld); //Add the gameobject to the pooledSkulls list

            //Turn off gravity and collider for the skulls in inventory
            rb = skullHeld.GetComponent<Rigidbody>();
            Collider collider = skullHeld.GetComponent<Collider>();

            collider.enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
        }
    }

}
