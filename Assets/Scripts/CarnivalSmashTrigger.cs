using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalSmashTrigger : MonoBehaviour
{
    public WhackEmGameManager whackemGM;

    [SerializeField] GameObject rulesUI;
    [SerializeField] int ticketCost = 1;
    [SerializeField] bool buttonPressed;
    public Transform gameplayPosition;
    public Transform player;

    [SerializeField] public float triggerDistance;
    public float distanceFromGame;
    public GameObject prompt;
    public Menu menu;
    

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        menu = GameObject.FindObjectOfType<Menu>();
    }


    private void Update()
    {
        if (buttonPressed)
        {
            //Hide cursor again
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        //find distance between the player and game booth
        distanceFromGame = Vector3.Distance(player.transform.position, this.transform.position);

        if(distanceFromGame <= triggerDistance && player.position != gameplayPosition.position)
        {

            // pull up button
            // if button pressed, then bring up UI
            if (menu.usingJoystick)
            {
                prompt = menu.controllerPrompt; //If a controller is detected set prompt for controller
            }
            else
            {
                prompt = menu.keyboardPrompt; //If controller not detected set prompt for keyboard
            }

            prompt.SetActive(true);
            if (Input.GetButton("ActionButton") && !whackemGM.gameWon){
                ShowGameUI();
            }
        } 
    }

    private void ShowGameUI()
    {
        if (!whackemGM.gameWon)
        {
            //Only show the rules screen if player has not picked up the mallet
            if (!whackemGM.weaponEquip.haveMallet && !whackemGM.gameJustFinished)
            {
                //Display game rules screen with play buttons
                rulesUI.SetActive(true);
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
            whackemGM.gameOn = false;
            whackemGM.gameOver = false;
            whackemGM.gameJustFinished = false;
            buttonPressed = false;
        }
    }

    //Pay the cost to play the game
    public void PlayGame()
    {
        whackemGM.gameOn = true;
        buttonPressed = true;

        //Unlock player camera movement, put player in position, lock player body movement
        FPSController.Instance.canMove = true;
        FPSController.Instance.GetComponent<CharacterController>().enabled = false;
        player.position = gameplayPosition.position;

        //Turn off the game rules screen
        rulesUI.SetActive(false);

        //Spend the required ticket cost for the game
        TicketManager.Instance.SpendTicket(ticketCost);


        //* When game is played, make mallet appear in player hands.
        //* If game is lost, mallet disappears.
        //* If game is won, mallet stays in inventory.
    }

    public void LeaveGame()
    {
        //Hide the cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Hide the game rules UI
        rulesUI.SetActive(false);

        //Unlock player movement
        FPSController.Instance.canMove = true;
    }
}
