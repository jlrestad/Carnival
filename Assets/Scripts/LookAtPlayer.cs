using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{

    public GameObject player;
    public Transform target;

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;

        transform.LookAt(target);
    }
}
