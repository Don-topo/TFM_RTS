using UnityEngine;

public interface IAttackable
{
    public int CurrentHealth { get; }
    public int MaxHealth { get; }

    public Transform TargetPosition { get; }

    public void ApplyDamage(int damageAmount);
    public void Die();
}
