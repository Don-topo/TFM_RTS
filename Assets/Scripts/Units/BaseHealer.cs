using UnityEngine;

public class BaseHealer : BaseUnit, IHealer
{

    protected override void Start()
    {

    }

    protected override void Update()
    {

    }

    public Transform Transform => throw new System.NotImplementedException();

    public void Heal(BaseUnit unit)
    {
        throw new System.NotImplementedException();
    }

    public void Heal(Vector3 attackPosition)
    {
        throw new System.NotImplementedException();
    }
    
}
