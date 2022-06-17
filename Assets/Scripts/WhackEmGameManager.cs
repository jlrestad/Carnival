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
    public GameObject[] taunts;
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
    //for taunt loop
    [HideInInspector] public bool tauntCritVisible;
  
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
    public float speedCap;

    [Header("UI")]
    public GameObject gameUI;
    //public TextMeshProUGUI ticketsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winloseText;

    [Header("TAROT CARD")]
    public GameObject displayPickupScreen;
    public GameObject BGCard;
    public Sprite BGImage;
    public Sprite cardImage;

    [Header("AUDIO & LIGHTS")]
    public AudioSource minigameAudio;
    public GameObject minigameLight;

    [Space(10)]
    /*[HideInInspector]*/ public bool gameWon;
    /*[HideInInspector]*/ public bool gameOver;
    float randomPopUpTime;
    float randomTauntTime;
    float randomStayTime;
    public bool levelLoaded;
    [HideInInspector] bool stopPopUp;
    [HideInInspector] public GameObject currentWeapon;
    bool runOnce; //Controls pickupweapon
    Menu menu;
    [HideInInspector] public GameObject gameWeapon;
    [HideInInspector] public GameObject playerWeapon;

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
        //Default setting if it isn't manually set.
        if (speedCap == 0)
        {
            speedCap = 0.5f;
        }

        weaponEquip = FindObjectOfType<WeaponEquip>();
        menu = FindObjectOfType<Menu>();

        //Tarot Cards
        cardImage = GameObject.FindGameObjectWithTag(("MeleeGame")).GetComponentInChildren<GameCard>().GetComponent<Image>().sprite;
        BGImage = BGCard.GetComponent<Image>().sprite;

        //Tickets
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
        //Run this when the WhackEm game is on.
        if (gameOn)
        {
            if (!weaponEquip.haveMallet && !runOnce)
            {
                //weaponEquip.PickUpWeapon();
                runOnce = true; 
                weaponEquip.haveMallet = true; 
                weaponEquip.crossHair.SetActive(true);
            }
            
            if (!minigameAudio.isPlaying || !minigameLight.activeInHierarchy)
            {
                minigameLight.SetActive(true);

                if (minigameAudio.volume == 0)
                {
                    minigameAudio.volume = 0.5f;
                }
                minigameAudio.Play();
            }

            // fixes bug causing mouse to appear when critter pops up
            Cursor.lockState = CursorLockMode.Locked;

            //Display the game UI
            DisplayTextUI();

            //Begin the game timer
            if (!stopPopUp)
            {
                StartCoroutine(CountDownTimer());
                //Foratting
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
                gameWon = true;
                StartCoroutine(WinLoseUI());
            }
            else if (score < scoreLimit && timeLeft <= 0 && !gameOver)
            {
                gameWon = false;
                StartCoroutine(WinLoseUI());
            }

            //Update ticket count
            HudManager.Instance.DisplayTicketAmount();

            if (HudManager.Instance.redTickets < 0)
            {
                HudManager.Instance.redTickets = 0;
                //winloseText.text = "NEED TICKETS";

                //Ticket is needed in order to play...
                gameOn = false;
            }
        }
        else if (gameOver)
        {
            StartCoroutine(ShutDownGame());
        }
    }

    IEnumerator ShutDownGame()
    {
        yield return new WaitForSeconds(1.5f);

        float audio = minigameAudio.volume;
        float speed = 0.01f;

        for (float i = audio; i > 0; i -= speed)
        {
            minigameAudio.volume = i;
            yield return null;
        }

        //Turn off the light 
        minigameLight.SetActive(false);

        minigameAudio.Stop();

        minigameAudio.volume = 0.5f;
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
        runOnce = false; 
        stopPopUp = false;
        gameJustFinished = false;
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

        if (!gameWon)
        {
            //* Put weapon back
            weaponEquip.haveMallet = false;
            weaponEquip.crossHair.SetActive(false);
            gameWeapon.SetActive(true);
            playerWeapon.SetActive(false);
        }

    }

    public void IncreaseSpeed()
    {
        if( (minRando / divideSpeedBy) > speedCap)
        {
            minRando /= divideSpeedBy;
        }

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

    //Display the win or lose screen for a short time.
    IEnumerator WinLoseUI()
    {
        //Display lose message
        winloseText.enabled = true;
        //gameOver = true;

        if (gameWon)
        {
            winloseText.text = "You have won...";

            if (!weaponEquip.weaponList.Contains(currentWeapon))
            {
                weaponEquip.weaponList.Add(currentWeapon);
                weaponEquip.currentWeapon = currentWeapon;
                weaponEquip.weaponNumber++;
                weaponEquip.isEquipped = true;
            }

            DisplayGameCard();
        } else
        {
            winloseText.text = "YOU LOSE...";
        }

        stopPopUp = true;
        gameOn = false;
        //gameOver = false;
        gameJustFinished = true;
        weaponEquip.whackEmActive = false;
        timerText.enabled = false;
        
        yield return new WaitForSeconds(2);
        ResetGame();

        CarnivalSmashTrigger.Instance.UnLockPlayer();
        winloseText.text = (" ");
        winloseText.enabled = false;
        gameOver = true;
        gameUI.SetActive(false);
    }

    //Choose random enemy with random appear times
    IEnumerator EnemyPopUp()
    {
        while (levelLoaded) //Allow coroutine to load on Start. This way it isn't being updated every frame.
        {
            while (gameOn && !stopPopUp && weaponEquip.haveMallet) //But don't do anything until the game is on.
            {
                // create queue of custom class (params to call)
                //Debug.Log("Entered Game");

                while (!gameJustFinished && !gameWon)
                {
                    WhackEmRoutine routine = new WhackEmRoutine();
                    int critUp = routine.up, critTaunt = routine.taunt;
                    bool tauntBool = routine.addTaunt;
                    
                    //Check if the critter has shown itself.
                    critterIsVisible = critters[critUp].GetComponent<WhackEmEnemy>().isVis; //check if current critter is visible
                    tauntCritVisible = critters[critTaunt].GetComponent<WhackEmEnemy>().isVis; //check if taunt critter is visible
                    
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

    //
    // TAROT CARD SYSTEM
    
    //Show the card that was won
    public void DisplayGameCard()
    {
        //Display the card that was won
        displayPickupScreen.SetActive(true);

        //Transition from card display to weapon card
        StartCoroutine(DisplayCardWon());
    }

    //Transition from displayed card to weapon indicator card
    public IEnumerator DisplayCardWon()
    {
        //Wait 1 second before being able to click so the card won screen isn't exited by accident.
        yield return new WaitForSeconds(1);
        //After the wait a click will close the card won screen and return movement to the player.
        yield return new WaitUntil(() => Input.GetButtonDown("Fire1"));

        //Turn off card won display screen
        displayPickupScreen.SetActive(false);

        //Let player move when the display screen is off.
        FPSController.Instance.GetComponent<CharacterController>().enabled = true;

        //Display the current weapon card
        menu.DisplayWeaponCard();
        //Menu.Instance.DisplayWeaponCard();
    }

}
