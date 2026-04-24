using UnityEngine;

[CreateAssetMenu(fileName = "Build Building", menuName = "Building/Action/Build Building")]
public class BuildBuildingAction : BaseAction
{
    public override bool Blocked(ActionInfo actionInfo) => false;

    public override bool CanExecute(ActionInfo actionInfo)
    {
        // Check if the player has enought resources
        // Check if the build placement requirements are fullfiled
        return true;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        BaseBuilding baseBuilding = actionInfo.Action as BaseBuilding;
        baseBuilding.BuildBuilding();
    }
}
