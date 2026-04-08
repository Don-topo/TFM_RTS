using UnityEngine;

public class UIMinimapManager : MonoBehaviour
{
    [SerializeField] private Camera camera;

    [Header("Events")]
    [SerializeField] private ActionClicked actionClicked;
    [SerializeField] private ActionExecuted actionExecuted;

    private BaseAction selectedAction;

    private void Awake()
    {
        // Subscribe to events
        actionClicked.Register(MapClick);
        actionExecuted.Register(ActionMapExecuted);
    }

    private void OnDestroy()
    {
        // Unsubscribe events
        actionClicked.Unregister(MapClick);
        actionExecuted.Unregister(ActionMapExecuted);
    }

    private void MapClick(BaseAction baseAction)
    {

    }

    private void ActionMapExecuted(BaseAction baseAction)
    {

    }
}
