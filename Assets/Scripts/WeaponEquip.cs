using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;


public class WeaponEquip : MonoBehaviour
{
    [Space(15)]
    public GameObject gun;
    public GameObject mallet;
    [Space(15)]
    [SerializeField] public List<GameObject> weaponList;
    [Space(15)]
    public int weaponNumber = 0;
    [Space(15)]
    [SerializeField] bool inInventory;
    [SerializeField] bool isEquipped;
    [Space(15)]
    [SerializeField] private GameObject closestWeapon = null;

    private GameObject currentWeapon = null;
    private Weapon newWeapon;
    [SerializeField]private bool haveGun, haveMallet;

    private void Awake()
    {
        //Instance = this;
    }

    private void Start()
    {
       
    }

    void Update()
    {
        FindClosestWeapon();

        if (Input.GetKeyDown(KeyCode.E) && !isEquipped && !haveGun && closestWeapon.tag == "Gun" || Input.GetKeyDown(KeyCode.E) && !isEquipped && !haveMallet && closestWeapon.tag == "Mallet")
        {
            GetWeapon();
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

    public GameObject FindClosestWeapon()
    {
        float distanceToClosestWeapon = Mathf.Infinity;
        //Weapon closestWeapon = null;
        Weapon[] allWeapons = GameObject.FindObjectsOfType<Weapon>();

        foreach (Weapon currWeapon in allWeapons)
        {
            float distanceToWeapon = (currWeapon.transform.position - this.transform.position).sqrMagnitude;

            if (distanceToWeapon < distanceToClosestWeapon)
            {
                distanceToClosestWeapon = distanceToWeapon;
                newWeapon = currWeapon; //the closest weapon
                string weaponName = newWeapon.gameObject.name.ToString(); //get the name of the closest weapon
                closestWeapon = GameObject.Find(weaponName); //get the game object the Weapon script is attached to using the name
            }
        }

        return closestWeapon;
    }

    public void ChangeWeapon()
    {
        //Debug.Log("List amount: " + weapons.Count);

        //Roll scroll wheel forward
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            //If there is already a weapon equipped, hide it.
            if (closestWeapon != null && weaponList.Count > 0) //weapon equipped and there is a weapon in the list
            {
                closestWeapon.SetActive(false);
                weaponNumber++; //move to next list weapon

                //Set the bounds.
                if (weaponNumber > weaponList.Count)
                {
                    weaponNumber = 1;
                }

                currentWeapon.SetActive(true); //show the weapon
            }
            //else if (currentWeapon == null && hasWeapon) //no weapon equipped but there is a weapon in the list
            //{
            //    weaponNumber++;
            //    currentWeapon = weapons[weaponNumber];
            //    currentWeapon.SetActive(true);
            //}



            //Roll scroll wheel back
            //if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            //{
            //    //If there is already a weapon equipped, hide it.
            //    if (currentWeapon != null && weapons.Count > 0)
            //    {
            //        currentWeapon.SetActive(false);
            //        weaponNumber--; //move to the previous list weapon
            //        currentWeapon = weapons[weaponNumber]; //set current weapon to the index number
            //        currentWeapon.SetActive(false); //show the weapon
            //    }
            //    else if (weapons.Count > 0)
            //    {
            //        weaponNumber++;
            //        currentWeapon = weapons[weaponNumber];
            //        currentWeapon.SetActive(true);
            //    }

            //    //Set the bounds.        
            //    if (weaponNumber < 0)
            //    {
            //        weaponNumber = 0;
            //    }
        }
    }

    /// ...EQUIP WEAPONS... ///

    public void GetWeapon()
    {
        isEquipped = true;

        weaponList.Add(closestWeapon); //add new weapon to the list
        int index = weaponList.IndexOf(closestWeapon);

        weaponNumber++; //increase by 1

        //Check which weapon it is and get the tag.
        if (weaponList[index].tag == "Gun" && !haveGun)
        {
            currentWeapon = gun;
            haveGun = true;
        }
        if (weaponList[index].tag == "Mallet" && !haveMallet)
        {
            currentWeapon = mallet;
            haveMallet = true;
        }

        Debug.Log("Got " + closestWeapon.tag + "!");
        Debug.Log("Current weapon is " + currentWeapon);

        weaponList[index].SetActive(false); //hide this object

        ShowWeapon();

        //LOCK PLAYER MOVEMENT FOR BOOTH GAME
        //player.GetComponentInParent<CharacterController>().enabled = false;

        //haveGun = true; //Gun is had!
        //isEquipped = true; //And is now equipped!
        //inInventory = false; //Haven't put in inventory yet.                         
    }

    public void HideWeapon()
    {
        Debug.Log("Unequip!");

        inInventory = true;
        isEquipped = false;
        currentWeapon.SetActive(false);

        ////player.GetComponentInParent<CharacterController>().enabled = true;

        //isEquipped = false; //Is now unequipped.
        //inInventory = true; //Put in inventory.
    }

    void ShowWeapon()
    {
        Debug.Log("Equip!");

        inInventory = false;
        isEquipped = true;
        currentWeapon.SetActive(true);

        //isEquipped = true; //Is now equipped.
        //inInventory = false;
    }
}
