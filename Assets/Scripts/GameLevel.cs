using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameLevel : MonoBehaviour
{
    public static GameLevel Instance;

    [SerializeField] int buildIndex; //get the build index to be able to unload scene
    public GameObject[] targetArray;
    public List<GameObject> targetList;
    [SerializeField] GameObject gameBooth;
    //WeaponEquip WE;
    //Menu menu;
    //Gun gun;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        //menu = FindObjectOfType<Menu>();
        //gun = FindObjectOfType<Gun>();
        //WE = FindObjectOfType<WeaponEquip>();

        targetArray = GameObject.FindGameObjectsWithTag("MovingTarget");
        targetList.AddRange(targetArray);
        buildIndex = SceneManager.GetActiveScene().buildIndex;
    }

    public void Return(string levelName)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        
        //Remove game booth scene from hierarchy
        SceneManager.UnloadSceneAsync(levelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        //Unhide the hub level booth
        gameBooth.SetActive(true);
    }

    public void LoadGame()
    {
        //Hide the hub level booth
        gameBooth.SetActive(false);
    }
}
