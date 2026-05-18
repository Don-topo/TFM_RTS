using System.Runtime.CompilerServices;
using Unity.Behavior;
using UnityEngine;

public class BaseBuilding : CommonActions, IHealable
{
    [Header("Placement Materials")]
    [field: SerializeField] public Material PlaceMaterial { get; private set; }
    [Header("Events")]
    [SerializeField] protected ResourceEvent resourceEvent;
    [SerializeField] protected RefreshUIEvent refreshEvent;
    [Header("Info")]
    [field: SerializeField] public SO_Building SO_building { get; private set; }
    [field: SerializeField] public BehaviorGraphAgent GraphAgent { get; private set; }
    [field: SerializeField] public bool IsConstructed { get; set; } = false;
    [Header("Effects")]
    [SerializeField] private GameObject explotionPrefab;

    public float StartTime;

    private Renderer[] baseRenderers;
    private Material[] initialMaterials;
    
    protected override void Start()
    {
        base.Start();
        if (!IsConstructed)
        {
            SaveMaterials();
            BuildBuilding();
        }
        CurrentHealth = IsConstructed ? SO_BaseUnit.Health : 1;          
        MaxHealth = SO_building.Health;        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void BuildBuilding()
    {
        GraphAgent.SetVariableValue<SO_Building>("SO Building", SO_building);
        GraphAgent.SetVariableValue<BaseBuilding>("Base Building", this);
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
        GraphAgent.SetVariableValue<Vector3>("TargetPosition", transform.position);
        GraphAgent.SetVariableValue<SO_Building>("BuildToConstruct", SO_building);
    }

    public virtual void DestroyBuilding()
    {        
        // Refund spend resources based on building state and health
        float refund = CalculateRefund();
        // Return build resources
        SO_Resource resourceRefund = SO_building.Cost.SO_Wood;
        //resourceRefund.

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

        // Play explotion
        GameObject explotionInstance = Instantiate(explotionPrefab, transform.position, Quaternion.identity);
        Destroy(explotionInstance, explotionInstance.GetComponent<ParticleSystem>().main.duration);
        // Destroy game object
        Destroy(gameObject);
    }

    private float CalculateRefund()
    {
        float refund = 1.0f;
        if (!IsConstructed) return refund;

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
        if(gameObject != null)
        {
            if (initialMaterials != null && baseRenderers != null && initialMaterials.Length == baseRenderers.Length)
            {
                for (int i = 0; i < baseRenderers.Length; i++)
                {
                    if (baseRenderers[i].material != null)
                    {
                        baseRenderers[i].material = initialMaterials[i];
                    }
                }
            }
        }        
    }

    public virtual void BuildConstructed() { }
}
