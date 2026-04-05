using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent(typeof(GridLayoutGroup))]
public class UIMultipleUnits : MonoBehaviour
{
    [SerializeField] private UIUnitSelectedButton unitButtonPrefab;
    [SerializeField] private int maxUnitSelectedSupported = 40;
    [SerializeField] private int columns = 4;

    private GridLayoutGroup gridLayoutGroup;
    private List<CommonActions> unitsSelected = new();
    private List<UIUnitSelectedButton> unitsSelectedButtons = new();

    private void Awake()
    {
        gridLayoutGroup = GetComponent<GridLayoutGroup>();
    }
    public void Enable(List<CommonActions> unitsSelected)
    {
        gameObject.SetActive(true);
      
        foreach(CommonActions unitSelected in unitsSelected)
        {
            // Create a new Button for each unit selected
            UIUnitSelectedButton newButton = Instantiate(unitButtonPrefab, transform);
            // Set unit image
            newButton.GetComponent<Image>().sprite = unitSelected.SO_BaseUnit.Icon;
            // Fill button
            newButton.Enable(unitSelected, () => HandleClick(unitSelected));
            
            // Adapt button size
            unitsSelectedButtons.Add(newButton);

        }

        // Save the list for Disable action
        this.unitsSelected = unitsSelected;
    }

    public void Disable()
    {
        foreach(UIUnitSelectedButton unitSelectedButton in unitsSelectedButtons)
        {
            unitSelectedButton.TryGetComponent(out Button button);
            button.onClick.RemoveAllListeners();
            Destroy(unitSelectedButton.gameObject);            
        }

        unitsSelectedButtons.Clear();
        gameObject.SetActive(false);        
    }

    private void HandleClick(CommonActions selectedUnit)
    {
        if (Keyboard.current.shiftKey.isPressed)
        {
            selectedUnit.Deselect();
        }
        else
        {
            // Deselect all selected units
            foreach(CommonActions unit in unitsSelected)
            {
                unit.Deselect();
            }

            // Select the selected unit
            selectedUnit.Select();
        }
    }
}
