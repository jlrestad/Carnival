using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquip : MonoBehaviour
{
    public static WeaponEquip Instance;

    [Space(15)]
    [SerializeField] GameObject gunHold;
    [SerializeField] GameObject malletHold;
    //[SerializeField] GameObject hubBooth;
    public GameObject[] gameCards; //Holds the winning cards
    public GameObject actionPrompt, gameBooth;
    
    [Space(15)]
    //public List<List<GameObject>> inventoryList = new List<List<GameObject>>(); //Holds throwables
    public List<GameObject> weaponList = new List<GameObject>(); //Holds weapons
    //public List<GameObject> skullList = new List<GameObject>(); //Holds throwables

    [Space(15)]
    public int weaponNumber = 0;

    [Space(15)]
    [SerializeField] bool inInventory;
    public bool isEquipped;

    [Space(15)]
    public GameObject closestWeapon = null;
    [SerializeField] float pickUpRange = 1f;
    Vector3 distanceToPlayer;

    [SerializeField] private bool haveGun, haveMallet;
    public Canvas crossHair;
    public string levelName;
    public GameObject currentWeapon = null;
    private Weapon newWeapon;
    Transform skullHold;

    [HideInInspector] public Menu menu;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        menu = FindObjectOfType<Menu>();
        skullHold = GameObject.Find("SkullHold").transform;

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

        //Show crosshair only if weapon is equipped.
        if (isEquipped/* || skullHold.childCount > 0*/)
        {
            crossHair.enabled = true;
        }
        else
        {
            crossHair.enabled = false;
        }
        
        //Player input
        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !haveGun && closestWeapon.tag == "Gun" || distanceToPlayer.magnitude <= pickUpRange && 
            Input.GetButtonDown("ActionButton") && !haveMallet && closestWeapon.tag == "Mallet")
        {
            gameBooth = GameObject.FindGameObjectWithTag(levelName);

            //If there is already a weapon equipped, hide it.
            if (isEquipped)
            {
                currentWeapon.SetActive(false);
            }

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
                //if (haveMallet)
                //{
                //    actionPrompt.SetActive(false);
                //}
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

                //Get the name of the layer -- which is the name of the game level
                int layerNumber = closestWeapon.layer;
                levelName = LayerMask.LayerToName(layerNumber);

                gameBooth = GameObject.FindGameObjectWithTag(levelName);
            }
        }

        return closestWeapon; //returns the closest weapon game object
    }

    //public GameObject HubBooth()
    //{
    //    hubBooth = gameBooth;

    //    return hubBooth;
    //}

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

        closestWeapon.SetActive(false); //hide the picked up weapon
        //actionPrompt.SetActive(false); //hide action prompt

        EquipWeapon(); //picked up weapon is equipped
    }

    // Bring weapon out of inventory:
    void EquipWeapon()
    {
        Debug.Log("Equip!");

        inInventory = false;
        isEquipped = true;
        currentWeapon.SetActive(true); //show held weapon 
    }

    // Put weapon in inventory:
    public void UnequipWeapon()
    {
        Debug.Log("Unequip!");

        inInventory = true;
        isEquipped = false;
        currentWeapon.SetActive(false); //hide held weapon
    }
}
