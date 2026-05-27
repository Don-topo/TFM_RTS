using UnityEngine;
using UnityEngine.UIElements;

[CreateAssetMenu(fileName = "Build Building", menuName = "Building/Action/Build Building")]
public class BuildBuildingAction : BaseAction
{
    [field: SerializeField] public SO_Building BuildingToBuild { get; private set; }
    [field: SerializeField] public GameObject PlaceBuilding {  get; private set; }


    public override bool Blocked(ActionInfo actionInfo)
    {
        if(actionInfo.Action == null) return false;
        if(actionInfo.Action.TryGetComponent<BaseBuilding>(out BaseBuilding building))
        {
            return !(CheckIfThereIsAvailableResources(actionInfo) && building.IsConstructed && !building.IsRepairing);
        }
        else
        {
            return false;
        }        
    }

    public override bool CanExecute(ActionInfo actionInfo)
    {
        BaseBuilding baseBuilding = actionInfo.Action as BaseBuilding;
        if (baseBuilding == null) return false;

        // Check if the player has enought resources
        // Check if the build placement requirements are fullfiled
        return CheckIfThereIsAvailableResources(actionInfo) && 
            CheckRestrictions(actionInfo.Hit.point) && 
            baseBuilding.IsConstructed && 
            !baseBuilding.IsRepairing;
    }

    public override void Execute(ActionInfo actionInfo)
    {       
        BaseBuilding baseBuilding = actionInfo.Action as BaseBuilding;   
        baseBuilding.ResetActions();
        if (CheckIfThereIsAvailableResources(actionInfo))
        {
            Instantiate(BuildingToBuild.UnitPrefab, actionInfo.Hit.point, Quaternion.identity);
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
