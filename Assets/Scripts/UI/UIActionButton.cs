using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class UIActionButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image actionIcon;
    [SerializeField] private Tooltip tooltip;
    [SerializeField] private ResourceEvent resourceEvent;

    private Button button;
    private Key hotkey;
    private RectTransform rectTransform;
    private bool assignedThisFrame;

    // Update
    BaseAction action;
    List<CommonActions> commonActions = new List<CommonActions>();

    private static readonly string FOOD_FORMAT = "{0} <color=#F54927>Food</color>\n";
    private static readonly string ELECTICITY_FORMAT = "{0} <color=#E5F200>Electricity</color>\n";
    private static readonly string WOOD_FORMAT = "{0} <color=#6E3300>Wood</color>\n";
    private static readonly string IRON_FORMAT = "{0} <color=##242424>Iron</color>\n";
    private static readonly string STONE_FORMAT = "{0} <color=#C7C7C7>Stone</color>\n";
    private static readonly string POPULATION_FORMAT = "{0} <color=#B500B5>Population</color>\n";
    private static readonly string HOTKEY_FORMAT = "(<color=#FFFF00>{0}</color>)\n";

    private void Awake()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        Disable();      
    }

    private void Update()
    {
        if (button.interactable && Keyboard.current[hotkey].wasReleasedThisFrame && hotkey != Key.None && !assignedThisFrame)
        {
            button.onClick?.Invoke();
        }
        assignedThisFrame = false;
    }

    public void Enable(BaseAction action, List<CommonActions> commonActions, UnityAction unityAction)
    {
        resourceEvent.Register(HandleResourceEvent);
        this.action = action;
        this.commonActions = commonActions;
        // Safety action to update if previous actions are displayed
        button.onClick.RemoveAllListeners();
        SetIcon(action.Icon);
        hotkey = action.HotKey;
        button.onClick.AddListener(unityAction);
        UpdateInteractable();
        assignedThisFrame = true;
        if(tooltip != null)
        {
            tooltip.SetTooltipText(GetActionTooltipText(action));
        }        
    }

    public void Disable()
    {
        resourceEvent.Unregister(HandleResourceEvent);
        // Empty sprite
        SetIcon(null);
        // Button not interactable
        button.interactable = false;
        button.onClick.RemoveAllListeners();
        if (tooltip != null)
        {
            tooltip.HideTooltip();
        }
        // Stop al calls
        CancelInvoke();
    }

    public void SetIcon(Sprite icon)
    {
        if(icon == null)
        {
            actionIcon.enabled = false;
        }
        else
        {
            actionIcon.enabled = true;
            actionIcon.sprite = icon;
        }        
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Invoke(nameof(ShowTooltip), tooltip.ShowWaitTime);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if(tooltip != null)
        {
            tooltip.HideTooltip();
        }
        CancelInvoke();
    }

    private void ShowTooltip()
    {
        if(tooltip != null)
        {
            tooltip.ShowTooltip();
            tooltip.RectTransform.position = new Vector2(
                    rectTransform.position.x - rectTransform.rect.width / 2f,
                    rectTransform.position.y + rectTransform.rect.height / 2f);
        }        
    }

    private String GetActionTooltipText(BaseAction action)
    {
        string tooltipText;
        SO_ResourceCost cost = null;
        // Add Action name
        tooltipText = action.Name;
        // Add Action Hotkey
        if(action.HotKey != Key.None)
        {
            tooltipText += string.Format(HOTKEY_FORMAT, action.HotKey);
        }
        else
        {
            tooltipText += "\n";
        }
        // Add cost info
        if (action is BuildBuildingAction building)
        {
            cost = building.BuildingToBuild.Cost;            
        }
        else if(action is RecruitUnitAction unit)
        {
            cost = unit.UnitToBuild.Cost;
        }
        
        if(cost != null)
        {
            if(cost.Food > 0)
            {
                tooltipText += string.Format(FOOD_FORMAT, cost.Food);
            }
            if (cost.Electricity > 0)
            {
                tooltipText += string.Format(ELECTICITY_FORMAT, cost.Electricity);
            }
            if (cost.Wood > 0)
            {
                tooltipText += string.Format(WOOD_FORMAT, cost.Wood);
            }
            if (cost.Iron > 0)
            {
                tooltipText += string.Format(IRON_FORMAT, cost.Iron);
            }
            if (cost.Stone > 0)
            {
                tooltipText += string.Format(STONE_FORMAT, cost.Stone);
            }
            if (cost.Population > 0)
            {
                tooltipText += string.Format(POPULATION_FORMAT, cost.Population);
            }
        }

        return tooltipText;
    }

    private void UpdateInteractable()
    {
        button.interactable = this.commonActions
            .Any(commandable => !this.action.Blocked(new ActionInfo(commandable, new RaycastHit(), this.action.UIPosition)));
    }

    private void HandleResourceEvent(ResourceOP resource)
    {
        if(this.commonActions.Count != 0 && action != null)
        {
            UpdateInteractable();
        }        
    }
}
