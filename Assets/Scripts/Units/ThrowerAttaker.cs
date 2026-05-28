using System.Collections;
using Unity.Behavior;
using UnityEngine;

public class ThrowerAttaker : BaseAttacker
{
    [SerializeField] private GameObject throwItem;
    [SerializeField] private ParticleSystem effectParticle;


    private Transform throwParent;
    private Vector3 startItemPosition;
    private Collider[] enemyColliders;

    protected override void Awake()
    {
        base.Awake();

        startItemPosition = throwItem.transform.localPosition;
        throwParent = throwItem.transform.parent;
    }

    protected override void Start()
    {
        base.Start();        
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if(throwItem != null)
        {
            Destroy(throwItem);
        }
        if(effectParticle != null)
        {
            Destroy(effectParticle);
        }
    }

    public void ThrowProjectile()
    {
        throwItem.transform.SetParent(null);
        Vector3 startPosition = throwItem.transform.position;
        Vector3 endPosition = throwItem.transform.position + throwItem.transform.forward * 3;
        IAttackable attackable = null;

        if(behaviorGraphAgent.GetVariable("TargetGameObject", out BlackboardVariable<GameObject> targetGameObjectVariable) 
            && targetGameObjectVariable != null && targetGameObjectVariable.Value != null)
        {
            endPosition = targetGameObjectVariable.Value.transform.position + Vector3.up;
            attackable = targetGameObjectVariable.Value.GetComponent<IAttackable>();
        }
        else if(behaviorGraphAgent.GetVariable("TargetPosition", out BlackboardVariable<Vector3> targetLocationVariable))
        {
            endPosition = targetLocationVariable.Value;
        }
        StartCoroutine(MoveThrowableItem(startPosition, endPosition, attackable));
    }

    private IEnumerator MoveThrowableItem(Vector3 startPosition, Vector3 endPosition, IAttackable attackable)
    {
        float time = 0f;
        const float speed = 2f;

        while (time < 1f)
        {
            throwItem.transform.position = Vector3.Lerp(startPosition, endPosition, time);
            time += Time.deltaTime * speed;
            yield return null;
        }

        effectParticle.transform.SetParent(null);
        effectParticle.transform.position = endPosition;
        effectParticle.Play();

        throwItem.transform.SetParent(throwParent);
        throwItem.transform.localPosition = startItemPosition;

        DamageEnemies(endPosition, attackable);
    }

    private void DamageEnemies(Vector3 endPosition, IAttackable attackable)
    {
        if(attackable != null && attackable.TargetPosition != null)
        {
            attackable.ApplyDamage(AttackInfo.AttackDamage);
        }

        if (AttackInfo.IsAreaEffect)
        {
            int hits = Physics.OverlapSphereNonAlloc(
                endPosition,
                AttackInfo.AreaOfEffectRadius,
                enemyColliders,
                AttackInfo.DamageableLayer
            );

            for (int i = 0; i < hits; i++)
            {
                if (enemyColliders[i].TryGetComponent(out IAttackable nearbyDamageable)
                    && attackable != nearbyDamageable)
                {
                    nearbyDamageable.ApplyDamage(
                        AttackInfo.CalculateAreaOfEffectDamage(endPosition, nearbyDamageable.TargetPosition.position)
                    );
                }
            }

            /*foreach (Collider enemyCollider in enemyColliders)
            {
                if(enemyCollider.TryGetComponent(out IAttackable attackable1) && attackable != attackable1)
                {
                    attackable1.ApplyDamage(AttackInfo.CalculateAreaOfEffectDamage(endPosition, attackable1.TargetPosition.position));
                }
            }*/
        }
    }
}
