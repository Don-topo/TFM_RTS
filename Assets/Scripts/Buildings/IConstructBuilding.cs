using UnityEngine;

public interface IConstructBuilding
{
    public bool IsUnderConstruction { get; }
    public GameObject ConstructBuilding(SO_Building building, Vector3 targetPosition);
    public void CancelConstruct();
}
