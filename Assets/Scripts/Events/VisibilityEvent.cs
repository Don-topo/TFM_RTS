using UnityEngine;

public class Vision
{
    public IHideable Hideable;
    public bool IsVisible;

    public Vision(IHideable hideable, bool isVisible)
    {
        this.Hideable = hideable;
        this.IsVisible = isVisible;
    }
}

[CreateAssetMenu(fileName = "VisibilityEvent", menuName = "Events/Visibility")]
public class VisibilityEvent : GameEvent<Vision>{}
