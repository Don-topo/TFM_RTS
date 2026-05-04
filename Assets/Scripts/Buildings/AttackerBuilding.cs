using UnityEngine;

public class AttackerBuilding : BaseBuilding, IAttacker
{
    public Transform Transform => throw new System.NotImplementedException();

    public void Attack(IAttackable attackable)
    {
        throw new System.NotImplementedException();
    }

    public void Attack(Vector3 attackPosition)
    {
        // Nothing to do here?
    }

}
