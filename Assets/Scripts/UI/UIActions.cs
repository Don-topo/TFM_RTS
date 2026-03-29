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
        BaseAction[] baseActions = 
            unitsSelected.Count > 0 ? unitsSelected.ElementAt(0).Actions : null;

        if(baseActions.Length == 0) return;

        //

        for(int i = 0; i < baseActions.Length; i++)
        {
            uIActionButtons[baseActions[i].UIPosition].Enable(baseActions[i], unitsSelected, Click(baseActions[i]));            
        }
    }
}
