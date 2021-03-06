using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CH
{
    public class AttackState : State
    {
        public SearchState sState;
        public TauntState tState;
        public DeathState dState;
        public HurtState hState;
        public ChaseState cState;

        public int atkCounter = 0;

        public override State RunCurrentState()
        {
            // if player is out of range, move into idle state
            if (!PlayerDetector(bossAtr.maxAtkDistance) && PlayerDetector(bossAtr.maxChaseDistance) && fov.canSeePlayer)
            {
                agent.speed = bossAtr.chaseSpeed;
                return cState;
            }

            if (!fov.canSeePlayer)
            {
                agent.destination = (agent.transform.position);
                return sState;
            }

            if (bossAtr.targetHit && bossAtr.heartHit && bossAtr.whichHit <= 2)  // sends to the hurt state under correct conditions
            {
                return hState;
            }

            // if boss heart has been hit 3 times, enter death state
            if (bossAtr.whichHit > 2) 
            {
                return dState;
            }

            // follow player around and look in direction
            MoveTowardPlayer();

            // left swing, right swing
            // numbers of swings = 2 + number of heart hits
            if(atkCounter < 2 + bossAtr.whichHit && timeCounter <= 0)
            {
                AttackPattern();

                RandomizeTimer(3 - bossAtr.whichHit, 5 - bossAtr.whichHit); // randomizes swing / taunt time depending on how many times the heart has been hit
            }

            StartCoroutine(CountDownTimer());

            // goes into taunt after complete number of swings
            if (atkCounter >= 2 + bossAtr.whichHit && timeCounter <= 0)
            {
                atkCounter = 0;         // reset attack counter for next cycle
                return tState;
            }

            // remain in this state
            return this;
        }

        private void LeftSwing()
        {
            //Debug.Log("left swing");
            // need left swing animation to happen when called
            Attack();
        }
        private void RightSwing()
        {
            //Debug.Log("right swing");
            // need left swing animation to happen when called
            Attack();
        }

        private void Attack()
        {
            // if boss is within a certain distance when this is called, player loses 1 ticket
        }

        private void AttackPattern()
        {
            if (atkCounter == 0 || atkCounter == 2)
            {
                // have left arm attack
                LeftSwing();
            }
            else if (atkCounter == 1 || atkCounter == 3)
            {
                // have right arm attack
                RightSwing();
            }

            atkCounter++;
        }
    }
}
