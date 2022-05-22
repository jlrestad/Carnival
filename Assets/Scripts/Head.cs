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

    private void Update()
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

    public void PickUpSkull() 
    {
        this.gameObject.SetActive(false);  //Turn off skull in scene

        skullParent.gameObject.SetActive(true);  //Turn on player's skullParent

        playerWeapon.currentWeapon = skullParent.gameObject;
        playerWeapon.skull = skullParent.transform.GetChild(0).gameObject;
        playerWeapon.skull.transform.parent = skullParent; //Set the parent of the skull that is held.

        playerWeapon.holdingSkull = true;
        playerWeapon.inInventory = false;
    }

    public void ThrowSkull()
    {
        //Using this keyword because there are multiple skulls in the scene and we only want to affect the skull that is held.
        playerWeapon.skull.transform.parent = null;

        rb.isKinematic = false;
        rb.useGravity = true;
        collider.enabled = true;

        // Throw
        rb.velocity = skullParent.transform.forward * throwSpeed;

        playerWeapon.holdingSkull = false;
        playerWeapon.inInventory = false;
        playerWeapon.isEquipped = true;

        //Infinite skulls
        StartCoroutine(ReturnSkull());
    }

 
    //Return the skull to the player hands.
    IEnumerator ReturnSkull()
    {
        canThrow = true;

        yield return new WaitForSeconds(returnSkullTime);

        playerWeapon.skull.transform.parent = skullParent;

        collider.enabled = false;
        rb.useGravity = false;
        rb.isKinematic = true;

        playerWeapon.skull.transform.position = skullParent.position;
        playerWeapon.skull.transform.parent = skullParent.transform;

        playerWeapon.holdingSkull = true;
        playerWeapon.isEquipped = true;
        
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
}
