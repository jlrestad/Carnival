using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuProtecc : MonoBehaviour
{
    //Just a dumb little class to protect the game manager from being destroyed on load
    void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

}
