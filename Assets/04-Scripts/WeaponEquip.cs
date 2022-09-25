using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WeaponEquip : MonoBehaviour
{
    [TextArea]
    [SerializeField] string notes;

    public static WeaponEquip Instance;

    [Header("UI")]
    public GameObject crossHair;
    public GameObject actionPrompt;
    public List<GameObject> weaponCards;
    int BGCount;

    [Space(15)]
    public GameObject gunHold;
    public GameObject malletHold;
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
    public GameObject gameWeapon = null;
    [HideInInspector] public GameObject _closestWeapon = null;
    public GameObject currentWeapon = null;
    //public GameObject closestSkull = null;
    //[SerializeField] float pickUpRange = 1.5f;
    Vector3 distanceToPlayer;

    [Space(15)]
    public GameObject skull;
    //[SerializeField] Collider skullCollider;
    //[SerializeField] Rigidbody skullRB;

    [Space(2)]
    /*[HideInInspector] */
    public bool haveGun, haveMallet, haveSkull, holdingSkull;

    //public bool whackEmActive = false;
    //public bool skillshotActive = false;
    public bool gameMenuDisplayed;

    [Space(15)]
    public string gameName;
    private Weapon newWeapon;
    //Head head; //Get the Head script for skull

    [HideInInspector] public RaycastHit hit;
    [SerializeField] int maxHitDistance = 10;
    public Menu menu;
    //GameBooth gameBooth = new GameBooth();
    CasketBasketsGameManager CBManager;
    SkillShotGameManager SSManager;
    CarnivalSmashGameManager CSManager;


    private void Awake()
    {
        Instance = this;

        CBManager = GameObject.FindObjectOfType<CasketBasketsGameManager>();
        CSManager = GameObject.FindObjectOfType<CarnivalSmashGameManager>();
        SSManager = GameObject.FindObjectOfType<SkillShotGameManager>();

    }

    private void Start()
    {
        menu = FindObjectOfType<Menu>();
    }

    void Update()
    {
        GetActionPromptIcon();

        FindClosestWeapon();

        if (isEquipped && !CBManager.gameOn && !SSManager.gameOn && !CSManager.gameOn)
        {
            ChangeWeapon();
        }

        //RAYCAST
        DetectMiniGames();
        
    }

    void GetActionPromptIcon()
    {
        //Detect if joystick or keyboard is used and SET the correct prompt variable.
        if (menu.usingJoystick)
        {
            actionPrompt = menu.controllerPrompt; //If a controller is detected set prompt for controller
        }
        else
        {
            actionPrompt = menu.keyboardPrompt; //If controller not detected set prompt for keyboard
        }
    }

    public void DisplayCrossHair()
    {
        //CROSSHAIR/RETICLE
        if (isEquipped)
        {
            //Show crosshair only if weapon is equipped.
            crossHair.SetActive(true);
        }
        else
        {
            crossHair.SetActive(false);
        }
    }

    // 
    //DETECTS THE GAME, SHOWS THE PROMPT, AND ACTIVATES THE GAME RULES MENU WHEN ACTION BUTTON IS PRESSED.
    public void DetectMiniGames()
    {
        if (Physics.Raycast(transform.position, transform.forward, out hit, maxHitDistance))
        {
            Transform hitTransform = hit.transform;

            bool isSS = hitTransform.CompareTag("ShootingGame");
            bool isCS = hitTransform.CompareTag("MeleeGame");
            bool isCB = hitTransform.CompareTag("ThrowingGame");

            //Debug.Log(hitTransform.tag);
            //Debug.DrawRay(transform.position, transform.forward);

            //Find distance of the game to the player
            distanceToPlayer = (hitTransform.position - transform.position);

            if (!gameMenuDisplayed && distanceToPlayer.sqrMagnitude < maxHitDistance)
            {
                if (isCS && !CSManager.gameOn && !CSManager.isPaused)
                {
                    actionPrompt.SetActive(true);
                    crossHair.SetActive(true);

                    if (Input.GetButtonDown("ActionButton"))
                        CSManager.ShowGameRules();
                }
                else if (isCB && !CBManager.gameOn && !CBManager.isPaused)
                {
                    actionPrompt.SetActive(true);
                    crossHair.SetActive(true);

                    if (Input.GetButtonDown("ActionButton"))
                        CBManager.ShowGameRules();
                }
                else if (isSS && !SSManager.gameOn && !SSManager.isPaused)
                {
                    actionPrompt.SetActive(true);
                    crossHair.SetActive(true);

                    if (Input.GetButtonDown("ActionButton"))
                        SSManager.ShowGameRules();
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
            if (weaponList.Count > 1) //Don't scroll through weapons if 1 or no weapons are held.
                WeaponScrollPositive();
        }

        //SCROLL WHEEL BACKWARD
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0 && isEquipped || Input.GetButtonDown("WeaponScroll-") && isEquipped)
        {
            if (weaponList.Count > 1)
                WeaponScrollNegative();
        }
    }

    void WeaponScrollPositive()
    {
        //Turn off current weapon card
        weaponCards[weaponNumber].GetComponent<Image>().enabled = false;

        //Unequip current weapon.
        if (weaponList.Count > 0)
        {
            //If there is already a weapon equipped, hide it.
            currentWeapon.SetActive(false);
        }

        if (weaponList.Count >= 1)
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

        //Turn on next weapon card
        weaponCards[weaponNumber].GetComponent<Image>().enabled = true;

        //Change current weapon to the next weapon in the list.
        currentWeapon = weaponList[weaponNumber];

        //Equip the weapon
        if (currentWeapon == skullParent)
        {
            //Equip skull
            skullParent.SetActive(true); //Make the first pooled skull visible.
            skullParent.transform.GetChild(0).gameObject.SetActive(true);
            holdingSkull = true;
        }
        if (currentWeapon != skullParent)
        {
            //Equip other weapon
            currentWeapon.SetActive(true); //show the weapon
            holdingSkull = false;
        }
    }

    void WeaponScrollNegative()
    {
        //Turn off current weapon card
        weaponCards[weaponNumber].GetComponent<Image>().enabled = false;

        //Unequip current weapon.
        if (weaponList.Count > 0)
        {
            //If there is already a weapon equipped, hide it.
            currentWeapon.SetActive(false);
        }

        //** WEAPON NUMBER BOUNDS
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

        //Turn on previous weapon card
        weaponCards[weaponNumber].GetComponent<Image>().enabled = true;

        //Change current weapon to the previous weapon in the list.
        currentWeapon = weaponList[weaponNumber];

        //Equip the weapon
        if (currentWeapon == skullParent)
        {
            //Equip skull
            skullParent.SetActive(true); //Make the first pooled skull visible.
            skullParent.transform.GetChild(0).gameObject.SetActive(true);
            holdingSkull = true;
        }
        if (currentWeapon != skullParent)
        {
            //Equip other weapon
            currentWeapon.SetActive(true);
            holdingSkull = false;
        }
    }

    public void ChangeTarot()
    {
        //Need the weapon number in the list
        //int index = weaponList.IndexOf()
        //weaponCards[weaponNumber].GetComponent<Image>().enabled = true;
    }

    /// ...EQUIP WEAPONS SECTION... ///

    //If weapon is won, then sets the current weapon to the newly won weapon.
    //public void WinAndAssignWeapon()
    //{
    //    //GUN
    //    //if (closestWeapon.CompareTag("Gun") && !haveGun && skillshotActive)
    //    if (haveGun)
    //    {
    //        currentWeapon = gunHold;
    //    }

    //    //MALLET
    //    //if (closestWeapon.CompareTag("Mallet") && !haveMallet && whackEmActive)
    //    if (haveMallet)
    //    {
    //        currentWeapon = malletHold;
    //    }

    //    //SKULL
    //    if (haveSkull)
    //    {
    //        currentWeapon = skullParent;
    //    }
    //}

    // Bring weapon out of inventory:
    //void EquipWeapon()
    //{
    //    if (weaponNumber > weaponList.Count) { weaponNumber = weaponList.Count - 1; }

    //    //Equip weapon (except for skull)
    //    if (currentWeapon != skullParent && holdingSkull)
    //    {
    //        holdingSkull = false;

    //        //Put away skulls before equiping weapon.
    //        skullParent.transform.GetChild(0).gameObject.SetActive(false);

    //        //Equip weapon
    //        currentWeapon.SetActive(true);
    //    }
    //    else if (currentWeapon != skullParent)
    //    {
    //        currentWeapon.SetActive(true); //show held weapon 
    //    }

    //    //Equip skull
    //    if (currentWeapon == skullParent)
    //    {
    //        skullParent.transform.GetChild(0).gameObject.SetActive(true); //Make skull visible
    //        holdingSkull = true;
    //    }
    //}


    ////Put weapon in inventory:
    //public void UnequipWeapon()
    //{
    //    isEquipped = false;

    //    if (weaponNumber < 0) { weaponNumber = 0;  }
        
    //    if (currentWeapon != skullParent)
    //    {
    //        currentWeapon.SetActive(false); //hide held weapon
    //        inInventory = true;
    //    }
    //    else
    //    {
    //        if (holdingSkull)
    //        {
    //            skullParent.transform.GetChild(0).gameObject.SetActive(false); //hide the skull
    //            holdingSkull = false;
    //            inInventory = true;
    //        }
    //    }
    //}
}
