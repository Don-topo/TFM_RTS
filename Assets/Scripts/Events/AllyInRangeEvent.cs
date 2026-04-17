using UnityEngine;

[CreateAssetMenu(fileName = "AllyInRangeEvent", menuName = "Events/Ally In Range")]
public class AllyInRangeEvent : GameEvent<IHealable>
{
    public IHealable Unit { get; private set; }
}
