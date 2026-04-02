using UnityEngine;

[CreateAssetMenu(fileName = "ResourceEvent", menuName = "Events/Resource Event")]
public class ResourceEvent : GameEvent<SO_Resource>
{
    public SO_Resource SO_Resource { get; private set; }
    public int Amount { get; private set; }
}
