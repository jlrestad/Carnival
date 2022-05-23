using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class TicketManager : MonoBehaviour
{
    public static TicketManager Instance;

    public int redTickets = 5;
    public int blueTickets = 1;
    
    public UnityEngine.UI.Slider redSlider;
    public UnityEngine.UI.Slider blueSlider;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Set the slider components
        redSlider = GameObject.Find("RedTickets").GetComponent<UnityEngine.UI.Slider>();
        blueSlider = GameObject.Find("BlueTickets").GetComponent<UnityEngine.UI.Slider>();
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

}
