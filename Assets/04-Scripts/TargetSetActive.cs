using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetSetActive : MonoBehaviour
{
    public static TargetSetActive Instance;

    public MovingTarget movingTarget;
    public SkillShotGameManager skillshotGM;

    //[HideInInspector] public MeshRenderer meshRenderer;

    Animator animator;

    public Transform hideSpot;
    public GameObject targetParent;

    public float flipTime;
    [SerializeField] float shakeTime;
    public bool reachedEnd;
    public bool targetHit;
    public bool isFlipped;
    public bool hasGone;
    [SerializeField] bool hasBeenHit;

    public GameObject bigHitFX;
    public AudioSource targetAudio;
    public AudioClip goodHitSound, badHitSound, flipSound, shakeSound;
    public bool flippable;

    private void Start()
    {
        //flipTime = Random.Range(0.7f, 1.5f); //Random time to flip the target.
        flippable = true;
        skillshotGM = GetComponentInParent<SkillShotGameManager>();
        movingTarget = GetComponentInParent<MovingTarget>();
        animator = GetComponentInParent<Animator>();
    }

    private void Update()
    {
        if (flippable)
        {
            flipTime = UnityEngine.Random.Range(0.75f, 1.25f); //Random time to flip the target.

            StartCoroutine(FlipAround());
            if (skillshotGM.isPaused)
            {
                StopAllCoroutines();
            }
        }
    }

    //Controls when to restart the loop
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TargetHome")
        {
            skillshotGM.SendOneHome(movingTarget.targetPrefab, movingTarget.parentPos);
        }
    }

    //Flip target from front to back within the flip time set
    public IEnumerator FlipAround()
    {
        while (skillshotGM.gameOn && !skillshotGM.isPaused)
        {
            //If Positive
            if (isFlipped && !skillshotGM.gameOver)
            {
                yield return new WaitForSeconds(flipTime);

                //Flip to negative
                animator.SetBool("isNeg", true);

                isFlipped = false;
                targetHit = false;

                yield return new WaitForSeconds(flipTime);
            }

            //If Negative
            if (!isFlipped && !skillshotGM.gameOver && !skillshotGM.isPaused)
            {
                yield return new WaitForSeconds(flipTime);

                //Flip to positive
                animator.SetBool("isNeg", false);
                isFlipped = true;

                yield return new WaitForSeconds(flipTime);
            }

            if (skillshotGM.gameOver /*|| !skillshotGM.gameOn && !skillshotGM.isPaused*/)
            {
                animator.SetBool("isNeg", false);
                isFlipped = false;
            }

            flippable = true;
            yield return null;
        }
    }

    //Target moves down and hides after being hit, add to the score
    public void HitTarget()
    {
        //If Positive
        if (isFlipped && !hasBeenHit)
        {
            //Play FX and Audio
            bigHitFX.SetActive(true);
            targetAudio.PlayOneShot(goodHitSound);

            animator.SetBool("isHit", true);

            skillshotGM.score++;

            hasBeenHit = true;
            //Debug.Log("Score is: " + skillshotGM.score);
        }

        //if the wrong side is hit, take a point
        // don't go below 0
        if (!isFlipped)
        {
            targetAudio.PlayOneShot(badHitSound);
            if(skillshotGM.score > 0)
            {
                //Debug.Log("Score was: " + skillshotGM.score);
                skillshotGM.score--;
                //Debug.Log("Decrement score: " + skillshotGM.score);
            }
        }
       
    }



}
