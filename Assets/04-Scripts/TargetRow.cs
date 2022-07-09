using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRow : MonoBehaviour
{
    SkillShotGameManager skillshotGM;
    GameObject target;
    public Transform leftPos, rightPos, parentPos;
    public bool levelLoaded;
    public bool moveLeft;
    int direction;

    private void Awake()
    {
        levelLoaded = true;

        //Controls the direction of the targets and where they start
        if (moveLeft) { direction = -1; parentPos = rightPos; }
        else { direction = 1; parentPos = leftPos; }
    }

    private void Start()
    {
        
    }
}
