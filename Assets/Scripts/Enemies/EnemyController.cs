using System.Collections;
using Unity.Behavior;
using UnityEngine;

public class EnemyController : BaseAttacker
{
    [Header("Death Components")]
    [SerializeField] private bool explodes;
    [SerializeField] private GameObject deathExplotion;
    [SerializeField] private GameObject target;
    [Header("Enemy Events")]
    [SerializeField] DeathEnemy deathEnemy;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        if(target != null)
        {
            Attack(target.transform.position);
        }
    }

    public void SetTarget(GameObject target)
    {
        this.target = target;
    }

    public void AttackTarget()
    {
        if(target != null)
        {
            Attack(target.transform.position);
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
    protected override void OnDestroy()
    {
        base.OnDestroy();        
    }

    public override void Die()
    {
        if (explodes && deathExplotion != null)
        {
            GameObject exp = Instantiate(deathExplotion, transform);
            exp.transform.parent = null;
            exp.GetComponent<EnemyExplotion>().SetInfo(AttackInfo);
        }
        deathEnemy.Raise(null);
        base.Die();
    }
}
