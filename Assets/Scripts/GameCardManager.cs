using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCardManager : MonoBehaviour
{
    public static GameCardManager Instance;

    GameObject player;
    [SerializeField] WeaponEquip WE;

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
        pos = transform.position;
    }

    private void Update()
    {
        DisplayGameCard();
    }

    public void DisplayGameCard()
    {
        if (targetsList.Count == targetsArray.Length)
        {
            for (int i = 0; i < WE.gameCards.Length; i++)
            {
                if (WE.gameCards[i].name == WE.levelName)
                {
                    WE.gameCards[i].SetActive(true);
                }
            }
        }
    }
}
