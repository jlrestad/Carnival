using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterEnemy : MonoBehaviour
{
    public static CritterEnemy Instance;

    public bool isVis;
    public bool hasBeenHit;

    //AudioSource audiosource;

    private void Awake()
    {
        Instance = this;
        isVis = false;
    }

    public void HitEnemy()
    {
        if (hasBeenHit)
        {
            gameObject.SetActive(false); //hide the enemy
            hasBeenHit = false; //reset the bool
        }
    }
}
