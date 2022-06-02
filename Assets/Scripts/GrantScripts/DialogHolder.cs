using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogHolder : MonoBehaviour
{
    /*
     * -A modular script used to store individual dialog lines, attached to trigger zones or other kinds of gameobjects.
     * -This dialog system is meant to be used in the project "FUnfair," and as needed in prototyping projects.
     * -Note that this system only works in 3D at present.
     * Grant Hargraves - May 2022
     */
    //=========================|FIELDS|=========================
    public string spokenDialog = ""; //The dialog spoken out loud in-game. Public so that the DialogReader script can see it.
    public string guideDialog = ""; //The dialog used for tutorials and guidance for the player. Public so that the DialogReader script can see it.
    //=========================|METHODS|=========================
    //No methods here. This script is only meant to hold variables :)
}
