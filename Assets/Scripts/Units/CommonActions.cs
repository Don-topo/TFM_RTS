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
    [SerializeField] protected Animator animator;
    [Header("Events")]
    [SerializeField] protected UpdateHealthEvent updateHealthEvent;
    [SerializeField] protected UnitDeathEvent unitDeathEvent;

    public Transform TargetPosition => transform;

    // Base Unit Events
    public UnitSelectedEvent unitSelectEvent;
    public UnitDeselectEvent unitDeselectEvent;
    private BaseAction[] startingActions;
    public bool isDead { get; protected set; } = false;
    private Collider col;
    private Rigidbody rb;

    protected virtual void Start()
    {
        startingActions = SO_BaseUnit.UnitPrefab.GetComponent<CommonActions>().Actions;        
    }

    protected virtual void Awake()
    {
        startingActions = SO_BaseUnit.UnitPrefab.GetComponent<CommonActions>().Actions;
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
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
        if (isDead) return;

        // Safety check to avoid errors
        if(selectionDecal != null)
        {
            selectionDecal.gameObject.SetActive(true);
        }
        IsSelected = true;
        PlaySelectionAudio();
        // Send notification
        unitSelectEvent.Raise(this);
    }

    public void SetCommandsOverrides(BaseAction[] actions)
    {
        if (actions == null || actions.Length == 0)
        {
            Actions = startingActions;
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
        if(isDead) return;

        CurrentHealth = Mathf.Clamp(CurrentHealth - damageAmount, 0, CurrentHealth);
        if (IsSelected)
        {
            updateHealthEvent.Raise(this);
        }        
        if(CurrentHealth <= 0)
        {
            isDead = true;
            Die();
        }
    }

    public virtual void Die()
    {
        if(animator != null)
        {
            animator.SetTrigger("Death");           
        }

        col.enabled = false;
        rb.isKinematic = true;
        rb.detectCollisions = false;
        Deselect();
        unitDeathEvent.Raise(this);
        float delayTime = this is BaseBuilding ? 0f : 3f;
        Destroy(gameObject, delayTime);
    }

    private void PlaySelectionAudio()
    {
        if(SO_BaseUnit.SelectionAudioClips.Count > 0)
        {
            AudioManager.SetAudioClips(SO_BaseUnit.SelectionAudioClips);
            AudioManager.PlayAudio();
        }
    }
}
