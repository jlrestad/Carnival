using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CH
{
    public class DeathState : State
    {
        private bool deathRan;
        public override State RunCurrentState()
        {
            agent.isStopped = true;

            if(!deathRan)
            {
                Death();
                deathRan = true;
            }

            return this;
        }

        public void Death()
        {
            // run death animation
            Debug.Log("dead");
        }
    }
}