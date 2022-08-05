using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//** THIS CLASS INHERITS FROM THE GameBooth.cs SCRIPT **//

public class CarnivalSmashGameManager : GameBooth
{
    public static CarnivalSmashGameManager Instance;

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

        //Set timer info
        timeLeft = GetTimeCounter();
    }

    private void Update()
    {
        //When the game is on, player is holding the skull and can bring up the game rules menu.
        if (gameOn)
        {
            playerWeapon.transform.GetChild(0).gameObject.SetActive(true); //Show player holding weapon

            StartCoroutine(CountDownTimer());

            //PAUSE
            if (Input.GetButtonDown("Menu"))
            {
                ShowGameRules();
            }
        }

        //Hide the skull if the game is lost. Add the skull weapon to the weapon list if the game is won.
        if (gameWon && !gameOn)
        {
            WE.weaponList.Add(playerWeapon);
        }
        else if (!gameWon && !gameOn)
        {
            playerWeapon.transform.GetChild(0).gameObject.SetActive(false);
        }

        //Timer formatting
        if (timeLeft >= 10)
        {
            timerText.text = ("00:" + (int)timeLeft);
        }
        else
        {
            timerText.text = ("00:0" + (int)timeLeft);
        }
    }
}
