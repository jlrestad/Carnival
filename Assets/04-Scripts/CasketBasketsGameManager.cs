using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;


//** THIS CLASS INHERITS FROM THE GameBooth.cs SCRIPT **//

public class CasketBasketsGameManager : GameBooth
{
    public static CasketBasketsGameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Get the sprite from the tarot cards
        inactiveCardSprite = GetInactiveCardSprite();
        activeCardSprite = GetActiveCardSprite();

        //Set timer info
        timeLeft = GetTimeCounter();

        GetGameManagerScript(GetComponent<CasketBasketsGameManager>());
    }

    private void Update()
    {
        //ShowGameRulesPause();

        //* Need to release player from being locked when the game is over or exited.

        if (gameOn)
        {
            //playerWeapon.transform.GetChild(0).gameObject.SetActive(true); //Show player holding weapon

            StartCoroutine(CountDownTimer());

            if (Input.GetButtonDown("Menu"))
            {
                ShowGameRules();
                Time.timeScale = 0; //Pause the timer (this stops it from visibly counting down, but when timeScale = 1 the time drops to where it would be at if never set to 0 -- So need to pause time counter coroutine..)
            }
        }

        if (timeLeft >= 10)
        {
            timerText.text = ("00:" + (int)timeLeft);
        }
        else
        {
            timerText.text = ("00:0" + (int)timeLeft);
        }
    }

    void ShowGameRulesPause()
    {
        if (gameOn && Input.GetButtonDown("Menu"))
        {
            gameRules.SetActive(true);
            WeaponEquip.Instance.gameRulesDisplayed = true;
            Time.timeScale = 0;
        }
    }

}
