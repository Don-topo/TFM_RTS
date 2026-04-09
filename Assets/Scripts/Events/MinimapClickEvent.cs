using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MinimapEventInfo
{
    public MouseButton MouseButton {  get; private set; }
    public RaycastHit RaycastHit { get; private set; }

    public MinimapEventInfo(MouseButton mouseButton, RaycastHit raycastHit)
    {
        MouseButton = mouseButton;
        RaycastHit = raycastHit;
    }
}

[CreateAssetMenu(fileName = "MinimapClickEvent", menuName = "Events/Minimap click", order = 106)]
public class MinimapClickEvent : GameEvent<MinimapEventInfo>
{
    MinimapEventInfo minimapEventInfo;
}
