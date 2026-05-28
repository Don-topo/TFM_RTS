using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AttackInfo", menuName = "Units/Attack Info", order = 7)]
public class SO_AttackInfo : ScriptableObject
{
    [Header("Base Attributes")]
    [field: SerializeField] public int AttackDamage { get; private set; } = 20;
    [field: SerializeField] public float AttackRange { get; private set; } = 2f;
    [field: SerializeField] public float AttackSpeed { get; private set; } = 1f;
    [Header("Releated Actions audio")]
    [field: SerializeField] public List<AudioClip> AttackAudioClips;
    [field: SerializeField] public List<AudioClip> PatrolAudioClips;
    [Header("Effects")]
    [field: SerializeField] public ParticleSystem AttackEffect { get; private set; }    
    [field: SerializeField] public float AreaOfEffectRadius { get; private set; } = 2;
    [Header("Area Config")]
    [field: SerializeField] public bool IsDetachedAreaProyectile { get; private set; } = false;
    [field: SerializeField] public bool IsAttachedAreaEffect {  get; private set; } = false;
    [field: SerializeField] public float XArea { get; private set; } = 2f;
    [field: SerializeField] public float YArea { get; private set; } = 2f;
    [field: SerializeField] public float ZArea { get; private set; } = 2f;
    [field: SerializeField] public LayerMask DamageableLayer { get; private set; }

    public int CalculateAreaOfEffectDamage(Vector3 impactPoint, Vector3 targetPosition)
    {
        if (!IsDetachedAreaProyectile) return 0;

        float distance = Vector3.Distance(impactPoint, targetPosition);

        return Mathf.Clamp(Mathf.CeilToInt(AttackDamage * (1 - (distance / AreaOfEffectRadius))), 0, AttackDamage);
    }
}
