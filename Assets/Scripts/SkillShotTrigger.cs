using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillShotTrigger : MonoBehaviour
{
    public SkillShotGameManager skillshotGM;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            TicketManager.Instance.tickets -= 1;

            skillshotGM.gameOn = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Reset bools for a new game;
            skillshotGM.gameOn = false;
            skillshotGM.gameOver = false;
        }
    }

    
}
