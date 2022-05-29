using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCard : MonoBehaviour
{
    GameObject player;
    public GameObject cardWon;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Start()
    {
        //Boss appears after x cards. This increments the count.
        player.GetComponent<FPSController>().cardCount++;
    }
}
