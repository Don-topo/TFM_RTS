using UnityEngine;

public interface IAttacker 
{
    public Transform Transform { get; }
    public void Attack(IAttackable attackable);
    public void Attack(Vector3 attackPosition);
}
