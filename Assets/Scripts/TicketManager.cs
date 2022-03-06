using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TicketManager : MonoBehaviour
{
    public static TicketManager Instance;

    public int tickets = 5;

    private void Awake()
    {
        Instance = this;
    }
}
