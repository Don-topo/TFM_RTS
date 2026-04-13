using UnityEngine;

[CreateAssetMenu(fileName = "SOBaseUnit", menuName = "Units/SO_BaseUnit", order = 103)]
public class SO_BaseUnit : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "Unit Name";
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public GameObject UnitPrefab { get; private set; }
    [field: SerializeField] public int Health { get; private set; } = 100;
    [field: SerializeField] public float GenerationTime { get; private set; } = 8;
    [field: SerializeField] public SO_ResourceCost Cost { get; private set; }
    [field: SerializeField] public SO_AttackInfo AttackInfo { get; private set; }
}
