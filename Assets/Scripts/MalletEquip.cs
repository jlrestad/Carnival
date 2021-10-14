using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MalletEquip : MonoBehaviour
{
    public GameObject player;
    public GameObject activeWeapon;
    public GameObject gameMallet;

    [SerializeField] WeaponType weaponType = WeaponType.Mallet;

    [SerializeField] bool isEquipped;
    [SerializeField] bool haveMallet;
    [SerializeField] bool inInventory;

    public float pickUpRange = 1f;

    Vector3 distanceToPlayer;
    //[HideInInspector] public BoxCollider collider;

    public void Start()
    {
        //collider = GetComponent<BoxCollider>();
        player = GameObject.Find("FPSPlayer");
        gameMallet = GameObject.Find("GameMallet"); 
    }

    public void Update()
    {
        distanceToPlayer = player.transform.position - transform.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isEquipped && activeWeapon.activeInHierarchy == false && haveMallet == false)
        {
            GetMallet(weaponType);
        }
        else if (Input.GetKeyDown(KeyCode.E) && activeWeapon.activeInHierarchy == true && isEquipped == true && inInventory == false)
        {
            HideWeapon(weaponType);
        }
        else if (Input.GetKeyDown(KeyCode.Alpha1) && activeWeapon.activeInHierarchy == false && isEquipped == false && inInventory == true)
        {
            ShowWeapon(weaponType);
        }    
        else if (Input.GetKeyDown(KeyCode.Alpha1) && activeWeapon.activeInHierarchy == true && isEquipped == true)
        {
            HideWeapon(weaponType);
        }  
    }

    private void LateUpdate()
    {

    }

    public void GetMallet(WeaponType weaponType)
    {
        Debug.Log("Got the mallet!");

        gameMallet.SetActive(false);
        activeWeapon.SetActive(true);

        haveMallet = true; //Mallet is had!
        isEquipped = true; //And is now equipped!
        inInventory = false; //Haven't put in inventory yet.

    }

    void HideWeapon(WeaponType weaponType)
    {
        Debug.Log("Unequip!");

        activeWeapon.SetActive(false);

        isEquipped = false; //Is now unequipped.
        inInventory = true; //Put in inventory.
    }

    void ShowWeapon(WeaponType weaponType)
    {
        Debug.Log("ReEquip!");

        activeWeapon.SetActive(true);

        isEquipped = true; //Is now equipped.
    }

}
