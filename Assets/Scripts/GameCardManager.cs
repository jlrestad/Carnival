using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCardManager : MonoBehaviour
{
    public static GameCardManager Instance;

    GameObject player;
    [SerializeField] WeaponEquip WE;
    [SerializeField] WhackEmGameManager whackemGM;
    [SerializeField] Menu menu;

    Vector3 pos;

    [Space(15)]
    public GameObject[] critterArray;
    public List<GameObject> critterList;
    [SerializeField] GameObject cardDisplay;
    [SerializeField] GameObject cardWon;
    bool gameWon;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        WE = player.GetComponent<WeaponEquip>();
        whackemGM = GetComponent<WhackEmGameManager>();
        critterArray = GetComponent<WhackEmGameManager>().critters;
        menu = WE.menu;
        pos = transform.position;
    }

    private void Update()
    {
        gameWon = whackemGM.gameWon;

        //Better optimized if this is checked somewhere else...
        if (gameWon)
        {
            Debug.Log("SHOW CARD");
            DisplayGameCard();
        }

        //Activate boss
        if (player.GetComponent<FPSController>().cardCount == 3)
        {
            player.GetComponent<FPSController>().tent.SetActive(false);
            player.GetComponent<FPSController>().boss.SetActive(true);
        }
    }

    // RETURNS THE GAME OBJECT THAT HOLDS THE CARD
    public GameObject DisplayGameCard()
    {
        for (int i = 0; i < WE.gameCards.Length; i++)
        {
            if (WE.gameCards[i].name == WE.levelName)
            {
                //Display the card that was won
                WE.gameCards[i].SetActive(true);
                //Set the gameObject for method return
                cardDisplay = WE.gameCards[i];
                //Set the gameObject to display card
                cardWon = cardDisplay.GetComponentInChildren<GameCard>().gameObject;

                //Transition from card display back to game display
                StartCoroutine(DisplayCardWon());
            }
        }

        return cardDisplay;
    }

    // TRANSITION FROM CARD DISPLAY SCREEN BACK TO GAME DISPLAY
    public IEnumerator DisplayCardWon()
    {
        yield return new WaitForSeconds(1);

        if (cardDisplay.GetComponentInChildren<Image>().enabled == true)
        {
            cardDisplay.GetComponentInChildren<Image>().enabled = false;
            cardWon.GetComponent<Image>().enabled = true;
        }
        
    }
}
