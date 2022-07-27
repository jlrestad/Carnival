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
    }

    //public void PlayGame()
    //{
    //    WeaponEquip.Instance.gameRulesDisplayed = false;
    //    minigameHUD.SetActive(true);
    //}

    //public void ExitGame()
    //{
    //    WeaponEquip.Instance.gameRulesDisplayed = false;
    //}

    //public void ResetGame()
    //{
    //    //Will reset things back to default
    //    WeaponEquip.Instance.actionPrompt.SetActive(false);
    //}

    //public void HideCursor()
    //{
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //}

    //public void ShowCursor()
    //{
    //    Cursor.lockState = CursorLockMode.Confined;
    //    Cursor.visible = true;
    //}
}
