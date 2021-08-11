using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

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
    public float delayTime = 3f;

    int counter = 0;

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

        Debug.Log(GM.gameState);
    }

    public void PauseGame()
    {
        counter = 1;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    public void UnpauseGame()
    {
        counter = 0;
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void DelayQuit() 
    {
        Invoke("Quit", 2f);
    }

    public void Quit()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #endif
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

        SceneManager.LoadScene("Level01", LoadSceneMode.Additive);
        introAudio.volume = 1;
        
    }

    public void AudioFade()
    {
        StartCoroutine(FadeMixerGroup.StartFade(audioMixer, exposedParam, 3, 0));
    }

    //public void OnGUI()
    //{
    //    // Menu layout
    //    GUI.BeginGroup(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 800));
    //    GUI.Box(new Rect(0, 0, 100, 200),"Menu");

    //    if (GUI.Button(new Rect(10, 40, 80, 30), "Start"))
    //    {
    //        StartGame();
    //    }
    //    if (GUI.Button(new Rect(10, 160, 80, 30), "Quit"))
    //    {
    //        Quit();
    //    }
    //    GUI.EndGroup();
    //}
}
