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
    [field: SerializeField] public GameObject WeaponGameObject { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        behaviorGraphAgent.SetVariableValue("SO Attack Info", AttackInfo);
        unitEnterRange.Register(UnitInRange);
        unitOutOfRange.Register(UnitOutOfRange);   
    }

    protected override void Start()
    {
        base.Start();
        attackSystem.SetAttackRange(AttackInfo.AttackRange);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        unitEnterRange.Unregister(UnitInRange);
        unitOutOfRange.Unregister(UnitOutOfRange);
    }

    void OnDrawGizmos()
    {
        if (WeaponGameObject == null) return;

        Vector3 boxSize = new Vector3(AttackInfo.XArea, AttackInfo.YArea, AttackInfo.ZArea);
        Vector3 forward = WeaponGameObject.transform.right;
        Vector3 offset = AttackInfo.AddOffset ? (WeaponGameObject.transform.right * (boxSize.z * 0.5f)) : Vector3.zero;
        Vector3 center = WeaponGameObject.transform.position + offset;

        Gizmos.matrix = Matrix4x4.TRS(
            center,
            Quaternion.LookRotation(forward, WeaponGameObject.transform.up),
            Vector3.one
        );

        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(Vector3.zero, boxSize);
    }

    public void Attack(IAttackable attackable)
    {
        if (isDead) return;
        PlayAttackAudio();
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", attackable.TargetPosition.gameObject);
        behaviorGraphAgent.SetVariableValue<UnitActions>("UnitActions", UnitActions.Attack);
    }

    public void Attack(Vector3 attackPosition)
    {
        if (isDead) return;
        PlayAttackAudio();
        behaviorGraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
        behaviorGraphAgent.SetVariableValue("TargetPosition", attackPosition);
        behaviorGraphAgent.SetVariableValue<UnitActions>("UnitActions", UnitActions.Attack);
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
            behaviorGraphAgent.SetVariableValue("UnitActions", UnitActions.Attack);
        }        
    }

    protected virtual void UnitOutOfRange(IAttackable enemyOutOfRange)
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
            behaviorGraphAgent.SetVariableValue("TargetLocation", enemyOutOfRange.TargetPosition.position);
            
            if(this is EnemyController)
            {
                this.GetComponent<EnemyController>().AttackTarget();
            }
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
