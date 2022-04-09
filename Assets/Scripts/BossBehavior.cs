using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossBehavior : MonoBehaviour
{
    GameObject player; //ref to player character
    NavMeshAgent agent; //ref to navmeshagent component attached to boss. Allows boss to move along the baked path. (Window > AI > Navigation)
    Vector3 distanceFromPlayer; //used to check how far the player is from the boss
    [SerializeField] float maxDistance; //distance to begin chasing player (editable in the Inspector)
    [SerializeField] float turnSpeed; //turn speed in degrees per second

    private void Awake()
    {
        //Initialize variables
        player = GameObject.FindGameObjectWithTag("Player"); //finds the player by searching for the tag (assigned in Inspector)
        agent = GetComponent<NavMeshAgent>(); //finds the navmeshagent component that is attached to the game object that this script is attached to (Boss)
    }

    private void Update()
    {
        //Calculate distance to the player
        distanceFromPlayer = transform.position - player.transform.position;


        if (distanceFromPlayer.magnitude <= maxDistance)
        {
            //Chase player
            agent.destination = (player.transform.position);

            //Look at player
            transform.rotation = Quaternion.RotateTowards(transform.rotation, player.transform.rotation, turnSpeed * Time.deltaTime);
        }

        //* Create a field of vision. If player is within field of vision, then chase player.
        //* if player is within x distance -- attack
        //* if player cannot be found, then search for player -- look around, move around, until player is detected
        //* Search speed most likely slower than chase speed.
    }

}
