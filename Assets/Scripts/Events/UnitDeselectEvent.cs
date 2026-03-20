using UnityEngine;

[CreateAssetMenu(fileName = "UnitDeselectedEvent", menuName = "Events/Unit Deselected")]
public class UnitDeselectEvent : GameEvent<CommonActions>
{
    public ISelectable Unit { get; private set; }
}
