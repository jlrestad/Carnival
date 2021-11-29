using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Target : MonoBehaviour
{
    public static Target Instance;

    Menu menu;
    Target target;
    MovingTarget movingTarget;

    [HideInInspector] public WhackEmEnemy spawnHead;
    [SerializeField] float health = 100f;
    
    [SerializeField] bool targetHit;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        movingTarget = GetComponentInParent<MovingTarget>();
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health < 0f)
        {
            health = 0f;
            Die();
        }
    }

    public void HitTarget()
    {
        //Flip target back after being hit
        transform.rotation = Quaternion.Euler(2f, 0, 0);

        targetHit = true;
        //Keep from scoring multiple points
        if (targetHit)
        {
            //Add game object to Moving Target array
            movingTarget.targetsList.Add(this.gameObject);
        }

        transform.position = transform.position;
    }

    void Die()
    {
        if (CompareTag("Critter"))
        {
            //Call method to throwable object
            spawnHead.SpawnHead();

            this.gameObject.SetActive(false);
        }
    }
}
