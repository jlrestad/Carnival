using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyScriptTester : MonoBehaviour
{
    public HudManager myHUD;
    public int ticketAmount = 0;
    public bool RedTicket = true;
    private void OnEnable()
    {
        if(RedTicket)
        {
            myHUD.HealthTicket(ticketAmount);
        }
        else 
        {
            myHUD.ContinueTicket(ticketAmount);
        }
        
    }
}
