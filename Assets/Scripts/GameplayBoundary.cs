using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameplayBoundary : MonoBehaviour
{
    public WhackEmGameManager whackemGM;

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
            whackemGM.gameOn = false;
            whackemGM.gameOver = false;
        }
    }

    //Gives the player time to prepare for the enemies.
    //This time could also be used to play a sound that lets the player know the game has started.
    IEnumerator GameStartDelay()
    {
        yield return new WaitForSeconds(whackemGM.gameDelayTime);
        whackemGM.gameOn = true;
    }
}
