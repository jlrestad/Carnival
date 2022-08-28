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

        ShowCursor();
        ResetPlayerHud();
        ResetAudioPrefs();
        ResetToTitle();
    }

    //RESETS THE PLAYERHUD BACK TO THE STARTING PREFS
    public void ResetPlayerHud()
    {
        //Turn off Tarot card images (menu does not work, must use the Instance of Menu to clear the values)
        Menu.Instance.gameCard.GetComponent<Image>().enabled = false;
        Menu.Instance.gameCardBG.GetComponent<Image>().enabled = false;

        //Remove the Tarot card sprites.
        if (Menu.Instance.gameCard.GetComponent<Image>().sprite != null)
            Menu.Instance.inactiveWeapon = null;
        if (Menu.Instance.gameCardBG.GetComponent<Image>().sprite != null)
            Menu.Instance.activeWeapon = null;
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
