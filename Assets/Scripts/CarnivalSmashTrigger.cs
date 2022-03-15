using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarnivalSmashTrigger : MonoBehaviour
{
    public WhackEmGameManager whackemGM;
   
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TicketManager.Instance.tickets -= 1;

            whackemGM.gameOn = true;
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
}
