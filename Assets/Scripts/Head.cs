using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    public static Head Instance;

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
        skullParent = GameObject.Find("SkullParent").transform;
        rb = this.GetComponent<Rigidbody>();
        collider = this.GetComponent<Collider>();
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

        //skull = playerWeapon.skull;
        //if (playerWeapon.skull != null)
        //{
        //    rb = playerWeapon.skull.GetComponent<Rigidbody>();
        //    collider = playerWeapon.skull.GetComponent<Collider>();
        //}

        //Get the distance the skull is from the player.
        distanceToPlayer = player.transform.position - transform.position;

        //PICKUP SKULL
        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetButtonDown("ActionButton"))
        {
            PickUpSkull();
        }

        //THROW SKULL
        if (Input.GetButtonDown("Fire1") && playerWeapon.holdingSkull || Input.GetAxis("RtTrigger") > 0 && playerWeapon.holdingSkull && canThrow)
        {
            ThrowSkull();
            canThrow = false;
            
            //playerWeapon.addToCount = playerWeapon.skullsParent.transform.childCount;
            //menu.skullCountText.text = playerWeapon.addToCount.ToString();
        }

        //NEXT SKULL IN INVENTORY
        //if (Input.GetButtonUp("Fire1") && playerWeapon.holdingSkull || Input.GetAxis("RtTrigger") == 0 && playerWeapon.holdingSkull)
        //{
        //    NextSkull();
        //}
        //else
        //{
        //    //For controller
        //    canThrow = true;
        //}
    }

    public void PickUpSkull() 
    {
        playerWeapon.haveSkull = true;

        //Put skull in SkullHolder on FPSPlayer
        collider.enabled = false;
        rb.isKinematic = true;

        //this.transform.position = skullParent.position;
        //this.transform.parent = skullParent.transform;

        //Only allow one item to be picked up at a time.
        if (skullParent.childCount == 0)
        {
            Debug.Log("Test skullParent childcount");
            this.transform.position = skullParent.position;
            this.transform.parent = skullParent.transform;
        }
        //else
        //{
        //    //Make it look like the skull is being picked up -- even though skulls are infinite.
        //    gameObject.SetActive(false);
        //}

        playerWeapon.holdingSkull = true;
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

            //* Get the mesh renderer of the heart
            heartMR = other.GetComponent<MeshRenderer>();

            StartCoroutine(ChangeHeartColor());
        }
        else
        {
            return;
        }
    }

    //Temp method to test skull colliding with heart. Change heart color when hit, then change back.
    IEnumerator ChangeHeartColor()
    {
        //* Change the color
        heartMR.material.color = Color.red;
        //* Change back after x time
        yield return new WaitForSeconds(2);
        heartMR.material.color = Color.gray;
    }

    public void ThrowSkull()
    {
        //Using this kewyword because there are multiple skulls in the scene and we only want to affect the skull that is held.
        this.transform.parent = null;
        rb.isKinematic = false;
        collider.enabled = true;

        // Throw
        rb.velocity = skullParent.transform.forward * throwSpeed;

        playerWeapon.holdingSkull = false;

        //Infinite skulls
        StartCoroutine(ReturnSkull());
    }

    // Had to break split the ThrowSkull method into two parts in order for the Xbox controller trigger to work properly.
    //void NextSkull()
    //{
    //    //If there are more skulls, make the next skull visible.
    //    if (playerWeapon.skullsParent.transform.childCount != 0)
    //    {
    //        playerWeapon.skullsParent.transform.GetChild(0).gameObject.SetActive(true);
    //    }
    //    else
    //    {
    //        //Out of skulls
    //        playerWeapon.isEquipped = false;

    //        //Turn off the skull hold count UI
    //        menu.skullCountUI.SetActive(false);

    //        //Check if there are weapons in inventory.
    //        if (playerWeapon.weaponList.Count > 1)
    //        {
    //            playerWeapon.inInventory = true;
    //        }
    //    }
    //}

    //Return the skull to the player hands.
    IEnumerator ReturnSkull()
    {
        yield return new WaitForSeconds(returnSkullTime);

        PickUpSkull();
    }
}
