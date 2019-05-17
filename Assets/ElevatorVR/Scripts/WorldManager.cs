using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : MonoBehaviour
{

    public int startingLevel;

    [ReadOnlyField]
    public int currentLevel;


    [ReadOnlyField]
    public bool isInElevator;

    //fix this
    [ReadOnlyField]
    public LevelManager levelManager;


    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (levelManager.ListOfEnemies.Length == 0)
        {
            //can go to next scene
        }

    }


    

}
