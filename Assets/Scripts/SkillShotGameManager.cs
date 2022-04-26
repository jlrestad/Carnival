using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillShotGameManager : MonoBehaviour
{
    //public Transform leftPos, rightPos, parentPos;
    [HideInInspector] public WeaponEquip weaponEquip;

    public bool targetFlipped;
    public bool gameOn;
    public bool reachedEnd;
    public bool gameWon;
    bool levelLoaded;

    [Header("UI")]
    public GameObject gameUI;
    public TextMeshProUGUI ticketsText;
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
    public GameObject displayCard;
    public Sprite cardImage;

    [HideInInspector] public bool gameOver;

    private void Awake()
    {
        levelLoaded = true;
    }

    private void Start()
    {
        cardImage = displayCard.GetComponent<Image>().sprite;

        //Tickets
        ticketsText.text = ("Tickets: " + TicketManager.Instance.tickets);
        scoreText.text = (score + "/" + scoreLimit);
        //Timer
        resetTime = timeCounter; //Store this for the reset
        timeLeft = resetTime; //Time left is set to user defined variable of timeCounter

        weaponEquip = FindObjectOfType<WeaponEquip>();
    }

    private void Update()
    {
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
                WinUI();
            }
            else if (score < scoreLimit && timeLeft <= 0 && !gameOver)
            {
                //Display win or lose
                StartCoroutine(LoseUI());
            }

            //Update ticket count
            ticketsText.text = ("Tickets: " + TicketManager.Instance.tickets);

            if (TicketManager.Instance.tickets < 0)
            {
                TicketManager.Instance.tickets = 0;
                ticketsText.text = "NEED TICKETS";

                //Ticket is needed in order to play...
                gameOn = false;
            }
        }
        else if (!gameOn)
        {
            gameUI.SetActive(false);
            ResetGame(); //Reset the variables back to original
        }
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
        //Score
        score = 0;
        scoreText.text = (score + "/" + scoreLimit);

        //Speed
        //minRando = minRandoTemp;
        //maxRando = maxRandoTemp;

        //winloseUI
        //gameUI.SetActive(false);

        //Time
        timeLeft = resetTime;
        timerText.text = ("00:" + (int)timeLeft);
        //gameUI.SetActive(false);

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

    void WinUI()
    {
        DisplayGameCard();
        gameWon = true;
        gameOver = true;
    }

    //Display the win or lose screen for a short time.
    private IEnumerator LoseUI()
    {
        //Display lose message
        winloseText.enabled = true;
        winloseText.text = "You have lost...";
        timerText.text = "";
        gameWon = false;

        yield return new WaitForSeconds(2);

        winloseText.text = (" ");
        winloseText.enabled = false;
        gameOver = true;
    }

    public void DisplayGameCard()
    {
        //Display the card that was won
        displayCard.GetComponent<Image>().enabled = true;

        //Transition from card display back to game display
        StartCoroutine(DisplayCardWon());
    }

    //Transition from displayed card to weapon indicator card
    public IEnumerator DisplayCardWon()
    {
        yield return new WaitForSeconds(1);

        displayCard.GetComponent<Image>().enabled = false;

        //Display the current weapon card
        Menu.Instance.DisplayWeaponCard();

    }

    public void PoolObjects(GameObject targetPrefab, List<GameObject> pooledTargets, int poolAmount, Transform leftPos, Transform rightPos, Transform parentPos, Transform targetParent)
    {
        GameObject target;

        //Pool the amount of targets needed and hold them in a list.
        for (int i = 0; i < poolAmount; i++)
        {
            target = Instantiate(targetPrefab, parentPos, instantiateInWorldSpace: false) as GameObject;
            target.SetActive(false);
            target.transform.parent = targetParent; //Set the targets inside this gameObject folder
            pooledTargets.Add(target);

            //Turn of the collider for the parent object.
            parentPos.GetComponent<BoxCollider>().enabled = false;
        }
    }

    //Move target; give time between each target
    public IEnumerator MoveTargets(List<GameObject> pooledTargets, Transform parentPos, int direction, float moveSpeed, float timeBetweenTargets)
    {
        if (gameOn && weaponEquip.haveGun)
        {
            //Start the targets moving
            for (int i = 0; i < pooledTargets.Count; i++)
            {
                pooledTargets[i].SetActive(true);
                pooledTargets[i].transform.Translate(direction * Vector3.right * (moveSpeed * Time.deltaTime), Space.Self);
                yield return new WaitForSeconds(timeBetweenTargets);
            }

            //When targets reach the end start over at the parent position.
            for (int i = 0; i < pooledTargets.Count; i++)
            {
                //Allow target to be hit again.
                pooledTargets[i].GetComponentInChildren<TargetSetActive>().targetHit = false;

                //Start the loop over
                if (pooledTargets[i].GetComponent<TargetSetActive>().reachedEnd)
                {
                    pooledTargets[i].transform.Translate(Vector3.zero);
                    pooledTargets[i].SetActive(false);

                    pooledTargets[i].transform.position = parentPos.position;

                    yield return new WaitForSeconds(timeBetweenTargets);
                    pooledTargets[i].GetComponent<TargetSetActive>().reachedEnd = false;

                }
            }
        }
    }
    
}
