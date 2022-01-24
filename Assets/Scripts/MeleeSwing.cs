using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    public Target target;
    GameCardManager cardManager;
    //[SerializeField] WhackEmEnemy newWhackEm;
    //[SerializeField] GameObject closestWhackEm;
    [SerializeField] WhackEmEnemy[] whackEmEnemy;
    [SerializeField] GameObject headPrefab;
    [SerializeField] Rigidbody rb;
    Vector3 distanceToPlayer;
    [SerializeField] float meleeRange = 2f;
    [SerializeField] int damage = 50;

    private void Start()
    {
        //target = Target.Instance;
        whackEmEnemy = FindObjectsOfType<WhackEmEnemy>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1") || Input.GetAxis("RtTrigger") > 0 && Input.GetAxis("RtTrigger") < 0.9)
        {
            MeleeAttack();
        }    
        if (Input.GetButtonUp("Fire1") || Input.GetAxis("RtTrigger") == 0.9)
        {
            Return();
        }
        else 
        {
            return;
        }

        //Find ClosestWhackEm script
        //Doesn't update when player moves -- need to fix!
        foreach (WhackEmEnemy whackEm in whackEmEnemy)
        {
            distanceToPlayer = transform.position - whackEm.transform.position;

            if (distanceToPlayer.magnitude <= meleeRange)
            {
                target = whackEm.GetComponent<Target>();
                rb = whackEm.GetComponent<Rigidbody>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Critter"))
        {
            cardManager = other.GetComponentInParent<GameCardManager>();

            target.health -= damage; 

            if (target.health <= 0f)
            {
                target.health = 0f;

                //Add game object to Moving Target array.
                cardManager.targetsList.Add(other.gameObject);

                //Spawn the head used as throwing object
                SpawnHead();
                //Turn off critter
                other.gameObject.SetActive(false);
            }

            Debug.Log("Smashed enemy!");
        }
    }

    public void SpawnHead()
    {
        //rb.isKinematic = false;

        //Spawn the head used as throwing object
        Instantiate(headPrefab, rb.transform.position, Quaternion.identity);
    }

    // Find the enemy that is closest to the player
    //public GameObject FindClosestWhackEm()
    //{
    //    float distanceToClosesWhackEm = Mathf.Infinity;

    //    WhackEmEnemy[] allWhackEms = GameObject.FindObjectsOfType<WhackEmEnemy>(); //Array to hold all weapons of the scene

    //    // Move through the list of weapons to find the closest
    //    foreach (WhackEmEnemy whackEm in allWhackEms)
    //    {
    //        float distanceToWhackEm = (whackEm.transform.position - this.transform.position).sqrMagnitude;

    //        if (distanceToWhackEm < distanceToClosesWhackEm)
    //        {
    //            distanceToClosesWhackEm = distanceToWhackEm; //update the closest weapon
    //            newWhackEm = whackEm; //set the closest weapon
    //            string targetName = newWhackEm.gameObject.name.ToString(); //get the name of the closest weapon

    //            closestWhackEm = GameObject.Find(targetName); //use the name of the weapon to get the game object that is attached so it can be returned

    //            whackEmList.Add(closestWhackEm);

    //            distanceToPlayer = transform.position - closestWhackEm.transform.position;
    //        }
    //    }

    //    return closestWhackEm;
    //}

    public void MeleeAttack()
    {
        transform.Rotate(Vector3.right, 90f);
    }

    public void Return()
    {
        transform.Rotate(Vector3.right, 30f);
    }

    protected void LateUpdate()
    {
        //Lock z and y rotation
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, 0f);
    }
}
