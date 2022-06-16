using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogDismissal : MonoBehaviour
{
    //Simple script. Disables the selected gameobject when the action button is pressed.
    //=========================|FIELDS|=========================
    public GameObject toDismiss;

    //=========================|METHODS|=========================

    void Update()
    {
        if(Input.GetButton("Advance"))
        {
            toDismiss.SetActive(false);
        }
    }
}
