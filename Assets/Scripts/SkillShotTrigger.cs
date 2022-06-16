using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShotTrigger : MonoBehaviour
{
    public SkillShotGameManager skillshotGM;

    [SerializeField] GameObject rulesUI;
    [SerializeField] int ticketCost = 1;
    [SerializeField] bool buttonPressed;
    public Transform gameplayPosition;
    public Transform player;


    [SerializeField] public float triggerDistance;
    [HideInInspector] public float distanceFromGame;
    public GameObject prompt;
    public Menu menu;

    bool gameRulesOn;
    private MovingTarget[] movingTargets;
        
    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        //prompt = player.GetComponent<WeaponEquip>().actionPrompt;
        menu = GameObject.FindObjectOfType<Menu>();
    }

    private void Update()
    {
        //if (prompt == null)
        //{
        //    prompt = player.GetComponent<WeaponEquip>().actionPrompt;
        //}

        movingTargets = FindObjectsOfType<MovingTarget>();
        if (buttonPressed)
        {
            //Hide cursor again
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;

        }

        //Find distance between player and gamebooth
        distanceFromGame = Vector3.Distance(player.transform.position, this.transform.position);

        if (distanceFromGame <= triggerDistance && player.position != gameplayPosition.position && !skillshotGM.gameWon)
        {
            //Debug.Log("Entered skillshot area");

            if (!gameRulesOn)
            {
                //Display action prompt when near an interactive booth.
                player.GetComponent<WeaponEquip>().actionPrompt.SetActive(true);
            }
            
            if (Input.GetButton("ActionButton") && !skillshotGM.gameWon)
            {
                Debug.Log("E Pressed");
                //Bring up the game rule UI
                ShowGameUI();
            }
        }
    }

    private void ShowGameUI()
    {
        //Show the game rules
        gameRulesOn = true;
        //Turn off the prompt
        prompt.SetActive(false);

        if (!skillshotGM.gameWon)
        {
            //Only show the rules screen if player has not picked up the gun
            if (!skillshotGM.weaponEquip.haveGun && !skillshotGM.gameJustPlayed)
            {
                //Display game rules screen with play buttons
                rulesUI.SetActive(true);
            }
            else
            {
                player.GetComponent<WeaponEquip>().gameRulesDisplayed = false;
            }

            //Lock player camera movement until a button is pressed
            if (!buttonPressed)
            {
                FPSController.Instance.canMove = false;
            }

            //If controller type is keyboard give mouse control
            if (!Menu.Instance.usingJoystick)
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
            buttonPressed = false;
        }
    }

    //Pay the cost to play the game
    public void PlayGame()
    {
        //reset targets
        skillshotGM.gameOn = true;
        buttonPressed = true;
        gameRulesOn = false;

        //Unlock player camera movement
        FPSController.Instance.canMove = true;
        //Disable character controller so that player can't walk.
        FPSController.Instance.GetComponent<CharacterController>().enabled = false;
        //Put player in the play-position.
        player.position = gameplayPosition.position;

        //Turn off the game rules screen
        rulesUI.SetActive(false);

        //Spend the required ticket cost for the game
        HudManager.Instance.HealthTicket(ticketCost);

        foreach (MovingTarget movingTarget in movingTargets)
        {
            //Debug.Log("reset");
            movingTarget.ResetTargets();
        }
        //* When game is played, make mallet appear in player hands.
        //* If game is lost, mallet disappears.
        //* If game is won, mallet stays in inventory.
    }

    public void LeaveGame()
    {
        gameRulesOn = false;

        //Hide the cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Hide the game rules UI
        rulesUI.SetActive(false);

        //Unlock player movement
        FPSController.Instance.canMove = true;
    }

}
