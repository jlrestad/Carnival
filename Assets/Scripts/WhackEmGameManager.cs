using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackEmGameManager : MonoBehaviour
{
    public GameObject[] spawnLocations;
    public float randomAppearTime;
    //public float randomStayTime;
    public GameObject whackemEnemy;
    public Transform enemyTransform;

    //array of spawn spots
    //randomize the spawn spot choses
    //timer for how long critter is at spawn spot
    //random time

    private void Start()
    {
        //Find the spawn locations for the enemy.
        spawnLocations = GameObject.FindGameObjectsWithTag("CritterSpawn");
        enemyTransform = whackemEnemy.transform;
    }

    private void Update()
    {
        StartCoroutine(RandomSpawn());
    }


    //create an ienumerator that waits for random seconds to instantiate an enemy at a spawn point
    IEnumerator RandomSpawn()
    {
        randomAppearTime = UnityEngine.Random.Range(5f, 10f) * Time.deltaTime;
        int randomSpawnLocation = UnityEngine.Random.Range(0, spawnLocations.Length);
        Debug.Log(randomSpawnLocation);
        yield return new WaitForSeconds(randomAppearTime);

        //Make enemy visible at random spawn point
        enemyTransform.position = spawnLocations[randomSpawnLocation].transform.position;
        whackemEnemy.SetActive(true);

        yield return new WaitForSeconds(randomSpawnLocation);
    }
}
