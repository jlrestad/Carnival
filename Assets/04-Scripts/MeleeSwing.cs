using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    public static MeleeSwing Instance;

    private Transform player;
    private Camera playerCamera;
    [SerializeField] float range = 5f;

    CharacterController characterController;

    [SerializeField] bool canSwing;
    [SerializeField] float force = 10.0f;
    [HideInInspector] RaycastHit hit;

    [Header("VFX")]
    [SerializeField] GameObject hitEnemyVFX; //Enemy hit VFX prefab
    [SerializeField] GameObject hitColliderVFX; //Collider hit VFX prefab
    [Header("AUDIO")]
    [SerializeField] AudioSource hitEnemySound; //Sound played when enemy is hit
    [SerializeField] AudioSource hitColliderSound; //Sound played when collider is hit

    [Header("SPAWNED OBJECTS")]
    [SerializeField] GameObject brokenCrate; //Broken crate prefab
    [SerializeField] GameObject VFXSpawnPoint; //GameObject where VFX will show on mallet

    Vector3 distanceToPlayer;
    
    private new Collider enemyCollider;

    //for boss fight
    //[SerializeField] BossCritterBehaviors[] bossCritters;
    //[SerializeField] CritterSpawnerManager spawnerManager;
    //[SerializeField] GameObject hold; //reference the object hold so it actually finds the critter
    //[SerializeField] float bossRange; //set to 1 in inspector. for some reason it keeps reverting to 2 when i initialize in code
    //public GameObject boss;
    //public SkillShotGameManager skillshotGM;  //** -- Why is this needed?

    // these are for testing purposes-- remove from full game. see notes in update
    //public bool ssWon;
    //public bool csWon;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //Initialize
        player = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera = player.GetComponentInChildren<Camera>();
        characterController = player.GetComponent<CharacterController>();

        canSwing = true;

        //spawnerManager = FindObjectOfType<CritterSpawnerManager>();
        //skillshotGM = FindObjectOfType<SkillShotGameManager>();
        //boss = GameObject.FindGameObjectWithTag("Boss");
        //hold = GameObject.FindGameObjectWithTag("ObjectHold");
    }

    private void Update()
    {
        //can remove from full game.
        // need to comment these two lines when in boss AI scene, and check the boxes for these bools in the inspector
        //if (skillshotGM.gameWon) ssWon = true;
        //if (whackemGM.gameWon) csWon = true;

        if (Input.GetButtonDown("Fire1") && canSwing || Input.GetAxis("RtTrigger") > 0 && canSwing)
        {
            Debug.DrawLine(playerCamera.transform.position, playerCamera.transform.forward, Color.green); //Draw a line to show the direction of the raycast.

            //Physically swing the mallet.
            StartCoroutine(SwingMallet());

            //Send a raycast out from the player as far as the range.
            if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.TransformDirection(Vector3.forward), out hit, range))
            {
                //Debug.Log(hit.distance);
                Debug.Log(hit.transform.name); //Return the name of what the raycast hit.

                Transform target = hit.transform.GetComponent<Transform>(); //For breakable
                CritterEnemy enemy = hit.transform.GetComponent<CritterEnemy>();
                CarnivalSmashGameManager carnivalsmashGM = hit.transform.GetComponentInParent<CarnivalSmashGameManager>();

                enemyCollider = hit.collider;
                

                //FOR BREAKABLES
                if (target != null && target.CompareTag("CrateBreakable"))
                {
                    //Swap unbroken for broken
                    Instantiate(brokenCrate, target.transform.position, target.transform.rotation);
                    Destroy(target.gameObject);
                }
                if (target.CompareTag("CrateBroken"))
                {
                    //Add force to the broken object rigidbody
                    hit.rigidbody.AddForce(target.up * force);
                }

                //FOR CRITTERS
                if (target != null && enemy)
                {
                    //Show hit VFX to let player know it has been hit.
                    GameObject hitVfx = Instantiate(hitEnemyVFX, enemy.transform.position, Quaternion.identity);
                    Destroy(hitVfx, 0.5f);

                    enemy.hasBeenHit = true;

                    hitEnemySound.Play();

                    //Turn off enemy after hit
                    enemy.HitEnemy();

                    //Increase speed after each hit
                    carnivalsmashGM.IncreaseSpeed();

                   
                    //Add to the score
                    if (!carnivalsmashGM.isTaunting)
                    {
                        carnivalsmashGM.score++;
                    }
                }
                else if (hit.collider && !enemy)
                {
                    //Show hit VFX to let player know it has been hit.
                    GameObject hitVfx = Instantiate(hitColliderVFX, VFXSpawnPoint.transform.position, Quaternion.identity);
                    Destroy(hitVfx, 0.5f);
                }
                //if minigames won and boss is active run this loop
                // replace if line with this line after we don't need boss AI scene anymore
                // if (skillshotGM.gameWon && whackemGM.gameWon && boss.activeInHierarchy)

                //*** CODE FOR BOSS APPEARANCE ***  
                //if (ssWon && csWon && boss.activeInHierarchy)
                //{
                //    Debug.Log("BossAI melee");
                //    //this part for Boss fight
                //    //continualy find boss critters && find the object hold for bossAI fight
                //    bossCritters = FindObjectsOfType<BossCritterBehaviors>();
                //    Transform hld = hold.transform;

                //    if (Physics.Raycast(hld.position, hld.forward, out hit, bossRange))
                //    {
                //        Debug.Log("Hit: " + hit.collider.name);
                //        BossCritterBehaviors bossCritter = hit.transform.GetComponent<BossCritterBehaviors>();
                //        bossCritter.hasBeenHit = true;
                //    }
                //}
                //else
                //{
                //this part for CS game
                //Get raycast hit information and use it to calculate damage
                //Look into spherecast to see if this will be better 

            }
        }
    }

    IEnumerator SwingMallet()
    {
        //Keep player from clicking the card screen off by accident. 
        if (CarnivalSmashGameManager.Instance.displayScreen.activeInHierarchy)
        {
            canSwing = false;
            yield return new WaitForSeconds(1);
            canSwing = true;
        }

        transform.Rotate(Vector3.right, 60f);
        canSwing = false;

        yield return new WaitForSeconds(0.2f);

        transform.Rotate(Vector3.right, -60f);
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

    //Lock X and Y of the mallet during the swing.
    protected void LateUpdate()
    {
        //Lock z and y rotation
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, 0f);
    }
}
