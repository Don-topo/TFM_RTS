using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Recruit Unit", menuName = "Buildings/Actions/Recruit Unit", order = 100)]
public class RecruitUnitAction : BaseAction
{
    [field: SerializeField] public BaseUnit UnitToBuild { get; private set; }

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return CheckIfThereIsAvailableResources(actionInfo) && actionInfo.Action is BaseBuilding;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        RecruitBuilding building = (RecruitBuilding)actionInfo.Action;

        // Check if there is enought resources
        if (!CheckIfThereIsAvailableResources(actionInfo)) return;

        // Build Unit
        building.RecruitUnit();
    }

    private bool CheckIfThereIsAvailableResources(ActionInfo actionInfo)
    {
        return UnitToBuild.SO_BaseUnit.Cost.Food <= UIResources.Food
            && UnitToBuild.SO_BaseUnit.Cost.Wood <= UIResources.Wood
            && UnitToBuild.SO_BaseUnit.Cost.Stone <= UIResources.Stone
            && UnitToBuild.SO_BaseUnit.Cost.Iron <= UIResources.Iron
            && UnitToBuild.SO_BaseUnit.Cost.Electricity <= UIResources.Electricity;
    }
}
