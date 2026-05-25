using UnityEngine;

public class Vision
{
    private IHideable hideable;
    private bool isVisible;

    public Vision(IHideable hideable, bool isVisible)
    {
        this.hideable = hideable;
        this.isVisible = isVisible;
    }
}

[CreateAssetMenu(fileName = "VisibilityEvent", menuName = "Events/Visibility")]
public class VisibilityEvent : GameEvent<Vision>{}
