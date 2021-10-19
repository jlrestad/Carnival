using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunEquip : MonoBehaviour
{
    public static GunEquip Instance;

    public WeaponEquip weaponEquip;

    public Transform player;
    public Transform holsterPos;
    public GameObject activeWeapon;
    public GameObject holsteredWeapon;

    [SerializeField] bool isEquipped;
    public bool haveGun;
    [SerializeField] bool inInventory;

    public float pickUpRange;

    [HideInInspector] public Vector3 distanceToPlayer;
    [HideInInspector] public Vector3 distanceToHolster;

    public void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        weaponEquip = player.GetComponent<WeaponEquip>();
    }

    public void Update()
    {
        distanceToPlayer = player.position - transform.position;
        distanceToHolster = holsterPos.position - player.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isEquipped && !haveGun)
        {
            GetGun();
        }
        else if (Input.GetButtonDown("Fire2") && activeWeapon.activeInHierarchy && isEquipped && !inInventory)
        {
            HideWeapon();
        }
        else if (Input.GetButtonDown("Fire2") && !activeWeapon.activeInHierarchy && !isEquipped && inInventory)
        {
            ShowWeapon();
        }
    }

    public void GetGun()
    {
        Debug.Log("Equipped gun!");

        weaponEquip.weaponList.Add(this.gameObject); //Add this weapon to the weapons list.

        holsteredWeapon.SetActive(false);
        activeWeapon.SetActive(true);

        //LOCK PLAYER MOVEMENT FOR BOOTH GAME
        //player.GetComponentInParent<CharacterController>().enabled = false;

        haveGun = true; //Gun is had!
        isEquipped = true; //And is now equipped!
        inInventory = false; //Haven't put in inventory yet.                         
    }

    public void HideWeapon()
    {
        Debug.Log("Unequip!");

        activeWeapon.SetActive(false);

        //player.GetComponentInParent<CharacterController>().enabled = true;

        isEquipped = false; //Is now unequipped.
        inInventory = true; //Put in inventory.
    }

    void ShowWeapon()
    {
        Debug.Log("ReEquip!");

        activeWeapon.SetActive(true);

        isEquipped = true; //Is now equipped.
        inInventory = false;
    }

    //void GetCharacterController()
    //{
    //    if (isEquipped)
    //        player.GetComponent<CharacterController>().enabled = false;
    //    else
    //        player.GetComponent<CharacterController>().enabled = true;

    //}
}
