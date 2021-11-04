using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquip : MonoBehaviour
{
    public static WeaponEquip Instance;

    [Space(15)]
    public GameObject gunHold;
    public GameObject malletHold;

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

    [SerializeField] private GameObject currentWeapon = null;
    private Weapon newWeapon;
    [SerializeField]private bool haveGun, haveMallet;
    public Canvas crossHair;
    public Menu menu;
    public GameObject actionPrompt;
    public string levelName;

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
        ChangeWeapon();

        if (isEquipped)
        {
            crossHair.enabled = true;
        }
        else
        {
            crossHair.enabled = false;
        }

        
        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !haveGun && closestWeapon.tag == "Gun" || distanceToPlayer.magnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !haveMallet && closestWeapon.tag == "Mallet")
        {
            //If there is already a weapon equipped, hide it.
            if (isEquipped)
            {
                currentWeapon.SetActive(false);
            }
            PickUpWeapon();
            menu.ChangeLevel(levelName);
        }
        else if (Input.GetButtonDown("Fire2") && isEquipped && !inInventory)
        {
            HideWeapon();
        }
        else if (Input.GetButtonDown("Fire2") && !isEquipped && inInventory)
        {
            ShowWeapon();
        }

        //SHOW ACTION/INTERACT PROMPT
        //if (distanceToPlayer.magnitude <= pickUpRange)
        //{
        //    //If within pickup range show the prompt.
        //    actionPrompt.SetActive(true);

        //    //Even if weapon is equipped, hide it and pick up new weapon.
        //    if (Input.GetButton("ActionButton") && !haveGun)
        //    {
        //        currentWeapon.SetActive(false);
        //        PickUpWeapon();
        //    }

        //    //If have gun and closest weapon is a gun don't show the prompt.
        //    if (haveGun)
        //    {
        //        if (Input.GetButton("ActionButton"))
        //        {
        //            currentWeapon.SetActive(false);
        //            PickUpWeapon();
        //        }

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

    // FIND WEAPON GAME OBJECT CLOSEST TO PLAYER
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

        return closestWeapon; //returns the closest weapon game object
    }

    // Use the mouse-wheel to scroll through the weapon list:
    public void ChangeWeapon()
    {
        //Debug.Log("List amount: " + weapons.Count);

        //Roll scroll wheel forward
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            //If there is already a weapon equipped, hide it.
            if (isEquipped && weaponList.Count > 1) //weapon equipped and there is a weapon in the list
            {
                currentWeapon.SetActive(false);

                weaponNumber++; //move to next list weapon

                //Check bounds.
                if (weaponNumber > weaponList.Count - 1)
                {
                    weaponNumber = 0;
                }

                currentWeapon = weaponList[weaponNumber]; //change current weapon to the new scrolled weapon
                currentWeapon.SetActive(true); //show the weapon
            }
        }
        
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (isEquipped && weaponList.Count > 1)
            {
                currentWeapon.SetActive(false);

                weaponNumber--;

                if (weaponNumber < 0)
                {
                    weaponNumber = weaponList.Count -1;
                }

                currentWeapon = weaponList[weaponNumber];

                currentWeapon.SetActive(true);
            }
        }
    }


    /// ...EQUIP WEAPONS SECTION... ///

    public void PickUpWeapon()
    {
        isEquipped = true;
        weaponNumber++; //increase by 1

        //Check which weapon it is and get the tag.
        if (closestWeapon.tag == "Gun" && !haveGun)
        {
            currentWeapon = gunHold;
            weaponList.Add(currentWeapon);
            haveGun = true;
        }
        if (closestWeapon.tag == "Mallet" && !haveMallet)
        {
            currentWeapon = malletHold;
            weaponList.Add(malletHold);
            haveMallet = true;
        }

        //Debug.Log("Got " + closestWeapon.tag + "!");
        //Debug.Log("Current weapon is " + currentWeapon);

        closestWeapon.SetActive(false); //deactivate this to show it has been picked up

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
