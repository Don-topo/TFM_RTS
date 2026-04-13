using UnityEngine;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.GridLayoutGroup;

public abstract class CommonActions : MonoBehaviour, ISelectable, IAttackable
{
    [field: SerializeField] public bool IsSelected { get; protected set; }

    [field: SerializeField] protected DecalProjector selectionDecal;
    [field: SerializeField] public BaseAction[] Actions { get; private set; }
    [field: SerializeField] public SO_BaseUnit SO_BaseUnit { get; protected set; }
    [field: SerializeField] public int CurrentHealth { get; protected set; }
    [field: SerializeField] public int MaxHealth {  get; protected set; }

    public Transform TargetPosition => transform;

    // Base Unit Events
    public UnitSelectedEvent unitSelectEvent;
    public UnitDeselectEvent unitDeselectEvent;

    private BaseAction[] initialActions;

    protected virtual void Start()
    {

    }

    protected virtual void Awake()
    {

    }

    protected virtual void Update()
    {
        
    }

    protected virtual void OnDestroy()
    {

    }

    public virtual void Deselect()
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

    public virtual void Select()
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

    public void ApplyDamage(int damageAmount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth - damageAmount, 0, CurrentHealth);
        if(CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Die()
    {
        Destroy(gameObject);
    }
}
