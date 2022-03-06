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
            StartCoroutine(GameStartDelay());
            TicketManager.Instance.tickets -= 1;
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

    //Gives the player time to prepare for the enemies.
    //This time could also be used to play a sound that lets the player know the game has started.
    IEnumerator GameStartDelay()
    {
        yield return new WaitForSeconds(skillshotGM.gameDelayTime);
        skillshotGM.gameOn = true;
    }
}
