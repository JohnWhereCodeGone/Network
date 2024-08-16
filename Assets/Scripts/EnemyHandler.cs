using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

// spawn enemies within specified area
// 

public class EnemyHandler : NetworkBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private BoxCollider2D spawnZone;

    float tempTimer = 1f;

    NetworkVariable<float> waveDelay = new NetworkVariable<float>();
    NetworkVariable<int> enemyCounter = new NetworkVariable<int>();
    NetworkVariable<Vector2> spawnpos = new NetworkVariable<Vector2>();

    // Start is called before the first frame update
    void Start()
    {
        //StartCoroutine(SpawnEnemies());

    }


    private Vector2 setSpawnLocation()
    {
        Vector2 spawnRange = new Vector2(Random.Range(0, transform.position.x + spawnZone.size.x) , Random.Range(0, transform.position.y + spawnZone.size.y));


        return spawnRange;
    }

    private IEnumerator SpawnEnemies()
    {
        
        bool isSpawning = true;
        while (isSpawning)
        {
            for(int i = 0; i < 15; i++)
            {
                GameObject temp = Instantiate(enemyPrefab);
                enemyPrefab.transform.position = (Vector3)setSpawnLocation();
                
            }


            yield return new WaitForSeconds(tempTimer);
            tempTimer -= 0.25f;

        }
    }
    // Update is called once per frame
    void Update()
    {
        tempTimer += Time.deltaTime;
        if (IsServer && tempTimer >= 5f)
        {
            SpawnEmenyRPC();
            SpawnEmenyRPC();
            SpawnEmenyRPC();
            tempTimer = 0f;
        }
    }


    [Rpc(SendTo.Server)]
    private void SpawnEmenyRPC()
    {
        GameObject temp = Instantiate(enemyPrefab);
        temp.GetComponent<NetworkObject>().Spawn(true);
        enemyPrefab.transform.position = (Vector3)setSpawnLocation();

    }
}
