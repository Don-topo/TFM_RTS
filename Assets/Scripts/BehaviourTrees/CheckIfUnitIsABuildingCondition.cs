using System;
using Unity.Behavior;
using UnityEngine;

[Serializable, Unity.Properties.GeneratePropertyBag]
[Condition(name: "CheckIfUnitIsABuilding", story: "[Self] is Building", category: "Conditions", id: "617e44f83ea9a18300542ddc2b4c39f9")]
public partial class CheckIfUnitIsABuildingCondition : Condition
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    public override bool IsTrue() => Self.Value.GetComponent<BaseBuilding>() != null;

}
