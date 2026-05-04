using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent (typeof(NavMeshAgent), typeof(BehaviorGraphAgent))]
public class BaseUnit : CommonActions, IMoveable, IHealable
{   
    public float GetNavMeshAgentRadius => navMeshAgent.radius;
    protected BehaviorGraphAgent behaviorGraphAgent;
    protected NavMeshAgent navMeshAgent;

   
    protected override void Awake()
    {
        base.Awake();
        // Get components
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();
        CurrentHealth = SO_BaseUnit.Health;
        MaxHealth = CurrentHealth;
        // Set behaviour agent
        behaviorGraphAgent.SetVariableValue("Command", UnitActions.Stop);        
    }

    // Update is called once per frame
    protected override void Update()
    {
        
    }

    public void Move(Transform transform)
    {
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Move);
        behaviorGraphAgent.SetVariableValue("TargetGameObject", transform.gameObject);        
    }

    public void Move(Vector3 position)
    {
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Move);
        behaviorGraphAgent.SetVariableValue("TargetPosition", position);
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
    }

    public void Stop()
    {
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Stop);
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
    }

    public void Heal(int amount)
    {
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
    }
}
