using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Mallet : MonoBehaviour
{
    //[SerializeField] int damage = 10;
    [SerializeField] GameObject target;
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
        transform.rotation = Quaternion.RotateTowards(transform.rotation, target.transform.rotation, speed * Time.deltaTime);
    }
}
