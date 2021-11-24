using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayBoundary : MonoBehaviour
{
    //[SerializeField] GameObject leaveGameMessage;
    [SerializeField] string levelName;
    //[SerializeField] GameObject gameBooth;
    //[SerializeField] GameObject hubBooth;
    public GameObject[] gamesArray;
    [SerializeField] GameObject player;
    //Scene theScene;
    //[SerializeField] int sceneIndex;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        //gameBooth = WE.gameBooth;
        //theScene = SceneManager.GetSceneByName(levelName);
    }

    private void Update()
    {
        levelName = player.GetComponent<WeaponEquip>().levelName;
    }

    //private void Update()
    //{
    //    sceneIndex = theScene.buildIndex;
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Start the game
            for (int i = 0; i < gamesArray.Length; i++)
            {
                //Get layer name of the game
                int layerNumber = gamesArray[i].layer;
                string layerName = LayerMask.LayerToName(layerNumber);

                //If layer name matches level name, activate the game
                if (layerName == levelName)
                {
                    gamesArray[i].SetActive(true);
                }
            }

            // Old code when in loaded scene
            //Activate game level menu
            //leaveGameMessage.SetActive(true);

            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
        }
    }
    
    //public void LeaveGame()
    //{
    //    Cursor.visible = false;
    //    Cursor.lockState = CursorLockMode.Locked;

    //    hubBooth.SetActive(true);
    //    SceneManager.UnloadSceneAsync(levelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);
    //}
}
