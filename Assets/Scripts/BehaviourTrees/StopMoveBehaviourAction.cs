using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;
using UnityEngine.AI;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "StopMove", story: "[Agent] Stop movement", category: "Action/Navigation", id: "61ff982ecb76b37ef4f34ffe501dd919")]
public partial class StopMoveBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Agent;

    private NavMeshAgent navMeshAgent;

    protected override Status OnStart()
    {
        if(Agent.Value.gameObject.TryGetComponent(out navMeshAgent))
        {
            // Stop and clear agent path
            navMeshAgent.ResetPath();
            return Status.Running;
        }
        return Status.Failure;
    }
}

