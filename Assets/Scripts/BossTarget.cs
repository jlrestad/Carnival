using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTarget : MonoBehaviour
{
    Animator animator;

    public Transform hideSpot;
    public GameObject targetParent;
    public int flipTime;
    public int downTime;  //How long the target will be down.
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
        while (!targetHit)
        {
            if (!isFlipped)
            {
                yield return new WaitForSeconds(flipTime);
                isFlipped = true; //Neg side showing

                animator.SetBool("isPos", false);
                animator.SetBool("isNeg", true);
            }

            if (isFlipped)
            {
                yield return new WaitForSeconds(flipTime);
                isFlipped = false; //Pos side showing

                animator.SetBool("isNeg", false);
                animator.SetBool("isPos", true);

            }
            yield return null;
        }
    }

    //Target moves down and hides after being hit, add to the score
    public IEnumerator HitTarget()
    {
        //slide down and hide
        if (!isFlipped)
        {
            targetHit = true;
            //transform.position = Vector3.Lerp(transform.position, hideSpot.position, 1.0f);
            //transform.position = hideSpot.position;
            //Turn to negative side.
            //animator.SetBool("isPos", false);
            //animator.SetBool("isNeg", true);
            animator.SetBool("reset", false);
            animator.SetBool("isHit", true);  //Target is hit, move down
        }

        yield return new WaitForSeconds(downTime);
        
        targetHit = false;

        //Target returns to parent position after downTime has passed.
        //transform.position = Vector3.Lerp(transform.position, targetParent.transform.position, 1.0f);
        //transform.position = targetParent.transform.position;

        //Turn to positive side.
        //animator.SetBool("isPos", true);
        //animator.SetBool("isNeg", false);
        animator.SetBool("reset", true);  //Target return to regular position
        animator.SetBool("isHit", false);  //Target has not been hit and is allowed to spin.
    }
}
