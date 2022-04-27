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
        //Only show the rules screen if player has not picked up the mallet
        if (!whackemGM.weaponEquip.haveMallet)
        {
            //Display game rules screen with play buttons
            rulesUI.SetActive(true);
        }

        //Lock player camera movement until a button is pressed
        if (!buttonPressed)
        {
            FPSController.Instance.canMove = false;
            FPSController.Instance.GetComponent<CharacterController>().enabled = false;
        }

        //If controller type is keyboard give mouse control
        if (!Menu.Instance.usingJoystick)
        {
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Reset bools for a new game;
            whackemGM.gameOn = false;
            whackemGM.gameOver = false;
        }
    }

    //Pay the cost to play the game
    public void PlayGame()
    {
        //Hide the cursor
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        whackemGM.gameOn = true;
        buttonPressed = true;

        //Unlock player camera movement
        FPSController.Instance.canMove = true;
        player.position = gameplayPosition.position;

        //* This will need to be turned back on after the game is over.
        //FPSController.Instance.GetComponent<CharacterController>().enabled = true;

        //Turn off the game rules screen
        rulesUI.SetActive(false);

        //Spend the required ticket cost for the game
        TicketManager.Instance.SpendTicket(ticketCost);

        WeaponEquip.Instance.whackEmActive = true;
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
