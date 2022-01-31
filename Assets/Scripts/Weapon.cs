using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public static Weapon Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        
    }
}
