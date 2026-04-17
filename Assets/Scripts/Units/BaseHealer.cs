using UnityEngine;

public class BaseHealer : BaseUnit, IHealer
{

    [field: SerializeField] public SO_HealInfo HealInfo { get; private set; }

    protected override void Start()
    {

    }

    protected override void Update()
    {

    }

    public Transform Transform => transform;

    public void Heal(BaseUnit unit)
    {
        //behaviorGraphAgent.SetVariableValue()
    }

    public void Heal(Vector3 healPosition)
    {
        throw new System.NotImplementedException();
    }
    
}
