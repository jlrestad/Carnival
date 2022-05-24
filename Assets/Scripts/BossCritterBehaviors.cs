using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossCritterBehaviors : MonoBehaviour
{

    public GameObject player;
    public Transform target;
    public Quaternion initial;

    public bool hasBeenHit;

    public GameObject head;

    private void Start()
    {
        this.hasBeenHit = false;
    }

    private void Update()
    {
        
        player = GameObject.FindGameObjectWithTag("Player");
        target = player.transform;

        Vector3 dir = target.position - transform.position;
        Quaternion rotation = Quaternion.LookRotation(dir);

        Vector3 newRot = Quaternion.Lerp(this.transform.rotation, rotation, 1).eulerAngles;
        this.transform.rotation = Quaternion.Euler(0f, newRot.y, 0f);

        if (hasBeenHit)
        {
            Debug.Log("BossCritter hit! Disappear!");
            
            //needs code for skull to spawn
            //need to hook up to tickets system
            StartCoroutine(disappear());
        }
        

    }

    IEnumerator disappear() //this method can be replaced with the animation & skull spawn
    {
        //save initial position
        Vector3 spot = transform.position;
        
        this.gameObject.transform.Translate(-Vector3.up * Time.deltaTime);
        yield return new WaitForSeconds(1);
        this.gameObject.SetActive(false);

        //This block instantiates a skull @ position. used head as placeholder, but throws errors atm (unsure what needs to be hooked up atm. does pop up tho)
        // replace head with skull prefab
        /*
        head = Instantiate(head, spot, Quaternion.identity);          
        yield return new WaitForSeconds(2);
        Destroy(head);
        */

        StopCoroutine(disappear());
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
