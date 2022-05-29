using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackEmEnemy : MonoBehaviour
{
    public static WhackEmEnemy Instance;

    WhackEmGameManager whackemGM;
    public int health;
    public int maxHealth = 100;
    public bool hasBeenHit;
    public bool isVis;

    private void Awake()
    {
        Instance = this;
        isVis = false;
    }

    void Start()
    {
        health = maxHealth;
        whackemGM = GetComponent<WhackEmGameManager>();
    }

    public void HealthManager()
    {
        if (hasBeenHit)
        {
            gameObject.SetActive(false);
            hasBeenHit = false; //reset bool
        }
    }

}
