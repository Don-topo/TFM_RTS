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

        unitRecruitedEvent.Register(HandleUnitSpawn);
        UnitDeathEvent.Register(HandleUnitDeath);
    }

    private void OnDestroy()
    {
        unitRecruitedEvent.Unregister(HandleUnitSpawn);
        UnitDeathEvent.Unregister(HandleUnitDeath);
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
}
