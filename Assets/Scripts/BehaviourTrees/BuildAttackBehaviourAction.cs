using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BuildAttack", story: "[Self] Attack [TargetGameObject]", category: "Action", id: "b02f8be3d55d1461e2d67f0e751f6d2b")]
public partial class BuildAttackBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;
    [SerializeReference] public BlackboardVariable<SO_AttackInfo> AttackInfo;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Enemies;

    private BaseUnit baseUnit;
    private Transform selfTransform;
    private Transform targetTransform;
    private List<Collider> targetColliders;
    private float lastAttack;
    private IAttackable targetAttackable;
    private AudioSource audioSource;
    private ParticleSystem particleSystem;

    protected override Status OnStart()
    {
        if (Self.Value == null || TargetGameObject.Value == null || Enemies.Value == null) return Status.Failure;
            
        selfTransform = Self.Value.transform;
        particleSystem = Self.Value.GetComponent<AttackerBuilding>().AttackInfo.AttackEffect;
        baseUnit = selfTransform.GetComponent<BaseUnit>();
        targetTransform = TargetGameObject.Value.transform;
        targetAttackable = TargetGameObject.Value.GetComponent<IAttackable>();
        audioSource = Self.Value.GetComponent<AudioSource>();
        lastAttack = Time.time;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (TargetGameObject.Value == null || targetAttackable.CurrentHealth == 0) return Status.Success;

        if (!Enemies.Value.Contains(TargetGameObject.Value))
        {
            return Status.Running;
        }
        if (Time.time > lastAttack + AttackInfo.Value.AttackSpeed)
        {
            Attack();
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    private void Attack()
    {
        lastAttack = Time.time;        
        if (audioSource != null) audioSource.Play();
        targetAttackable.ApplyDamage(AttackInfo.Value.AttackDamage);
    }
}

