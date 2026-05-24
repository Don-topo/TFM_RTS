using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "RepairBuilding", menuName = "Building/Action/Repair Building")]
public class RepairAction : BaseAction
{
    public override bool Blocked(ActionInfo actionInfo)
    {
        if(actionInfo.Action != null && actionInfo.Action.TryGetComponent<BaseBuilding>(out BaseBuilding building))
        {
            return !(actionInfo.Action.CurrentHealth < actionInfo.Action.SO_BaseUnit.Health && building.IsConstructed && !building.IsRepairing);
        }
        
        return false;
    }

    public override bool CanExecute(ActionInfo actionInfo)
    {
        if(actionInfo.Action != null && actionInfo.Action.TryGetComponent<BaseBuilding>(out BaseBuilding building))
        {
            return actionInfo.Action.CurrentHealth < actionInfo.Action.SO_BaseUnit.Health && building.IsConstructed && !building.IsRepairing;
        }

        return false;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        BaseBuilding baseBuilding = actionInfo.Action as BaseBuilding;
        if(baseBuilding is RecruitBuilding)
        {
            ((RecruitBuilding)baseBuilding).CancellAllUnits();
        }
        baseBuilding.RepairBuilding();
    }
}
