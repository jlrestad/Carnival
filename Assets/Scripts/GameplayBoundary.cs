using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class GameplayBoundary : MonoBehaviour
{
    //[SerializeField] string levelName;
    //[SerializeField] GameCardManager cardManager;
    //GameObject game;
    //[SerializeField] GameObject gamePrefab;
    //[SerializeField] GameObject gameSpawn;
    public WhackEmGameManager whackemGM;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            whackemGM.gameOn = true;
            whackemGM.tickets = whackemGM.tickets - 1;
            //game = Instantiate(gamePrefab, gameSpawn.transform);
            //game.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            whackemGM.gameOn = false;
            whackemGM.ResetGame();
            //game.SetActive(false);
        }
    }
}
