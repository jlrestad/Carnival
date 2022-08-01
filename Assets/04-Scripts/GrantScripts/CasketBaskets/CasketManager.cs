using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CasketManager : MonoBehaviour
{
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
    [Header("STATS")]
    [Tooltip("The speed this coffin will move up and down at the beginning of the game.")]
    [SerializeField] float baseMoveSpeed;
    [Tooltip("The highest possible speed this coffin is allowed to move up and down.")]
    [SerializeField] float maximumMoveSpeed;
    [Tooltip("The base time between when this coffin was last shut and can open again.")]
    [SerializeField] float baseClosedTime;
    [Tooltip("The minimum amount of time between when this coffin was last shut and can open again.")]
    [SerializeField] float minimumClosedTime;
    [Tooltip("The amount of time the coffin will spend shaking before it opens.")]
    [SerializeField] float shakeTime;
    [Tooltip("The base amount of time between the coffin choosing a new position to move to. Lower = more spastic movement.")]
    [SerializeField] float baseGoalShiftTime;
    [Tooltip("The minimum amount of time between the coffin choosing a new position to move to. Lower = more spastic movement.")]
    [SerializeField] float minimumGoalShiftTime;
    [Tooltip("The minimum distance the coffin will move each time it shifts positions. Increase for more polar results.")]
    [SerializeField] float minimumGoalShiftDistance;
    [Space(5)]

    [Header("PLUG-INS")]
    [Tooltip("A gameobject located the farthest down on the track this coffin is allowed to go.")]
    [SerializeField] GameObject bottomPosition;
    [Tooltip("A gameobject located the farthest up on the track this coffin is allowed to go.")]
    [SerializeField] GameObject topPosition;
    //insert reference to animator
    //insert reference to audio sources
    [Space(5)]

    [Header("INTERNAL/DEBUG")]
    [Tooltip("How fast the coffin will currently move up and down along the track.")]
    [SerializeField] float currentSpeed;
    [Tooltip("The position the coffin is currently heading toward.")]
    [SerializeField] Vector3 currentGoal;
    [Tooltip("Keeps track of whether the coffin is currently open.")]
    public bool isOpen = false;
    [Tooltip("Keeps track of how long the coffin has been closed.")]
    [SerializeField] float currentClosedTimer = 0f;
    [Tooltip("Keeps track of how long this coffin should wait before moving. Note this value is slightly randomized.")]
    [SerializeField] float currentGoalShiftTime;


    #endregion
    //==================================================
    //=========================|BUILT-IN METHODS|
    //==================================================
    private void Start()
    {
        currentGoal = transform.position;
        StartCoroutine(goalShiftTimer());
    }

    private void Update()
    {
        if(gameObject.transform.position != currentGoal) //if not currently at our goal position...
        {
            transform.position = Vector3.MoveTowards(transform.position, currentGoal, currentSpeed); //move toward the goal at the speed of currentspeed
        }
    }

    //==================================================
    //=========================|CUSTOM METHODS|
    //==================================================

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
            rando *= rando; //multiply rando times itself to increase the distance
            if (rando > topPosition.transform.position.y) rando = topPosition.transform.position.y; //set to the top position if it would have gone higher
            if (rando < bottomPosition.transform.position.y) rando = bottomPosition.transform.position.y; //set to the bottom position if it would have gone lower
            randoGoal = new Vector3(currentGoal.x, rando, currentGoal.z); //set the randogoal position instead to the new amount
        }
        //-----

        currentGoal = randoGoal; //set the new goal position
        StartCoroutine(goalShiftTimer());  //wait until it's time to do this again
    }

    //--------------------------------------------------|attemptOpen|
    public void attemptOpen()
    {
        if(! isOpen && currentClosedTimer <= 0) //if the coffin is closed and the closed timer has run to zero...
        {
            StartCoroutine("OpenStart");
        }
    }

    //==================================================
    //=========================|COROUTINES|
    //==================================================
    private IEnumerator goalShiftTimer()
    {
        float rando = Random.Range(0.5f, 2f);
        yield return new WaitForSeconds(currentGoalShiftTime * rando);
        shiftGoal();
    }
}
