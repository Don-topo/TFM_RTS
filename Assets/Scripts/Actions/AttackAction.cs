using System.Data;
using UnityEngine;

[CreateAssetMenu(fileName = "Attack", menuName = "Units/Actions/Attack", order = 99)]
public class AttackAction : BaseAction
{
    [SerializeField] MoveAction moveAction;

    public override bool Blocked(ActionInfo actionInfo) => false;

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action is IAttackable && actionInfo.Hit.collider != null;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        IAttacker attacker = actionInfo.Action as IAttacker;
        if (attacker == null) return;
        IAttackable attackable = actionInfo.Hit.collider.GetComponent<IAttackable>();
        if (attackable == null) return;
        if(moveAction != null)
        {
            //attacker.Attack(moveAction);
        }
        else
        {
            attacker.Attack(attackable);
        }        
    }
}
