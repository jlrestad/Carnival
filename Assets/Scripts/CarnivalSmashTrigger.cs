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

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }


    private void Update()
    {
        if (buttonPressed)
        {
            //Hide cursor again
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(!whackemGM.gameWon && other.CompareTag("Player"))
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
