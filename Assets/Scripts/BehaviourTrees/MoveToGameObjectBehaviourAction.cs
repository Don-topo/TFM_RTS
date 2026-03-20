using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "MoveToGameObject", story: "[Agent] Moves To [TargetGameObject]", category: "Action/Navigation", id: "87d9a963394ea9341f576c2386ac173c")]
public partial class MoveToGameObjectBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

    private NavMeshAgent navMeshAgent;
    private Vector3 startPosition;

    protected override Status OnStart()
    {
        // Safety Check
        if(!Agent.Value.TryGetComponent(out navMeshAgent) || TargetGameObject.Value == null)
        {
            return Status.Failure;
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
        // Sanity check to avoid errors
        if (navMeshAgent.pathPending) return Status.Running;

        // Get TargetPosition
        Vector3 targetPosition = TargetGameObject.Value.gameObject.transform.position;

        if (Vector3.Distance(targetPosition, startPosition) >= 0.25f)
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
        // Nothing to do here?
    }
}

