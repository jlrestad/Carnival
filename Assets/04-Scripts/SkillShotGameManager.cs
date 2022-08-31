using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillShotGameManager : GameBooth
{
    public static SkillShotGameManager Instance;
    public bool gameOver;
    public bool targetFlipped;
    public bool reachedEnd;
    public bool gameJustPlayed;
    //bool levelLoaded;

    public MovingTarget[] movingTarget;

    bool runOnce; //Controls pickupweapon

    private void Awake()
    {
        Instance = this;

        WE = GetWEScript();
    }

    private void Start()
    {
        //Get the sprite from the tarot cards
        inactiveCardSprite = GetInactiveCardSprite();
        activeCardSprite = GetActiveCardSprite();
        timerText = GetTimerText();

        //Set timer info
        timeLeft = GetTimeCounter();

        movingTarget = FindObjectsOfType<MovingTarget>();
    }

    private void Update()
    {
        //GAMEON
        if (gameOn)
        {
            //Hide weapon, if holding one, before holding new weapon.
            if (WE.currentWeapon != null && WE.currentWeapon != WE.gunHold)
            {
                WE.currentWeapon.SetActive(false);
                DisablePreviousActiveCard();
            }

            //Set text for this game
            scoreText = GetScoreText();
            timerText = GetTimerText();
            winLoseText = GetWinLoseText();

            //WEAPON
            playerWeapon.SetActive(true); //Show player holding weapon
            WE.currentWeapon = playerWeapon;
            //Display Proper Tarot if a different weapon was in hand during game start.
            if (WE.haveGun)
            {
               EnableGameActiveCard();
            }

            //TIMER
            StartCoroutine(CountDownTimer());

            //SCORE
            ScoreDisplay();

            //WIN/LOSE
            StartCoroutine(WinLoseDisplay());

            //
            //PAUSE - Game Rules Menu
            if (Input.GetButtonDown("Menu"))
            {
                ShowGameRules();
            }

            if (gameWon)
            {
                WinTickets(3, 1);

                if (!ssWon)
                {
                    DisplayGameCard();
                }
                else
                {
                    ResetGame();
                }

                ssWon = true;
                WE.haveGun = true;
                WE.gameWeapon = null;
                WE.currentWeapon = WE.gunHold;
            }
        }
        else if (!gameOn)
        {
            if (showLostText)
            {
                StartCoroutine(ShutDownGameMusicAndLights());
            }

            // If the game has never been won, then the player does not keep the weapon.
            if (!ssWon)
            {
                playerWeapon.SetActive(false); //Remove weapon from player's hands.
            }
        }
    }

    //
    //CREATES A POOL OF TARGETS
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

    //
    //IF TARGET HAS REACHED THE END THEN GO BACK TO THE BEGINNING
    public void SendOneHome(GameObject trgt, Transform parentPos)
    {
        trgt.GetComponentInChildren<TargetSetActive>().isFlipped = false;
        trgt.GetComponentInChildren<TargetSetActive>().reachedEnd = false;
        trgt.SetActive(false);
        trgt.transform.position = parentPos.position;
    } 

    //
    //CONTROLS THE TARGET MOVEMENT
    public IEnumerator MoveTargets(List<GameObject> pooledTargets, Transform parentPos, int direction, float moveSpeed, float timeBetweenTargets)
    {
        int i = 0; 

        while(i < pooledTargets.Count && !isPaused)
        {
            if(gameOn)
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
