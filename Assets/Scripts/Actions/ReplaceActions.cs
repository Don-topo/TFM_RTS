using UnityEngine;

[CreateAssetMenu(fileName = "Override Commands", menuName = "Units/Commands/Override Commands", order = 110)]
public class ReplaceActions : BaseAction
{
    [field: SerializeField] public BaseAction[] Commands { get; private set; }

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action != null;
    }

    public override bool Blocked(ActionInfo actionInfo) => false;

    public override void Execute(ActionInfo actionInfo)
    {
        actionInfo.Action.SetCommandsOverrides(Commands);
    }
}