using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "HealTarget", story: "[Self] Heal some [HealInfo] to [TagetGameObject]", category: "Action", id: "bb490fa1b3ab02007af5a5e9ad723096")]
public partial class HealTargetBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<SO_HealInfo> HealInfo;
    [SerializeReference] public BlackboardVariable<GameObject> TagetGameObject;

    protected override Status OnStart()
    {
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        return Status.Success;
    }

    protected override void OnEnd()
    {
    }
}

