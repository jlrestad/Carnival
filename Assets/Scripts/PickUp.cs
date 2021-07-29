using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdDest;
    public GameObject player;
    
    public bool isHolding;
    public float pickUpRange;
    public float throwSpeed = 30.0f;

    [HideInInspector] public Vector3 distanceToPlayer;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public new BoxCollider collider;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void Update()
    {
        distanceToPlayer = holdDest.position - transform.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isHolding)
        {
            Grab();
        }
        else if (Input.GetKeyDown(KeyCode.Q) && isHolding)
        {
            Drop();
        }
        else if (Input.GetKeyDown(KeyCode.E) && isHolding)
        {
            Throw();
        }
    }

    void Grab()
        {
            Debug.Log("Grab!");

            collider.enabled = false;
            rb.isKinematic = true;
            
            this.transform.position = holdDest.position;
            this.transform.parent = GameObject.Find("ObjectHold").transform;

            isHolding = true;
        }

        void Drop()
        {
            Debug.Log("Dropped!");

            this.transform.parent = null;

            rb.isKinematic = false;
            collider.enabled = true;

            isHolding = false;
        }

    void Throw()
    {
        Debug.Log("Throw!");

        this.transform.parent = null;

        rb.isKinematic = false;
        collider.enabled = true;

        // Throw
        rb.AddForce(player.transform.forward * throwSpeed, ForceMode.Impulse);
        //rb.velocity = player.transform.forward * throwSpeed;

        isHolding = false;
    }
}
