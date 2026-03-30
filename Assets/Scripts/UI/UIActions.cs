using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class UIActions : MonoBehaviour
{
    // Events
    [SerializeField] ActionClicked actionClickedEvent;

    [SerializeField] private UIActionButton[] uIActionButtons;

    public void EnableActionButtons(List<CommonActions> unitsSelected)
    {
        UpdateActionsUIButtons(unitsSelected);
    }

    public void DisableActionButtons()
    {
        foreach(UIActionButton actionButton in uIActionButtons)
        {
            actionButton.Disable();
        }
    }

    private UnityAction Click(BaseAction action)
    {
        return () => actionClickedEvent.Raise(action);
    }

    private void UpdateActionsUIButtons(List<CommonActions> unitsSelected)
    {
        List<BaseAction> baseActions = 
            unitsSelected.Count > 0 ? unitsSelected.ElementAt(0).Actions.ToList() : new List<BaseAction>();

        if(baseActions.Count == 0) return;

        foreach(CommonActions unitSelected in unitsSelected)
        {
            if(unitSelected.Actions != null)
            {
                baseActions = baseActions.Intersect(unitSelected.Actions.ToList()).ToList();
            }
        }

        for(int i = 0; i < baseActions.Count; i++)
        {
            uIActionButtons[baseActions[i].UIPosition].Enable(baseActions[i], unitsSelected, Click(baseActions[i]));            
        }
    }
}
