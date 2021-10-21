using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    public float health = 100f;
    [SerializeField] GameObject spawnHead;
    [SerializeField] GameObject headPrefab;

    private void Awake()
    {
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
            //Call method to throwable object
            WhackEmEnemy.Instance.SpawnHead();

            this.gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

}
