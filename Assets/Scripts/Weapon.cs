using System.Collections;
using System.Collections.Generic;
using UnityEngine;

<<<<<<< Updated upstream
public enum WeaponType
{
    Gun,
    Mallet,
    Dynomite
}

public class Weapon : MonoBehaviour
{



=======
public class Weapon : MonoBehaviour
{
    public static Weapon Instance;

    private void Awake()
    {
        Instance = this;
    }
>>>>>>> Stashed changes
}
