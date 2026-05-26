using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.GridLayoutGroup;

[RequireComponent(typeof(Camera))]
public class FogOfWar : MonoBehaviour
{
    public static FogOfWar Instance { get; private set; }
    [Header("Events")]
    [SerializeField] private UnitRecruitedEvent unitRecruitedEvent;
    [SerializeField] private UnitDeathEvent UnitDeathEvent;
    [SerializeField] private BuildingCreatedEvent buildingCreatedEvent;
    [SerializeField] private BuildingDestroyedEvent buildingDestroyedEvent;
    private Camera fogOfWarCamera;
    private Texture2D visionTexture;
    private Rect textureRect;
    private HashSet<IHideable> hideables = new(1000);

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogError($"Multiple FogVisibilityManagers cannot exist! Disabling {name}");
            enabled = false;
            return;
        }
        Instance = this;

        fogOfWarCamera = GetComponent<Camera>();
        visionTexture = new Texture2D(fogOfWarCamera.targetTexture.width, fogOfWarCamera.targetTexture.height);
        textureRect = new Rect(0, 0, visionTexture.width, visionTexture.height);

        //Bus<UnitSpawnEvent>.RegisterForAll(HandleUnitSpawn);
        unitRecruitedEvent.Register(HandleUnitSpawn);
        //Bus<UnitDeathEvent>.RegisterForAll(HandleUnitDeath);
        UnitDeathEvent.Register(HandleUnitDeath);

        //Bus<BuildingSpawnEvent>.RegisterForAll(HandleBuildingSpawn);
        //buildingCreatedEvent.Register(HandleBuildingSpawn);
        //Bus<BuildingDeathEvent>.RegisterForAll(HandleBuildingDeath);
        buildingDestroyedEvent.Register(HandleBuildingDeath);
        //TODO
        //Bus<PlaceholderSpawnEvent>.RegisterForAll(HandlePlaceholderSpawn);
        //Bus<PlaceholderDestroyEvent>.RegisterForAll(HandlePlaceholderDestroy);

        //Bus<SupplySpawnEvent>.OnEvent[Owner.Unowned] += HandleSupplySpawn;
        //Bus<SupplyDepletedEvent>.OnEvent[Owner.Unowned] += HandleSupplyDepleted;
    }

    private void OnDestroy()
    {
        //Bus<UnitSpawnEvent>.UnregisterForAll(HandleUnitSpawn);
        unitRecruitedEvent.Unregister(HandleUnitSpawn);
        //Bus<UnitDeathEvent>.UnregisterForAll(HandleUnitDeath);
        UnitDeathEvent.Unregister(HandleUnitDeath);

        //Bus<BuildingSpawnEvent>.UnregisterForAll(HandleBuildingSpawn);
        //buildingCreatedEvent.Unregister(HandleBuildingSpawn);
        //Bus<BuildingDeathEvent>.UnregisterForAll(HandleBuildingDeath);
        buildingDestroyedEvent.Unregister(HandleBuildingDeath);

        // TODO
        //Bus<PlaceholderSpawnEvent>.UnregisterForAll(HandlePlaceholderSpawn);
        //Bus<PlaceholderDestroyEvent>.UnregisterForAll(HandlePlaceholderDestroy);

        //Bus<SupplySpawnEvent>.OnEvent[Owner.Unowned] -= HandleSupplySpawn;
        //Bus<SupplyDepletedEvent>.OnEvent[Owner.Unowned] -= HandleSupplyDepleted;
    }

    private void LateUpdate()
    {
        ReadPixelsToVisionTexture();

        foreach (IHideable hideable in hideables)
        {
            SetUnitVisibilityStatus(hideable);
        }
    }

    public bool IsVisible(Vector3 position)
    {
        Vector3 screenPoint = fogOfWarCamera.WorldToScreenPoint(position);
        Color visibilityColor = visionTexture.GetPixel((int)screenPoint.x, (int)screenPoint.y);
        return visibilityColor.r > 0.9f;
    }

    private void ReadPixelsToVisionTexture()
    {
        RenderTexture previousRenderTexture = RenderTexture.active;

        RenderTexture.active = fogOfWarCamera.targetTexture;
        visionTexture.ReadPixels(textureRect, 0, 0);
        RenderTexture.active = previousRenderTexture;
    }

    private void SetUnitVisibilityStatus(IHideable hideable)
    {

        hideable.SetVisible(IsVisible(hideable.TargetPosition.position));
    }

    private void HandleUnitSpawn(BaseUnit baseUnit)
    {
        hideables.Add(baseUnit);        
    }

    private void HandleUnitDeath(CommonActions unit)
    {
        hideables.Remove(unit);
    }

    private void HandleBuildingSpawn(BaseBuilding baseBuilding)
    {
        hideables.Add(baseBuilding);
    }

    private void HandleBuildingDeath(BaseBuilding baseBuilding)
    {
        hideables.Remove(baseBuilding);
    }

    /*private void HandleSupplySpawn(SupplySpawnEvent evt)
    {
        hideables.Add(evt.Supply);
    }

    private void HandleSupplyDepleted(SupplyDepletedEvent evt)
    {
        hideables.Remove(evt.Supply);
    }*/

    /*private void HandlePlaceholderSpawn(PlaceholderSpawnEvent evt)
    {
        hideables.Add(evt.Placeholder);
    }

    private void HandlePlaceholderDestroy(PlaceholderDestroyEvent evt)
    {
        hideables.Remove(evt.Placeholder);
    }*/
}
