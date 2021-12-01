using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdDest;
    public Transform skullHold;
    public GameObject player;
    new AudioSource audio;
    
    public bool isHolding;
    public float pickUpRange = .5f;
    public float throwSpeed = 30.0f;

    [HideInInspector] public Vector3 distanceToPlayer;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public Collider collider;

    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<Collider>();
        audio = GetComponent<AudioSource>();

        player = GameObject.FindGameObjectWithTag("Player");
        holdDest = GameObject.Find("ObjectHold").transform;
        skullHold = GameObject.Find("SkullHold").transform;
    }

    public void Update()
    {
        distanceToPlayer = holdDest.position - transform.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetButtonDown("ActionButton") && !isHolding && !WeaponEquip.Instance.isEquipped)
        {
            Grab();
        }
        else if (Input.GetButtonDown("ActionButton") && isHolding)
        {
            Drop();
        }
        else if (Input.GetButtonDown("Fire1") && isHolding || Input.GetAxis("RtTrigger") > 0 && isHolding)
        {
            Throw();
        }

        // Turn off audio looping when thrown object stops moving.
        if (rb.velocity.magnitude < 0.7f) { audio.loop = false; }

    }

    // Parents the object to the Player at specified location.
    void Grab()
    {
        if (CompareTag("Head"))
        {
            //Allow only 6 skulls to be held.
            if (skullHold.childCount < 6)
            {
                //Put skulls into a list
                //player.GetComponent<WeaponEquip>().skullList.Add(gameObject);
                
                //Hide skull in scene
                gameObject.SetActive(false);

                collider.enabled = false;
                rb.isKinematic = true;

                //Add skull to the hold position on FPSPlayer
                this.transform.position = skullHold.position;
                this.transform.parent = skullHold.transform;
            }

            //Show first skull in count
            skullHold.GetChild(0).gameObject.SetActive(true);

            isHolding = true;
        }
       else
       {
            //Debug.Log("Grab!");

            collider.enabled = false;
            rb.isKinematic = true;
         
            // Only allow one item to be picked up at a time.
            if (holdDest.childCount == 0)
            {
                this.transform.position = holdDest.position;
                this.transform.parent = holdDest.transform;
            }

            isHolding = true;
       }

    }

    // Unparents the object from the Player.
    void Drop()
    {
        //Debug.Log("Dropped!");
        
        if (CompareTag("Head"))
        {
            return;
        }
        else
        {
            this.transform.parent = null;

            rb.isKinematic = false;
            collider.enabled = true;

            isHolding = false;
        }
    }

    // Adds  force to the object being thrown.
    void Throw()
    {
        //Debug.Log("Throw!");

        this.transform.parent = null;

        rb.isKinematic = false;
        collider.enabled = true;

        // Throw
        rb.velocity = holdDest.transform.forward * throwSpeed; //Throws with an arc

        //Show first skull in count
        if (skullHold.childCount != 0)
        {
            skullHold.GetChild(0).gameObject.SetActive(true);
        }

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
