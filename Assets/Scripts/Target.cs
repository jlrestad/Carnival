using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100f;
    MeshRenderer enemy;

    private void Start()
    {
        enemy = GetComponent<MeshRenderer>();
    }

    public void TakeDamage(int damageAmount)
    {
        health -= damageAmount;

        if (health <= 0f)
        {
            Die();
        }
    }

    void Die()
    {
        if (CompareTag("WhackEm"))
        {
            MeshRenderer head = GetComponentInChildren<MeshRenderer>();
            Rigidbody rb = GetComponentInChildren<Rigidbody>();
            
            head.enabled = true;
            rb.isKinematic = false;

            //TODO: Create a spawn point on critter where the head will appear after it is killed. This should keep
            // the mesh from bouncing around and allow the head mesh to be enabled.

            enemy.enabled = false;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
