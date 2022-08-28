using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameRestart : MonoBehaviour
{
    Menu menu;

    private void Awake()
    {
        menu = FindObjectOfType<Menu>();
    }

    //Method that will set game values back to beginning game state
    public void ResetGamePrefs()
    {
        //Turn off Tarot card images
        menu.gameCard.GetComponent<Image>().enabled = false;
        menu.gameCardBG.GetComponent<Image>().enabled = false;
        
        //Remove the Tarot card sprites.
        if (menu.gameCard.GetComponent<Image>().sprite != null)
            menu.inactiveWeapon = null;
        if (menu.gameCardBG.GetComponent<Image>().sprite != null)
            menu.activeWeapon = null;
    }
}
