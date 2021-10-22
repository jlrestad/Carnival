using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquip : MonoBehaviour
{
    public static WeaponEquip Instance;

    [Space(15)]
    public GameObject gun;
    public GameObject mallet;

    [Space(15)]
    [SerializeField] public List<GameObject> weaponList;

    [Space(15)]
    public int weaponNumber = 0;

    [Space(15)]
    [SerializeField] bool inInventory;
    public bool isEquipped;

    [Space(15)]
    [SerializeField] private GameObject closestWeapon = null;
    [SerializeField] float pickUpRange = 1f;
    Vector3 distanceToPlayer;

    private GameObject currentWeapon = null;
    private Weapon newWeapon;
    [SerializeField]private bool haveGun, haveMallet;
    public Canvas crossHair;
    public Menu menu;
    public GameObject actionPrompt;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //To make the action prompt appear
        menu = FindObjectOfType<Menu>();
        //actionPrompt = menu.ePrompt; //Turned off while working in level scene
    }

    void Update()
    {
        FindClosestWeapon();
        //ChangeWeapon();

        if (isEquipped)
        {
            crossHair.enabled = true;
        }
        else
        {
            crossHair.enabled = false;
        }

        
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

        //Show action prompt if within pickup range
        //if (distanceToPlayer.magnitude <= pickUpRange)
        //{
        //    //If within pickup range and nothing is equipped show the prompt.
        //    if (!isEquipped)
        //    {
        //        actionPrompt.SetActive(true);
        //    }
        //    if (haveGun)
        //    {
        //        //If have gun and closest weapon is a gun don't show the prompt.
        //        if (closestWeapon.CompareTag("Gun"))
        //        {
        //            actionPrompt.SetActive(false);
        //        }
        //        //If have gun but closest weapon is not a gun show the prompt.
        //        else
        //        {
        //            actionPrompt.SetActive(true);
        //        }
        //    }
        //}
        //else
        //{
        //    actionPrompt.SetActive(false);
        //}
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
        //Debug.Log("List amount: " + weapons.Count);

        //Roll scroll wheel forward
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            //If there is already a weapon equipped, hide it.
            if (currentWeapon != null) //weapon equipped and there is a weapon in the list
            {
                currentWeapon.SetActive(false);

                weaponNumber++; //move to next list weapon

                //Check bounds.
                if (weaponNumber > weaponList.Count)
                {
                    weaponNumber = 1;
                }

                currentWeapon = weaponList[weaponNumber]; //change current weapon to the new scrolled weapon
                currentWeapon.SetActive(true); //show the weapon
            }
            else
            {

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

    /// ...EQUIP WEAPONS SECTION... ///

    public void PickUpWeapon()
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
    }

    // Put weapon in inventory:
    public void HideWeapon()
    {
        Debug.Log("Unequip!");

        inInventory = true;
        isEquipped = false;
        currentWeapon.SetActive(false);
    }

    // Bring weapon out of inventory:
    void ShowWeapon()
    {
        Debug.Log("Equip!");

        inInventory = false;
        isEquipped = true;
        currentWeapon.SetActive(true);
    }
}
