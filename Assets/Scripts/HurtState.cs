using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CH
{
    public class HurtState : State
    {
        private bool hurtRan;

        public IdleState iState;

        public override State RunCurrentState()
        {
            agent.isStopped = true;

            if (!hurtRan)
            {
                Hurt();
                hurtRan = true;
                timeCounter = 5;
            }

            StartCoroutine(CountDownTimer());

            if (hurtRan && timeCounter == 0)
            {
                hurtRan = false;
                agent.isStopped = false;
                return iState;
            }

            return this;
        }

        public void Hurt()
        {
            // run hurt animation
            Debug.Log("hurt");
        }
    }
}
