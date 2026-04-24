using UnityEngine;

[CreateAssetMenu(fileName = "Building", menuName = "Buildings/Building")]
public class BaseBuilding : CommonActions
{
    [field: SerializeField] public Material PlaceMaterial { get; private set; }
    [SerializeField] protected ResourceEvent resourceEvent;
    [SerializeField] private BuildingState state;
    [SerializeField] protected SO_Building so_building;
    
    protected override void Start()
    {
        base.Start();
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
        // Destroy game object
        Destroy(gameObject);
        // Refund spend resources based on building state and health
        float refund = CalculateRefund();
        // Return build resources
        ResourceOP resourceOP = new ResourceOP(so_building.Cost.SO_Food, -so_building.Cost.Food, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(so_building.Cost.SO_Wood, so_building.Cost.Wood, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(so_building.Cost.SO_Stone, so_building.Cost.Stone, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(so_building.Cost.SO_Iron, so_building.Cost.Iron, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(so_building.Cost.SO_Electricity, -so_building.Cost.Electricity, 0);
        resourceEvent.Raise(resourceOP);
        resourceOP = new ResourceOP(so_building.Cost.SO_Population, -so_building.Cost.Population, 0);
        resourceEvent.Raise(resourceOP);
    }

    private float CalculateRefund()
    {
        float refund = 1.0f;
        if (state.CurrentState.Equals(BuildingState.State.Construction)) return refund;

        return 0f;
    }
}
