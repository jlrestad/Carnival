using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawnerManager : MonoBehaviour
{
    public GameObject player; //reference player to get position
    public GameObject spawnedCritter;
    private GameObject spawnDestroy;
    public List<GameObject> critterList;
    public float xPos;
    public float zPos;
    public float yPos;
    public int critterCount;
    public int stayUpTime = 3;

    void Start()
    {
        StartCoroutine(SpawnCritters());
        //DestroyCritters();
    }

    IEnumerator SpawnCritters()
    {
        //get player position and move parent to position
        player = GameObject.FindGameObjectWithTag("Player");
        //move parent game object to player position
        Debug.Log("Spawning");
        while(critterCount < 5)
        {
            xPos = Random.Range(player.transform.position.x - 5, player.transform.position.x + 5);
            yPos = player.transform.position.y - 2;
            zPos = Random.Range(player.transform.position.z - 5, player.transform.position.z + 5);

            spawnDestroy = Instantiate(spawnedCritter, new Vector3(xPos, 10, zPos), Quaternion.identity);
            spawnDestroy.SetActive(true);
            critterList.Add(spawnDestroy);
            critterCount += 1;
        }
        yield return new WaitForSeconds(stayUpTime);
        foreach (var critter in critterList)
        {
            Destroy(critter);
            Debug.Log("Hiding");
        }
        critterList = null;
    }

}
