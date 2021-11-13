using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayBoundary : MonoBehaviour
{
    [SerializeField] GameObject leaveGameMessage;
    WeaponEquip WE;
    string sceneName;

    private void Awake()
    {
        WE = FindObjectOfType<WeaponEquip>();
        sceneName = WE.levelName;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //activate game level menu
            leaveGameMessage.SetActive(true);

            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
    }
    
    public void LeaveGame()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(sceneName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

    }
}
