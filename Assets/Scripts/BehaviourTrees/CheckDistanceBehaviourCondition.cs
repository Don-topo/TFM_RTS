using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckDistance", story: "Check [Self] distance to [TargetGameObject]", category: "Conditions", id: "00e1a36f1681919f19f555e7edb6c17b")]
public partial class CheckDistanceBehaviourCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;

    public override bool IsTrue()
    {
        if(Self == null || TargetGameObject == null) return false;
        return Vector3.Distance(Self.Value.transform.position, TargetGameObject.Value.transform.position) < Self.Value.GetComponent<BaseAttacker>().AttackInfo.AttackRange;
    }
}
