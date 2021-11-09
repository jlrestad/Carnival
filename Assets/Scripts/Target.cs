using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Target : MonoBehaviour
{
    public static Target Instance;

    Menu menu;

    [HideInInspector] public WhackEmEnemy spawnHead;

    [SerializeField] float health = 100f;
    [SerializeField] int score = 1;
    [SerializeField] List<Target> targets;
    int targetAmount;

    private void Awake()
    {
        Instance = this;

        menu = GetComponent<Menu>();

        targets.Add(GetComponent<Target>());
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
            spawnHead.SpawnHead();

            this.gameObject.SetActive(false);
        }
        else
        {
            //gameObject.SetActive(false);
            //GameLevel.Instance.targetList.RemoveAt(0);

            //Disable the movement
            MovingTarget mt = gameObject.GetComponent<MovingTarget>();
            mt.enabled = false;

            //Flip target back after being hit
            transform.rotation = Quaternion.Euler(90f, 0, 0);
        }
    }

}
