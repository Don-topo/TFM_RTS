using System.Runtime.CompilerServices;
using Unity.Behavior;
using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Buildings/Building")]
public class BaseBuilding : CommonActions, IHealable
{
    [Header("Placement Materials")]
    [field: SerializeField] public Material PlaceMaterial { get; private set; }
    [Header("Events")]
    [SerializeField] protected ResourceEvent resourceEvent;
    [Header("Info")]
    [field: SerializeField] public SO_Building SO_building { get; private set; }
    [field: SerializeField] public BehaviorGraphAgent GraphAgent { get; private set; }
    [field: SerializeField] public bool IsConstructed { get; set; } = false;
    [SerializeField] private RefreshUIEvent refreshEvent;

    public float StartTime;

    private Renderer[] baseRenderers;
    private Material[] initialMaterials;
    
    protected override void Start()
    {
        base.Start();
        if (!IsConstructed)
        {
            SaveMaterials();
        }
        CurrentHealth = IsConstructed ? SO_BaseUnit.Health : 1;          
        MaxHealth = SO_building.Health;
        GraphAgent.SetVariableValue<SO_Building>("SO Building", SO_building);
        GraphAgent.SetVariableValue<BaseBuilding>("Base Building", this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void BuildBuilding(Vector3 targetPosition)
    {
        // Spend Resources
        ResourceOP resourceOP = new ResourceOP(SO_building.Cost.SO_Food, -SO_building.Cost.Food, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Wood, -SO_building.Cost.Wood, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Stone, -SO_building.Cost.Stone, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Iron, -SO_building.Cost.Iron, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Electricity, SO_building.Cost.Electricity, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Population, SO_building.Cost.Population, 0);
        resourceEvent.Raise(resourceOP);
        GraphAgent.SetVariableValue("BuildingActions", BuildingActions.Build);
        GraphAgent.SetVariableValue<Vector3>("TargetPosition", targetPosition);
    }

    public virtual void DestroyBuilding()
    {        
        // Refund spend resources based on building state and health
        float refund = CalculateRefund();
        // Return build resources
        ResourceOP resourceOP = new ResourceOP(SO_building.Cost.SO_Food, SO_building.Cost.Food, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Wood, SO_building.Cost.Wood, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Stone, SO_building.Cost.Stone, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Iron, SO_building.Cost.Iron, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Electricity, -SO_building.Cost.Electricity, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(SO_building.Cost.SO_Population, -SO_building.Cost.Population, 0);
        resourceEvent.Raise(resourceOP);

        // TODO Play explotion
        // Destroy game object
        Destroy(gameObject);
    }

    private float CalculateRefund()
    {
        float refund = 1.0f;
        if (IsConstructed) return refund;

        refund = (refund * CurrentHealth) / MaxHealth;

        return refund;
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        RefreshUI();
    }

    public void RefreshUI()
    {
        refreshEvent.Raise(true);
    }

    private void SaveMaterials()
    {
        baseRenderers = GetComponentsInChildren<Renderer>();
        initialMaterials = new Material[baseRenderers.Length];
        for (int i = 0; i < baseRenderers.Length; i++)
        {
            initialMaterials[i] = baseRenderers[i].material;
            baseRenderers[i].material = PlaceMaterial;
        }
    }

    public void RestoreMaterials()
    {
        if(initialMaterials  != null && baseRenderers != null && initialMaterials.Length == baseRenderers.Length)
        {
            for(int i = 0; i < baseRenderers.Length; i++)
            {
                baseRenderers[i].material = initialMaterials[i];
            }
        }
    }
}
