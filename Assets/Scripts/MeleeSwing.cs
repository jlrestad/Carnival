using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    public Transform player;
    [SerializeField] float range = 25f;

    CharacterController characterController;
    public int health;
    GameCardManager cardManager;
    public Transform spawnTransform;

    //[SerializeField] WhackEmEnemy newWhackEm;
    //[SerializeField] GameObject closestWhackEm;
    [SerializeField] WhackEmEnemy[] whackEmEnemy;
    [SerializeField] WhackEmGameManager whackemGM;
    [SerializeField] GameObject headPrefab;
    [SerializeField] float meleeRange = 3.0f;
    [SerializeField] int damage;
    [SerializeField] bool canSwing;
    [HideInInspector] RaycastHit hit;
    [SerializeField] GameObject hitVfxPrefab;

    Vector3 distanceToPlayer;
    
    private new Collider enemyCollider;


    private void Start()
    {
        //Initialize
        player = GameObject.FindGameObjectWithTag("Player").transform;
        characterController = player.GetComponent<CharacterController>();
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


            //Get raycast hit information and use it to calculate damage
            //Look into spherecast to see if this will be better 
            if (Physics.Raycast(player.position, player.forward, out hit, range))
            {
                //Debug.Log(hit.distance);
                //Debug.DrawLine(player.position, player.forward, Color.yellow);
                //Debug.Log(hit.transform.name);
                
                //Target target = hit.transform.GetComponent<Target>();
                WhackEmEnemy enemy = hit.transform.GetComponent<WhackEmEnemy>();

                enemyCollider = hit.collider;

                if (enemy != null)
                {
                    //Show hit VFX to let player know it has been hit.
                    GameObject hitVfx = Instantiate(hitVfxPrefab, enemy.transform.position, Quaternion.identity);
                    Destroy(hitVfx, 0.5f);
                    enemy.hasBeenHit = true;

                    cardManager = enemy.GetComponentInParent<GameCardManager>();

                    //Increase speed after each hit
                    whackemGM.IncreaseSpeed();
                    //Turn off enemy after hit
                    enemy.HealthManager();

                    //Add to the score
                    if (!whackemGM.isTaunting)
                    {
                        whackemGM.score++;
                    }
                    else
                    {
                        //Taunting so no points given
                        Debug.Log("HAHA ~ No point!");
                    }

                    //Add enemy to the list
                    cardManager.critterList.Add(enemy.gameObject);

                    //Spawn the head used as throwing object
                    SpawnHead();
                    
                    Debug.Log("Smashed enemy!");
                }
            }

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

    public void SpawnHead()
    {
        //Spawn the head used as throwing object
        Instantiate(headPrefab, spawnTransform.position, Quaternion.identity);
    }

    IEnumerator MeleeAttack()
    {
        transform.Rotate(Vector3.right, 90f);
        canSwing = false;

        yield return new WaitForSeconds(0.05f);

        transform.Rotate(Vector3.right, -45f);
        canSwing = true;
    }

    //void MeleeAttack()
    //{
    //    //Get raycast hit information and use it to calculate damage
    //    if (Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, range))
    //    {
    //        Debug.Log(hit.transform.name);
    //        Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
    //        //Target target = hit.transform.GetComponent<Target>();
    //        WhackEmEnemy enemy = hit.transform.GetComponent<WhackEmEnemy>();

    //        if (enemy != null && enemy.CompareTag("Critter"))
    //        {
    //            //Get references from this enemy
    //            WhackEmEnemy thisEnemy = enemy.GetComponent<WhackEmEnemy>();
    //            cardManager = enemy.GetComponentInParent<GameCardManager>();

    //            //Do random damage
    //            damage = Random.Range(50, 100);
    //            //Minus damage amount from enemy
    //            thisEnemy.health -= damage;
    //            //Check the health of the enemy
    //            thisEnemy.HealthManager();

    //            if (thisEnemy.hasBeenHit)
    //            {
    //                whackemGM.IncreaseSpeed();
    //            }

    //            //Only add to the score if health went to 0.
    //            if (thisEnemy.health <= 0)
    //            {
    //                //Add to the score
    //                if (!whackemGM.isTaunting)
    //                {
    //                    whackemGM.score++;
    //                }
    //                else
    //                {
    //                    Debug.Log("HAHA ~ No point!");
    //                }

    //                //Add enemy to the list
    //                cardManager.critterList.Add(enemy.gameObject);

    //                //Spawn the head used as throwing object
    //                SpawnHead();
    //            }
    //            Debug.Log("Smashed enemy!");
    //        }
    //    }
    //}

    protected void LateUpdate()
    {
        //Lock z and y rotation
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, 0f);
    }
}
