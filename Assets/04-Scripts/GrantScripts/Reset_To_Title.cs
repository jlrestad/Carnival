using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Reset_To_Title : MonoBehaviour
{
    public GameObject titleMenu;
    public GameObject titleCam;

    private void Start()
    {
        Menu myMenu = GameObject.FindObjectOfType<Menu>();
        titleCam = myMenu.titleCamera;
        titleMenu = myMenu.titleScreen;

    }
    public void ResetToTitle()
    {
        
        titleMenu.SetActive(true);
        titleCam.SetActive(true);
        SceneManager.SetActiveScene(SceneManager.GetSceneAt(0));
        SceneManager.UnloadSceneAsync(1);
    }
}
