using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    /*
     * A simple script to allow players to hop on and ride moving platforms.
     * Grant Hargraves 8/2022
     */

    private void OnCollisionEnter(CharacterController collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            Debug.Log("Parented");
            collision.gameObject.transform.SetParent(gameObject.transform, true);
        }
    }
    private void OnCollisionExit(Collision collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            collision.gameObject.transform.parent = null;
        }
    }
}
