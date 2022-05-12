using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;

namespace CH
{
    public abstract class State : MonoBehaviour
    {
        public GameObject player; //ref to player character
        public NavMeshAgent agent; //ref to navmeshagent component attached to boss. Allows boss to move along the baked path. (Window > AI > Navigation)

        protected BossAttributes bossAtr;
        protected float timeCounter = 0;

        [SerializeField] public Vector3 distanceFromPlayer;

        private void Awake()
        {
            //Initialize variables
            player = GameObject.FindGameObjectWithTag("Player"); //finds the player by searching for the tag (assigned in Inspector)
            bossAtr = agent.GetComponent<BossAttributes>(); // gives access to boss behavior variables
        }

        public abstract State RunCurrentState();

        protected bool PlayerDetector(float reqDistance)           // method used to detect the player within the distance
        {
            bossAtr.distanceFromPlayer = agent.transform.position - player.transform.position;

            if (bossAtr.distanceFromPlayer.magnitude <= reqDistance)
            {
                return true;

            }
            else
            {
                return false;
            }
        }

        protected void RandomizeTimer(int min, int max)
        {
            System.Random random = new System.Random();
            timeCounter = (float)(random.NextDouble() * (max - min) + min);
        }

        protected IEnumerator CountDownTimer()
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

        protected void MoveTowardPlayer()
        {
            agent.destination = (player.transform.position);
            agent.transform.rotation = Quaternion.RotateTowards(agent.transform.rotation, player.transform.rotation, bossAtr.turnSpeed * Time.deltaTime);
        }
    }

}
