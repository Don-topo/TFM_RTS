using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "HealTarget", story: "[Self] Heal some [HealInfo] to [TagetGameObject]", category: "Action", id: "bb490fa1b3ab02007af5a5e9ad723096")]
public partial class HealTargetBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<SO_HealInfo> HealInfo;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Allies;

    private NavMeshAgent navMeshAgent;
    private Transform selfTransform;
    private Transform targetTransform;
    private IHealable targetToHeal;
    private float lastHealing;

    protected override Status OnStart()
    {
        selfTransform = Self.Value.transform;
        navMeshAgent = selfTransform.GetComponent<NavMeshAgent>();
        targetTransform = TargetGameObject.Value.transform;
        targetToHeal = TargetGameObject.Value.GetComponent<IHealable>();
        lastHealing = Time.time;

        if (!Allies.Value.Contains(TargetGameObject.Value))
        {
            navMeshAgent.SetDestination(targetTransform.position);
            navMeshAgent.isStopped = false;
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (TargetGameObject == null || targetToHeal.CurrentHealth == targetToHeal.MaxHealth
            || targetToHeal.CurrentHealth == 0) return Status.Success;

        if(!Allies.Value.Contains(TargetGameObject.Value)) return Status.Running;
        
        LookAtTarget();

        if (Time.time > lastHealing + HealInfo.Value.HealSpeed)
        {
            Heal();
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if (navMeshAgent != null && navMeshAgent.isOnNavMesh && navMeshAgent.enabled)
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

    private void Heal()
    {
        lastHealing = Time.time;
        targetToHeal.Heal(HealInfo.Value.Amount);
    }
}

