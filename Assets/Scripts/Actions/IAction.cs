using UnityEngine;

public interface IAction
{
    public bool IsSingleUnitAction { get; }
    bool CanExecute(ActionInfo actionInfo);
    void Execute(ActionInfo actionInfo);
}
