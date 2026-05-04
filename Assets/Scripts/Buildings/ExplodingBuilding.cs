using UnityEngine;

public class ExplodingBuilding : BaseBuilding
{
    private SphereCollider sphereCollider;

    protected override void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        // Set mine radius
    }

    private void OnTriggerEnter(Collider other)
    {
        // Check if a enemy activates the mine
        if (other.gameObject.CompareTag("Enemy"))
        {
            // Apply damage to all nearby enemies
            Explode();
        }
    }

    private void Explode()
    {
        // Play explotion

        // Get nearby enemies
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, 1f);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                IAttackable enemy = (IAttackable)enemyCollider;
                enemy.ApplyDamage(12);
            }
        }

        // Destroy mine
        Destroy(gameObject);
    }
}
