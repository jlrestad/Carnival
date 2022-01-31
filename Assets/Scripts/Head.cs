using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] WeaponEquip playerWeapon;
    [SerializeField] int throwSpeed = 5;
    [SerializeField] bool canThrow;
    public Transform skullsParent; //parent of skull being held

    private void Start()
    {
        canThrow = true;

        player = GameObject.FindGameObjectWithTag("Player");
        playerWeapon = player.GetComponent<WeaponEquip>();

        if (!playerWeapon.holdingSkull)
        {
            skullsParent = GameObject.Find("Skulls").transform;
        }
      
    }

    private void Update()
    {
        //if (Input.GetAxis("RtTrigger") > 0)
        //{
        //    canThrow = false;
        //}
        //else
        //{
        //    canThrow = true;
        //}

        if (Input.GetButtonDown("Fire1") && playerWeapon.holdingSkull || Input.GetAxis("RtTrigger") > 0 && playerWeapon.holdingSkull && canThrow)
        {
            ThrowSkull();
            canThrow = false;
            
            playerWeapon.addToCount = playerWeapon.skullsParent.transform.childCount;
            Menu.Instance.skullCountText.text = playerWeapon.addToCount.ToString();
        }
        if (Input.GetButtonUp("Fire1") && playerWeapon.holdingSkull || Input.GetAxis("RtTrigger") == 0 && playerWeapon.holdingSkull)
        {
            NextSkull();
        }
        else
        {
            canThrow = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bucket"))
        {
            Debug.Log("Head trigger working");
            other.GetComponentInParent<GameCardManager>().targetsList.Add(other.transform.gameObject);

            this.gameObject.SetActive(false);
        }
        else
        {
            return;
        }
    }

    public void ThrowSkull()
    {
        //If skulls were unequipped then make visible again.
        playerWeapon.skullsParent.SetActive(true); // ****
        skullsParent = GameObject.Find("Skulls").transform;

        //Turn on weapon script so that it can be found again.
        skullsParent.transform.GetChild(0).gameObject.GetComponent<Weapon>().enabled = true;
        playerWeapon.skull.tag = "Head";

        this.transform.parent = null;

        Rigidbody rb = this.GetComponent<Rigidbody>();
        rb.isKinematic = false;

        this.GetComponent<Collider>().enabled = true;

        // Throw
        rb.velocity = -this.transform.forward * throwSpeed; //Throws with an arc

        //
        //Check if skull parent is empty
        //if (skullsParent.transform.childCount == 0)
        //{
        //    //After last skull is thrown, player has nothing equipped.
        //    playerWeapon.isEquipped = false;

        //    //Out of skulls but still have weapon in inventory.
        //    if (playerWeapon.haveMallet && !playerWeapon.isEquipped || playerWeapon.haveGun && !playerWeapon.isEquipped)
        //    {
        //        playerWeapon.inInventory = true;
        //    }

        //    playerWeapon.isEquipped = false; //Nothing in hand
        //    playerWeapon.haveSkull = false; //Out of skulls
        //    playerWeapon.holdingSkull = false; //Not holding skull

        //    //Remove the weapon from the list
        //    playerWeapon.weaponList.Remove(skullsParent.gameObject);
        //}
        //else
        //{
        //    skullsParent.transform.GetChild(0).gameObject.SetActive(true);
        //}
    }

    void NextSkull()
    {
        //If there are more skulls, make the next skull visible.
        if (skullsParent.transform.childCount != 0)
        {
            skullsParent.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            //Out of skulls
            playerWeapon.isEquipped = false;

            //Turn off the skull hold count UI
            Menu.Instance.skullCountUI.SetActive(false);

            //Check if there are weapons in inventory.
            if (playerWeapon.weaponList.Count > 1)
            {
                playerWeapon.inInventory = true;
            }
        }
    }
}
