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
                isFlipped = true;

                animator.SetBool("isPos", false);
                animator.SetBool("isNeg", true);
            }

            if (isFlipped)
            {
                yield return new WaitForSeconds(flipTime);
                isFlipped = false;

                animator.SetBool("isNeg", false);
                animator.SetBool("isPos", true);

            }
            yield return null;
        }
    }

    //Target moves down and hides after being hit, add to the score
    public IEnumerator HitTarget()
    {
        targetHit = true;

        //Turn to negative side.
        animator.SetBool("isPos", false);
        animator.SetBool("isNeg", true);

        //slide down and hide
        if (isFlipped)
        {
            transform.position = Vector3.Lerp(transform.position, hideSpot.position, 1.0f);
        }

        yield return new WaitForSeconds(downTime);
        
        targetHit = false;

        //Target returns to parent position after downTime has passed.
        transform.position = Vector3.Lerp(transform.position, targetParent.transform.position, 1.0f);
    }
}
