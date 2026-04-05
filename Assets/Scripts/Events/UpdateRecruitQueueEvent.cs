using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpdateRecruitQueueEvent", menuName = "Events/Update Recruit Queue")]
public class UpdateRecruitQueueEvent : GameEvent<List<BaseUnit>>
{
    public List<BaseUnit> UnitsInQueue {  get; private set; }
}
