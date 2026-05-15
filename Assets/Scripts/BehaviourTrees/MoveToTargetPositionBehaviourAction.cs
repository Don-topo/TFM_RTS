using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTargetPosition", story: "[Self] Moves to [TargetPosition]", category: "Action/Navigation", id: "bd5c295e08aa056a66439d325d1b2b01")]
public partial class MoveToTargetPositionBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Vector3> TargetPosition;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    protected override Status OnStart()
    {
        navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) return Status.Failure;
        animator = Self.Value.GetComponentInChildren<Animator>();
        if (animator == null) return Status.Failure;

        if (Self.Value.GetComponent<CommonActions>().isDead && navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
            return Status.Success;
        }
        // Check if the agent is already at that position
        if (Vector3.Distance(navMeshAgent.transform.position, TargetPosition.Value) <= navMeshAgent.stoppingDistance)
        {
            return Status.Success;
        }
        
        // Set the new destination
        navMeshAgent.SetDestination(TargetPosition.Value);

        return Status.Running;

    }

    protected override Status OnUpdate()
    {
        if(Self.Value.GetComponent<CommonActions>().isDead && navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;      
            return Status.Success;
        }
        animator.SetFloat("MoveSpeed", navMeshAgent.velocity.magnitude);
        // Finish if unity is calculating the path
        if (navMeshAgent.pathPending) return Status.Running;

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        animator.SetFloat("MoveSpeed", 0);
    }
}

