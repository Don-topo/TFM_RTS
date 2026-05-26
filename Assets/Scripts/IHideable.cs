using UnityEngine;

public interface IHideable
{
    public Transform TargetPosition { get; }
    public bool IsVisible { get; }
    public void SetVisible(bool isVisible);
}
