using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Recruit Unit", menuName = "Buildings/Actions/Recruit Unit", order = 100)]
public class RecruitUnitAction : BaseAction
{
    [field: SerializeField] public SO_BaseUnit UnitToBuild { get; private set; }

    public override bool Blocked(ActionInfo actionInfo)
    {
        BaseBuilding buildling = actionInfo.Action as BaseBuilding;
        return !CheckIfThereIsAvailableResources(actionInfo) || (buildling != null && !buildling.IsConstructed);
    }

    public override bool CanExecute(ActionInfo actionInfo)
    {
        BaseBuilding buildling = actionInfo.Action as BaseBuilding;
        bool a = CheckIfThereIsAvailableResources(actionInfo) && buildling != null && buildling.IsConstructed;
        return CheckIfThereIsAvailableResources(actionInfo) && buildling != null && buildling.IsConstructed;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        RecruitBuilding building = (RecruitBuilding)actionInfo.Action;
        bool t = !CheckIfThereIsAvailableResources(actionInfo) || (building != null && !building.IsConstructed);
        // Check if there is enought resources
        if (!CheckIfThereIsAvailableResources(actionInfo) && (building != null && !building.IsConstructed)) return;

        // Build Unit
        building.RecruitUnit(UnitToBuild);
    }

    private bool CheckIfThereIsAvailableResources(ActionInfo actionInfo)
    {
        return UnitToBuild.Cost.Food <= UIResources.Food
            && UnitToBuild.Cost.Wood <= UIResources.Wood
            && UnitToBuild.Cost.Stone <= UIResources.Stone
            && UnitToBuild.Cost.Iron <= UIResources.Iron
            && UnitToBuild.Cost.Population + UIResources.Population <= UIResources.MaxPopulation;
    }
}
