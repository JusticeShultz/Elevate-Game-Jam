using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelSpawnParameters")]
public class EnemySpawnSO : ScriptableObject
{
    [System.Serializable]
    public struct EnemySpawn
    {
        //enemy type,
        public int SpawnLocationIndex;
        public float SpawnTimer;
    }

    public int enemyInitializeValue;
    public List<EnemySpawn> EnemySpawnData = new List<EnemySpawn>();
}
