using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    /*
     * Handles Player Stats,
     *    Player Score
     *    other stats
     *    
     *    current level is handled on world manager
     */

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




    void Start()
    {
        DontDestroyOnLoad(gameObject);

        playerCurrentHP = playerStartHP;
        playerIsAlive = true;
        playerIsInvicible = false;

        OnSceneLoad();
    }


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

    private void OnSceneLoad()
    {
        enemySpawner = GameObject.FindGameObjectWithTag("LevelManager").GetComponent<EnemySpawner>();
        enemySpawner.GetComponent<EnemySpawner>().player = this;
    }

}
