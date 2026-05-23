using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "RepairBuilding", menuName = "Building/Action/Repair Building")]
public class RepairAction : BaseAction
{
    public override bool Blocked(ActionInfo actionInfo)
    {
        BaseBuilding building = actionInfo.Action.GetComponent<BaseBuilding>();
        return !(actionInfo.Action.CurrentHealth < actionInfo.Action.SO_BaseUnit.Health && building.IsConstructed && !building.IsRepairing);
    }

    public override bool CanExecute(ActionInfo actionInfo)
    {
        BaseBuilding building = actionInfo.Action.GetComponent<BaseBuilding>();
        return actionInfo.Action.CurrentHealth < actionInfo.Action.SO_BaseUnit.Health && building.IsConstructed && !building.IsRepairing;
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
