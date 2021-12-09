using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackEmEnemy : MonoBehaviour
{
    public static WhackEmEnemy Instance;

    private void Awake()
    {
        Instance = this;
    }
}
