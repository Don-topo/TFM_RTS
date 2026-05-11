using Unity.Behavior;
using UnityEngine;

public class BuilderBuilding : BaseBuilding, IConstructBuilding
{
    public bool IsUnderConstruction => throw new System.NotImplementedException();
    [SerializeField] private BaseAction CancelBuildingCommand;

    public void CancelConstruct()
    {
        throw new System.NotImplementedException();
    }

    public GameObject ConstructBuilding(SO_Building building, Vector3 targetPosition)
    {
        GameObject instance = Instantiate(building.UnitPrefab, targetPosition, Quaternion.identity);
        throw new System.NotImplementedException();
    }
}
