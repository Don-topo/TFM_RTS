using UnityEngine;

public class ExplodingBuilding : BaseBuilding
{
    [Header("Required Components")]
    [SerializeField] private SO_AttackInfo attackInfo;
    [SerializeField] private SphereCollider sphereCollider;
    [SerializeField] private float actionRadius = 1f;

    protected override void Awake()
    {
        // Set mine radius
        sphereCollider.radius = actionRadius;
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
        // Get nearby enemies
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, attackInfo.AttackRange);

        foreach (Collider enemyCollider in enemiesInRange)
        {
            if (enemyCollider.CompareTag("Enemy"))
            {
                if (enemyCollider.TryGetComponent<IAttackable>(out var attackable))
                {
                    attackable.ApplyDamage(attackInfo.AttackDamage);
                }                
            }
        }

        // Destroy mine
        Destroy(gameObject);
    }
}
