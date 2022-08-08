using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//** THIS CLASS INHERITS FROM THE GameBooth.cs SCRIPT **//

public class CarnivalSmashGameManager : GameBooth
{
    public static CarnivalSmashGameManager Instance;

    //Bigger nums = slower speed
    [Header("ENEMY POPUP SPEED")]
    public float minRando; private float minRandoTemp;
    public float maxRando; private float maxRandoTemp;
    public float divideSpeedBy; //The amount that the random number is divided by when enemy has been hit.
    public float speedCap;
    float randomPopUpTime;
    float randomTauntTime;
    float randomStayTime;

    int randomEnemy;
    public bool gameJustFinished;
    [HideInInspector] public bool tauntCritVisible;

    [HideInInspector] public bool popUp;
    [HideInInspector] public bool critterIsVisible;
    [HideInInspector] public bool gameIsRunning;
    [HideInInspector] public bool isTaunting = false;

    public GameObject[] critters;
    public GameObject[] taunts;

    public bool levelLoaded; //Allows coroutine to be called from Start
    [HideInInspector] bool stopPopUp;

    private void Awake()
    {
        Instance = this;

        WE = GetWEScript();
        levelLoaded = true;
    }

    private void Start()
    {
        //Get the sprite from the tarot cards
        inactiveCardSprite = GetInactiveCardSprite();
        activeCardSprite = GetActiveCardSprite();

        //Set timer info
        timeLeft = GetTimeCounter();

        StartCoroutine(EnemyPopUp());
    }

    private void Update()
    {
        //GAMEON
        if (gameOn)
        {
            //Set text for this game
            scoreText = GetScoreText();
            timerText = GetTimerText();
            winLoseText = GetWinLoseText();

            //WEAPON
            playerWeapon.SetActive(true); //Show player holding weapon

            //TIMER
            StartCoroutine(CountDownTimer());

            //SCORE
            ScoreDisplay();

            //WIN/LOSE
            StartCoroutine(WinLoseDisplay());

            //
            //PAUSE - Game Rules Menu
            if (Input.GetButtonDown("Menu"))
            {
                ShowGameRules();
            }
        }
        else if (!gameOn && showLostText)
        {
            StartCoroutine(ShutDownGame());
        }
    }

    public void IncreaseSpeed()
    {
        if ((minRando / divideSpeedBy) > speedCap)
        {
            minRando /= divideSpeedBy;
        }

        maxRando /= divideSpeedBy;
    }

    //Choose random enemy with random appear times
    IEnumerator EnemyPopUp()
    {
        while (levelLoaded) //Allow coroutine to load on Start. This way it isn't being updated every frame.
        {
            while (gameOn && !stopPopUp) //But don't do anything until the game is on.
            {
                // create queue of custom class (params to call)
                //Debug.Log("Entered Game");

                while (!gameJustFinished && !gameWon)
                {
                    WhackEmRoutine routine = new WhackEmRoutine(); //Uses contructor to pick random enemy.
                    int critUp = routine.up; //Random enemy that is up
                    int critTaunt = routine.taunt; //Random enemy that is taunting
                    bool tauntBool = routine.addTaunt;

                    //Check if the critter has shown itself.
                    critterIsVisible = critters[critUp].GetComponent<CritterEnemy>().isVis; //check if current critter is visible
                    tauntCritVisible = critters[critTaunt].GetComponent<CritterEnemy>().isVis; //check if taunt critter is visible

                    //Get random times that critter will be visible
                    randomStayTime = UnityEngine.Random.Range(minRando * 1.5f, maxRando * 1.5f); //Amount of time enemy is up
                    randomTauntTime = randomStayTime / 2;
                    randomPopUpTime = UnityEngine.Random.Range(minRando, maxRando); //Amount of time between popping up

                    //if main creature is not visible
                    if (!critterIsVisible)
                    {
                        //raise main creature
                        yield return new WaitForSeconds(randomPopUpTime);

                        critters[critUp].SetActive(true);
                        critterIsVisible = true;

                        //check if specified taunt creat bool is on and its not visible already (not main creat)
                        if (tauntBool && !tauntCritVisible)
                        {
                            TauntPosition position = taunts[critTaunt].GetComponentInChildren<TauntPosition>(); //Finds the taunt position of the taunt enemy

                            taunts[critTaunt].SetActive(true);
                            taunts[critTaunt].transform.position = new Vector3(position.tauntPosition.position.x, position.tauntPosition.position.y, position.tauntPosition.position.z);
                            tauntCritVisible = true;

                            yield return new WaitForSeconds(randomTauntTime);

                            tauntCritVisible = false;
                            taunts[critTaunt].SetActive(false);
                        }

                        //bring both down
                        yield return new WaitForSeconds(randomStayTime);

                        critterIsVisible = false;
                        critters[critUp].SetActive(false);
                    }
                    //Debug.Log("score = " + score + " game won " + gameWon);
                }
            }

            yield return null;
        }
    }
}
