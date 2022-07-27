using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TauntPosition : MonoBehaviour
{
    [TextArea]
    [SerializeField] string notes;

    public static TauntPosition Instance;
    public Transform tauntPosition;
    public Transform enemyPosition;
    private GameObject thisEnemy;

    public void OnValidate()
    {
        notes = "Drag this parent Critter to the Enemy Position. Taunt Position is automatically filled.";
    }

    void Awake()
    {
        Instance = this;

        //Taunt position of the enemy 
        tauntPosition = this.transform;
    }

    //* TEMP COMMENTED OUT
    void Start()
    {
        //Enemy parent game object to get position from
        thisEnemy = GetComponentInParent<WhackEmEnemy>().gameObject;


        //Original position of the enemy
        enemyPosition = thisEnemy.transform;

    }


}
