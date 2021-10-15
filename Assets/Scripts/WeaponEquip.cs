using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public enum WeaponTypes
{
    Shoot,
    Melee,
    Throw
}

public class WeaponEquip : MonoBehaviour
{
    public static WeaponEquip Instance;

    //private List<GameObject> weapons = new List<GameObject>();
    [SerializeField] GameObject[] weapons;

    [SerializeField] GameObject currentWeapon = null;
    int weaponNumber = -1;



    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        //Forward
        if (Input.GetAxisRaw("Mouse ScrollWheel") > 0)
        {
            if (currentWeapon != null)
            {
                currentWeapon.SetActive(false);
            }
           
            weaponNumber = (weaponNumber + 1);

            if (weaponNumber >= weapons.Length)
            {
                weaponNumber = 0;
            }

            currentWeapon = weapons[weaponNumber];
            currentWeapon.SetActive(true);
        }

        //Reverse
        if (Input.GetAxisRaw("Mouse ScrollWheel") < 0)
        {
            if (currentWeapon != null)
            {
                currentWeapon.SetActive(false);
            }

            weaponNumber = (weaponNumber - 1);

            if (weaponNumber < 0)
            {
                weaponNumber = weapons.Length-1;
            }

            currentWeapon = weapons[weaponNumber];
            currentWeapon.SetActive(true);
        }

        //Put weapon away
        if (Input.GetButtonDown("Fire2") && currentWeapon.activeInHierarchy == true)
        {
            currentWeapon.SetActive(false);
        }
    }


}
