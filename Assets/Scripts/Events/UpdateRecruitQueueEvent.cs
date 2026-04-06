using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpdateRecruitQueueEvent", menuName = "Events/Update Recruit Queue")]
public class UpdateRecruitQueueEvent : GameEvent<List<SO_BaseUnit>>
{
    public List<SO_BaseUnit> UnitsInQueue {  get; private set; }
}
