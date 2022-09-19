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

    void Update()
    {

        if (!skillshotGM.gameOver && !skillshotGM.gameWon)
        {
            StartCoroutine(skillshotGM.MoveTargets(pooledTargets, parentPos, direction, moveSpeed, timeBetweenTargets));
            
            if (skillshotGM.isPaused)
            {
                //StopCoroutine(skillshotGM.MoveTargets(pooledTargets, parentPos, direction, moveSpeed, timeBetweenTargets));
                StopAllCoroutines();
            }
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
           
            target.GetComponentInChildren<TargetSetActive>().hasGone = false;
            target.GetComponentInChildren<TargetSetActive>().isFlipped = false;
            target.GetComponentInChildren<TargetSetActive>().reachedEnd = false;
            target.transform.position = parentPos.position;
            target.SetActive(false);
        }
    }
}
