using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class UIUnitGroups : MonoBehaviour
{
    [SerializeField] List<UnitGroup> unitGroups = new List<UnitGroup>();
    private List<CommonActions> unitsSelected;


    private void Update()
    {
        if (Keyboard.current.ctrlKey.IsPressed())
        {
            foreach(UnitGroup unitGroup in unitGroups)
            {
                if (Keyboard.current[unitGroup.HotKey].wasReleasedThisFrame && unitsSelected.Count > 0)
                {
                    unitGroup.UIUnitGroup.Enable(unitsSelected, unitGroup.HotKey, HandleClick(unitsSelected));
                }
            }
        }
    }

    public void Enable(List<CommonActions> units)
    {
        unitsSelected = units;
    }

    private UnityAction HandleClick(List<CommonActions> selectedUnits)
    {
        return () =>
        {
            // Deselect all selected units
            foreach (CommonActions unit in unitsSelected)
            {
                unit.Deselect();
            }

            foreach (CommonActions unit in selectedUnits)
            {
                // Select the selected unit
                unit.Select();
            }
        };
    }
}
