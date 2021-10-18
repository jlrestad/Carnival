using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;


public class WeaponEquip : MonoBehaviour
{
    public static WeaponEquip Instance;

    //[SerializeField] GameObject gun, mallet; //use to find distance from player (magnitude)
    [Space(15)]
    [SerializeField] public List<GameObject> weapons;
    [Space(15)]
    public GameObject currentWeapon = null;
    public int weaponNumber = 0;


    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        //Debug.Log("List amount: " + weapons.Count);

        //Roll scroll wheel forward
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            //If there is already a weapon equipped, hide it.
            if (currentWeapon != null && weapons.Count > 0)
            {
                currentWeapon.SetActive(false);
                weaponNumber++; //move to next list weapon
                currentWeapon = weapons[weaponNumber]; //set current weapon to the index number
                currentWeapon.SetActive(true); //show the weapon
            }

            //Set the bounds.
            if (weaponNumber > weapons.Count)
            {
                weaponNumber = 0;
            }

            //Roll scroll wheel back
            if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
            {
                //If there is already a weapon equipped, hide it.
                if (currentWeapon != null && weapons.Count > 0)
                {
                    currentWeapon.SetActive(false);
                    weaponNumber--; //move to the previous list weapon
                    currentWeapon = weapons[weaponNumber]; //set current weapon to the index number
                    currentWeapon.SetActive(false); //show the weapon
                }

                //Set the bounds.        
                if (weaponNumber < 0)
                {
                    weaponNumber = 0;
                }
            }
        }
    }
}
