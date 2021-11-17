using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayBoundary : MonoBehaviour
{
    [SerializeField] GameObject leaveGameMessage;
    WeaponEquip WE;
    [SerializeField] string levelName;
    //Scene theScene;
    //[SerializeField] int sceneIndex;

    private void Awake()
    {
        WE = FindObjectOfType<WeaponEquip>();
        levelName = WeaponEquip.Instance.levelName;
        //theScene = SceneManager.GetSceneByName(levelName);
    }

    //private void Update()
    //{
    //    sceneIndex = theScene.buildIndex;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Activate game level menu
            leaveGameMessage.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    public void LeaveGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        SceneManager.UnloadSceneAsync(levelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

    }
}
