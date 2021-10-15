using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mallet : MonoBehaviour
{
    //[SerializeField] int damage = 10;
    [SerializeField] float speed = 5f;
    RaycastHit hit;
    [SerializeField] LayerMask layerMask;

    void Start()
    {
        
    }


    void FixedUpdate()
    {
      
    }

    public void Swing()
    {
    }
}
