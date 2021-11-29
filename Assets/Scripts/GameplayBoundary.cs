using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayBoundary : MonoBehaviour
{
    [SerializeField] string levelName;
  
    public GameObject[] gamesArray;
    [SerializeField] GameObject player;
    [SerializeField] GameCardManager cardManager;
    WeaponEquip WE;

    //public bool ShootingCard, MeleeCard, ThrowingCard;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        WE = player.GetComponent<WeaponEquip>();
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

            //Cursor.visible = true;
            //Cursor.lockState = CursorLockMode.None;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        //if have card, restart game
        if (other.CompareTag("Player"))
        {
            for (int i = 0; i < gamesArray.Length; i++)
            {
                //Get layer name of the game
                int layerNumber = gamesArray[i].layer;
                string layerName = LayerMask.LayerToName(layerNumber);

                //If layer name matches level name, activate the game
                if (layerName == levelName)
                {
                    gamesArray[i].SetActive(false);

                    //Restart targets at starting position
                    //RestartTargetsPosition();
                }
            }
        }
    }

    public void RestartTargetsPosition()
    {
        for (int i = 0; i < cardManager.targetsArray.Length; i++)
        {
            Debug.Log("Targets: " + cardManager.targetsArray.Length);
            Vector3 startPos = cardManager.targetsArray[i].GetComponent<Target>().startPos.position;
            Debug.Log("Target Position: " + startPos);
            cardManager.targetsArray[i].transform.position = startPos;
            cardManager.targetsArray[i].transform.rotation = Quaternion.Euler(90f, 0f, 0f);
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
