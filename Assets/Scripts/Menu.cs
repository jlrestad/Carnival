using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    GameManager GM;

    [Header("AUDIO")]
    public AudioMixer audioMixer;
    public AudioSource introAudio;
    public AudioSource pauseSound;
    public string exposedParam;

    [Header("OBJECTS")]
    public GameObject titleScreen;
    public GameObject titleCamera;

    [Header("MENUS")]
    public GameObject pauseMenu;

    [Header("LEVEL LOAD")]
    public string levelName;
    [SerializeField] float delayTime = 3f;

    int counter = 0; //Used to handle pause.
    public GameObject ePrompt;

    private void Awake()
    {
        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;
    }

    public void HandleOnStateChange()
    {
        Debug.Log("OnStateChange!");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && counter == 0)
        {
            PauseGame();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && counter == 1)
        {
            pauseSound.Play();

            UnpauseGame();
        }
    }

    public void StartGame()
    {
        // Start game scene
        GM.SetGameState(GameState.LEVEL_ONE);

        Invoke("LoadLevel", delayTime);
        //LoadLevel();

        Debug.Log(GM.GameState);
    }

    public void PauseGame()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        counter = 1;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
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
        titleScreen.SetActive(false);
        titleCamera.SetActive(false);

        SceneManager.LoadScene(levelName, LoadSceneMode.Additive);
        introAudio.volume = 1;
    }

    public void ChangeLevel(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
    }

    public void AudioFade()
    {
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, exposedParam, 3, 0));
    }
}
