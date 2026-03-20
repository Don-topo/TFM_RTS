using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToTargetPosition", story: "[Agent] Moves to [TargetPosition]", category: "Action/Navigation", id: "bd5c295e08aa056a66439d325d1b2b01")]
public partial class MoveToTargetPositionBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<Vector3> TargetPosition;

    private NavMeshAgent navMeshAgent;

    protected override Status OnStart()
    {
        // Safety check to avoid errors
        if(!Agent.Value.TryGetComponent(out navMeshAgent) || TargetPosition.Value == null)
        {
            return Status.Failure;
        }

        // Check if the agent is already at that position
        if(Vector3.Distance(navMeshAgent.transform.position, TargetPosition.Value) <= navMeshAgent.stoppingDistance)
        {
            return Status.Success;
        }
        
        // Set the new destination
        navMeshAgent.SetDestination(TargetPosition.Value);

        return Status.Running;

    }

    protected override Status OnUpdate()
    {
        // Finish if unity is calculating the path
        if(navMeshAgent.pathPending) return Status.Running;

        if(navMeshAgent.remainingDistance <= navMeshAgent.stoppingDistance)
        {
            return Status.Success;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        // Nothing to do here?
    }
}

