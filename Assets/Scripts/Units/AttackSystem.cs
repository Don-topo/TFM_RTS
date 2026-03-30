using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackSystem : MonoBehaviour
{
    [SerializeField] private UnitInRangeEvent unitInRangeEvent;
    [SerializeField] private UnitDeathEvent unitDeathEvent;
    public List<IAttackable> GetAttackableEnemies() => enemiesVisible;
    public List<IAttackable> GetNearEnemies() => enemiesInRange;
    
    private List<IAttackable> enemiesInRange = new List<IAttackable>();
    private List<IAttackable> enemiesVisible = new List<IAttackable>();
    private SphereCollider sphereCollider;

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
    }

    private void UnitDeath(BaseUnit unitDeathEvent)
    {

    }
}
