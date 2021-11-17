using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayBoundary : MonoBehaviour
{
    [SerializeField] GameObject leaveGameMessage;
    [SerializeField] string levelName;
    [SerializeField] GameObject gameBooth;
    [SerializeField] GameObject hubBooth;
    WeaponEquip WE;
    //Scene theScene;
    //[SerializeField] int sceneIndex;

    private void Awake()
    {
        levelName = WeaponEquip.Instance.levelName;
        hubBooth = WeaponEquip.Instance.HubBooth();
        //WE = GetComponent<WeaponEquip>();
        //gameBooth = WE.gameBooth;
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

        hubBooth.SetActive(true);
        SceneManager.UnloadSceneAsync(levelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    }
}
