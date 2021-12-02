using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Head : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bucket"))
        {
            Debug.Log("Head trigger working");
            other.GetComponentInParent<GameCardManager>().targetsList.Add(other.transform.gameObject);

            this.gameObject.SetActive(false);
        }
    }
}
