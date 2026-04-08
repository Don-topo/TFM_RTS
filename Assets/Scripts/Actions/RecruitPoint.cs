using UnityEngine;

[CreateAssetMenu(fileName = "Recruit Point", menuName = "Buildings/Actions/Set Recruit Unit Destination Point", order = 101)]
public class RecruitPoint : BaseAction
{
    [field: SerializeField] public LayerMask IgnoreTerrain { get; private set; }

    public override bool Blocked(ActionInfo actionInfo) => false;

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action is RecruitBuilding;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        RecruitBuilding recruitBuilding = actionInfo.Action as RecruitBuilding;
        if ((IgnoreTerrain.value & (1 << recruitBuilding.gameObject.layer)) == 0){
            // Check if the selected position is valid
            recruitBuilding.SetRecruitDestination(actionInfo.Hit.point);
        }        
    }
}
