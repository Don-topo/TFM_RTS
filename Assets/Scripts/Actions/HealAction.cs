using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Units/Actions/Heal", order = 102)]
public class HealAction : BaseAction
{
    public override bool Blocked(ActionInfo actionInfo) => false;

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action is IHealable && actionInfo.Hit.collider != null;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        IAttacker attacker = actionInfo.Action as IAttacker;
        if (attacker == null) return;
        IAttackable attackable = actionInfo.Hit.collider.GetComponent<IAttackable>();
        IHealable unitToHeal = (IHealable)actionInfo.Action;
        if(unitToHeal != null)
        {

        }
    }
}
