using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

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
        if (applyForEveryResource)
        {
            ResourceOP resourceOP = new ResourceOP(SO_building.Cost.SO_Wood, 0, Resource.MaxAmount);
            resourceEvent.Raise(resourceOP);
            resourceOP = new ResourceOP(SO_building.Cost.SO_Food, 0, Resource.MaxAmount);
            resourceEvent.Raise(resourceOP);
            resourceOP = new ResourceOP(SO_building.Cost.SO_Iron, 0, Resource.MaxAmount);
            resourceEvent.Raise(resourceOP);
            resourceOP = new ResourceOP(SO_building.Cost.SO_Stone, 0, Resource.MaxAmount);
            resourceEvent.Raise(resourceOP);
        }
        else
        {
            switch (Resource.ResourceTypes)
            {
                case ResourcesType.Food:                    
                    resourceEvent.Raise(new ResourceOP(SO_building.Cost.SO_Food, 0, -Resource.MaxAmount));
                    break;
                case ResourcesType.Wood:
                    resourceEvent.Raise(new ResourceOP(SO_building.Cost.SO_Wood, 0, -Resource.MaxAmount));
                    break;
                case ResourcesType.Stone:
                    resourceEvent.Raise(new ResourceOP(SO_building.Cost.SO_Stone, 0, -Resource.MaxAmount));
                    break;
                case ResourcesType.Iron:
                    resourceEvent.Raise(new ResourceOP(SO_building.Cost.SO_Iron, 0, -Resource.MaxAmount));
                    break;
                case ResourcesType.Electricity:
                    resourceEvent.Raise(new ResourceOP(SO_building.Cost.SO_Electricity, 0, -Resource.MaxAmount));
                    break;
                case ResourcesType.Population:
                    resourceEvent.Raise(new ResourceOP(SO_building.Cost.SO_Population, 0, -Resource.MaxAmount));
                    break;
                default:
                    break;
            }
        }        
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