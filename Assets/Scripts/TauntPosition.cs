using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntPosition : MonoBehaviour
{
    public static TauntPosition Instance;
    public Transform tauntPosition;
    public Transform enemyPosition;
    public GameObject thisEnemy;

    void Awake()
    {
        Instance = this;

        //Taunt position of the enemy 
        tauntPosition = this.transform;
    }

    //* TEMP COMMENTED OUT
    //void Start()
    //{
    //    //Enemy parent game object to get position from
    //    thisEnemy = GetComponentInParent<WhackEmEnemy>().gameObject;

    //    //Original position of the enemy
    //    enemyPosition = thisEnemy.transform;

    //}


}
