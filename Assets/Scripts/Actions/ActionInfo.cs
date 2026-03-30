using UnityEngine;

public struct ActionInfo
{
    public CommonActions Action { get; private set; }
    public RaycastHit Hit { get; private set; }
    public int GroupPosition { get; private set; }

    public ActionInfo(CommonActions action, RaycastHit hit, int groupPosition)
    {
        Action = action;
        Hit = hit;
        GroupPosition = groupPosition;
    }
}
