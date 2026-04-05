using UnityEngine;

[CreateAssetMenu(fileName = "Resource Cost", menuName = "Resources/Resource Cost", order = 1)]
public class SO_ResourceCost : ScriptableObject
{
    [field: SerializeField] public int Food { get; private set; } = 5;
    [field: SerializeField] public SO_Resource SO_Food {  get; private set; }
    [field: SerializeField] public int Wood { get; private set; } = 0;
    [field: SerializeField] public SO_Resource SO_Wood { get; private set; }
    [field: SerializeField] public int Stone { get; private set; } = 0;
    [field: SerializeField] public SO_Resource SO_Stone { get; private set; }
    [field: SerializeField] public int Iron { get; private set; } = 0;
    [field: SerializeField] public SO_Resource SO_Iron { get; private set; }
    [field: SerializeField] public int Electricity { get; private set; } = 0;
    [field: SerializeField] public SO_Resource SO_Electricity { get; private set; }
    [field: SerializeField] public int Population { get; private set; }
    [field: SerializeField] public SO_Resource SO_Population { get; private set; }
}
