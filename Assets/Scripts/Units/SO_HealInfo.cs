using UnityEngine;

[CreateAssetMenu(fileName = "HealInfo", menuName = "Units/SO Heal Info", order = 101)]
public class SO_HealInfo : ScriptableObject
{
    [field: SerializeField] public int Amount { get; set; } = 20;
    [field: SerializeField] public float HealSpeed { get; set; } = 4.5f;
    [field: SerializeField] public float HealRange { get; set; } = 2.5f;
}
