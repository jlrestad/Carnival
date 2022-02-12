using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WhackEmGameManager : MonoBehaviour
{
    public GameObject[] critters;
    public GameObject whackemEnemy;
    public bool gameOn;
    public bool popUp;
    public bool critterIsVisible;
    public bool gameIsRunning;
    public int tickets;
    public int score;
    int randomEnemy;
    public GameObject ticketsUI;
    public GameObject scoreUI;
    public TextMeshProUGUI ticketsText;
    public TextMeshProUGUI scoreText;

    private void Start()
    {
        tickets = 3;
        ticketsText.text = ("Tickets: " + tickets);
        scoreText.text = (score + "/3");
        //popUp = critters[randomEnemy].GetComponent<WhackEmEnemy>().isVis;
    }

    private void Update()
    {
        //Run this when the WhackEm game is on.
        if (gameOn)
        {
            scoreUI.SetActive(true);

            //Update ticket count
            ticketsText.text = ("Tickets: " + tickets);

            if (tickets <= 0)
            {
                tickets = 0;
                ticketsUI.SetActive(false);
            }

            StartCoroutine(EnemyPopUp());

            //Enemy pop-up: Iterate through the list of critters and run the coroutine only if it has not popped up yet.
     
        }
        else
        {
            scoreUI.SetActive(false);
        }
    }

    IEnumerator CountDown()
    {
        yield return null;
    }

    IEnumerator EnemyPopUp()
    {
        //Get random pop-up times
        int randomPopUpTime = UnityEngine.Random.Range(1, 2);
        int randomStayTime = UnityEngine.Random.Range(3, 5);
        int randomHideTime = UnityEngine.Random.Range(6, 10);

        //Get a random critter
        randomEnemy = UnityEngine.Random.Range(0, 9);
        critterIsVisible = critters[randomEnemy].GetComponent<WhackEmEnemy>().isVis;
        while (!popUp)
        {
            //Iterate through the array of enemies and check if it's visible or not.
            for (int i = 0; i < critters.Length; i++)
            {
                if (critterIsVisible)
                {
                    yield return 0;
                }
                if (!critterIsVisible)
                {
                    //Random critter appears at a random time.
                    yield return new WaitForSeconds(randomPopUpTime);
                    critters[randomEnemy].SetActive(true);
                    critterIsVisible = true; //set individual critter's visibility to true
                    popUp = critterIsVisible;

                    //Visible critter stays for random time before disappearing.
                    yield return new WaitForSeconds(randomStayTime);
                    critterIsVisible = false; //set individual critter's visibility to false
                    critters[randomEnemy].SetActive(false);
                    popUp = critterIsVisible;
                    yield return new WaitForSeconds(randomHideTime);
                }
            }
        }
    }   

}
