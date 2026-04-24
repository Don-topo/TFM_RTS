using UnityEngine;

[CreateAssetMenu(fileName = "Destroy Building", menuName = "Units/Commands/Destroy Building")]
public class DestroyBuildingAction : BaseAction
{
    public override bool Blocked(ActionInfo actionInfo) => false;

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action is BaseBuilding;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        if(actionInfo.Action is BaseBuilding building)
        {
            building.DestroyBuilding();
        }
    }
}
