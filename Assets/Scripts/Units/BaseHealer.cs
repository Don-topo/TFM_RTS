using System.Collections.Generic;
using System.Linq;
using Unity.Behavior;
using UnityEngine;

public class BaseHealer : BaseUnit, IHealer
{

    [field: SerializeField] public SO_HealInfo HealInfo { get; private set; }
    [SerializeField] private HealingSystem healingSystem;
    [SerializeField] private AllyInRangeEvent unitEnterRange;
    [SerializeField] private AllyInRangeEvent unitOutOfRange;
    public Transform Transform => transform;

    protected override void Awake()
    {
        base.Awake();
        behaviorGraphAgent.SetVariableValue("SO Heal Info", HealInfo);
        unitEnterRange.Register(UnitInRange);
        unitOutOfRange.Register(UnitOutOfRange);
    }

    protected override void Start()
    {
        base.Start();
        healingSystem.SetRange(HealInfo.HealRange);
    }

    protected override void Update()
    {
        base.Update();

        if(healingSystem.AlliesInRange().Count > 0)
        {
            if (behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable))
            {
                behaviorGraphAgent.SetVariableValue("TargetGameObject", GetFirstDamaged(healingSystem.AlliesInRange().ConvertAll(
                damage => damage.TargetPosition.gameObject)));
                behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Heal);
            }            
        }
       
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        unitEnterRange.Unregister(UnitInRange);
        unitOutOfRange.Unregister(UnitOutOfRange);
    }

    public void Heal(IHealable unit)
    {
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", unit.TargetPosition.gameObject);
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Heal);
    }

    public void Heal(Vector3 healPosition)
    {
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
        behaviorGraphAgent.SetVariableValue("TargetPosition", healPosition);
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Heal);
    }

    private void UnitInRange(IHealable unit)
    {
        List<GameObject> targets = SetNearbyAlliesOnBlackboard();

        if (behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            && targetVariable.Value == null && targets.Count > 0)
        {

            behaviorGraphAgent.SetVariableValue("TargetGameObject", GetFirstDamaged(targets));
            behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Heal);
        }
    }

    private void UnitOutOfRange(IHealable unit)
    {
        List<GameObject> targets = SetNearbyAlliesOnBlackboard();

        if (!behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            || unit.TargetPosition.gameObject != targetVariable.Value) return;

        if (targets.Count > 0)
        {
            if (GetFirstDamaged(targets) != default)
            {
                behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", GetFirstDamaged(targets));
                behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Heal);
            }            
        }      
    }

    private List<GameObject> SetNearbyAlliesOnBlackboard()
    {
        List<GameObject> nearbyAllies = healingSystem.AlliesInRange().ConvertAll(
                    damage => damage.TargetPosition.gameObject);
        nearbyAllies.Sort(new ClosestGameObjectComparer(transform.position));

        behaviorGraphAgent.SetVariableValue("Allies", nearbyAllies);

        return nearbyAllies;
    }

    private GameObject GetFirstDamaged(List<GameObject> allies)
    {
        return allies.FirstOrDefault(ally => ally.GetComponent<BaseUnit>().CurrentHealth < ally.GetComponent<BaseUnit>().MaxHealth);
    }
}
