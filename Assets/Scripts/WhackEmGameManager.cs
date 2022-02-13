using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class WhackEmGameManager : MonoBehaviour
{
    public GameObject[] critters;
    public GameObject whackemEnemy;
    [HideInInspector] public bool gameOn;
    [HideInInspector] public bool popUp;
    [HideInInspector] public bool critterIsVisible;
    [HideInInspector] public bool gameIsRunning;
    [HideInInspector] public int tickets;
    [HideInInspector] public int score; //the player kills
    [HideInInspector] public int limit = 3; //the amount needed to win

    [Header("Enemy Appear speed")]
    public float minSpeed = 2.0f; public float maxSpeed = 3.0f; //bigger nums = slower speed

    [Header("UI")]
    public GameObject ticketsUI;
    public GameObject scoreUI;
    public TextMeshProUGUI ticketsText;
    public TextMeshProUGUI scoreText;

    WhackEmEnemy whackemScript;
    float randomPopUpTime;
    float randomStayTime;
    bool levelLoaded;
    int randomEnemy;


    private void Awake()
    {
        levelLoaded = true;
    }

    private void Start()
    {
        //Tickets
        tickets = 3;
        ticketsText.text = ("Tickets: " + tickets);
        scoreText.text = (score + "/" + limit);
        
        whackemScript = whackemEnemy.GetComponent<WhackEmEnemy>();

        StartCoroutine(EnemyPopUp());
    }

    private void Update()
    {
        //Run this when the WhackEm game is on.
        if (gameOn)
        {
            scoreUI.SetActive(true);
            scoreText.text = (score + "/" + limit);

            //Update ticket count
            ticketsText.text = ("Tickets: " + tickets);

            if (tickets <= 0)
            {
                tickets = 0;
                ticketsText.text = "NEED TICKETS";

                //Ticket is needed in order to play...
                gameOn = false;
            }

            if (score >= limit)
            {
                scoreText.text = "WIN!";
                //gameOn = false;
            }

            //if time runs out before score == limit then lose
        }
        else
        {
            scoreUI.SetActive(false);
        }
    }

    public void ResetScore()
    {
        //Score
        score = 0;
        scoreText.text = (score + "/" + limit);

        //Speed
        minSpeed = 2.0f;
        maxSpeed = 3.0f;
    }

    //IEnumerator CountDown()
    //{
    //    yield return null;
    //}

    IEnumerator EnemyPopUp()
    {
        while (levelLoaded) //Allow coroutine to load on Start.
        {
            while (gameOn) //But don't do anything until the game is on.
            {
                //Iterate through the array of enemies and check if it's visible or not.
                for (int i = 0; i < critters.Length; i++)
                {
                    //Random enemy
                    randomEnemy = UnityEngine.Random.Range(0, 9);
                    //Random speeds
                    randomStayTime = UnityEngine.Random.Range(0.5f, 3.0f);
                    randomPopUpTime = UnityEngine.Random.Range(minSpeed, maxSpeed);
                   
                    //Check through each enemy to see if it is visible
                    critterIsVisible = critters[i].GetComponent<WhackEmEnemy>().isVis;

                    //Check if the enemy is already visible.
                    if (!critterIsVisible)
                    {
                        //Random enemy appears at a random time.
                        yield return new WaitForSeconds(randomPopUpTime);
                        critters[randomEnemy].SetActive(true);
                        critterIsVisible = true; //set individual critter's visibility to true

                        //Visible enemy stays for random time before disappearing.
                        yield return new WaitForSeconds(randomStayTime);
                        critterIsVisible = false; //set individual critter's visibility to false
                        critters[randomEnemy].SetActive(false);
                    }
                    else
                    {
                        //Start the coroutine over.
                        StartCoroutine(EnemyPopUp());
                    }
                }
            }
            yield return null;
        }
    }   
}
