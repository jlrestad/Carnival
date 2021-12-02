using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCard : MonoBehaviour
{
    GameObject player;

    private void OnValidate()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Awake()
    {
        player.GetComponent<FPSController>().cardCount++;
    }
}
