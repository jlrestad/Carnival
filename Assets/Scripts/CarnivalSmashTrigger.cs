using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalSmashTrigger : MonoBehaviour
{
    public WhackEmGameManager whackemGM;

    [SerializeField] GameObject rulesUI;
    [SerializeField] int ticketCost = 1;
    [SerializeField] bool buttonPressed;
   
    private void OnTriggerEnter(Collider other)
    {
        //Keep the rulesUI from popping on
        if (!whackemGM.weaponEquip.haveMallet)
        {
            //* Display game rules screen with play buttons
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
            Cursor.lockState = CursorLockMode.None;
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
        whackemGM.gameOn = true;
        buttonPressed = true;
        
        if (buttonPressed)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        //Turn off the game rules screen
        rulesUI.SetActive(false);

        //Spend the required ticket cost for the game
        TicketManager.Instance.SpendTicket(ticketCost);

        //Unlock player camera movement
        FPSController.Instance.canMove = true;
    }
}
