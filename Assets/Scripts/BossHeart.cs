using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHeart : MonoBehaviour
{
    public BossAttributes bossAttributes;
    public int bossHealth;
    public int hitAmount;
    public bool heartHit;


    private void Awake()
    {
        bossAttributes = GetComponentInParent<BossAttributes>();
    }

    private void Start()
    {
        bossHealth = bossAttributes.bossHealth;
        hitAmount = bossAttributes.whichHit;
    }

    private void Update()
    {
        //Update varables
        bossAttributes.bossHealth = bossHealth;
        bossAttributes.whichHit = hitAmount;
    }

    //Takes away the damage amount to the boss' health and adds to the hit amount.
    public void DoDamage(int damageAmount)
    {
        heartHit = true; 

        bossHealth -= damageAmount;  //Take away the damage amount from health
        hitAmount++;  //Increment the hit 
    }

}
