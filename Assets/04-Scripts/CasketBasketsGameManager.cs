using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//** THIS CLASS INHERITS FROM THE GameBooth.cs SCRIPT **//

public class CasketBasketsGameManager : GameBooth
{
    public static CasketBasketsGameManager Instance;
    /*
     * This script handles the connection between the gameplay elements of the CasketBaskets minigame and the overall game manager and player character.
     * Jes Restad & Grant Hargraves 8/2022
     */
    //==================================================
    //=========================|FIELDS|
    //==================================================
    #region FIELDS
    [Header("STATS")]
    [Tooltip("The amount each coffin's movement speeds up when taking a hit.")]
    [SerializeField] float speedChange;
    [Tooltip("How much the frequency of position changes increases for each coffin when taking a hit.")]
    [SerializeField] float goalShiftChange;
    [Tooltip("How much shorter each coffin needs to stay shut before opening again when taking a hit.")]
    [SerializeField] float closedTimeChange;
    [Tooltip("How long this game manager waits between attempting to open a random coffin. Note that coffins will only open at the end of their 'CoffinClosed' coold, so this will not always work.")]
    [SerializeField] float coffinPickerWaitTime = 2;
    [Tooltip("How much to reduce coffinPickerWaitTime each time a coffin is hit.")]
    [SerializeField] float waitTimeReduction = 0.05f;

    [Header("PLUG-IN")]
    [Tooltip("The scripts attached to each individual coffin go here so their methods can be accessed individually.")]
    public List<CasketManager> casketList = new List<CasketManager>();
    [Space(20)]
    [Tooltip("The AudioSource used for playing minigame-wide sounds, such as the spook-loop or lose buzzer.")]
    [SerializeField] AudioSource tentAudio;
    [SerializeField] AudioSource CBBuzzer;
    [SerializeField] AudioClip CBSpook;
    [SerializeField] AudioClip CBSpookFail;
    [SerializeField] AudioClip CBLose;
    [SerializeField] AudioClip CBWin;


    [Header("INTERNAL/DEBUG")]
    [Tooltip("Tracks when the game is turned on or off so methods can be run accordingly.")]
    public bool isRunning = false;
    [Tooltip("A debug int that shows how many caskets are present in the game ")]
    [SerializeField] int CBAmount;
    [Tooltip("tracks whether the game has ended or not, to allow effects to play properly.")]
    [SerializeField] bool hasEnded = false;
    public float delty = 0f;
    #endregion
    //==================================================
    //=========================|BUILT-IN METHODS|
    //==================================================
    #region BUILT-IN METHODS
    //--------------------------------------------------|Awake|
    private void Awake()
    {
        Instance = this; //set this as an instance so it can be universally referenced
        WE = GetWEScript(); //reference to WeaponEquip.cs
        CBAmount = casketList.Count; //set CBAmount to the proper number of caskets
    }

    //--------------------------------------------------|Start|
    private void Start()
    {
        //Get the sprite from the tarot cards
        inactiveCardSprite = GetInactiveCardSprite();
        activeCardSprite = GetActiveCardSprite();

        //Set timer info
        timeLeft = GetTimeCounter();
    }

    //--------------------------------------------------|Update|
    private void Update()
    {
        //When the game turns on, run GameStart
        if (gameOn)
        {
            //WIN/LOSE - If Won: Adds weapon to the array of weapons; If Lost: Runs ResetGame()
            StartCoroutine(WinLoseDisplay());
            
            if (!isRunning)
            {
                //Hide weapon, if holding one, before holding new weapon.
                if (WE.currentWeapon != null && WE.currentWeapon != WE.skullParent)
                    WE.currentWeapon.SetActive(false);

                //Game UI
                scoreText = GetScoreText(); 
                timerText = GetTimerText();
                winLoseText = GetWinLoseText();

                scoreText.text = "SURVIVE";

                //WEAPON - Player holds weapon to play game
                if (!WE.haveSkull)
                {
                    playerWeapon.SetActive(true); //Show player holding weapon
                    playerWeapon.transform.GetChild(0).gameObject.SetActive(true); //Show player holding weapon
                }

                GameStart();

                //Pause
                if (Input.GetButtonDown("Menu")) //pausing during minigame
                {
                    ShowGameRules();
                }
            }
        }
        else if (!gameOn && isRunning)
        {
            ////If the game has never been won then put the skulls back.
            //if (!gameWon && !cbWon)
            //{
            //    WE.holdingSkull = false;
            //    playerWeapon.SetActive(false);
            //}

            ////If the game is lost
            //if (!gameWon && showLostText)
            //{
            //    WE.holdingSkull = false;
            //}

            GameEnd();
        }

        if (!gameOn && !cbWon)
        {
            playerWeapon.SetActive(false); //Remove weapon from player's hands.
            WE.holdingSkull = false;
        }


        //-----Intensity effects-----
        if (score >= 1 && score < casketList.Count) //if score is within range
        {
            if(tentAudio.clip != CBSpook) //if the sfx isn't playing (to prevent infinite loop)
            {
                tentAudio.clip = CBSpook;
                tentAudio.loop = true;
                tentAudio.Play();
            }
            
        }
        else if(score <= 0 && tentAudio.clip == CBSpook)
        {
            tentAudio.clip = null;
        }

        //-----Below handles whether the player wins or loses the minigame-----
        // NOTE: score is used to track how many coffins are open at once
        if (timeLeft <= 1 && score < casketList.Count && !hasEnded) //if we've reached the end and the coffins are not all open...
        {
            hasEnded = true;
            gameWon = true; //the game is marked as being won!
        }
        if (timeLeft > 1 && score >= casketList.Count && !hasEnded) //if all coffins are open at once and the game isn't almost over...
        {
            hasEnded = true;
            gameWon = false; //the player has lost the game.
            StartCoroutine(PlayLoseFX()); //start playing the lose FX before the game ends
        }
        if(timeLeft <= 0 && gameOn)
        {
            gameOn = false; //end the game

            //* This is already being called under GameEnd()
            //DisplayGameCard(); //show the player their prize!
        }
        if (gameOn)
        StartCoroutine(CountDownTimer()); //start game timer every frame which is dangerous but this is how we're doing it I guess
    }
    #endregion

    //==================================================
    //=========================|CUSTOM METHODS|
    //==================================================
    #region CUSTOM METHODS
    //--------------------------------------------------|GameStart|

    public void GameStart()
    {
        hasEnded = false;
        ScoreDisplay();
        
        CBBuzzer.Play();
        if (!isRunning)
        {
            foreach (CasketManager CM in casketList)
            {
                CM.CoffinStart(); //tell each coffin to start moving
            }
        }
        isRunning = true; //so that this doesn't get called multiple times from update
        StartCoroutine(PickTimer()); //start picking coffins to open up
    }

    //--------------------------------------------------|GameEnd|
    public void GameEnd()
    {
        isRunning = false; //stop the coffin movement

        StartCoroutine(ShutDownGameMusicAndLights());

        StopCoroutine(PickTimer()); //keep the script from opening any more coffins

        foreach(CasketManager CM in casketList)
        {
            CM.CoffinReset(); //reset each coffin to its original state
            score = 0; //reset score to zero
        }

        if (gameWon)
        {
            //* When game is played after being won, this will keep the win description screen from being displayed again.
            if (!cbWon)
            {
                DisplayGameCard();
                //Debug.Log("CB is WON");
            }
            else
            {
                ResetGame();
            }

            cbWon = true;
            WE.haveSkull = true;
            WE.gameWeapon = null;
            WE.currentWeapon = WE.skullParent;

            tentAudio.PlayOneShot(CBWin);
        }
        else
        {
            tentAudio.PlayOneShot(CBLose);
            ResetGame();
        }
    }

    //--------------------------------------------------|RegisterHit|
    public void RegisterHit()
    {
        foreach (CasketManager CM in casketList)
        {
            //Make the game harder by speeding things up a tiny bit
            CM.AddSpeed(speedChange);
            CM.ReduceGoalShiftTime(goalShiftChange);
            CM.ReduceClosedTime(closedTimeChange);
            if(coffinPickerWaitTime > 1)
            {
                coffinPickerWaitTime -= waitTimeReduction;
            }
        }
    }
    //--------------------------------------------------|PickCoffin|
    public void PickCoffin()
    {
        StopCoroutine(PickTimer()); //stops the coroutine this was called from in order to prevent infinite looping.
        int randomCoffin = Random.Range(0, casketList.Count); //pick a random coffin from the list
        casketList[randomCoffin].AttemptOpen(); //attempt to tell that coffin to open
        StartCoroutine(PickTimer()); //start the coroutine again to pick another coffin
    }
    #endregion

    //==================================================
    //=========================|COROUTINES|
    //==================================================
    #region COROUTINES
    public IEnumerator PlayLoseFX()
    {
        tentAudio.Stop();
        tentAudio.PlayOneShot(CBSpookFail);
        yield return new WaitForSeconds(2); //let the lose FX play
        GameEnd();
        //ResetGame(); //end the game //* This doesn't seem to be doing anything. Moved it to the bottom of GameEnd().
    }

    public IEnumerator PickTimer()
    {
        if(isRunning)
        {
            yield return new WaitForSeconds(coffinPickerWaitTime);
            PickCoffin();
        }
    }
    #endregion
}
