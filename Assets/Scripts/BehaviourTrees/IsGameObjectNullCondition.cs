using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "IsGameObjectNull", story: "Check if the [GameObject] is not null", category: "Variable Conditions", id: "f6889e62d092d5df9d01e719e6262b81")]
public partial class IsGameObjectNullCondition : Condition
{
    [SerializeReference] new public BlackboardVariable<GameObject> GameObject;

    public override bool IsTrue()
    {
        return GameObject.Value != null;
    }

}
