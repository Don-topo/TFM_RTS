using UnityEngine;

[CreateAssetMenu(fileName = "StopAction", menuName = "Units/Actions/Stop", order = 101)]
public class StopAction : BaseAction
{
    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action is BaseUnit;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        BaseUnit unit = (BaseUnit)actionInfo.Action;
        unit.StopMove();
    }
}
