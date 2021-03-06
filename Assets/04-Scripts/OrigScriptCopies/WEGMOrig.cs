/*

using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WhackEmGameManager : MonoBehaviour
{
    public static WhackEmGameManager Instance;

    public GameObject[] critters;
    public GameObject whackemEnemy;
    [HideInInspector] public WeaponEquip weaponEquip;
    [SerializeField] GameCardManager cardManager;
    //public int tickets = 3;

    public bool gameOn;
    public bool gameJustFinished;
    [HideInInspector] public bool popUp;
    [HideInInspector] public bool critterIsVisible;
    [HideInInspector] public bool gameIsRunning;
    [HideInInspector] public bool isTaunting = false;

    [Header("SCORE")]
    [SerializeField] int scoreLimit; //the amount needed to win
    [HideInInspector] public int score; //the player kills

    [Header("TIMER")]
    [SerializeField] private float timeCounter = 30; //used to count down the time
    private float timeLeft; //used to set the amount of time to countdown by
    private float resetTime;

    //Bigger nums = slower speed
    [Header("ENEMY POPUP SPEED")]
    public float minRando; private float minRandoTemp;
    public float maxRando; private float maxRandoTemp;
    public float divideSpeedBy; //The amount that the random number is divided by when enemy has been hit.

    [Header("UI")]
    public GameObject gameUI;
    public TextMeshProUGUI ticketsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winloseText;

    [Header("TAROT CARD")]
    public GameObject displayCard;
    public Sprite cardImage;

    [Space(10)]
    //[HideInInspector]
    public bool gameWon;
    //[HideInInspector]
    public bool gameOver;
    float randomPopUpTime;
    float randomStayTime;
    public bool levelLoaded;
    [HideInInspector] bool stopPopUp;

    int randomEnemy;
    public MonoBehaviour script;

    private void Awake()
    {
        Instance = this;
        script = Instance;
        levelLoaded = true;
    }

    private void Start()
    {
        weaponEquip = FindObjectOfType<WeaponEquip>();
        cardImage = displayCard.GetComponent<Image>().sprite;

        //Tickets
        TicketManager.Instance.DisplayTicketAmount();
        scoreText.text = (score + "/" + scoreLimit);
        //Timer
        resetTime = timeCounter; //Store this for the reset
        timeLeft = resetTime; //Time left is set to user defined variable of timeCounter
        //Speeds
        minRandoTemp = minRando;
        maxRandoTemp = maxRando;

        //Coroutines will start but wait until gameOn is true to begin.
        StartCoroutine(EnemyPopUp());
    }

    private void Update()
    {
        if (gameOn)
        {
            // alert weapon equip that the game is active and mallet can be picked up
            weaponEquip.whackEmActive = true;
        }

        //Run this when the WhackEm game is on.
        if (gameOn && weaponEquip.haveMallet)
        {
            // fixes bug causing mouse to appear when critter pops up
            Cursor.lockState = CursorLockMode.Locked;
            //Display the game UI
            DisplayTextUI();

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

            //Display Win/Lose
            if (score >= scoreLimit && timeLeft > 0 && !gameOver)
            {
                WinUI();
            }
            else if (score < scoreLimit && timeLeft <= 0 && !gameOver)
            {
                //Display win or lose
                StartCoroutine(LoseUI());

                //* Put weapon back
                weaponEquip.haveMallet = false;
                weaponEquip.currentWeapon.SetActive(false);
                weaponEquip._closestWeapon.SetActive(true);
            }

            //Update ticket count
            TicketManager.Instance.DisplayTicketAmount();

            if (TicketManager.Instance.tickets < 0)
            {
                TicketManager.Instance.tickets = 0;
                ticketsText.text = "NEED TICKETS";

                //Ticket is needed in order to play...
                gameOn = false;
            }
        }
        else
        {
            //   gameUI.SetActive(false);
            //   ResetGame(); //Reset the variables back to original
        }
    }

    public void DisplayTextUI()
    {
        //Display the scoreUI
        gameUI.SetActive(true);
        scoreText.text = (score + "/" + scoreLimit);

        //Display the timerUI
        timerText.enabled = true;
    }

    public void ResetGame()
    {
        stopPopUp = false;
        weaponEquip.whackEmActive = false;

        //Score
        score = 0;
        scoreText.text = (score + "/" + scoreLimit);

        //Speed
        minRando = minRandoTemp;
        maxRando = maxRandoTemp;

        //Time
        timeLeft = resetTime;
        timerText.text = ("00:" + (int)timeLeft);

        gameUI.SetActive(false);

    }

    public void IncreaseSpeed()
    {
        minRando /= divideSpeedBy;
        maxRando /= divideSpeedBy;
    }

    IEnumerator CountDownTimer()
    {
        //Wait for 1 second so that the starting number is displayed.
        yield return new WaitForSeconds(0.5f);

        timeLeft -= Time.deltaTime;
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            critters[randomEnemy].SetActive(false); //Turn off the remaining enemy when the game is done
            stopPopUp = true; //Controls being able to pause enemypopup coroutine 
        }
        else
        {
            yield return null;
        }
    }

    void WinUI()
    {
        DisplayGameCard();
        gameWon = true; //Win weapon card and keep weapon in inventory.
        stopPopUp = true; //Stop the EnemyPopUp coroutine.
        gameOver = true; //The game has either been won or time has run out.
    }

    //Display the win or lose screen for a short time.
    IEnumerator LoseUI()
    {
        //Display lose message
        winloseText.enabled = true;
        winloseText.text = "You have lost...";
        gameWon = false;
        stopPopUp = true;
        gameOn = false;
        gameJustFinished = true;
        weaponEquip.whackEmActive = false;
        timerText.enabled = false;


        yield return new WaitForSeconds(2);

        //Clear and turn off lose message
        winloseText.text = (" ");
        winloseText.enabled = false;
        gameOver = true;
        ResetGame();
    }

    //Choose random enemy with random appear times
    IEnumerator EnemyPopUp()
    {
        while (levelLoaded) //Allow coroutine to load on Start.
        {
            while (gameOn && !stopPopUp && weaponEquip.haveMallet) //But don't do anything until the game is on.
            {
                for (int i = 0; i < critters.Length; i++)
                {
                    randomEnemy = UnityEngine.Random.Range(0, 5);  //Choose random enemy to pop-up
                    int randomTaunt = UnityEngine.Random.Range(0, 5); //Choose random enemy that will taunt
                    TauntPosition position = critters[randomEnemy].GetComponentInChildren<TauntPosition>(); //Finds the taunt position of the taunt enemy
                   
                    randomStayTime = UnityEngine.Random.Range(minRando * 1.5f, maxRando * 1.5f); //Amount of time enemy is up
                    randomPopUpTime = UnityEngine.Random.Range(minRando, maxRando); //Amount of time between popping up

                    //Check through each enemy to see if it is visible
                    critterIsVisible = critters[i].GetComponent<WhackEmEnemy>().isVis;

                    //Check to see if enemy is not visible.
                    if (!critterIsVisible)
                    {
                        //Check if the enemy is taunting
                        if (randomEnemy == randomTaunt)
                        {
                            Debug.Log("TAUNTING!!");

                            //Enemy is active
                            critters[randomEnemy].SetActive(true);
                            critterIsVisible = true; //Set the enemy to visible so that a new random number is called.

                            //Enemy appears at taunt position
                            critters[randomEnemy].transform.position = new Vector3(position.tauntPosition.position.x, position.tauntPosition.position.y, position.tauntPosition.position.z);
                            isTaunting = true;

                            yield return new WaitForSeconds(0.3f); //Amount of time to stay in taunt position

                            //Debug.Log("Stopped taunting");

                            //Move back to normal position
                            critters[randomEnemy].transform.position = new Vector3(position.enemyPosition.position.x, position.enemyPosition.position.y, position.enemyPosition.position.z);
                            critterIsVisible = false;

                            critters[randomEnemy].SetActive(false);
                            isTaunting = false;
                        }
                        else
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

    //Show the card that was won
    public void DisplayGameCard()
    {
        //Display the card that was won
        displayCard.GetComponent<Image>().enabled = true;

        //Transition from card display to weapon card
        StartCoroutine(DisplayCardWon());
    }

    //Transition from displayed card to weapon indicator card
    public IEnumerator DisplayCardWon()
    {
        yield return new WaitForSeconds(1);

        displayCard.GetComponent<Image>().enabled = false;

        //Display the current weapon card
        Menu.Instance.DisplayWeaponCard();

    }

}



*/