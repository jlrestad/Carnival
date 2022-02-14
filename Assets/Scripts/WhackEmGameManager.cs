using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WhackEmGameManager : MonoBehaviour
{
    public GameObject[] critters;
    public GameObject whackemEnemy;
    [SerializeField] private float timeCounter = 30; //used to count down the time
    private float timeLeft; //used to set the amount of time to countdown by

    [HideInInspector] public bool gameOn;
    [HideInInspector] public bool popUp;
    [HideInInspector] public bool critterIsVisible;
    [HideInInspector] public bool gameIsRunning;
    [HideInInspector] public int tickets;
    [HideInInspector] public int score; //the player kills
    [SerializeField] int scoreLimit = 3; //the amount needed to win

    [Header("Enemy Appear speed")]
    public float minSpeed = 2.0f; public float maxSpeed = 3.0f; //bigger nums = slower speed

    [Header("UI")]
    public GameObject ticketsUI;
    public GameObject scoreUI;
    public GameObject timerUI;
    public GameObject winloseUI;
    [HideInInspector] public TextMeshProUGUI ticketsText;
    [HideInInspector] public TextMeshProUGUI scoreText;
    [HideInInspector] public TextMeshProUGUI timerText;
    [HideInInspector] public TextMeshProUGUI winloseText;

    WhackEmEnemy whackemScript;
    float randomPopUpTime;
    float randomStayTime;
    bool levelLoaded;
    int randomEnemy;
    IEnumerator beginPopUp;


    private void Awake()
    {
        levelLoaded = true;
    }

    private void Start()
    {
        //Set coroutine variable so it can easily be stopped.
        beginPopUp = EnemyPopUp();
        //Text
        ticketsText = ticketsUI.GetComponentInChildren<TextMeshProUGUI>();
        scoreText = scoreUI.GetComponentInChildren<TextMeshProUGUI>();
        timerText = timerUI.GetComponentInChildren<TextMeshProUGUI>();
        winloseText = winloseUI.GetComponentInChildren<TextMeshProUGUI>();
        //Tickets
        tickets = 3;
        ticketsText.text = ("Tickets: " + tickets);
        scoreText.text = (score + "/" + scoreLimit);
        //Timer
        timeLeft = timeCounter;

        whackemScript = whackemEnemy.GetComponent<WhackEmEnemy>();


        //Coroutine will start but wait until gameOn is true to begin.
        StartCoroutine(EnemyPopUp());
    }

    private void Update()
    {
        //Run this when the WhackEm game is on.
        if (gameOn)
        {
            //Display the game UI
            DisplayUI();

            //Begin the game timer
            StartCoroutine(CountDownTimer());

            //Display win or lose
            WinLoseManager();

            //Update ticket count
            ticketsText.text = ("Tickets: " + tickets);

            if (tickets < 0)
            {
                tickets = 0;
                ticketsText.text = "NEED TICKETS";

                //Ticket is needed in order to play...
                gameOn = false;
            }
        }
        else
        {
            scoreUI.SetActive(false);
        }
    }

    public void DisplayUI()
    {
        //Display the scoreUI
        scoreUI.SetActive(true);
        scoreText.text = (score + "/" + scoreLimit);

        //Display the timerUI
        timerUI.SetActive(true);
        if (timeLeft >= 10)
        {
            timerText.text = ("00:" + (int)timeLeft);
        }
        else
        {
            timerText.text = ("00:0" + (int)timeLeft);
        }

        //Display win/lose UI
        winloseUI.SetActive(true);
    }

    public void ResetGame()
    {
        //Score
        score = 0;
        scoreText.text = (score + "/" + scoreLimit);

        //Speed
        minSpeed = 2.0f;
        maxSpeed = 3.0f;

        //winloseUI
        winloseUI.SetActive(false);
        winloseText.text = (" ");

        //Time
        timeLeft = timeCounter;
        if (timeLeft >= 10)
        {
            timerText.text = ("00:" + (int)timeLeft);
        }
        else
        {
            timerText.text = ("00:0" + (int)timeLeft);
        }
    }

    void WinLoseManager()
    {
        if (score >= scoreLimit && timeLeft > 0)
        {
            //WIN!
            //Get tickets
            //Add card to inventory
            winloseText.text = "You have won...";
        }
        else if (score < scoreLimit && timeLeft <= 0)
        {
            //Lose!
            winloseText.text = "You have lost...";
        }
    }

    IEnumerator CountDownTimer()
    {
        //Wait for 1 second so that the starting number is displayed.
        yield return new WaitForSeconds(1);

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            //Stop Coroutine enemypopup
            StopCoroutine(beginPopUp);
        }
    }

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
                    randomStayTime = UnityEngine.Random.Range(minSpeed * 1.5f, maxSpeed * 1.5f);
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
                        //Start the coroutine over. (not sure this is doing anything..)
                        StartCoroutine(EnemyPopUp());
                    }
                }
            }
            yield return null;
        }
    }   
}
