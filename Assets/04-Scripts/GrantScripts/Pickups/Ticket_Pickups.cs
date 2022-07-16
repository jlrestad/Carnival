using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ticket_Pickups : MonoBehaviour
{
    /*
     * This script handles the management and effects of the individual pickup item it's attached to.
     * ---NOTE: This script should be attached to the parent gameobject that holds both the active and inactive groups.
     * This is to make sure the object behaves properly when using an object pooler.
     * ---For management of what happens when a pickup is collected, check the main Player Controller script.
     * Grant Hargraves 7/2022
     */
    //==================================================
    //=========================|FIELDS|
    //==================================================
    [HideInInspector] public enum PickupType { RedTicket, TicketString, BlueTicket };
    //-------------------------
    [Header("STATS")]
    [Tooltip("The type of pickup this script is attached to. This determines what methods will be triggered in the player's main script")]
    public PickupType myPickupType;
    //-------------------------
    [Header("PLUG-INS")]
    [Tooltip("The group that will show when the pickup has not been collected yet and is active in the scene.")]
    [SerializeField] GameObject myActiveGroup;
    [Tooltip("The group that will display effects and appear invisible as soon as the pickup is collected.")]
    [SerializeField] GameObject myInActiveGroup;
    [Tooltip("Put here whatever script manages the tickets/health that the player has.")]
    [SerializeField] PlayerHealthManager healthScript;
    //-------------------------
    [Header("INTERNAL/DEBUG")]
    [SerializeField] bool healthScriptPresent = true; //keeps script from throwing errors if healthScript is not present

    //==================================================
    //=========================|BUILT-IN METHODS|
    //==================================================

    void Start()
    {
        if(healthScript == null) //if we can't find a PlayerHealthManager script assigned to this object...
        {
            healthScriptPresent = false; //flag the health script as not present to prevent bugs
        }
        if(! myActiveGroup.activeInHierarchy)
        {
            myActiveGroup.SetActive(true);
        }
    }

    //==================================================
    //=========================|CUSTOM METHODS|
    //==================================================
    public void CollectPickup()
    {
        if(myPickupType == PickupType.RedTicket && healthScriptPresent)
        {
            //healthScript stuff goes here
        }
        if (myPickupType == PickupType.TicketString && healthScriptPresent)
        {
            //healthScript stuff goes here
        }
        if (myPickupType == PickupType.BlueTicket && healthScriptPresent)
        {
            //healthScript stuff goes here
        }
        if(! healthScriptPresent)
        {
            Debug.Log("Couldn't find a PlayerHealthManager attached to this pickup. Collect Pickup failed.");
        }
    }


    //-------------------------
    //This method is used to turn off the pickup when collected, and to turn it back on when called from an object pooler.=====|TogglePickupActive|
    public void TogglePickupActive()
    {
        if(myActiveGroup.activeInHierarchy && healthScriptPresent) //if the active group is currently active...
        {
            //NOTE: This usually occurs when a pickup is collected by the player.
            myActiveGroup.SetActive(false); //turn it off
            myInActiveGroup.SetActive(true); //turn on the inactive group
            StartCoroutine(disableTimer()); //start the timer for this object to disable itself
        }
        else if(myActiveGroup.activeInHierarchy && healthScriptPresent)
        {
            //NOTE: This usually occurs when respawned or called by an object pooler.
            myActiveGroup.SetActive(true);
            myInActiveGroup.SetActive(false);
            //MyInactive group should already be inactive at this point anyway, but just in case this is called by some other script...

        }
        else if(! healthScriptPresent)
        {
            Debug.Log("Couldn't find a PlayerHealthManager attached to this pickup. Toggle active failed.");
        }
    }
    //-------------------------|DisableTimer|
    private IEnumerator disableTimer()
    {
        yield return new WaitForSeconds(1.5f);
        myInActiveGroup.SetActive(false); //disable the inactive group
        this.gameObject.SetActive(false); //disable the parent gameobject of this script
        //The above is performed in order to make the object compatible with object poolers.
    }
}
