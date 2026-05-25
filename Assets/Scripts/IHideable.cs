using UnityEngine;

public interface IHideable
{
    public Transform Transform { get; }
    public bool IsVisible { get; }
    public void SetVisible(bool isVisible);
}
