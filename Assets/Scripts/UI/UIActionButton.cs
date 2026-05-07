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
            tooltip.SetTooltipText(GetCommandTooltipText(action));
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
                    rectTransform.position.x + rectTransform.rect.width / 2f,
                    rectTransform.position.y + rectTransform.rect.height / 2f);
        }        
    }

    private String GetCommandTooltipText(BaseAction action)
    {
        string tooltipText;
        tooltipText = action.name;
        tooltipText += action.HotKey.ToString();

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
