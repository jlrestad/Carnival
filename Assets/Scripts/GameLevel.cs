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
    [SerializeField] string levelName;
    GameObject levelOne, player;
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
        buildIndex = SceneManager.GetActiveScene().buildIndex;

        levelName = "GameLevel"; //Find a way to set level name based on which game is played.

        levelOne = Menu.Instance.levelOne;
        player = Menu.Instance.player;
    }

    public void Return()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(levelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        levelOne.SetActive(true);
        player.SetActive(true);
    }
}
