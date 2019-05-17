using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "LevelSpawnParameters")]
public class EnemySpawnSO : ScriptableObject
{

    [System.Serializable]
    public struct EnemySpawn
    {
        public EnemySpawner.EnemyTypes enemyType;
        public int SpawnLocationIndex;
        public float SpawnTimer;
    }

    public int enemyInitializeValue;
    public List<EnemySpawn> EnemySpawnData = new List<EnemySpawn>();
}
