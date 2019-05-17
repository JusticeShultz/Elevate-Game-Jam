using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drone_Enemy : MonoBehaviour
{
    public GameObject RootRotation;
    public GameObject ShootParticle;
    public NavMeshAgent Agent;
    public float ShotChargeTime = 4.0f;

    public float shotDamage = 10000000f;

    public float MaxHealth = 1.0f;

    [ReadOnlyField] public PlayerManager player;

    [ReadOnlyField] public float CurrentHealth;
    [ReadOnlyField] public float StoppingDistance;
    [ReadOnlyField] public bool ChargingShot = false;

    private void Start()
    {
        CurrentHealth = MaxHealth;
        StoppingDistance = Agent.stoppingDistance;

        //gameObject.SetActive(false);
    }

    private void Update()
    {
        RootRotation.transform.LookAt(PlayerReference.Player.transform.position);

        if (Vector3.Distance(transform.position, PlayerReference.Player.transform.position) <= StoppingDistance)
        {
            Agent.isStopped = true;

            if(!ChargingShot)
            {
                ChargingShot = true;
                StartCoroutine(KillPlayer());
            }

            return;
        }

        if(Agent.isOnNavMesh)
            Agent.SetDestination(PlayerReference.Player.transform.position);
    }

    public void TakeDamage(float damageAmount)
    {
        CurrentHealth -= damageAmount;

        if (CurrentHealth <= 0)
            gameObject.SetActive(false);
    }

    public void InitializeDrone(PlayerManager _player ,EnemySpawnSO.EnemySpawn enemyStats, Vector3 pos)
    {
        player = _player;
        transform.position = pos;
        //set up ai shit like speed and shit
    }

    private IEnumerator KillPlayer()
    {

        yield return new WaitForSeconds(ShotChargeTime);

        if (gameObject.activeSelf == false)
        {
            yield break;
        }

        ShootParticle.SetActive(true);

        player.TakeDamage(shotDamage);
    }
}
