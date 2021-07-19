using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUp : MonoBehaviour
{
    public Transform holdDest;
    public bool isHolding;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log("You have entered box space.");

            // GRAB
            if (Input.GetKey(KeyCode.E))
            {
                Debug.Log("Grabbing!");

                GetComponent<BoxCollider>().enabled = false;
                GetComponent<Rigidbody>().useGravity = false;
                this.transform.position = holdDest.position;
                this.transform.parent = GameObject.Find("ObjectHold").transform;

                isHolding = true;

            }

            // THROW
            if (Input.GetKey(KeyCode.E) && isHolding)
            {
                Debug.Log("Throwing!");

                this.transform.parent = null;
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<BoxCollider>().enabled = false;

                isHolding = false;
            }
        }
    }

    void GrabObject()
    {
        // GRAB
        if (Input.GetKey(KeyCode.E))
        {
            GetComponent<Rigidbody>().useGravity = false;
            this.transform.position = holdDest.position;
            this.transform.parent = GameObject.Find("ObjectHold").transform;

            isHolding = true;

        }
        
        // THROW
        if (Input.GetKey(KeyCode.E) && isHolding)
        {
            this.transform.parent = null;
            GetComponent<Rigidbody>().useGravity = true;
        }
    }

}
