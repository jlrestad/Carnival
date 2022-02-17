using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    public int health;
    GameCardManager cardManager;
    public Transform spawnTransform;
    //[SerializeField] WhackEmEnemy newWhackEm;
    //[SerializeField] GameObject closestWhackEm;
    [SerializeField] WhackEmEnemy[] whackEmEnemy;
    [SerializeField] WhackEmGameManager whackemGM;
    [SerializeField] GameObject headPrefab;
    Vector3 distanceToPlayer;
    [SerializeField] float meleeRange = 2f;
    [SerializeField] int damage = 50;
    [SerializeField] bool canSwing;

    private void Start()
    {
        //target = Target.Instance;
        whackEmEnemy = FindObjectsOfType<WhackEmEnemy>();
        whackemGM = FindObjectOfType<WhackEmGameManager>();
        spawnTransform = GetComponentInChildren<Transform>();
        canSwing = true;
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetAxis("RtTrigger") > 0 && canSwing)
        {
            StartCoroutine(MeleeAttack());
        }

        //Find ClosestWhackEm script
        //Doesn't update when player moves -- need to fix!
        foreach (WhackEmEnemy whackEm in whackEmEnemy)
        {
            distanceToPlayer = transform.position - whackEm.transform.position;

            if (distanceToPlayer.magnitude <= meleeRange)
            {
                health = whackEm.GetComponent<WhackEmEnemy>().health;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Critter"))
        {
            //Get references from this enemy
            WhackEmEnemy thisEnemy = other.GetComponent<WhackEmEnemy>();
            cardManager = other.GetComponentInParent<GameCardManager>();

            //Do random damage
            damage = Random.Range(50, 100);
            //Minus damage amount from enemy
            thisEnemy.health -= damage;
            //Check the health of the enemy
            thisEnemy.HealthManager();

            if (thisEnemy.hasBeenHit)
            {
                whackemGM.IncreaseSpeed();
            }

            if (thisEnemy.health <= 0)
            {
                //Add to the score
                whackemGM.score++;
                //Add enemy to the list
                cardManager.critterList.Add(other.gameObject);

                //Spawn the head used as throwing object
                SpawnHead();
            }



            Debug.Log("Smashed enemy!");
        }
    }

    public void SpawnHead()
    {
        //Spawn the head used as throwing object
        Instantiate(headPrefab, spawnTransform.position, Quaternion.identity);
    }

    IEnumerator MeleeAttack()
    {
        transform.Rotate(Vector3.right, 90f);
        canSwing = false;

        yield return new WaitForSeconds(0.5f);

        transform.Rotate(Vector3.right, -20f);
        canSwing = true;
    }

    protected void LateUpdate()
    {
        //Lock z and y rotation
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, 0f);
    }
}
