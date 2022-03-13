using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TargetSetActive : MonoBehaviour
{
    public static TargetSetActive Instance;

    MovingTarget movingTarget;
    SkillShotGameManager skillshotGM;
    [HideInInspector] public MeshRenderer meshRenderer;
    public Transform hideSpot;
    public GameObject targetFace;
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
        meshRenderer = targetFace.GetComponent<MeshRenderer>();
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
            Debug.Log("Trigger working");
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
                meshRenderer.material.color = Color.red; //negative side
            }

            if (isFlipped && !skillshotGM.gameOver)
            {
                yield return new WaitForSeconds(flipTime);
                isFlipped = false;
                meshRenderer.material.color = Color.green; //positive side
            }
            if (skillshotGM.gameOver)
            {
                meshRenderer.material.color = Color.red;
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
        if (targetHit && !isFlipped)
        {
            skillshotGM.score++;

            //cardManager.critterList.Add(this.gameObject);
        }
        else if (targetHit && isFlipped)
        {
            skillshotGM.score--;

            if (skillshotGM.score < 0)
            {
                skillshotGM.score = 0;
            }
        }
    }



}
