using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.LowLevel;
using static UnityEngine.UI.GridLayoutGroup;

public class UIMinimapManager : MonoBehaviour, IPointerExitHandler, IPointerEnterHandler, IPointerMoveHandler, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private Camera minimapCamera;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;

    [Header("Events")]
    [SerializeField] private ActionClicked actionClicked;
    [SerializeField] private ActionExecuted actionExecuted;
    [SerializeField] private MinimapClickEvent minimapClicked;

    private BaseAction selectedAction;
    private bool mouseOnMinimap;
    private RectTransform rectTransform;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
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
        selectedAction = baseAction;
    }

    private void ActionMapExecuted(BaseAction baseAction)
    {
        selectedAction = null;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        mouseOnMinimap = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //mouseOnMinimap = true;
    }

    public void OnPointerMove(PointerEventData eventData)
    {
        MoveVirtualCameraTarget(eventData.position);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Left && selectedAction == null)
        {
            mouseOnMinimap = true;
            MoveVirtualCameraTarget(eventData.position);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Check wich mouse button was released
        if (eventData.button == PointerEventData.InputButton.Left)
        {
            mouseOnMinimap = false;
            RaiseClickEvent(eventData.position, MouseButton.Left);
        }
        else if (eventData.button == PointerEventData.InputButton.Right)
        {
            RaiseClickEvent(eventData.position, MouseButton.Right);
        }
    }

    private void MoveVirtualCameraTarget(Vector3 mousePosition)
    {
        if (!mouseOnMinimap) return;

        if (RaycastFromMousePosition(mousePosition, out RaycastHit hit))
        {
            mainCamera.transform.position = hit.point;
        }
    }


    private void RaiseClickEvent(Vector2 mousePosition, MouseButton button)
    {
        RaycastFromMousePosition(mousePosition, out RaycastHit hit);
        {
            minimapClicked.Raise(new MinimapEventInfo(button, hit));            
        }
    }

    private bool RaycastFromMousePosition(Vector2 mousePosition, out RaycastHit hit)
    {
        float widthMultiplier = minimapCamera.scaledPixelWidth / rectTransform.rect.width;
        float heightMultiplier = minimapCamera.scaledPixelHeight / rectTransform.rect.height;

        Vector2 convertedMousePosition = new(
            mousePosition.x * widthMultiplier,
            mousePosition.y * heightMultiplier
        );

        Ray cameraRay = minimapCamera.ScreenPointToRay(convertedMousePosition);
        return Physics.Raycast(cameraRay, out hit, float.MaxValue, layerMask);

    }
}
