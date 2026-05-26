using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class AttackSystem : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private EnemyInRangeEvent unitInRangeEvent;
    [SerializeField] private EnemyInRangeEvent unitOutRangeEvent;
    [SerializeField] private UnitDeathEvent unitDeathEvent;
    [SerializeField] private VisibilityEvent unitVisibilityEvent;
    
    private List<IAttackable> enemiesInRange = new List<IAttackable>();
    private List<IAttackable> enemiesVisible = new List<IAttackable>();
    private SphereCollider sphereCollider;

    public void SetAttackRange(float range) => sphereCollider.radius = range;
    public List<IAttackable> GetEnemiesInRange() => enemiesInRange;
    public List<IAttackable> GetVisibleEnemies() => enemiesVisible;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
    }

    private void OnDestroy()
    {
        foreach(IAttackable enemy in enemiesInRange)
        {
            unitVisibilityEvent.Unregister(HandleVisivilityChange);
        }
        unitDeathEvent.Unregister(UnitDeath);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get only if the object is an enemy and can take damage
        if(other.TryGetComponent(out IAttackable enemy) && !other.CompareTag(transform.parent.tag))
        {
            enemiesInRange.Add(enemy);
            if(other.TryGetComponent(out IHideable hideable))
            {
                unitVisibilityEvent.Register(HandleVisivilityChange);
                if (hideable.IsVisible)
                {
                    enemiesVisible.Add(enemy);
                    unitInRangeEvent.Raise(enemy);
                }
            }
            else
            {
                enemiesVisible.Add(enemy);
                unitInRangeEvent.Raise(enemy);
            }
                
            unitDeathEvent.Register(UnitDeath);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out IAttackable enemy))
        {            
            if(enemy is IHideable)
            {
                unitVisibilityEvent.Unregister(HandleVisivilityChange);
            }
            
            enemiesVisible.Remove(enemy);
            enemiesInRange.Remove(enemy);
            unitOutRangeEvent.Raise(enemy);
        }

        // Check if the unitDeath event trigger this method and the list is empty
        if(enemiesInRange.Count == 0)
        {
            unitDeathEvent.Unregister(UnitDeath);
        }
    }

    private void UnitDeath(CommonActions unitDeathEvent)
    {
        // Check if the death unit is on attack range
        if (enemiesInRange.Contains((IAttackable)unitDeathEvent))
        {
            // Trigger Manually TriggerExit
            OnTriggerExit(unitDeathEvent.GetComponent<Collider>());
        }
    }

    private void HandleVisivilityChange(Vision vision)
    {
        IAttackable damageable = vision.Hideable.TargetPosition.GetComponent<IAttackable>();
        if (vision.IsVisible)
        {
            enemiesVisible.Add(damageable);            
            unitInRangeEvent.Raise(damageable);            
        }
        else
        {
            enemiesVisible.Remove(damageable);
            unitOutRangeEvent.Raise(damageable);                       
        }
    }
}
