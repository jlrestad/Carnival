using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingTarget : MonoBehaviour
{
    public static MovingTarget Instance;

    SkillShotGameManager skillshotGM;

    [TextArea]
    [SerializeField] string notes;

    [Header("MOVEMENT")]
    public Transform leftPos;
    public Transform rightPos;
    public Transform parentPos;
    [Space(10)]
    public float moveSpeed;
    public float timeBetweenTargets;
    [HideInInspector] public int direction;

    [Header("BOOLS")]
    public bool moveLeft;
    bool gameOn;

    [Header("POOLED OBJECTS")]
    public GameObject targetPrefab;
    [Space(10)]
    public int poolAmount;
    [Space(10)]
    public List<GameObject> pooledTargets = new List<GameObject>();
    Transform targetParent;

    private void Awake()
    {
        if (moveLeft) { direction = -1; parentPos = rightPos; }
        else { direction = 1; parentPos = leftPos; }
    }

    private void Start()
    {
        targetParent = this.transform;
        skillshotGM = GetComponentInParent<SkillShotGameManager>();
        skillshotGM.PoolObjects(targetPrefab, pooledTargets, poolAmount, leftPos, rightPos, parentPos, targetParent);
    }

    void Update()
    {
        if (!skillshotGM.gameOver)
        {
            StartCoroutine(skillshotGM.MoveTargets(pooledTargets, parentPos, direction, moveSpeed, timeBetweenTargets));
        }
        else
        {
            //Turn off targets if game is over
            // **** Need to lock player within gameplay boundary so targets can finish thos loop for replay ****.
            for (int i = 0; i < pooledTargets.Count; i++)
            {
                pooledTargets[i].transform.Translate(direction * Vector3.right * (moveSpeed * Time.deltaTime), Space.Self);
                //pooledTargets[i].transform.position = parentPos.position;
                if (skillshotGM.reachedEnd)
                {
                    pooledTargets[i].SetActive(false);
                    //skillshotGM.PoolObjects(targetPrefab, pooledTargets, poolAmount, leftPos, rightPos, parentPos, targetParent);
                }
            }
        }
    }

}
