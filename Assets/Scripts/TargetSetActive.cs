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
    public int flipTime;
    public bool reachedEnd;
    public bool targetHit;
    public bool isFlipped;


    private void Awake()
    {
        Instance = this;
    }

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
            //Debug.Log("Trigger working");
            reachedEnd = true;
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
                isFlipped = true;
                //meshRenderer.material.color = Color.red; //negative side

                //Rotate the target to the positive side.
                //targetParent.transform.rotation = Quaternion.Euler(0, 180, 0);
                animator.SetBool("isHit", true);
            }

            if (isFlipped && !skillshotGM.gameOver)
            {
                yield return new WaitForSeconds(flipTime);
                isFlipped = false;
                //meshRenderer.material.color = Color.green; //positive side

                //Rotate the target back around
                //targetParent.transform.rotation = Quaternion.Euler(0, 0, 0);
                animator.SetBool("isHit", false);
            }

            if (skillshotGM.gameOver || !skillshotGM.gameOn)
            {
                //meshRenderer.material.color = Color.red;
                targetParent.transform.rotation = Quaternion.Euler(0, 0, 0); //Turn all targets to the backside.
            }
            yield return null;
        }
    }

    //Target moves down and hides after being hit, add to the score
    public void HitTarget()
    {
        //slide down and hide
        if (!isFlipped)
        {
            transform.position = Vector3.Lerp(transform.position, hideSpot.position, 1.0f);
        }

        targetHit = true;

        //Keep from scoring multiple points
        if (targetHit && isFlipped)
        {
            skillshotGM.score--;

            //Keep the score from being less than 0.
            if (skillshotGM.score < 0)
            {
                skillshotGM.score = 0;
            }
        }
        else if (targetHit && !isFlipped)
        {
            skillshotGM.score++;
        }
    }



}
