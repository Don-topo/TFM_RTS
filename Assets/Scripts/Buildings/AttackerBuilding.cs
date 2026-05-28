using System.Collections.Generic;
using Unity.Behavior;
using UnityEngine;

public class AttackerBuilding : BaseBuilding, IAttacker
{
    [Header("Basic Info")]
    public Transform Transform => transform;
    [SerializeField] private AttackSystem attackSystem;
    [field: SerializeField] public GameObject WeaponGameObject {  get; private set; }
    [Header("Events")]
    [SerializeField] private EnemyInRangeEvent unitEnterRange;
    [SerializeField] private EnemyInRangeEvent unitOutOfRange;
    [Header("Attack Info")]
    [field: SerializeField] public SO_AttackInfo AttackInfo { get; private set; }
    [field: SerializeField] public ParticleSystem AttackParticle {  get; private set; }


    protected override void Awake()
    {
        base.Awake();
        audioSource = GetComponent<AudioSource>();
        GraphAgent.SetVariableValue("SO Attack Info", AttackInfo);
        unitEnterRange.Register(UnitInRange);
        unitOutOfRange.Register(UnitOutOfRange);
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        unitEnterRange.Unregister(UnitInRange);
        unitOutOfRange.Unregister(UnitOutOfRange);
    }


    public void Attack(IAttackable attackable)
    {
        PlayAttackAudio();
        GraphAgent.SetVariableValue<GameObject>("TargetGameObject", attackable.TargetPosition.gameObject);
        GraphAgent.SetVariableValue("BuildingActions", BuildingActions.Attack);
    }

    public void Attack(Vector3 attackPosition){}

    private void PlayAttackAudio()
    {
        if (AttackInfo.AttackAudioClips.Count > 0)
        {
            AudioManager.SetAudioClips(AttackInfo.AttackAudioClips);
            AudioManager.PlayAudio();
        }
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

    private void UnitInRange(IAttackable enemyInRange)
    {
        List<GameObject> targets = SetNearbyEnemiesOnBlackboard();
        if(!IsConstructed || IsRepairing) return;
        if (GraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            && targetVariable.Value == null && targets.Count > 0)
        {
            GraphAgent.SetVariableValue("TargetGameObject", targets[0]);
            GraphAgent.SetVariableValue("BuildingActions", BuildingActions.Attack);
        }
    }

    public void ResetAttackSystem() 
    {
        UnitInRange(null);
    }

    public override void RepairBuilding()
    {
        GraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
        GraphAgent.SetVariableValue<List<GameObject>>("Enemies", null);
        base.RepairBuilding();
    }

    private void UnitOutOfRange(IAttackable enemyOutOfRange)
    {
        List<GameObject> targets = SetNearbyEnemiesOnBlackboard();
        if (!IsConstructed || IsRepairing) return;
        if (!GraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetVariable)
            || enemyOutOfRange.TargetPosition.gameObject != targetVariable.Value) return;

        if (targets.Count > 0)
        {
            GraphAgent.SetVariableValue("TargetGameObject", targets[0]);
            GraphAgent.SetVariableValue("BuildingActions", BuildingActions.Attack);
        }
        else
        {
            GraphAgent.SetVariableValue<GameObject>("TargetGameObject", null);
            GraphAgent.SetVariableValue("BuildingActions", BuildingActions.Stop);
        }
    }

    private List<GameObject> SetNearbyEnemiesOnBlackboard()
    {
        List<GameObject> nearbyEnemies = attackSystem.GetEnemiesInRange().ConvertAll(
                    damage => damage.TargetPosition.gameObject);
        nearbyEnemies.Sort(new ClosestGameObjectComparer(transform.position));

        GraphAgent.SetVariableValue("Enemies", nearbyEnemies);

        return nearbyEnemies;
    }

}
