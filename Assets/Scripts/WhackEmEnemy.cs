using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackEmEnemy : MonoBehaviour
{
    public static WhackEmEnemy Instance;

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
    }

    void HealthManager()
    {
        if (health <= 0)
        {
            health = 0;
        }
        else if (health > 0 && health < maxHealth)
        {
            hasBeenHit = true;
        }
        else
        {
            hasBeenHit = false;
        }
    }

}
