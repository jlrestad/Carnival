using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    public static TicketManager Instance;

    public int tickets = 5;
    public TextMeshProUGUI ticketsText;

    private void Awake()
    {
        Instance = this;

        ticketsText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void DisplayTicketAmount()
    {
        ticketsText.text = ("Tickets: " + tickets);
    }

    public void SpendTicket(int ticketAmount)
    {
        tickets -= ticketAmount;

        //Update the ticket amount
        DisplayTicketAmount();
    }

}
