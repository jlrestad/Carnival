using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCritterBehaviors : MonoBehaviour
{

    public GameObject player;
    public Transform target;

    private void Update()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;

        transform.LookAt(target);
    }

    // For boss fight. Turned prefabs into triggers to allow
    // For some reason OnCollisionEnter does not work.
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("COLLISON using trigger");
            // need to hook up red cards for player health during boss fight
        }
    }
}
