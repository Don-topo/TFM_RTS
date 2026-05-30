using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;

public class BaseHealer : BaseUnit, IHealer
{

    [field: SerializeField] public SO_HealInfo HealInfo { get; private set; }
    [SerializeField] private HealingSystem healingSystem;
    [SerializeField] private AllyInRangeEvent unitEnterRange;
    [SerializeField] private AllyInRangeEvent unitOutOfRange;
    public Transform Transform => transform;

    protected override void Awake()
    {
        base.Awake();
        behaviorGraphAgent.SetVariableValue("SO Heal Info", HealInfo);
        unitEnterRange.Register(UnitInRange);
        unitOutOfRange.Register(UnitOutOfRange);
    }

    protected override void Start()
    {
        base.Start();
        healingSystem.SetRange(HealInfo.HealRange);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        unitEnterRange.Unregister(UnitInRange);
        unitOutOfRange.Unregister(UnitOutOfRange);
    }

    public void Heal(IHealable unit)
    {
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", unit.TargetPosition.gameObject);
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Heal);
    }

    public void Heal(Vector3 healPosition)
    {
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
        behaviorGraphAgent.SetVariableValue("TargetPosition", healPosition);
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Heal);
    }

    private void UnitInRange(IHealable unit)
    {
        List<GameObject> targets = SetNearbyAlliesOnBlackboard();

        if (behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            && targetVariable.Value == null && targets.Count > 0)
        {

            behaviorGraphAgent.SetVariableValue("TargetGameObject", GetFirstDamaged(targets));
        }
    }

    private void UnitOutOfRange(IHealable unit)
    {
        List<GameObject> targets = SetNearbyAlliesOnBlackboard();

        if (!behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            || unit.TargetPosition.gameObject != targetVariable.Value) return;

        if (targets.Count > 0)
        { 
            behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", GetFirstDamaged(targets));
        }
        else
        {
            behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
            behaviorGraphAgent.SetVariableValue("TargetPosition", unit.TargetPosition.position);


           /* behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
            behaviorGraphAgent.GetVariable("UnitActions", out BlackboardVariable<UnitActions> currentAction);
            behaviorGraphAgent.SetVariableValue("TargetLocation", enemyOutOfRange.TargetPosition.position);*/
        }
    }

    private List<GameObject> SetNearbyAlliesOnBlackboard()
    {
        List<GameObject> nearbyEnemies = healingSystem.AlliesInRange().ConvertAll(
                    damage => damage.TargetPosition.gameObject);
        nearbyEnemies.Sort(new ClosestGameObjectComparer(transform.position));

        behaviorGraphAgent.SetVariableValue("Allies", nearbyEnemies);

        return nearbyEnemies;
    }

    private GameObject GetFirstDamaged(List<GameObject> allies)
    {
        return allies.FirstOrDefault(ally => ally.GetComponent<BaseUnit>().CurrentHealth < ally.GetComponent<BaseUnit>().MaxHealth);
    }
}
