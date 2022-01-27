using System.Collections;
using System.Collections.Generic;
using TMPro;
using System.Data.Common;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquip : MonoBehaviour
{
    public static WeaponEquip Instance;

    [Space(15)]
    [SerializeField] GameObject gunHold;
    [SerializeField] GameObject malletHold;
    public GameObject skullsParent;
    public GameObject skullHold;
    [HideInInspector] public int addToCount;
    public GameObject[] gameCards; //Holds the winning cards
    public GameObject actionPrompt;
    //public int addSkull;

    [Space(15)]
    public List<GameObject> weaponList = new List<GameObject>(); //Holds weapons

    [Space(15)]
    public int weaponNumber = 0;

    [Space(15)]
    public bool inInventory;
    public bool isEquipped;

    [Space(15)]
    public GameObject closestWeapon = null;
    [SerializeField] float pickUpRange = 1.5f;
    Vector3 distanceToPlayer;

    public bool haveGun, haveMallet, haveSkull, holdingSkull, usingFlashlight;
    public Canvas crossHair;
    [HideInInspector] public string levelName;
    public GameObject currentWeapon = null;
    private Weapon newWeapon;
    [SerializeField] GameObject skull;
    [SerializeField] Collider skullCollider;
    [SerializeField] Rigidbody skullRB;

    [HideInInspector] public Menu menu;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        menu = FindObjectOfType<Menu>();
        skullHold = GameObject.Find("SkullHold");

        //Detect if joystick or keyboard is used an display correct prompt.
        if (menu.usingJoystick)
        {
            actionPrompt = menu.controllerPrompt; //If a controller is detected set prompt for controller
        }
        else
        {
            actionPrompt = menu.keyboardPrompt; //If controller not detected set prompt for keyboard
        }
    }

    void Update()
    {
        FindClosestWeapon();
        ChangeWeapon();

        //FOR SKULL PICKUP
        if (closestWeapon.CompareTag("Head")) 
        {
            skull = closestWeapon;

            skullCollider = skull.GetComponent<Collider>();
            skullRB = skull.GetComponent<Rigidbody>();

            //Add skulls to weapon list.
            if (!weaponList.Contains(skullsParent))
            {
                weaponList.Add(skullsParent);
            }
        }

        if (currentWeapon == skullsParent)
        {
            //Update amount of skulls being held.
            addToCount = skullsParent.transform.childCount;
            menu.skullCountText.text = addToCount.ToString();
        }

        // * *
        //Check if skull parent is empty
        if (skullsParent.transform.childCount <= 0 && weaponList.Contains(skullsParent))
        {
            //Reset the hold count
            addToCount = 0;
            menu.skullCountText.text = skullsParent.transform.childCount.ToString();

            //Remove the weapon from the list
            weaponList.Remove(skullsParent);

            //Out of skulls but have other weapons.
            if (haveMallet || haveGun)
            {
                //Set the current weapon to the first in the list.
                currentWeapon = weaponList[0];
            }

            haveSkull = false; //Out of skulls
            holdingSkull = false; //Not holding skull
        }

        //
        //RETICLE
        if (isEquipped)
        {
            //Show crosshair only if weapon is equipped.
            crossHair.enabled = true;

            //If input for flashlight, put the weapon away and use flashlight.
            if (GetComponent<FPSController>().useFlashlight)
            {
                Debug.Log("this is the current weapon: " + currentWeapon.name);

                currentWeapon.SetActive(false);

                GetComponent<FPSController>().flashlightHold.SetActive(true);
                GetComponent<FPSController>().flashlightOn = true;

                isEquipped = false;
                inInventory = true;
            }

        }
        else
        {
            crossHair.enabled = false;
        }

        //
        //PLAYER INPUT
        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !haveGun && closestWeapon.CompareTag("Gun") ||
            distanceToPlayer.magnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !haveMallet && closestWeapon.CompareTag("Mallet") ||
            distanceToPlayer.magnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && closestWeapon.CompareTag("Head"))
        {
            //If there is already a weapon equipped, hide it.
            if (isEquipped)
            {
                currentWeapon.SetActive(false);
            }

            //Pick up and equip weapon.
            PickUpWeapon();
        }
        else if (Input.GetButtonDown("Fire2") && isEquipped && !inInventory)
        {
            UnequipWeapon();
        }
        else if (Input.GetButtonDown("Fire2") && !isEquipped && inInventory)
        {
            EquipWeapon();
        }

        if (!closestWeapon.CompareTag("Head"))
        {
            //SHOW ACTION/INTERACT PROMPT
            if (distanceToPlayer.magnitude <= pickUpRange)
            {
                //If within pickup range show the prompt.
                actionPrompt.SetActive(true);

                //Even if weapon is equipped, hide it and pick up new weapon.
                if (Input.GetButton("ActionButton") && !haveGun)
                {
                    PickUpWeapon();
                }

                //If have gun and closest weapon is a gun don't show the prompt.
                if (haveGun)
                {
                    if (closestWeapon.CompareTag("Gun"))
                    {
                        actionPrompt.SetActive(false);
                    }
                    //If have gun but closest weapon is not a gun show the prompt.
                    else
                    {
                        actionPrompt.SetActive(true);
                    }
                }
            }
            else
            {
                actionPrompt.SetActive(false);
            }
        }
    }

    //public void SkullInventoryManager()
    //{
    //    //Allow only 6 skulls to be held.
    //    if (skullParent.transform.childCount < 6)
    //    {
    //        //Hide the skull from in the scene.
    //        skullParent.gameObject.SetActive(true);

    //        skullCollider.enabled = false;
    //        skullRB.isKinematic = true;

    //        //Add skull to the skull hold position on FPSPlayer.
    //        skull.transform.position = skullParent.transform.position;
    //        skull.transform.parent = skullParent.transform;
    //    }

    //    //Show only the first child skull object.
    //    //skullsParent.GetChild(0).gameObject.SetActive(true);

    //    //Show first skull in count
    //    if (skullParent.transform.childCount != 0)
    //    {
    //        skullParent.transform.GetChild(0).gameObject.SetActive(true);
    //    }

    //    if (skullParent.transform.childCount == 0)
    //    {
    //        //Out of skulls but still have weapon in inventory.
    //        if (haveMallet && !isEquipped || haveGun && !isEquipped)
    //        {
    //            inInventory = true;
    //        }

    //        //playerWeapon.isEquipped = false; //Nothing in hand
    //        haveSkull = false; //Out of skulls
    //        holdingSkull = false; //Not holding skull

    //        //Remove the weapon from the list
    //        weaponList.Remove(skullParent);
    //    }
    //}


    // FIND WEAPON GAME OBJECT CLOSEST TO PLAYER
    public GameObject FindClosestWeapon()
    {
        float distanceToClosestWeapon = Mathf.Infinity;

        Weapon[] allWeapons = GameObject.FindObjectsOfType<Weapon>(); //Array to hold all weapons of the scene

        // Move through the list of weapons to find the closest
        foreach (Weapon currWeapon in allWeapons)
        {
            //Find the distance of all weapons
            float distanceToWeapon = (currWeapon.transform.position - this.transform.position).sqrMagnitude;

            //Compare distance of weapon to previously closest weapon
            if (distanceToWeapon < distanceToClosestWeapon)
            {
                distanceToClosestWeapon = distanceToWeapon; //update the closest weapon
                newWeapon = currWeapon; //set the closest weapon
                string weaponName = newWeapon.gameObject.name.ToString(); //get the string name of the closest weapon

                closestWeapon = GameObject.Find(weaponName); //find game object using the string name

                distanceToPlayer = transform.position - closestWeapon.transform.position; //used later to determine distance to pick up weapon

                //Get the name of the layer -- which is the name of the game level
                //int layerNumber = closestWeapon.layer;
                //levelName = LayerMask.LayerToName(layerNumber);

                //gameBooth = GameObject.FindGameObjectWithTag(levelName);
            }
        }

        return closestWeapon; //returns the closest weapon game object
    }

    // Use the mouse-wheel to scroll through the weapon list:
    public void ChangeWeapon()
    {
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

                //if (currentWeapon == skullsParent)
                //{
                //    //Make the parent group visible.
                //    skullsParent.SetActive(true);

                //    //Make the first skull visible.
                //    //skullsParent.transform.GetChild(0).gameObject.SetActive(true);
                //}
            }
        }
        
        //Roll scroll wheel backward
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

        //Only increase the weapon number once, not every time a skull is picked up.
        if (!weaponList.Contains(skullsParent))
        {
            weaponNumber++; //increase by 1
        }

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
        if (closestWeapon.tag == "Head")
        {
            currentWeapon = skullsParent;

            //Allow only 6 skulls to be held.
            if (skullsParent.transform.childCount < 6)
            {
                skull.SetActive(true);

                skullCollider.enabled = false;
                skullRB.isKinematic = true;

                //Add skull to the skull hold position on FPSPlayer.
                skull.transform.position = skullsParent.transform.position;
                skull.transform.parent = skullsParent.transform;
            }

            haveSkull = true;
            holdingSkull = true;
        }

        //Hides mallet and gun from scene.
        if (!CompareTag("Head"))
        {
            //Hide the weapon object in scene.
            closestWeapon.SetActive(false); //hide the picked up weapon
        }

        EquipWeapon(); //picked up weapon is equipped
    }

    // Bring weapon out of inventory:
    void EquipWeapon()
    {
        inInventory = false;
        isEquipped = true;

        //Hide flashlight if holding
        if (GetComponent<FPSController>().flashlightOn)
        {
            GetComponent<FPSController>().flashlightHold.SetActive(false);
            GetComponent<FPSController>().flashlightOn = false;
            usingFlashlight = false;
        }

        if (currentWeapon != skullsParent)
        {
            currentWeapon.SetActive(true); //show held weapon 
        }
        
        if (skullsParent.transform.childCount != 0)
        {
            Debug.Log("Turn on skull count UI.");
            menu.skullCountUI.SetActive(true);
            menu.skullCountText.text = addToCount.ToString();

            skullsParent.SetActive(true);
            skullsParent.transform.GetChild(0).gameObject.SetActive(true);
        }

        holdingSkull = true;
    }

    // Put weapon in inventory:
    public void UnequipWeapon()
    {
        //Debug.Log("Unequip!");

        inInventory = true;
        isEquipped = false;

        if (!currentWeapon.CompareTag("SkullHolder"))
        {
            currentWeapon.SetActive(false); //hide held weapon
        }
        else
        {
            skullsParent.transform.GetChild(0).gameObject.SetActive(false); //hide the skull
            holdingSkull = false;
        }

    }
}
