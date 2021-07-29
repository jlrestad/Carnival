using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingTarget : MonoBehaviour
{
    public float moveSpeed;
    public GameObject point01, point02;
    Vector3 pos;
    public bool movedUp;


    private void Start()
    {
        pos = transform.position;
    }

    private void FixedUpdate()
    {
        StartCoroutine(Move());
    }

    void MoveUp()
    {
        pos.y = point01.transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed);
        //pos = transform.position;
        movedUp = true;
    }

    void MoveDown()
    {
        pos.y = point02.transform.position.y;
        transform.position = Vector3.MoveTowards(transform.position, pos, moveSpeed);
        //pos = transform.position;
        movedUp = false;
    }

    IEnumerator Move()
    {
        if (!movedUp)
        {
            yield return new WaitForSeconds(0.3f);
            MoveUp();
        }
        else
        {
            yield return new WaitForSeconds(0.3f);
            MoveDown();
        }

    }

}
