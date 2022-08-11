using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterEnemy : MonoBehaviour
{
    public bool isVis;
    public bool hasBeenHit;

    AudioSource hitFX;

    public void HitEnemy()
    {
        if (hasBeenHit)
        {
            gameObject.SetActive(false); //hide the enemy
            hasBeenHit = false; //reset the bool
        }

        //isVis = false;
    }
}
