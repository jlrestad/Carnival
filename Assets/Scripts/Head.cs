using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public static Head Instance;

    BossHeart bossHeart;
    [SerializeField] GameObject player;
    [SerializeField] WeaponEquip playerWeapon;
    [SerializeField] Menu menu;
    [SerializeField] int throwSpeed = 5;
    [SerializeField] int pickUpRange;

    public float returnSkullTime = 1.0f;
    Vector3 distanceToPlayer;

    MeshRenderer heartMR; //MeshRenderer of the Boss' heart.
    Rigidbody rb;
    Collider collider;
    GameObject skull;
    Transform skullParent;
    

    public bool canThrow;


    private void Start()
    {
        Instance = this;

        canThrow = true;

        menu = GameObject.Find("GameManager").GetComponent<Menu>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerWeapon = player.GetComponent<WeaponEquip>();
        collider = GetComponent<Collider>();
        skullParent = playerWeapon.skullParent.transform;
    }   

    private void FixedUpdate()
    {
        // For Controller:
        //if (Input.GetAxis("RtTrigger") > 0)
        //{
        //    canThrow = false;
        //}
        //else
        //{
        //    canThrow = true;
        //}

        //Get the rigidbody and collider from the player's skull-weapon.
        if (playerWeapon.skull != null)
        {
            rb = playerWeapon.skull.GetComponent<Rigidbody>();
            collider = playerWeapon.skull.GetComponent<Collider>();
        }

        //Get the distance the skull is from the player.
        distanceToPlayer = player.transform.position - transform.position;

        //PICKUP SKULL
        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetButtonDown("ActionButton"))
        {
            //If holding a weapon, put it away.
            if (playerWeapon.isEquipped && playerWeapon.currentWeapon != skullParent)
            {
                playerWeapon.currentWeapon.SetActive(false);
            }

            PickUpSkull();
        }

        //THROW SKULL
        if (Input.GetButtonDown("Fire1") && playerWeapon.holdingSkull || Input.GetAxis("RtTrigger") > 0 && playerWeapon.holdingSkull && canThrow)
        {
            ThrowSkull();
            canThrow = false;
        }
    }

    //
    // SKULL TRIGGER ENTER
    private void OnTriggerEnter(Collider other)
    {
        //For Casket Baskets game
        if (other.CompareTag("Bucket"))
        {
            Debug.Log("Head trigger working");
            other.GetComponentInParent<GameCardManager>().critterList.Add(other.transform.gameObject);

            //If skull hits the bucket then hide it from the scene.
            playerWeapon.skull.SetActive(false);
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

    //
    // PICKUP SKULL
    public void PickUpSkull() 
    {
        this.gameObject.SetActive(false);  //Turn off skull in scene

        //skullParent.gameObject.SetActive(true);  //Turn on player's skullParent

        playerWeapon.currentWeapon = skullParent.gameObject; //Set current weapon
        playerWeapon.skull = skullParent.transform.GetChild(0).gameObject; //Set the skull that is held
        playerWeapon.skull.transform.gameObject.SetActive(true); //Make the skull visible

        playerWeapon.holdingSkull = true;
        playerWeapon.inInventory = false;
    }

    //
    // THROW SKULL
    public void ThrowSkull()
    {
        //Debug.Log("Skull Thrown");

        playerWeapon.skull.transform.parent = null; //Detach from parent

        //Use gravity so the skull can use physics movement
        rb.isKinematic = false;
        rb.useGravity = true;
        collider.enabled = true;

        // Throw
        rb.velocity = skullParent.transform.forward * throwSpeed;

        playerWeapon.holdingSkull = false;
        playerWeapon.inInventory = false;
        playerWeapon.isEquipped = true;

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
        yield return new WaitForSeconds(0.5f);

        NextSkull();

        //
        //RETURN THE THROWN SKULL BACK TO THE INVENTORY
        List<GameObject> skulls = SkullManager.Instance.pooledSkullsList;

        for (int i = 0; i < skulls.Count; i++)
        {
            if (skulls[i].transform.parent == null)
            {
                yield return new WaitForSeconds(Head.Instance.returnSkullTime);

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

                skulls[i].SetActive(false);  //Hide the skull in inventory
            }
        }

        //Used to keep the skull from doing more than 1 hit to the heart
        if (bossHeart != null) 
        { 
            bossHeart.canDamage = true;
        }

        //If the weapon is switched after the skull has been thrown, hide the skull that returns to the players hand.
        if (playerWeapon.currentWeapon != playerWeapon.skullParent)
        {
            skullParent.transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    void NextSkull()
    {
        //After skull is thrown, make the next skull visible
        playerWeapon.skull = skullParent.transform.GetChild(0).gameObject; //Make the 0th child a current skull that is held.
        playerWeapon.skull.transform.gameObject.SetActive(true); //Make it visible

        playerWeapon.holdingSkull = true;
        playerWeapon.isEquipped = true;
    }

}
