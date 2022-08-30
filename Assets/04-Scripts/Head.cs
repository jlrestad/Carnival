using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public static Head Instance;

    BossHeart bossHeart;
    [SerializeField] GameObject player;
    [SerializeField] WeaponEquip WE;
    [SerializeField] ParticleSystem flameVFX;
    [SerializeField] Menu menu;
    [SerializeField] int throwSpeed = 5;
    [SerializeField] int pickUpRange;

    public float skullLifetime = 0.8f;
    Vector3 distanceToPlayer;

    MeshRenderer heartMR; //MeshRenderer of the Boss' heart.
    public Rigidbody rb;
    public Collider collider;
    public GameObject skull;
    public Transform skullParent;
    

    public bool canThrow;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        canThrow = true;

        menu = GameObject.Find("GameManager").GetComponent<Menu>();
        player = GameObject.FindGameObjectWithTag("Player");
        WE = FindObjectOfType<WeaponEquip>();
        collider = GetComponent<Collider>();
        flameVFX = GetComponentInChildren<ParticleSystem>();
        skullParent = WE.skullParent.transform;

        SetSkullWeapon();

        skull = WE.skull;

        //Get the rigidbody and collider of the 1st index of the pooled skull list.
        rb = skull.GetComponent<Rigidbody>();
        collider = skull.GetComponent<Collider>();
    }

    private void Update()
    {
        // For Controller:
        if (Input.GetAxis("RtTrigger") > 0)
        {
            canThrow = false;
        }
        else
        {
            canThrow = true;
        }

        //THROW SKULL
        if (Input.GetButtonDown("Fire1") && WE.holdingSkull || Input.GetAxis("RtTrigger") > 0 && WE.holdingSkull && canThrow)
        {
            //Debug.Log("this code is reachable");

            ThrowSkull();
            canThrow = false;
        }
    }

    //
    // SKULL TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        //For Casket Baskets game
        if (other.CompareTag("Goal"))
        {
            //If skull hits the bucket then hide it from the scene.
            //WE.skull.SetActive(false);
        }

        if (other.CompareTag("BossHeart"))
        {
            //Do damage
            bossHeart = other.GetComponent<BossHeart>();
            bossHeart.HitHeart();
        }
        else
        {
            return;
        }
    }

    public void SetSkullWeapon()
    {
        WE.currentWeapon = skullParent.gameObject; //Set current weapon
        WE.skull = skullParent.transform.GetChild(0).gameObject; //Set the skull that is held
        WE.skull.SetActive(true); //Make the skull visible

        WE.holdingSkull = true;
        //WE.inInventory = false;
    }

    //
    // THROW SKULL
    public void ThrowSkull()
    {
        //Debug.Log("Skull Thrown");

        skull.transform.parent = null; //Detach from parent
       
        //Use gravity so the skull can use physics movement
        rb.useGravity = true;
        rb.isKinematic = false;
        collider.enabled = true;

        // Throw
        rb.velocity = skull.transform.forward * throwSpeed;

        flameVFX.Play();

        WE.holdingSkull = false;
       
        //Infinite skulls
        StartCoroutine(ReturnSkullToInventory());

    }


    //Return the skull to the player's inventory.
    IEnumerator ReturnSkullToInventory()
    {
        //Debug.Log("Next Skull");

        canThrow = true;

        //
        //GET THE NEXT SKULL FROM THE POOL
        yield return new WaitForSeconds(skullLifetime);

        NextSkull();

        //
        //RETURN THE THROWN SKULL BACK TO THE INVENTORY
        List<GameObject> skulls = SkullManager.Instance.pooledSkullsList;

        for (int i = 0; i < skulls.Count; i++)
        {
            if (skulls[i].transform.parent == null)
            {
                yield return new WaitForSeconds(0.1f);
                
                //Bring the thrown skull back into the inventory under SkullParent of FPSPlayer
                skulls[i].transform.parent = skullParent;
                skulls[i].transform.position = skullParent.position;
                skulls[i].transform.rotation = skullParent.rotation;

                //Set Rigidbody and Collider
                rb = skulls[i].GetComponent<Rigidbody>();
                Collider collider = skulls[i].GetComponent<Collider>();

                collider.enabled = false;
                rb.isKinematic = true;
                rb.useGravity = false;
                WE.holdingSkull = true;

                skulls[i].SetActive(false);  //Hide the skull in inventory
            }
        }

        //Used to keep the skull from doing more than 1 hit to the heart
        if (bossHeart != null) 
        { 
            bossHeart.canDamage = true;
        }

        //If the weapon is switched after the skull has been thrown, hide the skull that returns to the players hand.
        if (WE.currentWeapon != WE.skullParent)
        {
            skullParent.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void NextSkull()
    {
        //After skull is thrown, make the next skull visible
        WE.skull = skullParent.transform.GetChild(0).gameObject; //Make the 0th child a current skull that is held.
        WE.skull.transform.gameObject.SetActive(true); //Make it visible

        WE.holdingSkull = true;
        WE.isEquipped = true;
    }

}
