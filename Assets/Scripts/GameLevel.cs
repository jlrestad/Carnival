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
    Menu menu;
    Gun gun;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        menu = FindObjectOfType<Menu>();
        gun = FindObjectOfType<Gun>();

        targetArray = GameObject.FindGameObjectsWithTag("MovingTarget");
        targetList.AddRange(targetArray);
        buildIndex = SceneManager.sceneCount;
    }

    private void Update()
    {
        //If player shoots target, return to main level
        //if (gun.hit.transform.name == "Target")
        //{
        //    Return();
        //}
    }

    public void Return()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(buildIndex, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        //TO-DO: Turn FPSplayer and Level Object back on
    }
}
