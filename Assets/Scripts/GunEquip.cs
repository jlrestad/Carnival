using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GunEquip : MonoBehaviour
{
    public Transform player;
    public Transform holsterPos;
    public GameObject activeWeapon;
    public GameObject holsteredWeapon;

    [SerializeField] bool isEquipped;
    [SerializeField] bool haveGun;
    [SerializeField] bool inInventory;

    [SerializeField] WeaponType weaponType = WeaponType.Gun;

    public float pickUpRange;

    [HideInInspector] public Vector3 distanceToPlayer;
    [HideInInspector] public Vector3 distanceToHolster;
    //[HideInInspector] public BoxCollider collider;

    // Start is called before the first frame update
    public void Start()
    {
        //collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    public void Update()
    {
        distanceToPlayer = player.position - transform.position;
        distanceToHolster = holsterPos.position - player.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isEquipped && activeWeapon.activeInHierarchy == false && haveGun == false)
        {
            GetGun(weaponType);
        }
        //else if (distanceToHolster.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && isEquipped && activeWeapon.activeInHierarchy == true)
        //{
        //    Unequip();
        //}
        else if (Input.GetKeyDown(KeyCode.E) && activeWeapon.activeInHierarchy == true && isEquipped == true && inInventory == false)
        {
            HideWeapon(weaponType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && activeWeapon.activeInHierarchy == false && isEquipped == false && inInventory == true)
        {
            ShowWeapon(weaponType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2) && activeWeapon.activeInHierarchy == true && isEquipped == true)
        {
            HideWeapon(weaponType);
        }
    }

    public void GetGun(WeaponType weaponType)
    {
        Debug.Log("Equipped gun!");

        holsteredWeapon.SetActive(false);
        activeWeapon.SetActive(true);

        //LOCK PLAYER MOVEMENT FOR BOOTH GAME
        //player.GetComponentInParent<CharacterController>().enabled = false;

        haveGun = true; //Mallet is had!
        isEquipped = true; //And is now equipped!
        inInventory = false; //Haven't put in inventory yet.                         
    }

    public void HideWeapon(WeaponType weaponType)
    {
        Debug.Log("Unequip!");

        //holsteredWeapon.SetActive(true);
        activeWeapon.SetActive(false);

        //player.GetComponentInParent<CharacterController>().enabled = true;

        isEquipped = false; //Is now unequipped.
        inInventory = true; //Put in inventory.
    }

    void ShowWeapon(WeaponType weaponType)
    {
        Debug.Log("ReEquip!");

        activeWeapon.SetActive(true);

        isEquipped = true; //Is now equipped.
    }

    //void GetCharacterController()
    //{
    //    if (isEquipped)
    //        player.GetComponent<CharacterController>().enabled = false;
    //    else
    //        player.GetComponent<CharacterController>().enabled = true;

    //}
}
