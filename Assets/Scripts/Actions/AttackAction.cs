using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Units/Actions/Attack", order = 99)]
public class AttackAction : BaseAction
{
    [SerializeField] MoveAction moveAction;

    public override bool Blocked(ActionInfo actionInfo)
    {
        if(actionInfo.Action == null) return false;
        if(actionInfo.Action.TryGetComponent<BaseBuilding>(out BaseBuilding building))
        {
            return !(building.IsConstructed && !building.IsRepairing);
        }

        return false;
    }

    public override bool CanExecute(ActionInfo actionInfo)
    {
        if (actionInfo.Action.TryGetComponent<BaseBuilding>(out BaseBuilding building))
        {
            return !(building.IsConstructed && !building.IsRepairing) &&
                actionInfo.Action is IAttackable && actionInfo.Hit.collider != null;
        }
        return actionInfo.Action is IAttackable && actionInfo.Hit.collider != null;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        IAttacker attacker = actionInfo.Action as IAttacker;
        // Safety check

        if (attacker == null) return;
        IAttackable attackable = actionInfo.Hit.collider.GetComponent<IAttackable>();
        // Check if enemy is selected directly
        if(attackable != null)
        {
            attacker.Attack(attackable);
        }
        // Check if the unit can move
        else if(moveAction != null)
        {
            attacker.Attack(moveAction.CalculateMovePosition(actionInfo));
        }
        // Otherwise move to selected point
        else
        {
            attacker.Attack(actionInfo.Hit.point);
        }        
    }
}
