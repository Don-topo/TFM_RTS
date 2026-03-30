using UnityEngine;

[CreateAssetMenu(fileName = "UnitDeathEvent", menuName = "Events/Unit Death")]
public class UnitDeathEvent : GameEvent<BaseUnit>
{
    public BaseUnit deathUnit {  get; private set; }
}
