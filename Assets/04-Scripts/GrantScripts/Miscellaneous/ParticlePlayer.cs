using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePlayer : MonoBehaviour
{
    /*
     * This script is a simple utility script that can be called to play any particle effects nested underneath the object it is attached to.
     * Grant Hargraves 7/2022
     */
    //==================================================
    //=========================|FIELDS|
    //==================================================
    [SerializeField] List<ParticleSystem> myParticleSystems = new List<ParticleSystem>(); //an editable list of all the particle systems parented under this object.
    //==================================================
    //=========================|BUILT-IN METHODS|
    //==================================================
    //When enabled, the object will play all its particle effects automatically.
    private void OnEnable()
    {
        PlayFX();
    }
    //==================================================
    //=========================|CUSTOM METHODS|
    //==================================================
    //This method can also be called directly if needed.
    public void PlayFX()
    {
        for(int i = 0; i < myParticleSystems.Count; i++)
        {
            ParticleSystem currentFX = myParticleSystems[i];
            currentFX.Play();
        }
    }

}
