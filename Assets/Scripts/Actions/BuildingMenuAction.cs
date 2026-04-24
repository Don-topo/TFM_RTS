using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Building Menu", menuName = "Building/Actions/BuildingMenu", order = 110)]
public class BuildingMenuAction : BaseAction
{
    [field: SerializeField] public List<BaseAction> Actions { get; private set; }

    public override bool Blocked(ActionInfo actionInfo) => false;

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action != null;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        actionInfo.Action.SetCommandsOverrides(Actions.ToArray());
    }
}
