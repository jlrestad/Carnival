using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace CH
{
    public class TauntState : State
    {
        public AttackState aState;
        public SearchState sState;
        public DeathState dState;
        public HurtState hState;
        public ChaseState cState;

        private bool tauntRan;

        public CritterSpawnerManager spawnManager;
        private void Start()
        {
            spawnManager = FindObjectOfType<CritterSpawnerManager>();
        }
        public override State RunCurrentState()
        {

            if (timeCounter == 0 && !tauntRan)
            {
                agent.isStopped = true;
                Taunt();
                tauntRan = true;
                RandomizeTimer(3 - bossAtr.whichHit, 5 - bossAtr.whichHit); // randomizes swing / taunt time depending on how many times the heart has been hit
            }

            StartCoroutine(CountDownTimer());

            if(timeCounter == 0 && tauntRan)
            {
                tauntRan = false;       // reset tauntRan for next cycle
                agent.isStopped = false;
                return sState;
            }

            if (bossAtr.targetHit && bossAtr.heartHit && bossAtr.whichHit < 2)  // sends to the hurt state under correct conditions
            {
                return hState;
            }

            if (bossAtr.whichHit > 2)
            {
                return dState;
            }

            // stop following the target and taunt
            // critters will spawn
            
            // resumes attack state after taunting
            return this;
        }

        private void CritterSpawn()
        {
            // method to make critters spawn
        }

        private void Taunt()
        {
            // method to call taunt animation
            agent.destination = (agent.transform.position);   // stops boss from continuing the previous movement if player leaves fov during taunt
            Debug.Log("taunting");
            StartCoroutine(spawnManager.SpawnCritters());
        }
    }
}
