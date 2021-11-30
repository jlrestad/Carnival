using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameplayBoundary : MonoBehaviour
{
    [SerializeField] string levelName;
  
    [SerializeField] GameObject player;
    [SerializeField] GameCardManager cardManager;

    [SerializeField] GameObject game;
    [SerializeField] GameObject targetPrefab;
    [SerializeField] GameObject targetSpawn;
    WeaponEquip WE;

    //public bool ShootingCard, MeleeCard, ThrowingCard;

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
            game = Instantiate(targetPrefab, targetSpawn.transform);
            //game = Instantiate(targetPrefab, targetSpawn.transform.position, Quaternion.Euler(0f, 90f, 0f));

            game.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            game.SetActive(false);

            //RestartTargetsPosition();
        }
    }

    public void RestartTargetsPosition()
    {

        //for (int i = 0; i < cardManager.targetsArray.Length; i++)
        //{
        //    Debug.Log("Targets: " + cardManager.targetsArray.Length);
        //    Vector3 startPos = cardManager.targetsArray[i].GetComponent<Target>().startPos.position;

        //    Debug.Log("Target Position: " + startPos);
        //    cardManager.targetsArray[i].transform.position = startPos;
        //    cardManager.targetsArray[i].transform.rotation = Quaternion.Euler(90f, 0f, 0f);
        //}
    }
}
