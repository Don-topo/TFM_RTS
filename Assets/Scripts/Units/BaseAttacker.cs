using UnityEngine;

public class BaseAttacker : BaseUnit, IAttacker
{
    public Transform Transform => throw new System.NotImplementedException();

    public void Attack(IAttackable attackable)
    {
        throw new System.NotImplementedException();
    }

    public void Attack(Vector3 attackPosition)
    {
        throw new System.NotImplementedException();
    }
}
