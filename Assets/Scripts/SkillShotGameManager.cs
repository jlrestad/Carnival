using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillShotGameManager : MonoBehaviour
{
    public static SkillShotGameManager Instance;

    //public Transform leftPos, rightPos, parentPos;
    [HideInInspector] public WeaponEquip weaponEquip;

    public bool targetFlipped;
    public bool gameOn;
    public bool reachedEnd;
    public bool gameWon;
    public bool gameJustPlayed;
    bool levelLoaded;

    [Header("UI")]
    public GameObject gameUI;
    //public TextMeshProUGUI ticketsText;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI timerText;
    public TextMeshProUGUI winloseText;

    //[Header("SPEED")]
    //public float minRando; private float minRandoTemp;
    //public float maxRando; private float maxRandoTemp;

    [Header("SCORE")]
    [SerializeField] int scoreLimit; //the amount needed to win
    [HideInInspector] public int score; //the player kills

    [Header("TIMER")]
    [SerializeField] private float timeCounter = 30; //used to count down the time
    private float timeLeft; //used to set the amount of time to countdown by
    private float resetTime;

    [Header("TAROT CARD")]
    public GameObject displayPickupScreen;
    public GameObject BGCard;
    public Sprite cardImage;
    public Sprite BGImage;

    [HideInInspector] public bool gameOver;

    public MovingTarget[] movingTarget;

    //public GameObject minigameAudio;
    public AudioSource minigameAudio;
    public GameObject minigameLight;
    bool runOnce; //Controls pickupweapon
    [HideInInspector] public GameObject gameWeapon;
    [HideInInspector] public GameObject playerWeapon;

    [HideInInspector] public GameObject currentWeapon; //The new weapon to be added to weaponList of WE.

    private void Awake()
    {
        Instance = this; 

        levelLoaded = true;
    }

    private void Start()
    {
        //Tarot Cards
        cardImage = GameObject.FindGameObjectWithTag(("ShootingGame")).GetComponentInChildren<GameCard>().GetComponent<Image>().sprite;
        BGImage = BGCard.GetComponent<Image>().sprite;

        //Tickets
        //ticketsText.text = ("Tickets: " + HudManager.Instance.redTickets);
        scoreText.text = (score + "/" + scoreLimit);
        //Timer
        resetTime = timeCounter; //Store this for the reset
        timeLeft = resetTime; //Time left is set to user defined variable of timeCounter

        weaponEquip = FindObjectOfType<WeaponEquip>();

        movingTarget = FindObjectsOfType<MovingTarget>();
    }

    private void Update()
    {
        if (gameOn)
        {
            if (!weaponEquip.haveGun && !runOnce)
            {
                //weaponEquip.PickUpWeapon();
                runOnce = true;
                weaponEquip.haveGun = true;
                weaponEquip.crossHair.SetActive(true);
            }
            


            if (! minigameAudio.isPlaying || ! minigameLight.activeInHierarchy)
            {
                minigameLight.SetActive(true);

                if (minigameAudio.volume == 0)
                {
                    minigameAudio.volume = 0.5f;
                }
                minigameAudio.Play();
            }
        }
        //Run this when the WhackEm game is on.
        if (gameOn && weaponEquip.haveGun)
        {
            //Display the game UI
            DisplayTextUI();

            //Begin the game timer
            StartCoroutine(CountDownTimer());
            if (timeLeft >= 10)
            {
                timerText.text = ("00:" + (int)timeLeft);
            }
            else
            {
                timerText.text = ("00:0" + (int)timeLeft);
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
            //ticketsText.text = ("Tickets: " + HudManager.Instance.redTickets);

            if (HudManager.Instance.redTickets < 0)
            {
                HudManager.Instance.redTickets = 0;
                //winLoseText.text = "NEED TICKETS";

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
        yield return new WaitForSeconds(2);

        float audio = minigameAudio.volume;
        float speed = 0.01f;

        for (float i = audio; i > 0; i -= speed)
        {
            minigameAudio.volume = i;
            yield return null;
        }

        minigameAudio.Stop();

        minigameAudio.volume = 0.5f;
    }

    
    public void DisplayTextUI()
    {
        //Display the scoreUI
        gameUI.SetActive(true);
        scoreText.text = (score + "/" + scoreLimit);

        //Display the timerUI
        timerText.enabled = true; ;
    }

    public void ResetGame()
    {
        runOnce = false; 

        //weaponEquip.skillshotActive = false;

        //Score
        score = 0;
        scoreText.text = (score + "/" + scoreLimit);

        //Speed
        //minRando = minRandoTemp;
        //maxRando = maxRandoTemp;

        //Time
        timeLeft = resetTime;
        timerText.text = ("00:" + (int)timeLeft);

        if (!gameWon)
        {
            //* Put weapon back
            weaponEquip.haveGun = false;
            weaponEquip.crossHair.SetActive(false);
            gameWeapon.SetActive(true);
            playerWeapon.SetActive(false);
        }
    }


    IEnumerator CountDownTimer()
    {
        //Wait for 1 second so that the starting number is displayed.
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

    IEnumerator WinLoseUI()
    {
        //Display lose message
        winloseText.enabled = true;

        if (gameWon)
        {
            winloseText.text = "YOU WIN!";

            if (!weaponEquip.weaponList.Contains(currentWeapon))
            {
                weaponEquip.weaponList.Add(currentWeapon);
                weaponEquip.currentWeapon = currentWeapon;
                weaponEquip.weaponNumber++;
                weaponEquip.isEquipped = true;
            }

            DisplayGameCard();
        }
        else
        {
            winloseText.text = "YOU LOSE...";
            ResetGame();
        }

        gameOn = false;
        gameJustPlayed = true;
        weaponEquip.skillshotActive = false;
        timerText.enabled = false;
      
        yield return new WaitForSeconds(2);

        // lock player here until WinLoseUI done--
        //if player leaves trigger area before this loop finishes, cannot win replay
        //Clear and turn off lose message
        
        CarnivalSmashTrigger.Instance.UnLockPlayer();
        winloseText.text = (" ");
        winloseText.enabled = false;
        gameOver = true;
        gameUI.SetActive(false);
    }

    public void DisplayGameCard()
    {
        //Display the card that was won
        displayPickupScreen.SetActive(true);

        //Transition from card display back to game display
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
        //Turn off the light 
        minigameLight.SetActive(false);

        //Display the current weapon card
        Menu.Instance.DisplayWeaponCard();

    }
    
    public void PoolObjects(GameObject targetPrefab, List<GameObject> pooledTargets, int poolAmount, Transform parentPos, Transform targetParent)
    {
        GameObject target;
        poolAmount = (int)timeCounter - 2;
        //Pool the amount of targets needed and hold them in a list.
        while(pooledTargets.Count < poolAmount)
        {
            target = Instantiate(targetPrefab, parentPos, instantiateInWorldSpace: false) as GameObject;
            target.SetActive(false);
            target.transform.parent = targetParent; //Set the targets inside this gameObject folder
            pooledTargets.Add(target);

            //Turn of the collider for the parent object.
            parentPos.GetComponent<BoxCollider>().enabled = false;
        }
    }

    public void SendOneHome(GameObject trgt, Transform parentPos)
    {
        trgt.SetActive(false);
        trgt.GetComponentInChildren<TargetSetActive>().reachedEnd = false;
        trgt.transform.position = parentPos.position;
    } 

    //Move target; give time between each target
    public IEnumerator MoveTargets(List<GameObject> pooledTargets, Transform parentPos, int direction, float moveSpeed, float timeBetweenTargets)
    {
        int i = 0; //undo to here!

        while(i < pooledTargets.Count)
        {
            if(gameOn && weaponEquip.haveGun)
            {
                //if target at beginning or end, turn off
                if (pooledTargets[i].transform.position == parentPos.position || pooledTargets[i].GetComponentInChildren<TargetSetActive>().reachedEnd)
                {
                    pooledTargets[i].SetActive(false);
                }

                //call translate while it hasn't reached end
                if (!pooledTargets[i].GetComponentInChildren<TargetSetActive>().reachedEnd && !pooledTargets[i].GetComponentInChildren<TargetSetActive>().hasGone)
                {
                    pooledTargets[i].SetActive(true);
                    pooledTargets[i].transform.Translate(direction * Vector3.right * (moveSpeed * Time.deltaTime), Space.Self);
                }

                yield return new WaitForSeconds(timeBetweenTargets);

                if (pooledTargets[i].GetComponentInChildren<TargetSetActive>().reachedEnd)
                {
                    SendOneHome(pooledTargets[i], parentPos);
                }
                i++;
            }
            else
            {   
                SendOneHome(pooledTargets[i], parentPos);
                i++;
            }
        }
    }
    

}
