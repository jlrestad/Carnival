using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCardManager : MonoBehaviour
{
    //**                                                    **//
    //** ACTIVATES THE BOSS WHEN THE 3 CARDS HAVE BEEN WON  **//
    //**                                                    **//

    public static GameCardManager Instance;

    public GameObject player;
    public WeaponEquip WE;
    Menu menu;

    Vector3 pos;

    [Header("SKILLSHOT GAME")]
    [SerializeField] SkillShotGameManager skillshotGM;

    [Header("SMASH GAME")]
    [SerializeField] WhackEmGameManager whackemGM;
    public GameObject[] critterArray;
    public List<GameObject> critterList;

    [Header("TAROT CARDS")]
    [SerializeField] GameObject cardDisplay;
    [SerializeField] GameObject cardWon;
    bool gameWon;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
            //player = GameObject.FindGameObjectWithTag("Player");
            //WE = player.GetComponent<WeaponEquip>();
            //menu = FindObjectOfType<Menu>(); ;
        //pos = transform.position;
    }

    private void Update()
    {

        //if (WE.gameName == "MeleeGame")
        //{
        //    //Debug.Log("level name is melee game");
        //    whackemGM = FindObjectOfType<WhackEmGameManager>();
        //    critterArray = whackemGM.critters;

        //    Debug.Log("Carnival Smash won: " + gameWon);
        //}
        //else if (WE.levelName.Equals("ShootingGame"))
        //{
        //    skillshotGM = FindObjectOfType<SkillShotGameManager>();
        //    gameWon = skillshotGM.gameWon;
        //    Debug.Log("Skill Shot won: " + gameWon);
        //}

        //Activate boss
        //if (player.GetComponent<FPSController>().cardCount == 3)
        //{
        //    player.GetComponent<FPSController>().tent.SetActive(false);
        //    player.GetComponent<FPSController>().boss.SetActive(true);
        //}
    }

    //// RETURNS THE GAME OBJECT THAT HOLDS THE CARD
    //public GameObject DisplayGameCard()
    //{
    //    for (int i = 0; i < WE.gameCards.Length; i++)
    //    {
    //        if (WE.gameCards[i].name == WE.gameName)
    //        {
    //            Debug.Log("TEST is working");

    //            //Display the card that was won
    //            WE.gameCards[i].SetActive(true);
    //            //Set the gameObject to be displayed
    //            cardDisplay = WE.gameCards[i];
    //            //Set the gameObject to display card
    //            cardWon = cardDisplay.GetComponent<GameCard>().gameObject;

    //            //Transition from card display back to game display
    //            StartCoroutine(DisplayCardWon());
    //        }
    //    }

    //    return cardWon;
    //}

    //// TRANSITION FROM CARD DISPLAY SCREEN BACK TO GAME DISPLAY
    //public IEnumerator DisplayCardWon()
    //{
    //    yield return new WaitForSeconds(1);

    //    cardDisplay.GetComponent<Image>().enabled = false;
    //    cardWon.GetComponent<Image>().enabled = true;
    //}

}
