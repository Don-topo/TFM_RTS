using UnityEngine;

[CreateAssetMenu(fileName = "Heal", menuName = "Units/Actions/Heal", order = 102)]
public class HealAction : BaseAction
{
    [SerializeField] MoveAction moveAction;

    public override bool Blocked(ActionInfo actionInfo) => false;

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action is IHealable && actionInfo.Hit.collider != null;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        IHealer healer = actionInfo.Action as IHealer;
        if (healer == null) return;
        IHealable healable = actionInfo.Hit.collider.GetComponent<IHealable>();
        if (healable != null)
        {
            healer.Heal(healable);
        }
        else if (moveAction != null)
        {
            healer.Heal(moveAction.CalculateMovePosition(actionInfo));
        }
        else
        {
            healer.Heal(actionInfo.Hit.point);
        }
    }
}
