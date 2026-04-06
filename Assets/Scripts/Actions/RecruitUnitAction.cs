using Unity.VisualScripting;
using UnityEngine;

[CreateAssetMenu(fileName = "Recruit Unit", menuName = "Buildings/Actions/Recruit Unit", order = 100)]
public class RecruitUnitAction : BaseAction
{
    [field: SerializeField] public SO_BaseUnit UnitToBuild { get; private set; }

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
        building.RecruitUnit(UnitToBuild);
    }

    private bool CheckIfThereIsAvailableResources(ActionInfo actionInfo)
    {
        return UnitToBuild.Cost.Food + UIResources.Food <= UIResources.MaxFood
            && UnitToBuild.Cost.Wood <= UIResources.Wood
            && UnitToBuild.Cost.Stone <= UIResources.Stone
            && UnitToBuild.Cost.Iron <= UIResources.Iron
            && UnitToBuild.Cost.Population + UIResources.Population <= UIResources.MaxPopulation;
    }
}
