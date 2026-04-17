using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "SetNextPatrolPosition", story: "[Self] set next [TargetPosition] from [PatrolPositions]", category: "Action", id: "d61333e5a6157f388057cd1452943cbc")]
public partial class SetNextPatrolPositionBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<Vector3> TargetPosition;
    [SerializeReference] public BlackboardVariable<List<Vector3>> PatrolPositions;

    protected override Status OnStart()
    {
        if(TargetPosition.Value == Vector3.zero)
        {
            TargetPosition.Value = PatrolPositions.Value[1];
            return Status.Success;
        }
        TargetPosition.Value = PatrolPositions.Value[(PatrolPositions.Value.IndexOf(TargetPosition.Value) + 1) % PatrolPositions.Value.Count];
        return Status.Success;
    }

    /*protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }*/
}

