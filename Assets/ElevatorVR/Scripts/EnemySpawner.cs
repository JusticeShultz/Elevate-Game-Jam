using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnLocations;

    public EnemySpawnSO enemySpawnData;

    public GameObject enemyPrefab;

    [ReadOnlyField]
    public bool hasEnemiesToSpawn;

    [ReadOnlyField]
    public List<EnemySpawnSO.EnemySpawn> enemySpawnDataCopy;

    [ReadOnlyField]
    public List<Drone_Enemy> enemyObjectPool;

    [ReadOnlyField]
    public PlayerManager player;

    void Start()
    {
        //create  on start?
        enemySpawnDataCopy = new List<EnemySpawnSO.EnemySpawn>(enemySpawnData.EnemySpawnData);

        if (enemySpawnData.enemyInitializeValue > 0)
        {
            if (enemySpawnDataCopy.Count > 0)
            {
                hasEnemiesToSpawn = true;
            }

            for (int i = 0; i < enemySpawnData.enemyInitializeValue; i++)
            {
                enemyObjectPool.Add(Instantiate(enemyPrefab, spawnLocations[0].position, Quaternion.identity).GetComponent<Drone_Enemy>());
            }
            
        }
    }

    void Update()
    {
        //cant spawn more than one per frame
        if (hasEnemiesToSpawn == true && enemySpawnDataCopy[0].SpawnTimer <= Time.timeSinceLevelLoad)
        {
            //do shit
            //TODO

            Drone_Enemy temp = GetAvaibleDrone();

            temp.gameObject.SetActive(true);
            temp.InitializeDrone(player, enemySpawnDataCopy[0], spawnLocations[enemySpawnDataCopy[0].SpawnLocationIndex].position);

            enemySpawnDataCopy.RemoveAt(0);

            if (enemySpawnDataCopy.Count == 0)
            {
                hasEnemiesToSpawn = false;
            }
        }

        if (hasEnemiesToSpawn == false)
        {
            foreach (Drone_Enemy m in enemyObjectPool)
            {
                if (m.gameObject.activeSelf == true)
                {
                    return;
                }
            }

            //If no enemies and no active enemies
            /*
             *Ready to load new scene 
             * 
             * 
             */
            ElevatorController.Elevator.LoadNextScene();
        }
    }

    private Drone_Enemy GetAvaibleDrone()
    {
        for (int i = 0; i < enemyObjectPool.Count; i++)
        {
            if (enemyObjectPool[i].gameObject.activeSelf == false)
            {
                return enemyObjectPool[i];
            }
        }

        Drone_Enemy temp = Instantiate(enemyPrefab, spawnLocations[0].position, Quaternion.identity).GetComponent<Drone_Enemy>();

        enemyObjectPool.Add(temp);

        return temp;
    }
}
