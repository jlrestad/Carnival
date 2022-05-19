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
    public Queue<List<GameObject>> critterQueue = new Queue<List<GameObject>>();
    
    // to turn triggers on
    private Collider SDCollider;

    public IEnumerator SpawnCritters()
    {
        critterCount = 0;
        critterList = new List<GameObject>();

        //get player position and move parent to position
        player = GameObject.FindGameObjectWithTag("Player");

        Debug.Log("Spawning");
        //check queue length, no more than 15 on (3 sets) at a time
        if(critterQueue.Count < 3)
        {
            //spawn 5 critters
            while (critterList.Count < 5)
            {
                
                //try to find ground -- needs a little fine tune
                if(Physics.Raycast(player.transform.position, Vector3.down, out RaycastHit hit))
                {
                    yPos = hit.point.y;
                }
                else
                { 
                    yPos = player.transform.position.y - 1;
                }
                //spawn area coordinates
                xPos = Random.Range(player.transform.position.x - 5, player.transform.position.x + 5);
                zPos = Random.Range(player.transform.position.z - 5, player.transform.position.z + 5);

                // add wait to stagger spawns
                yield return new WaitForSeconds(1f);

                //instantiate a critter
                spawnDestroy = Instantiate(spawnedCritter, new Vector3(xPos, yPos, zPos), Quaternion.identity);
                spawnDestroy.SetActive(true);

                //get collider and turn trigger on
                SDCollider = spawnedCritter.GetComponent<Collider>();
                SDCollider.isTrigger = true;

                critterList.Add(spawnDestroy);
                critterCount += 1;
            }

            critterQueue.Enqueue(critterList);
        }
        
        // wait for up time then call destroy on group that's been up the longest
        yield return new WaitForSeconds(stayUpTime);
        DestroySpawns();
    }


    public void DestroySpawns()
    {
        if(critterQueue != null)
        {
            List<GameObject> list = critterQueue.Dequeue();
            foreach(var critter in list)
            {
                Destroy(critter);
            }
        }
    }


}
