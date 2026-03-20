using UnityEngine;

[CreateAssetMenu(fileName = "ActionClicked", menuName = "Events/Action Clicked")]
public class ActionClicked : GameEvent<BaseAction>
{
    public BaseAction action;
}
