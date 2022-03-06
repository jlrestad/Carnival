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
    public bool canThrow;
    Rigidbody rb;
    Collider collider;
    GameObject skull;

    private void Start()
    {
        Instance = this;

        canThrow = true;
        menu = GameObject.Find("GameManager").GetComponent<Menu>();
        player = GameObject.FindGameObjectWithTag("Player");
        playerWeapon = player.GetComponent<WeaponEquip>();
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

        skull = playerWeapon.skull;
        rb = playerWeapon.skull.GetComponent<Rigidbody>();
        collider = playerWeapon.skull.GetComponent<Collider>();

        if (Input.GetButtonDown("Fire1") && playerWeapon.holdingSkull || Input.GetAxis("RtTrigger") > 0 && playerWeapon.holdingSkull && canThrow)
        {
            ThrowSkull();
            canThrow = false;
            
            playerWeapon.addToCount = playerWeapon.skullsParent.transform.childCount;
            menu.skullCountText.text = playerWeapon.addToCount.ToString();
        }
        if (Input.GetButtonUp("Fire1") && playerWeapon.holdingSkull || Input.GetAxis("RtTrigger") == 0 && playerWeapon.holdingSkull)
        {
            NextSkull();
        }
        //else
        //{
        //    canThrow = true;
        //}
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bucket"))
        {
            Debug.Log("Head trigger working");
            other.GetComponentInParent<GameCardManager>().critterList.Add(other.transform.gameObject);

            //If skull hits the bucket then hide it from the scene.
            playerWeapon.skull.SetActive(false);
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

        //Turn on weapon script so that it can be found again.
        playerWeapon.skullsParent.transform.GetChild(0).gameObject.GetComponent<Weapon>().enabled = true;
        playerWeapon.skull.tag = "Head";

        transform.parent = null;
        rb.isKinematic = false;
        collider.enabled = true;

        // Throw
        rb.velocity = playerWeapon.transform.forward * throwSpeed;

        StartCoroutine(TurnOffGameObject());
    }

    // Had to break split the ThrowSkull method into two parts in order for the Xbox controller trigger to work properly.
    void NextSkull()
    {
        //If there are more skulls, make the next skull visible.
        if (playerWeapon.skullsParent.transform.childCount != 0)
        {
            playerWeapon.skullsParent.transform.GetChild(0).gameObject.SetActive(true);
        }
        else
        {
            //Out of skulls
            playerWeapon.isEquipped = false;

            //Turn off the skull hold count UI
            menu.skullCountUI.SetActive(false);

            //Check if there are weapons in inventory.
            if (playerWeapon.weaponList.Count > 1)
            {
                playerWeapon.inInventory = true;
            }
        }
    }

    IEnumerator TurnOffGameObject()
    {
        yield return new WaitForSeconds(3f);
        playerWeapon.skull.SetActive(false);
    }
}
