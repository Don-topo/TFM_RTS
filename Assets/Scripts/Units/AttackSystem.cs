using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackSystem : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private UnitInRangeEvent unitInRangeEvent;
    [SerializeField] private UnitDeathEvent unitDeathEvent;
    
    private List<IAttackable> enemiesInRange { get; } = new List<IAttackable>();
    private List<IAttackable> enemiesVisible { get; } = new List<IAttackable>();
    private SphereCollider sphereCollider;

    public void SetAttackRange(float range) => sphereCollider.radius = range;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnDestroy()
    {
        unitDeathEvent.Unregister(UnitDeath);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get only if the object is an enemy and can take damage
        if(other.TryGetComponent(out IAttackable enemy) && !other.CompareTag(tag))
        {
            enemiesInRange.Add(enemy);
            // TODO Check if the enemy is visible
            enemiesVisible.Add(enemy);
            unitInRangeEvent.Raise(enemy);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IAttackable enemy))
        {
            enemiesVisible.Remove(enemy);
            enemiesInRange.Remove(enemy);
            
        }

        // Check if the unitDeath event trigger this method and the list is empty
        if(enemiesInRange.Count == 0)
        {
            unitDeathEvent.Unregister(UnitDeath);
        }
    }

    private void UnitDeath(BaseUnit unitDeathEvent)
    {
        // Check if the death unit is on attack range
        if (enemiesInRange.Contains((IAttackable)unitDeathEvent))
        {
            // Trigger Manually TriggerExit
            OnTriggerExit(unitDeathEvent.GetComponent<Collider>());
        }
    }
}
