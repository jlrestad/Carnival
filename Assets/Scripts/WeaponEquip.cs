using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;


public class WeaponEquip : MonoBehaviour
{
    public static WeaponEquip Instance;

<<<<<<< Updated upstream
    //[SerializeField] GameObject gun, mallet; //use to find distance from player (magnitude)
    [Space(15)]
    [SerializeField] public List<GameObject> weapons;
=======
    [Space(15)]
    public GameObject gun;
    public GameObject mallet;

    [Space(15)]
    [SerializeField] public List<GameObject> weaponList;

>>>>>>> Stashed changes
    [Space(15)]
    public GameObject currentWeapon = null;
    public int weaponNumber = 0;
<<<<<<< Updated upstream
=======

    [Space(15)]
    [SerializeField] bool inInventory;
    public bool isEquipped;

    [Space(15)]
    [SerializeField] private GameObject closestWeapon = null;
    [SerializeField] float pickUpRange = 1f;
    Vector3 distanceToPlayer;
>>>>>>> Stashed changes


    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
<<<<<<< Updated upstream
=======
        FindClosestWeapon();
        //ChangeWeapon();

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isEquipped && !haveGun && closestWeapon.tag == "Gun" || distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isEquipped && !haveMallet && closestWeapon.tag == "Mallet")
        {
            PickUpWeapon();
        }
        else if (Input.GetButtonDown("Fire2") && isEquipped && !inInventory)
        {
            HideWeapon();
        }
        else if (Input.GetButtonDown("Fire2") && !isEquipped && inInventory)
        {
            ShowWeapon();
        }
    }

    // Find the weapon that is closest to the player
    public GameObject FindClosestWeapon()
    {
        float distanceToClosestWeapon = Mathf.Infinity;

        Weapon[] allWeapons = GameObject.FindObjectsOfType<Weapon>(); //Array to hold all weapons of the scene

        // Move through the list of weapons to find the closest
        foreach (Weapon currWeapon in allWeapons)
        {
            float distanceToWeapon = (currWeapon.transform.position - this.transform.position).sqrMagnitude;

            if (distanceToWeapon < distanceToClosestWeapon)
            {
                distanceToClosestWeapon = distanceToWeapon; //update the closest weapon
                newWeapon = currWeapon; //set the closest weapon
                string weaponName = newWeapon.gameObject.name.ToString(); //get the name of the closest weapon

                closestWeapon = GameObject.Find(weaponName); //use the name of the weapon to get the game object that is attached so it can be returned

                distanceToPlayer = transform.position - closestWeapon.transform.position; //use the distance to restrict how far a player can grab weapon
            }
        }

        return closestWeapon;
    }

    // Use the mouse-wheel to scroll through the weapon list:
    public void ChangeWeapon()
    {
>>>>>>> Stashed changes
        //Debug.Log("List amount: " + weapons.Count);

        //Roll scroll wheel forward
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            //If there is already a weapon equipped, hide it.
            if (currentWeapon != null && weapons.Count > 0)
            {
                currentWeapon.SetActive(false);
                weaponNumber++; //move to next list weapon
                currentWeapon = weapons[weaponNumber]; //set current weapon to the index number
                currentWeapon.SetActive(true); //show the weapon
            }

            //Set the bounds.
            if (weaponNumber > weapons.Count)
            {
                weaponNumber = 0;
            }

            //Roll scroll wheel back
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                //If there is already a weapon equipped, hide it.
                if (currentWeapon != null && weapons.Count > 0)
                {
                    currentWeapon.SetActive(false);
                    weaponNumber--; //move to the previous list weapon
                    currentWeapon = weapons[weaponNumber]; //set current weapon to the index number
                    currentWeapon.SetActive(false); //show the weapon
                }

                //Set the bounds.        
                if (weaponNumber < 0)
                {
                    weaponNumber = 0;
                }
            }
        }
    }
}
