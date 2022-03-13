using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SkillShotGameManager : MonoBehaviour
{
    //public Transform leftPos, rightPos, parentPos;
    WeaponEquip weaponEquip;

    public bool targetFlipped;
    public bool gameOn;
    public bool reachedEnd;
    bool levelLoaded;
    bool gameWon;

    [Header("UI")]
    public GameObject ticketsUI;
    public GameObject scoreUI;
    public GameObject timerUI;
    public GameObject winloseUI;
    [HideInInspector] public TextMeshProUGUI ticketsText;
    [HideInInspector] public TextMeshProUGUI scoreText;
    [HideInInspector] public TextMeshProUGUI timerText;
    [HideInInspector] public TextMeshProUGUI winloseText;

    [Header("SPEED")]
    //public float minRando; private float minRandoTemp;
    //public float maxRando; private float maxRandoTemp;

    [Header("SCORE")]
    [HideInInspector] public int score; //the player kills
    [SerializeField] int scoreLimit; //the amount needed to win

    [Header("TIMER")]
    [SerializeField] private float timeCounter = 30; //used to count down the time
    private float timeLeft; //used to set the amount of time to countdown by
    private float resetTime;

    [Space(15)]
    //public GameObject targetPrefab;
    public float gameDelayTime = 1.0f;


    [HideInInspector] public bool gameOver;

    private void Awake()
    {
        levelLoaded = true;
    }

    private void Start()
    {
        //Text
        ticketsText = ticketsUI.GetComponentInChildren<TextMeshProUGUI>();
        scoreText = scoreUI.GetComponentInChildren<TextMeshProUGUI>();
        timerText = timerUI.GetComponentInChildren<TextMeshProUGUI>();
        winloseText = winloseUI.GetComponentInChildren<TextMeshProUGUI>();
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
            DisplayUI();

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

            //Display win or lose
            StartCoroutine(WinLoseManager());

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
            scoreUI.SetActive(false);
            ResetGame(); //Reset the variables back to original
        }
    }


    public void DisplayUI()
    {
        //Display the scoreUI
        scoreUI.SetActive(true);
        scoreText.text = (score + "/" + scoreLimit);

        //Display the timerUI
        timerUI.SetActive(true);
        //timerText.text = ("00:" + (int)timeCounter);
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
        winloseUI.SetActive(false);
        winloseText.text = (" ");

        //Time
        timeLeft = resetTime;
        timerText.text = ("00:" + (int)timeLeft);
        timerUI.SetActive(false);

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

    //Display the win or lose screen for a short time.
    IEnumerator WinLoseManager()
    {
        if (score >= scoreLimit && timeLeft > 0 && !gameOver)
        {
            winloseUI.SetActive(true);
            winloseText.text = "You have won...";
            gameWon = true;

            yield return new WaitForSeconds(2);

            winloseUI.SetActive(false);
            gameOver = true;
        }
        if (score < scoreLimit && timeLeft <= 0 && !gameOver)
        {
            winloseUI.SetActive(true);
            winloseText.text = "You have lost...";
            gameWon = false;

            yield return new WaitForSeconds(2);

            winloseUI.SetActive(false);
            gameOver = true;
        }

        yield return null;
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
