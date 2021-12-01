using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ThrowablesInventory : MonoBehaviour
{
    [SerializeField] List<Transform> inventory;
    //[SerializeField] GameObject[] inventory;
    //[SerializeField] GameObject throwable;
    //PickUp pickup;

    private void Start()
    {
        //throwable = gameObject.GetComponentInChildren<Head>();
    }

    //private void Update()
    //{
    //    if (transform.childCount != 0)
    //    {
    //        for (int i = 0; i < transform.childCount; i++)
    //        {
    //            inventory.Add(transform.GetChild(i));
    //        }
    //    }
    //}
}
