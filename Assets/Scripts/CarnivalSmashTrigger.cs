using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalSmashTrigger : MonoBehaviour
{
    public static CarnivalSmashTrigger Instance;

    public WhackEmGameManager whackemGM;

    [SerializeField] GameObject rulesUI;
    [SerializeField] int ticketCost = 1;
    //[SerializeField] bool buttonPressed;
    public Transform gameplayPosition;
    public Transform player;

    [HideInInspector] public float triggerDistance;
    [HideInInspector] public float distanceFromGame;
    public GameObject prompt;
    public Menu menu;
    bool gameRulesOn;

    [Space(15)]
    public GameObject gameWeapon;
    public GameObject playerWeapon;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        menu = GameObject.FindObjectOfType<Menu>();
        whackemGM.currentWeapon = playerWeapon;

        //Set the weapon in the manager
        WhackEmGameManager.Instance.gameWeapon = gameWeapon;
        WhackEmGameManager.Instance.playerWeapon = playerWeapon;
    }


    private void Update()
    {
        //Detect if joystick or keyboard is used and SET the correct prompt variable.
        if (menu.usingJoystick)
        {
            prompt = menu.controllerPrompt; //If a controller is detected set prompt for controller
        }
        else
        {
            prompt = menu.keyboardPrompt; //If controller not detected set prompt for keyboard
        }

        //if (player.GetComponent<WeaponEquip>().whackEmActive)
        //{
        //    //Hide cursor again
        //    Cursor.lockState = CursorLockMode.Confined;
        //    Cursor.visible = true;
        //}

        //find distance between the player and game booth
        distanceFromGame = Vector3.Distance(player.transform.position, this.transform.position);

        if(distanceFromGame <= triggerDistance && player.position != gameplayPosition.position && !whackemGM.gameWon)
        {
            if (!gameRulesOn)
            {
                //Display action prompt when near an interactive booth.
                prompt.SetActive(true);
            }

            if (Input.GetButton("ActionButton") && !whackemGM.gameWon)
            {
                ShowGameUI();
            }
        } 
    }

    private void ShowGameUI()
    {
        gameRulesOn = true;
        //Turn off the prompt
        //prompt.SetActive(false);


        //If controller type is keyboard give mouse control
        if (!Menu.Instance.usingJoystick)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }


        if (!whackemGM.gameWon)
        {
            //Only show the rules screen if player has not picked up the mallet
            if (!whackemGM.weaponEquip.haveMallet && !whackemGM.gameJustFinished)
            {
                //Display game rules screen with play buttons
                rulesUI.SetActive(true);
            }
            else
            {
                player.GetComponent<WeaponEquip>().gameRulesDisplayed = false;
            }
        }

        //Lock camera movement
        if (!whackemGM.gameOn)
        {
            FPSController.Instance.canMove = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //* Set a timer for the player to come back into the trigger and play the game.
        //* If counter reaches 0, player loses their ticket and will be charged again.

        if (other.CompareTag("Player"))
        {
            //Reset bools for a new game;
            whackemGM.gameOn = false;
            whackemGM.gameOver = false;
            whackemGM.gameJustFinished = false;
            //buttonPressed = false;
        }
    }
    public void LockPlayerOnPlay()
    {
        //Unlock player camera movement, put player in position, lock player body movement
        FPSController.Instance.canMove = true;
        player.position = gameplayPosition.position;
        FPSController.Instance.GetComponent<CharacterController>().enabled = false;
    }

    public void UnLockPlayer()
    {
        //Hide the cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Allow player to walk again
        FPSController.Instance.GetComponent<CharacterController>().enabled = true;
        FPSController.Instance.canMove = true;
    }

    //Pay the cost to play the game
    public void PlayGame()
    {
        whackemGM.gameOn = true;
        //buttonPressed = true;
        gameRulesOn = false;

        LockPlayerOnPlay();

        //Turn off the game rules screen
        rulesUI.SetActive(false);

        //Spend the required ticket cost for the game
        HudManager.Instance.HealthTicket(ticketCost);

        //gameWeapon.SetActive(false); //Hides weapon in scene
        playerWeapon.SetActive(true); //Shows player holding weapon

        //player.GetComponent<WeaponEquip>().PickUpWeapon();
    }

    public void LeaveGame()
    {
        gameRulesOn = false;

        //Hide the game rules UI
        rulesUI.SetActive(false);

        //Unlock player movement
        UnLockPlayer();
    }
}
