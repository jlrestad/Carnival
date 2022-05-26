using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquip : MonoBehaviour
{
    [TextArea]
    [SerializeField] string notes;

    public static WeaponEquip Instance;

    [Header("UI")]
    public Canvas crossHair;
    public GameObject actionPrompt;
    public List<GameObject> weaponCardBG;
    int BGCount;

    [Space(15)]
    [SerializeField] GameObject gunHold;
    [SerializeField] GameObject malletHold;
    public GameObject skullParent; //This holds the skulls (now changed to 1 infinite skull)
    //public GameObject skullHold; //This identifies the weapon
    [HideInInspector] public int addToCount;
    //public int addSkull;

    [Space(15)]
    public List<GameObject> weaponList = new List<GameObject>(); //Holds weapons

    [Space(15)]
    public int weaponNumber = -1;

    [Space(15)]
    public bool inInventory;
    public bool isEquipped;

    [Space(15)]
    public GameObject closestWeapon = null;
    public GameObject prevWeapon = null;
    [HideInInspector] public GameObject _closestWeapon = null;
    public GameObject currentWeapon = null;
    public GameObject closestSkull = null;
    [SerializeField] float pickUpRange = 1.5f;
    Vector3 distanceToPlayer;

    [Space(15)]
    public GameObject skull;
    //[SerializeField] Collider skullCollider;
    //[SerializeField] Rigidbody skullRB;

    [Space(2)]
    /*[HideInInspector] */public bool haveGun, haveMallet, haveSkull, holdingSkull;

    public bool whackEmActive = false;
    public bool skillshotActive = false;

    [Space(15)]
    public string gameName;
    private Weapon newWeapon;
    //private Head newSkull;
    //public Head[] headSkull;
    Head head; //Get the Head script for skull

    [HideInInspector] RaycastHit hit;
    public Menu menu;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        menu = FindObjectOfType<Menu>();

        //weaponCardBG.Add(menu.gameCardBG); //Add the won game card bg to the list. Used to highlight which weapon is equipped

        //Detect if joystick or keyboard is used and display correct prompt.
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

        if (isEquipped || inInventory)
        {
            ChangeWeapon();
        }

        //FOR SKULL PICKUP * * *
        if (currentWeapon == skullParent)
        {
            //Get Head.cs from the closest skull
            head = currentWeapon.GetComponent<Head>();
            haveSkull = true;
        }

        if (holdingSkull)
        {
            //Add skulls to weapon list.
            if (!weaponList.Contains(skullParent))
            {
                weaponList.Add(skullParent);
            }
        }

        // * * *
        //RETICLE DISPLAY -- Only show crosshair if a weapon is equipped.
        if (isEquipped)
        {
            //Show crosshair only if weapon is equipped.
            crossHair.enabled = true;

            //If input for flashlight, put the weapon away and use flashlight.
            if (GetComponent<FPSController>().useFlashlight)
            {
                Debug.Log("this is the current weapon: " + currentWeapon.name);

                //Turn on the flashlight
                GetComponent<FPSController>().flashlightHold.SetActive(true);
                GetComponent<FPSController>().flashlightOn = true;

                isEquipped = false;
                inInventory = true;
            }
        }
        else
        {
            //Don't display reticle if nothing is equipped.
            crossHair.enabled = false;
        }

        //Debug.Log(weaponList.Count);
        // * * *
        //PLAYER INPUT
        if (distanceToPlayer.sqrMagnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !haveGun && closestWeapon.CompareTag("Gun") ||
            distanceToPlayer.sqrMagnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !haveMallet && closestWeapon.CompareTag("Mallet") ||
            distanceToPlayer.sqrMagnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && closestWeapon.CompareTag("Head"))
        {

            //Pick up and equip weapon.
            PickUpWeapon();
            BGCount = menu.BGCount; //get the count from Menu
        }
        else if (Input.GetButtonDown("Fire2") && isEquipped && !inInventory)
        {
            //Put weapon in inventory.
            UnequipWeapon();
        }
        else if (Input.GetButtonDown("Fire2") && !isEquipped && inInventory)
        {
            //Equip weapon from inventory.
            EquipWeapon();
        }

        if (!gameObject.CompareTag("Head") /*&& !closestWeapon.CompareTag("Untagged")*/)
        {
            //SHOW ACTION/INTERACT PROMPT
            //if (distanceToPlayer.magnitude <= pickUpRange && closestWeapon != skull)
            //{
                if (Physics.Raycast(transform.position, transform.forward, out hit, 2))
                {
                    if (hit.transform.GetComponent<Weapon>())
                    {
                        //If within pickup range show the prompt.
                        actionPrompt.SetActive(true);
                    }

                    //Even if weapon is equipped, hide it and pick up new weapon.
                    if (Input.GetButton("ActionButton") && !haveGun) //Because there are multiple guns in scene. If only one is avail then get rid of the bool. ***
                    {
                        PickUpWeapon();
                    }

                    // ** Because there are multiple guns in scene. If only one then this is uneccessary. **
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

    // FIND WEAPON GAME OBJECT CLOSEST TO PLAYER
    public GameObject FindClosestWeapon()
    {
        float distanceToClosestWeapon = Mathf.Infinity; 

        Weapon[] allWeapons = GameObject.FindObjectsOfType<Weapon>(); //Array to hold all weapons of the scene

        // Move through the list of weapons to find the closest
        foreach (Weapon currWeapon in allWeapons)
        {
            //Find the distance of each weapon
            float distanceToWeapon = (currWeapon.transform.position - transform.position).sqrMagnitude;

            //Compare distance of weapon to previously closest weapon
            if (distanceToWeapon < distanceToClosestWeapon)
            {
                distanceToClosestWeapon = distanceToWeapon; //update the closest weapon
                newWeapon = currWeapon; //set the closest weapon

                string weaponName = newWeapon.gameObject.name.ToString(); //get the name of the closest weapon
           
                closestWeapon = GameObject.Find(weaponName); //find game object using the string name

                _closestWeapon = closestWeapon;

                distanceToPlayer = transform.position - closestWeapon.transform.position; //used later to determine distance to pick up weapon

                int layerNumber = closestWeapon.layer;
                gameName = LayerMask.LayerToName(layerNumber);
            }
        }
        return closestWeapon; //returns the closest weapon game object
    }

    //WEAPON SCROLL MANAGER
    public void ChangeWeapon()
    {
        //SCROLL WHEEL FORWARD
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0 && isEquipped || Input.GetButtonDown("WeaponScroll+") && isEquipped)
        {
            WeaponScrollPositive();
        }

        //SCROLL WHEEL BACKWARD
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && isEquipped || Input.GetButtonDown("WeaponScroll-") && isEquipped)
        {
            WeaponScrollNegative();
        }
    }

    void WeaponScrollPositive()
    {
        //Turn off previous weapon BG
        weaponCardBG[weaponNumber].GetComponent<Image>().enabled = false;

        //Unequip current weapon.
        if (weaponList.Count > 1 && currentWeapon != skullParent)
        {
            //If there is already a weapon equipped, hide it.
            currentWeapon.SetActive(false);

            //Menu.Instance.gameCardBG.GetComponent<Image>().enabled = false; //* This turns off 2nd card, leaves 1st card on
        }
        if (weaponList.Count > 1 && currentWeapon == skullParent && holdingSkull)
        {
            //Hide the child of skulls parent, not the parent (which is the current weapon) so that more skulls may be collected.
            skullParent.transform.GetChild(0).gameObject.SetActive(false);
            holdingSkull = false;
        }

        if (weaponList.Count > 1)
        {
            //Move to the next weapon in the list.
            weaponNumber++;
        }

        //Check bounds of weapon number.
        if (weaponNumber > weaponList.Count - 1)
        {
            weaponNumber = 0; //go back to the beginning
        }
        if (weaponNumber < 0)
        {
            weaponNumber = weaponList.Count;
        }

        //Turn on new weapon BG
        weaponCardBG[weaponNumber].GetComponent<Image>().enabled = true;

        //Change current weapon to the next weapon in the list.
        currentWeapon = weaponList[weaponNumber];

        //Equip the weapon
        if (currentWeapon == skullParent)
        {
            //Equip skull
            skullParent.transform.GetChild(0).gameObject.SetActive(true); //Put in inventory.
            holdingSkull = true;
        }
        else
        {
            //Equip other weapon
            currentWeapon.SetActive(true); //show the weapon
            holdingSkull = false;
        }
    }

    void WeaponScrollNegative()
    {
        //Turn off previous weapon BG
        weaponCardBG[weaponNumber].GetComponent<Image>().enabled = false;

        //Unequip current weapon.
        if (weaponList.Count > 1 && currentWeapon != skullParent)
        {
            //If there is already a weapon equipped, hide it.
            currentWeapon.SetActive(false);
        }
        if (weaponList.Count > 1 && currentWeapon == skullParent && holdingSkull)
        {
            //Hide the child of skulls parent, not the parent (which is the current weapon) so that more skulls may be collected.
            skullParent.transform.GetChild(0).gameObject.SetActive(false); //Put in inventory.
            holdingSkull = false;
        }

        //** WEAPONNUMBER BOUNDS
        //Check bounds of weapon number.
        if (weaponNumber > weaponList.Count - 1)
        {
            weaponNumber = 0; //go back to the beginning
        }
        else if (weaponNumber <= 0)
        {
            weaponNumber = weaponList.Count - 1;
        }
        else
        {
            weaponNumber--;
        }

        //Turn on previous weapon BG
        weaponCardBG[weaponNumber].GetComponent<Image>().enabled = true;

        //Change current weapon to the previous weapon in the list.
        currentWeapon = weaponList[weaponNumber];

        //Equip the weapon
        if (currentWeapon == skullParent)
        {
            //Equip skull
            skullParent.transform.GetChild(0).gameObject.SetActive(true);
            holdingSkull = true;
        }
        else
        {
            //Equip other weapon
            currentWeapon.SetActive(true);
            holdingSkull = false;
        }
    }

    /// ...EQUIP WEAPONS SECTION... ///

    public void PickUpWeapon()
    {
        //isEquipped = true;

        //GUN
        if (closestWeapon.CompareTag("Gun") && !haveGun && skillshotActive)
        {
            currentWeapon = gunHold;
            weaponList.Add(currentWeapon);
            haveGun = true;
            prevWeapon = closestWeapon;
            closestWeapon.SetActive(false); //hide the picked up weapon

            //After picking up the weapon, equip it.
            EquipWeapon(); //picked up weapon is equipped
        }
        //MALLET
        if (closestWeapon.CompareTag("Mallet") && !haveMallet && whackEmActive)
        {
            currentWeapon = malletHold;
            weaponList.Add(malletHold);
            haveMallet = true;
            prevWeapon = closestWeapon;
            closestWeapon.SetActive(false); //hide the picked up weapon

            //After picking up the weapon, equip it.
            EquipWeapon(); //picked up weapon is equipped
        }
    }

    // Bring weapon out of inventory:
    void EquipWeapon()
    {
        inInventory = false;
        isEquipped = true;
        weaponNumber++;
        if (weaponNumber > weaponList.Count) { weaponNumber = weaponList.Count - 1; }

        //Equip weapon (except for skull)
        if (currentWeapon != skullParent && holdingSkull)
        {
            holdingSkull = false;

            //Put away skulls before equiping weapon.
            skullParent.transform.GetChild(0).gameObject.SetActive(false);
            //Equip weapon
            currentWeapon.SetActive(true);
        }
        else if (currentWeapon != skullParent)
        {
            currentWeapon.SetActive(true); //show held weapon 
        }

        //Equip skull
        if (currentWeapon == skullParent)
        {
            skullParent.transform.GetChild(0).gameObject.SetActive(true); //Make skull visible
            holdingSkull = true;
        }
    }


    //Put weapon in inventory:
    public void UnequipWeapon()
    {
        isEquipped = false;

        if (weaponNumber < 0) { weaponNumber = 0;  }
        
        if (currentWeapon != skullParent)
        {
            currentWeapon.SetActive(false); //hide held weapon
            inInventory = true;
        }
        else
        {
            if (holdingSkull)
            {
                skullParent.transform.GetChild(0).gameObject.SetActive(false); //hide the skull
                holdingSkull = false;
                inInventory = true;
            }
        }
    }
}
