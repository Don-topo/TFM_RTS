using UnityEngine;

[CreateAssetMenu(fileName = "UnitInRangeEvent", menuName = "Events/Unit In Range")]
public class UnitInRangeEvent : GameEvent<IAttackable>
{
    public IAttackable Unit { get; private set; }
}
