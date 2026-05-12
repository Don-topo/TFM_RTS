using UnityEngine;
using System.Linq;

public class ProductionBuilding : BaseBuilding
{
    [Header("Resource to produce")]
    [field: SerializeField] public SO_Resource Resource { get; private set; }
    [field: SerializeField] private bool applyForEveryResource;

    protected override void Start()
    {        
        base.Start();        
    }

    protected override void Update()
    {
        // Check if the generation of the resource is completed
        if(IsConstructed && !Resource.ProducesOnlyOneTime && Resource.ObtainingTime + StartTime <= Time.time)
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
        if (applyForEveryResource)
        {
            foreach(ResourcesType resourceType in System.Enum.GetValues(typeof(ResourcesType)))
            {                
                Resource.ResourceTypes = resourceType;
                ResourceOP resourceOP = new ResourceOP(Resource, Resource.ObtainedAmount, Resource.MaxAmount);
                resourceEvent.Raise(resourceOP);
            }
        }
        else
        {
            ResourceOP resourceOP = new ResourceOP(Resource, Resource.ObtainedAmount, Resource.MaxAmount);
            resourceEvent.Raise(resourceOP);
        }        
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();
    }

    public override void BuildConstructed()
    {
        base.BuildConstructed();
        StartTime = Time.time;
        if (Resource.ProducesOnlyOneTime)
        {
            ProduceResource();
        }        
    }
}