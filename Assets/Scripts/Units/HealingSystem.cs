using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class HealingSystem : MonoBehaviour
{
    [Header("Events")]
    [SerializeField] private AllyInRangeEvent allyInRangeEvent;
    [SerializeField] private UnitDeathEvent unitDeathEvent;

    private List<IHealable> alliesInRange = new List<IHealable>();
    private SphereCollider sphereCollider;
    private float range;

    public void SetRange(float range) => this.range = range;
    public List<IHealable> AlliesInRange() => alliesInRange;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.radius = range;
    }

    private void OnDestroy()
    {
        unitDeathEvent.Unregister(UnitDeath);
    }

    private void OnTriggerEnter(Collider other)
    {
        // Get only if the object is an ally and can take damage
        if (other.TryGetComponent(out IHealable ally))
        {
            alliesInRange.Add(ally);
            allyInRangeEvent.Raise(ally);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.TryGetComponent(out  IHealable healable))
        {
            alliesInRange.Remove(healable);
        }

        if (alliesInRange.Count == 0)
        {
            unitDeathEvent.Unregister(UnitDeath);
        }
    }

    private void UnitDeath(BaseUnit baseUnit)
    {              
        // Check if the death unit is an ally on range
        if (alliesInRange.Contains((IHealable)baseUnit))
        {
            // Trigger Manually TriggerExit
            OnTriggerExit(baseUnit.GetComponent<Collider>());
        }
    }
}
