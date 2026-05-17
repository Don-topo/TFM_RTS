using System;
using UnityEngine.InputSystem;

[Serializable]
public class UnitGroup
{
    public Key HotKey;
    public UIUnitGroup UIUnitGroup;

    public UnitGroup(Key hotKey, UIUnitGroup uiUnitGroup)
    {
        HotKey = hotKey;
        UIUnitGroup = uiUnitGroup;
    }
}
