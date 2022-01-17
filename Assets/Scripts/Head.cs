using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] WeaponEquip playerWeapon;
    [SerializeField] int throwSpeed = 5;
    public Transform skullsParent; //parent of skull being held

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        playerWeapon = player.GetComponent<WeaponEquip>();

        if (!playerWeapon.haveSkull)
            skullsParent = GameObject.Find("Skulls").transform;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") && playerWeapon.holdingSkull|| Input.GetAxis("RtTrigger") > 0 && playerWeapon.holdingSkull)
        {
            ThrowSkull();
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
        playerWeapon.skullsParent.SetActive(true);
        skullsParent = GameObject.Find("Skulls").transform;

        playerWeapon.weaponList.Remove(this.gameObject);
        this.transform.parent = null;

        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = false;

        GetComponent<Collider>().enabled = true;

        //
        // Throw
        rb.velocity = transform.forward * throwSpeed; //Throws with an arc

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
}
