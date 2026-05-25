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
    private Animator animator;
    private BaseUnit baseUnit;
    private Transform selfTransform;
    private Transform targetTransform;
    private List<Collider> targetColliders;
    private float lastAttack;
    private IAttackable targetAttackable;

    protected override Status OnStart()
    {
        if (Self.Value == null || TargetGameObject.Value == null || Enemies.Value == null) return Status.Failure;

        selfTransform = Self.Value.transform;
        navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();
        baseUnit = Self.Value.GetComponent<BaseUnit>();
        animator = Self.Value.GetComponentInChildren<Animator>();
        targetTransform = TargetGameObject.Value.transform;
        targetAttackable = TargetGameObject.Value.GetComponent<IAttackable>();
        lastAttack = Time.time;

        if (Self.Value.GetComponent<CommonActions>().isDead && navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
            return Status.Success;
        }
        if (!Enemies.Value.Contains(TargetGameObject.Value))
        {
            navMeshAgent.SetDestination(targetTransform.position);
            navMeshAgent.isStopped = false;
            if(animator != null)
            {
                animator.SetFloat("MoveSpeed", 1);// navMeshAgent.speed);
            }            
        }

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if(TargetGameObject == null || targetAttackable.CurrentHealth == 0) return Status.Success;
        if (Self.Value.GetComponent<CommonActions>().isDead && navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
            return Status.Success;
        }

        if (!Enemies.Value.Contains(TargetGameObject.Value))
        {
            return Status.Running;
        }
        navMeshAgent.isStopped = true;
        if(animator != null)
        {
            animator.SetFloat("MoveSpeed", 0);// navMeshAgent.speed);
        }
        
        LookAtTarget();

        if(Time.time > lastAttack + AttackInfo.Value.AttackSpeed)
        {
            Attack();
        } 

        return Status.Running;
    }

    protected override void OnEnd()
    {
        if(navMeshAgent != null && navMeshAgent.isOnNavMesh && navMeshAgent.enabled && navMeshAgent.isOnNavMesh)
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
        if(animator != null)
        {
            animator.SetTrigger("Attack");
        }
        lastAttack = Time.time;
        targetAttackable.ApplyDamage(AttackInfo.Value.AttackDamage);       
    }
}

