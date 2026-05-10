using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "ConstructBuilding", story: "[Self] construct [SO_Building] to [TargetPosition]", category: "Action", id: "6c118eeef84d95867712ba399be64fcf")]
public partial class ConstructBuildingBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<SO_Building> SO_Building;
    [SerializeReference] public BlackboardVariable<Vector3> TargetPosition;
    [SerializeReference] public BlackboardVariable<BaseBuilding> BuildingUnderConstruction;

    private float startBuildTime;
    private BaseBuilding completedBuilding;
    private Renderer buildingRenderer;
    private Vector3 startPosition;
    private Vector3 endPosition;
    private float targetHealth;

    protected override Status OnStart()
    {
        GameObject building = GameObject.Instantiate(SO_Building.Value.UnitPrefab, TargetPosition.Value, Quaternion.identity);
        startBuildTime = Time.time;
        startPosition = TargetPosition.Value - Vector3.up * buildingRenderer.bounds.size.y;
        endPosition = TargetPosition.Value;
        //buildingRenderer = completedBuilding.MainRenderer;
        buildingRenderer.transform.position = startPosition;
        return OnUpdate();
    }

    protected override Status OnUpdate()
    {
        float normalizedTime = (Time.time - startBuildTime) / SO_Building.Value.GenerationTime;

        targetHealth += Time.deltaTime * (SO_Building.Value.Health / SO_Building.Value.GenerationTime);
        if (targetHealth >= 1)
        {
            int healAmount = Mathf.FloorToInt(targetHealth);
            //completedBuilding.Heal(healAmount);
            targetHealth -= healAmount;
        }

        buildingRenderer.transform.position = Vector3.Lerp(startPosition, endPosition, normalizedTime);

        return normalizedTime >= 1 ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
    }
}

