using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour
{
    private WeaponType weaponType;
    public float health = 100f;

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
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

}
