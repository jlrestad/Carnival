using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetSetActive : MonoBehaviour
{
    public static TargetSetActive Instance;

    MovingTarget movingTarget;
    SkillShotGameManager skillshotGM;

    //[HideInInspector] public MeshRenderer meshRenderer;

    Animator animator;

    public Transform hideSpot;
    public GameObject targetParent;
    public float flipTime;
    public float shakeTime;
    public bool reachedEnd;
    public bool targetHit;
    public bool isFlipped;
    public bool hasGone;

    public GameObject bigHitFX;
    public AudioSource targetAudio;
    public AudioClip goodHitSound, badHitSound, flipSound, shakeSound;

    //Unneeded since more than one target uses this script.
    //private void Awake()
    //{
    //    Instance = this;
    //}

    private void Start()
    {
        skillshotGM = GetComponentInParent<SkillShotGameManager>();
        movingTarget = GetComponentInParent<MovingTarget>();
        animator = GetComponentInParent<Animator>();
        //meshRenderer = targetFace.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        StartCoroutine(FlipAround());

    }

    //Controls when to restart the loop
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TargetHome")
        {
            reachedEnd = true;
            hasGone = true;
        }
    }

    //Flip target from front to back within the flip time set
    public IEnumerator FlipAround()
    {
        while (skillshotGM.gameOn)
        {
            if (!isFlipped && !skillshotGM.gameOver)
            {
                yield return new WaitForSeconds(flipTime);
                animator.SetBool("shake", true);
                yield return new WaitForSeconds(shakeTime);
                isFlipped = true;

                animator.SetBool("shake", false);
                animator.SetBool("isPos", false);
                animator.SetBool("isNeg", true);
            }

            if (isFlipped && !skillshotGM.gameOver)
            {
                yield return new WaitForSeconds(flipTime);
                animator.SetBool("shake", true);
                yield return new WaitForSeconds(shakeTime);
                isFlipped = false;


                animator.SetBool("shake", false);
                animator.SetBool("isNeg", false);
                animator.SetBool("isPos", true);
            }

            //if (skillshotGM.gameOver || !skillshotGM.gameOn)
            //{
            //    targetParent.transform.rotation = Quaternion.Euler(0, 0, 0); //Turn all targets to the backside.
            //}
            yield return null;
        }
    }

    //Target moves down and hides after being hit, add to the score
    public void HitTarget()
    {
        //slide down and hide
        if (!isFlipped)
        {
            bigHitFX.SetActive(true);
            targetAudio.PlayOneShot(goodHitSound);
            transform.position = Vector3.Lerp(transform.position, hideSpot.position, 1.0f);
        }

        targetHit = true;

        if (targetHit && !isFlipped)
        {
            skillshotGM.score++;
            //Debug.Log("Score is: " + skillshotGM.score);
        }

        //if the wrong side is hit, take a point
        // don't go below 0
        if( targetHit && isFlipped)
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
