using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CH
{
    public class IdleState : State
    {
        public AttackState aState;
        public TauntState tState;
        public DeathState dState;
        public HurtState hState;

        public override State RunCurrentState()
        {
            if(PlayerDetector())
            {
                return aState;
            } else
            {
                return this;
            }

        }
    }
}
