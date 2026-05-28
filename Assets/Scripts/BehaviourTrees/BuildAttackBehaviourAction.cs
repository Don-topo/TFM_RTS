using System;
using System.Collections.Generic;
using Unity.Behavior;
using Unity.Properties;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;
using Action = Unity.Behavior.Action;

[Serializable, GeneratePropertyBag]
[NodeDescription(name: "BuildAttack", story: "[Self] Attack [TargetGameObject]", category: "Action", id: "b02f8be3d55d1461e2d67f0e751f6d2b")]
public partial class BuildAttackBehaviourAction : Action
{
    [SerializeReference] public BlackboardVariable<GameObject> Self;
    [SerializeReference] public BlackboardVariable<GameObject> TargetGameObject;
    [SerializeReference] public BlackboardVariable<SO_AttackInfo> AttackInfo;
    [SerializeReference] public BlackboardVariable<List<GameObject>> Enemies;

    private AttackerBuilding attackerBuilding;
    private Transform selfTransform;
    private Transform targetTransform;
    private Collider[] targetColliders;
    private float lastAttack;
    private IAttackable targetAttackable;
    private AudioSource audioSource;
    private ParticleSystem particleSystem;
    private GameObject weapon;

    protected override Status OnStart()
    {
        if (Self.Value == null || TargetGameObject.Value == null || Enemies.Value == null) return Status.Failure;
            
        selfTransform = Self.Value.transform;
        attackerBuilding = selfTransform.GetComponent<AttackerBuilding>();
        particleSystem = Self.Value.GetComponent<AttackerBuilding>().AttackParticle;
        targetTransform = TargetGameObject.Value.transform;
        targetAttackable = TargetGameObject.Value.GetComponent<IAttackable>();
        audioSource = Self.Value.GetComponent<AudioSource>();
        weapon = Self.Value.GetComponent<AttackerBuilding>().WeaponGameObject;
        return Status.Running;
    }

    protected override Status OnUpdate()
    {
        if (TargetGameObject.Value == null || targetAttackable.CurrentHealth == 0) return Status.Success;

        if (!Enemies.Value.Contains(TargetGameObject.Value))
        {
            return Status.Running;
        }
        
        LookAtTarget();

        if (Time.time > lastAttack + AttackInfo.Value.AttackSpeed && !Self.Value.GetComponent<AttackerBuilding>().IsRepairing)
        {
            Attack();
        }

        return Status.Running;
    }

    protected override void OnEnd()
    {
    }

    private void Attack()
    {
        lastAttack = Time.time;        
        if (audioSource != null) audioSource.Play();
        if(attackerBuilding.AttackParticle != null)
        {
            attackerBuilding.AttackParticle.Play();
        }
        if (attackerBuilding.AttackInfo.IsAttachedAreaEffect)
        {
            Vector3 forward = weapon.transform.right;
            Vector3 boxSize = new Vector3(AttackInfo.Value.XArea, AttackInfo.Value.YArea, AttackInfo.Value.ZArea);
            Vector3 center =
                weapon.transform.position +
                weapon.transform.right * (boxSize.z * 0.5f);

            Collider[] hits = Physics.OverlapBox(
                center,
                boxSize * 0.5f,
                Quaternion.LookRotation(forward, weapon.transform.up),
                AttackInfo.Value.DamageableLayer
            );

            foreach (Collider collider in hits)
            {
                if (collider.CompareTag("Enemy"))
                {
                    collider.GetComponent<IAttackable>().ApplyDamage(attackerBuilding.AttackInfo.AttackDamage);
                }                    
            }
        }
        else
        {
            targetAttackable.ApplyDamage(AttackInfo.Value.AttackDamage);
        }        
    }    

    private void LookAtTarget()
    {
        Vector3 direction = targetTransform.position - weapon.transform.position;

        if (direction != Vector3.zero)
        {
            weapon.transform.rotation = Quaternion.LookRotation(direction) * Quaternion.Euler(0, -90, 0);
        }
    }
}

