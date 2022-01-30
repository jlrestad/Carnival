using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArmFollowMovement : MonoBehaviour
{
    bool canSwing;

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetAxis("RtTrigger") > 0 && canSwing)
        {
            StartCoroutine(MeleeAttack());
        }
    }

    IEnumerator MeleeAttack()
    {
        transform.Rotate(Vector3.right, 90f);
        canSwing = false;

        yield return new WaitForSeconds(0.5f);

        transform.Rotate(Vector3.right, 30f);
        canSwing = true;
    }
}
