using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameCardManager : MonoBehaviour
{
    public static GameCardManager Instance;

    GameObject player;
    [SerializeField] WeaponEquip WE;
    [SerializeField] Menu menu;

    Vector3 pos;
    public float moveSpeed = 0.1f;

    [Space(15)]
    public GameObject[] targetsArray;
    public List<GameObject> targetsList;
    [SerializeField] GameObject cardDisplay;
    [SerializeField] GameObject cardWon;


    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        WE = player.GetComponent<WeaponEquip>();
        menu = WE.menu;
        pos = transform.position;
    }

    private void Update()
    {
        //Better optimized if this method is called from somewhere else...
        if (targetsList.Count == targetsArray.Length)
        {
            DisplayGameCard();
        }

        //Activate boss
        //if (player.GetComponent<FPSController>().cardCount == 3)
        //{
        //    player.GetComponent<FPSController>().tent.SetActive(false);
        //    player.GetComponent<FPSController>().boss.SetActive(true);
        //}
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
                StartCoroutine(DisplayTransition());
            }
        }

        return cardDisplay;
    }

    // TRANSITION FROM CARD DISPLAY SCREEN BACK TO GAME DISPLAY
    IEnumerator DisplayTransition()
    {
        yield return new WaitForSeconds(1);

        if (cardDisplay.GetComponentInChildren<Image>().enabled == true)
        {
            cardDisplay.GetComponentInChildren<Image>().enabled = false;
            cardWon.GetComponent<Image>().enabled = true;
        }
        
    }
}
