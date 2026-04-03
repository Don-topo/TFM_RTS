using UnityEngine;

[CreateAssetMenu(fileName = "SO_Resource", menuName = "Resources/Resource", order = 99)]
public class SO_Resource : ScriptableObject
{
    [field: SerializeField] public float ObtainingTime { get; private set; } = 8.0f;
    [field: SerializeField] public int ObtainedAmount { get; private set; } = 15;
    [field: SerializeField] public int MaxAmount { get; private set; } = 1000;
    [field: SerializeField] public Sprite Icon { get; private set; }
    [field: SerializeField] public bool ProducesOnlyOneTime { get; private set; } = false;
}
