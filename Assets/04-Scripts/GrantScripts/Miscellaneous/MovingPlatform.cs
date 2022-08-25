using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    /*
     * A simple script to allow players to hop on and ride moving platforms.
     * Grant Hargraves 8/2022
     */

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("Parented");
            other.transform.SetParent(transform, true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.rotation = Quaternion.Euler(0, other.transform.rotation.y, 0); //keep the player from going lopsided

            other.transform.parent = null;
        }
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        Debug.Log("Parented");
    //        collision.gameObject.transform.SetParent(gameObject.transform, true);
    //    }
    //}
    //private void OnCollisionExit(Collision collision)
    //{
    //    if (collision.gameObject.CompareTag("Player"))
    //    {
    //        collision.gameObject.transform.parent = null;
    //    }
    //}
}
