using NUnit.Framework;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackInfo", menuName = "Units/Attack Info", order = 7)]
public class SO_AttackInfo : ScriptableObject
{
    [field: SerializeField] public int AttackDamage { get; private set; } = 20;
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public float AttackSpeed { get; private set; } = 1f;
    
}
