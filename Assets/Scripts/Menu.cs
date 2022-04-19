using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Experimental.TerrainAPI;
using System;
using System.Linq;
using TMPro;
using UnityEngine.UI;

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
    WhackEmGameManager whackemGM;
    SkillShotGameManager skillshotGM;
    WeaponEquip WE;

    //[Header("AUDIO")]
    //public AudioMixer audioMixer;
    //public AudioSource introAudio;
    //public AudioSource pauseSound;
    //public string exposedParam;

    [Header("OBJECTS")]
    [SerializeField] private GameObject titleScreen;
    [SerializeField] private GameObject titleCamera;
    public GameObject player;

    [Header("MENUS")]
    public GameObject pauseMenu;
    public GameObject skullCountUI;
    public TextMeshProUGUI skullCountText;
    GameObject firstButton;
    
    [Header("TAROT UI")]
    public GameObject[] gameCardSlots;
    public GameObject gameCardBG;
    public GameObject gameCard;
    public Sprite cardImage;

    [Space(10)]
    public string[] controllerArray = null;

    public bool usingJoystick;

    [Header("LEVEL LOAD")]
    //[SerializeField] private string levelName;

    int counter = -1; //Used to handle pause.
    public GameObject controllerPrompt, keyboardPrompt;

    private void Awake()
    {
        Instance = this; 

        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;

        controllerArray = Input.GetJoystickNames();



    }

    public void HandleOnStateChange()
    {
        Debug.Log("OnStateChange!");
    }

    private void Update()
    {
        controllerArray = Input.GetJoystickNames();
        whackemGM = FindObjectOfType<WhackEmGameManager>();
        skillshotGM = FindObjectOfType<SkillShotGameManager>();
        WE = FindObjectOfType<WeaponEquip>();

        player = GameObject.FindGameObjectWithTag("Player");

        //Detect if joystick is used.
        if (controllerArray == null)
        {
            usingJoystick = false;
        }
        else if (controllerArray.Length > 0 /*&& controllerArray[0] != ""*/)
        {
            usingJoystick = true;
        }

        if (Input.GetButtonDown("Menu") && counter == 0)
        {
            PauseGame();
        }
        else if (Input.GetButtonDown("Menu") && counter == 1)
        {
            //pauseSound.Play();

            UnpauseGame();
        }

        //Get the correct tarot card image from the carnival game manager scripts.
        if (WE != null)
        {
            if (WE.gameName == "MeleeGame")
            {
                cardImage = whackemGM.cardImage;
                return;
            }
            else if (WE.gameName == "ShootingGame")
            {
                cardImage = skillshotGM.cardImage;
                return;
            }
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
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        counter = 1;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;

        //Clear button selected
        EventSystem.current.SetSelectedGameObject(null);
        //Set selected button
        firstButton = GameObject.FindGameObjectWithTag("FirstButton");
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void UnpauseGame()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.lockState = CursorLockMode.Confined;
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
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        StartCoroutine(DelayQuit());

        Application.Quit();
    }

    IEnumerator DelayQuit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
        yield return new WaitForSeconds(0.5f);

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

        SceneManager.LoadScene(sceneBuildIndex:1, LoadSceneMode.Additive);
        
        //Clear level name on start
        //levelName = "";

        //introAudio.volume = 1;
    }

    public void ChangeLevel(string name)
    {
        //Clear button selected
        EventSystem.current.SetSelectedGameObject(null);

        SceneManager.LoadScene(name, LoadSceneMode.Additive);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;

        //StartCoroutine(DelayDeactivation());

        //Find first button in scene
        //firstButton = GameObject.FindGameObjectWithTag("FirstButton");
        //Set button
        //EventSystem.current.SetSelectedGameObject(firstButton);
    }

    //* fix so that cards will occupy the next available space (bool?)
    //Displays the card that was just won in the first most available spot, left to right.
    public void DisplayWeaponCard()
    {
        for (int i = 0; i < gameCardSlots.Length; i++)
        {
            //Set the game card UI
            gameCard = gameCardSlots[i].GetComponentInChildren<GameCard>().gameObject;
            gameCardBG = gameCardSlots[i].GetComponentInChildren<WeaponCardBackground>().gameObject;

            //If the first space is not enabled then enable it
            if (gameCard.GetComponent<Image>().sprite == null )
            {
                gameCard.GetComponent<Image>().enabled = true; //enables the image component
                gameCard.GetComponent<Image>().sprite = cardImage; //sets the image sprite to the game card that was won
                gameCardBG.GetComponent<Image>().enabled = true; //enables the background image to show that this weapon is equipped

                WE.weaponCardBG.Add(gameCardBG); //Add background to the list in WeaponEquip so it can be turned on/off when scrolling through weapons

                break; //break out because we've got what we want
            }
        }   
    }
}
