using UnityEngine;

[CreateAssetMenu(fileName = "VisionConfig", menuName = "Units/Vision Config")]
public class VisionConfig : ScriptableObject
{
    [field: SerializeField] public float VisionRange { get; private set; } = 5;
}
