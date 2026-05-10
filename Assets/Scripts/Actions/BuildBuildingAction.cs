using UnityEngine;

[CreateAssetMenu(fileName = "Build Building", menuName = "Building/Action/Build Building")]
public class BuildBuildingAction : BaseAction
{
    [field: SerializeField] public SO_Building BuildingToBuild { get; private set; }
    [field: SerializeField] public GameObject PlaceBuilding {  get; private set; }


    public override bool Blocked(ActionInfo actionInfo)
    {
        return !CheckIfThereIsAvailableResources(actionInfo);
    }

    public override bool CanExecute(ActionInfo actionInfo)
    {
        BaseBuilding baseBuilding = actionInfo.Action as BaseBuilding;
        if (baseBuilding == null) return false;

        // Check if the player has enought resources
        // Check if the build placement requirements are fullfiled
        return !CheckIfThereIsAvailableResources(actionInfo);
    }

    public override void Execute(ActionInfo actionInfo)
    {
        BaseBuilding baseBuilding = actionInfo.Action as BaseBuilding;
        if (CheckIfThereIsAvailableResources(actionInfo))
        {
            baseBuilding.BuildBuilding();
        }        
    }

    private bool CheckIfThereIsAvailableResources(ActionInfo actionInfo)
    {
        return BuildingToBuild.Cost.Food <= UIResources.Food
            && BuildingToBuild.Cost.Wood <= UIResources.Wood
            && BuildingToBuild.Cost.Stone <= UIResources.Stone
            && BuildingToBuild.Cost.Iron <= UIResources.Iron
            && BuildingToBuild.Cost.Population + UIResources.Population <= UIResources.MaxPopulation;
    }
}
