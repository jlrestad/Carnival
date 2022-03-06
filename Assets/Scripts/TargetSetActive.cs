using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetSetActive : MonoBehaviour
{
    public static TargetSetActive Instance;

    SkillShotGameManager skillshotGM;
    public bool reachedEnd;
    public bool targetHit;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        skillshotGM = GetComponentInParent<SkillShotGameManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "TargetHome")
        {
            Debug.Log("Trigger working");
            reachedEnd = true;
        }
    }

    public void HitTarget()
    {
        //Flip target back after being hit
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);

        targetHit = true;
        //Keep from scoring multiple points
        if (targetHit)
        {
            skillshotGM.score++;

            //flip target... etc

            //    //Add game object to Moving Target array.
            //    cardManager.critterList.Add(this.gameObject);
        }
    }
}
