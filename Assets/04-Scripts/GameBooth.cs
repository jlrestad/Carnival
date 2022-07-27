using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEditor;

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
    [SerializeField] int scoreLimit; //the amount needed to win
    [HideInInspector] public int score; //the player kills

    [Header("TIMER")]
    [SerializeField] private float timeCounter; //used to count down the time
    private float timeLeft; //used to set the amount of time to countdown by
    private float resetTime;

    [Header("TAROT CARD")]
    public GameObject displayScreen; //PlayerHUD game menu - manually set in Unity game manager script
    public GameObject activeCard; //Game card for active weapon - manually set in Unity game manager script
    public GameObject inactiveCard; //Game card for inactive weapon - manually set in Unity game manager scrit
    public Sprite inactiveCardSprite;
    public Sprite activeCardSprite;

    public bool gameOn;

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
        timeCounter = 60.0f;
    }

    //Set & Get the active card
    public GameObject ActiveCard
    {
        get { return activeCard; }
        set { activeCard = value; }
    }

    //Set and Get the inactive card
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
    public void PlayGame()
    {
        gameOn = true;
        WeaponEquip.Instance.gameRulesDisplayed = false;
        minigameHUD.SetActive(true);
    }

    public void ExitGame()
    {
        gameOn = false;
        WeaponEquip.Instance.gameRulesDisplayed = false;
        minigameHUD.SetActive(false);
    }

    public void ResetGame()
    {
        //Will reset things back to default
        gameOn = false;

        WeaponEquip.Instance.actionPrompt.SetActive(false);

        HideCursor();
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
}
