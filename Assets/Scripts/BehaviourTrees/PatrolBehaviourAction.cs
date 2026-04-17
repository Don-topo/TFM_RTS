using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Patrol", story: "[Self] Patrol setting [TargetPosition] using [PositionA] and [PositionB]", category: "Action", id: "25c7ba5cf9c0af878f104274dd2e6f81")]
public partial class PatrolBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Vector3> TargetPosition;
    [SerializeReference] public BlackboardVariable<Vector3> PositionA;
    [SerializeReference] public BlackboardVariable<Vector3> PositionB;

    private NavMeshAgent navMeshAgent;

    protected override Status OnStart()
    {
        // Safety Check
        if (!Self.Value.TryGetComponent(out navMeshAgent))
        {
            return Status.Failure;
        }

        // Check if the agent is already at that position
        if (Vector3.Distance(navMeshAgent.transform.position, TargetPosition.Value) <= navMeshAgent.stoppingDistance)
        {
            return Status.Success;
        }

        // Set the new destination
        if (TargetPosition.Value == Vector3.zero)
        {
            TargetPosition.Value = PositionB;
            navMeshAgent.SetDestination(TargetPosition.Value);
            return Status.Running;
        }
        
        TargetPosition.Value = PositionA;        
        navMeshAgent.SetDestination(TargetPosition.Value);

        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        // Finish if unity is calculating the path
        if (navMeshAgent.pathPending) return Status.Running;

        if (navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            // The agent arrived to the target
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

