using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.GridLayoutGroup;

public class CommonActions : MonoBehaviour, ISelectable
{
    [field: SerializeField] public bool IsSelected { get; protected set; }

    [field: SerializeField] protected DecalProjector selectionDecal;
    [field: SerializeField] public BaseAction[] Actions { get; private set; }
    [field: SerializeField] public SO_BaseUnit so_baseUnit { get; protected set; }
    // Base Unit Events
    public UnitSelectedEvent unitSelectEvent;
    public UnitDeselectEvent unitDeselectEvent;

    private BaseAction[] initialActions;

    protected virtual void Start()
    {

    }

    public void Deselect()
    {
        // Safety check to avoid errors
        if(selectionDecal != null)
        {
            selectionDecal.gameObject.SetActive(false);
        }
        IsSelected = false;
        // Send notification
        unitDeselectEvent.Raise(this);
    }

    public void Select()
    {
        // Safety check to avoid errors
        if(selectionDecal != null)
        {
            selectionDecal.gameObject.SetActive(true);
        }
        IsSelected = true;
        // Send notification
        unitSelectEvent.Raise(this);
    }

    public void SetCommandsOverrides(BaseAction[] actions)
    {
        if (actions == null || actions.Length == 0)
        {
            Actions = actions;
        }
        else
        {
            Actions = actions;
        }

        if (IsSelected)
        {
            unitSelectEvent.Raise(this);
        }
    }
}
