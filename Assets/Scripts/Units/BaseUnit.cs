using System.Collections.Generic;
using Unity.Behavior;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

[RequireComponent (typeof(NavMeshAgent), typeof(BehaviorGraphAgent))]
public class BaseUnit : CommonActions, IMoveable, IHealable
{
    [Header("Audio")]
    [SerializeField] protected List<AudioClip> moveAudioClips;
    [Header("Events")]
    [SerializeField] private UnitRecruitedEvent recruitedEvent;
    [SerializeField] private ResourceEvent resourceEvent;

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
        // Raise recruit event
        recruitedEvent.Raise(this);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        ResourceOP resource = new ResourceOP(SO_BaseUnit.Cost.SO_Population, SO_BaseUnit.Cost.Population, 0);
        resourceEvent.Raise(resource);
    }

    public void Move(Transform transform)
    {
        if (isDead) Stop();
        PlayMoveAudio();
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Move);
        behaviorGraphAgent.SetVariableValue("TargetGameObject", transform.gameObject);        
    }

    public void Move(Vector3 position)
    {
        if (isDead) Stop();
        PlayMoveAudio();
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
        if (isDead) return;
        CurrentHealth = Mathf.Clamp(CurrentHealth + amount, 0, MaxHealth);
        updateHealthEvent.Raise(this);
    }

    private void PlayMoveAudio()
    {
        if(moveAudioClips.Count > 0)
        {
            AudioManager.SetAudioClips(moveAudioClips);
            AudioManager.PlayAudio();
        }        
    }
}
