using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CH
{
    public class SearchState : State
    {
        public AttackState aState;
        public TauntState tState;
        public DeathState dState;
        public HurtState hState;
        public ChaseState cState;

        public override State RunCurrentState()
        {
            if (PlayerDetector(bossAtr.maxAtkDistance) && fov.canSeePlayer)                 // depending on distance change, start attacking, start searching, or keep chasing
            {
                agent.speed = bossAtr.atkSpeed;
                return aState;
            }
            else if (PlayerDetector(bossAtr.maxChaseDistance) && fov.canSeePlayer)
            {
                agent.speed = bossAtr.chaseSpeed;
                return cState;
            }
            else
            {
                return this;
            }
        }
    }
}
