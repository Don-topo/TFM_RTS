using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

[RequireComponent (typeof(Button))]
public class UIActionButton : MonoBehaviour
{
    [SerializeField] private Image actionIcon;

    private Button button;
    private Key hotkey;
    private bool assignedThisFrame;

    private void Awake()
    {
        button = GetComponent<Button>();
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
        // Safety action to update if previous actions are displayed
        button.onClick.RemoveAllListeners();
        SetIcon(action.Icon);
        hotkey = action.HotKey;
        button.onClick.AddListener(unityAction);
        button.interactable = true;
        assignedThisFrame = true;
    }

    public void Disable()
    {
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
}
