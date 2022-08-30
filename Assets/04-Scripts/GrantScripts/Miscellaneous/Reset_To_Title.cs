using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Reset_To_Title : MonoBehaviour
{
    public GameObject titleMenu;
    public GameObject titleCam;
    public Menu menu;
    [SerializeField] HudManager hudManager;

    private void Start()
    {
        menu = FindObjectOfType<Menu>();
        hudManager = FindObjectOfType<HudManager>();
        titleCam = menu.titleCamera;
        titleMenu = menu.titleScreen;
    }

    //CALLS THE METHODS TO SET RESET THE GAME
    public void ResetGamePrefs()
    {
        //Reset counter for pause screen
        menu.counter = -1;

        ShowCursor(); //Allows the cursor to be used on the Title menu
        ResetAudioPrefs(); //Sets the audio back to original levels
        ResetToTitle(); //Turns on the Title screen and camera of the Intro scene, and unloads the game scene.
        menu.ClearButton(); //Clears button into in Event System so that the 1st button of the menu is highlighted.
        menu.PlayIntroMusic(); //Play the Title audio
    }

    //RESETS THE AUDIO TO ORIGINAL LEVELS
    public void ResetAudioPrefs()
    {
        hudManager.myMixer.SetFloat("MusicVolume", hudManager.musicVolume);
        hudManager.myMixer.SetFloat("SFXVolume", hudManager.sfxVolume);
        hudManager.myMixer.SetFloat("PlayerVolume", hudManager.playerVolume);
    }

    //BRINGS THE GAME BACK TO THE TITLE SCREEN
    public void ResetToTitle()
    {
        //Actiate Title screen and Title camera of Intro scene
        titleMenu.SetActive(true);
        titleCam.SetActive(true);

        //Remove the loaded game level.
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        SceneManager.UnloadSceneAsync(1);
    }


    public void LockPlayer()
    {
        FPSController.Instance.canMove = true;
        FPSController.Instance.GetComponent<CharacterController>().enabled = false;
    }

    public void UnLockPlayer()
    {
        //Hide the cursor again
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        //Allow player to walk again
        FPSController.Instance.GetComponent<CharacterController>().enabled = true;
        FPSController.Instance.canMove = true;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

}
