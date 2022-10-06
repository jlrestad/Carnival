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
    GameObject saveCurrentWeapon; //Used to save current weapon info for re-equip


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
            //WEAPON EQUIP
            //1. Hide weapon, if holding one, before holding this game's weapon, & disable the active Tarot card for it.
            if (WE.currentWeapon != null && WE.currentWeapon != playerWeapon)
            {
                //DisablePreviousActiveCard();

                //Get the index of the current weapon that isnt this game's weapon so it can be disabled.
                weaponListIndex = WE.weaponList.IndexOf(WE.currentWeapon);
                WE.weaponCards[weaponListIndex].GetComponent<Image>().enabled = false;
                WE.currentWeapon.SetActive(false);
                saveCurrentWeapon = WE.currentWeapon; //Store this so it can be equipped
            }

            //2. Equip this game's weapon & assign to current weapon
            playerWeapon.SetActive(true); //Show player holding weapon
            playerWeapon.transform.GetChild(0).gameObject.SetActive(true); //Show player holding weapon
            WE.currentWeapon = playerWeapon;
            WE.holdingSkull = true;

            //3. Display Proper Tarot if a different weapon was in hand during game start.
            if (WE.haveSkull /*&& WE.currentWeapon != playerWeapon*/)
            {
                //EnableGameActiveCard();
                int index = WE.weaponList.IndexOf(playerWeapon); //Get the index of this weapon in the list
                WE.weaponCards[index].GetComponent<Image>().enabled = true; //Show the Tarot for this weapon
            }

            //turn on audio
            if (!tentAudio.enabled)
            {
                tentAudio.enabled = true;
            }

            ///WIN/LOSE - If Won: Adds weapon to the array of weapons; If Lost: Runs ResetGame()
            StartCoroutine(WinLoseDisplay());

            //Timer
            if (isRunning) 
            { 
                StartCoroutine(CountDownTimer());
            }

            //Pause
            if (Input.GetButtonDown("Menu")) //pausing during minigame
            {
                ShowGameRules();
            }

            if (!isRunning)
            {
                //Game UI
                scoreText = GetScoreText();
                timerText = GetTimerText();
                winLoseText = GetWinLoseText();

                scoreText.text = "SURVIVE";

                if (!isPaused)
                {
                    GameStart();
                }
            }
        }
        else if (!gameOn && isRunning)
        {
            GameEnd();

            if (showLostText)
            {
                if (!cbWon)
                {
                    playerWeapon.SetActive(false); //Remove weapon from player's hands.
                    WE.holdingSkull = false;

                    //If the game is lost and player has weapons, re-equip the last held weapon.
                    if (WE.weaponList.Count > 0)
                    {
                        WE.currentWeapon = saveCurrentWeapon;
                        WE.weaponCards[weaponListIndex].GetComponent<Image>().enabled = true; //Show the tarot of last held weapon
                        WE.currentWeapon.SetActive(true); //Show the last held weapon
                    }
                }
            }
            //If the game is quit and the weapon list is empty, set the current weapon to null.
            if (WE.weaponList.Count == 0)
            {
                WE.currentWeapon = null;
                playerWeapon.SetActive(false); //Remove weapon from player's hands.
            }
            else if (WE.weaponList.Count > 0 && !cbWon)
            {
                //Re-equip the weapon that was held before playing new minigame.
                if (saveCurrentWeapon != null)
                {
                    playerWeapon.SetActive(false); //Remove weapon from player's hands.
                    WE.currentWeapon = saveCurrentWeapon;
                    WE.weaponCards[weaponListIndex].GetComponent<Image>().enabled = true; //Show the tarot of last held weapon
                    WE.currentWeapon.SetActive(true); //Show the last held weapon
                }
            }
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
        else if(score == 0 && tentAudio.clip == CBSpook)
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
            showLostText = true; //Allows the lose condition to play in WinLoseDisplay()
            StartCoroutine(PlayLoseFX()); //start playing the lose FX before the game ends
        }
        if(timeLeft <= 0 && gameOn)
        {
            gameOn = false; //end the game
        }
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
        //StartCoroutine(ShutDownGameMusicAndLights()); //* This is called in ResetGame

        StopCoroutine(PickTimer()); //keep the script from opening any more coffins

        foreach(CasketManager CM in casketList)
        {
            CM.CoffinReset(); //reset each coffin to its original state
        }

        isRunning = false; //stop the coffin movement

        if (gameWon)
        {
            WinTickets(2, 1);
            ResetGame();

            //* When game is played after being won, this will keep the win description screen from being displayed again.
            if (!cbWon)
            {
                DisplayGameCard();
                //Debug.Log("CB is WON");
            }
            else
            {
                if (tentAudio.enabled)
                {
                    tentAudio.enabled = false;
                }
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
            if (tentAudio.enabled)
            {
                tentAudio.enabled = false;
            }
        }
    }

    //--------------------------------------------------|RegisterHit|
    public void RegisterHit()
    {
        //score--;

        //if (score < 0) { score = 0; }

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
    //PickCoffin data added to PickTimer() in order to simplify the code.
    //public void PickCoffin()
    //{
    //    //StopCoroutine(PickTimer()); //stops the coroutine this was called from in order to prevent infinite looping.
    //    if (!isPaused)
    //    {
    //        int randomCoffin = Random.Range(0, casketList.Count); //pick a random coffin from the list
    //        casketList[randomCoffin].AttemptOpen(); //attempt to tell that coffin to open
    //        StartCoroutine(PickTimer()); //start the coroutine again to pick another coffin
    //    }
    //}
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
    }

    public IEnumerator PickTimer()
    {
        if (isRunning)
        {
            if (!isPaused)
            {
                yield return new WaitForSeconds(coffinPickerWaitTime);

                int randomCoffin = Random.Range(0, casketList.Count); //pick a random coffin from the list
                casketList[randomCoffin].AttemptOpen(); //attempt to tell that coffin to open
                StartCoroutine(PickTimer()); //start the coroutine again to pick another coffin
            }
            else
            {
                //When the game is paused, wait until it was unpaused and start the coroutine again.
                yield return new WaitUntil(() => !isPaused);

                StartCoroutine(PickTimer());
            }
        }
    }
    #endregion
}
