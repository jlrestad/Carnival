using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    public Transform weaponDist;

    public bool isEquipped;
    public float pickUpRange;

    [HideInInspector] public Vector3 distanceToPlayer;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public BoxCollider collider;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        distanceToPlayer = weaponDist.position - transform.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isEquipped)
        {
            Equip();
        }
        else if (Input.GetKeyDown(KeyCode.E) && isEquipped)
        {
            Unequip();
        }
    }

    void Equip()
    {
        Debug.Log("Equip!");

        //collider.enabled = false;
        //rb.isKinematic = true;

        //this.transform.position = weaponDest.position;
        //this.transform.parent = GameObject.Find("ObjectHold").transform;
        this.gameObject.SetActive(false);

        isEquipped = true;
    }

    void Unequip()
    {
        Debug.Log("Unequip!");

        //this.transform.parent = null;

        //rb.isKinematic = false;
        //collider.enabled = true;

        this.gameObject.SetActive(true);

        isEquipped = false;
    }
}
