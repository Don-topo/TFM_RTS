using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIUnitGroup : MonoBehaviour
{
    [Header("Component Info")]
    [SerializeField] private Image icon;
    [SerializeField] private TextMeshProUGUI numKeyGroup;
    [SerializeField] private TextMeshProUGUI numOfUnitsInGroup;
    [Header("Events")]
    [SerializeField] private UnitDeathEvent unitDeathEvent;

    private Button button;
    private List<CommonActions> unitsInGroup;
    private Key key;
    private UnityAction action;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void Update()
    {
        if (Keyboard.current[key].wasReleasedThisFrame)
        {
            action?.Invoke();
        }
    }

    private void OnEnable()
    {
        unitDeathEvent.Register(UnitDeath);
    }

    private void OnDestroy()
    {
        unitDeathEvent.Unregister(UnitDeath);
    }

    public void Enable(List<CommonActions> units, Key key, UnityAction unityAction)
    {
        unitsInGroup = units;
        this.key = key;
        action = unityAction;
        gameObject.SetActive(true);

        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(unityAction);

        SetInfo();
    }

    public void Disable()
    {
        button.onClick.RemoveAllListeners();
        unitDeathEvent.Unregister(UnitDeath);
        gameObject.SetActive(false);
    }   

    private void SetInfo()
    {
        numOfUnitsInGroup.SetText(unitsInGroup.Count.ToString());
        icon.sprite = unitsInGroup.First().SO_BaseUnit.Icon;
    }

    private void UnitDeath(CommonActions unit)
    {
        unitsInGroup.Remove(unit);
        if(unitsInGroup.Count == 0)
        {
            Disable();
            return;
        }

        SetInfo();
    }
}
