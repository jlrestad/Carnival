using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SkillShotGameManager : GameBooth
{
    public static SkillShotGameManager Instance;
    [SerializeField] GameObject saveCurrentWeapon; //Used to save current weapon info for re-equip

    public bool gameOver;
    public bool targetFlipped;
    public bool reachedEnd;
    public bool gameJustPlayed;
    public List<GameObject> targetRows;
    public MovingTarget[] movingTarget;

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
            //WEAPON EQUIP
            //1. Hide weapon, if holding one, before holding this game's weapon, & disable the active Tarot card for it.
            if (WE.currentWeapon != null && WE.currentWeapon != playerWeapon)
            {
                weaponListIndex = WE.weaponList.IndexOf(WE.currentWeapon); //Get index of current weapon
                WE.weaponCards[weaponListIndex].GetComponent<Image>().enabled = false; //Hide the tarot of current weapon
                WE.currentWeapon.SetActive(false); //Hide the weapon
                saveCurrentWeapon = WE.currentWeapon; //Store this so it can be equipped
            }

            //2. Equip this game's weapon & assign to current weapon
            playerWeapon.SetActive(true); //Show player holding weapon
            WE.currentWeapon = playerWeapon;
            
            //3. Display proper Tarot for this weapon if game was won.
            if (WE.haveGun)
            {
                //EnableGameActiveCard();
                int index = WE.weaponList.IndexOf(playerWeapon); //Get the index of this weapon in the list
                WE.weaponCards[index].GetComponent<Image>().enabled = true; //Show the Tarot for this weapon
            }

            //Set text for this game
            scoreText = GetScoreText();
            timerText = GetTimerText();
            winLoseText = GetWinLoseText();

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
                WinTickets(2, 1);

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
                // If the game has never been won, then the player does not keep the weapon.
                if (!ssWon)
                {
                    playerWeapon.SetActive(false); //Remove weapon from player's hands.
                }
                //If the game is lost and player has weapons, re-equip the last held weapon.
                if (WE.weaponList.Count > 0)
                {
                    if (saveCurrentWeapon != null)
                    {
                        WE.currentWeapon = saveCurrentWeapon; //Assign current weapon to the previously held weapon.
                    }

                    WE.weaponCards[weaponListIndex].GetComponent<Image>().enabled = true; //Show the tarot of last held weapon
                    WE.currentWeapon.SetActive(true); //Show player holding the weapon.
                }
            }
            //If the game is quit and the weapon list is empty, set the current weapon to null.
            if (WE.weaponList.Count == 0)
            {
                WE.currentWeapon = null;
                playerWeapon.SetActive(false); //Remove weapon from player's hands.
            }
            else if (WE.weaponList.Count > 0 && !ssWon)
            {
                //Re-equip the weapon that was held before playing new minigame.
                if (saveCurrentWeapon != null)
                {
                    playerWeapon.SetActive(false); //Remove weapon from player's hands.
                    WE.currentWeapon = saveCurrentWeapon;
                    WE.weaponCards[weaponListIndex].GetComponent<Image>().enabled = true; //Show the tarot of last held weapon
                    WE.currentWeapon.SetActive(true); //Show the last held weapon
                }
            }
        }
    }

    //
    //CREATES A POOL OF TARGETS
    public void PoolObjects(GameObject targetPrefab, List<GameObject> pooledTargets, int poolAmount, Transform parentPos, Transform targetParent)
    {
        GameObject gameTarget;
        poolAmount = (int)timeCounter;

        //Pool the amount of targets needed and hold them in a list.
        while(pooledTargets.Count < poolAmount)
        {
            gameTarget = Instantiate(targetPrefab, parentPos, instantiateInWorldSpace: false) as GameObject;
            gameTarget.SetActive(false);
            gameTarget.transform.parent = targetParent; //Set the targets inside this gameObject folder
            pooledTargets.Add(gameTarget);

            //Turn of the collider for the parent object.
            parentPos.GetComponent<BoxCollider>().enabled = false;
        }
    }

    //
    //IF TARGET HAS REACHED THE END THEN GO BACK TO THE BEGINNING
    public void SendOneHome(GameObject trgt, Transform parentPos)
    {
        //Reset bools
        trgt.GetComponentInChildren<TargetSetActive>().isFlipped = false;
        trgt.GetComponentInChildren<TargetSetActive>().reachedEnd = false;
        trgt.GetComponentInChildren<TargetSetActive>().hasGone = false;
        //Turn off target
        trgt.SetActive(false);
        //Move target to parent position
        trgt.transform.position = parentPos.position;
    }

    //
    //CONTROLS THE TARGET MOVEMENT
    public IEnumerator MoveTargets(List<GameObject> pooledTargets, Transform parentPos, int direction, float moveSpeed, float timeBetweenTargets)
    {
        int i = 0;

        while (i < pooledTargets.Count && gameOn && !isPaused)
        {
            pooledTargets[i].SetActive(true);
            pooledTargets[i].transform.Translate(direction * Vector3.right * (moveSpeed * Time.deltaTime), Space.Self);
            yield return new WaitForSeconds(timeBetweenTargets);


            if (pooledTargets[i].GetComponentInChildren<TargetSetActive>().reachedEnd)
            {
                //SendOneHome(pooledTargets[i], parentPos);
                pooledTargets[i].SetActive(false);
            }
            i++;
        }
    }
}
