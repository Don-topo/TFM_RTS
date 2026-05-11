using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class BaseAction : ScriptableObject, IAction
{
    [field: SerializeField] public string Name { get; private set; } = "Command Name";
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public Key HotKey { get; private set; } = Key.None;
    [field: Range(-1, 8)][field: SerializeField] public int UIPosition { get; private set; }
    [field: SerializeField] public bool IsSingleUnitAction { get; private set; }
    [field: SerializeField] public bool UseClickToExecute { get; private set; }
    [field: SerializeField] public List<AudioClip> ExecuteAudio {  get; private set; }
    [field: SerializeField] public Restriction[] Restrictions { get; private set; }

    public abstract bool CanExecute(ActionInfo actionInfo);
    public abstract bool Blocked(ActionInfo actionInfo);
    public bool AllRestrictionsPass(Vector3 point) =>
        Restrictions.Length == 0 || Restrictions.All(restriction => restriction.CanPlace(point));
    public abstract void Execute(ActionInfo actionInfo);
}
