using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressManager : MonoBehaviour
{
    //This class won't make any sense if you didn't write it, just try not to think about it
    //=========================|FIELDS|=========================
    public GameObject SS_game;
    public GameObject CS_game;
    private SkillShotGameManager SS_GM;
    private WhackEmGameManager CS_GM;
    public GameObject introText; //reference to the text that needs to be set inactive for the first dialog to run
    [Header("AUTO")]
    public GameObject FirstDialog; //the first dialog triggers after the intro has played
    public GameObject SecondDialog; //the second dialog triggers after clearing 1 game
    public GameObject ThirdDialog; //the third dialog triggers after clearing both games
    public GameObject CinematicArea; //the cinematic area triggers after clearing both games
    [SerializeField] int dialogNumber = 0;
    //=========================|METHODS|=========================
    // Start is called before the first frame update
    void Start()
    {
        SS_GM = SS_game.GetComponent<SkillShotGameManager>();
        CS_GM = CS_game.GetComponent<WhackEmGameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckProgress();
    }

    void CheckProgress()
    {
        if(dialogNumber == 0 && introText.activeInHierarchy == false) //if the intro has been disabled and we haven't seen the first dialog yet
        {
            dialogNumber = 1; //mark that we've activated the first dialog point
            FirstDialog.SetActive(true); //activate the first dialog point
        }
        if(dialogNumber == 1 && (SS_GM.gameWon || CS_GM.gameWon)) //when one game has been won
        {
            dialogNumber = 2; //mark that we've activated the second dialog point
            SecondDialog.SetActive(true); //activate the second dialog point
        }
        if(dialogNumber == 2 && (SS_GM.gameWon && CS_GM.gameWon)) //when both games have been won
        {
            dialogNumber = 3; //mark that we've activated the third dialog point
            ThirdDialog.SetActive(true);
            CinematicArea.SetActive(true);
        }
    }
}
