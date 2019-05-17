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
    public float playerStartHP;
    [ReadOnlyField]
    public float playerCurrentHP;


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
    public GameObject LevelManagerGameObject;



    void Start()
    {
        playerCurrentHP = playerStartHP;
        playerIsAlive = true;
        playerIsInvicible = false;

        OnSceneLoad();
    }


    void Update()
    {


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
        LevelManagerGameObject = GameObject.FindGameObjectWithTag("LevelManager");
        LevelManagerGameObject.GetComponent<EnemySpawner>().player = this;
    }

}
