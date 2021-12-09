using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayBoundary : MonoBehaviour
{
    //[SerializeField] string levelName;
  
    [SerializeField] GameObject player;
    //[SerializeField] GameCardManager cardManager;

    GameObject game;
    [SerializeField] GameObject gamePrefab;
    [SerializeField] GameObject gameSpawn;
    WeaponEquip WE;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        WE = player.GetComponent<WeaponEquip>();
    }

    private void Update()
    {
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            game = Instantiate(gamePrefab, gameSpawn.transform);

            game.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            game.SetActive(false);
            //Destroy(game);
        }
    }
}
