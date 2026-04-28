using UnityEngine;

public class ProductionBuilding : BaseBuilding
{
    // Resource to produce
    [field: SerializeField] public SO_Resource resource { get; private set; }

    // Variables to hold and count the time past
    public float StartTime { get; private set; }

    protected override void Start()
    {
        CurrentHealth = so_building.CurrentHealth;
        MaxHealth = so_building.MaxHealth;
        base.Start();
        StartTime = Time.time;
        if(resource != null && resource.ProducesOnlyOneTime)
        {
            ProduceResource();
        }
    }

    protected override void Update()
    {
        // Check if the generation of the resource is completed
        if(!resource.ProducesOnlyOneTime && resource.ObtainingTime + StartTime <= Time.time)
        {
            // Generation complete, reset start value and raise resource event
            StartTime = Time.time;
            ProduceResource();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void ProduceResource()
    {
        ResourceOP resourceOP = new ResourceOP(resource, resource.ObtainedAmount, resource.MaxAmount);       
        resourceEvent.Raise(resourceOP);
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();
    }
}