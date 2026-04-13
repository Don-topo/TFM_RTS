using UnityEngine;

[CreateAssetMenu(fileName = "UnitRecruitedEvent", menuName = "Events/Unit Recruited")]
public class UnitRecruitedEvent : GameEvent<BaseUnit>
{
    public BaseUnit unit;
}
