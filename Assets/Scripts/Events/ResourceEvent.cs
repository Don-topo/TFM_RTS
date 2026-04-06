using UnityEngine;

public struct ResourceOP
{
    public SO_Resource SO_Resource { get; private set; }
    public int Amount { get; private set; }
    public int MaxAmount { get; private set; }

    public ResourceOP(SO_Resource sO_Resource, int Amount, int MaxAmount)
    {
        SO_Resource = sO_Resource;
        this.Amount = Amount;
        this.MaxAmount = MaxAmount;
    }
}

[CreateAssetMenu(fileName = "ResourceEvent", menuName = "Events/Resource Event")]
public class ResourceEvent : GameEvent<ResourceOP>
{
    ResourceOP resource;
    
}
