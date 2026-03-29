using UnityEngine;

[CreateAssetMenu(fileName = "ActionExecuted", menuName = "Events/Action Executed")]
public class ActionExecuted : GameEvent<BaseAction>
{
   public BaseAction action { get; private set; }
}
