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
    int direction;

    [Header("BOOLS")]
    public bool targetFlipped;
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
        //Controls the direction of the targets and where they start
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
        if (skillshotGM.gameOn)
        {
            //Controls the direction of the targets and where they start
            if (moveLeft) { direction = -1; parentPos = rightPos; }
            else { direction = 1; parentPos = leftPos; }
        }

        StartCoroutine(skillshotGM.MoveTargets(pooledTargets, parentPos, direction, moveSpeed, timeBetweenTargets));

    }

}
