using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CritterSpawnerManager : MonoBehaviour
{
    public GameObject player; //reference player to get position
    public GameObject spawnedCritter;
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
        while(critterCount < 6)
        {
            xPos = Random.Range(player.transform.position.x - 5, player.transform.position.x + 5);
            yPos = player.transform.position.y;
            zPos = Random.Range(player.transform.position.z - 5, player.transform.position.z + 5);

            Instantiate(spawnedCritter, new Vector3(xPos, yPos, zPos), Quaternion.identity);
            critterList.Add(spawnedCritter);
            critterCount += 1;
            yield return null;
        }

    }

    public void DestroyCritters()
    {
        new WaitForSeconds(stayUpTime);
        for(int i = 0; i < critterList.Count; i++)
        {
            critterList[i].SetActive(false);
            Debug.Log("Hid");
            Destroy(critterList[i]);
            Debug.Log("Destroyed");
        }
    }


}
