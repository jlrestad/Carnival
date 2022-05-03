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
    [SerializeField] private float timeCounter = 0; //used separate time between taunt / leftswing / rightswing

    bool taunt = true;
    bool leftSwing, rightSwing, canMove;

    private void Awake()
    {
        //Initialize variables
        player = GameObject.FindGameObjectWithTag("Player"); //finds the player by searching for the tag (assigned in Inspector)
        agent = GetComponent<NavMeshAgent>(); //finds the navmeshagent component that is attached to the game object that this script is attached to (Boss)
    }

    private void Update()
    {
        if(timeCounter == 0)     // will make boss start off with a taunt and then follow the attack pattern for rest of fight
        {
            StartCoroutine(AttackPattern());

            RandomizeTimer();
        }

        StartCoroutine(CountDownTimer());
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

    // method that runs the taunt
    private void Taunt()        // needs to stop movement
    {
        // need taunt animation to happen when called

    }

    private void LeftSwing()
    {
        // need left swing animation to happen when called

    }
    private void RightSwing()
    {
        // need left swing animation to happen when called
    }
    IEnumerator AttackPattern()
    {
        if (taunt)
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;

            // run the taunt method
            Taunt();

            yield return new WaitForSeconds(3); // give time to stand still
            // set bools for the next iteration or to pick up where it left off

            gameObject.GetComponent<NavMeshAgent>().isStopped = false;
            taunt = false;
            leftSwing = true;

        } else if (leftSwing)
        {
            // have left arm attack
            LeftSwing();
            // set bools for the next iteration or to pick up where it left off
            leftSwing = false;
            rightSwing = true;

        } else if (rightSwing)
        {
            // have right arm attack
            RightSwing();
            // set bools for the next iteration or to pick up where it left off
            rightSwing = false;
            taunt = true;

        }
    }

    private void RandomizeTimer()
    {
        System.Random random = new System.Random();
        timeCounter = (float)(random.NextDouble() * (4 - 1) + 1);
    }

    IEnumerator CountDownTimer()
    {
        //Wait for 1 second so that the starting number is displayed.
        yield return new WaitForSeconds(0.5f);

        timeCounter -= Time.deltaTime;
        if (timeCounter <= 0f)
        {
            timeCounter = 0f;
        }
        else
        {
            yield return null;
        }
    }
}
