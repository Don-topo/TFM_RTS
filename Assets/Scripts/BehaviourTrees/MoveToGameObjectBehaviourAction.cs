using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToGameObject", story: "[Self] Moves To [TargetGameObject]", category: "Action/Navigation", id: "87d9a963394ea9341f576c2386ac173c")]
public partial class MoveToGameObjectBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private Vector3 startPosition;

    protected override Status OnStart()
    {
        navMeshAgent = Self.Value.GetComponent<NavMeshAgent>();
        if (navMeshAgent == null) return Status.Failure;
        animator = Self.Value.GetComponentInChildren<Animator>();
        if(animator == null) return Status.Failure;

        if (Self.Value.GetComponent<CommonActions>().isDead && navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
            return Status.Success;
        }

        // Get the target position
        Vector3 targetPosition = TargetGameObject.Value.gameObject.transform.position;

        // Check if the agent is already at that position
        if (Vector3.Distance(navMeshAgent.transform.position, targetPosition) <= navMeshAgent.stoppingDistance)
        {
            return Status.Success;
        }

        // Move Agent to target
        navMeshAgent.SetDestination(targetPosition);
        startPosition = targetPosition;

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (Self.Value.GetComponent<CommonActions>().isDead && navMeshAgent != null)
        {
            navMeshAgent.ResetPath();
            navMeshAgent.isStopped = true;
            return Status.Success;
        }
        animator.SetFloat("MoveSpeed", navMeshAgent.velocity.magnitude);

        // Sanity check to avoid errors
        if (navMeshAgent.pathPending) return Status.Running;

        // Get TargetPosition
        Vector3 targetPosition = GetTargetPosition();

        if (Vector3.Distance(targetPosition, startPosition) >= navMeshAgent.stoppingDistance)
        {
            // Set new path and send the agent
            navMeshAgent.SetDestination(targetPosition);
            startPosition = navMeshAgent.destination;
            return Status.Running;
        }
        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // The agent arrived to the target
            return Status.Success;
        }

        return Status.Running;

    }

    protected override void OnEnd()
    {
        animator.SetFloat("MoveSpeed", 0);
    }

    private Vector3 GetTargetPosition()
    {
        Vector3 targetPosition;
        if (TargetGameObject.Value.TryGetComponent(out Collider collider))
        {
            targetPosition = collider.ClosestPoint(navMeshAgent.transform.position);
        }
        else
        {
            targetPosition = TargetGameObject.Value.transform.position;
        }

        return targetPosition;
    }
}

