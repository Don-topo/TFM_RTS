using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class BaseAttacker : BaseUnit, IAttacker
{
    public Transform Transform => throw new System.NotImplementedException();
    [SerializeField] private AttackSystem attackSystem;
    [SerializeField] private UnitInRangeEvent unitEnterRange;
    [SerializeField] private UnitInRangeEvent unitOutOfRange;
    [field: SerializeField] public SO_AttackInfo AttackInfo { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        behaviorGraphAgent.SetVariableValue("SO Attack Info", AttackInfo);
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

    public void Patrol(Vector3 targetPosition)
    {
        if (targetPosition == null) return;

        Vector3 currenPosition = gameObject.transform.position;
        List<Vector3> patrolPositions = new List<Vector3>
        {
            currenPosition,
            targetPosition
        };

        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Patrol);
        behaviorGraphAgent.SetVariableValue("PatrolPositions", patrolPositions);

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
