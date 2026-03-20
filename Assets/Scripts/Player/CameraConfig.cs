using UnityEngine;

[System.Serializable]
public class CameraConfig
{
    // Zoom
    [field: SerializeField] public float ZoomSpeed { get; set; }
    [field: SerializeField] public float MaxZoom {  get; set; }
    [field: SerializeField] public float MinZoom { get; set; }

    // Movement
    [field: SerializeField] public float MoveSpeed {  get; set; }
    // Rotation
    [field: SerializeField] public float RotationSpeed { get; set; }

}
