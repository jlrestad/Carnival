using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cinematic_Manager : MonoBehaviour
{
    /*
     * This script handles the introduction sequence, fades, and ending sequence(s) of the Funfair Demo.
     * Grant Hargraves - June 2022
     */
    //=========================|FIELDS|=========================
    [Header("PLUG-INS")]
    [Tooltip("The gameobject that performs the animations handled here")]
    [SerializeField] GameObject myBlackbox; //the gameobject that performs the animations we handle here.
    private Animator myAnimator; //the animator attribute attached to myBlackBox-- assigned automatically.
    private CinHolder currentHolder; //the Cinholder attached to the object we most recently collided with.


    //=========================|METHODS|=========================
    void Start()
    {
        myAnimator = myBlackbox.GetComponent<Animator>();
        if(myAnimator == null)
        {
            Debug.Log("Your Blackbox (for the cinematics) has no animator.");
        }
    }

    void Update()
    {
        if(Input.GetButton("Advance"))
        {
            myAnimator.SetBool("dismiss", true);
        }
    }

    public void playCinematic(string cinTitle)
    {
        if(cinTitle.Equals("Intro"))
        {
            myAnimator.SetBool("showStart", true); //start the intro sequence
            //and also turn off all the other bools
            myAnimator.SetBool("dismiss", false);
            myAnimator.SetBool("showStart", false);
            myAnimator.SetBool("blackScreen", false);
        }
        if(cinTitle.Equals("ShowcaseEnding"))
        {
            StartCoroutine(ShowcaseEndingSequence());
        }
    }

    public IEnumerator ShowcaseEndingSequence()
    {
        myAnimator.SetBool("blackScreen", true); //start the ending sequence
        yield return new WaitForSeconds(2f); //wait for the screen to go black
        myAnimator.SetBool("showEnding", true); //play the showcase ending sequence
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Dialog")
        {
            currentHolder = other.GetComponent<CinHolder>();
            if(currentHolder != null)
            {
                playCinematic(currentHolder.CinematicTitle);
            }
        }
    }
}
