using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private WeaponType weaponType;
    public float health = 100f;
<<<<<<< Updated upstream
=======
    [SerializeField] GameObject spawnHead;
    [SerializeField] GameObject headPrefab;

    private void Awake()
    {
    }
>>>>>>> Stashed changes

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
<<<<<<< Updated upstream
        //Destroy(gameObject);
        gameObject.SetActive(false);
=======
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
>>>>>>> Stashed changes
    }

}
