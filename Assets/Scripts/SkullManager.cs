using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullManager : MonoBehaviour
{
    public static SkullManager Instance;

    public GameObject skullPrefab;
    public int poolAmount;
    public Transform skullParent;
    public List<GameObject> pooledSkulls;
    [SerializeField] Rigidbody rb;

    private void Awake()
    {
        Instance = this; 
    }

    private void Start()
    {
        PoolSkulls(skullPrefab, pooledSkulls, poolAmount, skullParent);
    }

    private void Update()
    {
        //After skull is thrown, start the Coroutine that will return the thrown skull to inventory
        //if (skullPrefab.GetComponent<Head>().hasBeenThrown)
        //{
        //    StartCoroutine(skullPrefab.GetComponent<Head>().ReturnSkull());
        //}

        for (int i = 0; i < pooledSkulls.Count; i++)
        {
            GameObject returnSkull = pooledSkulls[i];
            bool canReturn = Head.Instance.hasBeenThrown;

            if (canReturn)
            {
                Debug.Log("Can Return");

                StartCoroutine(Head.Instance.ReturnSkull());
            }
        }
    }

    //
    // POOL SKULLS THAT THE PLAYER HOLDS
    public void PoolSkulls(GameObject skullPrefab, List<GameObject> pooledSkulls, int poolAmount, Transform skullParent)
    {
        GameObject skullHeld;

        //Pool the amount of targets needed and hold them in a list.
        while (pooledSkulls.Count < poolAmount)
        {
            skullHeld = Instantiate(skullPrefab, skullParent, instantiateInWorldSpace: false) as GameObject;
            skullHeld.SetActive(false);
            skullHeld.transform.parent = skullParent; //Set the targets inside this gameObject folder
            pooledSkulls.Add(skullHeld);
        }

        //Set kinematic and gravity for the held skulls
        GameObject[] heldSkulls = pooledSkulls.ToArray();

        foreach (GameObject skull in heldSkulls)
        {
            rb = skull.GetComponent<Rigidbody>();
            Collider collider = skull.GetComponent<Collider>();

            collider.enabled = false;
            rb.isKinematic = true;
            rb.useGravity = false;
            
        }
    }

}
