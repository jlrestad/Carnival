using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class CasketManager : MonoBehaviour
{
    public static CasketManager Instance;

    /*
     * This script manages the functions for the caskets in the minigame "Casket Baskets," including movement and interactivity
     * Note that this script only manages these functions for the individual gameobject it is attached to.
     * Also note this script only supports vertical movement of the coffins and must be modified to support horizontal movement.
     * Grant Hargraves 8/2022
     */
    //==================================================
    //=========================|FIELDS|
    //==================================================
    #region FIELDS
    [Header("STATS")]//--------------------------------------------------|STATS|
    [Tooltip("The speed this coffin will move up and down at the beginning of the game.")]
    [SerializeField] float baseMoveSpeed;
    [Tooltip("The highest possible speed this coffin is allowed to move up and down.")]
    [SerializeField] float maximumMoveSpeed;
    [Tooltip("The amount of time the coffin will spend shaking before it opens.")]
    [SerializeField] float shakeTime;
    [Tooltip("The base amount of time between the coffin choosing a new position to move to. Lower = more spastic movement.")]
    [SerializeField] float baseGoalShiftTime = 1;
    [Tooltip("The minimum amount of time between the coffin choosing a new position to move to. Lower = more spastic movement.")]
    [SerializeField] float minimumGoalShiftTime = 1;
    [Tooltip("The minimum distance the coffin will move each time it shifts positions. Increase for more polar results.")]
    [SerializeField] float minimumGoalShiftDistance = 0.2f;
    [Tooltip("How long the coffin needs to stay closed before opening again.")]
    [SerializeField] float closedTimer = 2f;
    [Space(5)]

    [Header("PLUG-INS")]//--------------------------------------------------|PLUG-INS|
    [Tooltip("A gameobject located the farthest down on the track this coffin is allowed to go.")]
    [SerializeField] GameObject bottomPosition;
    [Tooltip("A gameobject located the farthest up on the track this coffin is allowed to go.")]
    [SerializeField] GameObject topPosition;
    [Tooltip("DEBUG ONLY: The gameobject that will change color to show when open or closed.")]
    [SerializeField] GameObject coffinModel;
    [Space(20)]
    //insert reference to animator
    [SerializeField] Animator animator;
    [Tooltip("Any functional AudioSource inside the parent object. Must be set to loop to work properly.")]
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip CBClose;
    [SerializeField] AudioClip CBOpen;
    [SerializeField] AudioClip CBMove;
    [SerializeField] AudioClip CBShake;
    [SerializeField] AudioClip CBStop;

    [Space(5)]

    [Header("INTERNAL/DEBUG")]//--------------------------------------------------|INTERNAL/DEBUG|
    [Tooltip("How fast the coffin will currently move up and down along the track.")]
    [SerializeField] float currentSpeed;
    [Tooltip("The position the coffin is currently heading toward.")]
    [SerializeField] Vector3 currentGoal;
    [Tooltip("Keeps track of whether the coffin is currently open.")]
    public bool isOpen = false;
    [Tooltip("Tracks whether the coffin can be opened or not. ")]
    public bool canOpen = false;
    [Tooltip("Keeps track of how long this coffin should wait before moving. Note this value is slightly randomized.")]
    [SerializeField] float currentGoalShiftTime;
    [Tooltip("A reference to the skull that makes it inside the coffin so that it can be reused afterwards.")]
    [SerializeField] Head hitSkull;
    [Tooltip("Tracker for how long the coffin needs to stay closed before opening again.")]
    [SerializeField] float currentClosedTimer = 2f;
    [Tooltip("Tracks whether the coffin is moving or not to allow sounds and effects to play properly.")]
    [SerializeField] bool moving;
    #endregion
    //==================================================
    //=========================|BUILT-IN METHODS|
    //==================================================
    #region BUILT-IN METHODS
    //--------------------------------------------------|Start|
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        currentGoal = transform.position;

        animator = GetComponentInChildren<Animator>();
    }

    //--------------------------------------------------|Update|
    private void Update()
    {
        if(gameObject.transform.position != currentGoal) //if not currently at our goal position...
        {
            //Casket movement will stop when game is paused.
            if (!CasketBasketsGameManager.Instance.isPaused)
            {
                transform.position = Vector3.MoveTowards(transform.position, currentGoal, currentSpeed * Time.deltaTime); //move toward the goal at the speed of currentspeed
            }

            //-----SOUND EFFECTS-----
            if (!moving) //if we just started moving...
            {
                myAudio.clip = CBMove;
                myAudio.Play(); //play the "moving" sound in a loop
                moving = true; //trigger moving so that this doesn't play every frame
            }
        }
        else if(gameObject.transform.position == currentGoal && moving) //if we ARE at our goal position and just stopped moving...
        {
            moving = false; //turn off moving so that this doesn't play every frame
            myAudio.Stop();
            myAudio.PlayOneShot(CBStop);
        }

        if (!CasketBasketsGameManager.Instance.gameOn)
        {
            animator.SetBool("open", false);
            animator.SetBool("shaking", false);
            CasketBasketsGameManager.Instance.score = 0;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        //if this object is a trigger area inside the coffin, the coffin is open, and a "head" object enters the trigger area...
        if(isOpen && gameObject.tag == "Goal" && other.gameObject.tag == "Head")
        {
            CasketBasketsGameManager.Instance.RegisterHit();

            CloseFinish(); //close the casket and activate the functions for when the player gets a hit.
            hitSkull = other.gameObject.GetComponent<Head>(); //save a reference to this object so it can be disabled later.
            if (hitSkull == null) Debug.LogWarning("Skull was missing a 'Head' script. May not behave correctly.");
        }
    }
    #endregion
    //==================================================
    //=========================|CUSTOM METHODS|
    //==================================================
    #region CUSTOM METHODS
    //--------------------------------------------------|shiftGoal|
    //set a new position for the coffin to move toward between the top and bottom available positions
    public void shiftGoal()
    {
        StopCoroutine(goalShiftTimer()); //stops the coroutine so that it does not infinitely nest
        float rando = Random.Range(bottomPosition.transform.position.y, topPosition.transform.position.y); //randomize the goal Y
        Vector3 randoGoal = new Vector3(currentGoal.x, rando, currentGoal.z); //set the new goal position

        //-----Below increases the distance of the goal shift while keeping it random-----
        if (Vector3.Distance(transform.position, randoGoal) < minimumGoalShiftDistance) //if the amount it would move is less than the minimum...
        {
            rando *= rando; //multiply rando times itself to increase the distance, then keep it within bounds
            if (rando > topPosition.transform.position.y) rando = topPosition.transform.position.y;
            if (rando < bottomPosition.transform.position.y) rando = bottomPosition.transform.position.y;
            randoGoal = new Vector3(currentGoal.x, rando, currentGoal.z);
        }
        //-----

        currentGoal = randoGoal; //set the new goal position
        StartCoroutine(goalShiftTimer());  //wait until it's time to do this again
    }

    //--------------------------------------------------|AttemptOpen|
    public void AttemptOpen()
    {
        if(canOpen && !isOpen) //if the coffin is closed and the closed timer has run to zero...
        {
            StartCoroutine("OpenStart");
            animator.SetBool("open", false);
            animator.SetBool("shaking", true);
        }
    }

    //--------------------------------------------------|CloseFinish|
    public void CloseFinish()
    {
        //animation (close doors)
        animator.SetBool("open", false);

        //sfx

        isOpen = false;
        
        //CasketBasketsGameManager.Instance.score--; //utilize the score variable as a way of tracking how many coffins are currently open. Reduces by one.
        //Debug.Log("Coffin Hit");
        //CasketBasketsGameManager.Instance.RegisterHit(); //tell the parent class that the player scored a hit
    }

    //--------------------------------------------------|OpenFinish|
    public void OpenFinish()
    {
        //animation (open doors)
        animator.SetBool("open", true);
        animator.SetBool("shaking", false);

        //sfx

        isOpen = true;

        CasketBasketsGameManager.Instance.score++; //utilize the score variable as a way of tracking how many coffins are currently open. Adds one.
    }

    //--------------------------------------------------|AddSpeed|
    public void AddSpeed(float amt)
    {
        //keep the coffin's speed within bounds
        if (currentSpeed + amt >= maximumMoveSpeed) currentSpeed = maximumMoveSpeed;
        if (currentSpeed + amt <= baseMoveSpeed) currentSpeed = baseMoveSpeed;
        currentSpeed += amt; //add the extra speed to movement speed
    }

    //--------------------------------------------------|ReduceGoalShiftTime|
    public void ReduceGoalShiftTime(float amt)
    {
        if (currentGoalShiftTime + amt >= baseGoalShiftTime) currentGoalShiftTime = baseGoalShiftTime;
        if (currentGoalShiftTime + amt <= minimumGoalShiftTime) currentGoalShiftTime = minimumGoalShiftTime;
        currentGoalShiftTime -= amt; //reduce the goalshifttime by amt
    }

    //--------------------------------------------------|ReduceClosedTime|
    public void ReduceClosedTime(float amt)
    {
        if(closedTimer - amt > 0.0001)
        {
            closedTimer -= amt;
        }
    }

    //--------------------------------------------------|CoffinStart|
    public void CoffinStart()
    {
        
        StartCoroutine(goalShiftTimer());
        StartCoroutine(CoffinClosedTimer());
    }

    //--------------------------------------------------|CoffinReset|
    public void CoffinReset()
    {
        currentGoal.y = bottomPosition.transform.position.y; //move to the bottom
        animator.SetBool("open", false);
        animator.SetBool("shaking", false);

        canOpen = true;
        isOpen = false;
        currentSpeed = baseMoveSpeed; //set speed to base
        currentGoalShiftTime = baseGoalShiftTime; //set goal shift time to base
        currentClosedTimer = closedTimer; //set closed timer to base

        CasketBasketsGameManager.Instance.score = 0;
        StopAllCoroutines();
    }
    #endregion
    //==================================================
    //=========================|COROUTINES|
    //==================================================
    #region COROUTINES
    public IEnumerator goalShiftTimer()
    {
        float rando = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(currentGoalShiftTime * rando);
        shiftGoal();
    }

    private IEnumerator OpenStart()
    {
        //animation (shake)
        animator.SetBool("shaking", true);
        animator.SetBool("open", false);

        //-----DEBUG ONLY-----
        myAudio.PlayOneShot(CBShake);
        //-----
        //sfx
        yield return new WaitForSeconds(shakeTime);
        OpenFinish();
    }

    private IEnumerator CoffinClosedTimer()
    {
        canOpen = false;
        yield return new WaitForSeconds(currentClosedTimer);
        canOpen = true;
    }
    #endregion
}
