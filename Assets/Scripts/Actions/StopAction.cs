using UnityEngine;

[CreateAssetMenu(fileName = "StopAction", menuName = "Units/Actions/Stop", order = 101)]
public class StopAction : BaseAction
{
    public override bool Blocked(ActionInfo actionInfo)
    {
        if (actionInfo.Action.TryGetComponent<BaseBuilding>(out BaseBuilding building))
        {
            return !(building.IsConstructed && !building.IsRepairing);
        }

        return false;
    }

    public override bool CanExecute(ActionInfo actionInfo)
    {
        if (actionInfo.Action.TryGetComponent<BaseBuilding>(out BaseBuilding building))
        {
            return !(building.IsConstructed && !building.IsRepairing) && actionInfo.Action is BaseUnit;
        }

        return actionInfo.Action is BaseUnit;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        BaseUnit unit = (BaseUnit)actionInfo.Action;
        unit.Stop();
    }
}
