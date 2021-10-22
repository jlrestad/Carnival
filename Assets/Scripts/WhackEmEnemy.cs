using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WhackEmEnemy : MonoBehaviour
{
    public static WhackEmEnemy Instance;

    [SerializeField] GameObject spawnHead;
    [SerializeField] GameObject headPrefab;
    Rigidbody rb;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        rb = GetComponentInChildren<Rigidbody>();
    }

    public void SpawnHead()
    {
        //Spawn the head used as throwing object
        Instantiate(headPrefab, spawnHead.transform.position, Quaternion.identity);
        rb.isKinematic = false;
    }
}
