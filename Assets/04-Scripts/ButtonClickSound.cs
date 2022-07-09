using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonClickSound : Selectable
{
    BaseEventData m_BaseEvent;

    AudioSource audioSource;

    private void Update()
    {
        //Check if the button is highlighted
        if (IsHighlighted() == true) 
        {
            audioSource.Play();
        }
    }
}
