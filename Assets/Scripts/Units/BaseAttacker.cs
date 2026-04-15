using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class BaseAttacker : BaseUnit, IAttacker
{
    public Transform Transform => throw new System.NotImplementedException();
    [SerializeField] private AttackSystem attackSystem;
    [SerializeField] private UnitInRangeEvent unitEnterRange;
    [SerializeField] private UnitInRangeEvent unitOutOfRange;

    protected override void Awake()
    {
        base.Awake();
        behaviorGraphAgent.SetVariableValue("SO Attack Info", SO_BaseUnit.AttackInfo);
        unitEnterRange.Register(UnitInRange);
        unitOutOfRange.Register(UnitOutOfRange);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        unitEnterRange.Unregister(UnitInRange);
        unitOutOfRange.Unregister(UnitOutOfRange);
    }

   
    public void Attack(IAttackable attackable)
    {
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", attackable.TargetPosition.gameObject);
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Attack);
    }

    public void Attack(Vector3 attackPosition)
    {
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
        behaviorGraphAgent.SetVariableValue("TargetPosition", attackPosition);
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Attack);
    }

    private void UnitInRange(IAttackable enemyInRange)
    {
        List<GameObject> targets = SetNearbyEnemiesOnBlackboard();

        if (behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            && targetVariable.Value == null && targets.Count > 0)
        {
            behaviorGraphAgent.SetVariableValue("TargetGameObject", targets[0]);
        }
    }

    private void UnitOutOfRange(IAttackable enemyOutOfRange)
    {
        List<GameObject> targets = SetNearbyEnemiesOnBlackboard();

        if (!behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            || enemyOutOfRange.TargetPosition.gameObject != targetVariable.Value) return;

        if (targets.Count > 0)
        {
            behaviorGraphAgent.SetVariableValue("TargetGameObject", targets[0]);
        }
        else
        {
            behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
            behaviorGraphAgent.SetVariableValue("TargetPosition", enemyOutOfRange.TargetPosition.position);
        }
    }

    private List<GameObject> SetNearbyEnemiesOnBlackboard()
    {
        List<GameObject> nearbyEnemies = attackSystem.GetEnemiesInRange().ConvertAll(
                    damage => damage.TargetPosition.gameObject);
        nearbyEnemies.Sort(new ClosestGameObjectComparer(transform.position));

        behaviorGraphAgent.SetVariableValue("Enemies", nearbyEnemies);

        return nearbyEnemies;
    }
}
