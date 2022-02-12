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
    [SerializeField] WhackEmGameManager[] whackEmGM;
    [SerializeField] GameObject headPrefab;
    Vector3 distanceToPlayer;
    [SerializeField] float meleeRange = 2f;
    [SerializeField] int damage = 50;
    [SerializeField] bool canSwing;

    private void Start()
    {
        //target = Target.Instance;
        whackEmEnemy = FindObjectsOfType<WhackEmEnemy>();
        whackEmGM = FindObjectsOfType<WhackEmGameManager>();
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
            cardManager = other.GetComponentInParent<GameCardManager>();

            //Do random damage
            damage = Random.Range(50, 100);
            other.GetComponent<WhackEmEnemy>().health -= damage;
            
            if (other.GetComponent<WhackEmEnemy>().hasBeenHit)
            {
                IncreaseSpeed();
            }

            //
            if (health <= 0)
            {
                health = 0;

                //Add game object to Moving Target array.
                cardManager.critterList.Add(other.gameObject);

                //Spawn the head used as throwing object
                SpawnHead();
                //Turn off critter
                other.gameObject.SetActive(false);
            }

            Debug.Log("Smashed enemy!");
        }
    }

    //
    void IncreaseSpeed()
    {
        //Do random damage
        //if (whackEmEnemy.hasBeenHit)
        //Speed up appear time
        //

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
