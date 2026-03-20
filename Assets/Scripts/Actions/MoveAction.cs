using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Units/Actions/Move", order = 100)]
public class MoveAction : BaseAction
{
    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action is BaseUnit;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        BaseUnit unit = (BaseUnit)actionInfo.Action;
        if(actionInfo.Hit.collider != null && actionInfo.Hit.collider.TryGetComponent(out CommonActions action))
        {
            unit.Move(action.transform);
            return;
        }

        unit.Move(actionInfo.Hit.point);
    }
}
