using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    /*
     * 
     * Grant Hargraves 7/2022
     */
    //==================================================
    //=========================|FIELDS|
    //==================================================
    [Header("STATS")]
    [Tooltip("The maximum amount of red tickets (tix) the player can have.")]
    public int maxRedTix = 10;
    [Tooltip("The amount of red tickets (tix) the player starts the game with.")]
    public int startingRedTix = 0;
    [Tooltip("The maximum amount of blue tickets (tix) the player can have.")]
    public int maxBlueTix = 3;
    [Tooltip("The amount of blue tickets (tix) the player starts the game with.")]
    public int startingBlueTix = 0;
    //-------------------------
    [Header("PLUG-INS")]
    [Tooltip("Reference to the HUDManager, so that the HUD will be properly updated and minigames don't have to do anything new.")]
    [SerializeField] HudManager HudScript;
    [SerializeField] Slider redTixSlider;
    [SerializeField] Slider blueTixSlider;
    //-------------------------
    [Header("INTERNAL/DEBUG")]
    [Tooltip("How many red tickets (tix) the player currently has.")]
    public int currentRedTix = 0;
    [Tooltip("How many blue tickets (tix) the player currently has.")]
    public int currentBlueTix = 0;
    [Tooltip("Allows for time to pass between ticket animations")]
    [SerializeField] bool waiting = false;
    //==================================================
    //=========================|BUILT-IN METHODS|
    //==================================================
    void Start()
    {
        currentRedTix = startingRedTix;
        currentBlueTix = startingBlueTix;
        updateSliders();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //==================================================
    //=========================|CUSTOM METHODS|
    //==================================================
    public void AddRedTix(int tixAmt)
    {
        //add red tickets to the counter
    }

    public void LoseRedTix(int tixAmt)
    {
        //remove red tickets to the counter
    }

    public void AddBlueTix(int tixAmt)
    {

    }

    public void LoseBlueTix(int tixAmt)
    {

    }

    public void updateSliders()
    {
        redTixSlider.value = currentRedTix;
        blueTixSlider.value = currentBlueTix;
    }
}
