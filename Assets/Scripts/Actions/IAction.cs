using UnityEngine;

public interface IAction
{
    public bool IsSingleUnitCommand { get; }
    bool CanExecute(ActionInfo actionInfo);
    void Execute(ActionInfo actionInfo);
}
