using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBehavior : MonoBehaviour
{
    public GameObject tent;
    public Animator animator;

    GameObject player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player.GetComponent<FPSController>().cardCount == 3)
        {
            tent.SetActive(false);
        }
    }

}
