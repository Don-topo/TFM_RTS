using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class BaseAttacker : BaseUnit, IAttacker
{
    [Header("Basic Info")]
    public Transform Transform => transform;
    [SerializeField] private AttackSystem attackSystem;
    [Header("Events")]
    [SerializeField] private EnemyInRangeEvent unitEnterRange;
    [SerializeField] private EnemyInRangeEvent unitOutOfRange;
    [Header("Attack Info")]
    [field: SerializeField] public SO_AttackInfo AttackInfo { get; private set; }

    private AudioSource audioSource;

    protected override void Awake()
    {
        base.Awake();
        behaviorGraphAgent.SetVariableValue("SO Attack Info", AttackInfo);
        unitEnterRange.Register(UnitInRange);
        unitOutOfRange.Register(UnitOutOfRange);
        audioSource = GetComponent<AudioSource>();        
    }

    protected override void Start()
    {
        attackSystem.SetAttackRange(AttackInfo.AttackRange);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        unitEnterRange.Unregister(UnitInRange);
        unitOutOfRange.Unregister(UnitOutOfRange);
    }

   
    public void Attack(IAttackable attackable)
    {
        if (isDead) return;
        PlayAttackAudio();
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", attackable.TargetPosition.gameObject);
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Attack);
    }

    public void Attack(Vector3 attackPosition)
    {
        if (isDead) return;
        PlayAttackAudio();
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
        behaviorGraphAgent.SetVariableValue("TargetPosition", attackPosition);
        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Attack);
    }

    public void Patrol(Vector3 targetPosition)
    {
        if (targetPosition == null || isDead) return;
        PlayPatrolAutdio();

        Vector3 currenPosition = gameObject.transform.position;
        List<Vector3> patrolPositions = new List<Vector3>
        {
            currenPosition,
            targetPosition
        };

        behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Patrol);
        behaviorGraphAgent.SetVariableValue("PatrolPositions", patrolPositions);

    }

    private void UnitInRange(IAttackable enemyInRange)
    {
        List<GameObject> targets = SetNearbyEnemiesOnBlackboard();

        if (behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            && targetVariable.Value == null && targets.Count > 0)
        {
            behaviorGraphAgent.SetVariableValue("TargetGameObject", targets[0]);
            if(behaviorGraphAgent.GetVariable("UnitActions", out BlackboardVariable<UnitActions> act) && act != UnitActions.Patrol)
                behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Attack);
        }
    }

    private void UnitOutOfRange(IAttackable enemyOutOfRange)
    {
        List<GameObject> targets = SetNearbyEnemiesOnBlackboard();

        if (!behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            || enemyOutOfRange.TargetPosition.gameObject != targetVariable.Value) return;

        if (targets.Count > 0)
        {
            behaviorGraphAgent.SetVariableValue("TargetGameObject", targets[0]);
        }
        else
        {
            behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
            behaviorGraphAgent.GetVariable("UnitActions", out BlackboardVariable<UnitActions> currentAction);
            if (movePosition != null && Vector3.Distance(transform.position, (Vector3)movePosition) > navMeshAgent.stoppingDistance
                && currentAction != UnitActions.Patrol)
            {
                Move((Vector3)movePosition);
                return;
            }
            else
            {
                movePosition = null;
            }

            if (currentAction != UnitActions.Patrol && currentAction != UnitActions.Attack)
            {
                behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Stop);
            }
            // Uncomment this to set the enemy position as destination => move to enemy position after killing it
            //behaviorGraphAgent.SetVariableValue("TargetPosition", enemyOutOfRange.TargetPosition.position);
        }
    }

    private List<GameObject> SetNearbyEnemiesOnBlackboard()
    {
        List<GameObject> nearbyEnemies = attackSystem.GetEnemiesInRange().ConvertAll(
                    damage => damage.TargetPosition.gameObject);
        nearbyEnemies.Sort(new ClosestGameObjectComparer(transform.position));

        behaviorGraphAgent.SetVariableValue("Enemies", nearbyEnemies);

        return nearbyEnemies;
    }

    private void PlayAttackAudio()
    {
        if(AttackInfo.AttackAudioClips.Count > 0)
        {
            AudioManager.SetAudioClips(AttackInfo.AttackAudioClips);
            AudioManager.PlayAudio();
        }        
    }

    private void PlayPatrolAutdio()
    {
        if(AttackInfo.PatrolAudioClips.Count > 0)
        {
            AudioManager.SetAudioClips(AttackInfo.PatrolAudioClips);
            AudioManager.PlayAudio();
        }        
    }
}
