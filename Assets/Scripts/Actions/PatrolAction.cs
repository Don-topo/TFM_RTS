using UnityEngine;

[CreateAssetMenu(fileName = "Patrol", menuName = "Units/Actions/Patrol", order = 100)]
public class PatrolAction : BaseAction
{
    public override bool Blocked(ActionInfo actionInfo) => false;

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action is BaseAttacker;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        BaseAttacker attacker = (BaseAttacker)actionInfo.Action;
        if(attacker != null)
        {
            
        }
    }
}
