using UnityEngine;

public class BaseAttacker : BaseUnit, IAttacker
{
    public Transform Transform => throw new System.NotImplementedException();

    protected override void Awake()
    {
        base.Awake();
        behaviorGraphAgent.SetVariableValue("SO Attack Info", SO_BaseUnit.AttackInfo);
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
}
