using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;
using JetBrains.Annotations;

public class GameBooth : MonoBehaviour
{
    public static GameBooth Instance;

    [Header("UI")]
    public GameObject minigameHUD; //Manually set in Unity game manager script
    public GameObject gameRules; //Manually set in Unity game manager script
    //public TextMeshProUGUI ticketsText;
    public TextMeshProUGUI scoreText; //Manually set in Unity game manager script
    public TextMeshProUGUI timerText; //Manually set in Unity game manager script
    public TextMeshProUGUI winLoseText; //Manually set in Unity game manager script

    [Header("SCORE")]
    public int scoreLimit; //the amount needed to win
    public int score; //the player kills

    [Header("TIMER")]
    public float timeCounter; //used to count down the time
    public float timeLeft; //used to set the amount of time to countdown by
    public float resetTime;

    [Header("TAROT CARD")]
    public GameObject displayScreen; //Tarot card won screen (PlayerHud > Pickups > Skulls) - manually set in Unity game manager script
    public GameObject activeCard; //Game card for active weapon - manually set in Unity game manager script
    public GameObject inactiveCard; //Game card for inactive weapon - manually set in Unity game manager scrit
    public Sprite inactiveCardSprite;
    public Sprite activeCardSprite;

    [Header("GAME SETUP")]
    public bool gameOn;
    public bool gameWon;
    public bool showLostText;
    public bool isPaused;
    public bool playerDead;
    public GameObject playerWeapon;
    public Transform gameplayPosition;

    [Header("AUDIO & LIGHTING")]
    public AudioSource minigameAudio;
    public GameObject minigameLight;

    [Header("TRACK GAMES WON")]
    public bool ssWon;
    public bool csWon;
    public bool cbWon;

    [Header("SCRIPTS")]
    //public Menu menu;
    public WeaponEquip WE;
   

    private void Awake()
    {
        Instance = this;

        //menu = FindObjectOfType<Menu>();
    }

    #region CONTRUCTORS
    //Constructor sets the default values.
    public GameBooth()
    {
        this.activeCard = ActiveCard;
        this.inactiveCard = InactiveCard;

        scoreLimit = 20;

        resetTime = timeCounter; //Store this for the reset
        timeLeft = resetTime; //Time left is set to user defined variable of timeCounter
    
    }
    #endregion

    #region SETTERS
    //* * *
    //SETTERS

    //Set Timer Info
    public GameObject MinigameHUD
    {
        get { return minigameHUD; }
        set { minigameHUD = value; }
    }

    public float TimeCounter
    {
        get { return timeCounter; }
        set { timeCounter = value; }
    }

    //Set the active card
    public GameObject ActiveCard
    {
        get { return activeCard; }
        set { activeCard = value; }
    }

    //Set the inactive card
    public GameObject InactiveCard
    {
        get { return inactiveCard; }
        set { inactiveCard = value; }
    }

    public GameObject GameRules
    {
        get { return gameRules; }
        set { gameRules = value; }
    }
#endregion

    #region GETTERS
    // * * *
    //GETTERS
    public TextMeshProUGUI GetScoreText()
    {
        MinigameHUD[] elements = FindObjectsOfType<MinigameHUD>();

        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].CompareTag("ScoreText"))
            {
                scoreText = elements[i].GetComponent<TextMeshProUGUI>();
            }
        }
        scoreText.text = (score + "/" + scoreLimit);

        return scoreText;
    }

    public TextMeshProUGUI GetTimerText()
    {
        MinigameHUD[] elements = FindObjectsOfType<MinigameHUD>();

        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].CompareTag("TimerText"))
            {
                timerText = elements[i].GetComponent<TextMeshProUGUI>();
            }   
        }
        return timerText;
    }

    public TextMeshProUGUI GetWinLoseText()
    {
        MinigameHUD[] elements = FindObjectsOfType<MinigameHUD>();

        for (int i = 0; i < elements.Length; i++)
        {
            if (elements[i].CompareTag("WinLoseText"))
            {
                winLoseText = elements[i].GetComponent<TextMeshProUGUI>();
            }
        }
        return winLoseText;
    }

    public Sprite GetActiveCardSprite()
    {
        return activeCard.GetComponent<Image>().sprite;
    }

    public Sprite GetInactiveCardSprite()
    {
        return inactiveCard.GetComponent<Image>().sprite;
    }

    public WeaponEquip GetWEScript()
    {
        return WE = GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponEquip>(); //Get the script from the Player 
    }

    public GameObject GetGameRulesMenu()
    {
        return gameRules.gameObject; 
    }

    public float GetTimeCounter()
    {
        return timeCounter;
    }
    #endregion

    #region GAMEPLAY METHODS
    // * * *
    //SHARED GAMEPLAY METHODS
    
    //* Method to Win 3 red + 1 blue if minigame is won
    public void WinTickets()
    {
        StartCoroutine(HudManager.Instance.RedTixAddFX(3));
        StartCoroutine(HudManager.Instance.BlueTixAddFX(1));

        HudManager.Instance.redTickets += 3;
        HudManager.Instance.blueTickets += 1;
    }


    public void ShowGameRules()
    {
        //If the main pause menu is not visible, then show the game rules.
        if (Menu.Instance.counter != 1)
        {
            ShowCursor();

            Menu.Instance.counter = -1;

            if (gameOn)
            {
                isPaused = true;

                WE.isEquipped = false; //Hides the crosshair during pause
            }

            FPSController.Instance.canMove = false;
            WeaponEquip.Instance.gameMenuDisplayed = true;
            gameRules.SetActive(true);
        }
    }

    public void PlayGame()
    {
        isPaused = false;
        Menu.Instance.counter = -1;

        if (!gameOn)
        {
            gameOn = true;
            minigameHUD.SetActive(true);
            gameRules.SetActive(false);

            WE.gameWeapon = WE.closestWeapon; //Save the game object before it's turned off so it can be returned if game is lost.
            WE.isEquipped = true; //Shows the crosshair
            WE.closestWeapon.SetActive(false); //Hide weapon from scene.

            HudManager.Instance.HealthTicket(1); //Cost to play game.
            LockPlayerOnPlay(); //Puts player into game play position.
            HideCursor();
            GameBoothAudioAndLights();
        }
        else
        {
            gameRules.SetActive(false);
            WE.gameMenuDisplayed = false;
            WE.isEquipped = true; //Show the crosshair again.

            LockPlayerOnPlay();
            HideCursor();
        }
    }

    public void GameBoothAudioAndLights()
    {
        if (!minigameAudio.isPlaying || !minigameLight.activeInHierarchy)
        {
            //Make sure light is on - and turn it higher when game is in play.
            minigameLight.SetActive(true);
            minigameLight.GetComponent<Light>().intensity = 45;

            if (minigameAudio.volume == 0)
            {
                minigameAudio.volume = 0.7f;
            }
            minigameAudio.Play();
        }
    }

    public void ExitGame()
    {
        ResetGame();
    }

    //RESETS THE GAME BACK TO DEFAULT
    public void ResetGame()
    {
        //Check if there are tickets/health
        HudManager.Instance.GameOverCheck();
        Menu.Instance.counter = 0;

        //Reset bools
        gameOn = false;
        isPaused = false;
        showLostText = false;

        gameRules.SetActive(false);
        WE.gameMenuDisplayed = false;
        WE.actionPrompt.SetActive(false);
        minigameHUD.SetActive(false);

        FPSController.Instance.canMove = true;

        //Reset values
        score = 0;
        timeLeft = timeCounter;
        if (winLoseText != null) { winLoseText.text = ""; }

        StartCoroutine(ShutDownGameMusicAndLights());
        HideCursor();
        UnLockPlayer();

        //Hide the weapon if the game was not won.
        if (!gameWon && WE.gameWeapon)
        {
            WE.gameWeapon.SetActive(true);
        }

        gameWon = false;
    }
        
    public void LockPlayerOnPlay()
    {
        FPSController.Instance.canMove = true;
        FPSController.Instance.transform.position = gameplayPosition.position;
        FPSController.Instance.GetComponent<CharacterController>().enabled = false;
    }

    public void UnLockPlayer()
    {
        //Hide the cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Allow player to walk again
        FPSController.Instance.GetComponent<CharacterController>().enabled = true;
        FPSController.Instance.canMove = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    public void ScoreDisplay()
    {
        //Set the score boundary.
        if (score >= scoreLimit)
        {
            score = scoreLimit;

            if (timeLeft > 0)
            {
                gameWon = true;
                showLostText = false;
                gameOn = false;
            }
        }
        else if (score <= 0)
        {
            score = 0;

            if (timeLeft == 0)
            {
                gameWon = false;
                showLostText = true;
                gameOn = false;
            }
        }
        else if (score < scoreLimit && timeLeft == 0)
        {
            gameWon = false;
            showLostText = true;
            gameOn = false;
        }
    }

    public IEnumerator ShutDownGameMusicAndLights()
    {
        yield return new WaitForSeconds(0.5f);

        //Turn the light lower when game is not in play.
        minigameLight.GetComponent<Light>().intensity = 10;

        //Turn off the light 
        //minigameLight.SetActive(false);

        yield return new WaitForSeconds(1.0f);

        float audio = minigameAudio.volume;
        float speed = 0.01f;

        for (float i = audio; i > 0; i -= speed)
        {
            minigameAudio.volume = i;
            yield return null;
        }

        minigameAudio.Stop();

        minigameAudio.volume = 0.7f;
    }

    public IEnumerator WinLoseDisplay()
    {
        if (gameWon)
        {
            WinTickets();

            //Add weapon to the weapon list if it isn't already there.
            if (!WE.weaponList.Contains(playerWeapon))
            {
                WE.weaponList.Add(playerWeapon);
                //Debug.Log("Code is reaching here, adding the player weapon to the array.");
                WE.weaponNumber++;
                WE.isEquipped = true;
            }
        }
        else if (showLostText)
        {
            winLoseText.text = "YOU LOSE!";

            yield return new WaitForSeconds(2);
            ResetGame();
        }
    }

    public IEnumerator CountDownTimer()
    {
        if (timeLeft >= 10)
        {
            timerText.text = ("00:" + (int)timeLeft);
        }
        else
        {
            timerText.text = ("00:0" + (int)timeLeft);
        }

        //Wait so that the starting number is displayed.
        yield return new WaitForSeconds(0.5f);

        //Count down if the game is not paused.
        if (!isPaused)
        {
            timeLeft -= Time.deltaTime;
        }

        //Stop the countdown at 0.
        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
        }
        else
        {
            yield return null;
        }
    }
    #endregion

    #region TAROT CARD MECHANICS
    //
    // TAROT CARDS

    //Show the card that was won
    public void DisplayGameCard()
    {
        //Display the card that was won
        displayScreen.SetActive(true);
        WE.crossHair.SetActive(false);

        //Transition from card display to weapon card
        StartCoroutine(DisplayCardWon());
    }

    //Transition from displayed card to weapon indicator card
    public IEnumerator DisplayCardWon()
    {
        //Wait 1 second before being able to click so the card won screen isn't exited by accident.
        yield return new WaitForSeconds(1);
        //After the wait a click will close the card won screen and return movement to the player.
        yield return new WaitUntil(() => Input.GetButtonDown("Fire1") || Input.anyKeyDown);
        WE.gameMenuDisplayed = false;

        //Turn off card won display screen
        displayScreen.SetActive(false);
        WE.crossHair.SetActive(true);

        //Let player move when the display screen is off.
        FPSController.Instance.GetComponent<CharacterController>().enabled = true;

        //Display the current weapon card
        //menu.DisplayWeaponCard();
        Menu.Instance.DisplayWeaponCard();
        ResetGame();
    }
    #endregion
}
