using Unity.Behavior;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent (typeof(NavMeshAgent), typeof(BehaviorGraphAgent))]
public class BaseUnit : CommonActions, IMoveable
{   
    protected BehaviorGraphAgent behaviorGraphAgent;
    protected NavMeshAgent navMeshAgent;

   
    void Awake()
    {
        // Get components
        navMeshAgent = GetComponent<NavMeshAgent>();
        behaviorGraphAgent = GetComponent<BehaviorGraphAgent>();

        // Set behaviour agent
        behaviorGraphAgent.SetVariableValue("Command", UnitActions.Stop);        
    }

    // Update is called once per frame
    void Update()
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

    public void StopMove()
    {
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Stop);
    }
}
