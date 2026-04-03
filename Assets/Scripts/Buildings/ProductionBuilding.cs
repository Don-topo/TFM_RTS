using UnityEngine;

public class ProductionBuilding : BaseBuilding
{
    // Resource to produce
    [SerializeField] private SO_Resource resource;
    // Events to trigger
    [SerializeField] private ResourceEvent resourceEvent;

    // Variables to hold and count the time past
    private float startTime;

    protected override void Awake()
    {
        // Start counting time to produce
    }

    protected override void Start()
    {
        base.Start();
        startTime = Time.time;
    }

    protected override void Update()
    {
        // Check if the generation of the resource is completed
        if(resource.ObtainingTime + startTime <= Time.time)
        {
            // Generation complete, reset start value and raise resource event
            startTime = Time.time;
            ProduceResource();
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
    }

    private void ProduceResource()
    {
        resourceEvent.Raise(resource);
    }
}