using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

public class GameBooth : MonoBehaviour
{
    //public static GameBooth Instance;

    [Header("UI")]
    public GameObject minigameHUD; //Manually set in Unity game manager script
    public GameObject gameRules; //Manually set in Unity game manager script
    //public TextMeshProUGUI ticketsText;
    public TextMeshProUGUI scoreText; //Manually set in Unity game manager script
    public TextMeshProUGUI timerText; //Manually set in Unity game manager script
    public TextMeshProUGUI winLoseText; //Manually set in Unity game manager script

    [Header("SCORE")]
    [SerializeField] int scoreLimit; //the amount needed to win
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
    public GameObject playerWeapon;
    public Transform gameplayPosition;

    private void Awake()
    {
        //Instance = this;

        Debug.Log("time: " + timeCounter);
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
    public float TimeCounter
    {
        get { return timeCounter; }
        set { timeCounter = value; }
    }

    public float GetTimeCounter()
    {
        return timeCounter;
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
    //GETTERS FOR THE SPRITES
    public Sprite GetActiveCardSprite()
    {
        return activeCard.GetComponent<Image>().sprite;
    }

    public Sprite GetInactiveCardSprite()
    {
        return inactiveCard.GetComponent<Image>().sprite;
    }

    public GameObject GetGameRulesMenu()
    {
        return gameRules.gameObject; 
    }

    // * * *
    //GAME METHODS
    public void ShowGameRules()
    {
        ShowCursor();

        FPSController.Instance.canMove = false;
        gameRules.SetActive(true);
    }

    public void PlayGame()
    {
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
            Time.timeScale = 1;
            gameRules.SetActive(false);
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

        WeaponEquip.Instance.gameRulesDisplayed = false;
        WeaponEquip.Instance.actionPrompt.SetActive(false);
        FPSController.Instance.canMove = true;
        
        minigameHUD.SetActive(false);

        HideCursor();
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

        timeLeft -= Time.deltaTime;
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
