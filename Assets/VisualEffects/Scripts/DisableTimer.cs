using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisableTimer : MonoBehaviour
{
    /*
     * This utility script either destroys or disables a gameobject when a timer runs out.
     * Grant Hargraves - May 2022
     */
    //=========================|FIELDS|=========================
    public float timeBeforeDisable = 5; //time in seconds before the object is disabled/destroyed
    public bool destroyInstead = false; //whether to destroy the gameobject instead of disabling it
    //=========================|METHODS|=========================
    private void Start()
    {
        StartCoroutine(countdown()); //immediately upon start the countdown is started
    }

    private IEnumerator countdown()
    {
        yield return new WaitForSeconds(timeBeforeDisable); //wait for (timeBeforeDisable) seconds
        if(destroyInstead) //if the object is to be destroyed instead of disabled
        {
            Destroy(gameObject); //destroy the object
        }
        else //if the object is just to be disabled
        {
            gameObject.SetActive(false); //set the object to disabled
        }
    }
}
