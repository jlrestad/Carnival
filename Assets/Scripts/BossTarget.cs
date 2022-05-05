using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTarget : MonoBehaviour
{
    Animator animator;

    public Transform hideSpot;
    public GameObject targetParent;
    public int flipTime;
    public bool targetHit;
    public bool isFlipped;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponentInParent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(FlipAround());
    }

    //Flip target from front to back within the flip time set
    public IEnumerator FlipAround()
    {
     if (!isFlipped)
            {
                yield return new WaitForSeconds(flipTime);
                isFlipped = true;

                animator.SetBool("isHit", true);
            }

            if (isFlipped)
            {
                yield return new WaitForSeconds(flipTime);
                isFlipped = false;

                animator.SetBool("isHit", false);
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

        ////Keep from scoring multiple points
        //if (targetHit && isFlipped)
        //{
        //    skillshotGM.score--;

        //    //Keep the score from being less than 0.
        //    if (skillshotGM.score < 0)
        //    {
        //        skillshotGM.score = 0;
        //    }
        //}
        //else if (targetHit && !isFlipped)
        //{
        //    skillshotGM.score++;
        //}
    }
}
