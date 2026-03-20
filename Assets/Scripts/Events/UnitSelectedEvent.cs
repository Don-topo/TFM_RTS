using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UnitSelectedEvent", menuName = "Events/Unit Selected")]
public class UnitSelectedEvent : GameEvent<CommonActions>
{
    public ISelectable Unit { get; private set; }

}
