using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private UIActions uiActions;
    [SerializeField] private UIUnitBaseInfo uiUnitBase;
    [SerializeField] private UISingleUnit uiSingleUnit;
    [SerializeField] private UIMultipleUnits uIMultipleUnits;
    [SerializeField] private UISelectedBuilding uiSelectedBuilding;

    public UnitSelectedEvent unitSelectedEvent;
    public UnitDeselectEvent unitDeselectEvent;
    public RefreshUIEvent refreshUIEvent;
    private List<ISelectable> selectedUnits = new List<ISelectable>(12);
    
    private void Awake()
    {
        unitSelectedEvent.Register(UnitSelected);
        unitDeselectEvent.Register(UnitDeselected);
        refreshUIEvent.Register(HardRefreshByEvent);
    }

    private void Start()
    {
        uiActions.DisableActionButtons();
        uiUnitBase.Disable();
        uiSingleUnit.Disable();
        uIMultipleUnits.Disable();
        uiSelectedBuilding.Disable();
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

    private void HardRefreshByEvent(bool refresh)
    {
        UpdateUI();
    }

    private void UpdateUI()
    {
        if(selectedUnits.Count > 0)
        {
            if(selectedUnits.Count == 1)
            {
                if(selectedUnits.First() is BaseBuilding)
                {
                    CommonActions selectedUnit = (CommonActions)selectedUnits.First();
                    uiUnitBase.Enable((CommonActions)selectedUnits.First());
                    uiSelectedBuilding.Enable((BaseBuilding)selectedUnits.First());
                    List<CommonActions> list = new() { selectedUnit };
                    uiActions.EnableActionButtons(list);
                }
                else
                {
                    CommonActions selectedUnit = (CommonActions)selectedUnits.First();
                    uiUnitBase.Enable(selectedUnit);
                    uiSingleUnit.Enable(selectedUnit);
                    List<CommonActions> list = new() { selectedUnit };
                    uiActions.EnableActionButtons(list);
                }                
            }
            else
            {
                uiUnitBase.Disable();
                uiSingleUnit.Disable();
                List<CommonActions> list = new();
                list.AddRange(selectedUnits);
                uIMultipleUnits.Enable(list);
                uiSelectedBuilding.Disable();
            }                        
        }
        else
        {
            uiUnitBase.Disable();
            uiSingleUnit.Disable();
            uiActions.DisableActionButtons();
            uIMultipleUnits.Disable();
            uiSelectedBuilding.Disable();
        }
    }
}
