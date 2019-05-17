using UnityEngine;

public class BulletController : MonoBehaviour
{
    public TrailRenderer Trail;

    [ReadOnlyField] public float Damage = 0.0f;

    private void OnEnable()
    {
        Trail.enabled = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Trail.Clear();
        Trail.enabled = false;
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            collision.gameObject.transform.parent.parent.GetComponent<Drone_Enemy>().TakeDamage(Damage);
            Trail.Clear();
            Trail.enabled = false;
            gameObject.SetActive(false);
        }
    }
}