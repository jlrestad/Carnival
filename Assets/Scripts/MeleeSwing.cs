using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeSwing : MonoBehaviour
{
    [SerializeField] Target newTarget;
    [SerializeField] GameObject closestTarget;
    List<GameObject> targetList;
    Vector3 distanceToPlayer;
    [SerializeField] int damage = 10;

    private void Start()
    {
        newTarget = GameObject.FindObjectOfType<Target>();
    }

    private void Update()
    {
        //FindClosestTarget();

        if (Input.GetButtonDown("Fire1"))
        {
            MeleeAttack();
        }    
        if (Input.GetButtonUp("Fire1"))
        {
            Return();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy" || other.gameObject.tag == "WhackEm")
        {
            newTarget.TakeDamage(damage);

            Debug.Log("Damaged enemy!");
        }
    }

    // Find the enemy that is closest to the player
    //public GameObject FindClosestTarget()   
    //{
    //    float distanceToClosestTarget = Mathf.Infinity;

    //    Target[] allTargets = GameObject.FindObjectsOfType<Target>(); //Array to hold all weapons of the scene

    //    // Move through the list of weapons to find the closest
    //    foreach (Target target in allTargets)
    //    {
    //        float distanceToTarget = (target.transform.position - this.transform.position).sqrMagnitude;

    //        if (distanceToTarget < distanceToClosestTarget)
    //        {
    //            distanceToClosestTarget = distanceToTarget; //update the closest weapon
    //            newTarget = target; //set the closest weapon
    //            string targetName = newTarget.gameObject.name.ToString(); //get the name of the closest weapon

    //            closestTarget = GameObject.Find(targetName); //use the name of the weapon to get the game object that is attached so it can be returned

    //            targetList.Add(closestTarget);
    //        }
    //    }

    //    return closestTarget;
    //}

    public void MeleeAttack()
    {
        transform.Rotate(Vector3.right, 87f);
    }

    public void Return()
    {
        transform.Rotate(Vector3.right, 36f);
    }

    protected void LateUpdate()
    {
        //Lock z and y rotation
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 0f, 0f);
    }
}
