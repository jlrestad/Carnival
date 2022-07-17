using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class HudManager : MonoBehaviour
{
    /*
     * This script manages health/ticket measurement, game over conditions, flashlight indicators, etc.
     * !!!NOTE!!! Since player health is shown on the main gameplay HUD, the player's Health is managed through this script.
     * -Dialogue and Story progression are handled thru "Progress Manager," not here.
     * -Tarot Card Selection handled through "Weapon Equip," not here.
     * Jes Restad & Grant Hargraves 7/2022
     */
    //==================================================
    //=========================|FIELDS|
    //==================================================
    public static HudManager Instance;
    //-------------------------
    [Header ("STATS")]
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
    public UnityEngine.UI.Slider redSlider;
    public UnityEngine.UI.Slider blueSlider;
    [SerializeField] GameObject lightOn;
    [SerializeField] GameObject lightOff;
    //-----Sound Effects-----
    [SerializeField] AudioSource myAudio;
    [SerializeField] AudioClip gainRedTicket;
    [SerializeField] AudioClip loseRedTicket;
    [SerializeField] AudioClip gainBlueTicket;
    [SerializeField] AudioClip loseBlueTicket;
    [SerializeField] AudioClip gainTarot;
    //-----Visual Effects-----
    [SerializeField] GameObject redGainFX;
    [SerializeField] GameObject redLoseFX;
    [SerializeField] GameObject blueGainFX;
    [SerializeField] GameObject blueLoseFX;
    [SerializeField] GameObject gainTarotFX;
    //All the below are references to scripts attached to the Visual Effects Game Objects. 
    //These scripts allow them to play every particle effect parented under them.
    [HideInInspector] ParticlePlayer RGFX;
    [HideInInspector] ParticlePlayer RLFX;
    [HideInInspector] ParticlePlayer BGFX;
    [HideInInspector] ParticlePlayer BLFX;
    [HideInInspector] ParticlePlayer GTFX;
    //-------------------------
    [Header("INTERNAL/DEBUG")]
    [Tooltip("Player's current red ticket count.")]
    public int redTickets = 5;
    [Tooltip("Player's current blue ticket count.")]
    public int blueTickets = 1;
    [Tooltip("The amount of red tickets currently displayed on the bar.")]
    public int redTixDisplay = 0;
    [Tooltip("The amount of blue tickets currently displayed on the bar.")]
    public int blueTixDisplay = 0;
    [Tooltip("A utility bool that allows the player to not lose tickets when necessary")]
    public bool playerInvincible = false;
    //==================================================
    //=========================|BUILT-IN METHODS|
    //==================================================
    //--------------------------------------------------|Awake|
    private void Awake()
    {
        Instance = this;
    }
    //--------------------------------------------------|Start|
    private void Start()
    {
        //Set the starting amounts when the scene starts, then display the amounts on the sliders
        redTickets = startingRedTix;
        blueTickets = startingBlueTix;
        //Set the slider components
        redSlider = GameObject.FindGameObjectWithTag("RedTickets").GetComponent<UnityEngine.UI.Slider>();
        blueSlider = GameObject.FindGameObjectWithTag("BlueTickets").GetComponent<UnityEngine.UI.Slider>();
        //Set the flashlight indicator images
        lightOn = GameObject.FindGameObjectWithTag("FlashlightON");
        lightOff = GameObject.FindGameObjectWithTag("FlashlightOFF");

        //Once everything is set up, show how many tickets we have (should be none at the start)
        DisplayTicketAmount();
    }
    //==================================================
    //=========================|CUSTOM METHODS|
    //==================================================

    //--------------------------------------------------|DisplayTicketAmount|
    public void DisplayTicketAmount()
    {
        //Display the ticket bars with the current tickets meant to be shown.
        //Note that this is not always equal to how many the player actually HAS, since they are added one by one as the animation plays.
        if(redSlider != null && blueSlider != null)
        {
            redSlider.value = redTixDisplay;
            blueSlider.value = blueTixDisplay;
        }
        else
        {
            Debug.Log("Could not find red and blue ticket sliders. Sliders not updated.");
        }
    }

    //--------------------------------------------------|HealthTicket|
    //Adds or removes red tickets from the ticket bar while playing effects.
    public void HealthTicket(int tixAmt)
    {
        redTickets += tixAmt; //add/subtract tickets immediately, even if FX is still going. 
        //This helps avoid bugs if the player quickly starts a game after collecting tickets.
        if (redTickets > maxRedTix)
        {
            redTickets = maxRedTix;
        }
        else if(redTickets <= 0)
        {
            redTickets = 0;
            //GameOverCheck will only be called if player loses a minigame after spending their last ticket, or during the boss fight.
        }
        if(tixAmt > 0) //!!!NOTE!!! THIS SYSTEM IS CURRENTLY BACKWARDS FOR SOME UNHOLY REASON. Tickets are LOST when entering a POSITIVE number.
        {
            //start the coroutine that subtracts tickets from the bar.
            StartCoroutine(RedTixSubtractFX(redTickets - redTixDisplay)); //calculate how many tickets need to be subtracted from the bar instead of using tixAmt (in case tixAmt goes above max or below 0)
        }
        else if (tixAmt < 0) //!!!NOTE!!! THIS SYSTEM IS CURRENTLY BACKWARDS FOR SOME UNHOLY REASON. Tickets are ADDED when entering a NEGATIVE number.
        {
            //start the coroutine that adds the tickets onto the counter.
            StartCoroutine(RedTixAddFX(redTickets - redTixDisplay)); //calculate how many tickets need to be added to the bar instead of using tixAmt (in case tixAmt goes above max or below 0)
        }
        else if (tixAmt == 0)
        {
            Debug.Log("A value of 0 was added to the red ticket amount. Nothing was changed.");
        }
        
    }

    //--------------------------------------------------|ContinueTicket|
    //Adds or removes blue tickets from the ticket bar while playing effects. 
    //--Mostly the same logic as above, but gives the player maximum red tickets when consumed.
    public void ContinueTicket(int tixAmt)
    {
        blueTickets += tixAmt; //add/subtract tickets immediately, even if FX is still going. 
        //This helps avoid bugs if the player quickly starts a game after collecting tickets.
        if (blueTickets > maxBlueTix)
        {
            blueTickets = maxBlueTix;
        }
        else if (blueTickets <= 0)
        {
            blueTickets = 0;
        }
        if (tixAmt > 0) //!!!NOTE!!! THIS SYSTEM IS CURRENTLY BACKWARDS FOR SOME UNHOLY REASON. Tickets are LOST when entering a POSITIVE number.
        {
            //start the coroutine that subtracts tickets from the bar.
            StartCoroutine(BlueTixSubtractFX(blueTickets - blueTixDisplay)); //calculate how many tickets need to be subtracted from the bar instead of using tixAmt (in case tixAmt goes above max or below 0)
        }
        else if (tixAmt < 0) //!!!NOTE!!! THIS SYSTEM IS CURRENTLY BACKWARDS FOR SOME UNHOLY REASON. Tickets are ADDED when entering a NEGATIVE number.
        {
            //start the coroutine that adds the tickets onto the counter.
            StartCoroutine(BlueTixAddFX(blueTickets - blueTixDisplay)); //calculate how many tickets need to be added to the bar instead of using tixAmt (in case tixAmt goes above max or below 0)
        }
        else if (tixAmt == 0)
        {
            Debug.Log("A value of 0 was added to the blue ticket amount. Nothing was changed.");
        }
    }

    //--------------------------------------------------|GameOverCheck|
    //Checks whether the player has completely run out of tickets or not.
    //Called when the player loses a minigame or gets hit by the boss.
    public void GameOverCheck()
    {
        if(redTickets <= 0 && ! playerInvincible)//If the player is out of red tickets and not invincible...
        {
            if(blueTickets <= 0 && ! playerInvincible)//If the player is out of blue tickets and not invincible...
            {
                playGameOver();
            }
            else //If the player still has blue tickets left...
            {
                ContinueTicket(1); //consume a blue ticket and restore player to full health
            }
        }
    }

    //--------------------------------------------------|playGameOver|
    //The result of a successful GameOverCheck. Starts the Game Over Sequence.
    //Currently does nothing, as the Game Over sequence is not designed yet.
    public void playGameOver()
    {
        Debug.Log("Game Over sequence was triggered, but no sequence exists yet. Game Over failed.");
    }

    //--------------------------------------------------|GainTarot|
    //Plays a visual effect and sound effect when a tarot card is gained, then allows the tarot card to appear on screen.
    //Currently does nothing, as it cannot access the tarot card manager.
    public void GainTarot()
    {
        //GTFX.PlayFX();
        myAudio.PlayOneShot(gainTarot);
    }

    //--------------------------------------------------|FlashlightIndicator|
    //Toggles the flashlight indicator on and off
    public void FlashlightIndicator(bool on)
    {
        if (on)
        {
            lightOff.GetComponent<UnityEngine.UI.Image>().enabled = false;
            lightOn.GetComponent<UnityEngine.UI.Image>().enabled = true;
        }
        else
        {
            lightOn.GetComponent<UnityEngine.UI.Image>().enabled = false;
            lightOff.GetComponent<UnityEngine.UI.Image>().enabled = true;
        }
    }

    //==================================================
    //=========================|COROUTINES|
    //==================================================

    //--------------------------------------------------|RedTixAddFX|
    //Plays effects and changes the visual for the red ticket bar when adding tickets
    private IEnumerator RedTixAddFX(int tixAmt)
    {
        for(int i = 0; i < tixAmt; i++) //add the tickets on one by one until they reach the proper amount
        {
            redGainFX.gameObject.transform.position += new Vector3(0.55f, 0, 0); //move 0.55 units on X to the right (to the next ticket in the sequence)
            //RGFX.PlayFX();
            yield return new WaitForSeconds(0.2f); //wait while the first part of the animation plays
            redTixDisplay++; //add one to the bar
            DisplayTicketAmount(); //update the bar to show the new ticket
            myAudio.PlayOneShot(gainRedTicket);
            yield return new WaitForSeconds(0.3f); //wait for the animation to finish playing
        }
    }

    //--------------------------------------------------|RedTixSubtractFX|
    //Plays effects and changes the visual for the red ticket bar when subtracting tickets
    //The logic is written in a different order than in the above method to keep the effects from playing over an empty ticket slot.
    private IEnumerator RedTixSubtractFX(int tixAmt)
    {
        if(! playerInvincible)
        {
            for (int i = 0; i < tixAmt; i++) //take the tickets off one by one until they reach the proper amount
            {
                //RLFX.PlayFX();
                redTixDisplay--; //remove one from the bar
                DisplayTicketAmount(); //update the bar to show fewer tickets
                myAudio.PlayOneShot(loseRedTicket);
                yield return new WaitForSeconds(0.5f); //wait for the animation to play
                redLoseFX.gameObject.transform.position -= new Vector3(0.55f, 0, 0); //move 0.55 units on X to the left (to the previous ticket in the sequence)
            }
        }
    }

    //--------------------------------------------------|BlueTixAddFX|
    //Plays effects and changes the visual for the blue ticket bar when adding tickets
    private IEnumerator BlueTixAddFX(int tixAmt)
    {
        for (int i = 0; i < tixAmt; i++) //add the tickets on one by one until they reach the proper amount
        {
            blueGainFX.gameObject.transform.position += new Vector3(0.55f, 0, 0); //move 0.55 units on X to the right (to the next ticket in the sequence)
            //BGFX.PlayFX();
            yield return new WaitForSeconds(0.2f); //wait while the first part of the animation plays
            blueTixDisplay++; //add one to the bar
            DisplayTicketAmount(); //update the bar to show the new ticket
            myAudio.PlayOneShot(gainBlueTicket);
            yield return new WaitForSeconds(0.3f); //wait for the animation to finish playing
        }
        
    }

    //--------------------------------------------------|BlueTixSubtractFX|
    //Plays effects and changes the visual for the blue ticket bar when subtracting tickets
    //The logic is written in a different order than in the above method to keep the effects from playing over an empty ticket slot.
    private IEnumerator BlueTixSubtractFX(int tixAmt)
    {
        if(! playerInvincible)
        {
            for (int i = 0; i < tixAmt; i++) //take the tickets off one by one until they reach the proper amount
            {
                //BLFX.PlayFX();
                blueTixDisplay--; //remove one from the bar
                DisplayTicketAmount(); //update the bar to show fewer tickets
                myAudio.PlayOneShot(loseBlueTicket);
                yield return new WaitForSeconds(0.5f); //wait for the animation to play
                blueLoseFX.gameObject.transform.position -= new Vector3(0.55f, 0, 0); //move 0.55 units on X to the left (to the previous ticket in the sequence)
            }
            HealthTicket(-1 * maxRedTix); //refill the player's red tickets to full!
        }
    }
}
