using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossAttributes : MonoBehaviour
{
                                 /* commented out lines having to do with the heart should plug right in when the script is written */
    public GameObject target;
    public GameObject heart;

    protected BossTarget bossTrgt;
    //protected BossHeart bossHrt;


    [Space(15)]
    [SerializeField] public float maxChaseDistance; //distance to begin chasing player (editable in the Inspector)
    [SerializeField] public float maxAtkDistance; //distance to begin attacking player (editable in the Inspector)
    [SerializeField] public float turnSpeed; //turn speed in degrees per second
    [SerializeField] public float chaseSpeed; // speed of the boss while chasing
    [SerializeField] public float atkSpeed; // speed of the bosswhile attacking

    public Vector3 distanceFromPlayer;      // used to check how far the player is from the boss
    public int whichHit = 0;                // needs to increment everytime the heart is hit

    public bool targetHit, heartHit;

    private void Awake()
    {
        bossTrgt = target.GetComponent<BossTarget>();
        //bossHrt = heart.GetComponent<BossHeart>();
    }

    void Update()
    {
        targetHit = bossTrgt.targetHit;
        //heartHit = bossHrt.heartHit;

        if(heartHit)
        {
            //bossHrt.heartHit = false;
            heartHit = false;

            whichHit++;
        }
    }
}
