using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdDest;
    public GameObject player;
    new AudioSource audio;
    
    public bool isHolding;
    public float pickUpRange = .5f;
    public float throwSpeed = 30.0f;

    [HideInInspector] public Vector3 distanceToPlayer;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public new BoxCollider collider;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
        player = GameObject.FindGameObjectWithTag("Player");
        holdDest = GameObject.Find("ObjectHold").transform;
        audio = GetComponent<AudioSource>();
    }

    public void Update()
    {
        distanceToPlayer = holdDest.position - transform.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isHolding && !WeaponEquip.Instance.isEquipped)
        {
            Grab();
        }
        else if (Input.GetKeyDown(KeyCode.E) && isHolding)
        {
            Drop();
        }
        else if (Input.GetMouseButtonDown(0) && isHolding)
        {
            Throw();
        }

        // Turn off audio looping when thrown object stops moving.
        if (rb.velocity.magnitude < 0.7f) { audio.loop = false; }

    }

    // Parents the object to the Player at specified location.
    void Grab()
    {
        Debug.Log("Grab!");

        collider.enabled = false;
        rb.isKinematic = true;
         
        // Only allow one item to be picked up at a time.
        if (holdDest.childCount == 0)
        {
            this.transform.position = holdDest.position;
            this.transform.parent = GameObject.Find("ObjectHold").transform;
        }

        isHolding = true;
    }

    // Unparents the object from the Player.
    void Drop()
    {
        Debug.Log("Dropped!");

        this.transform.parent = null;

        rb.isKinematic = false;
        collider.enabled = true;

        isHolding = false;
    }

    // Adds  force to the object being thrown.
    void Throw()
    {
        Debug.Log("Throw!");

        this.transform.parent = null;

        rb.isKinematic = false;
        collider.enabled = true;

        // Throw
        rb.AddForce(player.transform.forward * throwSpeed, ForceMode.Impulse);
        //rb.velocity = player.transform.forward * throwSpeed; //Another way to move an object.

        isHolding = false;
    }
    
    // Randomizes and plays audio when object is thrown.
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag != "Player" && !isHolding && rb.velocity.magnitude >= 0.7f)
        {
            //if (rb.velocity.magnitude >= 0.3f || rb.velocity.magnitude < 0.7f)
            //{
            //    Debug.Log("LOOP AUDIO");
            //    audio.loop = true; //Continues to play audio when object rolls on the ground.
            //    audio.pitch = Random.Range(0.8f, 1.2f);
            //    audio.Play();
            //}
            //else
            //{
            //    audio.pitch = Random.Range(0.8f, 1.2f);
            //    audio.Play();
            //}

            if (rb.velocity.magnitude != 0f)
            {
                audio.pitch = Random.Range(0.8f, 1.2f);
                audio.Play();
            }
        }
    }
}
