using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIActions uiActions;
    [SerializeField] private UIUnitBaseInfo uiUnitBase;
    [SerializeField] private UISingleUnit uiSingleUnit;

    public UnitSelectedEvent unitSelectedEvent;
    public UnitDeselectEvent unitDeselectEvent;
    private List<ISelectable> selectedUnits = new List<ISelectable>(12);
    
    private void Awake()
    {
        unitSelectedEvent.Register(UnitSelected);
        unitDeselectEvent.Register(UnitDeselected);
    }

    private void Start()
    {
        //UIActions.DisableActionButtons();
        uiUnitBase.Disable();
        uiSingleUnit.Disable();
    }

    private void OnDestroy()
    {
        unitSelectedEvent.Unregister(UnitSelected);
        unitDeselectEvent.Unregister(UnitDeselected);
    }

    private void UnitSelected(CommonActions unitSelected)
    {
        selectedUnits.Add(unitSelected);
        UpdateUI();
    }

    private void UnitDeselected(CommonActions unitDeselect)
    {
        selectedUnits.Remove(unitDeselect);
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(selectedUnits.Count == 1)
        {
            CommonActions selectedUnit = (CommonActions)selectedUnits.First();
            uiUnitBase.Enable(selectedUnit);
            uiSingleUnit.Enable(selectedUnit);
        }
        else
        {
            uiUnitBase.Disable();
            uiSingleUnit.Disable();
        }
    }
}
