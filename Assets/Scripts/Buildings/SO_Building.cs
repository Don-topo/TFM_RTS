using UnityEngine;

[CreateAssetMenu(fileName = "SO_Building", menuName = "Buildings/SO_Building")]
public class SO_Building : ScriptableObject
{
    [field: SerializeField] public string Name { get; private set; } = "Building Name";
    [field: SerializeField] public int CurrentHealth { get; private set; } = 0;
    [field: SerializeField] public int MaxHealth { get; private set; } = 1200;
    [field: SerializeField] public SO_ResourceCost Cost { get; private set; }
    [field: SerializeField] public Sprite Icon { get; private set; }
}
