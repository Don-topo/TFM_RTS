using UnityEngine;

[CreateAssetMenu(fileName = "EnemyInRangeEvent", menuName = "Events/Enemy In Range")]
public class EnemyInRangeEvent : GameEvent<IAttackable>
{
    public IAttackable Unit { get; private set; }
}
