using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class DialogReader : MonoBehaviour
{
    /*
    * -A modular script used to read dialog lines one at a time from the DialogHolder script. Attach to the player.
    * -This dialog system is meant to be used in the project "FUnfair," and as needed in prototyping projects.
    * -Note that this system only works in 3D at present.
    * Grant Hargraves - May 2022 
    */

    /* ==========================================================
     * =========================|FIELDS|=========================
     * ==========================================================*/

    [Header("STATS")]
    [Tooltip("Time in seconds before the dialog box fades out.")]
    [SerializeField] float textFadeOutTime = 5; //time in seconds before the dialog box fades out.
    //-------------------------
    [Header("PLUG-IN")]
    [Tooltip("The gameobject holding the UI elements in the dialog box goes here.")]
    [SerializeField] GameObject DialogArea; //Gameobject holding the UI elements for the text box
    [Tooltip("Dialog to be 'read aloud' in the game. Displayed more prominently.")]
    [SerializeField] Text SpokenText; //Dialog to be read aloud in the game. Displayed more prominently.
    [Tooltip("Dialog used in tutorials or to guide the player. Displayed less prominently.")]
    [SerializeField] Text GuideText; //Dialog used in tutorials or to guide the player. Displayed less prominently.
    //-------------------------
    [Header("INTERNAL/DEBUG (Don't touch)")]
    [Tooltip("The animator attached to the DialogArea gameobject. Make sure it has one of these.")]
    [SerializeField] Animator TextAnimator; //The animator attached to the DialogArea gameobject.
    [Tooltip("A placeholder for referencing the DialogHolder script on dialog triggers.")]
    [SerializeField] DialogHolder currentDialog; //A placeholder for referencing the DialogHolder script on dialog triggers.

    /* ================================================================
     * =========================|MAIN METHODS|=========================
     * ================================================================*/ 

    //-----|Start|-----Assigns the proper reference to the TextAnimator variable.-----
    private void Start()
    {
        TextAnimator = DialogArea.GetComponent<Animator>(); //Take the animator component from the DialogArea object.
    }

    //-----|OnTriggerEnter|-----Handles the activation of dialog display when making contact with an object tagged as "Dialog."-----
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Dialog") //if we touch a trigger tagged as "Dialog"
        {
            currentDialog = other.GetComponent<DialogHolder>(); //reference the DialogHolder script on the object we just touched.
            if(currentDialog != null) //as long as currentDialog is not null...
            {
                if (TextAnimator != null) //as long as we have an animator component on the DialogArea object...
                {
                    SpokenText.text = currentDialog.spokenDialog; //input the dialog from the holder into the SpokenDialog text.
                    //GuideText.text = currentDialog.guideDialog; //input the dialog from the holder into the GuideDialog text.
                    textFadeOutTime = currentDialog.fadeOutTime; //input the amount of time for the text to fade out.
                    TextAnimator.SetTrigger("FadeIn"); //tell the animator to start fading the text box in.
                    StopCoroutine("FadeTimer"); //stop the timer in case it was already running
                    StartCoroutine("FadeTimer"); //(re)start the timer for the text to start fading out.
                }
                else
                {
                    Debug.Log("Dialog box has no animator component or it is otherwise null."); //give a debug message.
                }
            }
            else //if the currentdialog is read as null...
            {
                Debug.Log("Dialog object has no DialogHolder script or it is otherwise null."); //give a debug message.
            }
            other.gameObject.SetActive(false); //turn off the gameobject so we don't trigger the dialog again.
        }
    }

    private IEnumerator FadeTimer()
    {
        
        yield return new WaitForSecondsRealtime(textFadeOutTime); //wait for the amount of time specified by textFadeoutTime;
        TextAnimator.SetTrigger("FadeOut"); //tell the animator to start fading the text box out.
    }
}
