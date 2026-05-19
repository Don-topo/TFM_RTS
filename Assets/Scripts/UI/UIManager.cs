using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private UIActions uiActions;
    [SerializeField] private UIUnitBaseInfo uiUnitBase;
    [SerializeField] private UISingleUnit uiSingleUnit;
    [SerializeField] private UIMultipleUnits uIMultipleUnits;
    [SerializeField] private UISelectedBuilding uiSelectedBuilding;
    [SerializeField] private UIUnitGroups uiUnitGroups;
    [SerializeField] private GameObject winMenu;
    [SerializeField] private GameObject defeatMenu;
    [Header("Events")]
    [SerializeField] private UnitSelectedEvent unitSelectedEvent;
    [SerializeField] private UnitDeselectEvent unitDeselectEvent;
    [SerializeField] private RefreshUIEvent refreshUIEvent;
    [SerializeField] private UnitDeathEvent deathEvent;
    [SerializeField] private GameOverEvent gameOverEvent;
    [SerializeField] private VictoryEvent victoryEvent;

    private List<ISelectable> selectedUnits = new List<ISelectable>(12);
    
    private void Awake()
    {
        unitSelectedEvent.Register(UnitSelected);
        unitDeselectEvent.Register(UnitDeselected);
        refreshUIEvent.Register(HardRefreshByEvent);
        deathEvent.Register(DeathUnit);
        victoryEvent.Register(ShowVictory);
        gameOverEvent.Register(ShowDefeat);
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
        deathEvent.Unregister(DeathUnit);
        victoryEvent.Unregister(ShowVictory);
        gameOverEvent.Unregister(ShowDefeat);
    }

    private void UnitSelected(CommonActions unitSelected)
    {
        if (!selectedUnits.Contains(unitSelected))
        {
            selectedUnits.Add(unitSelected);
        }
        UpdateUI();
    }

    private void UnitDeselected(CommonActions unitDeselect)
    {
        selectedUnits.Remove(unitDeselect);
        UpdateUI();
    }

    private void DeathUnit(CommonActions unitDeath)
    {
        selectedUnits.Remove(unitDeath);
        UpdateUI();
    }

    private void HardRefreshByEvent(bool refresh)
    {
        UpdateUI();
    }

    private void ShowVictory(Null @null)
    {
        winMenu.SetActive(true);
    }

    private void ShowDefeat(Null @null)
    {
        defeatMenu.SetActive(true);
    }

    private void UpdateUI()
    {
        List<CommonActions> list1 = new();
        list1.AddRange(selectedUnits);
        uiUnitGroups.Enable(list1);
        if (selectedUnits.Count > 0)
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
