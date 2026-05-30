using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class EnemyExplotion : MonoBehaviour
{
    private int damage;

    private void Awake()
    {
        Destroy(this, 1f);
    }

    public void SetDamage(int damage)
    {
        this.damage = damage;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") || other.CompareTag("CommandPost"))
        {
            other.GetComponent<IAttackable>().ApplyDamage(damage);
        }
    }
}
