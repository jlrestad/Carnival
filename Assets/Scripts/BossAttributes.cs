using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttributes : MonoBehaviour
{

    [Space(15)]
    [SerializeField] public float maxDistance; //distance to begin chasing player (editable in the Inspector)
    [SerializeField] public float turnSpeed; //turn speed in degrees per second

    public Vector3 distanceFromPlayer; //used to check how far the player is from the boss
    public int whichHit = 0;               // needs to increment everytime the heart is hit

    public bool targetHit, heartHit;
}
