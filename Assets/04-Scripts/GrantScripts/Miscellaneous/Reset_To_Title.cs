using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Reset_To_Title : MonoBehaviour
{
    public GameObject titleMenu;
    public GameObject titleCam;

    [SerializeField] HudManager hudManager;

    private void Start()
    {
        Menu myMenu = GameObject.FindObjectOfType<Menu>();
        hudManager = FindObjectOfType<HudManager>();
        titleCam = myMenu.titleCamera;
        titleMenu = myMenu.titleScreen;

    }
    public void ResetToTitle()
    {
        //Reset minigames

        //Display the Intro scene
        titleMenu.SetActive(true);
        titleCam.SetActive(true);

        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        SceneManager.UnloadSceneAsync(1);

        //Reset the audio
        hudManager.myMixer.SetFloat("MusicVolume", hudManager.musicVolume);
        hudManager.myMixer.SetFloat("SFXVolume", hudManager.sfxVolume);
        hudManager.myMixer.SetFloat("PlayerVolume", hudManager.playerVolume);
    }
}
