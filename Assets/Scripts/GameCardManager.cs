using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    }

    public void DisplayGameCard()
    {
        for (int i = 0; i < WE.gameCards.Length; i++)
        {
            if (WE.gameCards[i].name == WE.levelName)
            {
                //Display the card that was won
                WE.gameCards[i].SetActive(true);

                GameCard[] gameCards = GameObject.FindObjectsOfType<GameCard>();

                //menu.DisplayGameCard(WE.gameCards[i]); //card not displaying under parented object
            }
        }
    }
}
