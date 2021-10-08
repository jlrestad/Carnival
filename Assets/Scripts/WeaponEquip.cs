using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponEquip : MonoBehaviour
{
    public Transform player;
    public Transform holsterPos;
    public GameObject activeWeapon;
    public GameObject holsteredWeapon;

    public bool isEquipped;
    public float pickUpRange;

    [HideInInspector] public Vector3 distanceToPlayer;
    [HideInInspector] public Vector3 distanceToHolster;
    [HideInInspector] public Rigidbody rb;
    [HideInInspector] public new BoxCollider collider;

    // Start is called before the first frame update
    public void Start()
    {
        rb = GetComponent<Rigidbody>();
        collider = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    public void Update()
    {
        distanceToPlayer = player.position - transform.position;
        distanceToHolster = holsterPos.position - player.position;

        if (distanceToPlayer.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && !isEquipped && activeWeapon.activeInHierarchy == false)
        {
            Equip();
        }
        else if (distanceToHolster.magnitude <= pickUpRange && Input.GetKeyDown(KeyCode.E) && isEquipped && activeWeapon.activeInHierarchy == true)
        {
            Unequip();
        }
    }

    void Equip()
    {
        Debug.Log("Equip!");

        holsteredWeapon.SetActive(false);
        activeWeapon.SetActive(true);

        //LOCK PLAYER MOVEMENT FOR BOOTH GAME
        //player.GetComponentInParent<CharacterController>().enabled = false;

        isEquipped = true;
    }

    void Unequip()
    {
        Debug.Log("Unequip!");

        holsteredWeapon.SetActive(true);
        activeWeapon.SetActive(false);

        player.GetComponentInParent<CharacterController>().enabled = true;

        isEquipped = false;
    }

    void GetCharacterController()
    {
        if (isEquipped)
            player.GetComponent<CharacterController>().enabled = false;
        else
            player.GetComponent<CharacterController>().enabled = true;

    }
}
