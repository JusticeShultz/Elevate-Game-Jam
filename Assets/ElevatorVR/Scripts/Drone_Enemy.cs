using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Drone_Enemy : MonoBehaviour
{
    public GameObject RootRotation;
    public GameObject ShootParticle;
    public GameObject DeathObject;
    public NavMeshAgent Agent;
    public float ShotChargeTime = 4.0f;

    public float shotDamage = 10000000f;

    public MeshRenderer meshRenderer;

    [ReadOnlyField] public PlayerManager player;

    [ReadOnlyField] public float CurrentHealth;
    [ReadOnlyField] public float StoppingDistance;
    [ReadOnlyField] public bool ChargingShot = false;

    private void Awake()
    {
        CurrentHealth = 1;
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
        {
            Instantiate(DeathObject, transform.position + new Vector3(0, 1.625f, 0), transform.rotation);
            gameObject.SetActive(false);
        }
    }

    public void InitializeDrone(PlayerManager _player, EnemySpawner.EnemyDroneStats stats, Vector3 pos)
    {
        meshRenderer.material = stats.mat;

        player = _player;
        transform.position = pos;
        CurrentHealth = stats.health;
        Agent.speed = stats.speed;
        Agent.angularSpeed = stats.angularVel;
        ShotChargeTime = stats.chargeSpeed;

        //set up ai shit like speed and shit
    }

    private IEnumerator KillPlayer()
    {

        yield return new WaitForSeconds(ShotChargeTime);

        if (ChargingShot == false)
        {
            yield break;
        }
        ShootParticle.SetActive(true);

        player.TakeDamage(shotDamage);
    }
}
