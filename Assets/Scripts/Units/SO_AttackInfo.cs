using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackInfo", menuName = "Units/Attack Info", order = 7)]
public class SO_AttackInfo : ScriptableObject
{
    [field: SerializeField] public int AttackDamage { get; private set; } = 20;
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public float AttackSpeed { get; private set; } = 1f;
    [field: SerializeField] public List<AudioClip> AttackAudioClips;
    [field: SerializeField] public List<AudioClip> PatrolAudioClips;
    [field: SerializeField] public ParticleSystem AttackEffect { get; private set; }
}
