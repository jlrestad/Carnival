using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using Random = UnityEngine.Random;

public class MovingTarget : MonoBehaviour
{
    public static MovingTarget Instance;

    Target target;
    GameObject player;
    [SerializeField] WeaponEquip WE;

    Vector3 pos;

    public int score;
    public float moveSpeed;
    public bool movedUp;

    [SerializeField] float randX, randY, randNum;
    [SerializeField] float xMinBoundary, yMinBoundary;
    [SerializeField] float xMaxBoundary, yMaxBoundary;

    [Space(15)]
    public GameObject[] targetsArray;
    public List<GameObject> targetsList;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        WE = player.GetComponent<WeaponEquip>();
        pos = transform.position;
        xMinBoundary = transform.position.x;
        yMinBoundary = transform.position.y;
    }

    private void Update()
    {
        DisplayGameCard();
    }

    public void DisplayGameCard()
    {
        if (targetsList.Count == targetsArray.Length)
        {
            for (int i = 0; i < WE.gameCards.Length; i++)
            {
                if (WE.gameCards[i].name == WE.levelName)
                {
                    WE.gameCards[i].SetActive(true);
                }
            }
        }
    }

    public void FixedUpdate()
    {
        Movement();
    }

    //protected void LateUpdate()
    //{
    //    //Lock x and z rotation
    //    transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
    //}

    public void Movement()
    {
        StartCoroutine(Move());
    }

    void MoveUp()
    {
        randX = Random.Range(xMinBoundary, xMaxBoundary);
        randY = Random.Range(yMinBoundary, yMaxBoundary);

        pos += new Vector3(0f, randY,0f);

        for (int i = 0; i < targetsList.Count; i++)
        {
            targetsList[i].transform.position = Vector3.MoveTowards(targetsList[i].transform.position, pos, moveSpeed);
            movedUp = true;
        }
    }

    void MoveDown()
    {
        //randomNum = Random.Range(0f, 1f);
        pos += new Vector3(0f, yMinBoundary, 0f);

        for (int i = 0; i < targetsList.Count; i++)
        {
            targetsList[i].transform.position = Vector3.MoveTowards(targetsList[i].transform.position, pos, moveSpeed);
            movedUp = false;
        }
    }

    public IEnumerator Move()
    {
        if (!movedUp)
        {
            Debug.Log("MOVING UP");

            yield return new WaitForSeconds(0.5f);
            MoveUp();
        }
        else
        {
            Debug.Log("MOVING DOWN");

            yield return new WaitForSeconds(0.5f);
            MoveDown();
        }

    }

}
