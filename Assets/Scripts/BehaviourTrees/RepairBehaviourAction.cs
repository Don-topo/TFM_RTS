using System;
using Unity.Behavior;
using UnityEngine;
using Action = Unity.Behavior.Action;
using Unity.Properties;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "Repair", story: "[Self] Building repairs", category: "Action", id: "76c27258304ccce02fcf35e5bf9b245f")]
public partial class RepairBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;

    private BaseBuilding building;
    private float startRepairTime;
    private float targetHealth;
    private float repairTime;
    private float accumulatedHeal = 0f;

    protected override Status OnStart()
    {
        startRepairTime = Time.time;
        building = Self.Value.GetComponent<BaseBuilding>();
        building.SaveMaterials();
        repairTime = building.SO_building.GenerationTime - (building.CurrentHealth * building.SO_building.GenerationTime) / building.MaxHealth;
        return OnUpdate();
    }

    protected override Status OnUpdate()
    {
        float maxHealth = building.MaxHealth;

        if (building.CurrentHealth >= maxHealth)
        {
            return Status.Success;
        }            

        float healPerSecond = maxHealth / building.SO_building.GenerationTime;

        accumulatedHeal += healPerSecond * Time.deltaTime;

        if (accumulatedHeal >= 1f)
        {
            int healAmount = Mathf.FloorToInt(accumulatedHeal);

            building.Heal(healAmount);

            accumulatedHeal -= healAmount;
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
        building.IsRepairing = false;
        building.RestoreMaterials();
        if(building is AttackerBuilding)
        {
            ((AttackerBuilding)building).ResetAttackSystem();
        }
        building.RefreshUI();
    }
}

