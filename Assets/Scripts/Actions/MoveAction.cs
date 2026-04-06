using UnityEngine;

[CreateAssetMenu(fileName = "MoveAction", menuName = "Units/Actions/Move", order = 100)]
public class MoveAction : BaseAction
{
    [SerializeField] private float radiusMultiplier = 3.5f;
    private int unitsOnLayer = 0;
    private int maxUnitsOnLayer = 1;
    private float circleRadius = 0;
    private float radialOffset = 0;

    public override bool CanExecute(ActionInfo actionInfo)
    {
        return actionInfo.Action is BaseUnit;
    }

    public override void Execute(ActionInfo actionInfo)
    {
        BaseUnit unit = (BaseUnit)actionInfo.Action;
        if(actionInfo.Hit.collider != null && actionInfo.Hit.collider.TryGetComponent(out CommonActions action))
        {
            unit.Move(action.transform);
            return;
        }

        unit.Move(CalculateMovePosition(actionInfo));
    }

    // Calculate circle oriented formation
    public Vector3 CalculateMovePosition(ActionInfo actionInfo)
    {

        if (actionInfo.Action is not BaseUnit unit) return actionInfo.Hit.point;

        if (actionInfo.GroupPosition == 0)
        {
            unitsOnLayer = 0;
            maxUnitsOnLayer = 1;
            circleRadius = 0;
            radialOffset = 0;
        }

        Vector3 newMovePosition = new(
            actionInfo.Hit.point.x + circleRadius * Mathf.Cos(radialOffset * unitsOnLayer),
            actionInfo.Hit.point.y,
            actionInfo.Hit.point.z + circleRadius * Mathf.Sin(radialOffset * unitsOnLayer)
            );

        unitsOnLayer++;


        if (unitsOnLayer >= maxUnitsOnLayer)
        {
            unitsOnLayer = 0;
            circleRadius += unit.GetNavMeshAgentRadius * radiusMultiplier;
            maxUnitsOnLayer = Mathf.FloorToInt(2 * Mathf.PI * circleRadius / (unit.GetNavMeshAgentRadius * 2));
            radialOffset = 2 * Mathf.PI / maxUnitsOnLayer;
        }

        return newMovePosition;
    }

    public override bool Blocked(ActionInfo actionInfo) => false;
}
