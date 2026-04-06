using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseAction : ScriptableObject, IAction
{
    [field: SerializeField] public string Name { get; private set; } = "Command Name";
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Key HotKey { get; private set; } = Key.None;
    [field: Range(-1, 8)][field: SerializeField] public int UIPosition { get; private set; }
    [field: SerializeField] public bool IsSingleUnitCommand { get; private set; }
    [field: SerializeField] public bool UseClickToExecute { get; private set; }

    public abstract bool CanExecute(ActionInfo actionInfo);
    public abstract bool Blocked(ActionInfo actionInfo);

    public abstract void Execute(ActionInfo actionInfo);
}
