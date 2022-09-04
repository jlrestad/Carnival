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
    public float shakeTime;
    public bool reachedEnd;
    public bool targetHit;
    public bool isFlipped;
    public bool hasGone;

    public GameObject bigHitFX;
    public AudioSource targetAudio;
    public AudioClip goodHitSound, badHitSound, flipSound, shakeSound;
    public bool flippable;
    public float OGFlipTime;

    //Unneeded since more than one target uses this script.
    //private void Awake()
    //{
    //    Instance = this;
    //}

    private void Start()
    {
        flippable = true;
        skillshotGM = GetComponentInParent<SkillShotGameManager>();
        movingTarget = GetComponentInParent<MovingTarget>();
        animator = GetComponentInParent<Animator>();
        OGFlipTime = flipTime;
        //meshRenderer = targetFace.GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(flippable)
        {
            flipTime = OGFlipTime * Random.Range(0.3f, 3f);
            StartCoroutine(FlipAround());
        }
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
        while (skillshotGM.gameOn && !skillshotGM.isPaused)
        {
            //If Positive
            if (!isFlipped && !skillshotGM.gameOver && !skillshotGM.isPaused)
            {
                yield return new WaitForSeconds(flipTime);

                //Flip to negative
                animator.SetBool("isNeg", true);

                isFlipped = true;
                //flippable = false;

            }

            //If Negative
            if (isFlipped && !skillshotGM.gameOver)
            {
                yield return new WaitForSeconds(flipTime);

                //Flip to positive
                animator.SetBool("isNeg", false);

                isFlipped = false;
                targetHit = false;
            }

            if (skillshotGM.gameOver || !skillshotGM.gameOn && !skillshotGM.isPaused)
            {
                //targetParent.transform.rotation = Quaternion.Euler(0, 0, 0); //Turn all targets to the backside.
                animator.SetBool("isNeg", true);
                isFlipped = true;
            }

            flippable = true;
            yield return null;
        }
    }

    //Target moves down and hides after being hit, add to the score
    public void HitTarget()
    {
        //If Positive
        //slide down and hide
        if (isFlipped)
        {
            //Play FX and Audio
            bigHitFX.SetActive(true);
            targetAudio.PlayOneShot(goodHitSound);

            //Move target to this position to hide
            transform.position = Vector3.Lerp(transform.position, hideSpot.position, 1.0f * Time.deltaTime);
        }

        targetHit = true;

        if (targetHit && isFlipped)
        {
            skillshotGM.score++;
            //Debug.Log("Score is: " + skillshotGM.score);
        }

        //if the wrong side is hit, take a point
        // don't go below 0
        if(targetHit && !isFlipped)
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
