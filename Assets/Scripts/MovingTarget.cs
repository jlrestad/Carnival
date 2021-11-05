using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class MovingTarget : MonoBehaviour
{
    public static MovingTarget Instance;

    public float moveSpeed;
    public GameObject point01, point02;
    Vector3 pos;
    public bool movedUp;
    [SerializeField] double angleToMove;
    [SerializeField] double radius;

    float timeCounter = 0f;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        pos = transform.position;
    }

    public void FixedUpdate()
    {
        //Target move around a cirle
        //timeCounter += Time.deltaTime;

        //float x = Mathf.Cos(timeCounter);
        //float y = Mathf.Sin(timeCounter);
        //float z = 0;

        //transform.position = new Vector3(x, y, z);

        Movement();
    }

    public void Movement()
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

    public IEnumerator Move()
    {
        if (!movedUp)
        {
            yield return new WaitForSeconds(0.5f);
            MoveUp();
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
            MoveDown();
        }

    }

}
