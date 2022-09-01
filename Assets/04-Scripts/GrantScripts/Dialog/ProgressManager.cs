using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class ProgressManager : MonoBehaviour
{
    /* 
     * This script handles the level progression for the PAX version of the Funfair game.
     * This includes enabling/disabling dialog, opening/closing gates, playing music, and triggering cutscenes
     * If looking for the script that dismisses the intro text, look for cinematicManager
     * Grant Hargraves 8/22
     */
    //=========================|FIELDS|=========================
    [Header("PLUG-INS")]
    [Tooltip("The gameobject that holds the game manager for SkillShot")]
    public GameObject SS_game;
    [Tooltip("The gameobject that holds the game manager for CarnivalSmash")]
    public GameObject CS_game;
    [Tooltip("The gameobject that holds the game manager for CasketBaskets")]
    public GameObject CB_game;
    [Space(10)]
    [Tooltip("Reference to the text that needs to be set inactive for the first dialog to run")]
    public GameObject introText;
    [Tooltip("The first dialog triggers after the intro has played. This is located where the player starts.")]
    public GameObject FirstDialog; //the first dialog triggers after the intro has played
    [Tooltip("The second dialog triggers after clearing 1 game. This collider covers the whole carnival.")]
    public GameObject SecondDialog; //the second dialog triggers after clearing 1 game
    [Tooltip("The third dialog triggers after clearing 2 games. This collider covers the whole carnival.")]
    public GameObject ThirdDialog; //the third dialog triggers after clearing 2 games
    [Tooltip("The fourth dialog triggers after clearing 3 games. This collider covers the whole carnival.")]
    public GameObject FourthDialog; //the third dialog triggers after clearing 3 games
    [Tooltip("The cinematic area triggers after clearing all games. When collided with this area starts the ending sequence.")]
    public GameObject CinematicArea; //the cinematic area triggers after clearing all games
    [Space(10)]
    [Tooltip("The ticket string that must be collected for the front gates of the carnival to open.")]
    public GameObject TicketString;
    [Tooltip("The audio mixer this game is using so that it can be manipulated.")]
    [SerializeField] AudioMixer myMixer;
    [Tooltip("The animator for the gate at the front of the carnival.")]
    public Animator frontGate;
    [Tooltip("The animator for the gate leading to the boss area.")]
    public Animator finalGate;
    [Tooltip("The dialog area just inside the carnival. When inactive the gate will close behind the player.")]
    public GameObject carnivalEntryArea;
    [Tooltip("The ending screen used for the ending of the game at PAX.")]
    [SerializeField] GameObject PAXEnding;
    [Space(10)]
    public AudioSource playerBGM;
    public AudioClip forestMusic;
    public AudioClip carnivalMusic;
    [Space(10)]
    public GameObject GunScreen, MalletScreen, SkullScreen;
    public bool ScreensUp = false;

    [Header("INTERNAL/DEBUG")]
    [SerializeField] int dialogNumber = 0;
    private CasketBasketsGameManager CB_GM;
    private SkillShotGameManager SS_GM;
    private CarnivalSmashGameManager CS_GM;
    [Tooltip("Tracks whether the player has collected the tickets out in front of the gate.")]
    [SerializeField] bool ticketsCollected = false;
    [Tooltip("Keeps track of which gates are open and shut instead of having to use all kinds of bools.")]
    [SerializeField] int gateState = 0;
    [SerializeField] bool SSComplete = false;
    [SerializeField] bool CSComplete = false;
    [SerializeField] bool CBComplete = false;
    [SerializeField] int CompleteCounter = 0;
    public Menu menu;
    //=========================|METHODS|=========================
    // Start is called before the first frame update
    void Start()
    {
        SS_GM = SS_game.GetComponent<SkillShotGameManager>();
        CS_GM = CS_game.GetComponent<CarnivalSmashGameManager>();
        CB_GM = CB_game.GetComponent<CasketBasketsGameManager>();

        menu = GameObject.FindObjectOfType<Menu>();
    }

    // Update is called once per frame
    void Update()
    {
        //-----Below allows the advancement text to only appear after the pickups screens are dismissed-----
        if (GunScreen.activeInHierarchy || MalletScreen.activeInHierarchy || SkullScreen.activeInHierarchy)
        {
            ScreensUp = true;
        }
        else
        {
            ScreensUp = false;
        }
        CheckProgress();
        
    }

    void CheckProgress()
    {
        //-----WIN CONDITIONS-----
        if (SS_GM.gameWon == true && SSComplete == false)
        {
            SSComplete = true; //turn on the completion bool if we've won the game for the first time.
            CompleteCounter++; //add one to the completion counter
        }
        if (CS_GM.gameWon == true && CSComplete == false)
        {
            CSComplete = true;
            CompleteCounter++;
        }
        if (CB_GM.gameWon == true && CBComplete == false)
        {
            CBComplete = true;
            CompleteCounter++;
        }
        
        //-----LEVEL ACCESS-----
        if (gateState == 0 && TicketString.activeInHierarchy == false) //if the ticket string was collected and the front gate has not opened yet...
        {
            frontGate.SetBool("openGate", true); //open the gate
            frontGate.SetBool("closeGate", false);
            gateState = 1; //mark that the front gate has opened
        }
        if(gateState == 1 && carnivalEntryArea.activeInHierarchy == false) //if the carnival has been entered and the dialog area has been disabled...
        {
            frontGate.SetBool("closeGate", true); //close the gate
            frontGate.SetBool("openGate", false);
            playerBGM.clip = carnivalMusic;
            playerBGM.Play();
            gateState = 2; //mark that the front gate has opened and closed
        }
        //-----DIALOG PROGRESSION-----
        if (dialogNumber == 0 && introText.activeInHierarchy == false) //if the intro has been disabled and we haven't seen the first dialog yet
        {
            dialogNumber = 1; //mark that we've activated the first dialog point
            FirstDialog.SetActive(true); //activate the first dialog point
        }
        if(dialogNumber == 1 && CompleteCounter == 1 && !ScreensUp) //when one game has been won and the pickup window is dismissed
        {
            dialogNumber = 2; //mark that we've activated the second dialog point
            SecondDialog.SetActive(true); //activate the second dialog point
        }
        if(dialogNumber == 2 && CompleteCounter == 2 && !ScreensUp) //when two games have been won and the pickup window is dismissed
        {
            dialogNumber = 3; //mark that we've activated the third dialog point
            ThirdDialog.SetActive(true); //play dialog
        }
        if(dialogNumber == 3 && CompleteCounter == 3 && !ScreensUp) //when all three games have been won and the pickup window is dismissed
        {
            dialogNumber = 4; //mark that we've activated the fourth dialog point
            FourthDialog.SetActive(true); //play dialog
            finalGate.SetBool("openGate", true); //open the final gate
            CinematicArea.SetActive(true);
        }
        //-----ENDING-----
        if (dialogNumber == 4 && CinematicArea.activeInHierarchy == false)
        {
            playEnding();
        }
    }

    void playEnding()
    {
        menu.ResetTarotCards();
        
        //keep player from moving
        FPSController.Instance.canMove = false;
        //mute all other audio groups
        myMixer.SetFloat("MusicVolume", -80); //set all the sound mixers to muted except for the GameOver group
        myMixer.SetFloat("SFXVolume", -80);
        myMixer.SetFloat("PlayerVolume", -80);
        PAXEnding.SetActive(true); //allow the ending scene to play
        Cursor.visible = true; //make the cursor visible
        Cursor.lockState = CursorLockMode.None;
    }
}
