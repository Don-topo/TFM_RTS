using UnityEngine;

public struct ActionInfo
{
    public CommonActions Action { get; private set; }
    public RaycastHit Hit { get; private set; }

    public ActionInfo(CommonActions action, RaycastHit hit)
    {
        Action = action;
        Hit = hit;
    }
}
