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
    [SerializeField] GameObject gameBooth, player;
    WeaponEquip WE;
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
        WE = FindObjectOfType<WeaponEquip>();

        targetArray = GameObject.FindGameObjectsWithTag("MovingTarget");
        targetList.AddRange(targetArray);
        buildIndex = SceneManager.GetActiveScene().buildIndex;


        player = Menu.Instance.player;
    }

    public void Return(string levelName)
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.UnloadSceneAsync(levelName, UnloadSceneOptions.UnloadAllEmbeddedSceneObjects);

        gameBooth.SetActive(true);
        player.SetActive(true);
    }

    public void LoadGame()
    {
        //player.SetActive(false);
        gameBooth.SetActive(false);
    }
}
