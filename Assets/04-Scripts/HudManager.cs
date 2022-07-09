using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;


public class HudManager : MonoBehaviour
{
    public static HudManager Instance;

    [Header ("TICKET MANAGER")]
    public int redTickets = 5;
    public int blueTickets = 1;
    public UnityEngine.UI.Slider redSlider;
    public UnityEngine.UI.Slider blueSlider;

    [Header("UI ELEMENTS")]
    [SerializeField] GameObject lightOn;
    [SerializeField] GameObject lightOff;



    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Set the slider components
        redSlider = GameObject.FindGameObjectWithTag("RedTickets").GetComponent<UnityEngine.UI.Slider>();
        blueSlider = GameObject.FindGameObjectWithTag("BlueTickets").GetComponent<UnityEngine.UI.Slider>();

        //Set the flashlight indicator images
        lightOn = GameObject.FindGameObjectWithTag("FlashlightON");
        lightOff = GameObject.FindGameObjectWithTag("FlashlightOFF");

        DisplayTicketAmount();
    }

    //Displays the ticket bars with the current tickets
    public void DisplayTicketAmount()
    {
        redSlider.value = redTickets;
        blueSlider.value = blueTickets;
    }

    //Updates the red health tickets
    public void HealthTicket(int ticketAmount)
    {
        redTickets -= ticketAmount;

        //Update the ticket amount
        DisplayTicketAmount();
    }

    //Updates the blue continue tickets
    public void ContinueTicket(int ticketAmount)
    {
        blueTickets -= ticketAmount;

        //Update the ticket amount
        DisplayTicketAmount();
    }

    //Changes the flashlight indicator
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

}
