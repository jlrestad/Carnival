using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


//** THIS CLASS INHERITS FROM THE GameBooth.cs SCRIPT **//

public class CarnivalSmashGameManager : GameBooth
{
    public static CarnivalSmashGameManager Instance;

    //Bigger nums = slower speed
    [Header("ENEMY POPUP SPEED")]
    public float minRando; /*private float minRandoTemp;*/
    public float maxRando; /*private float maxRandoTemp;*/
    public float divideSpeedBy; //The amount that the random number is divided by when enemy has been hit.
    public float speedCap;
    float maxRandoSaver;
    float randomPopUpTime;
    float randomTauntTime;
    float randomStayTime;

    [HideInInspector] public bool tauntCritVisible;
    public bool gameJustFinished;
    public bool stopPopUp; //Used to pause the coroutine
    /*[HideInInspector] */public bool popUp;
    /*[HideInInspector] */public bool critterIsVisible;
    /*[HideInInspector] */public bool isTaunting = false;
    /*[HideInInspector] */public bool gameIsRunning;

    GameObject saveCurrentWeapon; //Used to save current weapon info for re-equip

    [SerializeField] GameObject[] critters;
    [SerializeField] GameObject[] taunts;

    public bool levelLoaded; //Allows coroutine to be called from Start
    //int randomEnemy;

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
        stopPopUp = false;

        maxRandoSaver = maxRando; //Save this value so it can be returned
        StartCoroutine(EnemyPopUp());
    }

    private void Update()
    {
        //GAMEON
        if (gameOn)
        {
            //WEAPON EQUIP
            //1. Hide weapon, if holding one, before holding this weapon & disable the active Tarot card for it.
            if (WE.currentWeapon != null && WE.currentWeapon != playerWeapon)
            {
                weaponListIndex = WE.weaponList.IndexOf(WE.currentWeapon); //Get index of current weapon
                WE.weaponCards[weaponListIndex].GetComponent<Image>().enabled = false; //Hide the tarot of current weapon
                WE.currentWeapon.SetActive(false); //Hide the weapon
                saveCurrentWeapon = WE.currentWeapon; //Store this so it can be equipped
            }
           
            //2. Equip this game's weapon & assign to current weapon
            playerWeapon.SetActive(true); //Show player holding weapon
            WE.currentWeapon = playerWeapon;
            
            //3. Display proper Tarot for this weapon if game was won.
            if (WE.haveMallet)
            {
                //EnableGameActiveCard();
                int index = WE.weaponList.IndexOf(playerWeapon); //Get the index of this weapon in the list
                WE.weaponCards[index].GetComponent<Image>().enabled = true; //Show the Tarot for this weapon
            }

            //Set text for this game
            scoreText = GetScoreText();
            timerText = GetTimerText();
            winLoseText = GetWinLoseText();

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
                stopPopUp = true;
            }
            else if (!isPaused)
            {
                stopPopUp = false;
            }

            if (gameWon)
            {
                WinTickets(3, 1);

                if (!csWon)
                {
                    DisplayGameCard();
                }
                else
                {
                    ResetGame();
                }

                csWon = true;
                WE.haveMallet = true;
                WE.gameWeapon = null;
                WE.currentWeapon = WE.malletHold;
            }
        }
        else if (!gameOn)
        {
            maxRando = maxRandoSaver;

            if (!gameWon && showLostText)
            {
                stopPopUp = true;
                StartCoroutine(ShutDownGameMusicAndLights());
                if (!csWon)
                {
                    playerWeapon.SetActive(false); //Remove weapon from player's hands.
                }
                if (WE.weaponList.Count > 0)
                {
                    WE.currentWeapon = saveCurrentWeapon;
                    WE.weaponCards[weaponListIndex].GetComponent<Image>().enabled = true; //Show the tarot of last held weapon
                    WE.currentWeapon.SetActive(true); //Show the last held weapon
                }
            }
            //If the game is lost and the weapon list is empty, set the current weapon to null.
            if (WE.weaponList.Count == 0)
            {
                WE.currentWeapon = null;
                playerWeapon.SetActive(false); //Remove weapon from player's hands.
            }
        }
    }

    //Increases the speed that the critters appear.
    public void IncreaseSpeed()
    {
        //Throw error if denominator is smaller than numerator.
        if (divideSpeedBy < minRando)
        {
            Debug.LogError("DivideBySpeed must be larger than the minRando speed of CarnivalSmashGameManager.cs.");
        }

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
                WhackEmRoutine routine = new WhackEmRoutine(); //Uses contructor to pick random enemy.
                int critUp = routine.up; //Random enemy that is up
                int critTaunt = routine.taunt; //Random enemy that is taunting
                bool tauntBool = routine.addTaunt;

                //Check if the critter has shown itself.
                critterIsVisible = critters[critUp].GetComponent<CritterEnemy>().isVis; //check if current critter is visible
                tauntCritVisible = taunts[critTaunt].GetComponent<TauntPosition>().isVis; //check if taunt critter is visible
             
                //Get random times that critter will be visible
                randomStayTime = UnityEngine.Random.Range(minRando * 1.5f, maxRando * 1.5f); //Amount of time enemy is up
                randomTauntTime = randomStayTime / 2;
                randomPopUpTime = UnityEngine.Random.Range(minRando, maxRando); //Amount of time between popping up

                //POP-UP CRITTER
                if (!critterIsVisible)
                {
                    //raise main creature
                    yield return new WaitForSeconds(randomPopUpTime);
                    //make critter visible
                    critters[critUp].SetActive(true);
                    critterIsVisible = true;

                    //TAUNT CRITTER
                    //check if specified taunt critter bool is on and that it's not visible already (not main critter)
                    if (tauntBool && !tauntCritVisible)
                    {
                        TauntPosition position = taunts[critTaunt].GetComponent<TauntPosition>(); //Finds the taunt position of the taunt enemy

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
                yield return null;
            }
            yield return null;
        }
    }
}
