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
using System.Data.SqlTypes;

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

    [Header("AUDIO")]
    //public AudioMixer audioMixer;
    public AudioSource introAudio;
    //public AudioSource pauseSound;
    //public string exposedParam;

    [Header("OBJECTS")]
    public GameObject titleScreen;
    public GameObject titleCamera;
    //public GameObject player;

    [Header("MENUS")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject settingsMenu;
    //public GameObject skullCountUI;
    //public TextMeshProUGUI skullCountText;
    GameObject firstButton;
    
    [Header("TAROT UI")]
    public GameObject[] gameCardSlots;
    public GameObject gameCardBG;
    public GameObject gameCard;
    public Sprite cardImage;
    public Sprite bgImage;
    public int BGCount;
    [SerializeField] bool settingsOn;

    [Space(10)]
    public string[] controllerArray = null;

    [Header("BRIGHTNESS")]
    public Light sceneLight;
    //public float brightnessValue = 0.7f;

    public bool usingJoystick;

    [Header("LEVEL LOAD")]
    public GameObject loadScreen;
    public Slider loadBar;
    public TextMeshProUGUI loadPercentText;
    //[SerializeField] private string levelName;

    [Header("CONTROLLER PROMPT")]
    public GameObject controllerPrompt;
    public GameObject keyboardPrompt;
    /*[HideInInspector] */public int counter = -1; //Used to handle pause.

    private void Awake()
    {
        Instance = this; 

        GM = GameManager.Instance;
        GM.OnStateChange += HandleOnStateChange;

        controllerArray = Input.GetJoystickNames();
    }

    public void HandleOnStateChange()
    {
        //Debug.Log("OnStateChange!");
    }

    private void Update()
    {
        if (controllerArray == null) { controllerArray = Input.GetJoystickNames(); }
        whackemGM = FindObjectOfType<WhackEmGameManager>();
        skillshotGM = FindObjectOfType<SkillShotGameManager>();
        WE = FindObjectOfType<WeaponEquip>();

        //Set Player
        //player = GameObject.FindGameObjectWithTag("Player");

        if (settingsMenu.activeInHierarchy == true)
        {
            settingsOn = true;
        }
        else
        {
            settingsOn = false;
        }

        //DETECT CONTROLLER
        if (controllerArray == null)
        {
            usingJoystick = false;
        }
        else if (controllerArray.Length > 0 /*&& controllerArray[0] != ""*/)
        {
            usingJoystick = true;
            
            //Hide cursor
            Cursor.lockState = CursorLockMode.Confined;
            Cursor.visible = false;
        }

        if (Input.GetButtonDown("Menu") && counter == 0 && !CasketBasketsGameManager.Instance.gameOn)
        {
            PauseGame();
        }
        else if (Input.GetButtonDown("Menu") && counter == 1 && !settingsOn)
        {
            UnpauseGame();
            Cursor.visible = false;
        }

        //Get the correct tarot card image from the carnival game manager scripts. Uses the closest weapon method to get the game name.
        if (WE != null)
        {
            if (WE.gameName == "MeleeGame")
            {
                cardImage = whackemGM.cardImage;
                bgImage = whackemGM.BGImage;
                return;
            }
            else if (WE.gameName == "ShootingGame")
            {
                cardImage = skillshotGM.cardImage;
                bgImage = skillshotGM.BGImage;
                return;
            }
        }
    }

    //
    //START THE GAME
    public void StartGame()
    {
        //Start game scene
        GM.SetGameState(GameState.LEVEL_ONE);

        StartCoroutine(LoadLevel());

        //Debug.Log(GM.GameState);
    }

    public IEnumerator LoadLevel()
    {
        counter = 0;
        titleScreen.SetActive(false);

        loadScreen.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneBuildIndex: 1, LoadSceneMode.Additive);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            loadBar.value = progress;
            loadPercentText.text = String.Format((decimal)progress * 100 + "%", 2);

            yield return null;
        }

        loadScreen.SetActive(false);
        titleCamera.SetActive(false);

        //sceneLight = GameObject.FindGameObjectWithTag("SceneLight").GetComponent<Light>();
        //sceneLight = GameObject.Find("MoonLight").GetComponent<Light>();
    }

    //public void AddScene(string name)
    //{
    //    //Clear button selected
    //    EventSystem.current.SetSelectedGameObject(null);

    //    SceneManager.LoadScene(name, LoadSceneMode.Additive);

    //    //Hide mouse when new level loaded
    //    Cursor.lockState = CursorLockMode.Locked;
    //    Cursor.visible = false;
    //}

    //
    //PAUSE GAME
    public void PauseGame()
    {
        //Show mouse
        Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;

        counter = 1;
        pauseMenu.SetActive(true);
        Time.timeScale = 0;

        ClearButton();
    }

    //
    //UNPAUSE GAME
    public void UnpauseGame()
    {
        //Hide mouse
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = false;

        counter = 0;
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
    }

    //
    //EXIT THE SETTINGS MENU AND RETURN TO PREVIOUS MENU
    public void ExitSettingsMenu()
    {
        //Called from Settings menu Back button:
        
        if (counter == 1 )
        {
            pauseMenu.SetActive(true);
            settingsMenu.SetActive(false);
        }
        else
        {
            titleScreen.SetActive(true);
            settingsMenu.SetActive(false);
        }
    }

    //Get the main light and set the intensity to the value in Menu
    public IEnumerator GetSceneLight()
    {
        yield return new WaitForSeconds(1);

        sceneLight = GameObject.FindGameObjectWithTag("SceneLight").GetComponent<Light>();
        //sceneLight.intensity = brightnessValue;

        //Debug.Log("Scene Light:" + sceneLight);
        //Debug.Log("Brightness: " + brightnessValue);
    }

    public void ClearButton()
    {
        //Clear button selected
        EventSystem.current.SetSelectedGameObject(null);
        //Set selected button
        firstButton = GameObject.FindGameObjectWithTag("FirstButton");
        EventSystem.current.SetSelectedGameObject(firstButton);
    }

    public void StopIntroMusic()
    {
        introAudio.enabled = false;
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
        //StartCoroutine(DelayQuit());

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

    //Displays the card that was just won in the first most available spot, left to right.
    public void DisplayWeaponCard()
    {
        for (int i = 0; i < gameCardSlots.Length; i++)
        {
            //Set the game card UI
            gameCard = gameCardSlots[i].GetComponentInChildren<GameCard>().gameObject; //Get the gamecard Gameobject to be displayed at the bottom
            gameCardBG = gameCardSlots[i].GetComponentInChildren<WeaponCardBackground>().gameObject;


            //If the first cardslot space is not enabled then enable it
            if (gameCard.GetComponent<Image>().sprite == null)
            {
                //
                //ug.Log("Sprite is null");

                //Turn on card image
                gameCard.GetComponent<Image>().enabled = true; //enable the image component
                gameCard.GetComponent<Image>().sprite = cardImage; //set the image sprite to the game card that was won

                //Turn on cardBG image
                gameCardBG.GetComponent<Image>().enabled = true; //enables the background image to show that this weapon is equipped
                gameCardBG.GetComponent<Image>().sprite = bgImage;

                //Turn off previous BG
                if (i > 0)
                {
                    if (gameCardSlots[i - 1].GetComponentInChildren<WeaponCardBackground>().GetComponent<Image>().enabled == true)
                    {
                        gameCardSlots[i - 1].GetComponentInChildren<WeaponCardBackground>().GetComponent<Image>().enabled = false;
                    }
                }
                WE.weaponCardBG.Add(gameCardBG); //Add background to the list in WeaponEquip so it can be turned on/off when scrolling through weapons
                
                break; //break out because we've done what we want (no need to continue the iteration)
            }

        }   
    }

}
