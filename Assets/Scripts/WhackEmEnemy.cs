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


    // For boss fight. Turned prefabs into triggers to allow
    // For some reason OnCollisionEnter does not work.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("COLLISON using trigger");
            // need to hook up red cards for player health during boss fight
        }
    }

}
