using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
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

    protected override Status OnStart()
    {
        selfTransform = Self.Value.transform;
        baseUnit = selfTransform.GetComponent<BaseUnit>();
        targetTransform = TargetGameObject.Value.transform;
        targetAttackable = TargetGameObject.Value.GetComponent<IAttackable>();
        lastAttack = Time.time;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (TargetGameObject == null || targetAttackable.CurrentHealth == 0) return Status.Success;

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
        targetAttackable.ApplyDamage(AttackInfo.Value.AttackDamage);
    }
}

