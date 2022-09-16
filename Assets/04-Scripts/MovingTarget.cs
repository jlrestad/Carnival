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

    [HideInInspector] public WeaponEquip weaponEquip;

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
        skillshotGM.PoolObjects(targetPrefab, pooledTargets, poolAmount, parentPos, targetParent);
        weaponEquip = FindObjectOfType<WeaponEquip>();
    }

    public bool sentHome = false;
    
    void Update()
    {

        if (!skillshotGM.gameOver && !skillshotGM.gameWon)
        {
            //int count = 0;
            //count++;
            //Debug.Log("move targets " + count);
            StartCoroutine(skillshotGM.MoveTargets(pooledTargets, parentPos, direction, moveSpeed, timeBetweenTargets));
            
        }
        
        if (!skillshotGM.gameOn)
        {
            ResetTargets();
        }
    }

    public void ResetTargets()
    {
        //Debug.Log("left game area, resetting");
        foreach (GameObject target in pooledTargets)
        {
            if (target.GetComponentInChildren<TargetSetActive>().hasGone == true)
            {
                target.GetComponentInChildren<TargetSetActive>().hasGone = false;
            }
            if (target.GetComponentInChildren<TargetSetActive>().isFlipped == true)
            {
                target.GetComponentInChildren<TargetSetActive>().isFlipped = false;
            }
        }
    }
}
