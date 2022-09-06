using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class Reset_To_Title : MonoBehaviour
{
    public static Reset_To_Title Instance;

    public GameObject titleMenu;
    public GameObject titleCam;
    public Menu menu;
    [SerializeField] HudManager hudManager;

    private void Awake()
    {
        Instance = this;
    }
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
        Time.timeScale = 1;
        menu.counter = -1; //Reset counter for pause screen
        menu.controllerArray = null; //Clear the array

        ShowCursor(); //Allows the cursor to be used on the Title menu
        ResetAudioPrefs(); //Sets the audio back to original levels
        ResetToTitle(); //Turns on the Title screen and camera of the Intro scene, and unloads the game scene.
    }

    public void ClearButton()
    {
        //Clear button selected
        EventSystem.current.SetSelectedGameObject(null);
        //Set selected button
        GameObject firstButton = GameObject.FindGameObjectWithTag("FirstButton");
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    //RESETS THE AUDIO TO ORIGINAL LEVELS
    public void ResetAudioPrefs()
    {
        //Reset the audio levels of the mixer
        hudManager.myMixer.SetFloat("MusicVolume", hudManager.musicVolume);
        hudManager.myMixer.SetFloat("SFXVolume", hudManager.sfxVolume);
        hudManager.myMixer.SetFloat("PlayerVolume", hudManager.playerVolume);
        //Stop the music from the game scene and start the title screen music.
        menu.StopSceneMusic();
        menu.PlayTitleMusic(); //Play the Title audio
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
