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
    [SerializeReference] public BlackboardVariable<SO_Building> BuildingUnderConstruction;

    private float startBuildTime;
    private BaseBuilding completedBuilding;
    private Renderer[] buildingRenderers;
    private Vector3[] startPosition;
    private Vector3[] endPosition;
    private float targetHealth;

    protected override Status OnStart()
    {        
        GameObject building = GameObject.Instantiate(BuildingUnderConstruction.Value.UnitPrefab, TargetPosition.Value, Quaternion.identity);
        completedBuilding = building.GetComponent<BaseBuilding>();
        startBuildTime = Time.time;
        completedBuilding.StartTime = startBuildTime;
        buildingRenderers = completedBuilding.GetComponentsInChildren<Renderer>();
        startPosition = new Vector3[buildingRenderers.Length];
        endPosition = new Vector3[buildingRenderers.Length];
        for(int i = 0; i < buildingRenderers.Length; i++)
        {
            startPosition[i] = TargetPosition.Value - (Vector3.up * buildingRenderers[i].bounds.size.y);
            endPosition[i] = TargetPosition.Value;
            buildingRenderers[i].transform.position = startPosition[i];
        }
       
        return OnUpdate();
    }

    protected override Status OnUpdate()
    {
        float normalizedTime = (Time.time - startBuildTime) / SO_Building.Value.GenerationTime;

        targetHealth += Time.deltaTime * (SO_Building.Value.Health / SO_Building.Value.GenerationTime);
        if (targetHealth >= 1)
        {
            int healAmount = Mathf.FloorToInt(targetHealth);
            completedBuilding.Heal(healAmount);
            targetHealth -= healAmount;
        }

        for(int i = 0; i < buildingRenderers.Length; i++)
        {
            buildingRenderers[i].transform.position = Vector3.Lerp(startPosition[i], endPosition[i], normalizedTime);
        }       

        return normalizedTime >= 1 ? Status.Success : Status.Running;
    }

    protected override void OnEnd()
    {
        completedBuilding.IsConstructed = true;
        completedBuilding.RestoreMaterials();
        if(completedBuilding is ProductionBuilding a)
        {
            a.BuildConstructed();
        }
        completedBuilding.RefreshUI();
    }
}

