using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;
using System.Collections.Generic;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "AttackTarget", story: "[Self] Attack [TargetGameObject]", category: "Action", id: "d8a435a57dc4a19842a4791e6638ebf2")]
public partial class AttackTargetBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;
    [SerializeReference] public BlackboardVariable<SO_AttackInfo> AttackInfo;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Enemies;

    private NavMeshAgent navMeshAgent;
    private BaseUnit baseUnit;
    private Transform selfTransform;
    private Transform targetTransform;
    private List<Collider> targetColliders;
    private float lastAttack;
    private IAttackable targetAttackable;

    protected override Status OnStart()
    {
        selfTransform = Self.Value.transform;
        navMeshAgent = selfTransform.GetComponent<NavMeshAgent>();
        baseUnit = selfTransform.GetComponent<BaseUnit>();
        targetTransform = TargetGameObject.Value.transform;
        targetAttackable = TargetGameObject.Value.GetComponent<IAttackable>();
        lastAttack = Time.time;

        if (!Enemies.Value.Contains(TargetGameObject.Value))
        {
            navMeshAgent.SetDestination(targetTransform.position);
            navMeshAgent.isStopped = false;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(TargetGameObject == null || targetAttackable.CurrentHealth == 0) return Status.Success;

        if (!Enemies.Value.Contains(TargetGameObject.Value))
        {
            return Status.Running;
        }
        navMeshAgent.isStopped = true;
        LookAtTarget();

        if(Time.time > lastAttack + AttackInfo.Value.AttackSpeed)
        {
            Attack();
        } 

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if(navMeshAgent != null && navMeshAgent.isOnNavMesh && navMeshAgent.enabled)
        {
            navMeshAgent.isStopped = false;
        }
    }

    private void LookAtTarget()
    {
        Quaternion lookRotation = Quaternion.LookRotation(
                    (targetTransform.position - selfTransform.position).normalized,
                    Vector3.up
                );
        selfTransform.rotation = Quaternion.Euler(
            selfTransform.root.eulerAngles.x,
            lookRotation.eulerAngles.y,
            selfTransform.rotation.eulerAngles.z
        );
    }

    private void Attack()
    {
        lastAttack = Time.time;
        targetAttackable.ApplyDamage(AttackInfo.Value.AttackDamage);

    }
}

