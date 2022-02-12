using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class Target : MonoBehaviour
{
    public static Target Instance;

    BezierFollow moveAlongCurve;
    GameCardManager cardManager;

    [HideInInspector] public WhackEmEnemy spawnHead;

    public float health = 100f;

    public Transform startPos;
    
    public bool targetHit;

    private void Awake()
    {
        Instance = this;

        startPos = transform; //save starting position
    }

    private void Start()
    {
        cardManager = GetComponentInParent<GameCardManager>();
        moveAlongCurve = GetComponent<BezierFollow>();
    }

    //public void TakeDamage(int damageAmount)
    //{
    //    health -= damageAmount;

    //    if (health <= 0f)
    //    {
    //        health = 0f;

    //        if (CompareTag("Critter"))
    //        {
    //            SmashCritter();
    //        }

    //        //Add game object to Moving Target array.
    //        cardManager.targetsList.Add(this.gameObject);
    //    }
    //}

    public void HitTarget()
    {
        //Flip target back after being hit
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        targetHit = true;
        //Keep from scoring multiple points
        if (targetHit)
        {
            //Add game object to Moving Target array.
            cardManager.critterList.Add(this.gameObject);

            //Stop the target from moving after it's shot.
            moveAlongCurve.speedModifier = 0f;
        }
    }

    //public void SmashCritter()
    //{
    //    //Spawn the head
    //    spawnHead.SpawnHead();

    //    //Hide the critter
    //    this.gameObject.SetActive(false);
    //}
}
