using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBehavior : MonoBehaviour
{
    GameObject player;
    NavMeshAgent agent;
    Vector3 distanceFromPlayer;
    //Vector3 destination;
    [SerializeField] float maxDistance = 30f; //distance to begin chasing player

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        //Calculate distance to the player
        distanceFromPlayer = transform.position - player.transform.position;


        if (distanceFromPlayer.magnitude <= maxDistance)
        {
            //Chase player
            agent.destination = (player.transform.position);
        }
    }

}
