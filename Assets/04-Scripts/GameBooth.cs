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
    [HideInInspector] public int score; //the player kills

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
    public bool isPaused;
    public GameObject playerWeapon;
    public Transform gameplayPosition;
    //public Component gameManagerScript;

    [Header("SCRIPTS")]
    public WeaponEquip WE;
    //public Component className;

    private void Awake()
    {
        Instance = this;
    }

    //Constructor sets the default values.
    public GameBooth()
    {
        this.activeCard = ActiveCard;
        this.inactiveCard = InactiveCard;

        scoreLimit = 20;

        resetTime = timeCounter; //Store this for the reset
        timeLeft = resetTime; //Time left is set to user defined variable of timeCounter
    
    }

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


    // * * *
    //GETTERS

    public TextMeshProUGUI GetTimerText()
    {
        string _isCorrectText = minigameHUD.GetComponentInChildren<MinigameHUD>().tag;
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

    // * * *
    //GAME METHODS
    public void ShowGameRules()
    {
        ShowCursor();

        if (gameOn)
        {
            isPaused = true;
        }

        FPSController.Instance.canMove = false;
        WeaponEquip.Instance.gameRulesDisplayed = true;
        gameRules.SetActive(true);
    }

    public void PlayGame()
    {
        isPaused = false;

        if (!gameOn)
        {
            gameOn = true;
            minigameHUD.SetActive(true);
            gameRules.SetActive(false);
            LockPlayerOnPlay(); //Puts player into game play position.
            HideCursor();
        }
        else
        {
            gameRules.SetActive(false);
            WE.gameRulesDisplayed = false;
            LockPlayerOnPlay();
            HideCursor();
        }
    }

    public void ExitGame()
    {
        ResetGame();
    }

    //RESETS THE GAME BACK TO DEFAULT
    public void ResetGame()
    {
        gameOn = false;
        isPaused = false;

        WE.gameRulesDisplayed = false;
        WE.actionPrompt.SetActive(false);
        FPSController.Instance.canMove = true;
        minigameHUD.SetActive(false);

        //Reset Score
        score = 0;
        scoreText.text = (score + "/" + scoreLimit);

        //Reset Time
        timeLeft = timeCounter;
        timerText.text = ("00:" + (int)timeLeft);

        HideCursor();
        UnLockPlayer();

        //Hide the weapon if the game was not won.
        if (!gameWon)
        {
            playerWeapon.SetActive(false);
        }
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


    public IEnumerator CountDownTimer()
    {
        timerText.text = ("00:" + (int)timeLeft);

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
}
