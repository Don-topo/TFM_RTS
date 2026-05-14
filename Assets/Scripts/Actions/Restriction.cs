using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "Building Restriction", menuName = "Buildings/Restrictions", order = 7)]
public class Restriction : ScriptableObject
{
    [field: SerializeField] public float Radius { get; private set; } = 1f;
    [field: SerializeField] public LayerMask LayerMask { get; private set; }
    [field: SerializeField] public OverlapStyle HitDetectionStyle { get; private set; } = OverlapStyle.Sphere;
    [field: SerializeField] public bool MustBeFullyOnNaveMesh { get; private set; } = true;
    [field: SerializeField] public int NavMeshAgentTypeId { get; private set; }
    [field: SerializeField] public float NavMeshTolerance { get; private set; } = 0.1f;
    [field: SerializeField] public Vector3 Extents { get; private set; } = Vector3.one;
    [field: SerializeField] public ResourcesType ResourcePlaceToCheck { get; private set; }
    [field: SerializeField] public bool CheckForResource {  get; private set; }
    [field: SerializeField] public LayerMask ResourceLayer { get; private set; }

    private Collider[] hitColliders = new Collider[1];

    public bool CanPlace(Vector3 position)
    {
        int hits = HitDetectionStyle switch
        {
            OverlapStyle.Sphere => Physics.OverlapSphereNonAlloc(position, Radius, hitColliders, LayerMask),
            OverlapStyle.Box => Physics.OverlapBoxNonAlloc(position, Extents, hitColliders, Quaternion.identity, LayerMask),
            OverlapStyle.Vision => throw new System.NotImplementedException(),
            _ => throw new System.NotImplementedException(),
            //OverlapStyle.Vision => FogVisibilityManager.Instance.IsVisible(position) ? 0 : 1
        };

        if (MustBeFullyOnNaveMesh)
        {
            NavMeshQueryFilter queryFilter = new()
            {
                areaMask = NavMesh.AllAreas,
                agentTypeID = NavMeshAgentTypeId
            };
            bool isOnNavMesh = IsFullyOnNavMesh(position, queryFilter);

            return isOnNavMesh && hits == 0 && IsOnSupportedResource(position);
        }

        return hits == 0;
    }

    private bool IsFullyOnNavMesh(Vector3 position, NavMeshQueryFilter queryFilter)
    {
        bool isOnNavMesh = NavMesh.SamplePosition(
                        position + new Vector3(Extents.x, 0, Extents.z),
                        out NavMeshHit _, NavMeshTolerance, queryFilter);
        isOnNavMesh = isOnNavMesh && NavMesh.SamplePosition(
                        position + new Vector3(Extents.x, 0, -Extents.z),
                        out NavMeshHit _, NavMeshTolerance, queryFilter);
        isOnNavMesh = isOnNavMesh && NavMesh.SamplePosition(
                        position + new Vector3(-Extents.x, 0, -Extents.z),
                        out NavMeshHit _, NavMeshTolerance, queryFilter);
        isOnNavMesh = isOnNavMesh && NavMesh.SamplePosition(
                        position + new Vector3(-Extents.x, 0, Extents.z),
                        out NavMeshHit _, NavMeshTolerance, queryFilter);
        return isOnNavMesh;
    }

    private bool IsOnSupportedResource(Vector3 position)
    {
        if (ResourceLayer == 0 || !CheckForResource) return true;

        Collider[] res = Physics.OverlapSphere(position, Radius, ResourceLayer);
        if (res.Length > 0 && res[0].GetComponent<ResourceArea>().ResourceInArea.ResourceTypes == ResourcePlaceToCheck)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public enum OverlapStyle
    {
        Sphere,
        Box,
        Vision
    }
}