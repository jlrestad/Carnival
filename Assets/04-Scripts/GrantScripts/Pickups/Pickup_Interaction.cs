using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class Pickup_Interaction : MonoBehaviour
{
    /*
     * This script allows the player to interact with and collect pickups.
     * Grant Hargraves 7/2022
     */
    //==================================================
    //=========================|FIELDS|
    //==================================================
    [Header("STATS")]
    [SerializeField] float raycastDistance = 3;
    //-------------------------
    [Header("PLUG-INS")]
    [SerializeField] HudManager myHUD;
    [SerializeField] GameObject ActionIcon_Keyboard;
    [SerializeField] GameObject ActionIcon_Controller;
    [SerializeField] Text infoText;
    [SerializeField] Menu controlScript;
    //-------------------------
    [Header("INTERNAL/DEBUG")]
    [SerializeField] bool pickupInFocus = false;
    public bool usingController = false;
    [SerializeField] GameObject selectedPickup; //the pickup currently focused on
    //==================================================
    //=========================|BUILT-IN METHODS|
    //==================================================
    //--------------------------------------------------|Start|
    private void Start()
    {
        //specify the control scheme being used here.
        if(myHUD == null)
        {
            Debug.Log("HudManager script could not be found. Pickup interaction unavailable.");
        }
    }
    //--------------------------------------------------|Update|
    private void Update()
    {
        usingController = Menu.Instance.usingJoystick;

        CheckPickup();
    }
    //==================================================
    //=========================|CUSTOM METHODS|
    //==================================================
    private void CheckPickup()
    {
        RaycastHit hit; //declare a raycast to use
        if (Physics.Raycast(gameObject.transform.position, transform.TransformDirection(Vector3.forward), out hit, raycastDistance)) //fire out a raycast
        {
            //Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow); //show the raycast
            if(hit.collider.tag == "Pickup") //if hitting a collider with the "pickup" tag...
            {
                Ticket_Pickups pickupID;
                pickupID = hit.collider.GetComponent<Ticket_Pickups>();
                if (pickupID != null) //if the object has a pickups script on it
                {
                    if (!pickupInFocus)
                    {
                        pickupInFocus = true; //turn pickupInFocus on
                        infoText.text = pickupID.myPrompt;
                        infoText.gameObject.SetActive(true);
                        if (usingController) ActionIcon_Controller.SetActive(true); //use the proper action icon
                        else if (!usingController) ActionIcon_Keyboard.SetActive(true);
                    }
                    if (Input.GetButtonDown("ActionButton")) //if hitting the action button while looking at a valid pickup...
                    {
                        pickupID.CollectPickup(); //tell it to be collected
                        pickupID.TogglePickupActive(); //tell it to disable itself
                        //Below just turns off the selection icons instead of letting them linger
                        pickupInFocus = false; //turn pickupInFocus off
                        ActionIcon_Keyboard.SetActive(false); //turn all action icons off
                        ActionIcon_Controller.SetActive(false);
                        infoText.gameObject.SetActive(false);
                        hit.collider.enabled = false;
                    }
                }
                else
                {
                    Debug.Log("Could not find a Ticket_Pickups script on the collected object. Pickup failed.");
                }
                
            }
            else //if hitting anything else...
            {
                if (pickupInFocus)
                {
                    pickupInFocus = false; //turn pickupInFocus off
                    ActionIcon_Keyboard.SetActive(false); //turn all action icons off
                    ActionIcon_Controller.SetActive(false);
                    infoText.gameObject.SetActive(false);
                }
            }
        }
        else //if hitting nothing at all...
        {
            pickupInFocus = false; //turn pickupInFocus off
            ActionIcon_Keyboard.SetActive(false); //turn all action icons off
            ActionIcon_Controller.SetActive(false);
            infoText.gameObject.SetActive(false);
        }
    }
}
