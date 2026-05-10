using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Buildings/Building")]
public class BaseBuilding : CommonActions
{
    [field: SerializeField] public Material PlaceMaterial { get; private set; }
    [SerializeField] protected ResourceEvent resourceEvent;
    [SerializeField] private BuildingState state;
    [field: SerializeField] public SO_Building SO_building { get; private set; }
    
    protected override void Start()
    {
        base.Start();
        CurrentHealth = SO_building.Health;
        MaxHealth = CurrentHealth;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    public void BuildBuilding()
    {

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
        if (state.CurrentState.Equals(BuildingState.State.Construction)) return refund;

        return 0f;
    }

}
