using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeart : MonoBehaviour
{
    public BossAttributes bossAttributes;
    public int hitAmount;
    public bool heartHit;
    public bool canDamage;  //Used to keep skull from doing more than 1 hit if it enters the heart trigger more than once.


    private void Awake()
    {
        bossAttributes = GetComponentInParent<BossAttributes>();
    }

    private void Start()
    {
        canDamage = true;
        hitAmount = bossAttributes.whichHit;
    }

    private void Update()
    {
        //Update varables
        bossAttributes.whichHit = hitAmount;
    }

    //Hits heart and adds to the hit amount.
    public void HitHeart()
    {
        heartHit = true;

        if (canDamage)
        {
            hitAmount++;  //Increment the hit
        }
        canDamage = false; //Keeps the skull from doing more than 1 hit. Set to true in Head script
    }

}
