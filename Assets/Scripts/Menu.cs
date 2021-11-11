using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.TerrainAPI;

#if UNITY_EDITOR
using UnityEditor.PackageManager;
#endif

//#if ENABLE_INPUT_SYSTEM
//// New input system backends are enabled.
//#endif

//#if ENABLE_LEGACY_INPUT_MANAGER
//    // Old input backends are enabled.
//#endif

public class Menu : MonoBehaviour
{
    public static Menu Instance;

    GameManager GM;

    [Header("AUDIO")]
    public AudioMixer audioMixer;
    public AudioSource introAudio;
    public AudioSource pauseSound;
    public string exposedParam;

    [Header("OBJECTS")]
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject titleCamera;
    public GameObject levelOne;
    public GameObject player;

    [Header("MENUS")]
    public GameObject pauseMenu;
    [SerializeField] GameObject firstButton;
    bool isPaused;

    [Header("LEVEL LOAD")]
    [SerializeField] private string levelName;
    //[SerializeField] float delayTime = 3f;

    int counter = -1; //Used to handle pause.
    public GameObject ePrompt;

    private void Awake()
    {
        Instance = this; 

        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    public void HandleOnStateChange()
    {
        Debug.Log("OnStateChange!");
    }

    private void Update()
    {
        levelOne = GameObject.FindGameObjectWithTag("LevelObjects");
        player = GameObject.FindGameObjectWithTag("Player");

        if (Input.GetButtonDown("Menu") && counter == 0)
        {
            PauseGame();
        }
        else if (Input.GetButtonDown("Menu") && counter == 1)
        {
            pauseSound.Play();

            UnpauseGame();
        }
    }

    public void StartGame()
    {
        //Start game scene
        GM.SetGameState(GameState.LEVEL_ONE);

        //Invoke("LoadLevel", delayTime);
        LoadLevel();

        Debug.Log(GM.GameState);
    }

    public void PauseGame()
    {
        isPaused = true;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        counter = 1;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;

        //Clear
        EventSystem.current.SetSelectedGameObject(null);
        //Set
        firstButton = GameObject.FindGameObjectWithTag("FirstButton");
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void UnpauseGame()
    {
        isPaused = false;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        counter = 0;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    //public void DelayQuit() 
    //{
    //    #if UNITY_EDITOR
    //            UnityEditor.EditorApplication.isPlaying = false;
    //    #endif
    //    Invoke("Quit", 2f);
    //}

    public void Quit()
    {
        StartCoroutine("DelayQuit");
    }

    IEnumerator DelayQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        yield return new WaitForEndOfFrame();

        Application.Quit();
    }

    // Store player preferences
    //private void OnApplicationQuit()
    //{
    //    PlayerPrefs.SetString("QuitTime", "The application last closed at: " + System.DateTime.Now);
    //}

    public void LoadLevel()
    {
        counter = 0;
        titleScreen.SetActive(false);
        titleCamera.SetActive(false);

        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        
        //Clear level name on start
        levelName = "";

        introAudio.volume = 1;
    }

    public void ChangeLevel(string name)
    {
        //Clear button selected
        EventSystem.current.SetSelectedGameObject(null);

        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        //StartCoroutine(DelayDeactivation());

        //Find first button in scene
        //firstButton = GameObject.FindGameObjectWithTag("FirstButton");
        //Set button
        //EventSystem.current.SetSelectedGameObject(firstButton);
    }

    IEnumerator DelayDeactivation()
    {
        yield return new WaitForSeconds(0.5f);
        levelOne.SetActive(false);
        player.SetActive(false);
    }

    public void AudioFade()
    {
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, exposedParam, 2.5f, 0));
    }
}
