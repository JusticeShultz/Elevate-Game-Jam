using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public Transform[] spawnLocations;

    public EnemySpawnSO enemySpawnData;

    public GameObject enemyPrefab;

    [ReadOnlyField]
    public float timeSinceLoad;

    [ReadOnlyField]
    public bool hasEnemiesToSpawn;

    [ReadOnlyField]
    public List<EnemySpawnSO.EnemySpawn> enemySpawnDataCopy;

    [ReadOnlyField]
    public List<Drone_Enemy> enemyObjectPool;

    [ReadOnlyField]
    public PlayerManager player;

    [ReadOnlyField]
    public bool sceneIsRestarting;


    public enum EnemyTypes
    {
        Default,
        Fast,
        Tanky
    }


    [System.Serializable]
    public struct EnemyDroneStats
    {
        public float health;
        public float speed;
        public float angularVel;
        public float chargeSpeed;
    };

    public EnemyDroneStats DefaultDroneStat;
    public EnemyDroneStats FastDroneStat;
    public EnemyDroneStats TankyDroneStat;





    void Start()
    {
        timeSinceLoad = 0.0f;
        //create  on start?
        enemySpawnDataCopy = new List<EnemySpawnSO.EnemySpawn>(enemySpawnData.EnemySpawnData);

        sceneIsRestarting = false;

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
        if (sceneIsRestarting == false)
        {
            timeSinceLoad += Time.deltaTime;
        }

        //cant spawn more than one per frame
        if (hasEnemiesToSpawn == true && enemySpawnDataCopy[0].SpawnTimer <= timeSinceLoad)
        {
            //do shit
            //TODO

            Drone_Enemy temp = GetAvaibleDrone();

            temp.gameObject.SetActive(true);

            temp.InitializeDrone(player, GetStatFromDroneType(enemySpawnDataCopy[0].enemyType), spawnLocations[enemySpawnDataCopy[0].SpawnLocationIndex].position);

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

            Debug.Log("No more enemies, next level");
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

    public void RestartLevel()
    {
        //prevents multiple callbacks
        if (sceneIsRestarting == true)
        {
            return;
        }
        sceneIsRestarting = true;
        StartCoroutine(RestartingScene());
    }

    public EnemyDroneStats GetStatFromDroneType(EnemyTypes type)
    {
        switch (type)
        {
            case EnemyTypes.Default:
                return DefaultDroneStat;
            case EnemyTypes.Fast:

                return FastDroneStat;

            case EnemyTypes.Tanky:

                return TankyDroneStat;
            default:
                return DefaultDroneStat;
        }
        
    }


    public IEnumerator RestartingScene()
    {
        ElevatorController.Elevator.animator.SetTrigger("Close_trigger");

        yield return new WaitForSeconds(3.0f);

        enemySpawnDataCopy = new List<EnemySpawnSO.EnemySpawn>(enemySpawnData.EnemySpawnData);

        if (enemySpawnDataCopy.Count > 0)
        {
            hasEnemiesToSpawn = true;
        }

        foreach (Drone_Enemy m in enemyObjectPool)
        {
            m.gameObject.SetActive(false);
            m.ShootParticle.SetActive(false);
            m.ChargingShot = false;
            m.transform.position = spawnLocations[0].position;
        }

        ElevatorController.Elevator.animator.SetTrigger("Open_trigger");

        yield return new WaitForSeconds(3.0f);

        timeSinceLoad = 0.0f;
        sceneIsRestarting = false;



        /*
         * Logic
         * 
         * if player dies, it calls RestartLevel();
         * 
         * if the scene is already restarting, return
         * 
         * else start Courutine
         * 
         * starts close elevator animation
         * 
         * waits 3 secs for door to close
         * 
         * resets DataCopy
         * 
         * turns of all enemies, and brings em to spawnpoint
         * 
         * opens door,
         * 
         * waits 3 secs
         * 
         * resets timer, and turns off scene restart
         * 
         * 
         */
    }


}
