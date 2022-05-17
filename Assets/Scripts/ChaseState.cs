using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CH
{
    public class ChaseState : State
    {
        public SearchState sState;
        public TauntState tState;
        public DeathState dState;
        public HurtState hState;
        public AttackState aState;

        public override State RunCurrentState()
        {
            MoveTowardPlayer();

            // if boss heart has been hit 3 times, enter death state
            if (bossAtr.whichHit > 2)
            {
                return dState;
            }

            if (bossAtr.targetHit && bossAtr.heartHit && bossAtr.whichHit < 2)  // sends to the hurt state under correct conditions
            {
                return hState;
            }

            if (PlayerDetector(bossAtr.maxAtkDistance) && fov.canSeePlayer)                 // depending on distance change, start attacking, start searching, or keep chasing
            {
                agent.speed = bossAtr.atkSpeed;
                return aState;
            } else if (!PlayerDetector(bossAtr.maxChaseDistance) || !fov.canSeePlayer)
            {
                agent.destination = (agent.transform.position);
                return sState;
            } else
            {
                return this;
            }
        }
    }
}
