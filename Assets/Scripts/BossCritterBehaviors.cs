using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCritterBehaviors : MonoBehaviour
{

    public GameObject player;
    public Transform target;
    public Quaternion initial;

    private void Update()
    {
       
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;

        Vector3 dir = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir);

        Vector3 newRot = Quaternion.Lerp(this.transform.rotation, rotation, 1).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, newRot.y, 0f);

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
