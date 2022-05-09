using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    [TextArea]
    [SerializeField] string notes;

    public static WeaponEquip Instance;

    [Header("UI")]
    public Canvas crossHair;
    public GameObject actionPrompt;
    public List<GameObject> weaponCardBG;

    [Space(15)]
    [SerializeField] GameObject gunHold;
    [SerializeField] GameObject malletHold;
    public GameObject skullsParent; //This holds the skulls (now changed to 1 infinite skull)
    public GameObject skullHold; //This identifies the weapon
    [HideInInspector] public int addToCount;
    //public int addSkull;

    [Space(15)]
    public List<GameObject> weaponList = new List<GameObject>(); //Holds weapons

    [Space(15)]
    public int weaponNumber;

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
    [SerializeField] Collider skullCollider;
    [SerializeField] Rigidbody skullRB;

    [Space(2)]
    /*[HideInInspector] */public bool haveGun, haveMallet, haveSkull, holdingSkull;

    public bool whackEmActive = false;
    public bool skillshotActive = false;

    [Space(15)]
    public string gameName;
    private Weapon newWeapon;
    private Head newSkull;
    public Head[] headSkull;
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
        skullHold = GameObject.Find("SkullHold");
        //weaponCardBG = menu.gameCardBG; //Used to highlight which weapon is equipped

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
        ChangeWeapon();

        //FOR SKULL PICKUP * * *
        //if (gameObject.CompareTag("Head"))
        //{
        //    //Get Head.cs from the closest skull
        //    head = gameObject.GetComponent<Head>();
        //    currentWeapon = skullsParent;
        //    haveSkull = true;
        //}

        if (holdingSkull)
        {
            //Add skulls to weapon list.
            if (!weaponList.Contains(skullsParent))
            {
                weaponList.Add(skullsParent);
            }
        }
    //}

        //* Don't need anymore since skulls are infinite **
        //Check if skull parent is empty
        //if (skullsParent.transform.childCount <= 0 && weaponList.Contains(skullsParent))
        //{
        //    haveSkull = false; //Out of skulls
        //    holdingSkull = false; //Not holding skull

        //    //Reset the hold count UI
        //    addToCount = 0;
        //    menu.skullCountText.text = skullsParent.transform.childCount.ToString();

        //    //Remove the weapon from the list
        //    weaponList.Remove(skullsParent);

        //    //Out of skulls but have other weapons.
        //    if (haveMallet && !isEquipped || haveGun && !isEquipped)
        //    {
        //        //Set the current weapon to the first in the list.
        //        currentWeapon = weaponList[0];
        //        inInventory = true;
        //    }
        //}

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

                //Turn off weapon when flashlight is on
                //if (currentWeapon != skullsParent)
                //{
                //    currentWeapon.SetActive(false);
                //}
                //else
                //{
                //    skullsParent.transform.GetChild(0).gameObject.SetActive(false);
                //}

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
        

        // * * *
        //PLAYER INPUT
        if (distanceToPlayer.sqrMagnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !haveGun && closestWeapon.CompareTag("Gun") ||
            distanceToPlayer.sqrMagnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !haveMallet && closestWeapon.CompareTag("Mallet") ||
            distanceToPlayer.sqrMagnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && closestWeapon.CompareTag("Head"))
        {
            //If there is already a weapon equipped, hide it.
            //if (isEquipped && holdingSkull && !closestWeapon.CompareTag("Head"))
            //{
            //    skullsParent.transform.GetChild(0).gameObject.SetActive(false);
            //    holdingSkull = false;
            //}
            //else if (isEquipped)
            //{
            //    currentWeapon.SetActive(false);
            //}

            //Pick up and equip weapon.
            PickUpWeapon();
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

        if (!closestWeapon.CompareTag("Head") /*&& !closestWeapon.CompareTag("Untagged")*/)
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
    //}

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

            //    if (_closestWeapon == null)
              //  {
                    _closestWeapon = closestWeapon;
                //}
                //if (currentWeapon == null)
                //{
                //    currentWeapon = closestWeapon;
                //}

                distanceToPlayer = transform.position - closestWeapon.transform.position; //used later to determine distance to pick up weapon

                //If the closest weapon is a skull, then get the collider and rigidbody of that skull.
                //if (closestWeapon.CompareTag("Head"))
                //{
                //    skull = closestWeapon;
                    
                //    skullCollider = skull.GetComponent<Collider>();
                //    skullRB = skull.GetComponent<Rigidbody>();
                //}

                //* This was used for loading scenes, which we aren't using now, but kept it incase... Can delete at the end of project.
                //Get the name of the layer -- which is the name of the game level
                int layerNumber = closestWeapon.layer;
                gameName = LayerMask.LayerToName(layerNumber);

                //gameBooth = GameObject.FindGameObjectWithTag(levelName);
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
            //Unequip current weapon.
            if (weaponList.Count > 1 && currentWeapon != skullsParent)
            {
                //If there is already a weapon equipped, hide it.
                currentWeapon.SetActive(false);
            }
            if (weaponList.Count > 1 && currentWeapon == skullsParent)
            {
                //Hide the child of skulls parent, not the parent (which is the current weapon) so that more skulls may be collected.
                //skullsParent.transform.GetChild(0).gameObject.SetActive(false);
                skullsParent.SetActive(false);
                 holdingSkull = false;
            }

            //Move to the next weapon in the list.
            weaponNumber++;

            //Check bounds of weapon number.
            if (weaponNumber > weaponList.Count - 1)
            {
                //Reset back to the beginning of the list.
                weaponNumber = 0;
            }

            //Change current weapon to the next weapon in the list.
            currentWeapon = weaponList[weaponNumber];

            //Equip the weapon
            if (gameObject.CompareTag("Head"))
            {
                //Equip skull
                skullsParent.SetActive(true);
                holdingSkull = true;
            }
            else
            {
                //Equip other weapon
                currentWeapon.SetActive(true); //show the weapon
                holdingSkull = false;
            }
        }

        //SCROLL WHEEL BACKWARD
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && isEquipped || Input.GetButtonDown("WeaponScroll-") && isEquipped)
        {
            //Unequip current weapon.
            if (weaponList.Count > 1 && currentWeapon != skullsParent)
            {
                //If there is already a weapon equipped, hide it.
                currentWeapon.SetActive(false);
            }
            if (weaponList.Count > 1 && currentWeapon == skullsParent)
            {
                //Hide the child of skulls parent, not the parent (which is the current weapon) so that more skulls may be collected.
                //skullsParent.transform.GetChild(0).gameObject.SetActive(false);
                skullsParent.SetActive(false); //Put in inventory.
                holdingSkull = false;
            }

            //Move to the previous weapon in the list.
            weaponNumber--;

            //Check bounds of weapon number.
            if (weaponNumber < 0)
            {
                //Set the weapon number to the highest number.
                weaponNumber = weaponList.Count - 1;
            }

            //Change current weapon to the previous weapon in the list.
            currentWeapon = weaponList[weaponNumber];

            //Equip the weapon
            if (currentWeapon == skullsParent)
            {
                //Equip skull
                skullsParent.SetActive(true);
                //skullHold.SetActive(false);
                holdingSkull = true;
            }
            else
            {
                //Equip other weapon
                currentWeapon.SetActive(true);
                holdingSkull = false;
            }
            
        }
    }


    /// ...EQUIP WEAPONS SECTION... ///

    public void PickUpWeapon()
    {
        isEquipped = true;
        // Check which weapon it is and set it to the current weapon.

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
        //SKULL
        if (gameObject.CompareTag("Head"))
        {
            //Get Head.cs from the closest skull
            head = gameObject.GetComponent<Head>();
            currentWeapon = skullsParent;
            haveSkull = true;
        }
        //* Finds the closest skull, but can't equip the closest -- not updating.
        //if (gameObject.CompareTag("Head"))
        //{
        //    skull = closestWeapon;
        //    //head = skull.GetComponent<Head>();

        //    currentWeapon = skullsParent;
        //    haveSkull = true;
        //    //After picking up the weapon, equip it.
        //    //head.PickUpSkull();
        //    //EquipWeapon(); //picked up weapon is equipped
        //}
    }

    // Bring weapon out of inventory:
    void EquipWeapon()
    {
        inInventory = false;
        isEquipped = true;
        weaponNumber++;
        if (weaponNumber > weaponList.Count) { weaponNumber = weaponList.Count - 1; }

        //For BG of weapon card
        if (menu.cardImage.name.Equals(currentWeapon.tag))
        {
            Debug.Log("These are the same name");
        }

        //Hide flashlight if holding
        //if (GetComponent<FPSController>().flashlightOn)
        //{
        //    GetComponent<FPSController>().flashlightHold.SetActive(false);
        //    GetComponent<FPSController>().flashlightOn = false;
        //}

        //Equip weapon (except for skull)
        if (currentWeapon != skullsParent && holdingSkull)
        {
            holdingSkull = false;

            //Put away skulls before equiping weapon.
            skullsParent.SetActive(false);
            //skullHold.SetActive(false);

            currentWeapon.SetActive(true); //show held weapon 
        }
        else if (currentWeapon != skullsParent)
        {
            currentWeapon.SetActive(true); //show held weapon 
        }

        //Equip skull
        //if (currentWeapon == skullsParent)
        if (closestWeapon.CompareTag("Head"))
        {
            //head.PickUpSkull();

            //Debug.Log("This compare tag works");
            ////Allow only 6 skulls
            ////to be held.
            //if (skullsParent.transform.childCount < 6)
            //{
            //    holdingSkull = true;

            //    //Get the collider and rigidbody of the skull being held.
            //    skullCollider = skull.GetComponent<Collider>();
            //    skullRB = skull.GetComponent<Rigidbody>();

            //    //* This is set in FindClosestWeapon.
            //    //Disable collider and set rigidbody to kinematic so it can be pick up and thrown
            //    skullCollider.enabled = false;
            //    skullRB.isKinematic = true;

            //    //Add skull to the skull hold position on FPSPlayer.
            //    skull.transform.position = skullsParent.transform.position;
            //    skull.transform.parent = skullsParent.transform;

            //    //Show only the skull that is being held
            //    skullsParent.transform.GetChild(0).gameObject.SetActive(true);

            //    //Put skull in inventory (hide it from scene)
            //    for (int i = 1; i < skullsParent.transform.childCount; i++)
            //    {
            //        //Hide every skull that is not the 0th index
            //        skullsParent.transform.GetChild(i).gameObject.SetActive(false);
            //    }

            //Put skull in SkullHolder on FPSPlayer
        }

            //*Turn off Weapon script while holding so it won't be detected by FindClosestWeapon().
            //for (int i = 0; i < skullsParent.transform.childCount; i++)
            //{
            //    skullsParent.transform.GetChild(i).GetComponent<Weapon>().enabled = false;
            //}
            ////skull.tag = "Untagged"; //* temp set untagged so it wont be found as the closest weapon

            //if (skullsParent.transform.childCount != 0)
            //{
            //    //Show the skull count menu.
            //    menu.skullCountUI.SetActive(true);
            //    menu.skullCountText.text = addToCount.ToString();
            //}
            //else
            //{
            //    holdingSkull = false;
            //    haveSkull = false;
            //}
    }


    //Put weapon in inventory:
    public void UnequipWeapon()
    {
        inInventory = true;
        isEquipped = false;
        weaponNumber--;
        if (weaponNumber < 0) { weaponNumber = 0;  }
        
        if (currentWeapon != skullsParent)
        {
            currentWeapon.SetActive(false); //hide held weapon
        }
        else
        {
            //skullsParent.transform.GetChild(0).gameObject.SetActive(false); //hide the skull
            holdingSkull = false;
        }
    }

    //GameObject FindNearestSkull()
    //{
    //    foreach (Head thisSkull in headSkull)
    //    {
    //        float currentDist;
    //        float closestDist = Mathf.Infinity;
    //        currentDist = (transform.position - thisSkull.transform.position).sqrMagnitude;
    //        distanceToPlayer = transform.position - thisSkull.transform.position;

    //        if (currentDist < closestDist)
    //        {
    //            closestDist = currentDist;
    //            newSkull = thisSkull;
    //            closestSkull = newSkull.gameObject;
    //            skullRB = newSkull.GetComponent<Rigidbody>();
    //        }
    //        return closestSkull;
    //    }
    //    Debug.Log("Closest skull is " + closestSkull);
    //    return closestSkull;
    //}
}
