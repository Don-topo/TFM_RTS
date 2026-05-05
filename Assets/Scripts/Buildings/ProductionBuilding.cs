using UnityEngine;
using System.Linq;

public class ProductionBuilding : BaseBuilding
{
    [Header("Resource to produce")]
    [field: SerializeField] public SO_Resource resource { get; private set; }
    [field: SerializeField] private bool applyForEveryResource;

    // Variables to hold and count the time past
    public float StartTime { get; private set; }

    protected override void Start()
    {        
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
        /*if (applyForEveryResource)
        {
            foreach(ResourcesType resourceType in System.Enum.GetValues(typeof(ResourcesType)))
            {                
                resource.ResourceTypes = resourceType;
                ResourceOP resourceOP = new ResourceOP(resource, resource.ObtainedAmount, resource.MaxAmount);
                resourceEvent.Raise(resourceOP);
            }
        }
        else
        {*/
            ResourceOP resourceOP = new ResourceOP(resource, resource.ObtainedAmount, resource.MaxAmount);
            resourceEvent.Raise(resourceOP);
       // }        
    }

    public override void DestroyBuilding()
    {
        base.DestroyBuilding();
    }
}