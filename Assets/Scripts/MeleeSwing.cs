using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    public Target target;
    //[SerializeField] WhackEmEnemy newWhackEm;
    //[SerializeField] GameObject closestWhackEm;
    [SerializeField] WhackEmEnemy[] whackEmEnemy;
    Vector3 distanceToPlayer;
    [SerializeField] float meleeRange = 2f;
    [SerializeField] int damage = 50;

    private void Start()
    {
        target = Target.Instance;
        whackEmEnemy = FindObjectsOfType<WhackEmEnemy>();
    }

    private void Update()
    {
        //FindClosestWhackEm();

        if (Input.GetButtonDown("Fire1"))
        {
            MeleeAttack();
        }    
        if (Input.GetButtonUp("Fire1"))
        {
            Return();
        }


        foreach (WhackEmEnemy whackEm in whackEmEnemy)
        {
            distanceToPlayer = transform.position - whackEm.transform.position;

            if (distanceToPlayer.magnitude <= meleeRange)
            {
                target = whackEm.GetComponent<Target>();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "WhackEm" || other.gameObject.tag == "Enemy")
        {
            target.TakeDamage(damage);

            Debug.Log("Smashed enemy!");
        }
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
