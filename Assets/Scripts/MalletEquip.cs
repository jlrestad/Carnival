using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class MalletEquip : MonoBehaviour
{
    public static MalletEquip Instance;

    public WeaponEquip weaponEquip;

    public GameObject player;
    public GameObject activeWeapon;
    public GameObject gameMallet;

    [SerializeField] bool isEquipped;
    public bool haveMallet;
    [SerializeField] bool inInventory;

    public float pickUpRange = 1f;

    Vector3 distanceToPlayer;

    private void Awake()
    {
        Instance = this;
    }

    public void Start()
    {
        player = GameObject.Find("FPSPlayer");
        gameMallet = GameObject.Find("GameMallet");

        weaponEquip = player.GetComponent<WeaponEquip>();
    }

    public void Update()
    {
        distanceToPlayer = player.transform.position - transform.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isEquipped && !haveMallet)
        {
            GetMallet();
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

    public void GetMallet( )
    {
        Debug.Log("Got the mallet!");

        weaponEquip.weaponList.Add(this.gameObject); //Add this weapon to the weapons list.

        gameMallet.SetActive(false);
        activeWeapon.SetActive(true);

        haveMallet = true; //Mallet is had!
        isEquipped = true; //And is now equipped!
        inInventory = false; //Haven't put in inventory yet.

    }

    void HideWeapon( )
    {
        Debug.Log("Unequip!");

        activeWeapon.SetActive(false);

        isEquipped = false; //Is now unequipped.
        inInventory = true; //Put in inventory.
    }

    void ShowWeapon( )
    {
        Debug.Log("ReEquip!");

        activeWeapon.SetActive(true);

        isEquipped = true; //Is now equipped.
        inInventory = false;
    }

}
