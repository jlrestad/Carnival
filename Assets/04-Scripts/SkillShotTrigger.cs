using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShotTrigger : MonoBehaviour
{
    public SkillShotGameManager skillshotGM;

    [SerializeField] GameObject rulesUI;
    [SerializeField] int ticketCost = 1;
    //[SerializeField] bool buttonPressed;
    public Transform gameplayPosition;
    public Transform player;
    WeaponEquip WE;
    public int maxDistance = 4;


    //[SerializeField] public float triggerDistance;
    [HideInInspector] public float distanceFromGame;
    public GameObject prompt;
    public Menu menu;

    public bool gameRulesOn;
    private MovingTarget[] movingTargets;
        
    public GameObject gameWeapon;
    public GameObject playerWeapon;


    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        menu = GameObject.FindObjectOfType<Menu>();
        WE = player.GetComponent<WeaponEquip>();
        skillshotGM.currentWeapon = playerWeapon;

        //Set the weapon in the manager
        SkillShotGameManager.Instance.gameWeapon = gameWeapon;
        SkillShotGameManager.Instance.playerWeapon = playerWeapon;
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

        movingTargets = FindObjectsOfType<MovingTarget>();

        //if (player.GetComponent<WeaponEquip>().skillshotActive)
        //{
        //    //Hide cursor again
        //    Cursor.lockState = CursorLockMode.Confined;
        //    Cursor.visible = false;

        //}

        //Find distance between player and gamebooth
        distanceFromGame = Vector3.Distance(player.transform.position, this.transform.position);

        if (distanceFromGame <= maxDistance /*&& player.position != gameplayPosition.position*/ && !skillshotGM.gameWon)
        {
            //Debug.Log("Entered skillshot area");

            if (!gameRulesOn)
            {
                //Display action prompt when near an interactive booth.
                prompt.SetActive(true);
            }
            
            if (Input.GetButton("ActionButton") && !skillshotGM.gameWon)
            {
                ShowGameUI();
            }
        }
    }

    private void ShowGameUI()
    {
        //Turn off the prompt
        //WE.actionPrompt.SetActive(false);
        prompt.SetActive(false); //**

        //Show the game rules
        gameRulesOn = true;


        if (!skillshotGM.gameWon)
        {
            //Only show the rules screen if player has not picked up the gun
            if (!skillshotGM.weaponEquip.haveGun && !skillshotGM.gameJustPlayed)
            {
                //Display game rules screen with play buttons
                rulesUI.SetActive(true);
                FPSController.Instance.canMove = false;
            }
            else
            {
                rulesUI.SetActive(false);
                FPSController.Instance.canMove = true;
            }

            //If controller type is keyboard give mouse control
            if (!menu.usingJoystick)
            {
                Cursor.lockState = CursorLockMode.Confined;
                Cursor.visible = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //* Set a timer for the player to come back into the trigger and play the game.
        //* If counter reaches 0, player loses their ticket and will be charged again.

        if (other.CompareTag("Player"))
        {
            //Reset bools for a new game;
            skillshotGM.gameOn = false;
            skillshotGM.gameOver = false;
            skillshotGM.gameJustPlayed = false;
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
        //reset targets
        skillshotGM.gameOn = true;
        //buttonPressed = true;
        gameRulesOn = false;

        LockPlayerOnPlay();

        //Turn off the game rules screen
        rulesUI.SetActive(false);

        //Spend the required ticket cost for the game
        HudManager.Instance.HealthTicket(ticketCost);

        //gameWeapon.SetActive(false); //Hide weapon in scene
        playerWeapon.SetActive(true); //Show player holding weapon

        //player.GetComponent<WeaponEquip>().PickUpWeapon();


        //*
        foreach (MovingTarget movingTarget in movingTargets)
        {
            //Debug.Log("reset");
            movingTarget.ResetTargets();
        }
    }

    public void LeaveGame()
    {
        gameRulesOn = false;

        //Hide the game rules UI
        rulesUI.SetActive(false);

        UnLockPlayer();
    }

}
