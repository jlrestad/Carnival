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

    int whichHit = 0;               // needs to increment everytime the heart is hit
    int patternNumber = 0;
    bool bossAlive = true;

    private void Awake()
    {
        //Initialize variables
        player = GameObject.FindGameObjectWithTag("Player"); //finds the player by searching for the tag (assigned in Inspector)
        agent = GetComponent<NavMeshAgent>(); //finds the navmeshagent component that is attached to the game object that this script is attached to (Boss)
    }

    private void Update()
    {
        if(bossAlive)
        {
            if (timeCounter == 0)     // will make boss start off with a taunt and then follow the attack pattern for rest of fight
            {
                StartCoroutine(AttackPattern());

                RandomizeTimer(1, 4 - whichHit); // randomizes swing / taunt time depending on how many times the heart has been hit
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

            if(whichHit == 3)
            {
                bossAlive = false;
            }
        }

        if(!bossAlive)
        {
            Death();
        }
        //* Create a field of vision. If player is within field of vision, then chase player.
        //* if player is within x distance -- attack
        //* if player cannot be found, then search for player -- look around, move around, until player is detected
        //* Search speed most likely slower than chase speed.
    }

    // method that runs the taunt
    private void Taunt()
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

    private void Death()
    {
        // need death animation
    }
    IEnumerator AttackPattern()
    {
        if (patternNumber == 0)
        {
            gameObject.GetComponent<NavMeshAgent>().isStopped = true;           // stops movement

            // run the taunt method
            Taunt();

            yield return new WaitForSeconds(3 - whichHit); // give time to stand still, but less time depending on how many times the heart has been hit
      

            gameObject.GetComponent<NavMeshAgent>().isStopped = false;          // resumes movement
            patternNumber++;            // sets the pattern number depending on how many times the heart has been hit

        } else if (patternNumber == 1 || patternNumber == 3)
        {
            // have left arm attack
            LeftSwing();
            // sets the pattern number depending on how many times the heart has been hit

            if(whichHit == 0 || whichHit == 2)
            {
                patternNumber++;
            } else
            {
                patternNumber = 0;
            }

        } else if (patternNumber == 2 || patternNumber == 4)
        {
            // have right arm attack
            RightSwing();


            // sets the pattern number depending on how many times the heart has been hit
            if (whichHit == 1)
            {
                patternNumber++;
            } else
            {
                patternNumber = 0;
            }
        }
    }

    private void RandomizeTimer(int min, int max)
    {
        System.Random random = new System.Random();
        timeCounter = (float)(random.NextDouble() * (max - min) + min);
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
