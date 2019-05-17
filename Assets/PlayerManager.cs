using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerManager : MonoBehaviour
{
    /*
     * Handles Player Stats,
     *    Player Score
     *    other stats
     *    
     *    current level is handled on world manager
     */
    public static PlayerManager player;

    [Header("Player Stats")]
    [ReadOnlyField]
    public float playerCurrentHP;

    public float playerStartHP;


    [ReadOnlyField]
    public bool playerIsAlive = true;
    [ReadOnlyField]
    public bool playerIsInvicible = false;

    [Space(20)]
    public float playerInvicibilityDuration;

    //shit we can add
    [ReadOnlyField]
    public int shotsFired;
    [ReadOnlyField]
    public int enemiesKilled;
    [ReadOnlyField]
    public int someRandomShit;

    [Space(10)]
    [ReadOnlyField]
    public EnemySpawner enemySpawner;



    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }


    void Start()
    {
        player = this;

        DontDestroyOnLoad(gameObject);

        playerCurrentHP = playerStartHP;
        playerIsAlive = true;
        playerIsInvicible = false;
         enemySpawner = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<EnemySpawner>();
        enemySpawner.GetComponent<EnemySpawner>().player = this;
        //OnSceneLoad();
    }
    //https://itch.io/game/accept-admin/424561/1040770 ray
    //https://itch.io/game/accept-admin/424561/1624064 randal

    void Update()
    {
        if (enemySpawner == null)
        {
        }

    }


    public void TakeDamage(float damageAmount)
    {
        if (playerIsInvicible == false)
        {
            playerCurrentHP -= damageAmount;
            StartCoroutine(ActivateInvincibility());
        }

        if (playerCurrentHP <= 0)
        {
            playerIsAlive = false;

            enemySpawner.RestartLevel();

            //end game?
            //restartLevel
            //TODO
        }

    }


    //wont stack
    public IEnumerator ActivateInvincibility()
    {
        playerIsInvicible = true;

        yield return new WaitForSeconds(playerInvicibilityDuration);

        playerIsInvicible = false;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        enemySpawner = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<EnemySpawner>();
        enemySpawner.GetComponent<EnemySpawner>().player = this;
    }
}
