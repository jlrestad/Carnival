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
    public int tickets = 3;
    [SerializeField] private float timeCounter = 30; //used to count down the time
    private float timeLeft; //used to set the amount of time to countdown by
    private float resetTime;

    [HideInInspector] public bool gameOn;
    [HideInInspector] public bool popUp;
    [HideInInspector] public bool critterIsVisible;
    [HideInInspector] public bool gameIsRunning;
    [HideInInspector] public int score; //the player kills
    [SerializeField] int scoreLimit = 3; //the amount needed to win

    //bigger nums = slower speed
    [Header("Enemy Appear speed")]
    public float minRando; private float minRandoTemp; 
    public float maxRando; private float maxRandoTemp;
    public float divideSpeedBy; //The amount that the random number is divided by when enemy has been hit.

    [Header("UI")]
    public GameObject ticketsUI;
    public GameObject scoreUI;
    public GameObject timerUI;
    public GameObject winloseUI;
    [HideInInspector] public TextMeshProUGUI ticketsText;
    [HideInInspector] public TextMeshProUGUI scoreText;
    [HideInInspector] public TextMeshProUGUI timerText;
    [HideInInspector] public TextMeshProUGUI winloseText;

    //WhackEmEnemy whackemScript;
    float randomPopUpTime;
    float randomStayTime;
    bool levelLoaded;
    bool stopPopUp;
    int randomEnemy;


    private void Awake()
    {
        levelLoaded = true;
    }

    private void Start()
    {
        //Text
        ticketsText = ticketsUI.GetComponentInChildren<TextMeshProUGUI>();
        scoreText = scoreUI.GetComponentInChildren<TextMeshProUGUI>();
        timerText = timerUI.GetComponentInChildren<TextMeshProUGUI>();
        winloseText = winloseUI.GetComponentInChildren<TextMeshProUGUI>();
        //Tickets
        ticketsText.text = ("Tickets: " + tickets);
        scoreText.text = (score + "/" + scoreLimit);
        //Timer
        resetTime = timeCounter; //Store this for the reset
        timeLeft = resetTime; //Time left is set to user defined variable of timeCounter
        //Speeds
        minRandoTemp = minRando;
        maxRandoTemp = maxRando;
        //Enemy Script
        //whackemScript = whackemEnemy.GetComponent<WhackEmEnemy>();


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
            if (!stopPopUp)
            {
                StartCoroutine(CountDownTimer());
                if (timeLeft >= 10)
                {
                    timerText.text = ("00:" + (int)timeLeft);
                }
                else
                {
                    timerText.text = ("00:0" + (int)timeLeft);
                }
            }

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
            ResetGame(); //Reset the variables back to original
        }
    }

    public void DisplayUI()
    {
        //Display the scoreUI
        scoreUI.SetActive(true);
        scoreText.text = (score + "/" + scoreLimit);

        //Display the timerUI
        timerUI.SetActive(true);
        //timerText.text = ("00:" + (int)timeCounter);

        //Display win/lose UI
        winloseUI.SetActive(true);
    }

    public void ResetGame()
    {
        stopPopUp = false;

        //Score
        score = 0;
        scoreText.text = (score + "/" + scoreLimit);

        //Speed
        minRando = minRandoTemp;
        maxRando = maxRandoTemp;

        //winloseUI
        winloseUI.SetActive(false);
        winloseText.text = (" ");

        //Time
        timeLeft = resetTime;
        timerText.text = ("00:" + (int)timeLeft);
        timerUI.SetActive(false);

    }

    void WinLoseManager()
    {
        if (score >= scoreLimit && timeLeft > 0)
        {
            //WIN! Get tickets. Add card to inventory
            stopPopUp = true;

            winloseText.text = "You have won...";
        }
        else if (score < scoreLimit && timeLeft <= 0)
        {
            //Lose!
            winloseText.text = "You have lost...";
        }
    }

    public void IncreaseSpeed()
    {
        minRando /= divideSpeedBy;
        maxRando /= divideSpeedBy;
        Debug.Log("Speed has increased!");
    }

    IEnumerator CountDownTimer()
    {
        //Wait for 1 second so that the starting number is displayed.
        yield return new WaitForSeconds(0.5f);

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            stopPopUp = true; //Controls being able to pause enemypopup coroutine 
            critters[randomEnemy].SetActive(false); //Turn off the remaining enemy when the game is done
        }
        else
        {
            yield return null;
        }
    }

    IEnumerator EnemyStuck()
    {
        //Random number [1,2] to see if it takes 1 or 2 hits before the enemy hides.
        int isStuck = UnityEngine.Random.Range(1, 2); //1 false, 2 true
        if (isStuck == 1)
        {
            stopPopUp = false;
        }
        else
        {
            stopPopUp = true;
            yield return new WaitForSeconds(1);
            
            //** Pause EnemyPopUP so enemy is still visible for random time
            // If enemy is hit, turn off enemy
            // else if time runs out first, turn off enemy
        }
    }

    //Choose random enemy with random appear times
    IEnumerator EnemyPopUp()
    {
        while (levelLoaded) //Allow coroutine to load on Start.
        {
            while (gameOn && !stopPopUp) //But don't do anything until the game is on.
            {
                //StartCoroutine(EnemyStuck()); //** Check if enemy needs 2 hit to make it disappear.

                //Iterate through the array of enemies and check if it's visible or not.
                for (int i = 0; i < critters.Length; i++)
                {
                    //Random enemy
                    randomEnemy = UnityEngine.Random.Range(0, 9);
                    //Random speeds
                    randomStayTime = UnityEngine.Random.Range(minRando * 1.5f, maxRando * 1.5f);
                    randomPopUpTime = UnityEngine.Random.Range(minRando, maxRando);
                   
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
                        yield return null;
                    }
                }
            }
            yield return null;
        }
    }   
}
