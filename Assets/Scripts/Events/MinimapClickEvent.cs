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

public class MinimapClickEvent : GameEvent<MinimapEventInfo>
{
    MinimapEventInfo minimapEventInfo;
}
